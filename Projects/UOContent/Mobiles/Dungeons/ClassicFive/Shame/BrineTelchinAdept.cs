using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family (Telchines), L6. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class BrineTelchinAdept : BaseCreature
{
    [Constructible]
    public BrineTelchinAdept() : base(AIType.AI_Mage)
    {
        Name = "a telchine adept";

        Body = 16;
        Hue = 0x04F8;
        BaseSoundID = 278;

        SetStr(230, 260);
        SetDex(95, 115);
        SetInt(210, 240);

        SetHits(480, 500);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 60, 70);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.EvalInt, 78.0, 88.0);
        SetSkill(SkillName.Magery, 78.0, 88.0);
        SetSkill(SkillName.MagicResist, 95.0, 110.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;

        CanSwim = true;
    }

    public override string CorpseName => "a telchine adept's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
