using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder.md §1) - L5 trash. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class TideMaw : BaseCreature
{
    // Chilling Wake aura pulse timing; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public TideMaw() : base(AIType.AI_Melee)
    {
        Body = 16;
        Hue = 0x0530;
        BaseSoundID = 278;

        SetStr(150, 180);
        SetDex(60, 80);
        SetInt(40, 55);

        SetHits(280, 340);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 40, 50);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 38;

        CanSwim = true;
    }

    public override string CorpseName => "a frothing corpse";
    public override string DefaultName => "a whirling maw";

    public override int LootBagLevel => 4;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Riptide: 25% chance to drag the defender under and sap stamina. Knockback is a
        // flavor message only - no position change (no knockback primitive in this codebase).
        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The riptide sweeps you off balance!");
        }
    }

    public override void OnThink()
    {
        base.OnThink();

        // Chilling Wake: 2 cold damage/tick to adjacent players and pets.
        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 2);
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
