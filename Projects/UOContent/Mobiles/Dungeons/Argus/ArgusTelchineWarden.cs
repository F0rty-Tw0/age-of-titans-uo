using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L5 trash. Donor: Stone Harpy.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineWarden : BaseCreature
{
    [Constructible]
    public ArgusTelchineWarden() : base(AIType.AI_Melee)
    {
        Body = 73;
        Hue = 0x08A5;
        BaseSoundID = 402;

        SetStr(320, 350);
        SetDex(150, 175);
        SetInt(60, 80);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 28, 36);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 75.0, 90.0);
        SetSkill(SkillName.Wrestling, 72.0, 88.0);

        Fame = 4400;
        Karma = -4400;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a Telchine vault-warden's corpse";
    public override string DefaultName => "a Telchine vault-warden";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanFly => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average, 2);
    }
}
