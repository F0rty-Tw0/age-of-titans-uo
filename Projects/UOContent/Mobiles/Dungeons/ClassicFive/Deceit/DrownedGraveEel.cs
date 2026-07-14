using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Ambient. L4 trash. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class DrownedGraveEel : BaseCreature
{
    [Constructible]
    public DrownedGraveEel() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0841;
        BaseSoundID = 0xDB;

        SetStr(95, 120);
        SetDex(85, 105);
        SetInt(20, 30);

        SetHits(180, 200);

        SetDamage(9, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 28);
        SetResistance(ResistanceType.Poison, 28, 38);

        SetSkill(SkillName.Poisoning, 48.0, 60.0);
        SetSkill(SkillName.MagicResist, 38.0, 48.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 1200;
        Karma = -1200;

        VirtualArmor = 26;
    }

    public override string CorpseName => "a grave eel's corpse";
    public override string DefaultName => "a grave eel";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool BleedImmune => true;
    public override Poison HitPoison => Poison.Regular;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
