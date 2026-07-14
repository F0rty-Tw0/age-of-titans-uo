using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Ambient. L4 trash. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class DrownedBrinerot : BaseCreature
{
    [Constructible]
    public DrownedBrinerot() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0830;
        BaseSoundID = 456;

        SetStr(130, 155);
        SetDex(30, 42);
        SetInt(35, 48);

        SetHits(185, 205);

        SetDamage(9, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 24, 32);
        SetResistance(ResistanceType.Poison, 35, 45);

        SetSkill(SkillName.Poisoning, 55.0, 68.0);
        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 48.0, 58.0);
        SetSkill(SkillName.Wrestling, 48.0, 58.0);

        Fame = 1250;
        Karma = -1250;

        VirtualArmor = 28;
    }

    public override string CorpseName => "a mass of brine-rot";
    public override string DefaultName => "a mass of brine-rot";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
