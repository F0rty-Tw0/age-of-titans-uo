using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class NewbieCharon : NewbieElite
{
    [Constructible]
    public NewbieCharon() : base(AIType.AI_Melee)
    {
        Body = 31;
        Hue = 0x0455;
        BaseSoundID = 0x39D;

        SetStr(150, 180);
        SetDex(60, 80);
        SetInt(30, 45);

        SetHits(180, 220);

        SetDamage(8, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a headless corpse";
    public override string DefaultName => "the Boatless Ferryman";

    public override bool ClickTitle => false;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
