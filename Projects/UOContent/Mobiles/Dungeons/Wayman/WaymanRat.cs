using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - ambient/fodder. L3. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class WaymanRat : BaseCreature
{
    [Constructible]
    public WaymanRat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0964;
        BaseSoundID = 0x188;

        SetStr(85, 105);
        SetDex(80, 100);
        SetInt(20, 32);

        SetHits(130, 155);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 25, 33);
        SetResistance(ResistanceType.Fire, 10, 18);
        SetResistance(ResistanceType.Cold, 10, 18);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 10, 18);

        SetSkill(SkillName.MagicResist, 32.0, 42.0);
        SetSkill(SkillName.Tactics, 42.0, 55.0);
        SetSkill(SkillName.Wrestling, 42.0, 55.0);

        Fame = 1200;
        Karma = -1200;

        VirtualArmor = 28;
    }

    public override string CorpseName => "a gutter rat's corpse";
    public override string DefaultName => "a gutter rat";

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
