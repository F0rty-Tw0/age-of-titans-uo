using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian family. L3 trash. Donor: Ogre.
[SerializationGenerator(0, false)]
public partial class GaianClayborn : BaseCreature
{
    [Constructible]
    public GaianClayborn() : base(AIType.AI_Melee)
    {
        Body = 1;
        Hue = 0x09C2;
        BaseSoundID = 427;

        SetStr(130, 150);
        SetDex(45, 60);
        SetInt(40, 60);

        SetHits(150, 160);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 38);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a clayborn corpse";
    public override string DefaultName => "a gaian clayborn";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
