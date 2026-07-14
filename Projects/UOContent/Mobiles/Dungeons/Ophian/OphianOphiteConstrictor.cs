using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - Ophite hierophants, L6 trash. Donor: Giant Serpent.
[SerializationGenerator(0, false)]
public partial class OphianOphiteConstrictor : BaseCreature
{
    [Constructible]
    public OphianOphiteConstrictor() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0453;
        BaseSoundID = 219;

        SetStr(320, 350);
        SetDex(100, 120);
        SetInt(45, 60);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 40, 48);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.Poisoning, 70.0, 90.0);
        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "an Ophite temple-constrictor's corpse";
    public override string DefaultName => "an Ophite temple-constrictor";

    public override Poison PoisonImmune => Poison.Greater;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
