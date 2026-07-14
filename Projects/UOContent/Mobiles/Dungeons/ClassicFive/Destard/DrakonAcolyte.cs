using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon family. L6 trash. Donor: OphidianWarrior.
[SerializationGenerator(0, false)]
public partial class DrakonAcolyte : BaseCreature
{
    [Constructible]
    public DrakonAcolyte() : base(AIType.AI_Melee)
    {
        Body = 86;
        Hue = 0x0501;
        BaseSoundID = 634;

        SetStr(420, 450);
        SetDex(150, 170);
        SetInt(70, 90);

        SetHits(485, 495);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.Swords, 70.0, 90.0);
        SetSkill(SkillName.MagicResist, 75.0, 90.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);

        Fame = 5000;
        Karma = -5000;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a drakon acolyte's corpse";
    public override string DefaultName => "a drakon acolyte";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
