using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian family. L4 trash. Donor: Ettin.
// Spec calls for a "pack instinct +25% dmg while kin nearby" passive, but BaseCreature.PackInstinct
// has no value fitting the Gaian family (Canine/Ostard/Feline/Arachnid/Daemon/Bear/Equine/Bull only).
// Deviation: bonus skipped rather than inventing new plumbing; plain stat-block reskin.
[SerializationGenerator(0, false)]
public partial class GaianEarthbloodChampion : BaseCreature
{
    [Constructible]
    public GaianEarthbloodChampion() : base(AIType.AI_Melee)
    {
        Body = 18;
        Hue = 0x0972;
        BaseSoundID = 367;

        SetStr(180, 205);
        SetDex(65, 85);
        SetInt(40, 60);

        SetHits(228, 238);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 45);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 52.0, 64.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 62.0, 74.0);

        Fame = 3200;
        Karma = -3200;

        VirtualArmor = 44;
    }

    public override string CorpseName => "an earthblood champion's corpse";
    public override string DefaultName => "a gaian earthblood champion";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
