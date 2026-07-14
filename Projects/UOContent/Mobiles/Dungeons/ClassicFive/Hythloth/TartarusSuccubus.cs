using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash. Donor: Succubus.
[SerializationGenerator(0, false)]
public partial class TartarusSuccubus : BaseCreature
{
    [Constructible]
    public TartarusSuccubus() : base(AIType.AI_Mage)
    {
        Body = 149;
        Hue = 0x0021;
        BaseSoundID = 0x4B0;

        SetStr(495, 635);
        SetDex(125, 175);
        SetInt(505, 665);

        SetHits(830, 850);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Energy, 25);

        SetResistance(ResistanceType.Physical, 81, 91);
        SetResistance(ResistanceType.Fire, 71, 81);
        SetResistance(ResistanceType.Cold, 41, 51);
        SetResistance(ResistanceType.Poison, 51, 61);
        SetResistance(ResistanceType.Energy, 51, 61);

        SetSkill(SkillName.EvalInt, 91.0, 101.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.Meditation, 91.0, 101.0);
        SetSkill(SkillName.MagicResist, 100.0, 130.0);
        SetSkill(SkillName.Tactics, 81.0, 91.0);
        SetSkill(SkillName.Wrestling, 81.0, 91.0);

        Fame = 13200;
        Karma = -13200;

        VirtualArmor = 70;
    }

    public override string CorpseName => "a pit-succubus's corpse";
    public override string DefaultName => "a pit-succubus";

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
