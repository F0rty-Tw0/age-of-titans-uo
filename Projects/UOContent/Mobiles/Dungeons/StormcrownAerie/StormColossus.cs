using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L9 core trash. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class StormColossus : BaseCreature
{
    [Constructible]
    public StormColossus() : base(AIType.AI_Melee)
    {
        Body = 75;
        Hue = 0x0492;
        BaseSoundID = 604;

        SetStr(700, 780);
        SetDex(120, 150);
        SetInt(60, 90);

        SetHits(2200, 2400);

        SetDamage(20, 26);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.MagicResist, 95.0, 100.0);
        SetSkill(SkillName.Tactics, 95.0, 100.0);
        SetSkill(SkillName.Wrestling, 95.0, 100.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "a chained colossus's corpse";
    public override string DefaultName => "a chained colossus";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
