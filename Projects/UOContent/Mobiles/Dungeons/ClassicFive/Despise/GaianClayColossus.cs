using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L5 core. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class GaianClayColossus : BaseCreature
{
    [Constructible]
    public GaianClayColossus() : base(AIType.AI_Melee)
    {
        Body = 75;
        Hue = 0x0972;
        BaseSoundID = 604;

        SetStr(250, 280);
        SetDex(45, 65);
        SetInt(35, 55);

        SetHits(320, 350);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 53);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 3900;
        Karma = -3900;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a clay colossus's corpse";
    public override string DefaultName => "a clay colossus";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
