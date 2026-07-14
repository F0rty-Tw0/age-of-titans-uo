using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L3 trash.
// Donor: Slime.
[SerializationGenerator(0, false)]
public partial class ArgusOoze : BaseCreature
{
    [Constructible]
    public ArgusOoze() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0479;
        BaseSoundID = 456;

        SetStr(110, 130);
        SetDex(40, 55);
        SetInt(30, 45);

        SetHits(130, 160);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Poisoning, 50.0, 60.0);
        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a gilded ooze's remains";
    public override string DefaultName => "a gilded ooze";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
        AddLoot(LootPack.Gems);
    }
}
