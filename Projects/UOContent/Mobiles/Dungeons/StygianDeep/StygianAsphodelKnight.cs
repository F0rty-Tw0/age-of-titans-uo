using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: SkeletalKnight (body 147).
[SerializationGenerator(0, false)]
public partial class StygianAsphodelKnight : BaseCreature
{
    [Constructible]
    public StygianAsphodelKnight() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(420, 460);
        SetDex(100, 130);
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

    public override string CorpseName => "an asphodel knight's corpse";
    public override string DefaultName => "an asphodel knight";

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
