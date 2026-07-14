using ModernUO.Serialization;

namespace Server.Mobiles;

// The Labors (dev-docs/open-world-bestiary.md §6) - roaming world-hunt, L5. Donor: Boar.
[SerializationGenerator(0, false)]
public partial class LaborCalydonianBoar : DungeonElite
{
    [Constructible]
    public LaborCalydonianBoar() : base(AIType.AI_Melee)
    {
        Body = 0x122;
        Hue = 0x021E;
        BaseSoundID = 0xC4;

        SetStr(320, 360);
        SetDex(120, 150);
        SetInt(20, 35);

        SetHits(365, 380);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 25, 33);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 25, 33);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 80.0, 92.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 53;
    }

    public override string CorpseName => "the Calydonian Boar's corpse";
    public override string DefaultName => "the Calydonian Boar";

    public override bool ClickTitle => false;

    public override int EliteBagLevel => 5;

    // Tusk Sweep: 20% chance to stagger the defender and sap stamina. Knockback is a flavor
    // message only - no position change (no knockback primitive in this codebase).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The boar's tusks sweep your legs out!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
