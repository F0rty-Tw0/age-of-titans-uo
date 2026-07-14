using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L4 trash.
// Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class ArgusHarpy : BaseCreature
{
    [Constructible]
    public ArgusHarpy() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0798;
        BaseSoundID = 402;

        SetStr(180, 210);
        SetDex(120, 150);
        SetInt(55, 75);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 30);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 24);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 68.0, 85.0);
        SetSkill(SkillName.Wrestling, 60.0, 78.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a vault harpy's corpse";
    public override string DefaultName => "a vault harpy";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanFly => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager, 2);
    }
}
