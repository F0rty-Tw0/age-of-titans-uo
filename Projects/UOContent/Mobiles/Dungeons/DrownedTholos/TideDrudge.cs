using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder.md §1) - L4 trash. Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class TideDrudge : BaseCreature
{
    [Constructible]
    public TideDrudge() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0847;
        BaseSoundID = 471;

        SetStr(80, 100);
        SetDex(40, 55);
        SetInt(20, 30);

        SetHits(120, 160);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 28);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 10, 15);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 900;
        Karma = -900;

        VirtualArmor = 24;
    }

    public override string CorpseName => "a bloated corpse";
    public override string DefaultName => "a drowned oarsman";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 3;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Undertow: 20% chance to drag stamina down with the tide.
        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The undertow drags at your legs!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
