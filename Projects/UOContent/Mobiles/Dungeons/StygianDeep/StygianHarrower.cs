using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: HeadlessOne (body 31).
[SerializationGenerator(0, false)]
public partial class StygianHarrower : BaseCreature
{
    [Constructible]
    public StygianHarrower() : base(AIType.AI_Melee)
    {
        Body = 31;
        Hue = 0x0453;
        BaseSoundID = 0x39D;

        SetStr(480, 530);
        SetDex(130, 160);
        SetInt(50, 80);

        SetHits(2600, 2900);

        SetDamage(23, 29);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 105.0, 115.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "a harrower of souls's corpse";
    public override string DefaultName => "a harrower of souls";

    public override int LootBagLevel => 9;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
