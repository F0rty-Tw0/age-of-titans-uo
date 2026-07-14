using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon. L6 trash. Donor: Lava Serpent.
[SerializationGenerator(0, false)]
public partial class PyreSerpent : BaseCreature
{
    [Constructible]
    public PyreSerpent() : base(AIType.AI_Melee)
    {
        Body = 90;
        Hue = 0x0669;
        BaseSoundID = 219;

        SetStr(240, 270);
        SetDex(120, 145);
        SetInt(60, 85);

        SetHits(480, 530);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 72.0, 82.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a molten serpent's corpse";
    public override string DefaultName => "a molten serpent";

    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
