using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L4 trash.
// Donor: Stone Harpy.
[SerializationGenerator(0, false)]
public partial class ArgusStoneEye : BaseCreature
{
    [Constructible]
    public ArgusStoneEye() : base(AIType.AI_Melee)
    {
        Body = 73;
        Hue = 0x08A5;
        BaseSoundID = 402;

        SetStr(280, 310);
        SetDex(120, 150);
        SetInt(55, 75);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Poison, 25);

        SetResistance(ResistanceType.Physical, 46, 56);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 28, 36);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 70.0, 90.0);
        SetSkill(SkillName.Wrestling, 68.0, 88.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a stone-eyed harpy's corpse";
    public override string DefaultName => "a stone-eyed harpy";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanFly => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average, 2);
    }
}
