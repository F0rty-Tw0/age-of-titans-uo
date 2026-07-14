using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L4 trash. Donor: Wraith.
[SerializationGenerator(0, false)]
public partial class ArgusCoinwraith : BaseCreature
{
    [Constructible]
    public ArgusCoinwraith() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0479;
        BaseSoundID = 0x482;

        SetStr(150, 175);
        SetDex(140, 165);
        SetInt(190, 215);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 28, 36);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 24, 32);

        SetSkill(SkillName.EvalInt, 62.0, 72.0);
        SetSkill(SkillName.Magery, 62.0, 72.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 52.0, 62.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 2700;
        Karma = -2700;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a coin wraith's fading form";
    public override string DefaultName => "a coin wraith";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
        AddLoot(LootPack.Gems);
    }
}
