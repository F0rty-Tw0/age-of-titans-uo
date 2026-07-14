using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon. L7 trash. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class PyrePyromancer : BaseCreature
{
    [Constructible]
    public PyrePyromancer() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0026;
        BaseSoundID = 0x3E9;

        SetStr(260, 300);
        SetDex(150, 180);
        SetInt(340, 380);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 60);
        SetDamageType(ResistanceType.Energy, 20);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 60, 70);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 56;
    }

    public override string CorpseName => "an unburnt pyromancer's corpse";
    public override string DefaultName => "an unburnt pyromancer";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Greater;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 6;

    // Scald: 20% chance on a landed hit to sear the defender's focus.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The pyromancer's scald sears your focus!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
