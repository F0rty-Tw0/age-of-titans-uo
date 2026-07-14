using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L7 trash. Donor: Gargoyle.
[SerializationGenerator(0, false)]
public partial class TartarusGargoyle : BaseCreature
{
    [Constructible]
    public TartarusGargoyle() : base(AIType.AI_Mage)
    {
        Body = 4;
        Hue = 0x0455;
        BaseSoundID = 372;

        SetStr(340, 380);
        SetDex(160, 190);
        SetInt(220, 250);

        SetHits(645, 655);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);

        SetSkill(SkillName.EvalInt, 80.0, 95.0);
        SetSkill(SkillName.Magery, 80.0, 95.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 75.0, 90.0);
        SetSkill(SkillName.Wrestling, 70.0, 90.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a tartarus gargoyle's corpse";
    public override string DefaultName => "a tartarus gargoyle";

    public override bool CanFly => true;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
