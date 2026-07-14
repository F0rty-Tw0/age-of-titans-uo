using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - Kerykes bronze-servant line, L5
// trash. Donor: Imp.
[SerializationGenerator(0, false)]
public partial class CinderKeryxCourier : BaseCreature
{
    [Constructible]
    public CinderKeryxCourier() : base(AIType.AI_Mage)
    {
        Body = 74;
        Hue = 0x0798;
        BaseSoundID = 422;

        SetStr(140, 170);
        SetDex(100, 120);
        SetInt(100, 130);

        SetHits(300, 350);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 50, 60);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 70.0, 85.0);
        SetSkill(SkillName.Magery, 70.0, 85.0);
        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a keryx courier's corpse";
    public override string DefaultName => "a keryx courier";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
