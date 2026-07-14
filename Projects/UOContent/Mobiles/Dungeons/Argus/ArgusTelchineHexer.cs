using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L6 trash. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineHexer : BaseCreature
{
    [Constructible]
    public ArgusTelchineHexer() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0486;
        BaseSoundID = 0x3E9;

        SetStr(230, 260);
        SetDex(130, 155);
        SetInt(280, 310);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 15);
        SetDamageType(ResistanceType.Cold, 35);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 50, 58);
        SetResistance(ResistanceType.Poison, 50, 58);
        SetResistance(ResistanceType.Energy, 40, 48);

        SetSkill(SkillName.EvalInt, 75.0, 85.0);
        SetSkill(SkillName.Magery, 75.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a Telchine blight-hexer's corpse";
    public override string DefaultName => "a Telchine blight-hexer";

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
