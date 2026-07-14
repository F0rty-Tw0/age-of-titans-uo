using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon family. L6 trash. Donor: Raptor.
[SerializationGenerator(0, false)]
public partial class DrakonScaleHound : BaseCreature
{
    [Constructible]
    public DrakonScaleHound() : base(AIType.AI_Melee)
    {
        Body = 730;
        Hue = 0x0501;

        SetStr(430, 460);
        SetDex(180, 200);
        SetInt(100, 130);

        SetHits(495, 505);

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

        VirtualArmor = 46;
    }

    public override string CorpseName => "a scale-hound's corpse";
    public override string DefaultName => "a scale-hound";

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
