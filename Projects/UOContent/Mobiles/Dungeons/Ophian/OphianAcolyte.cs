using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum (dev-docs/gap-families-bestiary.md §6.5) - Terathan Keep serpents, L4 trash. Donor: Ophidian Mage.
[SerializationGenerator(0, false)]
public partial class OphianAcolyte : BaseCreature
{
    [Constructible]
    public OphianAcolyte() : base(AIType.AI_Mage)
    {
        Body = 85;
        Hue = 0x0847;
        BaseSoundID = 639;

        SetStr(160, 185);
        SetDex(100, 120);
        SetInt(90, 110);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.EvalInt, 55.0, 68.0);
        SetSkill(SkillName.Magery, 55.0, 68.0);
        SetSkill(SkillName.Poisoning, 50.0, 65.0);
        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 40.0, 55.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 38;
    }

    public override string CorpseName => "an ophian acolyte's corpse";
    public override string DefaultName => "an ophian acolyte";

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
