using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, L2. Donor: Hind.
[SerializationGenerator(0, false)]
public partial class GroveFawn : BaseCreature
{
    [Constructible]
    public GroveFawn() : base(AIType.AI_Melee)
    {
        Body = 0xED;
        Hue = 0x0851;

        SetStr(72, 92);
        SetDex(62, 82);
        SetInt(22, 35);

        SetHits(80, 90);

        SetDamage(5, 7);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 25);
        SetResistance(ResistanceType.Fire, 5, 10);
        SetResistance(ResistanceType.Cold, 5, 10);
        SetResistance(ResistanceType.Poison, 10, 15);
        SetResistance(ResistanceType.Energy, 5, 10);

        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 40.0, 50.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 550;
        Karma = -550;

        VirtualArmor = 20;
    }

    public override string CorpseName => "a dappled fawn corpse";
    public override string DefaultName => "a dappled fawn";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int GetAttackSound() => 0x82;
    public override int GetHurtSound() => 0x83;
    public override int GetDeathSound() => 0x84;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
