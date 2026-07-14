using ModernUO.Serialization;

namespace Server.Mobiles;

// Brood of Echidna (dev-docs/classic-five-enhancements.md) - Deceit skin, L4 band.
[SerializationGenerator(0, false)]
public partial class GraveAsp : EchidnaBrood
{
    [Constructible]
    public GraveAsp() : base("a grave asp", 0x0389, 222, 238, 12, 18, 2200)
    {
    }

    public override Poison HitPoison => Poison.Regular;

    // Lifedrain bite: self-heal on a successful hit.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            Hits += 8;
            defender.LocalOverheadMessage(MessageType.Regular, 0x489, false, "The asp's bite drains your vitality!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
