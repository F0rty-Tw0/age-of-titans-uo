using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L6 core. Donor: LavaSerpent.
[SerializationGenerator(0, false)]
public partial class DrakonLavaAdder : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath }; // "Lava Breath"

    [Constructible]
    public DrakonLavaAdder() : base(AIType.AI_Melee)
    {
        Body = 90;
        Hue = 0x0489;
        BaseSoundID = 219;

        SetStr(420, 450);
        SetDex(100, 120);
        SetInt(85, 105);

        SetHits(470, 490);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 55);
        SetDamageType(ResistanceType.Fire, 45);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 65, 80);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 70.0, 85.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 70.0, 85.0);

        Fame = 4300;
        Karma = -4300;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a lava adder's corpse";
    public override string DefaultName => "a lava adder";

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
