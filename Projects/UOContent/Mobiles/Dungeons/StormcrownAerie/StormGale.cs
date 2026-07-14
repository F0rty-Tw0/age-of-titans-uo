using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L8 core trash. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class StormGale : BaseCreature
{
    [Constructible]
    public StormGale() : base(AIType.AI_Melee)
    {
        Body = 13;
        Hue = 0x0481;
        BaseSoundID = 655;

        SetStr(480, 540);
        SetDex(190, 220);
        SetInt(120, 150);

        SetHits(800, 900);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a gale spirit's corpse";
    public override string DefaultName => "a gale spirit";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
