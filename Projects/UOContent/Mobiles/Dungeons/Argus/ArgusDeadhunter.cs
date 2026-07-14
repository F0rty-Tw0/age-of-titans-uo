using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L4 trash.
// Donor: Skeleton (also covers Zombie/Spectre/Shade/Wraith/Mummy/RottingCorpse in the spawn swap).
[SerializationGenerator(0, false)]
public partial class ArgusDeadhunter : BaseCreature
{
    [Constructible]
    public ArgusDeadhunter() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x08A5;
        BaseSoundID = 0x48D;

        SetStr(190, 220);
        SetDex(90, 115);
        SetInt(40, 60);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 28, 36);
        SetResistance(ResistanceType.Poison, 28, 36);
        SetResistance(ResistanceType.Energy, 16, 24);

        SetSkill(SkillName.MagicResist, 55.0, 68.0);
        SetSkill(SkillName.Tactics, 60.0, 75.0);
        SetSkill(SkillName.Wrestling, 58.0, 72.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a bound treasure-hunter's corpse";
    public override string DefaultName => "a bound treasure-hunter";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Regular;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
