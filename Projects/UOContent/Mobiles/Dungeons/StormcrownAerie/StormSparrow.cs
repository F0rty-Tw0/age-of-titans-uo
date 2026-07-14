using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - ambient/fodder, L8. Donor: Eagle.
[SerializationGenerator(0, false)]
public partial class StormSparrow : BaseCreature
{
    [Constructible]
    public StormSparrow() : base(AIType.AI_Melee)
    {
        Body = 5;
        Hue = 0x0481;
        BaseSoundID = 0x2EE;

        SetStr(260, 300);
        SetDex(220, 260);
        SetInt(50, 80);

        SetHits(780, 820);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 6000;
        Karma = -6000;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a lightning sparrow's corpse";
    public override string DefaultName => "a lightning sparrow";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
