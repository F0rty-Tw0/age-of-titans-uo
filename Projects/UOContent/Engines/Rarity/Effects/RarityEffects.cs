using System;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;
using Server.Collections;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Text;

namespace Server.Engines.Rarity;

// Public entry point + combat interpreter for the rarity effects engine.
//
// The loot roller (a later phase) calls ApplyVariant/ApplyLegendary. BaseWeapon calls the
// combat hooks (AdjustSwingDelay/AdjustHitChance/BeginWeaponHit/EndWeaponHit/AbsorbForDefender)
// on the pre-AOS/T2A melee path. Armor/jewelry passive effects hook through OnWornAdded/
// OnWornRemoved (no-op stubs until P3). Every combat hook returns immediately when the weapon
// carries no variant, so the cost on a plain weapon is a single field compare.
public static partial class RarityEffects
{
    private static readonly TimeSpan MarkDuration = TimeSpan.FromSeconds(5);

    // Fallback "+% damage taken" for a mark whose lane row carries no MarkBonusPct — e.g. a
    // mark-family unique retargeted onto a crit lane by the arming-group split (2026-07-12).
    // Without it those marks would apply +0% (a dead effect) and read "for bonus damage".
    // Both the combat dispatch (ApplyMark) and the tooltip (InjectMarkBonus) use this so the
    // number shown always matches the number applied.
    internal const int DefaultMarkBonusPct = 15;

    // Re-entrancy guard depth: a normal extra swing must not itself proc another (mirrors
    // BaseWeapon.InDoubleStrike). ExtraSwingChain relaxes this to depth 2 — its chained swing
    // may proc ONE further swing — so the guard is a depth counter, not a bool.
    private static int _extraSwingDepth;
    public static bool InExtraSwing => _extraSwingDepth > 0;

    // Extra swings and ripostes land at reduced strength (2026-07-11 balance pass): a granted
    // swing is a FULL free hit, and at 100% the cadence extra-swing lanes (Aiolos/Horme/Zephyr)
    // measured +25-60% DPS over their family packs while paying an 8-10% rider's budget. One
    // scalar prices the whole mechanic class; the swing still animates, procs, and leeches.
    private const double ExtraSwingDamageScalar = 0.65;

    // Set only while a guaranteed extra swing (Antilochos/Peleus) is resolving; AdjustHitChance
    // honors it so that swing always lands. Cleared in the same finally block.
    private static bool ForceHit;

    // Set only while a DoubleStrikeEveryN extra swing (Ocypete/Podarkes/Kabeiros) is resolving;
    // BeginWeaponHit honors it so the second strike always crits. Same try/finally lifetime as
    // ForceHit above.
    private static bool ForceCrit;

    // ---- Public API ---------------------------------------------------------------------

    // Assigns a root drop-variant at a rarity: sets root, rarity (clamped), hue, and single-click
    // name. Weapon roots may only go on weapons, armor roots on armor, and so on.
    public static void ApplyVariant(Item item, VariantRoot root, ItemRarity rarity)
    {
        if (item is not IVariantItem variant)
        {
            throw new ArgumentException("Item does not support rarity variants.", nameof(item));
        }

        if (root == VariantRoot.None)
        {
            ClearVariant(item);
            return;
        }

        ValidateRootForItem(root, item);

        var clamped = RaritySystem.Clamp(rarity, ((IRarity)item).MaxRarity);

        // Legendary rarity → resolve the matching named legendary entry.
        // Without this, ApplyVariant at Legendary creates a generic "Kaminos Platemail Gorget
        // [Legendary]" with root effects but no unique legendary clause or proper name.
        if (clamped == ItemRarity.Legendary)
        {
            foreach (var candidate in LegendaryRegistry.Entries)
            {
                if (candidate.Root != root)
                {
                    continue;
                }

                try
                {
                    ValidateFamilyForItem(candidate, item);
                    // Found a compatible legendary — delegate entirely.
                    ApplyLegendary(item, candidate.Id);
                    return;
                }
                catch (ArgumentException)
                {
                    // Item type doesn't match this entry; keep scanning.
                }
            }
            // No matching legendary entry found — fall through to generic variant below.
        }

        // A re-applied theme must not inherit the previous themed name — BuildRootName seeds
        // from item.Name, so "Zephyr Katana" would become "Zephyr Zephyr Katana". Reset to the
        // stock name (LabelNumber) whenever the item was already themed (variant re-roll, tier
        // upgrade, ex-legendary). ApplyLegendary needs no twin: it sets entry.Name outright.
        if (variant.VariantRoot != VariantRoot.None || variant.LegendaryId != 0)
        {
            item.Name = null;
        }

        variant.VariantRoot = root;
        variant.LegendaryId = 0;
        ((IRarity)item).Rarity = clamped;

        if (root == VariantRoot.Olympian && item is BaseJewel olympianJewel)
        {
            olympianJewel.OlympianStat = RollOlympianStat();
        }

        item.Hue = VariantRootInfo.GetBodyHue(root, clamped);
        item.Name = BuildRootName(item, root);
        ResetArmorWeight(item);
        item.InvalidateProperties();
    }

    // Assigns a specific legendary by id: sets its root, rarity=Legendary, hue, and proper-noun name.
    public static void ApplyLegendary(Item item, ushort legendaryId)
    {
        if (item is not IVariantItem variant)
        {
            throw new ArgumentException("Item does not support rarity variants.", nameof(item));
        }

        if (!LegendaryRegistry.TryGet(legendaryId, out var entry))
        {
            throw new ArgumentException($"Unknown legendary id {legendaryId}.", nameof(legendaryId));
        }

        ValidateRootForItem(entry.Root, item);
        ValidateFamilyForItem(entry, item);

        var rarity = RaritySystem.Clamp(ItemRarity.Legendary, ((IRarity)item).MaxRarity);

        variant.VariantRoot = entry.Root;
        variant.LegendaryId = legendaryId;
        ((IRarity)item).Rarity = rarity;

        if (entry.Root == VariantRoot.Olympian && item is BaseJewel olympianJewel)
        {
            olympianJewel.OlympianStat = RollOlympianStat();
        }

        item.Hue = entry.Hue != 0 ? entry.Hue : VariantRootInfo.GetBodyHue(entry.Root, ItemRarity.Legendary);
        item.Name = entry.Name;
        ResetArmorWeight(item);
        item.InvalidateProperties();
    }

    // Reverts an item to a plain, un-themed state.
    public static void ClearVariant(Item item)
    {
        if (item is not IVariantItem variant)
        {
            return;
        }

        variant.VariantRoot = VariantRoot.None;
        variant.LegendaryId = 0;
        ((IRarity)item).Rarity = ItemRarity.Common;
        item.Hue = 0;
        item.Name = null;
        ResetArmorWeight(item);
        item.InvalidateProperties();
    }

    // The light-armor "weight" lane no longer shaves the piece's own weight — it now boosts the
    // wearer's carry capacity (WornEffectState aggregate -> PlayerMobile.MaxWeight), per the
    // 2026-07-12 directive. This reset keeps the item at its true DefaultWeight on every
    // Apply/Clear, normalizing any piece that had a reduced weight persisted under the old model.
    private static void ResetArmorWeight(Item item)
    {
        if (item is BaseArmor armor)
        {
            armor.Weight = armor.DefaultWeight;
        }
    }

    // Armor/shield legendaries are restricted to the material family their name implies (e.g.
    // Kekrops is a ring-mail legendary, never plate) — weapon families keep the existing lax
    // behavior (no base-shape check) since that precedent predates P3a and isn't in scope here.
    private static void ValidateFamilyForItem(LegendaryEntry entry, Item item)
    {
        if (entry.Family == LegendaryRegistry.FamilyShields)
        {
            if (item is not BaseShield)
            {
                throw new ArgumentException($"Legendary {entry.Name} is a shield legendary.", nameof(item));
            }

            return;
        }

        if (entry.Family == LegendaryRegistry.FamilyJewelry)
        {
            if (item is not BaseJewel jewel)
            {
                throw new ArgumentException($"Legendary {entry.Name} is a jewelry legendary.", nameof(item));
            }

            var expectedLayer = entry.BaseIndex switch
            {
                LegendaryRegistry.JewelrySlotRing => Layer.Ring,
                LegendaryRegistry.JewelrySlotBracelet => Layer.Bracelet,
                LegendaryRegistry.JewelrySlotNecklace => Layer.Neck,
                _ => Layer.Earrings
            };

            if (jewel.Layer != expectedLayer)
            {
                throw new ArgumentException($"Legendary {entry.Name} is bound to a different jewelry slot.", nameof(item));
            }

            return;
        }

        if (entry.Family == LegendaryRegistry.FamilyClothing)
        {
            var boundToCorrectPiece = entry.BaseIndex switch
            {
                LegendaryRegistry.ClothingPieceBodySash => item is BodySash,
                LegendaryRegistry.ClothingPieceFancyShirt => item is FancyShirt,
                LegendaryRegistry.ClothingPieceKilt => item is Kilt,
                LegendaryRegistry.ClothingPieceRobe => item is Robe,
                LegendaryRegistry.ClothingPieceCloak => item is Cloak,
                LegendaryRegistry.ClothingPieceStrawHat => item is StrawHat,
                LegendaryRegistry.ClothingPieceWideBrimHat => item is WideBrimHat,
                LegendaryRegistry.ClothingPieceFeatheredHat => item is FeatheredHat,
                LegendaryRegistry.ClothingPieceCap => item is Cap,
                _ => false
            };

            if (!boundToCorrectPiece)
            {
                throw new ArgumentException($"Legendary {entry.Name} is bound to a different clothing piece.", nameof(item));
            }

            return;
        }

        if (entry.Family <= LegendaryRegistry.FamilyArchery)
        {
            // Weapon families (0-6): the item must be a weapon whose concrete type maps to this
            // family. An unmapped/subclassed weapon type is left to ValidateRootForItem's checks.
            if (item is BaseWeapon && WeaponFamilyMap.TryGetFamily(item, out var weaponFamily) &&
                weaponFamily != entry.Family)
            {
                throw new ArgumentException(
                    $"Legendary {entry.Name} does not match weapon family for {item.GetType().Name}.", nameof(item)
                );
            }

            return;
        }

        if (item is not BaseArmor armor)
        {
            return; // already rejected by ValidateRootForItem
        }

        var ok = entry.Family == LegendaryRegistry.FamilyMetalArmor
            ? armor.MaterialType is ArmorMaterialType.Ringmail or ArmorMaterialType.Chainmail or ArmorMaterialType.Plate
            : armor.MaterialType is ArmorMaterialType.Leather or ArmorMaterialType.Studded or ArmorMaterialType.Bone;

        if (!ok)
        {
            throw new ArgumentException(
                $"Legendary {entry.Name} does not match {armor.MaterialType} material.", nameof(item)
            );
        }
    }

    // Olympian: "+N to one stat rolled at creation" (20-jewelry.md §2) — rolled fresh every time
    // the item becomes (or re-becomes) an Olympian variant/legendary.
    private static StatType RollOlympianStat() => Utility.Random(3) switch
    {
        0 => StatType.Str,
        1 => StatType.Dex,
        _ => StatType.Int
    };

    private static string BuildRootName(Item item, VariantRoot root)
    {
        // Title-case the base shape ("double axe" -> "Double Axe") so the full name reads as a
        // title beside the capitalized root, e.g. "Menis Double Axe". ToLower first so a mixed/
        // upper-case source normalizes before Capitalize title-cases each word.
        var baseName = (item.Name ?? Localization.GetText(item.LabelNumber))?.ToLowerInvariant().Capitalize();
        // Root display names are stored lowercase; capitalize so the name reads "Dryas Leather Tunic".
        var rootName = VariantRootInfo.GetDisplayName(root).Capitalize();

        return string.IsNullOrEmpty(baseName) ? rootName : $"{rootName} {baseName}";
    }

    // Set immediately before AbsorbDamage runs (BaseWeapon.OnHit) so the armor/shield absorb
    // layer below — which has no crit context of its own — can read whether this incoming hit
    // was a crit. Mirrors the ForceHit pending-flag pattern above (guaranteed extra swings).
    private static bool _pendingHitCrit;

    public static void SetPendingHitCrit(bool isCrit) => _pendingHitCrit = isCrit;

    private static bool ConsumePendingHitCrit()
    {
        var v = _pendingHitCrit;
        _pendingHitCrit = false;
        return v;
    }

    // Set by AbsorbForDefenderArmor when a hit is shrugged (halved). BaseWeapon.OnHit consumes it
    // right before the main AOS.Damage to fold "Shrugged" into that hit's damage float (rather than
    // a separate line) — mirrors the crit-context flow. The shrug riders' own reflect/flame damage
    // runs earlier inside AbsorbForDefenderArmor, so it never inherits this display context.
    private static bool _pendingShrugDisplay;

    public static bool ConsumePendingShrugDisplay()
    {
        var v = _pendingShrugDisplay;
        _pendingShrugDisplay = false;
        return v;
    }

    // Same flow as shrug, for a successful shield parry (BaseShield.OnHit). Folds "Parried" into
    // the surviving damage number ("-1 Parried" on a full block, floored to 1 pre-AOS).
    private static bool _pendingParryDisplay;

    public static bool ConsumePendingParryDisplay()
    {
        var v = _pendingParryDisplay;
        _pendingParryDisplay = false;
        return v;
    }

    // P27 armor-pen % for the current hit, stashed by BeginWeaponHit (0 = none, 100 = ignore AR
    // entirely) and consumed exactly once by BaseWeapon's AR-absorb step. BeginWeaponHit rewrites
    // it every hit, so a hit that never reaches the absorb path cannot leak pen to the next hit.
    private static int _pendingArmorPen;

    private static void SetPendingArmorPen(int pct) => _pendingArmorPen = pct;

    public static int ConsumePendingArmorPen()
    {
        var v = _pendingArmorPen;
        _pendingArmorPen = 0;
        return v;
    }

    // ---- Internals ----------------------------------------------------------------------

    // Internal (not private) so WornEffectState can resolve the same (root, rarity) pair when
    // rebuilding a wearer's aggregate from their worn items.
    internal static (VariantRoot root, ItemRarity rarity) ResolveRootRarity(IVariantItem variant, ItemRarity rarity)
    {
        if (variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry))
        {
            return (entry.Root, ItemRarity.Legendary);
        }

        return (variant.VariantRoot, rarity);
    }

    private static bool IsUnderHpFraction(Mobile m, double fraction) =>
        m.HitsMax > 0 && m.Hits < m.HitsMax * fraction;

    private static bool IsFullHp(Mobile m) => m.Hits >= m.HitsMax;

    // B3 stacking-waste fix: apply an on-kill stat restore (S/M/L), spilling any portion that can't
    // land (the stat is already at max — e.g. a 25% stamina restore firing in the same kill as a
    // full-stamina restore) into HEALTH at 50% rate rather than wasting it. Floats show the real
    // deltas only. Health is the spill target, so an over-restore of HP simply caps (no re-spill).
    // `intended` is the amount THIS restore attempts to add (a full restore passes its headroom).
    // Internal so the spill math can be unit-tested directly (the on-kill call sites are private).
    internal static void RestoreWithSpill(Mobile m, char kind, int intended)
    {
        if (m == null || intended <= 0)
        {
            return;
        }

        int cur, max;

        switch (kind)
        {
            case 'S':
                {
                    cur = m.Stam;
                    max = m.StamMax;
                    break;
                }
            case 'M':
                {
                    cur = m.Mana;
                    max = m.ManaMax;
                    break;
                }
            default: // 'L' — life
                {
                    cur = m.Hits;
                    max = m.HitsMax;
                    break;
                }
        }

        var applied = Math.Min(intended, Math.Max(0, max - cur));

        if (applied > 0)
        {
            switch (kind)
            {
                case 'S':
                    {
                        m.Stam = cur + applied;
                        break;
                    }
                case 'M':
                    {
                        m.Mana = cur + applied;
                        break;
                    }
                default:
                    {
                        m.Hits = cur + applied;
                        break;
                    }
            }

            FloatingCombatText.ShowRestore(m, kind, applied);
        }

        var wasted = intended - applied;

        if (wasted <= 0 || kind == 'L')
        {
            return;
        }

        var life = wasted / 2;

        if (life <= 0)
        {
            return;
        }

        var before = m.Hits;
        m.Hits = Math.Min(m.HitsMax, m.Hits + life);
        var gained = m.Hits - before;

        if (gained > 0)
        {
            FloatingCombatText.ShowRestore(m, 'L', gained);
        }
    }

    private static void ValidateRootForItem(VariantRoot root, Item item)
    {
        var ok = root switch
        {
            // Legacy axe roots stay lenient (any weapon): the re-theme migrates non-axe weapons off
            // them on load, and the loot roller only puts them on axes. The 30 new weapon roots
            // below are family-locked (a phoibos root on an axe throws) via WeaponFamilyMap.
            VariantRoot.Zephyr or VariantRoot.Phobos or VariantRoot.Agrotera or VariantRoot.Pallas
                or VariantRoot.Stygian => item is BaseWeapon,
            VariantRoot.Aegis => item is BaseShield,
            // Polias is the Athena bulwark root for non-shield armor only — shields use Aegis
            // instead (framework §3). BaseShield : BaseArmor, so this must exclude it explicitly.
            VariantRoot.Polias => item is BaseArmor and not BaseShield,
            VariantRoot.Cyclopean or VariantRoot.Paean or VariantRoot.Tritonian or VariantRoot.Talarian =>
                item is BaseArmor,
            VariantRoot.Olympian or VariantRoot.Hecatean or VariantRoot.Tychean or VariantRoot.Nyxian
                or VariantRoot.Demetrian => item is BaseJewel,
            VariantRoot.Laurel or VariantRoot.Charis or VariantRoot.Maenad or VariantRoot.Hestian
                or VariantRoot.Arachne => item is BaseClothing,
            // Shield-only roots (framework §3 re-theme — Aegis handled above).
            VariantRoot.Amyntor or VariantRoot.Probolos or VariantRoot.Herkos or VariantRoot.Pnoe =>
                item is BaseShield,
            _ when TryGetArmorRootMaterial(root, out var material) =>
                item is BaseArmor armor && armor is not BaseShield && armor.MaterialType == material,
            _ when WeaponFamilyMap.IsWeaponRoot(root) => WeaponRootMatchesItemFamily(root, item),
            _ => false
        };

        if (!ok)
        {
            throw new ArgumentException($"Root {root} is not valid for item type {item.GetType().Name}.", nameof(root));
        }
    }

    // Per-material armor roots (framework §3 re-theme): a material-locked root is valid only on
    // body armor of exactly that material — a naias plate chest throws. Returns false for any root
    // that is not one of the 30 material roots. The root->material mapping is sourced from the
    // armor family definitions (Families/*.cs) via the registry.
    private static bool TryGetArmorRootMaterial(VariantRoot root, out ArmorMaterialType material) =>
        FamilyRegistry.TryGetArmorMaterial(root, out material);

    // A per-family weapon root (Phoibos, Rhaistes, …) is valid only on a weapon whose concrete type
    // maps to the same family (framework §3). An unmapped/subclassed weapon type fails closed.
    private static bool WeaponRootMatchesItemFamily(VariantRoot root, Item item) =>
        item is BaseWeapon
        && WeaponFamilyMap.TryGetRootFamily(root, out var rootFamily)
        && WeaponFamilyMap.TryGetFamily(item, out var itemFamily)
        && rootFamily == itemFamily;

    // Per-hit context flowing from BeginWeaponHit to EndWeaponHit. Carries BOTH clause slots: the
    // lane Signature (S1-3) and the legendary's unique Clause (P1-3). HitCount/RampReachedMax let
    // the post-hit stage resolve cadence- and ramp-gated signatures without re-reading state.
    public readonly struct WeaponHitContext
    {
        public bool Active { get; }
        public VariantRoot Root { get; }
        public ItemRarity Rarity { get; }
        public WeaponEffectRow Row { get; }
        public ClauseType Clause { get; }
        public short P1 { get; }
        public short P2 { get; }
        public short P3 { get; }
        public bool IsCrit { get; }
        public bool IsFirstHit { get; }
        public bool ExtraSwing { get; }
        public int DamageBonusPercent { get; }
        public ClauseType Signature { get; }
        public short S1 { get; }
        public short S2 { get; }
        public short S3 { get; }
        public int HitCount { get; }
        public bool RampReachedMax { get; }

        public WeaponHitContext(
            VariantRoot root, ItemRarity rarity, WeaponEffectRow row, ClauseType clause,
            short p1, short p2, short p3, bool isCrit, bool isFirstHit, bool extraSwing, int damageBonusPercent,
            ClauseType signature, short s1, short s2, short s3, int hitCount, bool rampReachedMax
        )
        {
            Active = true;
            Root = root;
            Rarity = rarity;
            Row = row;
            Clause = clause;
            P1 = p1;
            P2 = p2;
            P3 = p3;
            IsCrit = isCrit;
            IsFirstHit = isFirstHit;
            ExtraSwing = extraSwing;
            DamageBonusPercent = damageBonusPercent;
            Signature = signature;
            S1 = s1;
            S2 = s2;
            S3 = s3;
            HitCount = hitCount;
            RampReachedMax = rampReachedMax;
        }
    }
}
