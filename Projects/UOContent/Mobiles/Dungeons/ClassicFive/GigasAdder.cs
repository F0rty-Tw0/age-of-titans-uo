using ModernUO.Serialization;

namespace Server.Mobiles;

// Brood of Echidna (dev-docs/classic-five-enhancements.md) - Despise skin, L3 band.
[SerializationGenerator(0, false)]
public partial class GigasAdder : EchidnaBrood
{
    [Constructible]
    public GigasAdder() : base("a gigas adder", 0x08B2, 145, 158, 9, 14, 1400)
    {
    }

    public override Poison HitPoison => Poison.Lesser;

    // Tail-sweep: stamina drain to punish melee chasers.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The adder's tail sweeps your legs out from under you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
