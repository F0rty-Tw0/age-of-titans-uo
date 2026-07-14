using System;
using System.Collections.Generic;
using Server.Engines.Rarity;
using Server.Mobiles;

namespace Server.Engines.LootBags;

// Creature family -> pantheon domain (dev-docs/itemization/30-pantheon-bags.md §3). Drives the
// 70/30 themed-bag roll at the BaseCreature drop site and DungeonElite's always-themed bags.
// Same prefix idiom as UOContent.Tests BestiaryGuardTests, plus a named-single override for
// bosses that carry no family prefix of their own (Enkelados, Aiakos, ...).
public static class PantheonLootMap
{
    // Type names that merely collide with a family prefix but aren't ours (same list as
    // BestiaryGuardTests._stockCollisions) — must never resolve despite the prefix match.
    private static readonly HashSet<string> _stockCollisions = new()
    {
        "Cursed", "CursedSoul", "RestlessSoul", "Pyre"
    };

    private static readonly (string Prefix, PantheonDomain Domain)[] _prefixes =
    {
        ("Tide", PantheonDomain.Sea),
        ("Brine", PantheonDomain.Sea),
        ("Shore", PantheonDomain.Sea),
        ("Cinder", PantheonDomain.Forge),
        ("Wyld", PantheonDomain.Hunt),
        ("Lykai", PantheonDomain.Hunt),
        ("Storm", PantheonDomain.Sky),
        ("Peak", PantheonDomain.Sky),
        ("Stygian", PantheonDomain.Underworld),
        ("Drowned", PantheonDomain.Underworld),
        ("Restless", PantheonDomain.Underworld),
        ("Newbie", PantheonDomain.Underworld),
        ("Gaian", PantheonDomain.Nature),
        ("Grove", PantheonDomain.Nature),
        ("Pelasg", PantheonDomain.Nature),
        ("Tartarus", PantheonDomain.War),
        ("Drakon", PantheonDomain.War),
        ("Myrmi", PantheonDomain.War),
        ("Wayman", PantheonDomain.War),
        ("Cursed", PantheonDomain.Night),
        ("Mire", PantheonDomain.Night),
        ("Ophian", PantheonDomain.Night),
        ("Rime", PantheonDomain.Wind),
        ("Argus", PantheonDomain.Aegis),
        ("Pyre", PantheonDomain.Sun)
    };

    // Named singles that don't carry a family prefix at all — resolved by exact Type.
    private static readonly Dictionary<Type, PantheonDomain> _namedSingles = new()
    {
        [typeof(Enkelados)] = PantheonDomain.Nature,
        [typeof(Chthonios)] = PantheonDomain.Nature,
        [typeof(Aiakos)] = PantheonDomain.Underworld,
        [typeof(Minos)] = PantheonDomain.Underworld,
        [typeof(Thaumas)] = PantheonDomain.Sea,
        [typeof(Glaukos)] = PantheonDomain.Sea,
        [typeof(Ladon)] = PantheonDomain.War,
        [typeof(Pythios)] = PantheonDomain.War,
        [typeof(Alastor)] = PantheonDomain.War,
        [typeof(Eurynomos)] = PantheonDomain.War
    };

    // Resolved once per Type, then it's a single dict lookup per kill (single-threaded server,
    // no concurrency primitives needed). Null cached value = confirmed generic (no domain).
    private static readonly Dictionary<Type, PantheonDomain?> _cache = new();

    public static bool TryGetDomain(BaseCreature creature, out PantheonDomain domain) =>
        TryGetDomain(creature.GetType(), out domain);

    public static bool TryGetDomain(Type type, out PantheonDomain domain)
    {
        if (!_cache.TryGetValue(type, out var resolved))
        {
            resolved = Resolve(type);
            _cache[type] = resolved;
        }

        domain = resolved ?? default;
        return resolved.HasValue;
    }

    private static PantheonDomain? Resolve(Type type)
    {
        if (_namedSingles.TryGetValue(type, out var named))
        {
            return named;
        }

        var name = type.Name;

        if (_stockCollisions.Contains(name))
        {
            return null;
        }

        foreach (var (prefix, domain) in _prefixes)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return domain;
            }
        }

        return null;
    }
}
