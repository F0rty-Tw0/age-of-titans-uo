using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L4 trash. Donor: Ratman.
[SerializationGenerator(0, false)]
public partial class RimeThrall : BaseCreature
{
    [Constructible]
    public RimeThrall() : base(AIType.AI_Melee)
    {
        Body = 42;
        Hue = 0x047E;
        BaseSoundID = 437;

        SetStr(150, 175);
        SetDex(80, 100);
        SetInt(40, 60);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a rime-bound thrall's corpse";
    public override string DefaultName => "a rime-bound thrall";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
