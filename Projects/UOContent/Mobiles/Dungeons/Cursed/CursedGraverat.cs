using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L5 fodder. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class CursedGraverat : BaseCreature
{
    [Constructible]
    public CursedGraverat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0842;
        BaseSoundID = 0x188;

        SetStr(150, 175);
        SetDex(90, 110);
        SetInt(30, 45);

        SetHits(280, 310);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 22, 28);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 35.0, 45.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 3200;
        Karma = -3200;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a rat's gnawed carcass";
    public override string DefaultName => "a dig-site rat";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
