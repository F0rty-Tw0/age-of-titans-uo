using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder.md §1) - L4 trash. Donor: Sea Serpent.
// Also the Maelstrom Call add spawned by TideWarden.
[SerializationGenerator(0, false)]
public partial class TideBrinescale : BaseCreature
{
    [Constructible]
    public TideBrinescale() : base(AIType.AI_Melee)
    {
        Body = 150;
        Hue = 0x0481;
        BaseSoundID = 447;

        SetStr(70, 90);
        SetDex(90, 110);
        SetInt(20, 30);

        SetHits(100, 140);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 18, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 40, 50);

        SetSkill(SkillName.Poisoning, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 35.0, 45.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 900;
        Karma = -900;

        VirtualArmor = 22;
    }

    public override string CorpseName => "a serpent's corpse";
    public override string DefaultName => "a brinescale serpent";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override Poison HitPoison => Poison.Regular; // "Venom of the Deep"

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
