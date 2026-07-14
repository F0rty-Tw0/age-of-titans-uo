using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: SkeletalDragon (body 104).
[SerializationGenerator(0, false)]
public partial class StygianBoneColossus : BaseCreature
{
    [Constructible]
    public StygianBoneColossus() : base(AIType.AI_Melee)
    {
        Body = 104;
        Hue = 0x08A5;
        BaseSoundID = 0x488;

        SetStr(480, 530);
        SetDex(90, 120);
        SetInt(50, 80);

        SetHits(2800, 3100);

        SetDamage(24, 30);

        SetDamageType(ResistanceType.Physical, 25);
        SetDamageType(ResistanceType.Cold, 75);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 65, 75);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 105.0, 115.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "a bone colossus's corpse";
    public override string DefaultName => "a bone colossus";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 9;

    private static MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
