using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder.md §2) - elite. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class CinderCyclops : DungeonElite
{
    [Constructible]
    public CinderCyclops() : base(AIType.AI_Melee)
    {
        Name = "Brontes";
        Title = "the Last Cyclops";

        Body = 75;
        Hue = 0x0967;
        BaseSoundID = 604;

        SetStr(300, 340);
        SetDex(90, 110);
        SetInt(40, 55);

        SetHits(540, 550);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 58;
    }

    public override string CorpseName => "the Last Cyclops's corpse";

    public override bool BleedImmune => true;

    public override int EliteBagLevel => 6;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Hammerfall: 25% chance to stagger the defender and sap stamina. Knockback is a
        // flavor message only - no position change (no knockback primitive in this codebase).
        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 18;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The hammerfall knocks you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
