using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L6 trash. Donor: Terathan Matriarch.
[SerializationGenerator(0, false)]
public partial class MyrmiAiakidVenomcaster : BaseCreature
{
    [Constructible]
    public MyrmiAiakidVenomcaster() : base(AIType.AI_Mage)
    {
        Body = 72;
        Hue = 0x0021;
        BaseSoundID = 599;

        SetStr(280, 310);
        SetDex(110, 130);
        SetInt(170, 200);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Poison, 30);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 35, 42);
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

    public override string CorpseName => "an Aiakid venom-caster's corpse";
    public override string DefaultName => "an Aiakid venom-caster";

    public override Poison HitPoison => Poison.Deadly;
    public override Poison PoisonImmune => Poison.Deadly;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
