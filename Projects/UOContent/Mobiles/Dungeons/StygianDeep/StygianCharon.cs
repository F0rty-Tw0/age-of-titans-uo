using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep elite (dev-docs/dungeon-ladder.md §5). Callback: reuses the barrow
// NewbieCharon's Headless One body and deep-shadow hue, grown into the deep's ferryman.
[SerializationGenerator(0, false)]
public partial class StygianCharon : DungeonElite
{
    [Constructible]
    public StygianCharon() : base(AIType.AI_Mage)
    {
        Name = "Charon";
        Title = "Ferryman of the Deep";

        Body = 31;
        Hue = 0x0455;
        BaseSoundID = 0x39D;

        SetStr(500, 550);
        SetDex(140, 160);
        SetInt(400, 450);

        SetHits(2800, 3000);

        SetDamage(24, 30);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 50, 60);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 50, 60);

        SetSkill(SkillName.EvalInt, 105.0, 115.0);
        SetSkill(SkillName.Magery, 105.0, 115.0);
        SetSkill(SkillName.MagicResist, 105.0, 120.0);
        SetSkill(SkillName.Tactics, 100.0, 110.0);
        SetSkill(SkillName.Wrestling, 95.0, 105.0);

        Fame = 35000;
        Karma = -35000;

        VirtualArmor = 75;
    }

    public override string CorpseName => "a headless corpse";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 9;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }

    // Ferryman's Toll: 25% on a landed hit, drain mana and shove the defender back a step.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 20;
            defender.PublicOverheadMessage(MessageType.Regular, 0x480, true, "Charon demands his toll!");
        }
    }
}
