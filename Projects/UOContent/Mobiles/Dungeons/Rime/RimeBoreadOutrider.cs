using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L6 trash. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class RimeBoreadOutrider : BaseCreature
{
    [Constructible]
    public RimeBoreadOutrider() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0B0F;
        BaseSoundID = 402;

        SetStr(300, 340);
        SetDex(170, 200);
        SetInt(70, 90);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Cold, 30);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 46, 54);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a Boread outrider's corpse";
    public override string DefaultName => "a Boread outrider";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 5;

    // North Buffet: 20% chance on a landed hit to buffet the defender and sap stamina.
    // Knockback is a flavor message only - no position change (no knockback primitive in
    // this codebase).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The north wind buffets you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
