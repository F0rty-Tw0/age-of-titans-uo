using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - Pelagos crew (L5). Donor:
// "Skeletal Knight" per the design doc is body 57 (BoneKnight.cs) - same naming quirk
// established by TideHoplite.cs/TideMarine.cs.
[SerializationGenerator(0, false)]
public partial class TidePelagosBosun : BaseCreature
{
    [Constructible]
    public TidePelagosBosun() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(175, 205);
        SetDex(55, 70);
        SetInt(25, 40);

        SetHits(320, 360);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 33, 43);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 57.0, 67.0);
        SetSkill(SkillName.Tactics, 67.0, 77.0);
        SetSkill(SkillName.Wrestling, 67.0, 77.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 41;
    }

    public override string CorpseName => "the bosun's corpse";
    public override string DefaultName => "the Pelagos bosun";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Lash of the Deep: 20% chance to sap stamina.
        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The bosun's lash saps your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
