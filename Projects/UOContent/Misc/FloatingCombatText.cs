using System;
using System.Collections.Generic;
using Server.Network;

namespace Server.Misc;

// Overhead floating text for combat/item effects. Two conventions, applied at ~90 call sites
// (mostly in RarityEffects.*.cs plus BaseWeapon/Paralyze). Keep them when adding a new effect:
//
//  * POSITIONING: an OFFENSIVE effect (happens to an enemy) floats over the ENEMY and is shown to
//    both parties (ShowOffensiveStatus / ShowDamage). A BENEFICIAL effect (happens to you) floats
//    over YOU only (ShowSelfStatus / ShowRestore / ShowHeal, source == null).
//
//  * DE-NOISE — a busy hit can fire many procs; floating them all makes combat unreadable. So:
//      - DON'T float AoE-to-nearby procs (splash, mark-spread) or passive per-tick regen — only
//        discrete effects on the primary subject.
//      - Gate a stun float on CombatFxState.TryStun's bool return (it no-ops inside its immunity
//        window; floating "Stunned" unconditionally would lie).
//      - No "Hit" text — the damage number already conveys the hit; only "Miss" is shown.
//      - Restores capture the real delta (before/after) so ShowRestore no-ops at cap instead of
//        showing a phantom "+N".
public static class FloatingCombatText
{
    private const int DamageHue = 0x21; // red (melee/untyped)
    private const int IncomingDamageHue = 0x490; // bright pink (victim's own view)
    private const int SpellHue = 0x2B;  // red-orange
    private const int HealHue = 0x44;   // green

    // Public status/restore hues (cosmetic, tunable) — referenced by effect call sites.
    public const int PoisonHue = 0x3F;  // dark green
    public const int MissHue = 0x3B2;   // gray (a swing that whiffs)
    public const int CritHue = 0x26;    // bright red (critical / execute)
    public const int DebuffHue = 0x25;  // orange (offensive: stun, mark, heal-block, mana-drain, reflect)
    public const int StamHue = 0x99;    // gold (stamina restored)
    public const int ManaHue = 0x5;     // blue (mana restored)
    public const int BuffHue = 0x59;    // teal (self-buff: frenzy, warded, dodge, crit-ready)

    // Shared label literals. Referenced at the call sites (BaseWeapon.OnMiss, RarityEffects.
    // DoExtraSwing) AND by EndHit's extra-swing folding, so they must stay identical — keep them
    // as consts rather than scattering the string.
    public const string MissLabel = "Miss";
    public const string ExtraSwingLabel = "Extra Swing";

    // Ambient damage context — single-threaded game loop, set right before
    // Mobile.Damage()/AOS.Damage() and cleared right after.
    private static int _contextHue = DamageHue;
    private static string _contextLabel;
    private static bool _contextCrit;
    private static bool _contextShrug;
    private static bool _contextParry;

    // Weapon-hit batching: while a hit is resolving, the damage number and the statuses applied
    // during it are collected so each status can be PAIRED with the number that caused it — one
    // effect per overhead line, never merged. The defender's main hit takes its first status
    // ("-32 Critical! Stunned"); any further status ("Poisoned") floats on its own line, as does a
    // whiffed extra swing's "Miss". BaseWeapon.OnHit brackets each hit with BeginHit/EndHit.
    //
    // Frames form a STACK: a re-entrant extra swing (weapon.OnSwing mid-hit) pushes its OWN frame,
    // so the nested swing's damage and the status its procs apply pair on the nested swing's line
    // ("-1 Stunned") instead of leaking a bare "-1" while its stun mislands on the main line.
    // Statuses fired after the nested swing returns (e.g. the Notos stagger rider, "Extra Swing"
    // itself) fold into the then-innermost frame — the main hit's.
    private sealed class HitFrame
    {
        public Mobile Subject;
        public Mobile Other;

        // Subject's (defender's) main melee number and the crit/shrug/parry/extra-swing decorations
        // that describe it. EndHit emits "-N [Critical!] [Extra Swing]" and pairs the FIRST status
        // onto that line.
        public int Amount;
        public int Hue;
        public int IncomingHue;
        public bool Captured;
        public bool Crit;
        public bool Shrug;
        public bool Parry;
        public int ExtraSwings;
        public bool ExtraSwingMissed;
        public readonly List<string> Statuses = new();

        // Retaliation dealt back to the ATTACKER (Other) this hit (reflect/thorns and the
        // "Stunned"/"Heal Block" that ride them). Emitted per-proc as it resolves so each number
        // pairs with its OWN status ("-5 Reflect", then "-88 Stunned") — never summed or merged.
        // OtherPending holds a retaliation number until its status pairs onto it (or EndHit flushes
        // it bare). Untyped damage only; a labeled proc keeps its own float.
        public int OtherPending;
        public bool OtherPendingSet;

        public void Reset(Mobile subject, Mobile other)
        {
            Subject = subject;
            Other = other;
            Amount = 0;
            Captured = false;
            Crit = Shrug = Parry = false;
            ExtraSwings = 0;
            ExtraSwingMissed = false;
            Statuses.Clear();
            OtherPending = 0;
            OtherPendingSet = false;
        }
    }

    // Innermost frame = last. Popped frames return to the pool — zero steady-state allocation.
    private static readonly List<HitFrame> _frames = new();
    private static readonly List<HitFrame> _framePool = new();

    private static HitFrame CurrentFrame => _frames.Count > 0 ? _frames[^1] : null;

    public static void Initialize()
    {
        // Suppress the raw damage packet (0x0B) so clients stop rendering their own
        // "N [dps M]" numbers; the overhead text below replaces it.
        Mobile.VisibleDamageType = VisibleDamageType.None;
    }

    public static void SetSpellContext(string spellName)
    {
        _contextHue = SpellHue;
        _contextLabel = spellName;
    }

    public static void SetPoisonContext()
    {
        _contextHue = PoisonHue;
        _contextLabel = "Poison";
    }

    public static void ClearContext()
    {
        _contextHue = DamageHue;
        _contextLabel = null;
        _contextCrit = false;
        _contextShrug = false;
        _contextParry = false;
    }

    // A landed crit folds "Critical!" into the damage line (see ShowDamage) instead of a
    // separate float. BaseWeapon sets this around the melee AOS.Damage call.
    public static void SetCritContext() => _contextCrit = true;

    public static void ClearCritContext() => _contextCrit = false;

    // A shrugged (halved) hit folds "Shrugged" into the damage line the same way.
    public static void SetShrugContext() => _contextShrug = true;

    public static void ClearShrugContext() => _contextShrug = false;

    // A parried hit folds "Parried" into the damage line the same way.
    public static void SetParryContext() => _contextParry = true;

    public static void ClearParryContext() => _contextParry = false;

    private static string _healLabel;

    public static void SetHealContext(string source) => _healLabel = source;

    public static void ClearHealContext() => _healLabel = null;

    // Begin collecting one swing's floats into a single line over `defender`. A nested call
    // (re-entrant extra swing) pushes its own frame — each swing owns its own line.
    public static void BeginHit(Mobile defender, Mobile attacker)
    {
        HitFrame frame;

        if (_framePool.Count > 0)
        {
            frame = _framePool[^1];
            _framePool.RemoveAt(_framePool.Count - 1);
        }
        else
        {
            frame = new HitFrame();
        }

        frame.Reset(defender, attacker);
        _frames.Add(frame);
    }

    // Emit the innermost frame's collected line(s) and pop it.
    public static void EndHit()
    {
        var frame = CurrentFrame;

        if (frame == null)
        {
            return;
        }

        _frames.RemoveAt(_frames.Count - 1);

        // Defender's line: "-N [Critical!] [Extra Swing]" + its first status; further statuses and a
        // whiffed extra swing float on their own lines.
        if (frame.Subject != null)
        {
            EmitPairedLines(
                frame.Subject, frame.Other, frame.Hue, frame.IncomingHue,
                frame.Captured, frame.Amount, frame.Crit, frame.Shrug, frame.Parry, frame.ExtraSwings,
                frame.Statuses
            );

            if (frame.ExtraSwingMissed)
            {
                ShowSpan(frame.Subject, frame.Other, MissLabel, MissHue, MissHue);
            }
        }

        // A retaliation number left with no status pairs onto it (reflect with no rider) — flush bare.
        if (frame.Other != null && frame.OtherPendingSet)
        {
            FloatRetaliation(frame, frame.OtherPending, null);
        }

        frame.Reset(null, null); // drop Mobile refs while pooled
        _framePool.Add(frame);
    }

    // Emit one target's overhead lines. The damage number (if captured) plus its crit/shrug/parry
    // and extra-swing decorations form the first line, carrying the FIRST status paired onto it
    // ("-88 Stunned"). Every remaining status floats on its own line — effects are never merged.
    private static void EmitPairedLines(
        Mobile subject, Mobile other, int otherHue, int subjectHue,
        bool captured, int amount, bool crit, bool shrug, bool parry, int extraSwings, List<string> statuses
    )
    {
        Span<char> text = stackalloc char[256];
        var pos = 0;

        if (captured)
        {
            text[pos++] = '-';
            amount.TryFormat(text[pos..], out var written);
            pos += written;
            AppendSuffix(text, ref pos, crit, " Critical!");
            AppendSuffix(text, ref pos, shrug, " Shrugged");
            AppendSuffix(text, ref pos, parry, " Parried");
        }

        AppendExtraSwing(text, ref pos, extraSwings);

        var firstStatus = 0;

        if (statuses.Count > 0)
        {
            AppendLabel(text, ref pos, statuses[0]); // prepends a space
            firstStatus = 1;
        }

        // Drop the leading space when the line has no number (status/extra-swing-only); a numbered
        // line keeps the damage hue, a label-only line uses the debuff hue.
        var start = captured ? 0 : 1;

        if (pos > start)
        {
            var lineOther = captured ? otherHue : DebuffHue;
            var lineSubject = captured ? subjectHue : DebuffHue;
            ShowSpan(subject, other, text[start..pos], lineOther, lineSubject);
        }

        for (var i = firstStatus; i < statuses.Count; i++)
        {
            ShowSpan(subject, other, statuses[i], DebuffHue, DebuffHue);
        }
    }

    private static void AppendLabel(Span<char> text, ref int pos, string label)
    {
        if (pos + 1 + label.Length > text.Length)
        {
            return; // defensive: never overflow the overhead line
        }

        text[pos++] = ' ';
        label.CopyTo(text[pos..]);
        pos += label.Length;
    }

    // RarityEffects.DoExtraSwing adds one "Extra Swing" per bonus swing; multiple collapse to
    // "Extra Swing x2". Folded onto the hit line (it describes the swing, not a separate effect).
    private static void AppendExtraSwing(Span<char> text, ref int pos, int extraSwings)
    {
        if (extraSwings <= 0)
        {
            return;
        }

        AppendLabel(text, ref pos, ExtraSwingLabel);

        if (extraSwings > 1 && pos + 4 <= text.Length) // " x" + up to two digits
        {
            text[pos++] = ' ';
            text[pos++] = 'x';
            extraSwings.TryFormat(text[pos..], out var written);
            pos += written;
        }
    }

    // One retaliation float over the attacker (frame.Other), seen by both parties. amount <= 0 =
    // no number; label null = no label. "-5 Reflect" / "-88 Stunned" / "-5" / "Stunned".
    private static void FloatRetaliation(HitFrame frame, int amount, string label)
    {
        Span<char> text = stackalloc char[64 + (label?.Length ?? 0)];
        var pos = 0;

        if (amount > 0)
        {
            text[pos++] = '-';
            amount.TryFormat(text[pos..], out var written);
            pos += written;
        }

        if (label != null)
        {
            if (pos > 0)
            {
                text[pos++] = ' ';
            }

            label.CopyTo(text[pos..]);
            pos += label.Length;
        }

        if (pos > 0)
        {
            ShowSpan(frame.Other, frame.Subject, text[..pos], DebuffHue, DebuffHue);
        }
    }

    public static void ShowDamage(Mobile target, Mobile from, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        var hue = _contextHue;
        var incomingHue = _contextHue == DamageHue ? IncomingDamageHue : _contextHue;

        // A crit recolors the whole line to CritHue for both parties; shrug/parry keep the normal
        // damage hue and only tack on their suffix ("-5 Shrugged" / "-1 Parried").
        if (_contextCrit)
        {
            hue = CritHue;
            incomingHue = CritHue;
        }

        // Inside a weapon hit, the untyped main damage number over the defender is captured and folded
        // into the combined line by EndHit. Labeled damage (an elemental proc's "-10 (Lightning)", a
        // spell) always emits on its own line — hence the _contextLabel == null guard.
        var frame = CurrentFrame;

        if (frame != null && !frame.Captured && target == frame.Subject && _contextLabel == null)
        {
            frame.Amount = amount;
            frame.Hue = hue;
            frame.IncomingHue = incomingHue;
            frame.Crit = _contextCrit;
            frame.Shrug = _contextShrug;
            frame.Parry = _contextParry;
            frame.Captured = true;
            return;
        }

        // Untyped retaliation (reflect/thorns) back to the attacker during this hit — held until its
        // status pairs onto it ("-5 Reflect"). Per-proc: if a prior retaliation number is still
        // pending (two retaliations, no rider between), flush it bare first so numbers never merge.
        // A labeled proc keeps _contextLabel and falls through to its own float below.
        if (frame?.Other != null && target == frame.Other && _contextLabel == null)
        {
            if (frame.OtherPendingSet)
            {
                FloatRetaliation(frame, frame.OtherPending, null);
            }

            frame.OtherPending = amount;
            frame.OtherPendingSet = true;
            return;
        }

        Show(target, from, '-', amount, hue, incomingHue, _contextLabel, _contextCrit, _contextShrug, _contextParry);
    }

    public static void ShowHeal(Mobile target, Mobile from, int amount)
    {
        // OnHeal fires before Hits is raised, so clamp to the actual healed amount
        var effective = Math.Min(amount, target.HitsMax - target.Hits);

        if (effective > 0)
        {
            Show(target, from, '+', effective, HealHue, HealHue, _healLabel);
        }
    }

    private static void Show(
        Mobile target, Mobile source, char sign, int amount, int hue, int incomingHue, string label,
        bool crit = false, bool shrug = false, bool parry = false
    )
    {
        // "-19 (Flame Strike)" / "-49 Critical!" / "-5 Shrugged" / "-1 Parried" —
        // sign + digits + optional " (label)" + optional status suffixes.
        Span<char> text = stackalloc char[44 + (label?.Length ?? 0)];
        var pos = 0;
        text[pos++] = sign;
        amount.TryFormat(text[pos..], out var written);
        pos += written;

        if (label != null)
        {
            text[pos++] = ' ';
            text[pos++] = '(';
            label.CopyTo(text[pos..]);
            pos += label.Length;
            text[pos++] = ')';
        }

        AppendSuffix(text, ref pos, crit, " Critical!");
        AppendSuffix(text, ref pos, shrug, " Shrugged");
        AppendSuffix(text, ref pos, parry, " Parried");

        ShowSpan(target, source, text[..pos], hue, incomingHue);
    }

    private static void AppendSuffix(Span<char> text, ref int pos, bool active, string suffix)
    {
        if (active)
        {
            suffix.CopyTo(text[pos..]);
            pos += suffix.Length;
        }
    }

    // Offensive status floated over the enemy `target`; both the attacker (`source`)
    // and the target see it. Used for stun/mark/heal-block/poison/reflect/crit/miss.
    public static void ShowOffensiveStatus(Mobile target, Mobile source, string label) =>
        ShowOffensiveStatus(target, source, label, DebuffHue);

    public static void ShowOffensiveStatus(Mobile target, Mobile source, string label, int hue)
    {
        if (target == null || label == null)
        {
            return;
        }

        // Collect a status on the current swing's defender — EndHit pairs the first with the hit's
        // "-N" and floats the rest on their own lines. Extra-swing bookkeeping is folded onto the
        // hit line (Extra Swing) or floated apart (Miss), not treated as a status.
        var frame = CurrentFrame;

        if (frame != null && target == frame.Subject)
        {
            if (label == ExtraSwingLabel)
            {
                frame.ExtraSwings++;
            }
            else if (label == MissLabel)
            {
                frame.ExtraSwingMissed = true;
            }
            else
            {
                frame.Statuses.Add(label);
            }

            return;
        }

        // A status on the attacker (Reflect, Stunned, Heal Block): pairs with the retaliation number
        // that just landed ("-5 Reflect"), else floats alone ("Stunned"). One effect per line.
        if (frame?.Other != null && target == frame.Other)
        {
            var amount = frame.OtherPendingSet ? frame.OtherPending : 0;
            frame.OtherPending = 0;
            frame.OtherPendingSet = false;
            FloatRetaliation(frame, amount, label);
            return;
        }

        ShowSpan(target, source, label, hue, hue);
    }

    // Beneficial status/buff floated over `self` only (frenzy, warded, dodge, crit-ready...).
    public static void ShowSelfStatus(Mobile self, string label) => ShowSelfStatus(self, label, BuffHue);

    public static void ShowSelfStatus(Mobile self, string label, int hue)
    {
        if (self != null && label != null)
        {
            ShowSpan(self, null, label, hue, hue);
        }
    }

    // Beneficial resource restore over `self`: "+15 Stam" / "+20 Mana" / "+12 Life".
    // kind: 'S' stamina, 'M' mana, anything else = life/hits.
    public static void ShowRestore(Mobile self, char kind, int amount)
    {
        if (self == null || amount <= 0)
        {
            return;
        }

        var (unit, hue) = kind switch
        {
            'S' => ("Stam", StamHue),
            'M' => ("Mana", ManaHue),
            _   => ("Life", HealHue)
        };

        // "+15 Stam" — '+' + digits + ' ' + unit
        Span<char> text = stackalloc char[8 + unit.Length];
        var pos = 0;
        text[pos++] = '+';
        amount.TryFormat(text[pos..], out var written);
        pos += written;
        text[pos++] = ' ';
        unit.CopyTo(text[pos..]);
        pos += unit.Length;

        ShowSpan(self, null, text[..pos], hue, hue);
    }

    // Shared zero-alloc routing: floats `text` over `subject`, shown to the subject's own
    // client (subjectHue) and to `other`'s client (otherHue). When `other` is null only the
    // subject (or its damage master) sees it — the "beneficial, over self" case.
    private static void ShowSpan(Mobile subject, Mobile other, ReadOnlySpan<char> text, int otherHue, int subjectHue)
    {
        var ourState = subject.NetState ?? subject.GetDamageMaster(other)?.NetState;
        var theirState = other?.NetState ?? other?.GetDamageMaster(subject)?.NetState;

        if (ourState != null)
        {
            subject.PrivateOverheadMessage(MessageType.Regular, subjectHue, false, text, ourState);
        }

        if (theirState != null && theirState != ourState)
        {
            subject.PrivateOverheadMessage(MessageType.Regular, otherHue, false, text, theirState);
        }
    }
}
