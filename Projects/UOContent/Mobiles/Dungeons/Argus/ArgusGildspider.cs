using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L5 trash. Donor: Dread Spider.
[SerializationGenerator(0, false)]
public partial class ArgusGildspider : BaseCreature
{
    [Constructible]
    public ArgusGildspider() : base(AIType.AI_Melee)
    {
        Body = 11;
        Hue = 0x0798;
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

    public override string CorpseName => "a gilded vault-spider's husk";
    public override string DefaultName => "a gilded vault-spider";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override Poison HitPoison => Poison.Deadly;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
