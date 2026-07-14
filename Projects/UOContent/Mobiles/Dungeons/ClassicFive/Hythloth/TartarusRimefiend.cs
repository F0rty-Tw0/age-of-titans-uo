using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash.
// Donor: Ice Fiend.
[SerializationGenerator(0, false)]
public partial class TartarusRimefiend : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };

    [Constructible]
    public TartarusRimefiend() : base(AIType.AI_Mage)
    {
        Body = 43;
        Hue = 0x0455;
        BaseSoundID = 357;

        SetStr(565, 615);
        SetDex(222, 252);
        SetInt(262, 292);

        SetHits(890, 910);

        SetDamage(21, 26);

        SetResistance(ResistanceType.Physical, 61, 71);
        SetResistance(ResistanceType.Fire, 21, 31);
        SetResistance(ResistanceType.Cold, 66, 76);
        SetResistance(ResistanceType.Poison, 31, 41);
        SetResistance(ResistanceType.Energy, 36, 46);

        SetSkill(SkillName.EvalInt, 91.0, 101.0);
        SetSkill(SkillName.Magery, 91.0, 101.0);
        SetSkill(SkillName.MagicResist, 96.0, 111.0);
        SetSkill(SkillName.Tactics, 91.0, 101.0);
        SetSkill(SkillName.Wrestling, 91.0, 106.0);

        Fame = 14100;
        Karma = -14100;

        VirtualArmor = 63;
    }

    public override string CorpseName => "a rime-fiend's corpse";
    public override string DefaultName => "a rime-fiend";

    public override bool CanFly => true;

    public override int LootBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
