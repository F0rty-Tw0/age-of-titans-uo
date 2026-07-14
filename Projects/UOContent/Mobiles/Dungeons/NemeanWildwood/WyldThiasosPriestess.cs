using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Evil Mage (human
// body; T2A has no distinct huntress body).
[SerializationGenerator(0, false)]
public partial class WyldThiasosPriestess : BaseCreature
{
    [Constructible]
    public WyldThiasosPriestess() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0486;

        SetStr(190, 220);
        SetDex(90, 110);
        SetInt(145, 175);

        SetHits(610, 670);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 52);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 60, 70);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 82.0, 97.0);
        SetSkill(SkillName.Magery, 82.0, 97.0);
        SetSkill(SkillName.MagicResist, 67.0, 77.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a fallen priestess's corpse";
    public override string DefaultName => "the Thiasos priestess";

    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
