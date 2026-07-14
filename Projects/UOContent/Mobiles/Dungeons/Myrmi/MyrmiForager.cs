using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest (dev-docs/gap-families-bestiary.md §6.4) - Solen Hive + Terathan swarm, L3 trash. Donor: Black Solen Worker.
[SerializationGenerator(0, false)]
public partial class MyrmiForager : BaseCreature
{
    [Constructible]
    public MyrmiForager() : base(AIType.AI_Melee)
    {
        Body = 805;
        Hue = 0x0798;
        BaseSoundID = 959;

        SetStr(100, 125);
        SetDex(80, 100);
        SetInt(30, 45);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 18);
        SetResistance(ResistanceType.Cold, 10, 18);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 18);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a myrmex forager's corpse";
    public override string DefaultName => "a myrmex forager";

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
