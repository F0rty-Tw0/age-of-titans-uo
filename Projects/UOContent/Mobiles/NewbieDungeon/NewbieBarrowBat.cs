using ModernUO.Serialization;

namespace Server.Mobiles;

// L1 flappy filler: fast, fragile, annoying — movement/targeting practice alongside the
// grave rat, with a different silhouette and rhythm.
[SerializationGenerator(0, false)]
public partial class NewbieBarrowBat : BaseCreature
{
    [Constructible]
    public NewbieBarrowBat() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0455;
        BaseSoundID = 422;

        SetStr(15, 25);
        SetDex(60, 75);
        SetInt(10, 15);

        SetHits(22, 32);

        SetDamage(1, 3);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 5, 10);

        SetSkill(SkillName.MagicResist, 5.0, 15.0);
        SetSkill(SkillName.Tactics, 15.0, 25.0);
        SetSkill(SkillName.Wrestling, 20.0, 30.0);

        Fame = 100;
        Karma = -100;

        VirtualArmor = 8;
    }

    public override string CorpseName => "a barrow bat's corpse";
    public override string DefaultName => "a barrow bat";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 0;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
