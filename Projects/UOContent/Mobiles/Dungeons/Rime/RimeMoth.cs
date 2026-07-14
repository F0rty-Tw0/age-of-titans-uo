using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L4 ambient. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class RimeMoth : BaseCreature
{
    [Constructible]
    public RimeMoth() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0B0F;
        BaseSoundID = 422;

        SetStr(110, 135);
        SetDex(130, 155);
        SetInt(40, 55);

        SetHits(190, 220);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 36);
        SetResistance(ResistanceType.Fire, 10, 16);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 36.0, 46.0);
        SetSkill(SkillName.Tactics, 52.0, 62.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 2800;
        Karma = -2800;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a frost moth's corpse";
    public override string DefaultName => "a frost moth";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
