using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L5 core family. Donor: Sea
// Serpent.
[SerializationGenerator(0, false)]
public partial class TideReefserpent : BaseCreature
{
    [Constructible]
    public TideReefserpent() : base(AIType.AI_Melee)
    {
        Body = 150;
        Hue = 0x0851;
        BaseSoundID = 447;

        SetStr(170, 200);
        SetDex(75, 95);
        SetInt(30, 45);

        SetHits(300, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 42);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 30, 40);

        SetSkill(SkillName.Poisoning, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 40;

        CanSwim = true;
    }

    public override string CorpseName => "a reef serpent's corpse";
    public override string DefaultName => "a reef serpent";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison HitPoison => Poison.Greater; // "Venom of the Deep"

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
