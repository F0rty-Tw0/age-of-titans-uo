using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian ambient. L3. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class GaianDustadder : BaseCreature
{
    [Constructible]
    public GaianDustadder() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x09C2;
        BaseSoundID = 0xDB;

        SetStr(85, 105);
        SetDex(70, 90);
        SetInt(20, 32);

        SetHits(115, 135);

        SetDamage(6, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 24, 32);
        SetResistance(ResistanceType.Fire, 8, 14);
        SetResistance(ResistanceType.Cold, 8, 14);
        SetResistance(ResistanceType.Poison, 15, 22);

        SetSkill(SkillName.MagicResist, 32.0, 42.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 900;
        Karma = -900;

        VirtualArmor = 26;
    }

    public override string CorpseName => "a dust-adder's corpse";
    public override string DefaultName => "a dust-adder";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison HitPoison => Poison.Lesser;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
