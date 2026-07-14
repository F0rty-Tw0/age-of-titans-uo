using System.Text.Json.Serialization;
using Server.Network;

namespace Server.Regions;

// Mood-only dungeon region for the custom dungeon ladder: ambient light ramp, per-zone music
// (base Region JSON prop), optional screen flash on entry. No entry gating, no PvP rules —
// these dungeons are open world; only the newbie barrow (NewbieDungeonRegion) gates.
public class ThemedDungeonRegion : BaseRegion
{
    [JsonConstructor] // Don't include parent, since it is special
    public ThemedDungeonRegion(string name, Map map, int priority, params Rectangle3D[] area) : base(name, map, priority, area)
    {
    }

    public int LightLevel { get; set; } = 12;

    public bool EntryFlash { get; set; }

    public override bool AllowHousing(Mobile from, Point3D p) => false;

    public override void OnEnter(Mobile m)
    {
        base.OnEnter(m);

        if (EntryFlash && m.NetState != null)
        {
            m.NetState.SendScreenEffect(ScreenEffectType.DarkFlash);
        }
    }

    public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
    {
        global = LightLevel;
    }
}
