using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L5 trash.
// Donor: Dread Spider (also covers Giant Spider in the spawn swap).
[SerializationGenerator(0, false)]
public partial class ArgusVaultspider : BaseCreature
{
    [Constructible]
    public ArgusVaultspider() : base(AIType.AI_Melee)
    {
        Body = 11;
        Hue = 0x0486;
        BaseSoundID = 1170;

        SetStr(280, 320);
        SetDex(150, 180);
        SetInt(180, 220);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Poison, 80);

        SetResistance(ResistanceType.Physical, 42, 52);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 85, 95);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 55.0, 68.0);
        SetSkill(SkillName.Tactics, 62.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a vault spider's corpse";
    public override string DefaultName => "a vault spider";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    // Deadly bite: the vault spider's hoard-cursed venom.
    public override Poison HitPoison => Poison.Deadly;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
