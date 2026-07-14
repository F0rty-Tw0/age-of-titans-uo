using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: Skeleton (body Utility.RandomList(50, 56)).
[SerializationGenerator(0, false)]
public partial class StygianWight : BaseCreature
{
    [Constructible]
    public StygianWight() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0455;
        BaseSoundID = 0x48D;

        SetStr(420, 460);
        SetDex(100, 130);
        SetInt(50, 80);

        SetHits(2000, 2250);

        SetDamage(18, 23);

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

    public override string CorpseName => "an unransomed wight's corpse";
    public override string DefaultName => "an unransomed wight";

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
