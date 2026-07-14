using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - Kerykes bronze-servant line, L6
// trash. Donor: Ettin.
[SerializationGenerator(0, false)]
public partial class CinderKeryxSapper : BaseCreature
{
    [Constructible]
    public CinderKeryxSapper() : base(AIType.AI_Melee)
    {
        Body = 18;
        Hue = 0x0798;
        BaseSoundID = 367;

        SetStr(260, 300);
        SetDex(60, 80);
        SetInt(40, 60);

        SetHits(500, 550);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a keryx sapper's corpse";
    public override string DefaultName => "a keryx sapper";

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
