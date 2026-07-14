using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L3 trash. Donor: Headless One.
[SerializationGenerator(0, false)]
public partial class ArgusGilded : BaseCreature
{
    [Constructible]
    public ArgusGilded() : base(AIType.AI_Melee)
    {
        Body = 31;
        Hue = 0x0479;
        BaseSoundID = 0x39D;

        SetStr(130, 150);
        SetDex(65, 85);
        SetInt(28, 42);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 52.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 68.0);

        Fame = 1450;
        Karma = -1450;

        VirtualArmor = 33;
    }

    public override string CorpseName => "a gilded thrall's corpse";
    public override string DefaultName => "a gilded thrall";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanRummageCorpses => true;
    public override int Meat => 1;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
