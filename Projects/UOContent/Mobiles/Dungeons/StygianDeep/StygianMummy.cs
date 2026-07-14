using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: Mummy (body 154).
[SerializationGenerator(0, false)]
public partial class StygianMummy : BaseCreature
{
    [Constructible]
    public StygianMummy() : base(AIType.AI_Melee)
    {
        Body = 154;
        Hue = 0x08A5;
        BaseSoundID = 471;

        SetStr(420, 460);
        SetDex(80, 110);
        SetInt(50, 80);

        SetHits(2100, 2350);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 95.0, 105.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 20000;
        Karma = -20000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "a bound mummy's corpse";
    public override string DefaultName => "a bound mummy";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
