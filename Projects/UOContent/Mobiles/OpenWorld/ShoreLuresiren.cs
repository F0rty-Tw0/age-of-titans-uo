using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L4. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class ShoreLuresiren : BaseCreature
{
    [Constructible]
    public ShoreLuresiren() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0491;
        BaseSoundID = 402;

        SetStr(160, 188);
        SetDex(92, 110);
        SetInt(44, 62);

        SetHits(205, 238);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 43);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a harpy corpse";
    public override string DefaultName => "a luring siren";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int GetAttackSound() => 916;
    public override int GetAngerSound() => 916;
    public override int GetDeathSound() => 917;
    public override int GetHurtSound() => 919;
    public override int GetIdleSound() => 918;

    // Siren's Call: 20% chance to lure the defender and sap stamina on a landed swing.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 10;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The siren's call leaves you weak-kneed!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
