using ModernUO.Serialization;
using Server.Engines.Leveling;
using Server.Engines.LootBags;
using Server.Items;

namespace Server.Mobiles;

// Shared behavior for named dungeon elites and bosses: opt out of the global loot-bag roll
// (DropsLootBag => false) and force-drop guaranteed bag(s) on death, telegraphed with a
// corpse sparkle + shimmer sound so the reward reads even to a solo player.
[SerializationGenerator(0, false)]
public abstract partial class DungeonElite : BaseCreature
{
    protected DungeonElite(AIType aiType) : base(aiType)
    {
    }

    public override bool DropsLootBag => false;

    // Level of the guaranteed bag(s) this elite drops.
    public abstract int EliteBagLevel { get; }

    // Bosses raise this to drop more than one bag.
    public virtual int EliteBagCount => 1;

    public override void OnDeath(Container c)
    {
        base.OnDeath(c);

        // Anti-farm clamp: the guaranteed drop is for on-tier hunters. When the killing
        // blow comes from a player (or their pet) more than 2 levels above this elite,
        // downgrade to the normal trash-style roll at the same bag level. Last-hit rule —
        // a high-level friend finishing the kill costs the group the guarantee; tune later
        // if it stings in practice.
        var killer = (LastKiller as BaseCreature)?.GetMaster() ?? LastKiller;
        if (killer is PlayerMobile pm &&
            LevelSystem.GetLevel(pm) > LevelConfig.GetMobLevel(this) + 2 &&
            Utility.RandomDouble() >= LootBagConfig.ChanceForMobLevel(EliteBagLevel))
        {
            return;
        }

        // Elites are their god's champions: always the themed bag when the family maps
        // (no 70/30 — 30-pantheon-bags.md §2). Contents are god-locked at every tier.
        var themed = PantheonLootMap.TryGetDomain(this, out var domain);

        for (var i = 0; i < EliteBagCount; i++)
        {
            var bag = themed ? new LootBag(EliteBagLevel, domain) : new LootBag(EliteBagLevel);
            bag.DropItem(themed ? LootRoller.Roll(EliteBagLevel, domain) : LootRoller.Roll(EliteBagLevel));
            c.DropItem(bag);
        }

        Effects.SendLocationEffect(c, 0x3728, 10);
        Effects.PlaySound(c.Location, c.Map, 0x1F2);

        // Deco drop: 1% chance for level 7+ elites, after the anti-farm clamp above so
        // over-leveled farmers who got downgraded/denied the bag also get nothing here.
        if (LevelConfig.GetMobLevel(this) >= 7 && Utility.RandomDouble() < 0.01)
        {
            c.DropItem(CreateDecoDrop());
        }
    }

    protected virtual Item CreateDecoDrop() => EliteDecoDrops.Roll(LevelConfig.GetMobLevel(this));
}
