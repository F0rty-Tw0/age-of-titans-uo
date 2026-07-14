using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L6 trash.
// Donor: Lich.
[SerializationGenerator(0, false)]
public partial class ArgusHoardmage : BaseCreature
{
    [Constructible]
    public ArgusHoardmage() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0486;
        BaseSoundID = 0x3E9;

        SetStr(300, 340);
        SetDex(150, 180);
        SetInt(360, 400);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 15);
        SetDamageType(ResistanceType.Cold, 35);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 25, 33);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 85.0, 95.0);
        SetSkill(SkillName.Magery, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 72.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 82.0);

        Fame = 6000;
        Karma = -6000;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a hoard-cursed lich's corpse";
    public override string DefaultName => "a hoard-cursed lich";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override bool CanRummageCorpses => true;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
