using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L4 trash. Donor: Ant Lion.
[SerializationGenerator(0, false)]
public partial class MyrmiBurrower : BaseCreature
{
    [Constructible]
    public MyrmiBurrower() : base(AIType.AI_Melee)
    {
        Body = 787;
        Hue = 0x0966;
        BaseSoundID = 1006;

        SetStr(160, 185);
        SetDex(85, 105);
        SetInt(35, 50);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 45);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 18, 25);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a myrmex burrower's corpse";
    public override string DefaultName => "a myrmex burrower";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
