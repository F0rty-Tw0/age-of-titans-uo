using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L6 trash. Donor: Fire Gargoyle.
[SerializationGenerator(0, false)]
public partial class PyreKaminosBrand : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public PyreKaminosBrand() : base(AIType.AI_Melee)
    {
        Body = 130;
        Hue = 0x0669;
        BaseSoundID = 0x174;

        SetStr(220, 250);
        SetDex(150, 175);
        SetInt(140, 170);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Fire, 60);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 26, 34);
        SetResistance(ResistanceType.Energy, 26, 34);

        SetSkill(SkillName.EvalInt, 60.0, 70.0);
        SetSkill(SkillName.Magery, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 62.0, 72.0);
        SetSkill(SkillName.Tactics, 72.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a Kaminoi brand-bearer's corpse";
    public override string DefaultName => "a Kaminoi brand-bearer";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
