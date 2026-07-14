using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash.
// Donor: Stone Gargoyle.
[SerializationGenerator(0, false)]
public partial class TartarusStonewrath : BaseCreature
{
    [Constructible]
    public TartarusStonewrath() : base(AIType.AI_Melee)
    {
        Body = 67;
        Hue = 0x0455;
        BaseSoundID = 0x174;

        SetStr(550, 600);
        SetDex(160, 190);
        SetInt(180, 210);

        SetHits(875, 885);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 95.0, 110.0);
        SetSkill(SkillName.Tactics, 90.0, 105.0);
        SetSkill(SkillName.Wrestling, 85.0, 100.0);

        Fame = 13500;
        Karma = -13500;

        VirtualArmor = 65;
    }

    public override string CorpseName => "a stonewrath's corpse";
    public override string DefaultName => "a stonewrath";

    public override Poison PoisonImmune => Poison.Lethal;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 7;

    // Stone skin: 15% chance to reflect ~20% of the incoming blow back at the attacker.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            attacker.Damage(damage * 20 / 100, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The stone skin throws your blow back at you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
