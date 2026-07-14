using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Ismenian brood. L7. Donor: OphidianKnight.
[SerializationGenerator(0, false)]
public partial class DrakonIsmenianWard : BaseCreature
{
    [Constructible]
    public DrakonIsmenianWard() : base(AIType.AI_Melee)
    {
        Body = 86;
        Hue = 0x0489;
        BaseSoundID = 634;

        SetStr(585, 625);
        SetDex(155, 175);
        SetInt(60, 85);

        SetHits(655, 675);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.Swords, 85.0, 100.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 90.0, 105.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 9300;
        Karma = -9300;

        VirtualArmor = 59;
    }

    public override string CorpseName => "an Ismenian ward's corpse";
    public override string DefaultName => "an Ismenian ward";

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
