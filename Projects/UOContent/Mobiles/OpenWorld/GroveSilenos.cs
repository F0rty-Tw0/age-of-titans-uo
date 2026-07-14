using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion mini-boss, L3. Donor: Satyr.
// Location: the piping-glade NW of Britain [1666,1290]. Respawn 30-60 min.
[SerializationGenerator(0, false)]
public partial class GroveSilenos : DungeonElite
{
    [Constructible]
    public GroveSilenos() : base(AIType.AI_Melee)
    {
        Body = 271;
        Hue = 0x0491;
        BaseSoundID = 0x586;

        SetStr(108, 135);
        SetDex(78, 100);
        SetInt(30, 50);

        SetHits(155, 160);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "Silenos's corpse";
    public override string DefaultName => "Silenos, the Reveler-King";

    public override bool ClickTitle => false;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int EliteBagLevel => 3;

    // Maddening Pipes: 25% chance to stagger the defender and sap stamina. Knockback is a
    // flavor message only - no position change (no knockback primitive in this codebase).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Silenos's maddening pipes knock you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
