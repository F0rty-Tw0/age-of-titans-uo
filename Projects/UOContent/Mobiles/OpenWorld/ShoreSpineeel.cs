using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L3. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class ShoreSpineeel : BaseCreature
{
    [Constructible]
    public ShoreSpineeel() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0530;
        BaseSoundID = 0xDB;

        SetStr(108, 132);
        SetDex(90, 112);
        SetInt(30, 45);

        SetHits(125, 150);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "an eel corpse";
    public override string DefaultName => "a spined eel";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override Poison HitPoison => Poison.Regular;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
