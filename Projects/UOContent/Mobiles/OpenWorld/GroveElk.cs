using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, L3. Donor: Great Hart.
[SerializationGenerator(0, false)]
public partial class GroveElk : BaseCreature
{
    [Constructible]
    public GroveElk() : base(AIType.AI_Melee)
    {
        Body = 0xEA;
        Hue = 0x0844;

        SetStr(108, 135);
        SetDex(78, 100);
        SetInt(30, 50);

        SetHits(135, 160);

        SetDamage(7, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a grove elk corpse";
    public override string DefaultName => "a grove elk";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int GetAttackSound() => 0x82;
    public override int GetHurtSound() => 0x83;
    public override int GetDeathSound() => 0x84;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
