using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L4 core family. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class TideEel : BaseCreature
{
    [Constructible]
    public TideEel() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0481;
        BaseSoundID = 0xDB;

        SetStr(65, 85);
        SetDex(70, 90);
        SetInt(15, 25);

        SetHits(100, 140);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 16, 24);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 25, 35);

        SetSkill(SkillName.Poisoning, 45.0, 55.0);
        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 35.0, 45.0);
        SetSkill(SkillName.Wrestling, 35.0, 45.0);

        Fame = 800;
        Karma = -800;

        VirtualArmor = 18;

        CanSwim = true;
    }

    public override string CorpseName => "an eel's corpse";
    public override string DefaultName => "a coiling eel";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override Poison HitPoison => Poison.Regular; // "Venom of the Deep"

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
