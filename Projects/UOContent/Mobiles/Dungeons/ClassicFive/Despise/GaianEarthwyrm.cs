using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L4 core. Donor: GiantSerpent.
[SerializationGenerator(0, false)]
public partial class GaianEarthwyrm : BaseCreature
{
    [Constructible]
    public GaianEarthwyrm() : base(AIType.AI_Melee)
    {
        Body = 21;
        Hue = 0x09C2;
        BaseSoundID = 219;

        SetStr(150, 175);
        SetDex(55, 75);
        SetInt(35, 50);

        SetHits(195, 215);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 12, 18);
        SetResistance(ResistanceType.Cold, 12, 18);
        SetResistance(ResistanceType.Poison, 55, 70);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 56.0, 66.0);

        Fame = 2000;
        Karma = -2000;

        VirtualArmor = 38;
    }

    public override string CorpseName => "an earth-wyrm's corpse";
    public override string DefaultName => "a gaian earth-wyrm";

    public override Poison PoisonImmune => Poison.Lethal;
    public override Poison HitPoison => Poison.Regular;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
