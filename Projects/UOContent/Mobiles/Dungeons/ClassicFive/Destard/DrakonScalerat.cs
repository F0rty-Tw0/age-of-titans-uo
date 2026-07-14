using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L6 ambient. Donor: GiantRat.
[SerializationGenerator(0, false)]
public partial class DrakonScalerat : BaseCreature
{
    [Constructible]
    public DrakonScalerat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0501;
        BaseSoundID = 0x188;

        SetStr(400, 430);
        SetDex(115, 135);
        SetInt(40, 60);

        SetHits(460, 480);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 40.0, 55.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 41;
    }

    public override string CorpseName => "a scale-rat's corpse";
    public override string DefaultName => "a scale-rat";

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
