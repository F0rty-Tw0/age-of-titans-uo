using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L5 trash. Donor: Skeletal Knight.
[SerializationGenerator(0, false)]
public partial class RimeReaver : BaseCreature
{
    [Constructible]
    public RimeReaver() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x0485;
        BaseSoundID = 451;

        SetStr(210, 240);
        SetDex(120, 145);
        SetInt(50, 70);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Cold, 30);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 38, 46);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a rime-bound reaver's corpse";
    public override string DefaultName => "a rime-bound reaver";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
