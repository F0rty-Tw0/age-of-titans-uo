using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class NewbieGraveRat : BaseCreature
{
    [Constructible]
    public NewbieGraveRat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0481;
        BaseSoundID = 0x188;

        SetStr(15, 25);
        SetDex(30, 45);
        SetInt(5, 10);

        SetHits(20, 30);
        SetMana(0);

        SetDamage(1, 3);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 5, 10);
        SetResistance(ResistanceType.Poison, 10, 15);

        SetSkill(SkillName.MagicResist, 10.0, 15.0);
        SetSkill(SkillName.Tactics, 15.0, 20.0);
        SetSkill(SkillName.Wrestling, 15.0, 20.0);

        Fame = 40;
        Karma = -40;

        VirtualArmor = 6;

        SetSpeed(0.1, 0.2);
    }

    public override string CorpseName => "a rat corpse";
    public override string DefaultName => "a barrow rat";

    public override int LootBagLevel => 0;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
