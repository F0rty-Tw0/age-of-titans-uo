using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - ambient/fodder, L8. Donor: Wisp.
[SerializationGenerator(0, false)]
public partial class StormPuff : BaseCreature
{
    [Constructible]
    public StormPuff() : base(AIType.AI_Mage)
    {
        Body = 58;
        Hue = 0x0480;
        BaseSoundID = 466;

        SetStr(240, 270);
        SetDex(220, 250);
        SetInt(200, 230);

        SetHits(780, 820);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 10, 20);
        SetResistance(ResistanceType.Energy, 50, 60);

        SetSkill(SkillName.EvalInt, 80.0, 90.0);
        SetSkill(SkillName.Magery, 80.0, 90.0);
        SetSkill(SkillName.MagicResist, 80.0, 90.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 6000;
        Karma = -6000;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a charged wispling's corpse";
    public override string DefaultName => "a charged wispling";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
