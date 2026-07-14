using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder.md §2) - L5 trash. Donor: Ettin.
[SerializationGenerator(0, false)]
public partial class CinderThrall : BaseCreature
{
    [Constructible]
    public CinderThrall() : base(AIType.AI_Melee)
    {
        Body = 18;
        Hue = 0x0964;
        BaseSoundID = 367;

        SetStr(200, 240);
        SetDex(60, 80);
        SetInt(31, 50);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a slag thrall's corpse";
    public override string DefaultName => "a slag thrall";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Emberbrand: 20% chance to sear the defender's magic away.
        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The ember-brand sears your magic away!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
