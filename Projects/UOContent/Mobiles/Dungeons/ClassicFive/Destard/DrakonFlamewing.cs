using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon family. L7 trash. Donor: Drake.
[SerializationGenerator(0, false)]
public partial class DrakonFlamewing : BaseCreature
{
    // Weak fire breath: stock scalar off this skin's trash-tier HP keeps it a minor threat
    // (à la DrakescalePython, not Ladon's boosted LadonBreath).
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public DrakonFlamewing() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(60, 61);
        Hue = 0x0489;
        BaseSoundID = 362;

        SetStr(600, 650);
        SetDex(110, 130);
        SetInt(90, 110);

        SetHits(690, 700);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Fire, 30);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 65, 75);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 80.0, 90.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 9000;
        Karma = -9000;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a drakon flamewing's corpse";
    public override string DefaultName => "a drakon flamewing";

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
