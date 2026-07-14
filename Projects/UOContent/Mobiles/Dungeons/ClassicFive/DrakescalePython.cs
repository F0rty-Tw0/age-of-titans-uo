using ModernUO.Serialization;

namespace Server.Mobiles;

// Brood of Echidna (dev-docs/classic-five-enhancements.md) - Destard skin, L6 band.
[SerializationGenerator(0, false)]
public partial class DrakescalePython : EchidnaBrood
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public DrakescalePython() : base("a drakescale python", 0x0025, 520, 545, 20, 27, 5500)
    {
    }

    public override Poison HitPoison => Poison.Greater;

    // Weak fire breath: stock scalar off this skin's modest HP keeps it a minor threat.
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
