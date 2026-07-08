using System;
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

    // Ambient damage context — single-threaded game loop, set right before
    // Mobile.Damage()/AOS.Damage() and cleared right after.
    private static int _contextHue = DamageHue;
    private static string _contextLabel;

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
    }

    private static string _healLabel;

    public static void SetHealContext(string source) => _healLabel = source;

    public static void ClearHealContext() => _healLabel = null;

    public static void ShowDamage(Mobile target, Mobile from, int amount)
    {
        if (amount > 0)
        {
            var incomingHue = _contextHue == DamageHue ? IncomingDamageHue : _contextHue;
            Show(target, from, '-', amount, _contextHue, incomingHue, _contextLabel);
        }
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

    private static void Show(Mobile target, Mobile source, char sign, int amount, int hue, int incomingHue, string label)
    {
        // "-19 (Flame Strike)" — sign + digits + " (" + label + ")"
        Span<char> text = stackalloc char[16 + (label?.Length ?? 0)];
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

        ShowSpan(target, source, text[..pos], hue, incomingHue);
    }

    // Offensive status floated over the enemy `target`; both the attacker (`source`)
    // and the target see it. Used for stun/mark/heal-block/poison/reflect/crit/miss.
    public static void ShowOffensiveStatus(Mobile target, Mobile source, string label) =>
        ShowOffensiveStatus(target, source, label, DebuffHue);

    public static void ShowOffensiveStatus(Mobile target, Mobile source, string label, int hue)
    {
        if (target != null && label != null)
        {
            ShowSpan(target, source, label, hue, hue);
        }
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
