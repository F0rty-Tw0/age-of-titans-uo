using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Ismenian brood. L7. Donor: GiantSerpent.
[SerializationGenerator(0, false)]
public partial class DrakonIsmenianCoil : BaseCreature
{
    [Constructible]
    public DrakonIsmenianCoil() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0489;
        BaseSoundID = 219;

        SetStr(575, 610);
        SetDex(105, 125);
        SetInt(70, 95);

        SetHits(640, 660);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 35);
        SetDamageType(ResistanceType.Poison, 65);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 90, 100);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.Poisoning, 80.0, 95.0);
        SetSkill(SkillName.MagicResist, 60.0, 75.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 80.0, 95.0);

        Fame = 9100;
        Karma = -9100;

        VirtualArmor = 57;
    }

    public override string CorpseName => "an Ismenian coil's corpse";
    public override string DefaultName => "an Ismenian coil";

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
