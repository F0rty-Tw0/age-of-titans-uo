using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - Kerykes bronze-servant line, L6
// trash. Donor: Gargoyle.
[SerializationGenerator(0, false)]
public partial class CinderKeryxHerald : BaseCreature
{
    [Constructible]
    public CinderKeryxHerald() : base(AIType.AI_Mage)
    {
        Body = 4;
        Hue = 0x0798;
        BaseSoundID = 372;

        SetStr(200, 240);
        SetDex(70, 90);
        SetInt(100, 140);

        SetHits(470, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 70.0, 85.0);
        SetSkill(SkillName.Magery, 70.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 44;
    }

    public override string CorpseName => "the keryx herald's corpse";
    public override string DefaultName => "the keryx herald";

    public override bool CanFly => true;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
