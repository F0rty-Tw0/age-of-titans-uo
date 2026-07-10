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

        _marks[target] = new MarkInfo(marker, Core.TickCount + (long)duration.TotalMilliseconds, bonusPct, allSources);
        BuffHelper.AddCustomBuff(target, BuffIcon.EnemyOfOneDebuff, "Marked", duration);
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

    // Heal-block debuff (framework §9.3, capped at 3s by the caller).
    public static void SetHealBlock(Mobile target, TimeSpan duration)
    {
        if (target != null)
        {
            var ms = (long)Math.Min(duration.TotalMilliseconds, 3000);
            _healBlockUntil[target] = Core.TickCount + ms;
            BuffHelper.AddCustomBuff(target, BuffIcon.MortalStrike, "Heal Block", TimeSpan.FromMilliseconds(ms));
            target.InvalidateProperties(); // show the "Heal Block" tooltip line
            StartIndicatorPulse(target);
        }
    }

    public static bool IsHealBlocked(Mobile target) =>
        target != null && _healBlockUntil.TryGetValue(target, out var until) && Core.TickCount < until;

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
        if (attacker != null)
        {
            _nextHitCrit.Add(attacker);
        }
    }

    public static bool ConsumeNextHitCrit(Mobile attacker) => attacker != null && _nextHitCrit.Remove(attacker);

    // Maenad: arms (or refreshes) the struck-frenzy buff.
    public static void ArmFrenzy(Mobile m, int dmgPct, int swingPct, TimeSpan duration)
    {
        if (m != null)
        {
            _frenzy[m] = (dmgPct, swingPct, Core.TickCount + (long)duration.TotalMilliseconds);
            BuffHelper.AddCustomBuff(m, BuffIcon.Rage, "Frenzy", duration);
        }
    }

    public static int GetFrenzyDamagePct(Mobile m) =>
        m != null && _frenzy.TryGetValue(m, out var f) && Core.TickCount < f.ExpiryTick ? f.DmgPct : 0;

    public static int GetFrenzySwingPct(Mobile m) =>
        m != null && _frenzy.TryGetValue(m, out var f) && Core.TickCount < f.ExpiryTick ? f.SwingPct : 0;

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
        _nextHitCrit.Remove(m);

        if (_frenzy.Remove(m))
        {
            BuffHelper.RemoveBuff(m, BuffIcon.Rage);
        }

        _ramps.Remove(m);
    }
}
