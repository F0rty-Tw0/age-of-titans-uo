using System;
using Server.Mobiles;

namespace Server.Engines.BuffIcons;

// Applies buff-bar icons for effects that have no official OSI cliloc (custom rarity procs,
// and era-anachronistic debuffs we surface on T2A). Routes the display name through a single
// passthrough cliloc so the label is arbitrary text — tune the cliloc in ONE place if the
// client renders it blank.
public static class BuffHelper
{
    // ponytail: 1114057 = "~1_val~" — a cliloc that renders its single argument verbatim, so the
    // buff name is whatever string we pass. If ClassicUO shows a blank buff title, swap this for
    // another passthrough cliloc (verified in-client). This is the one knob to turn.
    private const int PassthroughCliloc = 1114057;

    // duration == default => indefinite icon (removed explicitly via Remove). Otherwise the icon
    // auto-expires client- and server-side on its own timer, which is what we want for effects
    // whose game-state expires lazily (no per-effect timer to hang a RemoveBuff on).
    //
    // Secondary cliloc is 0 on purpose: the client renders the secondary with the SAME args as
    // the title when no secondary args are sent (ClassicUO BuffDebuff handler), so a passthrough
    // secondary duplicated the whole label as a second tooltip line. 0 skips that line — the
    // tooltip stays a single "Name: benefits" line.
    public static void AddCustomBuff(
        Mobile m, BuffIcon icon, string label, TimeSpan duration = default, bool retainThroughDeath = false
    )
    {
        if (m is PlayerMobile pm)
        {
            pm.AddBuff(new BuffInfo(icon, PassthroughCliloc, 0, duration, label, retainThroughDeath));
        }
    }

    public static void RemoveBuff(Mobile m, BuffIcon icon) => (m as PlayerMobile)?.RemoveBuff(icon);
}
