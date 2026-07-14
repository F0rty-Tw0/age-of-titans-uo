using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L5 trash. Donor: Wraith.
[SerializationGenerator(0, false)]
public partial class RimeWight : BaseCreature
{
    [Constructible]
    public RimeWight() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0AF3;
        BaseSoundID = 0x482;

        SetStr(190, 220);
        SetDex(150, 175);
        SetInt(190, 220);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 70);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.EvalInt, 60.0, 70.0);
        SetSkill(SkillName.Magery, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a frostbound wight's corpse";
    public override string DefaultName => "a frostbound wight";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
