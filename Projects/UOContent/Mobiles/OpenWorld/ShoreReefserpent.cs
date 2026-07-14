using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L4. Donor: Giant Serpent
// (land-walking body, not Sea Serpent - see spec note on CantWalk).
[SerializationGenerator(0, false)]
public partial class ShoreReefserpent : BaseCreature
{
    [Constructible]
    public ShoreReefserpent() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0851;
        BaseSoundID = 219;

        SetStr(160, 188);
        SetDex(86, 106);
        SetInt(42, 60);

        SetHits(205, 238);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 43);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a serpent corpse";
    public override string DefaultName => "a cove serpent";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
