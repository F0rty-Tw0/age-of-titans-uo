using Server;
using Server.Engines.LootBags;
using Server.Engines.Rarity;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

// Reachability tripwire — born from the Aello bug: a clause can be dead-on-arrival because of
// its CARRIER (a two-handed weapon can never shield-parry; a lane without block never blocks).
// These tests make that class of design bug a loud failure for every future legendary batch.
[Collection("Sequential UOContent Tests")]
public class LegendaryReachabilityTests
{
    // Clauses that only act when a block/parry/dodge EVENT occurs on the wielder. Extend this
    // list when adding new event-gated weapon clauses.
    private static readonly ClauseType[] EventGatedWeaponClauses = { ClauseType.ExtraSwingOnParry };

    [Fact]
    public void EventGatedWeaponClauses_AreReachableByTheirCarrier()
    {
        var entries = LegendaryRegistry.Entries;

        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];

            if (entry.Family > LegendaryRegistry.FamilyArchery ||
                System.Array.IndexOf(EventGatedWeaponClauses, entry.Clause) < 0)
            {
                continue;
            }

            var item = LootRoller.ConstructForLegendary(entry);

            try
            {
                var weapon = Assert.IsAssignableFrom<BaseWeapon>(item);

                // Reachable if the carrier can pair a shield (parry), its own lane can block,
                // or the clause also answers a dodge (buildable via worn gear on any carrier).
                var canParry = weapon.Layer == Layer.OneHanded;
                var canBlock = WeaponEffectTable.Get(entry.Root, ItemRarity.Legendary).BlockPct > 0;
                var canDodge = (ClauseTraits.Get(entry.Clause) & ClauseTrigger.Dodge) != 0;

                Assert.True(
                    canParry || canBlock || canDodge,
                    $"{entry.Name} ({item.GetType().Name}): clause {entry.Clause} has no reachable trigger — " +
                    "two-handed carrier, no lane block, no dodge path"
                );
            }
            finally
            {
                item.Delete();
            }
        }
    }

    // A legendary must never promise a clause that has no live dispatch. The Deferred flag exists
    // for parking future clauses — parked clauses may not be handed to items.
    [Fact]
    public void EveryLegendaryClause_HasALiveDispatch()
    {
        var entries = LegendaryRegistry.Entries;

        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];

            Assert.True(
                ClauseTraits.HasDispatch(entry.Clause),
                $"{entry.Name} carries {entry.Clause}, which has no live dispatch (Deferred or unclassified)"
            );
        }
    }

    // ConstructForLegendary must produce a valid, apply-able carrier for EVERY registry entry —
    // this is the altar's reward path, so a single bad factory index would brick an offering.
    [Fact]
    public void ConstructForLegendary_YieldsValidCarrier_ForEveryEntry()
    {
        var entries = LegendaryRegistry.Entries;

        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            var item = LootRoller.ConstructForLegendary(entry);

            try
            {
                Assert.NotNull(item);
                RarityEffects.ApplyLegendary(item, entry.Id); // throws on any family/shape mismatch

                var variant = Assert.IsAssignableFrom<IVariantItem>(item);
                Assert.Equal(entry.Id, variant.LegendaryId);
            }
            finally
            {
                item.Delete();
            }
        }
    }
}
