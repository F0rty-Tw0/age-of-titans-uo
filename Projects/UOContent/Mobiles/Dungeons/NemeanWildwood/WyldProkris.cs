using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - mini-boss, between Atalanta (elite)
// and Elaphos (boss). Donor: Grizzly Bear.
[SerializationGenerator(0, false)]
public partial class WyldProkris : DungeonElite
{
    [Constructible]
    public WyldProkris() : base(AIType.AI_Melee)
    {
        Name = "Prokris";
        Title = "the Unerring";

        Body = 212;
        Hue = 0x0486;
        BaseSoundID = 0xA3;

        SetStr(320, 350);
        SetDex(160, 190);
        SetInt(60, 80);

        SetHits(710, 720);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 80.0, 90.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "an unerring huntress's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool BleedImmune => true;

    public override int EliteBagLevel => 7;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Never Misses: 25% chance to strike true and sap stamina. Knockback is a flavor
        // message only - no position change (no knockback primitive in this codebase).
        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Prokris's blow strikes true, knocking you back!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
