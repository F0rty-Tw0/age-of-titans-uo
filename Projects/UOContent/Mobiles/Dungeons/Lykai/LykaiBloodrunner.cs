using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - feral warband, L4 trash. Donor: Orc Bomber.
[SerializationGenerator(0, false)]
public partial class LykaiBloodrunner : BaseCreature
{
    [Constructible]
    public LykaiBloodrunner() : base(AIType.AI_Melee)
    {
        Body = 182;
        Hue = 0x0021;
        BaseSoundID = 0x45A;

        SetStr(160, 185);
        SetDex(100, 120);
        SetInt(35, 50);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 2900;
        Karma = -2900;

        VirtualArmor = 40;
    }

    public override string CorpseName => "an Arcadian blood-runner's corpse";
    public override string DefaultName => "an Arcadian blood-runner";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
