using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L5 trash. Donor: Gazer.
[SerializationGenerator(0, false)]
public partial class ArgusWatcher : BaseCreature
{
    [Constructible]
    public ArgusWatcher() : base(AIType.AI_Mage)
    {
        Body = 22;
        Hue = 0x0486;
        BaseSoundID = 377;

        SetStr(190, 220);
        SetDex(120, 145);
        SetInt(230, 260);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 28, 36);

        SetSkill(SkillName.EvalInt, 60.0, 70.0);
        SetSkill(SkillName.Magery, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a hundred-eyed watcher's remains";
    public override string DefaultName => "a hundred-eyed watcher";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
