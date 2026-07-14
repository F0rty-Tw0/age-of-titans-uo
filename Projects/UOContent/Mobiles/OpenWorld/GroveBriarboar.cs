using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves, L3. Donor: Boar.
[SerializationGenerator(0, false)]
public partial class GroveBriarboar : BaseCreature
{
    [Constructible]
    public GroveBriarboar() : base(AIType.AI_Melee)
    {
        Body = 0x122;
        Hue = 0x0844;
        BaseSoundID = 0xC4;

        SetStr(108, 132);
        SetDex(78, 98);
        SetInt(30, 45);

        SetHits(130, 155);

        SetDamage(7, 11);

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

    public override string CorpseName => "a briar boar corpse";
    public override string DefaultName => "a briar boar";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
