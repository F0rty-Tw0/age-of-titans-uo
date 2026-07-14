using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon family. L6 trash. Donor: GiantSerpent.
// Spelled "Serpentling" (spec's own note flags the source table's "Servpentling" as a typo).
[SerializationGenerator(0, false)]
public partial class DrakonSerpentling : BaseCreature
{
    [Constructible]
    public DrakonSerpentling() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x066D;
        BaseSoundID = 219;

        SetStr(400, 430);
        SetDex(90, 110);
        SetInt(60, 80);

        SetHits(465, 475);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Poison, 60);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 80, 95);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.Poisoning, 75.0, 95.0);
        SetSkill(SkillName.MagicResist, 40.0, 55.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 90.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a drakon serpentling's corpse";
    public override string DefaultName => "a drakon serpentling";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison HitPoison => Poison.Regular;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
