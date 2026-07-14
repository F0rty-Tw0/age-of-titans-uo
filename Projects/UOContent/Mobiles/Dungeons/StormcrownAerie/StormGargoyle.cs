using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L9 core trash. Donor: Gargoyle.
[SerializationGenerator(0, false)]
public partial class StormGargoyle : BaseCreature
{
    [Constructible]
    public StormGargoyle() : base(AIType.AI_Mage)
    {
        Body = 4;
        Hue = 0x0491;
        BaseSoundID = 372;

        SetStr(600, 650);
        SetDex(220, 250);
        SetInt(260, 300);

        SetHits(2100, 2350);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "a storm gargoyle's corpse";
    public override string DefaultName => "a storm gargoyle";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
