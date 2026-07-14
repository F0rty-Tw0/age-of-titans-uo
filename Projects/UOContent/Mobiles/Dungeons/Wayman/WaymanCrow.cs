using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - ambient/fodder. L3. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class WaymanCrow : BaseCreature
{
    [Constructible]
    public WaymanCrow() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0000;
        BaseSoundID = 422;

        SetStr(90, 110);
        SetDex(110, 135);
        SetInt(30, 45);

        SetHits(130, 155);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 26, 34);
        SetResistance(ResistanceType.Fire, 14, 22);
        SetResistance(ResistanceType.Cold, 12, 20);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 12, 20);

        SetSkill(SkillName.MagicResist, 38.0, 48.0);
        SetSkill(SkillName.Tactics, 48.0, 60.0);
        SetSkill(SkillName.Wrestling, 48.0, 60.0);

        Fame = 1250;
        Karma = -1250;

        VirtualArmor = 29;
    }

    public override string CorpseName => "a gallows crow's corpse";
    public override string DefaultName => "a gallows crow";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
