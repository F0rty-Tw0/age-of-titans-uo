using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L5 core family. Donor: Scorpion.
[SerializationGenerator(0, false)]
public partial class TideSpinecrab : BaseCreature
{
    [Constructible]
    public TideSpinecrab() : base(AIType.AI_Melee)
    {
        Body = 48;
        Hue = 0x0530;
        BaseSoundID = 397;

        SetStr(160, 190);
        SetDex(45, 60);
        SetInt(25, 40);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 34, 44);
        SetResistance(ResistanceType.Cold, 26, 36);
        SetResistance(ResistanceType.Poison, 45, 55);

        SetSkill(SkillName.Poisoning, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a spinecrab's corpse";
    public override string DefaultName => "a spinecrab";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Regular;
    public override Poison HitPoison => Poison.Greater; // "Venom of the Deep"

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
