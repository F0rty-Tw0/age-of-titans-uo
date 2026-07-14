using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Chained Titans sub-faction.
// L8 trash. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class TartarusTitanThrall : BaseCreature
{
    [Constructible]
    public TartarusTitanThrall() : base(AIType.AI_Melee)
    {
        Body = 75;
        Hue = 0x0455;
        BaseSoundID = 604;

        SetStr(690, 760);
        SetDex(116, 145);
        SetInt(41, 65);

        SetHits(870, 900);
        SetMana(0);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 61, 71);
        SetResistance(ResistanceType.Fire, 36, 46);
        SetResistance(ResistanceType.Cold, 31, 41);
        SetResistance(ResistanceType.Poison, 36, 46);
        SetResistance(ResistanceType.Energy, 36, 46);

        SetSkill(SkillName.MagicResist, 70.0, 105.0);
        SetSkill(SkillName.Tactics, 85.0, 105.0);
        SetSkill(SkillName.Wrestling, 85.0, 100.0);

        Fame = 13500;
        Karma = -13500;

        VirtualArmor = 60;
    }

    public override string CorpseName => "a chained thrall's corpse";
    public override string DefaultName => "a chained thrall";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
