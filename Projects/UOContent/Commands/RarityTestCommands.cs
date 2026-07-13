using System;
using System.Collections.Generic;
using Server.Engines.LootBags;
using Server.Engines.Rarity;
using Server.Items;
using Server.Targeting;

namespace Server.Commands;

// GM testing kit for the rarity system. The loot roller (a later phase) is the production path
// for generating variants/legendaries — these commands exist so every effect can be exercised
// in-game now (armor slot-sets, capstones, legendaries, stackable-poison riders).
public static class RarityTestCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("Legendary", AccessLevel.GameMaster, Legendary_OnCommand);
        CommandSystem.Register("GenVariant", AccessLevel.GameMaster, GenVariant_OnCommand);
        CommandSystem.Register("ClearVariant", AccessLevel.GameMaster, ClearVariant_OnCommand);
        CommandSystem.Register("GenArmorSet", AccessLevel.GameMaster, GenArmorSet_OnCommand);
        CommandSystem.Register("GenLegendaryFamily", AccessLevel.GameMaster, GenLegendaryFamily_OnCommand);
        CommandSystem.Register("GenVariantFamily", AccessLevel.GameMaster, GenVariantFamily_OnCommand);
        CommandSystem.Register("PantheonFxTest", AccessLevel.GameMaster, PantheonFxTest_OnCommand);
        CommandSystem.Register("GrantIchor", AccessLevel.GameMaster, GrantIchor_OnCommand);
    }

    // ---- Family name -> byte lookups -------------------------------------------------------

    private static readonly Dictionary<string, byte> _familyNames = new(StringComparer.OrdinalIgnoreCase)
    {
        { "axes", LegendaryRegistry.FamilyAxes },
        { "swords", LegendaryRegistry.FamilySwords },
        { "polearms", LegendaryRegistry.FamilyPolearms },
        { "maces", LegendaryRegistry.FamilyMaces },
        { "staves", LegendaryRegistry.FamilyStaves },
        { "fencing", LegendaryRegistry.FamilyFencing },
        { "daggers", LegendaryRegistry.FamilyFencing },
        { "archery", LegendaryRegistry.FamilyArchery },
        { "bows", LegendaryRegistry.FamilyArchery },
        { "metalarmor", LegendaryRegistry.FamilyMetalArmor },
        { "lightarmor", LegendaryRegistry.FamilyLightArmor },
        { "shields", LegendaryRegistry.FamilyShields },
        { "jewelry", LegendaryRegistry.FamilyJewelry },
        { "jewellery", LegendaryRegistry.FamilyJewelry },
        { "clothing", LegendaryRegistry.FamilyClothing },
    };

    // Resolve a name to (family, baseIndex). baseIndex = -1 means family-wide (all legendaries);
    // 0+ means filter to legendaries bound to that specific ladder type.
    // "dagger" → FamilyFencing + baseIndex 0 (just daggers).
    // "fencing" → FamilyFencing + baseIndex -1 (all fencing legendaries).
    // "plate"  → FamilyMetalArmor + baseIndex 2 (just plate, not ringmail/chainmail).
    private static bool TryResolve(string name, out byte family, out int baseIndex)
    {
        family = 0;
        baseIndex = -1;

        // Try as a weapon ladder type (specific weapon) — gives both family + baseIndex.
        foreach (var wf in FamilyRegistry.WeaponFamilies)
        {
            for (var i = 0; i < wf.LadderTypes.Length; i++)
            {
                if (string.Equals(wf.LadderTypes[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    family = wf.Family;
                    baseIndex = i;
                    return true;
                }
            }
        }

        // Try as a family name.
        if (_familyNames.TryGetValue(name, out family))
        {
            return true;
        }

        // Try as an armor material — returns the material's ladder index as baseIndex.
        foreach (var mf in FamilyRegistry.MetalArmorFamilies)
        {
            if (string.Equals(mf.Material.ToString(), name, StringComparison.OrdinalIgnoreCase))
            {
                family = mf.Family;
                baseIndex = mf.LadderIndex;
                return true;
            }
        }

        foreach (var lf in FamilyRegistry.LightArmorFamilies)
        {
            if (string.Equals(lf.Material.ToString(), name, StringComparison.OrdinalIgnoreCase))
            {
                family = lf.Family;
                baseIndex = lf.LadderIndex;
                return true;
            }
        }

        return false;
    }

    // Find a piece factory by name within an armor material's SetPieces (e.g. "gorget" → PlateGorget factory).
    private static Func<Item>? FindPieceFactory(byte family, int materialBaseIndex, string pieceName)
    {
        var families = family == LegendaryRegistry.FamilyMetalArmor
            ? FamilyRegistry.MetalArmorFamilies
            : FamilyRegistry.LightArmorFamilies;

        foreach (var f in families)
        {
            if (f.LadderIndex == materialBaseIndex)
            {
                foreach (var factory in f.SetPieces)
                {
                    if (PeekTypeName(factory).Contains(pieceName, StringComparison.OrdinalIgnoreCase))
                    {
                        return factory;
                    }
                }
            }
        }

        return null;
    }

    // Reads a factory's concrete type name. Item constructors register with the world (internal
    // map), so the probe MUST be deleted — a bare factory().GetType().Name leaks a persistent,
    // save-serialized item per call.
    private static string PeekTypeName(Func<Item> factory)
    {
        var probe = factory();
        var name = probe.GetType().Name;
        probe.Delete();
        return name;
    }

    // ---- [Legendary <id|name>] -------------------------------------------------------------

    [Usage("Legendary <id|name>")]
    [Description(
        "Targets an equippable item and mints it as the legendary with the given registry id " +
        "or exact name (see LegendaryRegistry). Sets rarity, root, name, hue and effects. Does " +
        "NOT enforce global uniqueness — an admin mint can duplicate an existing legendary."
    )]
    private static void Legendary_OnCommand(CommandEventArgs e)
    {
        if (e.Length != 1)
        {
            e.Mobile.SendMessage("Usage: [Legendary <id|name>");
            return;
        }

        LegendaryEntry entry = default;
        var found = ushort.TryParse(e.GetString(0), out var id) && LegendaryRegistry.TryGet(id, out entry);

        if (!found)
        {
            foreach (var candidate in LegendaryRegistry.Entries)
            {
                if (string.Equals(candidate.Name, e.GetString(0), StringComparison.OrdinalIgnoreCase))
                {
                    entry = candidate;
                    found = true;
                    break;
                }
            }
        }

        if (!found)
        {
            e.Mobile.SendMessage($"No legendary matches '{e.GetString(0)}' (registry id or exact name).");
            return;
        }

        e.Mobile.SendMessage($"Target the item to mint as {entry.Name} (#{entry.Id}).");
        e.Mobile.Target = new LegendaryTarget(entry.Id);
    }

    private sealed class LegendaryTarget : Target
    {
        private readonly ushort _id;

        public LegendaryTarget(ushort id) : base(-1, false, TargetFlags.None) => _id = id;

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not Item item || item is not IVariantItem)
            {
                from.SendMessage("That item cannot hold a rarity variant (need a weapon, armor, jewel, or clothing).");
                return;
            }

            try
            {
                RarityEffects.ApplyLegendary(item, _id);
                LegendaryRegistry.TryGet(_id, out var entry);
                from.SendMessage($"Minted {entry.Name} (#{_id}).");
            }
            catch (ArgumentException ex)
            {
                from.SendMessage(ex.Message);
            }
        }
    }

    // ---- [GenLegendaryFamily <family|weaponType> [piece]] ----------------------------------

    [Usage("GenLegendaryFamily <family|weaponType> [piece]")]
    [Description(
        "Fills your backpack with EVERY legendary in the given family. " +
        "Weapon families (swords/axes/fencing/polearms/maces/staves/archery) + optional weapon type " +
        "(dagger/katana etc.) → all legendaries for that family or base type. " +
        "Armor materials (plate/chainmail/ringmail/leather/studded/bone) → one sub-bag per piece type " +
        "with every legendary on that piece (5 legendaries × 6 plate pieces = 30 items). " +
        "Armor + piece name (gorget/helm/chest/arms/gloves/legs) → all legendaries on that one piece. " +
        "Examples: GenLegendaryFamily fencing / GenLegendaryFamily plate / GenLegendaryFamily plate gorget"
    )]
    private static void GenLegendaryFamily_OnCommand(CommandEventArgs e)
    {
        if (e.Length < 1 || !TryResolve(e.GetString(0), out var family, out var baseIndex))
        {
            e.Mobile.SendMessage("Usage: GenLegendaryFamily <family|weaponType> [piece]  " +
                "(e.g. GenLegendaryFamily fencing / GenLegendaryFamily dagger / GenLegendaryFamily plate gorget)");
            return;
        }

        var from = e.Mobile;
        var isArmor = family == LegendaryRegistry.FamilyMetalArmor || family == LegendaryRegistry.FamilyLightArmor;
        var entries = GetLegendaryEntries(family, baseIndex);

        if (entries.Count == 0)
        {
            from.SendMessage($"No legendaries found for {e.GetString(0)}.");
            return;
        }

        // Armor material + no piece → one sub-bag per piece type (all legendaries × all pieces).
        // Armor material + specific piece → one bag with all legendaries on that piece.
        // Weapon family / family-wide → one bag with all legendaries on random slots.
        if (isArmor && baseIndex >= 0 && e.Length < 2)
        {
            var mainBag = new Bag { Name = $"{e.GetString(0)} legendaries ({entries.Count} × all pieces)" };
            var pieceFactories = GetPieceFactories(family, baseIndex);
            var totalOk = 0;
            var totalFailed = 0;

            foreach (var pf in pieceFactories)
            {
                var pieceName = PeekTypeName(pf);
                var subBag = new Bag { Name = pieceName };
                var pieceOk = 0;

                foreach (var entry in entries)
                {
                    Item item = null;

                    try
                    {
                        item = pf();
                        RarityEffects.ApplyLegendary(item, entry.Id);
                        subBag.AddItem(item);
                        pieceOk++;
                    }
                    catch (Exception ex)
                    {
                        item?.Delete(); // constructed but never bagged — would leak on the internal map
                        from.SendMessage($"Failed to mint {entry.Name} on {pieceName}: {ex.Message}");
                        totalFailed++;
                    }
                }

                if (pieceOk > 0)
                {
                    mainBag.AddItem(subBag);
                    totalOk += pieceOk;
                }
                else
                {
                    subBag.Delete();
                }
            }

            if (totalOk > 0)
            {
                from.AddToBackpack(mainBag);
                from.SendMessage($"Added a bag with {totalOk} {e.GetString(0)} " +
                    $"legendaries across {pieceFactories.Length} piece types" +
                    (totalFailed > 0 ? $", {totalFailed} failed." : "."));
            }
            else
            {
                mainBag.Delete();
            }

            return;
        }

        // Single bag path: specific piece, weapon family, or family-wide.
        var pieceFactory = e.Length >= 2 && isArmor && baseIndex >= 0
            ? FindPieceFactory(family, baseIndex, e.GetString(1))
            : null;

        if (e.Length >= 2 && isArmor && baseIndex >= 0 && pieceFactory == null)
        {
            from.SendMessage($"No piece '{e.GetString(1)}' found for {e.GetString(0)}. " +
                "Try: helm, gorget, chest, arms, gloves, legs.");
            return;
        }

        if (e.Length >= 2 && !isArmor)
        {
            from.SendMessage("Piece argument is only valid for armor materials (plate/chainmail/ringmail/leather/studded/bone).");
            return;
        }

        var label = baseIndex >= 0
            ? $"{e.GetString(0)} legendaries ({entries.Count})"
            : $"{e.GetString(0)} family legendaries ({entries.Count})";
        var bag = new Bag { Name = label };
        var ok = 0;
        var failed = 0;

        foreach (var entry in entries)
        {
            Item item = null;

            try
            {
                item = pieceFactory != null ? pieceFactory() : LootRoller.ConstructForLegendary(entry);
                RarityEffects.ApplyLegendary(item, entry.Id);
                bag.AddItem(item);
                ok++;
            }
            catch (Exception ex)
            {
                item?.Delete(); // constructed but never bagged — would leak on the internal map
                from.SendMessage($"Failed to mint {entry.Name} (#{entry.Id}): {ex.Message}");
                failed++;
            }
        }

        if (ok > 0)
        {
            from.AddToBackpack(bag);
            from.SendMessage($"Added a bag with {ok} {e.GetString(0)} legendaries{(failed > 0 ? $", {failed} failed." : ".")}");
        }
        else
        {
            bag.Delete();
        }
    }

    // Collect legendary entries filtered by family + optional baseIndex.
    private static List<LegendaryEntry> GetLegendaryEntries(byte family, int baseIndex)
    {
        var entries = new List<LegendaryEntry>(LegendaryRegistry.Entries.Count);

        foreach (var entry in LegendaryRegistry.Entries)
        {
            if (entry.Family == family && (baseIndex < 0 || entry.BaseIndex == baseIndex))
            {
                entries.Add(entry);
            }
        }

        return entries;
    }

    // Return all piece factories for an armor material (SetPieces order).
    private static Func<Item>[] GetPieceFactories(byte family, int baseIndex)
    {
        var families = family == LegendaryRegistry.FamilyMetalArmor
            ? FamilyRegistry.MetalArmorFamilies
            : FamilyRegistry.LightArmorFamilies;

        foreach (var f in families)
        {
            if (f.LadderIndex == baseIndex)
            {
                return f.SetPieces;
            }
        }

        return Array.Empty<Func<Item>>();
    }

    // ---- [GenVariantFamily <family|weaponType> <rarity> [piece]] ---------------------------

    [Usage("GenVariantFamily <family|weaponType> <rarity> [piece]")]
    [Description(
        "Fills your backpack with EVERY root variant in the given family at the given rarity. " +
        "Weapons: all roots for that family or base type, each on a random weapon. " +
        "Armor materials (plate/bone/leather/etc.) → one sub-bag per piece type with every root " +
        "on that piece (e.g. 5 plate roots × 6 pieces = 30 Epic items). " +
        "Armor + piece (gorget/helm) → all roots on that one piece. " +
        "Examples: GenVariantFamily plate epic / GenVariantFamily bone gloves rare"
    )]
    private static void GenVariantFamily_OnCommand(CommandEventArgs e)
    {
        if (e.Length < 2 || !TryResolve(e.GetString(0), out var family, out var baseIndex) ||
            !Enum.TryParse<ItemRarity>(e.GetString(1), true, out var rarity))
        {
            e.Mobile.SendMessage("Usage: GenVariantFamily <family|weaponType> <rarity> [piece]  " +
                "(e.g. GenVariantFamily plate epic / GenVariantFamily bone gloves rare)");
            return;
        }

        var from = e.Mobile;
        var isArmor = family == LegendaryRegistry.FamilyMetalArmor || family == LegendaryRegistry.FamilyLightArmor;
        var roots = GetLaneRoots(family, baseIndex);

        if (roots.Length == 0)
        {
            from.SendMessage($"No roots found for {e.GetString(0)}.");
            return;
        }

        // Armor material + no piece → one sub-bag per piece type.
        if (isArmor && baseIndex >= 0 && e.Length < 3)
        {
            var pieceFactories = GetPieceFactories(family, baseIndex);

            if (pieceFactories.Length == 0)
            {
                from.SendMessage($"No piece factories found for {e.GetString(0)}.");
                return;
            }

            var mainBag = new Bag { Name = $"{e.GetString(0)} {rarity} ({roots.Length} × {pieceFactories.Length} pieces)" };
            var totalOk = 0;

            foreach (var pf in pieceFactories)
            {
                var pieceName = PeekTypeName(pf);
                var subBag = new Bag { Name = $"{pieceName} ({rarity})" };
                var pieceOk = 0;

                foreach (var root in roots)
                {
                    Item item = null;

                    try
                    {
                        item = pf();
                        RarityEffects.ApplyVariant(item, root, rarity);
                        subBag.AddItem(item);
                        pieceOk++;
                    }
                    catch (Exception ex)
                    {
                        item?.Delete(); // constructed but never bagged — would leak on the internal map
                        from.SendMessage($"Failed: {root} {rarity} on {pieceName}: {ex.Message}");
                    }
                }

                if (pieceOk > 0)
                {
                    mainBag.AddItem(subBag);
                    totalOk += pieceOk;
                }
                else
                {
                    subBag.Delete();
                }
            }

            if (totalOk > 0)
            {
                from.AddToBackpack(mainBag);
                from.SendMessage($"Added a bag with {totalOk} {e.GetString(0)} {rarity} variants across {pieceFactories.Length} piece types.");
            }
            else
            {
                mainBag.Delete();
            }

            return;
        }

        // Single bag path: specific piece, weapon family, or family-wide.
        Func<Item>? pieceFactory = null;

        if (e.Length >= 3 && isArmor && baseIndex >= 0)
        {
            pieceFactory = FindPieceFactory(family, baseIndex, e.GetString(2));

            if (pieceFactory == null)
            {
                from.SendMessage($"No piece '{e.GetString(2)}' found for {e.GetString(0)}.");
                return;
            }
        }
        else if (e.Length >= 3 && !isArmor)
        {
            from.SendMessage("Piece argument is only valid for armor materials.");
            return;
        }

        // Roots are material-locked, so a merged cross-material root list has no single piece
        // source that fits every root — reject up front instead of constructing doomed items.
        if (isArmor && baseIndex < 0)
        {
            from.SendMessage("Family-wide armor is not supported — use a specific material " +
                "(plate/chainmail/ringmail/leather/studded/bone).");
            return;
        }

        var factories = pieceFactory != null ? null : GetFamilyFactories(family, baseIndex);

        if (pieceFactory == null && factories.Length == 0)
        {
            from.SendMessage($"No item factories found for {e.GetString(0)}.");
            return;
        }

        var bagName = pieceFactory != null
            ? $"{e.GetString(0)} {e.GetString(2)} ({rarity})"
            : $"{e.GetString(0)} {rarity} ({roots.Length})";
        var bag = new Bag { Name = bagName };
        var ok = 0;

        foreach (var root in roots)
        {
            Item item = null;

            try
            {
                item = pieceFactory != null
                    ? pieceFactory()
                    : factories[Utility.Random(factories.Length)]();

                RarityEffects.ApplyVariant(item, root, rarity);
                bag.AddItem(item);
                ok++;
            }
            catch (Exception ex)
            {
                item?.Delete(); // constructed but never bagged — would leak on the internal map
                from.SendMessage($"Failed: {root} {rarity}: {ex.Message}");
            }
        }

        if (ok > 0)
        {
            from.AddToBackpack(bag);
            from.SendMessage($"Added a bag with {ok} {e.GetString(0)} {rarity} variants.");
        }
        else
        {
            bag.Delete();
        }
    }

    // Random-shape construction pool for the single-bag path — every family the root selector
    // can produce has a real item source (the old fallback built a bare Item(0x0), which
    // ApplyVariant rejects, leaking it on the internal map and yielding an empty bag for
    // shields/jewelry/clothing).
    private static Func<Item>[] GetFamilyFactories(byte family, int baseIndex) => family switch
    {
        <= LegendaryRegistry.FamilyArchery => FamilyRegistry.WeaponFamilies[family].Factories,
        LegendaryRegistry.FamilyMetalArmor or LegendaryRegistry.FamilyLightArmor => GetPieceFactories(family, baseIndex),
        LegendaryRegistry.FamilyShields => FamilyRegistry.ShieldFamilyDef.ShieldFactories,
        LegendaryRegistry.FamilyJewelry => FamilyRegistry.JewelryFamilyDef.Factories,
        LegendaryRegistry.FamilyClothing => FamilyRegistry.ClothingFamilyDef.Factories,
        _ => Array.Empty<Func<Item>>()
    };

    // Get all variant roots (lanes) for a family/material.
    private static VariantRoot[] GetLaneRoots(byte family, int baseIndex)
    {
        if (family <= LegendaryRegistry.FamilyArchery)
        {
            return FamilyRegistry.LaneRoots(FamilyRegistry.WeaponFamilies[family].Lanes);
        }

        if (family is LegendaryRegistry.FamilyMetalArmor or LegendaryRegistry.FamilyLightArmor)
        {
            var families = family == LegendaryRegistry.FamilyMetalArmor
                ? FamilyRegistry.MetalArmorFamilies
                : FamilyRegistry.LightArmorFamilies;

            // Family-wide: merge roots from all materials.
            if (baseIndex < 0)
            {
                var all = new List<VariantRoot>();

                foreach (var f in families)
                {
                    all.AddRange(FamilyRegistry.LaneRoots(f.Lanes));
                }

                return all.ToArray();
            }

            // Specific material.
            foreach (var f in families)
            {
                if (f.LadderIndex == baseIndex)
                {
                    return FamilyRegistry.LaneRoots(f.Lanes);
                }
            }
        }

        if (family == LegendaryRegistry.FamilyShields)
        {
            return FamilyRegistry.LaneRoots(FamilyRegistry.ShieldFamilyDef.Lanes);
        }

        if (family == LegendaryRegistry.FamilyJewelry)
        {
            return FamilyRegistry.LaneRoots(FamilyRegistry.JewelryFamilyDef.Lanes);
        }

        if (family == LegendaryRegistry.FamilyClothing)
        {
            return FamilyRegistry.LaneRoots(FamilyRegistry.ClothingFamilyDef.Lanes);
        }

        return Array.Empty<VariantRoot>();
    }

    // ---- [GrantIchor] ----------------------------------------------------------------------

    [Usage("GrantIchor [amount=100]")]
    [Description("Adds a stack of ichor (the altar salvage material) to your backpack for salvage/upgrade testing.")]
    private static void GrantIchor_OnCommand(CommandEventArgs e)
    {
        var amount = Math.Clamp(e.Length >= 1 ? e.GetInt32(0) : 100, 1, 60000);
        var from = e.Mobile;
        var ichor = new PantheonIchor(amount);

        if (!from.AddToBackpack(ichor))
        {
            ichor.MoveToWorld(from.Location, from.Map);
        }

        from.SendMessage($"Granted {amount} ichor.");
    }

    // ---- [PantheonFxTest] ------------------------------------------------------------------

    [Usage("PantheonFxTest")]
    [Description("Previews every pantheon domain's legendary proc flourish on you, 1.5s apart, ending with the devotion crown. For the in-client FX/sound verification pass.")]
    private static void PantheonFxTest_OnCommand(CommandEventArgs e)
    {
        var from = e.Mobile;
        var domains = Enum.GetValues<PantheonDomain>();

        for (var i = 0; i < domains.Length; i++)
        {
            var domain = domains[i];

            Timer.StartTimer(TimeSpan.FromSeconds(1.5 * i), () =>
            {
                if (from.Deleted || from.Map == null)
                {
                    return;
                }

                from.SendMessage($"{domain} — {PantheonFx.GetPatronName(domain)} ({PantheonFx.GetPerkText(domain)})");
                PantheonFx.PlayForDomain(from, domain, PantheonFx.SampleHue(domain), devoted: false);
            });
        }

        Timer.StartTimer(TimeSpan.FromSeconds(1.5 * domains.Length), () =>
        {
            if (from.Deleted || from.Map == null)
            {
                return;
            }

            from.SendMessage("Devotion crown (Sky domain, devoted)");
            PantheonFx.PlayForDomain(from, PantheonDomain.Sky, PantheonFx.SampleHue(PantheonDomain.Sky), devoted: true);
        });

        from.SendMessage("Pantheon FX preview started: 11 domains + devotion crown.");
    }

    // ---- [GenVariant] ----------------------------------------------------------------------

    [Usage("GenVariant <root> <rarity>")]
    [Description("Applies a root drop-variant at the given rarity to a targeted item, e.g. GenVariant Polias Epic.")]
    private static void GenVariant_OnCommand(CommandEventArgs e)
    {
        if (e.Length != 2 || !Enum.TryParse<VariantRoot>(e.GetString(0), true, out var root) ||
            !Enum.TryParse<ItemRarity>(e.GetString(1), true, out var rarity))
        {
            e.Mobile.SendMessage("Usage: GenVariant <root> <rarity>  (e.g. GenVariant Polias Epic)");
            return;
        }

        e.Mobile.SendMessage($"Target the item to become {root} {rarity}.");
        e.Mobile.BeginTarget(-1, false, TargetFlags.None, (from, targeted) =>
            {
                if (targeted is not Item item)
                {
                    from.SendMessage("That is not an item.");
                    return;
                }

                try
                {
                    RarityEffects.ApplyVariant(item, root, rarity);
                    from.SendMessage($"{item.GetType().Name} is now {root} {rarity}.");
                }
                catch (ArgumentException ex)
                {
                    from.SendMessage(ex.Message);
                }
            }
        );
    }

    // ---- [ClearVariant] --------------------------------------------------------------------

    [Usage("ClearVariant")]
    [Description("Reverts a targeted item to its plain, un-themed state.")]
    private static void ClearVariant_OnCommand(CommandEventArgs e)
    {
        e.Mobile.SendMessage("Target the item to clear.");
        e.Mobile.BeginTarget(-1, false, TargetFlags.None, (from, targeted) =>
            {
                if (targeted is not Item item)
                {
                    from.SendMessage("That is not an item.");
                    return;
                }

                RarityEffects.ClearVariant(item);
                from.SendMessage($"{item.GetType().Name} cleared.");
            }
        );
    }

    // ---- [GenArmorSet] ---------------------------------------------------------------------

    // Round-robin index: advances each call so a no-root GenArmorSet gives a different
    // uniform root each time (Adamas → Kaminos → ... → Akamatos → Adamas → ...).
    private static int _rrIndex;

    [Usage("GenArmorSet <material> [rarity=Epic] [root]")]
    [Description("Fills your backpack with a full armor set of the given material (Leather/Studded/Bone/Ringmail/Chainmail/Plate). Without a root, the material's five thematic roots advance round-robin per call — each call gives a uniform set of the next root (Adamas → Kaminos → ...); pass a root for an explicit uniform set.")]
    private static void GenArmorSet_OnCommand(CommandEventArgs e)
    {
        if (e.Length < 1 || !Enum.TryParse<ArmorMaterialType>(e.GetString(0), true, out var material))
        {
            e.Mobile.SendMessage("Usage: GenArmorSet <material> [rarity=Epic] [root]");
            return;
        }

        var rarity = ItemRarity.Epic;

        if (e.Length >= 2 && !Enum.TryParse(e.GetString(1), true, out rarity))
        {
            e.Mobile.SendMessage($"Unknown rarity '{e.GetString(1)}'.");
            return;
        }

        var uniformRoot = VariantRoot.None;

        if (e.Length >= 3 && !Enum.TryParse(e.GetString(2), true, out uniformRoot))
        {
            e.Mobile.SendMessage($"Unknown root '{e.GetString(2)}'.");
            return;
        }

        if (!FamilyRegistry.ArmorFamilyByMaterial.TryGetValue(material, out var def))
        {
            e.Mobile.SendMessage($"{material} is not a slot-set material (use Leather/Studded/Bone/Ringmail/Chainmail/Plate).");
            return;
        }

        var materialRoots = FamilyRegistry.LaneRoots(def.Lanes);
        
        if (uniformRoot == VariantRoot.None)
        {
            uniformRoot = materialRoots[_rrIndex % materialRoots.Length];
            _rrIndex++;
        }

        var pieces = new Item[def.SetPieces.Length];

        for (var i = 0; i < pieces.Length; i++)
        {
            pieces[i] = def.SetPieces[i]();
        }

        var bag = new Bag
        {
            Name = $"{uniformRoot} {material} armor set"
        };

        for (var i = 0; i < pieces.Length; i++)
        {
            var piece = pieces[i];
            var root = uniformRoot;

            try
            {
                RarityEffects.ApplyVariant(piece, root, rarity);
            }
            catch (ArgumentException ex)
            {
                piece.Delete();
                e.Mobile.SendMessage($"{piece.GetType().Name}: {ex.Message}");
                continue;
            }

            bag.AddItem(piece);
        }

        e.Mobile.AddToBackpack(bag);

        var rootLabel = uniformRoot != VariantRoot.None ? uniformRoot.ToString() : "mixed-root";
        e.Mobile.SendMessage($"Added a bag of {pieces.Length} {rootLabel} {rarity} {material} pieces to your pack.");
    }
}
