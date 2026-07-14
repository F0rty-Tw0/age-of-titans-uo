using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3) - Khaldun. L5 trash. Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class CursedDigger : BaseCreature
{
    [Constructible]
    public CursedDigger() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0842;
        BaseSoundID = 471;

        SetStr(220, 250);
        SetDex(90, 110);
        SetInt(60, 80);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a cursed digger's remains";
    public override string DefaultName => "a cursed digger";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
