using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L5 fodder. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class CursedGraveslime : BaseCreature
{
    [Constructible]
    public CursedGraveslime() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0851;
        BaseSoundID = 456;

        SetStr(160, 185);
        SetDex(60, 80);
        SetInt(30, 45);

        SetHits(280, 310);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Poison, 40);

        SetResistance(ResistanceType.Physical, 34, 42);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 35.0, 45.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 3200;
        Karma = -3200;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a puddle of ichor";
    public override string DefaultName => "a grave slime";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
