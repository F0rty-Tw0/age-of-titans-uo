using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L4 core. Donor: Troll.
[SerializationGenerator(0, false)]
public partial class GaianDelver : BaseCreature
{
    [Constructible]
    public GaianDelver() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(53, 54);
        Hue = 0x0455;
        BaseSoundID = 461;

        SetStr(170, 195);
        SetDex(55, 75);
        SetInt(30, 48);

        SetHits(195, 215);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 16, 23);
        SetResistance(ResistanceType.Cold, 16, 23);
        SetResistance(ResistanceType.Poison, 18, 26);
        SetResistance(ResistanceType.Energy, 16, 23);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 58.0, 70.0);
        SetSkill(SkillName.Wrestling, 56.0, 68.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a delver's corpse";
    public override string DefaultName => "a gaian delver";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
