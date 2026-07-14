using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L4 trash. Donor: Gazer Larva.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineApprentice : BaseCreature
{
    [Constructible]
    public ArgusTelchineApprentice() : base(AIType.AI_Mage)
    {
        Body = 778;
        Hue = 0x08A5;
        BaseSoundID = 377;

        SetStr(150, 175);
        SetDex(110, 130);
        SetInt(190, 215);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 18, 26);
        SetResistance(ResistanceType.Energy, 26, 34);

        SetSkill(SkillName.EvalInt, 62.0, 72.0);
        SetSkill(SkillName.Magery, 62.0, 72.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 52.0, 62.0);
        SetSkill(SkillName.Wrestling, 48.0, 58.0);

        Fame = 2650;
        Karma = -2650;

        VirtualArmor = 43;
    }

    public override string CorpseName => "a Telchine apprentice's remains";
    public override string DefaultName => "a Telchine apprentice";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
