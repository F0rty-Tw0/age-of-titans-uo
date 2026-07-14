using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L5. Donor: Scorpion.
[SerializationGenerator(0, false)]
public partial class BrineReefcrab : BaseCreature
{
    [Constructible]
    public BrineReefcrab() : base(AIType.AI_Melee)
    {
        Name = "a reef crab";

        Body = 48;
        Hue = 0x04F2;
        BaseSoundID = 397;

        SetStr(200, 230);
        SetDex(90, 110);
        SetInt(40, 60);

        SetHits(300, 320);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.Poisoning, 80.0, 95.0);
        SetSkill(SkillName.MagicResist, 40.0, 55.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a reef crab's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
