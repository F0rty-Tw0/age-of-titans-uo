using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves, Leleges cave-clan. L4 trash. Donor: Orc Bomber.
[SerializationGenerator(0, false)]
public partial class PelasgLelexTrapmaster : BaseCreature
{
    [Constructible]
    public PelasgLelexTrapmaster() : base(AIType.AI_Melee)
    {
        Body = 182;
        Hue = 0x0844;
        BaseSoundID = 0x45A;

        SetStr(160, 185);
        SetDex(100, 120);
        SetInt(35, 50);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a Leleges trap-master's corpse";
    public override string DefaultName => "a Leleges trap-master";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 3;

    // Deadfall: 15% chance to snag the attacker in a hidden deadfall trap.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            attacker.Stam -= 8;
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "A hidden deadfall snags you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
