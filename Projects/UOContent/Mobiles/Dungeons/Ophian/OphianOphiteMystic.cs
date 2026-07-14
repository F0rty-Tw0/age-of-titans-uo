using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - Ophite hierophants, L6 trash. Donor: Ophidian Archmage.
[SerializationGenerator(0, false)]
public partial class OphianOphiteMystic : BaseCreature
{
    [Constructible]
    public OphianOphiteMystic() : base(AIType.AI_Mage)
    {
        Body = 85;
        Hue = 0x0847;
        BaseSoundID = 639;

        SetStr(320, 350);
        SetDex(120, 140);
        SetInt(180, 210);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 45, 52);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.EvalInt, 85.0, 98.0);
        SetSkill(SkillName.Magery, 85.0, 98.0);
        SetSkill(SkillName.Poisoning, 65.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 55.0, 68.0);
        SetSkill(SkillName.Wrestling, 40.0, 55.0);

        Fame = 7200;
        Karma = -7200;

        VirtualArmor = 50;
    }

    public override string CorpseName => "an Ophite coil-mystic's corpse";
    public override string DefaultName => "an Ophite coil-mystic";

    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
