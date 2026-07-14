using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L6 trash. Donor: Giant Serpent.
[SerializationGenerator(0, false)]
public partial class CursedGravewurm : BaseCreature
{
    [Constructible]
    public CursedGravewurm() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0851;
        BaseSoundID = 219;

        SetStr(270, 300);
        SetDex(110, 135);
        SetInt(60, 85);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Poison, 30);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 32, 40);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 26, 34);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 72.0, 82.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a serpent's coiled remains";
    public override string DefaultName => "a cairn wurm";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
