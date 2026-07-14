using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband (dev-docs/gap-families-bestiary.md §6.6) - the first elite draw for the
// Orc Caves. Donor: Orc Brute.
[SerializationGenerator(0, false)]
public partial class LykaiNyktimos : DungeonElite
{
    [Constructible]
    public LykaiNyktimos() : base(AIType.AI_Melee)
    {
        Name = "Nyktimos";
        Title = "the Wolf-Crowned";

        Body = 189;
        Hue = 0x0483;
        BaseSoundID = 0x45A;

        SetStr(340, 370);
        SetDex(140, 160);
        SetInt(50, 65);

        SetHits(540, 550);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 68.0, 80.0);
        SetSkill(SkillName.Tactics, 80.0, 92.0);
        SetSkill(SkillName.Wrestling, 80.0, 92.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 58;
    }

    public override string CorpseName => "the Wolf-Crowned's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;
    public override bool BleedImmune => true;

    public override int EliteBagLevel => 6;

    // Rend and Run: 25% chance to bowl a target over for stamina on a successful hit.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Nyktimos bowls you off your feet!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
