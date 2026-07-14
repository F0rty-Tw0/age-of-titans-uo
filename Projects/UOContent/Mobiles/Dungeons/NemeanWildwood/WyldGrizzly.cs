using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Grizzly Bear.
[SerializationGenerator(0, false)]
public partial class WyldGrizzly : BaseCreature
{
    [Constructible]
    public WyldGrizzly() : base(AIType.AI_Melee)
    {
        Body = 212;
        Hue = 0x0483;
        BaseSoundID = 0xA3;

        SetStr(240, 280);
        SetDex(110, 140);
        SetInt(35, 55);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 60;
    }

    public override string CorpseName => "a moon-marked bear's corpse";
    public override string DefaultName => "a moon-marked bear";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
