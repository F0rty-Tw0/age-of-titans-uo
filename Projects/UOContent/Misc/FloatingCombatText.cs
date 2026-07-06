using System;
using Server.Network;

namespace Server.Misc;

public static class FloatingCombatText
{
    private const int DamageHue = 0x21; // red (melee/untyped)
    private const int SpellHue = 0x2B;  // red-orange
    private const int PoisonHue = 0x3F; // dark green
    private const int HealHue = 0x44;   // green

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
            Show(target, from, '-', amount, _contextHue, _contextLabel);
        }
    }

    public static void ShowHeal(Mobile target, Mobile from, int amount)
    {
        // OnHeal fires before Hits is raised, so clamp to the actual healed amount
        var effective = Math.Min(amount, target.HitsMax - target.Hits);

        if (effective > 0)
        {
            Show(target, from, '+', effective, HealHue, _healLabel);
        }
    }

    private static void Show(Mobile target, Mobile source, char sign, int amount, int hue, string label)
    {
        var ourState = target.NetState ?? target.GetDamageMaster(source)?.NetState;
        var theirState = source?.NetState ?? source?.GetDamageMaster(target)?.NetState;

        if (ourState == null && theirState == null)
        {
            return;
        }

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

        var span = text[..pos];

        if (ourState != null)
        {
            target.PrivateOverheadMessage(MessageType.Regular, hue, false, span, ourState);
        }

        if (theirState != null && theirState != ourState)
        {
            target.PrivateOverheadMessage(MessageType.Regular, hue, false, span, theirState);
        }
    }
}
