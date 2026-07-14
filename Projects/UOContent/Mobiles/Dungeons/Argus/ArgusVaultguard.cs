using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L4 trash. Donor: Skeletal Knight.
[SerializationGenerator(0, false)]
public partial class ArgusVaultguard : BaseCreature
{
    [Constructible]
    public ArgusVaultguard() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(200, 230);
        SetDex(95, 120);
        SetInt(40, 58);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 26, 34);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 18, 26);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 62.0, 75.0);
        SetSkill(SkillName.Wrestling, 60.0, 72.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a vault guardian's bones";
    public override string DefaultName => "a vault guardian";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
