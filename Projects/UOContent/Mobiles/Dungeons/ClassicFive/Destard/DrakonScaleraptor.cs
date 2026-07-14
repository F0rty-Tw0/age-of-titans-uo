using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L6 core. Donor: Raptor.
[SerializationGenerator(0, false)]
public partial class DrakonScaleraptor : BaseCreature
{
    [Constructible]
    public DrakonScaleraptor() : base(AIType.AI_Melee)
    {
        Body = 730;
        Hue = 0x0501;

        SetStr(440, 470);
        SetDex(190, 210);
        SetInt(90, 110);

        SetHits(490, 510);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 85.0, 100.0);
        SetSkill(SkillName.Wrestling, 80.0, 95.0);

        Fame = 4500;
        Karma = -4500;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a scale-raptor's corpse";
    public override string DefaultName => "a scale-raptor";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 5;

    public override int GetIdleSound() => 1573;

    public override int GetAngerSound() => 1570;

    public override int GetHurtSound() => 1572;

    public override int GetDeathSound() => 1571;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
