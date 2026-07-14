using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, L3. Donor: Grey Wolf.
[SerializationGenerator(0, false)]
public partial class GroveHuntwolf : BaseCreature
{
    [Constructible]
    public GroveHuntwolf() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(25, 27);
        Hue = 0x0844;
        BaseSoundID = 0xE5;

        SetStr(108, 135);
        SetDex(78, 100);
        SetInt(30, 50);

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

    public override string CorpseName => "a hunt wolf corpse";
    public override string DefaultName => "a hunt wolf";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
