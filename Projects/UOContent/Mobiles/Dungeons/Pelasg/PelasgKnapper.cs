using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves. L2 trash. Donor: Troglodyte.
[SerializationGenerator(0, false)]
public partial class PelasgKnapper : BaseCreature
{
    [Constructible]
    public PelasgKnapper() : base(AIType.AI_Melee)
    {
        Body = 267;
        Hue = 0x0844;
        BaseSoundID = 0x59F;

        SetStr(95, 115);
        SetDex(70, 90);
        SetInt(28, 40);

        SetHits(90, 110);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 24, 30);
        SetResistance(ResistanceType.Fire, 13, 19);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 13, 19);

        SetSkill(SkillName.MagicResist, 36.0, 46.0);
        SetSkill(SkillName.Tactics, 44.0, 56.0);
        SetSkill(SkillName.Wrestling, 44.0, 56.0);

        Fame = 1100;
        Karma = -1100;

        VirtualArmor = 27;
    }

    public override string CorpseName => "a Pelasgian stone-knapper's corpse";
    public override string DefaultName => "a Pelasgian stone-knapper";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
