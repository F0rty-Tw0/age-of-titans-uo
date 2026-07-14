using ModernUO.Serialization;

namespace Server.Mobiles;

// Brood of Echidna (dev-docs/classic-five-enhancements.md) - Shame skin, L5 band.
[SerializationGenerator(0, false)]
public partial class BrineSerpent : EchidnaBrood
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };

    [Constructible]
    public BrineSerpent() : base("a brine serpent", 0x059B, 360, 378, 16, 22, 3600)
    {
        CanSwim = true;
    }

    public override Poison HitPoison => Poison.Regular;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
