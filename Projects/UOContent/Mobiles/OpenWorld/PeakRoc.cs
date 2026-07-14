using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks, L4. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class PeakRoc : BaseCreature
{
    [Constructible]
    public PeakRoc() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0492;
        BaseSoundID = 402;

        SetStr(158, 185);
        SetDex(90, 108);
        SetInt(42, 60);

        SetHits(200, 235);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 43);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a roc corpse";
    public override string DefaultName => "a mountain roc";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int GetAttackSound() => 916;
    public override int GetAngerSound() => 916;
    public override int GetDeathSound() => 917;
    public override int GetHurtSound() => 919;
    public override int GetIdleSound() => 918;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
