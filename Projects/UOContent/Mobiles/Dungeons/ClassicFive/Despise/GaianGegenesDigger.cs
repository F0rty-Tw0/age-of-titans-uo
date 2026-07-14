using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gegenes sub-faction. L4. Donor: Troll.
[SerializationGenerator(0, false)]
public partial class GaianGegenesDigger : BaseCreature
{
    [Constructible]
    public GaianGegenesDigger() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(53, 54);
        Hue = 0x09C2;
        BaseSoundID = 461;

        SetStr(170, 195);
        SetDex(55, 75);
        SetInt(30, 48);

        SetHits(210, 230);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 16, 23);
        SetResistance(ResistanceType.Cold, 16, 23);
        SetResistance(ResistanceType.Poison, 18, 26);
        SetResistance(ResistanceType.Energy, 16, 23);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 58.0, 70.0);
        SetSkill(SkillName.Wrestling, 56.0, 68.0);

        Fame = 2550;
        Karma = -2550;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a gegenes digger's corpse";
    public override string DefaultName => "a gegenes digger";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
