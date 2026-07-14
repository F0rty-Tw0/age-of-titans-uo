using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - Ophite hierophants, L5 trash. Donor: Ophidian Knight.
[SerializationGenerator(0, false)]
public partial class OphianOphiteSerpentguard : BaseCreature
{
    [Constructible]
    public OphianOphiteSerpentguard() : base(AIType.AI_Melee)
    {
        Body = 86;
        Hue = 0x0798;
        BaseSoundID = 634;

        SetStr(220, 250);
        SetDex(125, 155);
        SetInt(40, 55);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 35, 42);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.Poisoning, 60.0, 80.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 48;
    }

    public override string CorpseName => "an Ophite serpent-guard's corpse";
    public override string DefaultName => "an Ophite serpent-guard";

    public override Poison PoisonImmune => Poison.Greater;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
