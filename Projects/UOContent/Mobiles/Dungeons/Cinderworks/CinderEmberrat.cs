using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - ambient fodder. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class CinderEmberrat : BaseCreature
{
    [Constructible]
    public CinderEmberrat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0655;
        BaseSoundID = 0x188;

        SetStr(130, 160);
        SetDex(60, 80);
        SetInt(20, 35);

        SetHits(260, 290);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 25, 35);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "an ember rat's corpse";
    public override string DefaultName => "an ember rat";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
