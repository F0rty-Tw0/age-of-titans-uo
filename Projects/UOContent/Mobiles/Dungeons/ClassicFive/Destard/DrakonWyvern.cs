using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L7 core. Donor: Wyvern.
[SerializationGenerator(0, false)]
public partial class DrakonWyvern : BaseCreature
{
    [Constructible]
    public DrakonWyvern() : base(AIType.AI_Melee)
    {
        Body = 62;
        Hue = 0x066D;
        BaseSoundID = 362;

        SetStr(560, 600);
        SetDex(160, 180);
        SetInt(65, 90);

        SetHits(640, 660);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Poison, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 95, 100);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.Poisoning, 70.0, 90.0);
        SetSkill(SkillName.MagicResist, 75.0, 90.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 9100;
        Karma = -9100;

        VirtualArmor = 57;
    }

    public override string CorpseName => "a drakon wyvern's corpse";
    public override string DefaultName => "a drakon wyvern";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 6;

    public override int GetAttackSound() => 713;

    public override int GetAngerSound() => 718;

    public override int GetDeathSound() => 716;

    public override int GetHurtSound() => 721;

    public override int GetIdleSound() => 725;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
