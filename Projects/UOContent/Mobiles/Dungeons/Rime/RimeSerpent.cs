using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L4 trash. Donor: Ice Serpent.
[SerializationGenerator(0, false)]
public partial class RimeSerpent : BaseCreature
{
    [Constructible]
    public RimeSerpent() : base(AIType.AI_Melee)
    {
        Body = 89;
        Hue = 0x0AF3;
        BaseSoundID = 219;

        SetStr(150, 175);
        SetDex(120, 145);
        SetInt(50, 65);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 70);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 12, 18);
        SetResistance(ResistanceType.Cold, 40, 48);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a glacial serpent's corpse";
    public override string DefaultName => "a glacial serpent";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override Poison HitPoison => Poison.Regular;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
