using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum (dev-docs/gap-families-bestiary.md §6.5) - Terathan Keep serpents, L5 trash. Donor: Giant Serpent.
[SerializationGenerator(0, false)]
public partial class OphianConstrictor : BaseCreature
{
    [Constructible]
    public OphianConstrictor() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0851;
        BaseSoundID = 219;

        SetStr(220, 250);
        SetDex(90, 110);
        SetInt(45, 60);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 40, 48);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.Poisoning, 65.0, 85.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 78.0);
        SetSkill(SkillName.Wrestling, 65.0, 78.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a sacred constrictor's corpse";
    public override string DefaultName => "a temple constrictor";

    public override Poison PoisonImmune => Poison.Greater;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
