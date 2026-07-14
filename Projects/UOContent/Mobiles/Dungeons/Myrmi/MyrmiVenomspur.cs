using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest (dev-docs/gap-families-bestiary.md §6.4) - Solen Hive + Terathan swarm, L5 trash. Donor: Red Solen Warrior.
[SerializationGenerator(0, false)]
public partial class MyrmiVenomspur : BaseCreature
{
    [Constructible]
    public MyrmiVenomspur() : base(AIType.AI_Melee)
    {
        Body = 782;
        Hue = 0x0021;
        BaseSoundID = 959;

        SetStr(220, 250);
        SetDex(125, 150);
        SetInt(40, 55);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.Poisoning, 60.0, 80.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a myrmex venomspur's corpse";
    public override string DefaultName => "a myrmex venomspur";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
