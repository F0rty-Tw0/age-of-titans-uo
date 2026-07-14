using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - Pelagos crew (L5). Donor:
// "Skeletal Knight" per the design doc is body 57 (BoneKnight.cs) - same naming quirk
// established by TideHoplite.cs/TideMarine.cs.
[SerializationGenerator(0, false)]
public partial class TidePelagosCaptain : BaseCreature
{
    [Constructible]
    public TidePelagosCaptain() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x0851;
        BaseSoundID = 451;

        SetStr(190, 220);
        SetDex(55, 70);
        SetInt(30, 45);

        SetHits(340, 375);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 46);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 20, 30);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 1650;
        Karma = -1650;

        VirtualArmor = 44;
    }

    public override string CorpseName => "the captain's corpse";
    public override string DefaultName => "the Pelagos captain";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Broadside: 15% chance to knock the attacker back and sap stamina. Knockback is a
        // flavor message only - no position change (no knockback primitive in this codebase),
        // same idiom as TideHerald.OnGaveMeleeAttack.
        if (Utility.RandomDouble() < 0.15)
        {
            attacker.Stam -= 10;
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The captain's broadside sends you reeling!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
