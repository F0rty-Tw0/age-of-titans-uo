using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L4 ambient. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class RimeVole : BaseCreature
{
    [Constructible]
    public RimeVole() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x047E;
        BaseSoundID = 0x188;

        SetStr(100, 125);
        SetDex(80, 100);
        SetInt(30, 45);

        SetHits(190, 220);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 26, 34);
        SetResistance(ResistanceType.Fire, 10, 16);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 32.0, 42.0);
        SetSkill(SkillName.Tactics, 48.0, 58.0);
        SetSkill(SkillName.Wrestling, 48.0, 58.0);

        Fame = 2800;
        Karma = -2800;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a snow vole's corpse";
    public override string DefaultName => "a snow vole";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
