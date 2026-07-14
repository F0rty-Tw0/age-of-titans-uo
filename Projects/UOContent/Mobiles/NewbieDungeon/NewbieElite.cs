using ModernUO.Serialization;
using Server.Engines.LootBags;
using Server.Items;

namespace Server.Mobiles;

// Shared behavior for newbie-dungeon elites. They opt out of the global loot-bag roll
// (DropsLootBag => false) and instead force-drop a guaranteed bag 2 on death, telegraphed
// with a corpse sparkle + shimmer sound so a solo player recognizes a real reward.
[SerializationGenerator(0, false)]
public abstract partial class NewbieElite : BaseCreature
{
    protected NewbieElite(AIType aiType) : base(aiType)
    {
    }

    public override bool DropsLootBag => false;

    public override void OnDeath(Container c)
    {
        base.OnDeath(c);

        var bag = new LootBag(2);
        bag.DropItem(LootRoller.Roll(2));
        c.DropItem(bag);

        Effects.SendLocationEffect(c, 0x3728, 10);
        Effects.PlaySound(c.Location, c.Map, 0x1F2);
    }
}
