using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, Bramble Court, L3. Donor: Corpser.
[SerializationGenerator(0, false)]
public partial class GroveBrambleWarden : BaseCreature
{
    [Constructible]
    public GroveBrambleWarden() : base(AIType.AI_Melee)
    {
        Body = 8;
        Hue = 0x0491;
        BaseSoundID = 684;

        SetStr(108, 135);
        SetDex(78, 100);
        SetInt(30, 50);

        SetHits(150, 160);

        SetDamage(7, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a bramble warden corpse";
    public override string DefaultName => "a bramble warden";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison HitPoison => Poison.Regular;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
