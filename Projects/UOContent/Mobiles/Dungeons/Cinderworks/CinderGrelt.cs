using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L6 trash. Donor: Stone Gargoyle.
[SerializationGenerator(0, false)]
public partial class CinderGrelt : BaseCreature
{
    [Constructible]
    public CinderGrelt() : base(AIType.AI_Melee)
    {
        Body = 67;
        Hue = 0x0966;
        BaseSoundID = 0x174;

        SetStr(220, 260);
        SetDex(60, 80);
        SetInt(40, 60);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a slag grotesque's corpse";
    public override string DefaultName => "a slag grotesque";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
