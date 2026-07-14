using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash.
// Donor: Gargoyle Enforcer.
[SerializationGenerator(0, false)]
public partial class TartarusScourgewing : BaseCreature
{
    [Constructible]
    public TartarusScourgewing() : base(AIType.AI_Mage)
    {
        Body = 0x2F2;
        Hue = 0x0022;
        BaseSoundID = 0x174;

        SetStr(785, 875);
        SetDex(165, 205);
        SetInt(205, 245);

        SetHits(820, 840);

        SetDamage(18, 23);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 56, 66);
        SetResistance(ResistanceType.Cold, 31, 41);
        SetResistance(ResistanceType.Poison, 38, 48);
        SetResistance(ResistanceType.Energy, 26, 36);

        SetSkill(SkillName.MagicResist, 122.0, 132.0);
        SetSkill(SkillName.Tactics, 82.0, 92.0);
        SetSkill(SkillName.Wrestling, 92.0, 102.0);
        SetSkill(SkillName.Swords, 92.0, 102.0);
        SetSkill(SkillName.Anatomy, 82.0, 92.0);
        SetSkill(SkillName.Magery, 92.0, 102.0);
        SetSkill(SkillName.EvalInt, 82.0, 102.0);
        SetSkill(SkillName.Meditation, 82.0, 102.0);

        Fame = 12100;
        Karma = -12100;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a scourge-wing's corpse";
    public override string DefaultName => "a scourge-wing";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
