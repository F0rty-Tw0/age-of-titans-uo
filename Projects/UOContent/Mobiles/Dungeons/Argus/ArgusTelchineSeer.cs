using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L6 trash. Donor: Gazer.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineSeer : BaseCreature
{
    [Constructible]
    public ArgusTelchineSeer() : base(AIType.AI_Mage)
    {
        Body = 22;
        Hue = 0x0486;
        BaseSoundID = 377;

        SetStr(230, 260);
        SetDex(130, 155);
        SetInt(280, 310);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 26, 34);
        SetResistance(ResistanceType.Cold, 24, 32);
        SetResistance(ResistanceType.Poison, 24, 32);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.EvalInt, 75.0, 85.0);
        SetSkill(SkillName.Magery, 75.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a Telchine evil-eye seer's remains";
    public override string DefaultName => "a Telchine evil-eye seer";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
