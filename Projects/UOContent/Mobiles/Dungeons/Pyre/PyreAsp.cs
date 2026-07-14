using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon. L5 trash. Donor: Lava Snake.
[SerializationGenerator(0, false)]
public partial class PyreAsp : BaseCreature
{
    [Constructible]
    public PyreAsp() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0655;
        BaseSoundID = 0xDB;

        SetStr(190, 220);
        SetDex(120, 145);
        SetInt(40, 60);

        SetHits(300, 340);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 20, 25);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 20, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a cinder asp's corpse";
    public override string DefaultName => "a cinder asp";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
