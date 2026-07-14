using System;
using Server.Engines.LootBags;
using Server.Engines.Rarity;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// Pure decision-logic tests for the pantheon-bag roll (dev-docs/itemization/30-pantheon-bags.md
// §5, §8) — no item construction, no world fixture. Mirrors LootRollerDecisionTests' idiom:
// LootRoller.RollDecision(bagLevel, domain) never touches an Item.
public class PantheonBagDecisionTests
{
    private static readonly PantheonDomain[] AllDomains = Enum.GetValues<PantheonDomain>();

    [Fact]
    public void EveryDomain_HasCandidates_InAtLeastOneCategory()
    {
        foreach (var domain in AllDomains)
        {
            var hasAny =
                LootRoller.DomainHasCandidates(domain, LootRoller.LootCategory.Weapon) ||
                LootRoller.DomainHasCandidates(domain, LootRoller.LootCategory.Armor) ||
                LootRoller.DomainHasCandidates(domain, LootRoller.LootCategory.Shield) ||
                LootRoller.DomainHasCandidates(domain, LootRoller.LootCategory.Jewelry) ||
                LootRoller.DomainHasCandidates(domain, LootRoller.LootCategory.Clothing);

            Assert.True(hasAny, $"{domain} has no candidates in any loot category");
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(7)]
    [InlineData(9)]
    [InlineData(10)]
    public void ThemedDecision_NeverLeaksAnotherDomain(int bagLevel)
    {
        foreach (var domain in AllDomains)
        {
            for (var i = 0; i < 500; i++)
            {
                var decision = LootRoller.RollDecision(bagLevel, domain);
                var actual = PantheonFx.GetDomain(decision.Theme);

                Assert.True(
                    actual == domain,
                    $"bag {bagLevel} domain {domain} rolled theme {decision.Theme} belonging to {actual}"
                );
            }
        }
    }

    [Fact]
    public void ThemedDecision_RespectsRarityWeights()
    {
        // Rarity is rolled before category/domain filtering (LootRoller.RollDecision(int,
        // PantheonDomain)), so the bag-level floor/ceiling from _rarityWeights still binds exactly:
        // bag 0 = 100% Uncommon, bag 10 = 100% Legendary (framework §8 unchanged by domain).
        foreach (var domain in AllDomains)
        {
            for (var i = 0; i < 200; i++)
            {
                Assert.Equal(ItemRarity.Uncommon, LootRoller.RollDecision(0, domain).Rarity);
                Assert.Equal(ItemRarity.Legendary, LootRoller.RollDecision(10, domain).Rarity);
            }
        }
    }

    [Fact]
    public void ThemedLegendary_AlwaysResolvesARegistryEntry()
    {
        // God-locked legendaries must exist for every reachable (domain, family, theme, base)
        // combo — a registry hole here means a themed bag 10 could resolve to no legendary at all.
        foreach (var domain in AllDomains)
        {
            for (var i = 0; i < 500; i++)
            {
                var decision = LootRoller.RollDecision(10, domain);

                Assert.True(
                    LegendaryRegistry.TryGetByRootAndBase(decision.Family, decision.Theme, (byte)decision.BaseIndex, out _),
                    $"{domain}: no legendary for family {decision.Family}, theme {decision.Theme}, base {decision.BaseIndex}"
                );
            }
        }
    }
}

// PantheonLootMap resolution (pantheon-bags §3, §7) — explicit Type -> domain checks, same
// no-fixture idiom (TryGetDomain(Type, ...) never touches an Item).
public class PantheonLootMapTests
{
    [Theory]
    [InlineData(typeof(TideHerald), PantheonDomain.Sea)]
    [InlineData(typeof(CinderHeart), PantheonDomain.Forge)]
    [InlineData(typeof(WyldStag), PantheonDomain.Hunt)]
    [InlineData(typeof(StormFather), PantheonDomain.Sky)]
    [InlineData(typeof(StygianLord), PantheonDomain.Underworld)]
    [InlineData(typeof(GaianDustadder), PantheonDomain.Nature)]
    [InlineData(typeof(TartarusAshhound), PantheonDomain.War)]
    [InlineData(typeof(CursedGraverat), PantheonDomain.Night)]
    [InlineData(typeof(RimeCheimon), PantheonDomain.Wind)]
    [InlineData(typeof(ArgusMidas), PantheonDomain.Aegis)]
    [InlineData(typeof(PyreCindermoth), PantheonDomain.Sun)]
    public void PantheonLootMap_ResolvesFamilies(Type creatureType, PantheonDomain expected)
    {
        Assert.True(PantheonLootMap.TryGetDomain(creatureType, out var domain), $"{creatureType.Name} did not resolve a domain");
        Assert.Equal(expected, domain);
    }

    // "Cursed"/"Pyre" are family prefixes; these three types merely collide with them by name
    // (or bear no prefix at all, in the Labor's case) and must resolve to no domain (generic bag).
    // Server.Engines.Quests.Samurai.CursedSoul in particular collides on Type.Name alone — a
    // namespace-qualified match would be a bug, since PantheonLootMap keys off type.Name.
    [Theory]
    [InlineData(typeof(Pyre))]
    [InlineData(typeof(Server.Engines.Quests.Samurai.CursedSoul))]
    [InlineData(typeof(RestlessSoul))]
    [InlineData(typeof(LaborNemeanLion))]
    public void PantheonLootMap_StockCollisions_StayGeneric(Type creatureType)
    {
        Assert.False(PantheonLootMap.TryGetDomain(creatureType, out _), $"{creatureType.Name} should resolve to no domain (generic bag)");
    }

    [Theory]
    [InlineData(typeof(Thaumas), PantheonDomain.Sea)]
    [InlineData(typeof(Ladon), PantheonDomain.War)]
    public void NamedSingles_Resolve(Type creatureType, PantheonDomain expected)
    {
        Assert.True(PantheonLootMap.TryGetDomain(creatureType, out var domain), $"{creatureType.Name} did not resolve a domain");
        Assert.Equal(expected, domain);
    }
}
