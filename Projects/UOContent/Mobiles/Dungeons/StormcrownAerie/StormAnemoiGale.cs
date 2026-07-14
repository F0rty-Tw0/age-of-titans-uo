using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L9. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class StormAnemoiGale : BaseCreature
{
    [Constructible]
    public StormAnemoiGale() : base(AIType.AI_Melee)
    {
        Body = 13;
        Hue = 0x0492;
        BaseSoundID = 655;

        SetStr(620, 690);
        SetDex(200, 230);
        SetInt(150, 180);

        SetHits(2200, 2400);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 47, 57);
        SetResistance(ResistanceType.Fire, 32, 42);
        SetResistance(ResistanceType.Cold, 42, 52);
        SetResistance(ResistanceType.Energy, 62, 72);

        SetSkill(SkillName.MagicResist, 92.0, 102.0);
        SetSkill(SkillName.Tactics, 92.0, 102.0);
        SetSkill(SkillName.Wrestling, 92.0, 102.0);

        Fame = 17500;
        Karma = -17500;

        VirtualArmor = 66;
    }

    public override string CorpseName => "a howling gale's corpse";
    public override string DefaultName => "a howling gale";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 8;

    // Windward Recoil: 15% chance on being hit to reflect a share of the blow back as lightning.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            attacker.Damage(damage / 4, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The gale reflects your blow as lightning!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
