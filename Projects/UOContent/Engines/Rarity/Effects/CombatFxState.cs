using System;
using System.Collections.Generic;
using Server.Engines.BuffIcons;

namespace Server.Engines.Rarity;

// Transient, NON-serialized combat state for rarity weapon effects. The server is
// single-threaded, so plain dictionaries are correct here — never a lock or
// ConcurrentDictionary (CLAUDE.md rule 3). All expiry is checked lazily against
// Core.TickCount on read. Entries for dead/deleted mobiles are evicted by the
// death/delete hook in RarityEffects and lazily on access.
// Exception: marks carry a short repeating "pulse" timer (_markPulse) purely for the
// visual glow + tooltip refresh — the mark's game state is still lazy-expiry.
public static class CombatFxState
{
    // A fight "ends" after this long with no tracked action; the next hit is a first-hit.
    private const long FightResetMs = 30_000;

    // Stun immunity window after any item stun (framework §9.2).
    private const long StunImmunityMs = 10_000;

    private struct AttackerState
    {
        public int HitCount;
        public long LastActionTick;
    }

    // P28: per-attacker consecutive-hit ramp (which target, how many hits in a row against it).
    private struct RampState
    {
        public Mobile Target;
        public int Stacks;
        public long LastActionTick;
    }

    private readonly struct MarkInfo
    {
        public readonly Mobile Marker;
        public readonly long ExpiryTick;
        public readonly int BonusPct;
        public readonly bool AllSources;

        public MarkInfo(Mobile marker, long expiryTick, int bonusPct, bool allSources)
        {
            Marker = marker;
            ExpiryTick = expiryTick;
            BonusPct = bonusPct;
            AllSources = allSources;
        }
    }

    private static readonly Dictionary<Mobile, AttackerState> _attackers = new();
    private static readonly Dictionary<Mobile, AttackerState> _defenders = new();
    private static readonly Dictionary<Mobile, MarkInfo> _marks = new();

    // Visual pulse timer per affected target — flashes a glow and refreshes the tooltip while any
    // target-side indicator (mark / heal-block) is active. One timer per target regardless of how
    // many indicators overlap; it self-terminates when none remain.
    private static readonly Dictionary<Mobile, TimerExecutionToken> _indicatorPulse = new();
    private static readonly TimeSpan IndicatorPulseInterval = TimeSpan.FromSeconds(1.25);
    private const int MarkHue = 0x25;      // orange — matches the "Marked" floating text
    private const int HealBlockHue = 0x21; // red — matches the offensive/debuff floating text

    private static readonly Dictionary<Mobile, long> _stunImmuneUntil = new();
    private static readonly Dictionary<Mobile, long> _healBlockUntil = new();
    private static readonly Dictionary<Mobile, int> _hitStacks = new();
    private static readonly HashSet<Mobile> _nextHitCrit = new();
    private static readonly Dictionary<Mobile, RampState> _ramps = new();

    // Maenad's struck-frenzy buff (+dmg%, Epic +swing% for 5s). Keyed by mobile since only one
    // frenzy window is ever active at a time (a fresh proc simply refreshes it).
    private static readonly Dictionary<Mobile, (int DmgPct, int SwingPct, long ExpiryTick)> _frenzy = new();

    // Penelope's web-snare: a dodge slows the attacker's swings by Pct% for a short window. Keyed by
    // mobile; a re-dodge refreshes it. Read by RarityEffects.AdjustSwingDelay.
    private static readonly Dictionary<Mobile, (int Pct, long ExpiryTick)> _snare = new();

    // Registers a landed hit for the attacker and returns the running hit count.
    // firstHitOfFight is true when combat had lapsed (30s idle) before this hit.
    public static int RegisterHit(Mobile attacker, out bool firstHitOfFight)
    {
        var now = Core.TickCount;

        if (!_attackers.TryGetValue(attacker, out var state) || now - state.LastActionTick > FightResetMs)
        {
            state = new AttackerState { HitCount = 0 };
            firstHitOfFight = true;
        }
        else
        {
            firstHitOfFight = false;
        }

        state.HitCount++;
        state.LastActionTick = now;
        _attackers[attacker] = state;

        return state.HitCount;
    }

    // A kill ends the killer's fight: clear their per-fight hit tracking (both offense and defense)
    // so the NEXT foe engaged counts as a fresh fight — "first hit of the fight" clauses re-arm and
    // the hit cadence restarts. Without this, killing one enemy and turning to another inside the
    // 30s window carries the dead fight's counters over, so first-hit never fires on the new enemy.
    // ponytail: resets on ANY kill, so in a multi-foe brawl a kill also re-arms first-hit against a
    // foe you were already fighting. Fine on PvE; move to per-target first-hit tracking if abused.
    public static void ResetFight(Mobile m)
    {
        if (m != null)
        {
            _attackers.Remove(m);
            _defenders.Remove(m);
        }
    }

    // Undo this swing's RegisterHit when it landed but dealt no damage (fully parried/blocked/
    // absorbed): a no-damage swing must not consume "first hit of the fight" nor advance the Nth-hit
    // cadence — only a hit that actually connects counts. `hitCountThisSwing` is the value RegisterHit
    // returned for the swing being undone; the rollback no-ops if anything advanced the counter since
    // (e.g. a nested extra swing landed), so it can never rewind a hit that did connect.
    public static void RollbackHit(Mobile attacker, int hitCountThisSwing)
    {
        if (attacker == null || hitCountThisSwing <= 0 ||
            !_attackers.TryGetValue(attacker, out var state) || state.HitCount != hitCountThisSwing)
        {
            return;
        }

        if (state.HitCount <= 1)
        {
            _attackers.Remove(attacker); // undo the first hit → the next connecting hit is first again
        }
        else
        {
            state.HitCount--;
            _attackers[attacker] = state;
        }
    }

    // Non-mutating fight-freshness check (no hit registration) — used by clauses that key off
    // "first X of any fight" for events outside the melee hit pipeline (e.g. a paralyze attempt).
    public static bool IsFightFreshForDefender(Mobile defender) =>
        defender == null || !_defenders.TryGetValue(defender, out var state) ||
        Core.TickCount - state.LastActionTick > FightResetMs;

    // Registers a hit taken by the defender; firstHitTaken is true when combat had lapsed
    // before this hit. Used by defensive legendary clauses (weapon Pallas, armor/shield P3a).
    // Idempotent within the same tick: the weapon-side and armor-side absorb steps both read
    // this for the same incoming hit, and the second caller must see the first caller's result
    // rather than re-rolling "first hit" into false or double-incrementing the hit counter.
    public static bool RegisterHitTaken(Mobile defender, out bool firstHitTaken) =>
        RegisterHitTaken(defender, out firstHitTaken, out _);

    public static bool RegisterHitTaken(Mobile defender, out bool firstHitTaken, out int hitCount)
    {
        var now = Core.TickCount;

        if (_defenders.TryGetValue(defender, out var already) && already.LastActionTick == now)
        {
            firstHitTaken = already.HitCount == 1;
            hitCount = already.HitCount;
            return true;
        }

        if (!_defenders.TryGetValue(defender, out var state) || now - state.LastActionTick > FightResetMs)
        {
            state = new AttackerState { HitCount = 0 };
            firstHitTaken = true;
        }
        else
        {
            firstHitTaken = false;
        }

        state.HitCount++;
        state.LastActionTick = now;
        _defenders[defender] = state;
        hitCount = state.HitCount;

        return true;
    }

    public static void SetMark(Mobile target, Mobile marker, int bonusPct, bool allSources, TimeSpan duration)
    {
        if (target == null || marker == null)
        {
            return;
        }

        // Strongest mark wins: a rival's weaker mark must not overwrite an active stronger one
        // (e.g. a +10% plain mark stomping a +25% all-sources mark). The owner may always
        // refresh their own mark; equal-bonus marks refresh too (last-writer keeps it simple).
        if (TryGetActiveMark(target, out var existing) && existing.Marker != marker && existing.BonusPct > bonusPct)
        {
            return;
        }

        _marks[target] = new MarkInfo(marker, Core.TickCount + (long)duration.TotalMilliseconds, bonusPct, allSources);
        BuffHelper.AddCustomBuff(target, BuffIcon.EnemyOfOneDebuff, $"Marked: +{bonusPct}% damage taken", duration);
        target.InvalidateProperties(); // show the "Marked" tooltip line
        StartIndicatorPulse(target);
    }

    // Whether a mark is currently active on the target, and its bonus % (for the tooltip line).
    public static bool TryGetMark(Mobile target, out int bonusPct)
    {
        if (TryGetActiveMark(target, out var mark))
        {
            bonusPct = mark.BonusPct;
            return true;
        }

        bonusPct = 0;
        return false;
    }

    public static bool IsMarked(Mobile target) => TryGetActiveMark(target, out _);

    // ---- Target-side visual indicators (glow pulse + tooltip lines) ---------------------------
    // Adds a tooltip line for every target-side status active on the mobile. Called from the
    // creature/player property builders so an attacker can read what they've applied to a target.
    public static void AddIndicatorProperties(Mobile target, IPropertyList list)
    {
        // 1114057 = "~1_val~" passthrough cliloc. The whole label must be a SINGLE hole (rule 14:
        // bare text in a PropertyList handler is treated as a delimiter, not literal text).
        if (TryGetMark(target, out var bonus))
        {
            var label = $"Marked +{bonus}%";
            list.Add(1114057, $"{label}");
        }

        if (IsHealBlocked(target))
        {
            list.Add(1114057, $"{"Heal Block"}");
        }

        if (target.Poisoned)
        {
            list.Add(1114057, $"{"Poisoned"}");
        }
    }

    // Hue of the highest-priority active indicator, or -1 if none are active.
    private static int ActiveIndicatorHue(Mobile target)
    {
        if (IsMarked(target))
        {
            return MarkHue;
        }

        if (IsHealBlocked(target))
        {
            return HealBlockHue;
        }

        return -1;
    }

    private static void StartIndicatorPulse(Mobile target)
    {
        if (target == null || _indicatorPulse.ContainsKey(target))
        {
            return; // one shared pulse per target — a second indicator rides the existing timer
        }

        Timer.StartTimer(IndicatorPulseInterval, IndicatorPulseInterval, () => PulseIndicators(target), out var token);
        _indicatorPulse[target] = token;
    }

    private static void StopIndicatorPulse(Mobile target)
    {
        if (target != null && _indicatorPulse.Remove(target, out var token))
        {
            token.Cancel();
        }
    }

    private static void PulseIndicators(Mobile target)
    {
        var hue = target is { Deleted: false } ? ActiveIndicatorHue(target) : -1;

        if (hue >= 0)
        {
            target.FixedEffect(0x374A, 10, 16, hue, 0);
            return;
        }

        // All indicators gone (cleared or lazily expired) — stop pulsing and drop the tooltip lines.
        StopIndicatorPulse(target);
        target?.InvalidateProperties();
    }

    // Returns the extra damage % a marked target takes from this attacker, or 0.
    // AllSources marks apply their bonus to every attacker; plain marks only to the marker.
    public static int GetMarkBonusFrom(Mobile target, Mobile attacker)
    {
        if (!TryGetActiveMark(target, out var mark))
        {
            return 0;
        }

        return mark.AllSources || mark.Marker == attacker ? mark.BonusPct : 0;
    }

    public static bool IsMarkedBy(Mobile target, Mobile marker) =>
        TryGetActiveMark(target, out var mark) && mark.Marker == marker;

    public static bool TryGetMarker(Mobile target, out Mobile marker)
    {
        if (TryGetActiveMark(target, out var mark))
        {
            marker = mark.Marker;
            return true;
        }

        marker = null;
        return false;
    }

    public static void ClearMark(Mobile target)
    {
        if (_marks.Remove(target))
        {
            BuffHelper.RemoveBuff(target, BuffIcon.EnemyOfOneDebuff);
            target.InvalidateProperties(); // pulse self-terminates if no other indicator remains
        }
    }

    private static bool TryGetActiveMark(Mobile target, out MarkInfo mark)
    {
        if (target != null && _marks.TryGetValue(target, out mark))
        {
            if (Core.TickCount < mark.ExpiryTick && mark.Marker is { Deleted: false })
            {
                return true;
            }

            _marks.Remove(target);
        }

        mark = default;
        return false;
    }

    // Applies a brief stun unless the target is inside its post-stun immunity window.
    // Duration is clamped to <= 2s (framework §9.2); a fired stun opens a 10s immunity.
    public static bool TryStun(Mobile target, TimeSpan duration)
    {
        if (target == null)
        {
            return false;
        }

        var now = Core.TickCount;

        if (_stunImmuneUntil.TryGetValue(target, out var until) && now < until)
        {
            return false;
        }

        var ms = Math.Min(duration.TotalMilliseconds, 2000);
        target.Paralyze(TimeSpan.FromMilliseconds(ms));
        _stunImmuneUntil[target] = now + StunImmunityMs;
        return true;
    }

    // Melanippos: extends an existing (or grants a fresh) stun-immunity window on kill.
    public static void ExtendStunImmunity(Mobile target, TimeSpan extra)
    {
        if (target == null)
        {
            return;
        }

        var now = Core.TickCount;
        var extraMs = (long)extra.TotalMilliseconds;

        _stunImmuneUntil[target] = (_stunImmuneUntil.TryGetValue(target, out var until) ? Math.Max(until, now) : now) + extraMs;
    }

    // PvP debuff budget: once a heal-block or snare lands on a PLAYER, another application is
    // locked out until the window plus this margin has passed — a coordinated group (or one
    // spammer) can no longer chain-refresh either debuff into a permanent state. Creatures are
    // exempt (PvE re-tag pacing is already limited by expiry checks at the apply sites).
    private const long PlayerDebuffLockoutMs = 6_000;

    private static readonly Dictionary<Mobile, long> _healBlockLockoutUntil = new();
    private static readonly Dictionary<Mobile, long> _snareLockoutUntil = new();

    // Heal-block debuff (framework §9.3, capped at 3s per application). Extend-only: a fresh,
    // shorter application (e.g. the bone capstone's 2s) never trims a longer window already
    // running — repeats extend the debuff, they cannot shorten it. Players additionally get one
    // application per lockout window (see PlayerDebuffLockoutMs).
    public static void SetHealBlock(Mobile target, TimeSpan duration)
    {
        if (target != null)
        {
            var now = Core.TickCount;

            if (target.Player && _healBlockLockoutUntil.TryGetValue(target, out var lockout) && now < lockout)
            {
                return;
            }

            var ms = (long)Math.Min(duration.TotalMilliseconds, 3000);
            var until = now + ms;

            if (_healBlockUntil.TryGetValue(target, out var existing) && existing > until)
            {
                return; // longer window already active — the buff icon for it is live too
            }

            _healBlockUntil[target] = until;

            if (target.Player)
            {
                _healBlockLockoutUntil[target] = until + PlayerDebuffLockoutMs;
            }

            BuffHelper.AddCustomBuff(target, BuffIcon.MortalStrike, "Heal Block: healing is suppressed", TimeSpan.FromMilliseconds(ms));
            target.InvalidateProperties(); // show the "Heal Block" tooltip line
            StartIndicatorPulse(target);
        }
    }

    public static bool IsHealBlocked(Mobile target) =>
        target != null && _healBlockUntil.TryGetValue(target, out var until) && Core.TickCount < until;

    // Test seam: remaining heal-block window in ms (0 when inactive) — asserts extend-only stacking.
    internal static long GetHealBlockRemaining(Mobile target) =>
        target != null && _healBlockUntil.TryGetValue(target, out var until)
            ? Math.Max(0, until - Core.TickCount)
            : 0;

    // Stacking hit-chance accumulator (Kyknos): each follow-up swing adds `step`, capped at `cap`.
    public static void AddHitStack(Mobile attacker, int step, int cap)
    {
        if (attacker == null)
        {
            return;
        }

        _hitStacks.TryGetValue(attacker, out var cur);
        _hitStacks[attacker] = Math.Min(cur + step, cap);
    }

    public static void ResetHitStack(Mobile attacker)
    {
        if (attacker != null)
        {
            _hitStacks.Remove(attacker);
        }
    }

    public static int GetHitStack(Mobile attacker) =>
        attacker != null && _hitStacks.TryGetValue(attacker, out var v) ? v : 0;

    // P28 ramp: registers a hit against `target`, returning the running consecutive-hit count.
    // Stacks reset to 1 on a target swap or after the fight lapses (30s idle), grow by one per
    // same-target hit, and clamp at maxStacks. reachedMaxThisHit is true only on the hit that
    // first climbs to the cap (so a "burst at max stacks" fires once per ramp-up, not every hit).
    public static int RegisterRampHit(Mobile attacker, Mobile target, int maxStacks, out bool reachedMaxThisHit)
    {
        reachedMaxThisHit = false;

        if (attacker == null || target == null)
        {
            return 0;
        }

        var now = Core.TickCount;
        var max = maxStacks > 0 ? maxStacks : 1;

        if (!_ramps.TryGetValue(attacker, out var state) || state.Target != target ||
            now - state.LastActionTick > FightResetMs)
        {
            state = new RampState { Target = target, Stacks = 1 };
        }
        else if (state.Stacks < max)
        {
            state.Stacks++;

            if (state.Stacks == max)
            {
                reachedMaxThisHit = true;
            }
        }

        state.LastActionTick = now;
        _ramps[attacker] = state;

        return state.Stacks;
    }

    public static int GetRampStacks(Mobile attacker) =>
        attacker != null && _ramps.TryGetValue(attacker, out var s) ? s.Stacks : 0;

    // Next-hit-guaranteed-crit flag (Kydon): a successful block arms the blocker's next hit.
    public static void SetNextHitCrit(Mobile attacker)
    {
        if (attacker != null && _nextHitCrit.Add(attacker))
        {
            // Indefinite icon — removed when the primed hit lands (ConsumeNextHitCrit) or the mobile
            // is evicted. Icon choice reuses an anachronistic strike icon (buff bar is intentionally
            // enabled on T2A); in-client rendering flagged for verification in the text-pass report.
            BuffHelper.AddCustomBuff(attacker, BuffIcon.LightningStrike, "Crit Ready: your next hit is a guaranteed crit");
        }
    }

    public static bool ConsumeNextHitCrit(Mobile attacker)
    {
        if (attacker == null || !_nextHitCrit.Remove(attacker))
        {
            return false;
        }

        BuffHelper.RemoveBuff(attacker, BuffIcon.LightningStrike);
        return true;
    }

    // Maenad: arms (or refreshes) the struck-frenzy buff.
    public static void ArmFrenzy(Mobile m, int dmgPct, int swingPct, TimeSpan duration)
    {
        if (m != null)
        {
            _frenzy[m] = (dmgPct, swingPct, Core.TickCount + (long)duration.TotalMilliseconds);
            var frenzyText = swingPct > 0 ? $"Frenzy: +{dmgPct}% damage, +{swingPct}% swing" : $"Frenzy: +{dmgPct}% damage";
            BuffHelper.AddCustomBuff(m, BuffIcon.Rage, frenzyText, duration);
        }
    }

    public static int GetFrenzyDamagePct(Mobile m) =>
        m != null && _frenzy.TryGetValue(m, out var f) && Core.TickCount < f.ExpiryTick ? f.DmgPct : 0;

    public static int GetFrenzySwingPct(Mobile m) =>
        m != null && _frenzy.TryGetValue(m, out var f) && Core.TickCount < f.ExpiryTick ? f.SwingPct : 0;

    // Ward-Surge (chainmail set capstone): taking a crit opens a window where the wearer's DR
    // rises to the suit-wide cap. Expiry-on-read like Frenzy; consumed in AbsorbForDefenderArmor.
    private static readonly Dictionary<Mobile, long> _wardSurgeUntil = new();

    public static void ArmWardSurge(Mobile m, TimeSpan duration)
    {
        if (m != null)
        {
            _wardSurgeUntil[m] = Core.TickCount + (long)duration.TotalMilliseconds;
            BuffHelper.AddCustomBuff(m, BuffIcon.Protection, "Ward-Surge: max damage reduction", duration);
        }
    }

    public static bool IsWardSurgeActive(Mobile m) =>
        m != null && _wardSurgeUntil.TryGetValue(m, out var until) && Core.TickCount < until;

    // Hypnos (StealthBreakRefundStam): tracks when a mobile last came OUT of hiding so "opened
    // the fight from stealth" can be checked when their first hit lands (the reveal happens at
    // attack time, seconds before the swing resolves). PlayerMobile.OnHiddenChanged records it.
    private const long RevealWindowMs = 10_000;

    private static readonly Dictionary<Mobile, long> _lastRevealTick = new();

    public static void RecordReveal(Mobile m)
    {
        if (m != null)
        {
            _lastRevealTick[m] = Core.TickCount;
        }
    }

    public static bool WasRecentlyRevealed(Mobile m) =>
        m != null && _lastRevealTick.TryGetValue(m, out var tick) && Core.TickCount - tick < RevealWindowMs;

    // Penelope web-snare: slows `target`'s swing speed by pct% for `duration`; a re-apply
    // refreshes — except on players, who get one snare per lockout window (PvP budget above).
    public static void SetSnare(Mobile target, int pct, TimeSpan duration)
    {
        if (target == null || pct <= 0)
        {
            return;
        }

        var now = Core.TickCount;

        if (target.Player && _snareLockoutUntil.TryGetValue(target, out var lockout) && now < lockout)
        {
            return;
        }

        var expiry = now + (long)duration.TotalMilliseconds;
        _snare[target] = (pct, expiry);

        if (target.Player)
        {
            _snareLockoutUntil[target] = expiry + PlayerDebuffLockoutMs;
        }
    }

    public static int GetSnarePct(Mobile target) =>
        target != null && _snare.TryGetValue(target, out var s) && Core.TickCount < s.ExpiryTick ? s.Pct : 0;

    // Drops every entry that references a mobile (as attacker, marked target, or marker).
    public static void Evict(Mobile m)
    {
        if (m == null)
        {
            return;
        }

        _attackers.Remove(m);
        _defenders.Remove(m);

        StopIndicatorPulse(m);

        var wasIndicated = false;

        if (_marks.Remove(m))
        {
            BuffHelper.RemoveBuff(m, BuffIcon.EnemyOfOneDebuff);
            wasIndicated = true;
        }

        _stunImmuneUntil.Remove(m);

        if (_healBlockUntil.Remove(m))
        {
            BuffHelper.RemoveBuff(m, BuffIcon.MortalStrike);
            wasIndicated = true;
        }

        if (wasIndicated && !m.Deleted)
        {
            m.InvalidateProperties();
        }

        _hitStacks.Remove(m);

        if (_nextHitCrit.Remove(m))
        {
            BuffHelper.RemoveBuff(m, BuffIcon.LightningStrike);
        }

        if (_frenzy.Remove(m))
        {
            BuffHelper.RemoveBuff(m, BuffIcon.Rage);
        }

        if (_wardSurgeUntil.Remove(m))
        {
            BuffHelper.RemoveBuff(m, BuffIcon.Protection);
        }

        _snare.Remove(m);
        _ramps.Remove(m);
        _lastRevealTick.Remove(m);
        _healBlockLockoutUntil.Remove(m);
        _snareLockoutUntil.Remove(m);
    }
}
