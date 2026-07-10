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

    // Weapon-hit batching: while a hit is resolving, the damage number plus every offensive status
    // applied to the defender are collected into ONE overhead line ("-32 Critical! Stunned Poisoned")
    // instead of separate floats. BaseWeapon.OnHit brackets each hit with BeginHit/EndHit.
    //
    // Frames form a STACK: a re-entrant extra swing (weapon.OnSwing mid-hit) pushes its OWN frame,
    // so the nested swing's damage and the statuses its procs apply fold into the nested swing's
    // line ("-1 Stunned") instead of leaking a bare "-1" while its stun mislands on the main line.
    // Statuses fired after the nested swing returns (e.g. the Notos stagger rider, "Extra Swing"
    // itself) fold into the then-innermost frame — the main hit's.
    private sealed class HitFrame
    {
        public Mobile Subject;
        public Mobile Other;
        public int Amount;
        public int Hue;
        public int IncomingHue;
        public bool Captured;
        public bool Crit;
        public bool Shrug;
        public bool Parry;
        public readonly List<string> Labels = new();

        // Secondary line for retaliation dealt back to the ATTACKER during this hit (reflect,
        // thorns, and the "Stunned"/"Heal Block" that ride them). The attacker is Other, not the
        // primary Subject, so without this its "-N" number and its status labels would stack as
        // separate overhead lines. Emitted as ONE line ("-1 Reflect Stunned") by EndHit, mirroring
        // the defender's line. Untyped damage only — a labeled proc ("-N (Burn)") keeps its own line.
        public int SecondaryAmount;
        public bool SecondaryCaptured;
        public readonly List<string> SecondaryLabels = new();

        public void Reset(Mobile subject, Mobile other)
        {
            Subject = subject;
            Other = other;
            Amount = 0;
            Captured = false;
            Crit = Shrug = Parry = false;
            Labels.Clear();
            SecondaryAmount = 0;
            SecondaryCaptured = false;
            SecondaryLabels.Clear();
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

        if (frame.Captured)
        {
            Span<char> text = stackalloc char[256];
            var pos = 0;
            text[pos++] = '-';
            frame.Amount.TryFormat(text[pos..], out var written);
            pos += written;

            AppendSuffix(text, ref pos, frame.Crit, " Critical!");
            AppendSuffix(text, ref pos, frame.Shrug, " Shrugged");
            AppendSuffix(text, ref pos, frame.Parry, " Parried");
            AppendDefenderLabels(text, ref pos, frame.Labels);

            ShowSpan(frame.Subject, frame.Other, text[..pos], frame.Hue, frame.IncomingHue);
        }
        else if (frame.Labels.Count > 0 && frame.Subject != null)
        {
            // Status(es) applied but no damage number this hit (e.g. fully absorbed) — labels only.
            Span<char> text = stackalloc char[256];
            var pos = 0;
            AppendDefenderLabels(text, ref pos, frame.Labels);

            if (pos > 1)
            {
                ShowSpan(frame.Subject, frame.Other, text[1..pos], DebuffHue, DebuffHue); // drop leading space
            }
        }

        // Attacker's retaliation line: "-1 Reflect Stunned" (or labels-only if nothing reflected).
        if ((frame.SecondaryCaptured || frame.SecondaryLabels.Count > 0) && frame.Other != null)
        {
            Span<char> text = stackalloc char[256];
            var pos = 0;

            if (frame.SecondaryCaptured)
            {
                text[pos++] = '-';
                frame.SecondaryAmount.TryFormat(text[pos..], out var written);
                pos += written;
            }

            AppendLabels(text, ref pos, frame.SecondaryLabels);

            var start = frame.SecondaryCaptured ? 0 : 1; // drop leading space when labels-only

            if (pos > start)
            {
                ShowSpan(frame.Other, frame.Subject, text[start..pos], DebuffHue, DebuffHue);
            }
        }

        frame.Reset(null, null); // drop Mobile refs while pooled
        _framePool.Add(frame);
    }

    private static void AppendLabels(Span<char> text, ref int pos, List<string> labels)
    {
        for (var i = 0; i < labels.Count; i++)
        {
            AppendLabel(text, ref pos, labels[i]);
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

    // The defender's line, with extra-swing folding. RarityEffects.DoExtraSwing adds one
    // "Extra Swing" per bonus swing, and a whiffed bonus swing's OnMiss adds "Miss" — both land
    // in the frame's Labels. Instead of leaking "Miss Extra Swing Extra Swing", they render as a single
    // tail: "Extra Swing" / "Extra Swing x2" / "Extra Swing Miss" / "Extra Swing x2 Miss". A
    // batched "Miss" can ONLY be an extra swing whiffing — a main-swing miss never opens a hit
    // batch (BeginHit runs only from OnHit) — so folding it here is safe. Every batched "Miss" is
    // paired with an "Extra Swing" (DoExtraSwing adds it whether the swing hit or missed), so the
    // summary always leads with "Extra Swing".
    private static void AppendDefenderLabels(Span<char> text, ref int pos, List<string> labels)
    {
        var extraSwings = 0;
        var extraSwingMissed = false;

        for (var i = 0; i < labels.Count; i++)
        {
            var label = labels[i];

            switch (label)
            {
                case ExtraSwingLabel:
                    {
                        extraSwings++;
                        break;
                    }
                case MissLabel:
                    {
                        extraSwingMissed = true;
                        break;
                    }
                default:
                    {
                        AppendLabel(text, ref pos, label);
                        break;
                    }
            }
        }

        if (extraSwings == 0)
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

        if (extraSwingMissed)
        {
            AppendLabel(text, ref pos, MissLabel);
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

        // Untyped retaliation (reflect/thorns) back to the attacker during this hit — fold into the
        // attacker's secondary line (EndHit emits it). Summed so multiple retaliations share the
        // line; a labeled proc keeps _contextLabel and falls through to its own float below.
        if (frame?.Other != null && target == frame.Other && _contextLabel == null)
        {
            frame.SecondaryAmount += amount;
            frame.SecondaryCaptured = true;
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

        // Fold a status on the current swing's defender into that swing's damage line (EndHit
        // emits it) — the innermost frame, so an extra swing's stun rides the extra swing's "-N".
        var frame = CurrentFrame;

        if (frame != null && target == frame.Subject)
        {
            frame.Labels.Add(label);
            return;
        }

        // A status on the attacker (Reflect, Stunned, Heal Block) joins the attacker's retaliation
        // line so it shares the "-N" instead of stacking on its own line.
        if (frame?.Other != null && target == frame.Other)
        {
            frame.SecondaryLabels.Add(label);
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
