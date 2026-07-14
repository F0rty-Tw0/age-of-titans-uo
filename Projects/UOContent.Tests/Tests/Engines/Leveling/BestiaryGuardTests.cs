using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Server.Engines.Leveling;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// Guard tests for the custom bestiary (dev-docs/beast-reference.md). Two silent failure
// modes exist by construction: a creature missing its MobLevelOverrides pin quietly
// mis-tiers on the HP heuristic (wrong XP/tag/bag), and a typo'd LootBagLevel literal
// diverges from the pin with no error anywhere. Both become red tests here.
public class BestiaryGuardTests
{
    private static readonly string[] _familyPrefixes =
    {
        "Newbie", "Tide", "Cinder", "Wyld", "Storm", "Stygian", "Gaian", "Drowned", "Brine",
        "Drakon", "Tartarus", "Pyre", "Rime", "Cursed", "Myrmi", "Ophian", "Lykai", "Argus",
        "Wayman", "Pelasg", "Grove", "Peak", "Mire", "Restless", "Shore", "Labor"
    };

    private static readonly string[] _namedSingles =
    {
        "Enkelados", "Aiakos", "Thaumas", "Ladon", "Alastor",
        "Chthonios", "Minos", "Glaukos", "Pythios", "Eurynomos"
    };

    // Rides the HP heuristic on purpose: the Echidna brood keeps its tag honest via HP.
    private static readonly string[] _heuristicIntentional =
    {
        "GigasAdder", "GraveAsp", "BrineSerpent", "DrakescalePython", "AshenBasilisk"
    };

    // Stock classes that merely collide with a family prefix — not ours.
    private static readonly string[] _stockCollisions = { "Cursed", "CursedSoul", "RestlessSoul", "Pyre" };

    // Non-combat props (no pin by design).
    private static readonly string[] _nonCombat = { "NewbieFerryman" };

    private static IEnumerable<Type> CustomCreatureTypes() =>
        typeof(BaseCreature).Assembly.GetTypes().Where(t =>
            t is { IsAbstract: false } &&
            t.IsSubclassOf(typeof(BaseCreature)) &&
            (Array.Exists(_familyPrefixes, p => t.Name.StartsWith(p, StringComparison.Ordinal)) ||
             _namedSingles.Contains(t.Name)) &&
            !_stockCollisions.Contains(t.Name) &&
            !_heuristicIntentional.Contains(t.Name) &&
            !_nonCombat.Contains(t.Name)
        );

    [Fact]
    public void EveryCustomCreature_HasALevelPin()
    {
        var unpinned = CustomCreatureTypes()
            .Where(t => !LevelConfig.MobLevelOverrides.ContainsKey(t))
            .Select(t => t.Name)
            .Order()
            .ToList();

        Assert.True(
            unpinned.Count == 0,
            $"Creatures missing a LevelConfig.MobLevelOverrides pin (silent mis-tier): {string.Join(", ", unpinned)}"
        );
    }

    [Fact]
    public void LootBagLevel_NeverDriftsMoreThanOneFromPin()
    {
        // GetUninitializedObject skips the ctor: LootBagLevel/EliteBagLevel getters are
        // either constant expressions or a pin lookup by type, so no constructed world
        // state is needed — this stays a pure, fast test.
        var drifted = new List<string>();

        foreach (var t in CustomCreatureTypes())
        {
            if (!LevelConfig.MobLevelOverrides.TryGetValue(t, out var pin))
            {
                continue; // covered by the pin test
            }

            var blank = (BaseCreature)RuntimeHelpers.GetUninitializedObject(t);

            var bag = blank is DungeonElite elite ? elite.EliteBagLevel : blank.LootBagLevel;

            if (Math.Abs(bag - pin) > 1)
            {
                drifted.Add($"{t.Name}: bag {bag} vs pin {pin}");
            }
        }

        Assert.True(
            drifted.Count == 0,
            $"Bag level drifted >1 from pin (typo or missed retune): {string.Join(", ", drifted)}"
        );
    }
}
