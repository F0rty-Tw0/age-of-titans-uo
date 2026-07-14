using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore mini-boss, L4. Donor: Scorpion.
// Location: the Trinsic coast reef [1285,3731] (APPROX COORDS - verify sand tiles in-game).
[SerializationGenerator(0, false)]
public partial class ShoreKarkinos : DungeonElite
{
    [Constructible]
    public ShoreKarkinos() : base(AIType.AI_Melee)
    {
        Body = 48;
        Hue = 0x0851;
        BaseSoundID = 397;

        SetStr(175, 195);
        SetDex(88, 106);
        SetInt(45, 62);

        SetHits(235, 240);

        SetDamage(13, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 46);
        SetResistance(ResistanceType.Fire, 18, 26);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 24, 32);
        SetResistance(ResistanceType.Energy, 18, 24);

        SetSkill(SkillName.MagicResist, 62.0, 72.0);
        SetSkill(SkillName.Tactics, 72.0, 86.0);
        SetSkill(SkillName.Wrestling, 72.0, 86.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 44;
    }

    public override string CorpseName => "the Karkinos's corpse";
    public override string DefaultName => "the Karkinos";

    public override bool ClickTitle => false;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;

    public override int EliteBagLevel => 4;

    // Crushing Claw: 25% chance to stagger the defender and sap stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 14;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The Karkinos's crushing claw knocks you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
