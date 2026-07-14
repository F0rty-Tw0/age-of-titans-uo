using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame ambient, L5. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class BrineJelly : BaseCreature
{
    [Constructible]
    public BrineJelly() : base(AIType.AI_Melee)
    {
        Name = "a stinging jelly";

        Body = 51;
        Hue = 0x0480;
        BaseSoundID = 456;

        SetStr(180, 210);
        SetDex(60, 80);
        SetInt(30, 45);

        SetHits(295, 315);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 10, 20);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.Poisoning, 40.0, 55.0);
        SetSkill(SkillName.MagicResist, 35.0, 50.0);
        SetSkill(SkillName.Tactics, 55.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 70.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a stinging jelly's remains";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
