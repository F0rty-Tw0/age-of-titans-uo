using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - Terathan Keep serpents, L7 trash. Donor: Ophidian Matriarch.
[SerializationGenerator(0, false)]
public partial class OphianTitanspawn : BaseCreature
{
    [Constructible]
    public OphianTitanspawn() : base(AIType.AI_Mage)
    {
        Body = 87;
        Hue = 0x0453;
        BaseSoundID = 644;

        SetStr(420, 460);
        SetDex(140, 160);
        SetInt(240, 270);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 34, 42);
        SetResistance(ResistanceType.Cold, 34, 42);
        SetResistance(ResistanceType.Poison, 52, 60);
        SetResistance(ResistanceType.Energy, 34, 42);

        SetSkill(SkillName.EvalInt, 88.0, 100.0);
        SetSkill(SkillName.Magery, 88.0, 100.0);
        SetSkill(SkillName.Poisoning, 70.0, 88.0);
        SetSkill(SkillName.MagicResist, 70.0, 82.0);
        SetSkill(SkillName.Tactics, 58.0, 70.0);
        SetSkill(SkillName.Wrestling, 45.0, 58.0);

        Fame = 9700;
        Karma = -9700;

        VirtualArmor = 56;
    }

    public override string CorpseName => "an ophian titan-spawn's corpse";
    public override string DefaultName => "an ophian titan-spawn";

    public override Poison PoisonImmune => Poison.Greater;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
