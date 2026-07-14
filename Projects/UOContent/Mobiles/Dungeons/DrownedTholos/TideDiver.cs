using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L4 core family. Donor: Ghoul.
[SerializationGenerator(0, false)]
public partial class TideDiver : BaseCreature
{
    [Constructible]
    public TideDiver() : base(AIType.AI_Melee)
    {
        Body = 153;
        Hue = 0x0481;
        BaseSoundID = 0x482;

        SetStr(80, 100);
        SetDex(55, 70);
        SetInt(15, 25);

        SetHits(120, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 28);
        SetResistance(ResistanceType.Cold, 22, 32);
        SetResistance(ResistanceType.Poison, 10, 15);

        SetSkill(SkillName.MagicResist, 35.0, 45.0);
        SetSkill(SkillName.Tactics, 42.0, 52.0);
        SetSkill(SkillName.Wrestling, 42.0, 52.0);

        Fame = 850;
        Karma = -850;

        VirtualArmor = 22;
    }

    public override string CorpseName => "a drowned diver's corpse";
    public override string DefaultName => "a drowned diver";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
