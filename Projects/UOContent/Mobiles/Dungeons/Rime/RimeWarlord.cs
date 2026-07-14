using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L7 trash. Donor: Arctic Ogre Lord.
[SerializationGenerator(0, false)]
public partial class RimeWarlord : BaseCreature
{
    [Constructible]
    public RimeWarlord() : base(AIType.AI_Melee)
    {
        Body = 135;
        Hue = 0x0485;
        BaseSoundID = 427;

        SetStr(420, 460);
        SetDex(100, 125);
        SetInt(90, 115);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 52, 62);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a hoarfrost warlord's corpse";
    public override string DefaultName => "a hoarfrost warlord";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;

    public override int LootBagLevel => 6;

    // Northwind Crush: 20% chance on a landed hit to buffet the defender and sap stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The northwind crushes you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
