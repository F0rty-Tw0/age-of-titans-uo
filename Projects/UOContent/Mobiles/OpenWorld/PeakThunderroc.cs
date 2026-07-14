using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks, L5. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class PeakThunderroc : BaseCreature
{
    [Constructible]
    public PeakThunderroc() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0492;
        BaseSoundID = 402;

        SetStr(228, 268);
        SetDex(96, 118);
        SetInt(50, 72);

        SetHits(320, 360);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 24, 32);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 82.0, 96.0);
        SetSkill(SkillName.Wrestling, 82.0, 96.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a roc corpse";
    public override string DefaultName => "a thunder roc";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int GetAttackSound() => 916;
    public override int GetAngerSound() => 916;
    public override int GetDeathSound() => 917;
    public override int GetHurtSound() => 919;
    public override int GetIdleSound() => 918;

    // Downdraft: 20% chance to buffet the defender and sap stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The thunder roc's downdraft knocks you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
