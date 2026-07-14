using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L8. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class StormAnemoiOutrider : BaseCreature
{
    [Constructible]
    public StormAnemoiOutrider() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0492;
        BaseSoundID = 402;

        SetStr(300, 340);
        SetDex(160, 190);
        SetInt(80, 110);

        SetHits(820, 900);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 8200;
        Karma = -8200;

        VirtualArmor = 56;
    }

    public override string CorpseName => "an anemoi outrider's corpse";
    public override string DefaultName => "an anemoi outrider";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    // Buffet: 20% chance on a landed hit to knock the defender back and sap stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The outrider's wind buffets you back!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
