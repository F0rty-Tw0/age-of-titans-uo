using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - ambient fodder, L3. Donor: Gazer Larva.
[SerializationGenerator(0, false)]
public partial class ArgusMoteling : BaseCreature
{
    [Constructible]
    public ArgusMoteling() : base(AIType.AI_Mage)
    {
        Body = 778;
        Hue = 0x0486;
        BaseSoundID = 377;

        SetStr(90, 110);
        SetDex(65, 85);
        SetInt(95, 120);

        SetHits(130, 155);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 24, 30);
        SetResistance(ResistanceType.Fire, 16, 22);
        SetResistance(ResistanceType.Cold, 12, 18);
        SetResistance(ResistanceType.Poison, 12, 18);
        SetResistance(ResistanceType.Energy, 18, 24);

        SetSkill(SkillName.EvalInt, 48.0, 58.0);
        SetSkill(SkillName.Magery, 48.0, 58.0);
        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 38.0, 48.0);
        SetSkill(SkillName.Wrestling, 35.0, 45.0);

        Fame = 1150;
        Karma = -1150;

        VirtualArmor = 28;
    }

    public override string CorpseName => "a drifting mote's remains";
    public override string DefaultName => "a drifting mote";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
