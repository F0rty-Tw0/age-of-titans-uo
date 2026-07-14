using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family (Telchines), L6. Donor: Sea Serpent.
[SerializationGenerator(0, false)]
public partial class BrineTelchinDrowner : BaseCreature
{
    [Constructible]
    public BrineTelchinDrowner() : base(AIType.AI_Melee)
    {
        Name = "a telchine drowner";

        Body = 150;
        Hue = 0x04F2;
        BaseSoundID = 447;

        SetStr(240, 270);
        SetDex(85, 105);
        SetInt(70, 95);

        SetHits(490, 510);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 73.0, 88.0);
        SetSkill(SkillName.Tactics, 73.0, 88.0);
        SetSkill(SkillName.Wrestling, 73.0, 88.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;

        // SeaSerpent donor is water-locked (CantWalk); this family shares land spawners,
        // so it swims AND walks (same fix as BrineLeechEel).
        CanSwim = true;
    }

    public override string CorpseName => "a telchine drowner's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
