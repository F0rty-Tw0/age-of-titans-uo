using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon. L5 trash. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class PyreCinderrat : BaseCreature
{
    [Constructible]
    public PyreCinderrat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0655;
        BaseSoundID = 0x188;

        SetStr(190, 220);
        SetDex(90, 115);
        SetInt(50, 70);

        SetHits(280, 310);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 20, 25);
        SetResistance(ResistanceType.Poison, 25, 30);
        SetResistance(ResistanceType.Energy, 20, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 46;
    }

    public override string CorpseName => "an ember rat's corpse";
    public override string DefaultName => "a pyre rat";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
