using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L5 trash. Donor: Bone Knight.
[SerializationGenerator(0, false)]
public partial class ArgusHoardknight : BaseCreature
{
    [Constructible]
    public ArgusHoardknight() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(230, 260);
        SetDex(110, 135);
        SetInt(55, 75);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 34, 42);
        SetResistance(ResistanceType.Poison, 24, 32);
        SetResistance(ResistanceType.Energy, 24, 32);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 4300;
        Karma = -4300;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a hoard-bound knight's remains";
    public override string DefaultName => "a hoard-bound knight";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
