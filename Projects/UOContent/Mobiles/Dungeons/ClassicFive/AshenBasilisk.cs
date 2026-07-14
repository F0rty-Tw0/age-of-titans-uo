using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Brood of Echidna (dev-docs/classic-five-enhancements.md) - Hythloth skin, L7 band.
[SerializationGenerator(0, false)]
public partial class AshenBasilisk : EchidnaBrood
{
    // Fire aura cadence; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public AshenBasilisk() : base("an ashen basilisk", 0x0455, 660, 715, 24, 32, 7200)
    {
    }

    public override Poison HitPoison => Poison.Deadly;

    public override void OnThink()
    {
        base.OnThink();

        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 3);
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
