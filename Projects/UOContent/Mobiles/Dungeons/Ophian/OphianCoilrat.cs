using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - ambient fodder, L4. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class OphianCoilrat : BaseCreature
{
    [Constructible]
    public OphianCoilrat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0798;
        BaseSoundID = 0x188;

        SetStr(155, 180);
        SetDex(95, 115);
        SetInt(35, 50);

        SetHits(190, 220);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 33, 40);
        SetResistance(ResistanceType.Fire, 18, 26);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 28, 36);
        SetResistance(ResistanceType.Energy, 18, 26);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 2000;
        Karma = -2000;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a nest rat's corpse";
    public override string DefaultName => "a nest rat";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
