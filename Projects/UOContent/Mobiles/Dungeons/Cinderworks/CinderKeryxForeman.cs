using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - Kerykes bronze-servant line, L6
// trash. Donor: Ogre Lord.
[SerializationGenerator(0, false)]
public partial class CinderKeryxForeman : BaseCreature
{
    [Constructible]
    public CinderKeryxForeman() : base(AIType.AI_Melee)
    {
        Body = 83;
        Hue = 0x0798;
        BaseSoundID = 427;

        SetStr(280, 320);
        SetDex(50, 65);
        SetInt(50, 70);

        SetHits(510, 550);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 2450;
        Karma = -2450;

        VirtualArmor = 56;
    }

    public override string CorpseName => "the keryx foreman's corpse";
    public override string DefaultName => "the keryx foreman";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Hammerfall: 25% chance to stagger the defender and sap stamina.
        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The hammerfall knocks you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
