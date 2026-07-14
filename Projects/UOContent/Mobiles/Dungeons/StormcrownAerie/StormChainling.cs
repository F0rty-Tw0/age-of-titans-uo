using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L8 core trash. Donor: Ettin.
[SerializationGenerator(0, false)]
public partial class StormChainling : BaseCreature
{
    [Constructible]
    public StormChainling() : base(AIType.AI_Melee)
    {
        Body = 18;
        Hue = 0x0492;
        BaseSoundID = 367;

        SetStr(320, 360);
        SetDex(100, 130);
        SetInt(60, 90);

        SetHits(820, 920);

        SetDamage(17, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 8500;
        Karma = -8500;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a chained ettin's corpse";
    public override string DefaultName => "a chained ettin";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
