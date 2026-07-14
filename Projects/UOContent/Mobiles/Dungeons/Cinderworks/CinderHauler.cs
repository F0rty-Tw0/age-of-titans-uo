using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L5 trash. Donor: Troll.
[SerializationGenerator(0, false)]
public partial class CinderHauler : BaseCreature
{
    [Constructible]
    public CinderHauler() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(53, 54);
        Hue = 0x0798;
        BaseSoundID = 461;

        SetStr(190, 220);
        SetDex(60, 80);
        SetInt(30, 50);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a bronze hauler's corpse";
    public override string DefaultName => "a bronze hauler";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
