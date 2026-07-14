using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L6. Donor: Giant Black Widow.
[SerializationGenerator(0, false)]
public partial class MireWidow : BaseCreature
{
    [Constructible]
    public MireWidow() : base(AIType.AI_Melee)
    {
        Body = 0x9D;
        Hue = 0x0844;
        BaseSoundID = 0x388;

        SetStr(315, 360);
        SetDex(108, 132);
        SetInt(58, 82);

        SetHits(470, 510);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 42, 52);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 92.0, 102.0);
        SetSkill(SkillName.Wrestling, 92.0, 102.0);

        Fame = 5500;
        Karma = -5500;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a giant spider corpse";
    public override string DefaultName => "a fen widow";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override Poison HitPoison => Poison.Deadly;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
