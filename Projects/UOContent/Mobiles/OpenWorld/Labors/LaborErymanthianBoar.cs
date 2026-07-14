using ModernUO.Serialization;

namespace Server.Mobiles;

// The Labors (dev-docs/open-world-bestiary.md §6) - roaming world-hunt, L7. Donor: Boar.
[SerializationGenerator(0, false)]
public partial class LaborErymanthianBoar : DungeonElite
{
    [Constructible]
    public LaborErymanthianBoar() : base(AIType.AI_Melee)
    {
        Body = 0x122;
        Hue = 0x0481;
        BaseSoundID = 0xC4;

        SetStr(480, 540);
        SetDex(170, 200);
        SetInt(20, 35);

        SetHits(690, 720);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 60, 68);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 28, 36);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 28, 36);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 92.0, 105.0);
        SetSkill(SkillName.Wrestling, 90.0, 102.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 66;
    }

    public override string CorpseName => "the Erymanthian Boar's corpse";
    public override string DefaultName => "the Erymanthian Boar";

    public override bool ClickTitle => false;

    public override int EliteBagLevel => 7;

    // Goring Charge: 25% chance to stagger the defender and sap stamina. Knockback is a flavor
    // message only - no position change (no knockback primitive in this codebase).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The boar's charge gores you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
