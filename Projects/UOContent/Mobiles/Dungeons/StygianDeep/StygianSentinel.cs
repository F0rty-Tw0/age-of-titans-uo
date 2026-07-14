using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Stygian Deep trash + boss-room custodian (dev-docs/dungeon-ladder.md §5).
// Donor: Fire Elemental (body 15), recolored and re-breathed to Cocytus cold.
[SerializationGenerator(0, false)]
public partial class StygianSentinel : BaseCreature
{
    [Constructible]
    public StygianSentinel() : base(AIType.AI_Melee)
    {
        Body = 15;
        Hue = 0x0454;
        BaseSoundID = 838;

        SetStr(400, 450);
        SetDex(150, 170);
        SetInt(150, 190);

        SetHits(2600, 2900);

        SetDamage(22, 28);

        SetDamageType(ResistanceType.Physical, 25);
        SetDamageType(ResistanceType.Cold, 75);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 65, 75);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 100.0, 110.0);
        SetSkill(SkillName.Wrestling, 95.0, 105.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 70;

        AddItem(new LightSource());
    }

    public override string CorpseName => "a stygian sentinel's corpse";
    public override string DefaultName => "a stygian sentinel";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 9;

    private static MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
