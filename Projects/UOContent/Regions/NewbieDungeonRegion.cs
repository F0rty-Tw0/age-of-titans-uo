using System.Text.Json.Serialization;
using Server.Engines.Leveling;
using Server.Mobiles;
using Server.Network;

namespace Server.Regions;

public class NewbieDungeonRegion : BaseRegion
{
    [JsonConstructor] // Don't include parent, since it is special
    public NewbieDungeonRegion(string name, Map map, int priority, params Rectangle3D[] area) : base(name, map, priority, area)
    {
    }

    public NewbieDungeonRegion(string name, Map map, Region parent, params Rectangle3D[] area)
        : base(name, map, parent, area)
    {
    }

    public NewbieDungeonRegion(string name, Map map, Region parent, int priority, params Rectangle3D[] area)
        : base(name, map, parent, priority, area)
    {
    }

    public int LightLevel { get; set; } = 12;

    public bool EntryFlash { get; set; }

    // Where over-level players get moved when the entry gate catches them AFTER arrival
    // (recall/teleport-ins that skip OnMoveInto). Must be OUTSIDE the dungeon — GoLocation
    // can't serve here because it doubles as the [NewbieBarrow teleport anchor inside.
    public Point3D EjectLocation { get; set; }

    public override bool AllowHousing(Mobile from, Point3D p) => false;

    public override bool YoungProtected => false;

    public override bool AllowHarmful(Mobile from, Mobile target)
    {
        var fromRoot = (from as BaseCreature)?.GetMaster() ?? from;
        var targetRoot = (target as BaseCreature)?.GetMaster() ?? target;

        if (fromRoot.AccessLevel > AccessLevel.Player)
        {
            return base.AllowHarmful(from, target);
        }

        if (fromRoot is PlayerMobile && targetRoot is PlayerMobile && fromRoot != targetRoot)
        {
            return false;
        }

        return base.AllowHarmful(from, target);
    }

    public override void OnCriminalAction(Mobile m, bool message)
    {
        // Newbie dungeon: no criminal flagging, no message.
    }

    public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
    {
        if (m is PlayerMobile pm && pm.AccessLevel == AccessLevel.Player &&
            LevelSystem.GetLevel(pm) > 3)
        {
            m.SendMessage("The way into the barrow is closed to you — your tale has outgrown it.");
            return false;
        }

        return base.OnMoveInto(m, d, newLocation, oldLocation);
    }

    public override void OnEnter(Mobile m)
    {
        base.OnEnter(m);

        if (m is PlayerMobile pm && pm.AccessLevel == AccessLevel.Player &&
            LevelSystem.GetLevel(pm) > 3)
        {
            if (EjectLocation != Point3D.Zero)
            {
                m.Location = EjectLocation;
            }

            m.SendMessage("The way into the barrow is closed to you — your tale has outgrown it.");
            return;
        }

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
