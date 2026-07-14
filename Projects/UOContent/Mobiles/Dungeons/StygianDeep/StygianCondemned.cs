using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: Shade (body 26).
[SerializationGenerator(0, false)]
public partial class StygianCondemned : BaseCreature
{
    [Constructible]
    public StygianCondemned() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0455;
        BaseSoundID = 0x482;

        SetStr(320, 370);
        SetDex(120, 150);
        SetInt(520, 580);

        SetHits(2500, 2800);

        SetDamage(22, 28);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 105.0, 115.0);
        SetSkill(SkillName.Magery, 105.0, 115.0);
        SetSkill(SkillName.MagicResist, 105.0, 120.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "a condemned shade's corpse";
    public override string DefaultName => "a condemned shade";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 9;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
