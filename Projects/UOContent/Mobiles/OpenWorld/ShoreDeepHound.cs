using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L4, the Deep Court. Donor: Hell Hound.
[SerializationGenerator(0, false)]
public partial class ShoreDeepHound : BaseCreature
{
    [Constructible]
    public ShoreDeepHound() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0530;
        BaseSoundID = 229;

        SetStr(158, 186);
        SetDex(91, 109);
        SetInt(42, 60);

        SetHits(200, 235);

        SetDamage(11, 15);

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

    public override string CorpseName => "a hell hound corpse";
    public override string DefaultName => "a deep-court hound";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
