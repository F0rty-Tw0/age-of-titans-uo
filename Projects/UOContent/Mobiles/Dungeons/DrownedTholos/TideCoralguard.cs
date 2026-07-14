using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L5 core family. Donor: Water
// Elemental.
[SerializationGenerator(0, false)]
public partial class TideCoralguard : BaseCreature
{
    [Constructible]
    public TideCoralguard() : base(AIType.AI_Melee)
    {
        Body = 16;
        Hue = 0x0532;
        BaseSoundID = 278;

        SetStr(190, 220);
        SetDex(45, 60);
        SetInt(30, 45);

        SetHits(320, 380);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 46);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 45, 55);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 1550;
        Karma = -1550;

        VirtualArmor = 44;

        CanSwim = true;
    }

    public override string CorpseName => "a coral-crusted husk";
    public override string DefaultName => "a coral guard";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Regular;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
