using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 trash. Donor: Great Hart.
[SerializationGenerator(0, false)]
public partial class WyldHart : BaseCreature
{
    [Constructible]
    public WyldHart() : base(AIType.AI_Melee)
    {
        Body = 0xEA;
        Hue = 0x0486;

        SetStr(175, 215);
        SetDex(110, 140);
        SetInt(30, 50);

        SetHits(440, 490);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a silver hart's corpse";
    public override string DefaultName => "a silver hart";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 5;

    public override int GetAttackSound() => 0x82;

    public override int GetHurtSound() => 0x83;

    public override int GetDeathSound() => 0x84;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
