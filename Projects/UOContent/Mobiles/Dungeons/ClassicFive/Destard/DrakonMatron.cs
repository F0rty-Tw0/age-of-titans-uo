using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L7 core. Donor: SerpentineDragon.
[SerializationGenerator(0, false)]
public partial class DrakonMatron : BaseCreature
{
    [Constructible]
    public DrakonMatron() : base(AIType.AI_Mage)
    {
        Body = 103;
        Hue = 0x066D;
        BaseSoundID = 362;

        SetStr(650, 690);
        SetDex(140, 160);
        SetInt(480, 530);

        SetHits(700, 720);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Poison, 25);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 95.0, 110.0);
        SetSkill(SkillName.Magery, 100.0, 115.0);
        SetSkill(SkillName.Meditation, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 95.0, 110.0);
        SetSkill(SkillName.Tactics, 65.0, 80.0);
        SetSkill(SkillName.Wrestling, 55.0, 80.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a drakon matron's corpse";
    public override string DefaultName => "a drakon matron";

    public override int LootBagLevel => 6;

    public override int GetIdleSound() => 0x2C4;

    public override int GetAttackSound() => 0x2C0;

    public override int GetDeathSound() => 0x2C1;

    public override int GetAngerSound() => 0x2C4;

    public override int GetHurtSound() => 0x2C3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
