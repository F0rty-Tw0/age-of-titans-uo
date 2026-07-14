using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L7 trash. Donor: Wraith.
[SerializationGenerator(0, false)]
public partial class CursedLampadWailer : BaseCreature
{
    [Constructible]
    public CursedLampadWailer() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0455;
        BaseSoundID = 0x482;

        SetStr(240, 270);
        SetDex(200, 225);
        SetInt(300, 330);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 40, 48);
        SetResistance(ResistanceType.Poison, 35, 42);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.EvalInt, 80.0, 90.0);
        SetSkill(SkillName.Magery, 80.0, 90.0);
        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a wraith's fading form";
    public override string DefaultName => "a Lampad wailer";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    // Hex-Drain: 20% chance on a landed hit to drain mana and taunt the defender.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 14;
            defender.PublicOverheadMessage(MessageType.Regular, 0x0455, true, "The Lampad wailer drains your will!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
