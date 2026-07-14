using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - ambient fodder, L3. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class ArgusGoldrat : BaseCreature
{
    [Constructible]
    public ArgusGoldrat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0479;
        BaseSoundID = 0x188;

        SetStr(110, 130);
        SetDex(70, 90);
        SetInt(20, 32);

        SetHits(130, 155);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 24, 30);
        SetResistance(ResistanceType.Fire, 14, 20);
        SetResistance(ResistanceType.Cold, 14, 20);
        SetResistance(ResistanceType.Poison, 16, 22);
        SetResistance(ResistanceType.Energy, 14, 20);

        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 38.0, 48.0);
        SetSkill(SkillName.Wrestling, 38.0, 48.0);

        Fame = 1100;
        Karma = -1100;

        VirtualArmor = 26;
    }

    public override string CorpseName => "a gilded rat's carcass";
    public override string DefaultName => "a gilded rat";

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
