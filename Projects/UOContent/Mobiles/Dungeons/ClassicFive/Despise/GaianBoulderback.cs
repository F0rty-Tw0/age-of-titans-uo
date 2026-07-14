using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L4 core. Donor: Ogre.
[SerializationGenerator(0, false)]
public partial class GaianBoulderback : BaseCreature
{
    [Constructible]
    public GaianBoulderback() : base(AIType.AI_Melee)
    {
        Body = 1;
        Hue = 0x09C2;
        BaseSoundID = 427;

        SetStr(190, 215);
        SetDex(50, 65);
        SetInt(40, 65);

        SetHits(220, 238);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 18, 25);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 52.0, 62.0);
        SetSkill(SkillName.Tactics, 58.0, 68.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 3050;
        Karma = -3050;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a boulder-back's corpse";
    public override string DefaultName => "a boulder-back";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
