using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L6. Donor: Kraken.
[SerializationGenerator(0, false)]
public partial class BrineCrusher : BaseCreature
{
    [Constructible]
    public BrineCrusher() : base(AIType.AI_Melee)
    {
        Name = "a brine crusher";

        Body = 77;
        Hue = 0x04F8;
        BaseSoundID = 353;

        SetStr(700, 740);
        SetDex(200, 220);
        SetInt(40, 60);

        SetHits(530, 545);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Cold, 30);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 30.0, 45.0);
        SetSkill(SkillName.Tactics, 55.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 70.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;

        // Kraken donor is water-locked (CantWalk); this family shares land spawners,
        // so it swims AND walks (same fix as BrineLeechEel).
        CanSwim = true;
    }

    public override string CorpseName => "a brine crusher's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
