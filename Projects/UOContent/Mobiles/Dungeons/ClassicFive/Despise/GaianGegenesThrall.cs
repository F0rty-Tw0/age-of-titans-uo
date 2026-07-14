using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gegenes sub-faction. L4. Donor: Ettin.
[SerializationGenerator(0, false)]
public partial class GaianGegenesThrall : BaseCreature
{
    [Constructible]
    public GaianGegenesThrall() : base(AIType.AI_Melee)
    {
        Body = 18;
        Hue = 0x09C2;
        BaseSoundID = 367;

        SetStr(180, 210);
        SetDex(60, 85);
        SetInt(40, 60);

        SetHits(220, 238);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 47);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 52.0, 65.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 62.0, 74.0);

        Fame = 3050;
        Karma = -3050;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a gegenes thrall's corpse";
    public override string DefaultName => "a gegenes thrall";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
