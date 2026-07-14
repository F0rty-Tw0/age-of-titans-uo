using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - ambient fodder, L4. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class OphianAsp : BaseCreature
{
    [Constructible]
    public OphianAsp() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0851;
        BaseSoundID = 0xDB;

        SetStr(155, 180);
        SetDex(95, 115);
        SetInt(35, 50);

        SetHits(190, 220);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 33, 40);
        SetResistance(ResistanceType.Fire, 18, 26);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 28, 36);
        SetResistance(ResistanceType.Energy, 18, 26);

        SetSkill(SkillName.Poisoning, 48.0, 62.0);
        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a temple asp's corpse";
    public override string DefaultName => "a temple asp";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
