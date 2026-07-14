using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L5. Donor: Sea Serpent.
[SerializationGenerator(0, false)]
public partial class BrineDeepcoil : BaseCreature
{
    [Constructible]
    public BrineDeepcoil() : base(AIType.AI_Melee)
    {
        Name = "a deep-coil serpent";

        Body = 150;
        Hue = 0x04F2;
        BaseSoundID = 447;

        SetStr(200, 230);
        SetDex(70, 90);
        SetInt(60, 85);

        SetHits(315, 340);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 50, 60);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 65.0, 80.0);
        SetSkill(SkillName.Tactics, 65.0, 80.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 50;

        // SeaSerpent donor is water-locked (CantWalk); this family shares land spawners,
        // so it swims AND walks (same fix as BrineLeechEel).
        CanSwim = true;
    }

    public override string CorpseName => "a deep-coil serpent's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
