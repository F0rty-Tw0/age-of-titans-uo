using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Giant Serpent.
[SerializationGenerator(0, false)]
public partial class WyldConstrictor : BaseCreature
{
    [Constructible]
    public WyldConstrictor() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0851;
        BaseSoundID = 219;

        SetStr(235, 265);
        SetDex(90, 110);
        SetInt(90, 110);

        SetHits(620, 690);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Poison, 60);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 75, 85);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.Poisoning, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 4800;
        Karma = -4800;

        VirtualArmor = 60;
    }

    public override string CorpseName => "a sacred constrictor's corpse";
    public override string DefaultName => "a sacred constrictor";

    public override Poison PoisonImmune => Poison.Greater;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
