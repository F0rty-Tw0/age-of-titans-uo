using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L6 core. Donor: Drake.
[SerializationGenerator(0, false)]
public partial class DrakonHatchling : BaseCreature
{
    [Constructible]
    public DrakonHatchling() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(60, 61);
        Hue = 0x0501;
        BaseSoundID = 362;

        SetStr(425, 455);
        SetDex(105, 125);
        SetInt(85, 105);

        SetHits(470, 490);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Fire, 25);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 4300;
        Karma = -4300;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a drakon hatchling's corpse";
    public override string DefaultName => "a drakon hatchling";

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
