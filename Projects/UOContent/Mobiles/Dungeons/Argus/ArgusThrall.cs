using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L3 trash.
// Donor: Headless One.
[SerializationGenerator(0, false)]
public partial class ArgusThrall : BaseCreature
{
    [Constructible]
    public ArgusThrall() : base(AIType.AI_Melee)
    {
        Body = 31;
        Hue = 0x0479;
        BaseSoundID = 0x39D;

        SetStr(135, 155);
        SetDex(60, 80);
        SetInt(30, 45);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 68.0);
        SetSkill(SkillName.Wrestling, 58.0, 70.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a hoard-thrall's corpse";
    public override string DefaultName => "a hoard-thrall";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanRummageCorpses => true;
    public override int Meat => 1;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
