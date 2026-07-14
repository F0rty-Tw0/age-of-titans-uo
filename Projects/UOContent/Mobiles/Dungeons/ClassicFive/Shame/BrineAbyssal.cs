using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L6. Donor: Deep Sea Serpent.
[SerializationGenerator(0, false)]
public partial class BrineAbyssal : BaseCreature
{
    [Constructible]
    public BrineAbyssal() : base(AIType.AI_Melee)
    {
        Name = "an abyssal serpent";

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
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 70.0, 85.0);
        SetSkill(SkillName.Tactics, 68.0, 83.0);
        SetSkill(SkillName.Wrestling, 68.0, 83.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;

        // DeepSeaSerpent donor is water-locked (CantWalk); this family shares land spawners,
        // so it swims AND walks (same fix as BrineLeechEel).
        CanSwim = true;
    }

    public override string CorpseName => "an abyssal serpent's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 5;

    // Undertow: a cold breath, distinct flavor from the family's melee-only serpents.
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
