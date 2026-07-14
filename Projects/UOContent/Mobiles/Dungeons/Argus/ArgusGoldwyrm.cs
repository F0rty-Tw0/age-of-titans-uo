using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L6 trash. Donor: Wyvern.
[SerializationGenerator(0, false)]
public partial class ArgusGoldwyrm : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public ArgusGoldwyrm() : base(AIType.AI_Melee)
    {
        Body = 62;
        Hue = 0x0479;
        BaseSoundID = 362;

        SetStr(310, 340);
        SetDex(140, 165);
        SetInt(70, 95);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a gold-cursed wyrm's corpse";
    public override string DefaultName => "a gold-cursed wyrm";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanFly => true;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
