using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L9 core trash. Donor: Ogre Lord.
[SerializationGenerator(0, false)]
public partial class StormOgreKing : BaseCreature
{
    [Constructible]
    public StormOgreKing() : base(AIType.AI_Melee)
    {
        Body = 83;
        Hue = 0x0492;
        BaseSoundID = 427;

        SetStr(750, 820);
        SetDex(140, 170);
        SetInt(70, 100);

        SetHits(2250, 2400);

        SetDamage(21, 26);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 58, 68);
        SetResistance(ResistanceType.Fire, 38, 48);
        SetResistance(ResistanceType.Cold, 38, 48);
        SetResistance(ResistanceType.Poison, 42, 52);
        SetResistance(ResistanceType.Energy, 48, 58);

        SetSkill(SkillName.MagicResist, 100.0, 110.0);
        SetSkill(SkillName.Tactics, 95.0, 105.0);
        SetSkill(SkillName.Wrestling, 95.0, 105.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "a storm-forged ogre lord's corpse";
    public override string DefaultName => "a storm-forged ogre lord";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 8;

    // Gale Buffet: 20% chance on a landed hit to buffet the defender and sap stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The gale buffets you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
