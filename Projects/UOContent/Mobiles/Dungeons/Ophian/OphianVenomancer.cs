using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - Terathan Keep serpents, L5 trash. Donor: Ophidian Mage.
[SerializationGenerator(0, false)]
public partial class OphianVenomancer : BaseCreature
{
    [Constructible]
    public OphianVenomancer() : base(AIType.AI_Mage)
    {
        Body = 85;
        Hue = 0x0851;
        BaseSoundID = 639;

        SetStr(220, 250);
        SetDex(130, 150);
        SetInt(120, 145);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 40, 48);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 65.0, 78.0);
        SetSkill(SkillName.Magery, 65.0, 78.0);
        SetSkill(SkillName.Poisoning, 60.0, 80.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 55.0, 68.0);
        SetSkill(SkillName.Wrestling, 45.0, 60.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 45;
    }

    public override string CorpseName => "an ophian venomancer's corpse";
    public override string DefaultName => "an ophian venomancer";

    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
