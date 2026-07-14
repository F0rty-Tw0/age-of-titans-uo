using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L4 ambient. Donor: Eagle.
[SerializationGenerator(0, false)]
public partial class RimeSparrow : BaseCreature
{
    [Constructible]
    public RimeSparrow() : base(AIType.AI_Melee)
    {
        Body = 5;
        Hue = 0x0AF3;
        BaseSoundID = 0x2EE;

        SetStr(110, 135);
        SetDex(140, 165);
        SetInt(30, 45);

        SetHits(190, 220);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 36);
        SetResistance(ResistanceType.Fire, 10, 16);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 35.0, 45.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 48.0, 58.0);

        Fame = 2800;
        Karma = -2800;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a frost sparrow's corpse";
    public override string DefaultName => "a frost sparrow";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override bool CanFly => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
