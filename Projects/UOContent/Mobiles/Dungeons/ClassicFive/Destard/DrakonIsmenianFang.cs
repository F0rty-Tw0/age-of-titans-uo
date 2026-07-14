using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Ismenian brood. L7. Donor: OphidianWarrior.
[SerializationGenerator(0, false)]
public partial class DrakonIsmenianFang : BaseCreature
{
    [Constructible]
    public DrakonIsmenianFang() : base(AIType.AI_Melee)
    {
        Body = 86;
        Hue = 0x066D;
        BaseSoundID = 634;

        SetStr(575, 615);
        SetDex(165, 185);
        SetInt(65, 90);

        SetHits(645, 665);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.Swords, 85.0, 100.0);
        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);

        Fame = 9200;
        Karma = -9200;

        VirtualArmor = 58;
    }

    public override string CorpseName => "an Ismenian fang's corpse";
    public override string DefaultName => "an Ismenian fang";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
