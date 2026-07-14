using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion mini-boss, L5. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class PeakTalos : DungeonElite
{
    [Constructible]
    public PeakTalos() : base(AIType.AI_Melee)
    {
        Body = 75;
        Hue = 0x0798;
        BaseSoundID = 604;

        SetStr(300, 340);
        SetDex(130, 160);
        SetInt(48, 68);

        SetHits(375, 380);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 52, 60);
        SetResistance(ResistanceType.Fire, 25, 33);
        SetResistance(ResistanceType.Cold, 24, 32);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 24, 32);

        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 97.0);

        Fame = 9000;
        Karma = -9000;

        VirtualArmor = 56;
    }

    public override string CorpseName => "Talos's shattered corpse";
    public override string DefaultName => "Talos, the Bronze Warden";

    public override bool ClickTitle => false;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 5;

    // Bronze Fist: 25% chance to stagger the defender and sap stamina. Knockback is a flavor
    // message only - no position change (no knockback primitive in this codebase).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Talos's bronze fist knocks you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
