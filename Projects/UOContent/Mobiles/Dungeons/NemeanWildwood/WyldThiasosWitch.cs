using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Evil Mage (human
// body; T2A has no distinct huntress body).
[SerializationGenerator(0, false)]
public partial class WyldThiasosWitch : BaseCreature
{
    [Constructible]
    public WyldThiasosWitch() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0851;

        SetStr(185, 215);
        SetDex(90, 110);
        SetInt(140, 170);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 65, 75);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 80.0, 95.0);
        SetSkill(SkillName.Magery, 80.0, 95.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a Thiasos witch's corpse";
    public override string DefaultName => "a Thiasos witch";

    public override Poison PoisonImmune => Poison.Greater;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
