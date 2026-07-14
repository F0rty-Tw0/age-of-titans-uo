using ModernUO.Serialization;

namespace Server.Mobiles;

// The Labors (dev-docs/open-world-bestiary.md §6) - roaming world-hunt, L8. Donor: Bull.
[SerializationGenerator(0, false)]
public partial class LaborCretanBull : DungeonElite
{
    [Constructible]
    public LaborCretanBull() : base(AIType.AI_Melee)
    {
        Body = 0xE9;
        Hue = 0x0021;
        BaseSoundID = 0x64;

        SetStr(620, 680);
        SetDex(150, 180);
        SetInt(30, 45);

        SetHits(900, 950);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 65, 75);
        SetResistance(ResistanceType.Fire, 32, 40);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 98.0, 112.0);
        SetSkill(SkillName.Wrestling, 95.0, 108.0);

        Fame = 18000;
        Karma = -18000;

        VirtualArmor = 72;
    }

    public override string CorpseName => "the Cretan Bull's corpse";
    public override string DefaultName => "the Cretan Bull";

    public override bool ClickTitle => false;

    public override bool BleedImmune => true;

    public override int EliteBagLevel => 8;

    // Maddened Charge: 25% chance to stagger the defender and sap stamina. Knockback is a
    // flavor message only - no position change (no knockback primitive in this codebase).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 18;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The bull's maddened charge knocks you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
