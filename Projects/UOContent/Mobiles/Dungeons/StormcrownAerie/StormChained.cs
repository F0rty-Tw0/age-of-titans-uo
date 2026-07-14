using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder.md §4) - elite. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class StormChained : DungeonElite
{
    [Constructible]
    public StormChained() : base(AIType.AI_Melee)
    {
        Name = "Enceladus";
        Title = "the Chainbreaker";

        Body = 75;
        Hue = 0x0492;
        BaseSoundID = 604;

        SetStr(780, 850);
        SetDex(140, 170);
        SetInt(80, 110);

        SetHits(2350, 2400);

        SetDamage(22, 28);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 50, 60);

        SetSkill(SkillName.MagicResist, 100.0, 110.0);
        SetSkill(SkillName.Tactics, 100.0, 110.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 22000;
        Karma = -22000;

        VirtualArmor = 72;
    }

    public override string CorpseName => "the Chainbreaker's corpse";

    public override bool BleedImmune => true;

    public override int EliteBagLevel => 9;

    // Titan's Wrath: 15% chance on being hit to reflect a share of the blow back as lightning.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            attacker.Damage(damage * 3 / 10, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The chains lash back with lightning!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
