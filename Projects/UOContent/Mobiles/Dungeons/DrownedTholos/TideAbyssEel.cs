using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L5 core family. Donor: Deep Sea
// Serpent.
[SerializationGenerator(0, false)]
public partial class TideAbyssEel : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath }; // "Brine Breath"

    [Constructible]
    public TideAbyssEel() : base(AIType.AI_Melee)
    {
        Body = 150;
        Hue = 0x0530;
        BaseSoundID = 447;

        SetStr(180, 210);
        SetDex(65, 85);
        SetInt(35, 50);

        SetHits(320, 380);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 80);
        SetDamageType(ResistanceType.Cold, 20);

        SetResistance(ResistanceType.Physical, 34, 44);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 25, 35);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 1550;
        Karma = -1550;

        VirtualArmor = 42;

        CanSwim = true;
    }

    public override string CorpseName => "a deep serpent's corpse";
    public override string DefaultName => "a deep serpent";

    public override int LootBagLevel => 4;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
