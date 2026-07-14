using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L4 trash. Donor: Hell Hound.
[SerializationGenerator(0, false)]
public partial class ArgusGildedhound : BaseCreature
{
    [Constructible]
    public ArgusGildedhound() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0479;
        BaseSoundID = 229;

        SetStr(210, 240);
        SetDex(140, 165);
        SetInt(35, 50);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 25, 33);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 26);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 65.0, 78.0);
        SetSkill(SkillName.Wrestling, 65.0, 78.0);

        Fame = 2800;
        Karma = -2800;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a gilded hound's carcass";
    public override string DefaultName => "a gilded hound";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
