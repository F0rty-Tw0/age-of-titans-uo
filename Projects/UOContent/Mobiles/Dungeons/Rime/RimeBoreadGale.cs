using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L7 trash. Donor: Ice Fiend.
[SerializationGenerator(0, false)]
public partial class RimeBoreadGale : BaseCreature
{
    [Constructible]
    public RimeBoreadGale() : base(AIType.AI_Mage)
    {
        Body = 43;
        Hue = 0x0485;
        BaseSoundID = 357;

        SetStr(310, 350);
        SetDex(160, 190);
        SetInt(300, 340);

        SetHits(610, 670);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 80);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 60, 70);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 34, 42);

        SetSkill(SkillName.EvalInt, 85.0, 95.0);
        SetSkill(SkillName.Magery, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 9600;
        Karma = -9600;

        VirtualArmor = 54;
    }

    public override string CorpseName => "the Boread gale-caller's corpse";
    public override string DefaultName => "the Boread gale-caller";

    public override bool CanFly => true;

    public override int LootBagLevel => 6;

    // Whiteout: 20% chance on a landed hit to drain mana and blind the defender in a squall.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 14;
            defender.PublicOverheadMessage(MessageType.Regular, 0x0485, true, "The gale-caller conjures a blinding whiteout!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
