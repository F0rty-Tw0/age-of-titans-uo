using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L4 trash. Donor: Scorpion.
[SerializationGenerator(0, false)]
public partial class MyrmiStinger : BaseCreature
{
    [Constructible]
    public MyrmiStinger() : base(AIType.AI_Melee)
    {
        Body = 48;
        Hue = 0x0798;
        BaseSoundID = 397;

        SetStr(160, 185);
        SetDex(90, 110);
        SetInt(35, 50);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Poisoning, 65.0, 85.0);
        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 2900;
        Karma = -2900;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a myrmex stinger's corpse";
    public override string DefaultName => "a myrmex stinger";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
