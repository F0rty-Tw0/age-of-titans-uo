using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gegenes sub-faction. L5. Donor: OgreLord.
// Spec calls for a "pack instinct" bonus, but BaseCreature.PackInstinct has no value fitting the
// Gaian family (Canine/Ostard/Feline/Arachnid/Daemon/Bear/Equine/Bull only) - same deviation as
// GaianEarthbloodChampion. Bonus skipped rather than inventing new plumbing; plain stat-block reskin.
[SerializationGenerator(0, false)]
public partial class GaianGegenesElder : BaseCreature
{
    [Constructible]
    public GaianGegenesElder() : base(AIType.AI_Melee)
    {
        Body = 83;
        Hue = 0x0455;
        BaseSoundID = 427;

        SetStr(265, 295);
        SetDex(55, 75);
        SetInt(45, 65);

        SetHits(350, 370);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 26, 33);
        SetResistance(ResistanceType.Cold, 26, 33);
        SetResistance(ResistanceType.Poison, 26, 33);
        SetResistance(ResistanceType.Energy, 26, 33);

        SetSkill(SkillName.MagicResist, 62.0, 72.0);
        SetSkill(SkillName.Tactics, 74.0, 86.0);
        SetSkill(SkillName.Wrestling, 72.0, 84.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 53;
    }

    public override string CorpseName => "a gegenes elder's corpse";
    public override string DefaultName => "a gegenes elder";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
