using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L7 trash. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class PyreKaminosAnvilguard : BaseCreature
{
    [Constructible]
    public PyreKaminosAnvilguard() : base(AIType.AI_Melee)
    {
        Body = 9;
        Hue = 0x0669;
        BaseSoundID = 357;

        SetStr(330, 370);
        SetDex(150, 175);
        SetInt(170, 200);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 52, 62);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 32, 42);

        SetSkill(SkillName.EvalInt, 68.0, 78.0);
        SetSkill(SkillName.Magery, 68.0, 78.0);
        SetSkill(SkillName.MagicResist, 80.0, 90.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 72.0, 82.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a Kaminoi anvil-guard's corpse";
    public override string DefaultName => "a Kaminoi anvil-guard";

    public override bool CanFly => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
