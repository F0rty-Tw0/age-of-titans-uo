using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash.
// Donor: Arcane Daemon. Donor's GetWeaponAbility (ConcussionBlow) is an ML-era override - stripped.
[SerializationGenerator(0, false)]
public partial class TartarusHexfiend : BaseCreature
{
    [Constructible]
    public TartarusHexfiend() : base(AIType.AI_Mage)
    {
        Body = 0x310;
        Hue = 0x0021;
        BaseSoundID = 0x47D;

        SetStr(560, 610);
        SetDex(222, 252);
        SetInt(360, 400);

        SetHits(815, 835);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 80);
        SetDamageType(ResistanceType.Fire, 20);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 71, 81);
        SetResistance(ResistanceType.Cold, 16, 26);
        SetResistance(ResistanceType.Poison, 56, 66);
        SetResistance(ResistanceType.Energy, 36, 46);

        SetSkill(SkillName.MagicResist, 86.0, 96.0);
        SetSkill(SkillName.Tactics, 71.0, 81.0);
        SetSkill(SkillName.Wrestling, 61.0, 81.0);
        SetSkill(SkillName.Magery, 81.0, 91.0);
        SetSkill(SkillName.EvalInt, 71.0, 81.0);

        Fame = 12100;
        Karma = -12100;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a hex-fiend's corpse";
    public override string DefaultName => "a hex-fiend";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
