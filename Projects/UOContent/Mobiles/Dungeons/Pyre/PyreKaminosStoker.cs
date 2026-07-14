using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L5 trash. Donor: Ogre.
[SerializationGenerator(0, false)]
public partial class PyreKaminosStoker : BaseCreature
{
    [Constructible]
    public PyreKaminosStoker() : base(AIType.AI_Melee)
    {
        Body = 1;
        Hue = 0x0021;
        BaseSoundID = 427;

        SetStr(210, 240);
        SetDex(55, 70);
        SetInt(35, 50);

        SetHits(320, 360);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 38, 48);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 18, 26);

        SetSkill(SkillName.MagicResist, 52.0, 62.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a Kaminoi stoker's corpse";
    public override string DefaultName => "a Kaminoi stoker";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
