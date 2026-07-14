using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - ambient fodder, L3. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class ArgusVaultmoth : BaseCreature
{
    [Constructible]
    public ArgusVaultmoth() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x08A5;
        BaseSoundID = 422;

        SetStr(95, 115);
        SetDex(90, 110);
        SetInt(20, 32);

        SetHits(130, 155);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 24, 30);
        SetResistance(ResistanceType.Fire, 14, 20);
        SetResistance(ResistanceType.Cold, 14, 20);
        SetResistance(ResistanceType.Poison, 14, 20);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.MagicResist, 32.0, 42.0);
        SetSkill(SkillName.Tactics, 38.0, 48.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 1100;
        Karma = -1100;

        VirtualArmor = 27;
    }

    public override string CorpseName => "a vault moth's remains";
    public override string DefaultName => "a vault moth";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanFly => true;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
