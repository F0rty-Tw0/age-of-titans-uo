using ModernUO.Serialization;

namespace Server.Mobiles;

// Greeter at the mouth of the Barrow of the Unremembered (dev-docs/newbie-dungeon.md §2).
// Blessed + CantWalk makes it a fixed, invulnerable prop rather than a combat NPC — same
// idiom as HarborMaster. Speaks once per approach: Say() fires only on the range-crossing
// transition (Prisoner.cs idiom), so leaving and re-entering range is the natural cooldown.
[SerializationGenerator(0, false)]
public partial class NewbieFerryman : BaseCreature
{
    [Constructible]
    public NewbieFerryman() : base(AIType.AI_Animal, FightMode.None)
    {
        Body = 31; // Headless One donor body
        Hue = 0x0455;
        BaseSoundID = 0x39D;
        Name = "the Ferryman's Shade";

        Blessed = true;
        CantWalk = true;
        Direction = Direction.South;
    }

    public override bool CanTeach => false;
    public override bool ClickTitle => false;

    public override void OnMovement(Mobile m, Point3D oldLocation)
    {
        base.OnMovement(m, oldLocation);

        if (m is PlayerMobile && m.Alive && InRange(m, 4) && !InRange(oldLocation, 4))
        {
            Say("Fresh blood. The dead ahead are weak — go blood them.");
        }
    }
}
