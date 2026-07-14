using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L9 core trash. Donor: White Wyrm.
[SerializationGenerator(0, false)]
public partial class StormWyrm : BaseCreature
{
    // Reuses EnergyBreath (local reskin defined in StormWisp.cs, same namespace).
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Levin Breath"

    [Constructible]
    public StormWyrm() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomBool() ? 180 : 49;
        Hue = 0x0480;
        BaseSoundID = 362;

        SetStr(650, 720);
        SetDex(160, 190);
        SetInt(120, 150);

        SetHits(2150, 2380);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 65, 75);

        // "High energy resist" passive: T2A has no meaningful per-element resist ceiling, so
        // the callout is expressed as a MagicResist skill well above this dungeon's other trash
        // (established idiom - see StormDrake).
        SetSkill(SkillName.MagicResist, 105.0, 115.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "a levin wyrm's corpse";
    public override string DefaultName => "a levin wyrm";

    public override int LootBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
