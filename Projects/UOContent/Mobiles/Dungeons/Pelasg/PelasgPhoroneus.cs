using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9) - Painted Caves elite. Donor: Troglodyte.
[SerializationGenerator(0, false)]
public partial class PelasgPhoroneus : DungeonElite
{
    [Constructible]
    public PelasgPhoroneus() : base(AIType.AI_Melee)
    {
        Name = "Phoroneus";
        Title = "the First-Cut";

        Body = 267;
        Hue = 0x0967;
        BaseSoundID = 0x59F;

        SetStr(200, 225);
        SetDex(120, 145);
        SetInt(60, 80);

        SetHits(235, 240);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 25, 33);
        SetResistance(ResistanceType.Poison, 25, 33);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 62.0, 75.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 44;
    }

    public override string CorpseName => "Phoroneus's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool BleedImmune => true;
    public override bool CanHeal => true;

    public override int EliteBagLevel => 4;

    // First Fire: 25% chance to knock the defender back and sap stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Phoroneus's first fire sears you back!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
