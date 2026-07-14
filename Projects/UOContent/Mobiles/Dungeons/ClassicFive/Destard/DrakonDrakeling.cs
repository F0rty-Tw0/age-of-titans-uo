using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L7 core. Donor: Drake.
[SerializationGenerator(0, false)]
public partial class DrakonDrakeling : BaseCreature
{
    [Constructible]
    public DrakonDrakeling() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(60, 61);
        Hue = 0x066D;
        BaseSoundID = 362;

        SetStr(590, 630);
        SetDex(115, 135);
        SetInt(95, 115);

        SetHits(660, 680);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Fire, 25);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 50, 60);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 9400;
        Karma = -9400;

        VirtualArmor = 59;
    }

    public override string CorpseName => "a drakon drakeling's corpse";
    public override string DefaultName => "a drakon drakeling";

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
