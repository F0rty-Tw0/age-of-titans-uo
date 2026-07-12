using System;
using System.Collections.Generic;
using Server.Collections;
using Server.Engines.BuffIcons;
using Server.Items;
using Server.Misc;

namespace Server.Engines.Rarity;

// Suit-wide totals after framework §9.4 stacking (strongest same-root instance counts full
// weight, every other same-root instance counts at half weight) and the §9.8 shared-pool
// clamps. Bonus AR and per-piece weight reduction are NOT included here — both are applied
// directly on the item itself (see RarityEffects.GetBonusArmorRating / ApplyVariant), per the
// P3a architecture call. Flagged: framework §9.8 also lists "bonus AR ≤ 15" as a suit-wide cap,
// which per-piece application does not enforce; a full Polias suit can exceed it. Not
// reconciled here — surfaced for the balance pass.
//
// P3b extends this aggregate with jewelry (Olympian/Hecatean/Tychean/Nyxian/Demetrian) and
// clothing (Laurel/Charis/Maenad/Hestian/Arachne) fields. Two P3a mechanics are reused rather
// than duplicated: Tychean's "incoming hit halved" feeds the SAME ShrugPct field/cap as
// Polias/Aegis shrug (mechanically identical roll), and Arachne's dodge feeds the SAME DodgePct
// field/cap as Talarian dodge (framework §9.8's shared pools).
public readonly struct WornAggregate
{
    public int DrPct { get; init; }
    public int ShrugPct { get; init; }
    public int ReflectPct { get; init; }
    public int FlameProcPct { get; init; }
    public int HpRegenPct { get; init; }
    public int HealsReceivedPct { get; init; }
    public bool AutoCure { get; init; }
    public int SpellDrPct { get; init; }
    public int ParaResistPct { get; init; }
    public int ResistSkillBonus { get; init; }
    public int StamRegenPct { get; init; }
    public int DodgePct { get; init; }
    public int ParryPct { get; init; }
    public int ParryDrPct { get; init; }
    public bool ParryThorns { get; init; }

    // Light-armor "weight" lane: a % boost to the wearer's carry capacity (PlayerMobile.MaxWeight),
    // suit-capped at CarryWeightCap. Replaces the old per-piece weight shave (2026-07-12).
    public int CarryWeightBonusPct { get; init; }

    // ---- P3b: jewelry ----
    public int LightningProcPct { get; init; }
    public int ManaRegenPct { get; init; }
    public int SpellDamagePct { get; init; }
    public int ManaLeechPct { get; init; }
    public int MissRerollPct { get; init; }
    public int HidingBonus { get; init; }
    public int StealthBonus { get; init; }
    public bool NightSight { get; init; }
    public int PoisonResistPct { get; init; }
    public int PotionEffectPct { get; init; }

    // ---- P3b: clothing ----
    public int OnKillStamina { get; init; }
    public int OnKillHp { get; init; }
    public int KarmaGainPct { get; init; }
    public int VendorPricePct { get; init; }
    public int FrenzyChancePct { get; init; }
    public int FrenzyDamagePct { get; init; }
    public int FrenzySwingPct { get; init; }
    public int StationaryRegenPct { get; init; }
    public bool StationaryAppliesMana { get; init; }

    // ---- P4: armor slot-set capstone. At most one set can be complete at a time (the six
    // body slots can't satisfy two materials' thresholds), so a single material field suffices.
    public bool HasCapstone { get; init; }
    public ArmorMaterialType CapstoneMaterial { get; init; }

    // ---- P5b: Divine Resonance. When two worn pieces carry the SAME clause, only one may
    // dispatch (DedupeClauses) — but the duplicate is not wasted: it "resonates", echoing as a
    // small category bonus. Offensive duplicates add ResonanceDamagePct to the wielder's hits;
    // defensive duplicates fed +2% DR each and utility duplicates +10% regen each into the
    // §9.8-capped pools during Rebuild (so the framework ceilings stay law). The counts are kept
    // for the buff-bar readout; at most 2 duplicates per category resonate.
    public int ResonanceOffense { get; init; }
    public int ResonanceDefense { get; init; }
    public int ResonanceUtility { get; init; }
    public int ResonanceDamagePct { get; init; }

    // The full Divine Resonance buff-bar text (with per-item source names), rebuilt every equip
    // change. SyncResonanceBuff keys on THIS rather than the raw counts, so a same-count source
    // swap (Part B5's equip bug) still refreshes the buff instead of leaving stale text.
    public string ResonanceText { get; init; }

    // ---- P6: Patron God devotion (Part B4: stacks ACROSS domains). Each pantheon domain with
    // >=3 worn legendaries pledges the wearer to that god; DevotionMask/ExarchMask are the
    // per-domain bitmasks (bit d = PantheonDomain d). Every pledged domain grants its perk
    // (offense domains sum into DevotionDamagePct; defense/utility domains fed the capped pools
    // during Rebuild), doubled for a domain at Exarch tier (>=5). A louder PantheonFx flourish
    // fires when a proccing root's domain is pledged.
    public int DevotionMask { get; init; }
    public int ExarchMask { get; init; }
    public int DevotionDamagePct { get; init; }

    // Convenience reads for the FX/tests: pledged to anything, at Exarch anywhere, or to a domain.
    public bool HasDevotion => DevotionMask != 0;
    public bool IsExarch => ExarchMask != 0;
    public bool IsDevotedTo(PantheonDomain domain) => (DevotionMask & (1 << (int)domain)) != 0;
    public bool IsExarchOf(PantheonDomain domain) => (ExarchMask & (1 << (int)domain)) != 0;
}

// Per-Mobile aggregate of every worn variant armor/shield/jewelry/clothing item, rebuilt on
// OnWornAdded/OnWornRemoved. Single-threaded plain dictionaries (CLAUDE.md rule 3) — evicted on
// death/delete via RarityEffects.OnMobileGone, same as CombatFxState.
public static class WornEffectState
{
    // §9.4 stacking is keyed by mechanic StackGroup, not raw root: the strongest piece in a
    // group counts full, every other piece in that group at half, so cross-material same-mechanic
    // pieces (e.g. a Talarian suit + an Ophis weapon) pool instead of each counting in full.
    private static readonly int GroupCount = VariantRootInfo.StackGroupCount;

    // Suit-wide hard ceilings (framework §9.8). DrCap/ReflectCap/HpRegenCap are public: the
    // burst consume sites in RarityEffects.Defense/.Worn clamp their effective totals against
    // them (bursts arm after Rebuild's clamp, so the cap must be re-asserted at consume time).
    public const int DrCap = 12;
    private const int ShrugCap = 20;
    public const int ReflectCap = 25;
    public const int HpRegenCap = 60;
    private const int SpellDrCap = 18;
    public const int DodgeCap = 12;
    public const int CarryWeightCap = 25; // suit-wide carry-capacity ceiling (user directive 2026-07-12)

    private static readonly Dictionary<Mobile, WornAggregate> _aggregates = new();
    private static readonly Dictionary<Mobile, List<LegendaryEntry>> _legendaries = new();
    private static readonly Dictionary<Mobile, DefaultSkillMod> _resistSkillMods = new();
    private static readonly Dictionary<Mobile, DefaultSkillMod> _hidingSkillMods = new();
    private static readonly Dictionary<Mobile, DefaultSkillMod> _stealthSkillMods = new();
    private static readonly Dictionary<Mobile, DefaultSkillMod> _animalTamingSkillMods = new();

    // Klotho (body sash relic): a stand-alone stacking burst (max 2 stacks). Narrow enough — one
    // relic, one behavior — that generalizing the ArmClauseBurst API below for it would be
    // over-engineering for a single caller.
    private static readonly Dictionary<Mobile, (int Stacks, long ExpiryTick)> _klothoStacks = new();

    // Timed "burst" windows for clauses that grant a temporary bonus after a trigger (crit
    // taken, dodge, etc). Keyed by clause so unrelated bursts on the same mobile (e.g. a
    // Talarian dodge burst and a Tritonian resist burst) never collide.
    private static readonly Dictionary<(Mobile, ClauseType), long> _clauseBursts = new();

    // Once-per-fight flags for clauses gated to a single trigger per combat window (framework
    // §9 rule 7 — resets only after 30s out of combat). Reuses CombatFxState's fight-freshness
    // read so "once per fight" always lines up with the same window as "first hit of fight".
    private static readonly HashSet<(Mobile, ClauseType)> _usedOncePerFight = new();

    // DeflectSecondaryFirstHit (Hyperbios & the re-themed armor carriers): armed on the wearer's
    // first hit taken each fight (AbsorbForDefenderArmor), consumed by ApplyMark to REBOUND the
    // first mark/poison/heal-block onto the attacker. A single-hit pending flag (not a burst
    // window) — always cleared by the consuming read.
    private static readonly HashSet<Mobile> _deflectSecondary = new();

    public static WornAggregate GetAggregate(Mobile wearer) =>
        wearer != null && _aggregates.TryGetValue(wearer, out var agg) ? agg : default;

    public static IReadOnlyList<LegendaryEntry> GetLegendaries(Mobile wearer) =>
        wearer != null && _legendaries.TryGetValue(wearer, out var list) ? list : Array.Empty<LegendaryEntry>();

    // Coverage list for Rebuild's StatBonusSplashSecondStat (Aither) check in the accessory loop.
    internal static readonly ClauseType[] HandledByRebuildAggregate = { ClauseType.StatBonusSplashSecondStat };

    // Coverage list for Rebuild's HealsReceivedBonusPct (Diadema) fold-in above.
    internal static readonly ClauseType[] HandledByHealsReceived = { ClauseType.HealsReceivedBonusPct };

    // Rebuilds the wearer's aggregate + legendary list from scratch by walking their worn
    // items (armor/shields, then jewelry/clothing). Called after any piece is added or removed.
    public static void Rebuild(Mobile wearer)
    {
        if (wearer?.Items == null)
        {
            return;
        }

        Span<int> strongestRarity = stackalloc int[GroupCount];
        strongestRarity.Clear();

        // P4 set capstone: Epic+ non-shield pieces per material (mirrors the slot-signature gate).
        Span<int> epicPieces = stackalloc int[ArmorSlotSignatureTable.MaterialCount];
        epicPieces.Clear();

        using var armorItems =
            PooledRefList<(VariantRoot Root, ItemRarity Rarity, ArmorEffectRow Row, bool IsShield, ArmorMaterialType Material, ArmorBodyType Slot)>
                .Create();
        using var weaponItems = PooledRefList<(VariantRoot Root, ItemRarity Rarity, WeaponEffectRow Row)>.Create();
        using var accessoryItems =
            PooledRefList<(VariantRoot Root, ItemRarity Rarity, AccessoryEffectRow Row, Item Source)>.Create();
        List<LegendaryEntry> legendaries = null;

        var wornItems = wearer.Items;

        for (var i = 0; i < wornItems.Count; i++)
        {
            var worn = wornItems[i];

            if (worn is BaseArmor armor && armor is IVariantItem armorVariant &&
                (armorVariant.VariantRoot != VariantRoot.None || armorVariant.LegendaryId != 0))
            {
                var isShield = armor is BaseShield;
                var (root, rarity) = RarityEffects.ResolveRootRarity(armorVariant, armor.Rarity);
                var row = ArmorEffectTable.Get(root, rarity, isShield);

                armorItems.Add((root, rarity, row, isShield, armor.MaterialType, armor.BodyPosition));

                if (!isShield && rarity >= ItemRarity.Epic && (int)armor.MaterialType < epicPieces.Length)
                {
                    epicPieces[(int)armor.MaterialType]++;
                }

                var group = VariantRootInfo.GetStackGroup(root);

                if ((int)rarity > strongestRarity[group])
                {
                    strongestRarity[group] = (int)rarity;
                }

                if (armorVariant.LegendaryId != 0 && LegendaryRegistry.TryGet(armorVariant.LegendaryId, out var armorEntry))
                {
                    (legendaries ??= new List<LegendaryEntry>()).Add(armorEntry);
                }
            }
            else if (worn is BaseJewel jewel && jewel is IVariantItem jewelVariant &&
                     (jewelVariant.VariantRoot != VariantRoot.None || jewelVariant.LegendaryId != 0))
            {
                var (root, rarity) = RarityEffects.ResolveRootRarity(jewelVariant, jewel.Rarity);
                var row = AccessoryEffectTable.Get(root, rarity, isClothing: false);

                accessoryItems.Add((root, rarity, row, jewel));

                var group = VariantRootInfo.GetStackGroup(root);

                if ((int)rarity > strongestRarity[group])
                {
                    strongestRarity[group] = (int)rarity;
                }

                if (jewelVariant.LegendaryId != 0 && LegendaryRegistry.TryGet(jewelVariant.LegendaryId, out var jewelEntry))
                {
                    (legendaries ??= new List<LegendaryEntry>()).Add(jewelEntry);
                }
            }
            else if (worn is BaseClothing clothing && clothing is IVariantItem clothingVariant &&
                     (clothingVariant.VariantRoot != VariantRoot.None || clothingVariant.LegendaryId != 0))
            {
                var (root, rarity) = RarityEffects.ResolveRootRarity(clothingVariant, clothing.Rarity);
                var row = AccessoryEffectTable.Get(root, rarity, isClothing: true, AccessoryEffectTable.IsDisplacingClothing(clothing));

                accessoryItems.Add((root, rarity, row, clothing));

                var group = VariantRootInfo.GetStackGroup(root);

                if ((int)rarity > strongestRarity[group])
                {
                    strongestRarity[group] = (int)rarity;
                }

                if (clothingVariant.LegendaryId != 0 &&
                    LegendaryRegistry.TryGet(clothingVariant.LegendaryId, out var clothingEntry))
                {
                    (legendaries ??= new List<LegendaryEntry>()).Add(clothingEntry);
                }
            }
            else if (worn is BaseWeapon weapon && weapon is IVariantItem weaponVariant &&
                     (weaponVariant.VariantRoot != VariantRoot.None || weaponVariant.LegendaryId != 0))
            {
                // A held weapon folds only its worn-side utility (spell DR / mana regen / dodge /
                // resist skill) into the aggregate, plus its legendary + lane signature. BaseShield
                // is a BaseArmor and is already handled above, so this is real weapons only.
                var (root, rarity) = RarityEffects.ResolveRootRarity(weaponVariant, weapon.Rarity);
                var row = WeaponEffectTable.Get(root, rarity);

                weaponItems.Add((root, rarity, row));

                var group = VariantRootInfo.GetStackGroup(root);

                if ((int)rarity > strongestRarity[group])
                {
                    strongestRarity[group] = (int)rarity;
                }

                if (weaponVariant.LegendaryId != 0 && LegendaryRegistry.TryGet(weaponVariant.LegendaryId, out var weaponEntry))
                {
                    (legendaries ??= new List<LegendaryEntry>()).Add(weaponEntry);
                }
            }
        }

        // P6 / B4 — Patron God devotion, counted HERE: after the item walk the list holds only real
        // legendaries (lane/slot signature synthetics are appended by the loops below), so only
        // genuine legendaries pledge. Devotion now STACKS across domains: every domain with >=3
        // worn legendaries sets its bit in devotionMask (>=5 also sets exarchMask).
        var devotionMask = 0;
        var exarchMask = 0;

        if (legendaries is { Count: >= 3 })
        {
            Span<int> domainCounts = stackalloc int[(int)PantheonDomain.Nature + 1];
            domainCounts.Clear();

            for (var i = 0; i < legendaries.Count; i++)
            {
                domainCounts[(int)PantheonFx.GetDomain(legendaries[i].Root)]++;
            }

            for (var d = 0; d < domainCounts.Length; d++)
            {
                if (domainCounts[d] >= 3)
                {
                    devotionMask |= 1 << d;
                }

                if (domainCounts[d] >= 5)
                {
                    exarchMask |= 1 << d;
                }
            }
        }

        var dr = 0;
        var shrug = 0;
        var reflect = 0;
        var flameProc = 0;
        var hpRegen = 0;
        var healsReceived = 0;
        var autoCure = false;
        var spellDr = 0;
        var paraResist = 0;
        var resistSkill = 0;
        var stamRegen = 0;
        var dodge = 0;
        var parry = 0;
        var parryDr = 0;
        var parryThorns = false;
        var carryWeightBonus = 0;

        var lightningProc = 0;
        var manaRegen = 0;
        var spellDamage = 0;
        var manaLeech = 0;
        var missReroll = 0;
        var hidingBonus = 0;
        var stealthBonus = 0;
        var nightSight = false;
        var poisonResist = 0;
        var potionEffect = 0;
        var onKillStamina = 0;
        var onKillHp = 0;
        var karmaGain = 0;
        var vendorPrice = 0;
        var frenzyChance = 0;
        var frenzyDamage = 0;
        var frenzySwing = 0;
        var stationaryRegen = 0;
        var stationaryAppliesMana = false;

        Span<int> statTotals = stackalloc int[3]; // 0 = Str, 1 = Dex, 2 = Int (Olympian)
        statTotals.Clear();

        // Only ONE instance per stack-group counts at full weight — ties at the same rarity (e.g.
        // two Legendary Talarian pieces, or a Talarian piece + an Ophis weapon) still pick just one
        // "strongest", not award both 100%. Shared across armor, weapons, and accessories.
        Span<bool> fullWeightConsumed = stackalloc bool[GroupCount];
        fullWeightConsumed.Clear();

        for (var i = 0; i < armorItems.Count; i++)
        {
            var (root, rarity, row, isShield, material, slot) = armorItems[i];
            var group = VariantRootInfo.GetStackGroup(root);
            var isStrongest = !fullWeightConsumed[group] && (int)rarity == strongestRarity[group];

            if (isStrongest)
            {
                fullWeightConsumed[group] = true;

                // Option A milestone: armor (not shields) at Epic+ reads its Signature from the
                // (material x slot) table instead of the per-root ArmorEffectTable row. Shields
                // and sub-Epic pieces keep the original root-keyed lookup unchanged.
                var (signature, s1, s2, s3) = !isShield && rarity >= ItemRarity.Epic
                    ? ArmorSlotSignatureTable.Get(material, slot)
                    : (row.Signature, row.S1, row.S2, row.S3);

                AppendSignature(ref legendaries, root, signature, s1, s2, s3);
            }

            var weight = isStrongest ? 100 : 50;

            dr += row.DrPct * weight / 100;
            shrug += row.ShrugPct * weight / 100;
            reflect += row.ReflectPct * weight / 100;
            flameProc += row.FlameProcPct * weight / 100;
            hpRegen += row.HpRegenPct * weight / 100;
            healsReceived += row.HealsReceivedPct * weight / 100;
            autoCure |= row.AutoCure;
            spellDr += row.SpellDrPct * weight / 100;
            paraResist += row.ParaResistPct * weight / 100;
            resistSkill += row.ResistSkillBonus * weight / 100;
            stamRegen += row.StamRegenPct * weight / 100;
            dodge += row.DodgePct * weight / 100;
            parry += row.ParryPct * weight / 100; // single shield slot — weight is always 100
            parryDr += row.ParryDrPct * weight / 100;
            parryThorns |= row.ParryThorns;

            // P2 re-theme per-material fields fold into the existing aggregate accumulators.
            poisonResist += row.PoisonResistPct * weight / 100;
            hidingBonus += row.HidingBonus * weight / 100;
            onKillStamina += row.OnKillStamPct * weight / 100;
            onKillHp += row.OnKillHpPct * weight / 100;
            carryWeightBonus += row.WeightReductionPct * weight / 100; // light-armor carry-capacity lane
        }

        for (var i = 0; i < accessoryItems.Count; i++)
        {
            var (root, rarity, row, source) = accessoryItems[i];
            var group = VariantRootInfo.GetStackGroup(root);
            var isStrongest = !fullWeightConsumed[group] && (int)rarity == strongestRarity[group];

            if (isStrongest)
            {
                fullWeightConsumed[group] = true;
            }

            var weight = isStrongest ? 100 : 50;

            // Olympian: the rolled stat (persisted on the item, BaseJewel.OlympianStat) receives
            // the weighted magnitude.
            if (root == VariantRoot.Olympian && source is BaseJewel jewel)
            {
                var statContribution = row.StatBonus * weight / 100;
                statTotals[StatIndex(jewel.OlympianStat)] += statContribution;

                // Aither: half of THIS item's own contribution also splashes to a second stat.
                // The doc says "your choice" — there's no player-choice primitive at this layer,
                // so a deterministic Str->Dex->Int->Str rotation stands in for it (flagged in the
                // P3b report).
                if (jewel is IVariantItem aitherVariant && aitherVariant.LegendaryId != 0 &&
                    LegendaryRegistry.TryGet(aitherVariant.LegendaryId, out var aitherEntry) &&
                    aitherEntry.Clause == ClauseType.StatBonusSplashSecondStat)
                {
                    statTotals[StatIndex(NextStat(jewel.OlympianStat))] += statContribution / 2;
                }
            }

            lightningProc += row.LightningProcPct * weight / 100;
            shrug += row.HitHalvedPct * weight / 100; // Tychean shares the shrug pool/cap
            dodge += row.DodgePct * weight / 100;       // Arachne shares the dodge pool/cap
            hpRegen += row.AllRegenPct * weight / 100;  // Demetrian all-regen
            stamRegen += row.AllRegenPct * weight / 100;
            manaRegen += (row.ManaRegenPct + row.AllRegenPct) * weight / 100;
            spellDamage += row.SpellDamagePct * weight / 100;
            manaLeech += row.ManaLeechPct * weight / 100;
            missReroll += row.MissRerollPct * weight / 100;
            hidingBonus += row.HidingBonus * weight / 100;
            stealthBonus += row.StealthBonus * weight / 100;
            nightSight |= row.NightSight;
            poisonResist += row.PoisonResistPct * weight / 100;
            potionEffect += row.PotionEffectPct * weight / 100;
            onKillStamina += row.OnKillStamina * weight / 100;
            onKillHp += row.OnKillHp * weight / 100;
            karmaGain += row.KarmaGainPct * weight / 100;
            vendorPrice += row.VendorPricePct * weight / 100;
            frenzyChance += row.FrenzyChancePct * weight / 100;
            frenzyDamage += row.FrenzyDamagePct * weight / 100;
            frenzySwing += row.FrenzySwingPct * weight / 100;
            stationaryRegen += row.StationaryRegenPct * weight / 100;
            stationaryAppliesMana |= row.StationaryAppliesMana;
        }

        for (var i = 0; i < weaponItems.Count; i++)
        {
            var (root, rarity, row) = weaponItems[i];
            var group = VariantRootInfo.GetStackGroup(root);
            var isStrongest = !fullWeightConsumed[group] && (int)rarity == strongestRarity[group];

            if (isStrongest)
            {
                fullWeightConsumed[group] = true;
                AppendSignature(ref legendaries, root, row.Signature, row.S1, row.S2, row.S3);
            }

            var weight = isStrongest ? 100 : 50;

            // A held weapon contributes ONLY its worn-side utility lanes (staves/fencing/archery
            // defensive lanes) to the aggregate; every offensive field stays on the hit pipeline.
            spellDr += row.SpellDrPct * weight / 100;
            manaRegen += row.ManaRegenPct * weight / 100;
            dodge += row.DodgePct * weight / 100;
            resistSkill += row.ResistSkillBonus * weight / 100;
            healsReceived += row.HealsReceivedPct * weight / 100; // Manteia staff lane
            autoCure |= row.AutoCure;
        }

        // P5b Divine Resonance: duplicates removed by the dedupe echo as category bonuses —
        // capped at 2 per category, folded in BEFORE the §9.8 clamps so every ceiling stays law.
        // B5: also collect per-pair source names for the buff-bar readout (cold path — a small
        // list allocation on equip changes only).
        var resonanceEchoes = new List<(int Category, string A, string B)>();
        var (resonanceOffense, resonanceDefense, resonanceUtility) = DedupeClausesCounted(legendaries, resonanceEchoes);

        resonanceOffense = Math.Min(resonanceOffense, 2);
        resonanceDefense = Math.Min(resonanceDefense, 2);
        resonanceUtility = Math.Min(resonanceUtility, 2);

        var resonanceDamage = resonanceOffense * 4; // ≤ +8% damage, consumed by BeginWeaponHit

        dr += resonanceDefense * 2;
        hpRegen += resonanceUtility * 10;
        stamRegen += resonanceUtility * 10;
        manaRegen += resonanceUtility * 10;

        var resonanceText = BuildResonanceText(resonanceOffense, resonanceDefense, resonanceUtility, resonanceEchoes);

        // P6 / B4 devotion perk — every PLEDGED domain grants its perk (offense sums into
        // DevotionDamagePct; defense/utility fold into the capped pools), doubled for any domain
        // at Exarch tier. Same magnitudes as one resonance echo, same pre-clamp fold.
        var devotionDamage = 0;

        if (devotionMask != 0)
        {
            for (var d = 0; d <= (int)PantheonDomain.Nature; d++)
            {
                if ((devotionMask & (1 << d)) == 0)
                {
                    continue;
                }

                var scale = (exarchMask & (1 << d)) != 0 ? 2 : 1;

                switch (PantheonFx.GetDomainCategory((PantheonDomain)d))
                {
                    case 0:
                        devotionDamage += 4 * scale;
                        break;
                    case 1:
                        dr += 2 * scale;
                        break;
                    default:
                        hpRegen += 10 * scale;
                        stamRegen += 10 * scale;
                        manaRegen += 10 * scale;
                        break;
                }
            }
        }

        // Diadema (Charis hat relic): +P1% to all healing received, folded into the heals-received
        // pool on Rebuild (WornStatMod) so the single AdjustHealAmount choke point picks it up.
        if (legendaries != null)
        {
            for (var i = 0; i < legendaries.Count; i++)
            {
                if (legendaries[i].Clause == ClauseType.HealsReceivedBonusPct)
                {
                    healsReceived += legendaries[i].P1 > 0 ? legendaries[i].P1 : 15;
                }
            }
        }

        // P4 — armor slot-set capstone: min(4, available slots) Epic+ pieces of one material
        // completes the set. First match wins; the slot math makes a second match impossible.
        var hasCapstone = false;
        var capstoneMaterial = default(ArmorMaterialType);

        for (var m = 0; m < epicPieces.Length; m++)
        {
            var threshold = CapstoneThreshold((ArmorMaterialType)m);

            if (threshold > 0 && epicPieces[m] >= threshold)
            {
                hasCapstone = true;
                capstoneMaterial = (ArmorMaterialType)m;
                break;
            }
        }

        // Evasion (leather capstone): +6% dodge, folded in BEFORE the cap clamp so the suit-wide
        // dodge ceiling stays law.
        if (hasCapstone && capstoneMaterial == ArmorMaterialType.Leather)
        {
            dodge += 6;
        }

        _aggregates.TryGetValue(wearer, out var previous);

        _aggregates[wearer] = new WornAggregate
        {
            DrPct = Math.Min(dr, DrCap),
            ShrugPct = Math.Min(shrug, ShrugCap),
            ReflectPct = Math.Min(reflect, ReflectCap),
            FlameProcPct = flameProc,
            HpRegenPct = Math.Min(hpRegen, HpRegenCap),
            HealsReceivedPct = healsReceived,
            AutoCure = autoCure,
            SpellDrPct = Math.Min(spellDr, SpellDrCap),
            ParaResistPct = paraResist,
            ResistSkillBonus = resistSkill,
            StamRegenPct = stamRegen,
            DodgePct = Math.Min(dodge, DodgeCap),
            ParryPct = parry,
            ParryDrPct = Math.Min(parryDr, DrCap),
            ParryThorns = parryThorns,
            CarryWeightBonusPct = Math.Min(carryWeightBonus, CarryWeightCap),

            LightningProcPct = lightningProc,
            ManaRegenPct = manaRegen,
            SpellDamagePct = spellDamage,
            ManaLeechPct = manaLeech,
            MissRerollPct = missReroll,
            HidingBonus = hidingBonus,
            StealthBonus = stealthBonus,
            NightSight = nightSight,
            PoisonResistPct = poisonResist,
            PotionEffectPct = potionEffect,

            OnKillStamina = onKillStamina,
            OnKillHp = onKillHp,
            KarmaGainPct = karmaGain,
            VendorPricePct = vendorPrice,
            FrenzyChancePct = frenzyChance,
            FrenzyDamagePct = frenzyDamage,
            FrenzySwingPct = frenzySwing,
            StationaryRegenPct = stationaryRegen,
            StationaryAppliesMana = stationaryAppliesMana,

            HasCapstone = hasCapstone,
            CapstoneMaterial = capstoneMaterial,

            ResonanceOffense = resonanceOffense,
            ResonanceDefense = resonanceDefense,
            ResonanceUtility = resonanceUtility,
            ResonanceDamagePct = resonanceDamage,
            ResonanceText = resonanceText,

            DevotionMask = devotionMask,
            ExarchMask = exarchMask,
            DevotionDamagePct = devotionDamage
        };

        SyncCapstoneBuff(wearer, previous, hasCapstone, capstoneMaterial);
        SyncResonanceBuff(wearer, previous, resonanceText);
        SyncDevotionBuff(wearer, previous, devotionMask, exarchMask);

        if (legendaries != null)
        {
            _legendaries[wearer] = legendaries;
        }
        else
        {
            _legendaries.Remove(wearer);
        }

        SyncResistSkillMod(wearer, resistSkill);
        SyncSkillMod(wearer, _hidingSkillMods, SkillName.Hiding, "RarityHiding", hidingBonus);
        SyncSkillMod(wearer, _stealthSkillMods, SkillName.Stealth, "RarityStealth", stealthBonus);
        SyncAnimalTamingSkillMod(wearer, legendaries);
        SyncOlympianStatMods(wearer, statTotals[0], statTotals[1], statTotals[2]);
        SyncNightSight(wearer, nightSight);
    }

    // Appends a synthetic legendary entry carrying a worn piece's lane signature (Id 0, no name,
    // Family 0xFF) so the ~15 worn-side clause-dispatch sites pick it up unchanged. Called only for
    // the strongest piece per stack-group, so each group contributes its signature at most once.
    // Internal so the synthesis shape can be unit-tested while every production row still leaves
    // Signature = None this phase.
    internal static void AppendSignature(
        ref List<LegendaryEntry> legendaries, VariantRoot root, ClauseType signature, short s1, short s2, short s3
    )
    {
        if (signature == ClauseType.None)
        {
            return;
        }

        (legendaries ??= new List<LegendaryEntry>()).Add(
            new LegendaryEntry(0, null, root, 0xFF, 0, signature, s1, s2, s3, 0)
        );
    }

    // P4 capstone metadata — sourced from each ArmorFamilyDefinition (Families/*.cs) via the
    // registry. Threshold = min(4, available slots): chainmail only has 3 piece shapes, so 3; every
    // other set material 4; non-set materials 0. Name/icon default to Plate's ("Siege-Shock"/
    // Knockout) for any material without a capstone, preserving the legacy switch defaults.
    private static int CapstoneThreshold(ArmorMaterialType material) => FamilyRegistry.CapstoneThreshold(material);

    internal static string CapstoneName(ArmorMaterialType material) => FamilyRegistry.CapstoneName(material);

    // Icons chosen to not collide with ones the rarity engine already adds/removes: the mark
    // (EnemyOfOneDebuff), heal-block (MortalStrike), frenzy (Rage), Ward-Surge burst (Protection),
    // crit-ready flag (LightningStrike), and the dodge/regen burst icons armed in ArmClauseBurst
    // (Invigorate, GiftOfLife, GiftOfRenewal, OrangePetals).
    internal static BuffIcon CapstoneIcon(ArmorMaterialType material) => FamilyRegistry.CapstoneIcon(material);

    // Keeps the indefinite set-bonus buff icon in step with the rebuilt aggregate; duration
    // default = indefinite, so completing a set shows the icon until the set is broken.
    private static void SyncCapstoneBuff(Mobile wearer, in WornAggregate previous, bool hasCapstone, ArmorMaterialType material)
    {
        if (previous.HasCapstone && (!hasCapstone || previous.CapstoneMaterial != material))
        {
            BuffHelper.RemoveBuff(wearer, CapstoneIcon(previous.CapstoneMaterial));
        }

        if (hasCapstone && (!previous.HasCapstone || previous.CapstoneMaterial != material))
        {
            BuffHelper.AddCustomBuff(wearer, CapstoneIcon(material), $"{CapstoneName(material)}: {CapstoneEffectText(material)}");
        }
    }

    // One-line description of what a completed set's capstone does (RarityEffects.WeaponHit/.Defense
    // apply these; §P4). Feeds the capstone buff-bar readout alongside its themed name.
    private static string CapstoneEffectText(ArmorMaterialType material) => material switch
    {
        ArmorMaterialType.Leather   => "+6% dodge",
        ArmorMaterialType.Studded   => "your hits poison the target",
        ArmorMaterialType.Bone      => "your hits heal-block the target",
        ArmorMaterialType.Ringmail  => "+8% reflect while struck",
        ArmorMaterialType.Chainmail => "damage reduction rises to its maximum for 5s after you take a crit",
        ArmorMaterialType.Plate     => "your hits briefly stun the target",
        _                           => "full set bonus"
    };

    // P5 — suit-wide dedupe safety net: a ClauseType dispatches at most once per wearer, even
    // when a real legendary and a slot/lane signature (or two worn legendaries) carry the same
    // clause. Every worn-side dispatch site iterates this list linearly, so collapsing
    // duplicates here covers them all at once. O(n²) over a handful of entries, rebuild-only.
    // Per-item dual-clause seams (a weapon's own signature + its own legendary) stay guarded by
    // the registry's "unique != signature within a lane" invariant instead.
    internal static void DedupeClauses(List<LegendaryEntry> legendaries) => DedupeClausesCounted(legendaries);

    // P5b Divine Resonance: the counted form reports how many duplicates were folded away, per
    // resonance category, so Rebuild can convert them into echo bonuses instead of pure waste.
    // B5: when `echoes` is supplied, each legendary-bearing resonating pair also records its
    // category and the two source names (real legendaries only, Id != 0) for the buff-bar readout.
    internal static (int Offense, int Defense, int Utility) DedupeClausesCounted(
        List<LegendaryEntry> legendaries, List<(int Category, string A, string B)> echoes = null
    )
    {
        if (legendaries == null || legendaries.Count < 2)
        {
            return default;
        }

        var offense = 0;
        var defense = 0;
        var utility = 0;

        for (var i = 0; i < legendaries.Count - 1; i++)
        {
            for (var j = legendaries.Count - 1; j > i; j--)
            {
                if (legendaries[j].Clause != legendaries[i].Clause)
                {
                    continue;
                }

                // Divine Resonance is a LEGENDARY-tier reward: a pair of purely synthetic lane/
                // slot signatures (both Id 0, i.e. an Epic-only suit) dedupes silently, no echo.
                var pairHasLegendary = legendaries[i].Id != 0 || legendaries[j].Id != 0;

                // Source names captured BEFORE the swap collapses the pair (synthetics carry none).
                var nameA = legendaries[i].Id != 0 ? legendaries[i].Name : null;
                var nameB = legendaries[j].Id != 0 ? legendaries[j].Name : null;

                if (Beats(legendaries[j], legendaries[i]))
                {
                    legendaries[i] = legendaries[j];
                }

                if (pairHasLegendary)
                {
                    var category = ResonanceCategory(legendaries[i].Clause);

                    switch (category)
                    {
                        case 0:
                            offense++;
                            break;
                        case 1:
                            defense++;
                            break;
                        default:
                            utility++;
                            break;
                    }

                    echoes?.Add((category, nameA, nameB));
                }

                legendaries.RemoveAt(j);
            }
        }

        return (offense, defense, utility);
    }

    // Resonance category from the clause's declared dispatch triggers (ClauseTraits — no second
    // per-clause table to drift). Offense wins over defense wins over utility for mixed clauses.
    private const ClauseTrigger OffensiveTriggers =
        ClauseTrigger.WeaponHitArm | ClauseTrigger.PostHitProc | ClauseTrigger.MarkRider |
        ClauseTrigger.ExtraSwingRider | ClauseTrigger.LightningProc | ClauseTrigger.SpellManaLeech;

    private const ClauseTrigger DefensiveTriggers =
        ClauseTrigger.WeaponBlock | ClauseTrigger.ArmorDefense | ClauseTrigger.ShieldParry |
        ClauseTrigger.ArmorHitRider | ClauseTrigger.Dodge | ClauseTrigger.SpellDr |
        ClauseTrigger.ParaResist | ClauseTrigger.PoisonResist | ClauseTrigger.MissReroll;

    private static int ResonanceCategory(ClauseType clause)
    {
        var triggers = ClauseTraits.Get(clause);

        if ((triggers & OffensiveTriggers) != 0)
        {
            return 0;
        }

        return (triggers & DefensiveTriggers) != 0 ? 1 : 2;
    }

    // Builds the full Divine Resonance buff text with per-item source names, or null when nothing
    // resonates. B5: naming the sources ("+4% damage (Klytios + Skiron)") both explains the echo
    // and — because SyncResonanceBuff keys on this whole string — fixes the equip bug where a
    // same-count source swap left the old text on the bar. Cold path (equip only) — allocations fine.
    private static string BuildResonanceText(
        int offense, int defense, int utility, List<(int Category, string A, string B)> echoes
    )
    {
        if (offense + defense + utility == 0)
        {
            return null;
        }

        var text = "Divine Resonance:";
        var parts = 0;

        if (offense > 0)
        {
            text += $" +{offense * 4}% damage{ResonanceSources(echoes, 0)}";
            parts++;
        }

        if (defense > 0)
        {
            text += $"{(parts > 0 ? "," : "")} +{defense * 2}% damage reduction{ResonanceSources(echoes, 1)}";
            parts++;
        }

        if (utility > 0)
        {
            text += $"{(parts > 0 ? "," : "")} +{utility * 10}% regen{ResonanceSources(echoes, 2)}";
        }

        return text;
    }

    // " (Klytios + Skiron)" for one category, or empty if no named sources were recorded. Multiple
    // echoes in the same category comma-join their names.
    private static string ResonanceSources(List<(int Category, string A, string B)> echoes, int category)
    {
        if (echoes == null)
        {
            return "";
        }

        var names = new List<string>();

        foreach (var (cat, a, b) in echoes)
        {
            if (cat != category)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(a))
            {
                names.Add(a);
            }

            if (!string.IsNullOrEmpty(b))
            {
                names.Add(b);
            }
        }

        return names.Count > 0 ? $" ({string.Join(" + ", names)})" : "";
    }

    // Keeps the indefinite Divine Resonance buff icon in step with the rebuilt aggregate — same
    // lifecycle as the capstone buff above. B5: keyed on the full source-naming text, so ANY
    // composition change (including a same-count source swap) refreshes it. Icon chosen to not
    // collide with any other icon the rarity engine adds/removes (see CapstoneIcon's comment).
    private static void SyncResonanceBuff(Mobile wearer, in WornAggregate previous, string text)
    {
        if (previous.ResonanceText == text)
        {
            return;
        }

        if (previous.ResonanceText != null)
        {
            BuffHelper.RemoveBuff(wearer, BuffIcon.ArcaneEmpowerment);
        }

        if (text != null)
        {
            BuffHelper.AddCustomBuff(wearer, BuffIcon.ArcaneEmpowerment, text);

            // Visibility: the moment a duplicate starts (or strengthens) an echo, say so overhead —
            // otherwise the player's only cue is a quiet buff-bar icon.
            FloatingCombatText.ShowSelfStatus(wearer, "Divine Resonance");
        }
    }

    // P6 / B4 — keeps the indefinite Patron God buff in step with the rebuilt aggregate (Surge icon,
    // unused anywhere else in the codebase). Devotion now stacks across domains, so the single buff
    // lists EVERY pledged patron and its perk. Keyed on the mask pair so any change re-syncs.
    private static void SyncDevotionBuff(Mobile wearer, in WornAggregate previous, int devotionMask, int exarchMask)
    {
        if (previous.DevotionMask == devotionMask && previous.ExarchMask == exarchMask)
        {
            return;
        }

        if (previous.DevotionMask != 0)
        {
            BuffHelper.RemoveBuff(wearer, BuffIcon.Surge);
        }

        if (devotionMask == 0)
        {
            return;
        }

        // Cold path (equip only) — plain string building is fine.
        var text = "";
        var overhead = "";

        for (var d = 0; d <= (int)PantheonDomain.Nature; d++)
        {
            if ((devotionMask & (1 << d)) == 0)
            {
                continue;
            }

            var domain = (PantheonDomain)d;
            var patron = PantheonFx.GetPatronName(domain);
            var exarch = (exarchMask & (1 << d)) != 0;
            var title = exarch ? "Exarch of " : "";
            var perk = ExarchPerkText(domain, exarch);

            text += text.Length > 0 ? $", {title}{patron} ({perk})" : $"Patron: {title}{patron} ({perk})";

            if (overhead.Length == 0)
            {
                overhead = exarch ? $"Exarch of {patron}" : $"Patron: {patron}";
            }
        }

        BuffHelper.AddCustomBuff(wearer, BuffIcon.Surge, text);
        FloatingCombatText.ShowSelfStatus(wearer, overhead);
    }

    // Exarch doubles the devotion perk — text mirrors the Rebuild fold above.
    private static string ExarchPerkText(PantheonDomain domain, bool isExarch) =>
        !isExarch
            ? PantheonFx.GetPerkText(domain)
            : PantheonFx.GetDomainCategory(domain) switch
            {
                0 => "+8% damage",
                1 => "+4% damage reduction",
                _ => "+20% regen"
            };

    // "Strongest wins". P-values compare raw — an entry relying on its dispatch-site default
    // (P=0) loses to any explicit value; acceptable for a safety net, since registry entries
    // carry explicit params. Ties prefer the shield-sourced entry (the parry-path gates key off
    // the source), then a real legendary (Id != 0) over a synthetic signature.
    private static bool Beats(in LegendaryEntry a, in LegendaryEntry b)
    {
        // "Every Nth" clauses: a smaller nonzero interval fires more often, so it is stronger.
        if (a.Clause is ClauseType.FlameProcEveryN or ClauseType.ParryForcesMissEveryN && a.P1 != b.P1)
        {
            return a.P1 > 0 && (b.P1 <= 0 || a.P1 < b.P1);
        }

        if (a.P1 != b.P1)
        {
            return a.P1 > b.P1;
        }

        if (a.P2 != b.P2)
        {
            return a.P2 > b.P2;
        }

        if (RarityEffects.IsShieldSourced(a) != RarityEffects.IsShieldSourced(b))
        {
            return RarityEffects.IsShieldSourced(a);
        }

        return a.Id != 0 && b.Id == 0;
    }

    private static int StatIndex(StatType type) => type switch
    {
        StatType.Dex => 1,
        StatType.Int => 2,
        _            => 0
    };

    private static StatType NextStat(StatType type) => type switch
    {
        StatType.Str => StatType.Dex,
        StatType.Dex => StatType.Int,
        _            => StatType.Str
    };

    private static void SyncOlympianStatMods(Mobile wearer, int strBonus, int dexBonus, int intBonus)
    {
        // StatMod offsets are immutable once created (no public setter) — remove-and-recreate is
        // the existing engine pattern (mirrors BaseJewel's own AOS stat-bonus wiring) and only
        // runs on equip/unequip (Rebuild), not per-tick, so the churn is cheap.
        wearer.RemoveStatMod("RarityOlympianStr");
        wearer.RemoveStatMod("RarityOlympianDex");
        wearer.RemoveStatMod("RarityOlympianInt");

        if (strBonus != 0)
        {
            wearer.AddStatMod(new StatMod(StatType.Str, "RarityOlympianStr", strBonus, TimeSpan.Zero));
        }

        if (dexBonus != 0)
        {
            wearer.AddStatMod(new StatMod(StatType.Dex, "RarityOlympianDex", dexBonus, TimeSpan.Zero));
        }

        if (intBonus != 0)
        {
            wearer.AddStatMod(new StatMod(StatType.Int, "RarityOlympianInt", intBonus, TimeSpan.Zero));
        }

        wearer.CheckStatTimers();
    }

    private static void SyncNightSight(Mobile wearer, bool nightSight)
    {
        if (nightSight)
        {
            wearer.LightLevel = LightCycle.DungeonLevel;
        }
        else if (wearer.LightLevel == LightCycle.DungeonLevel)
        {
            // ponytail: best-effort — only clears OUR contribution, and only when nothing else
            // has since changed it. A real Night Sight spell/potion active at the same instant
            // would be cut short by doffing Nyxian jewelry; flagged, not fixed (reconciling with
            // every personal-light source is a much larger change than this family needs).
            wearer.LightLevel = 0;
        }
    }

    private static void SyncResistSkillMod(Mobile wearer, int bonus) =>
        SyncSkillMod(wearer, _resistSkillMods, SkillName.MagicResist, "RarityResistingSpells", bonus);

    // Coverage list for SyncAnimalTamingSkillMod's AnimalTamingSkillBonus (Lachesis) check below.
    internal static readonly ClauseType[] HandledByAnimalTaming = { ClauseType.AnimalTamingSkillBonus };

    private static void SyncAnimalTamingSkillMod(Mobile wearer, List<LegendaryEntry> legendaries)
    {
        var bonus = 0;

        if (legendaries != null)
        {
            for (var i = 0; i < legendaries.Count; i++)
            {
                if (legendaries[i].Clause == ClauseType.AnimalTamingSkillBonus)
                {
                    bonus = legendaries[i].P1 > 0 ? legendaries[i].P1 : 15; // Lachesis
                }
            }
        }

        SyncSkillMod(wearer, _animalTamingSkillMods, SkillName.AnimalTaming, "RarityAnimalTaming", bonus);
    }

    private static void SyncSkillMod(Mobile wearer, Dictionary<Mobile, DefaultSkillMod> table, SkillName skill, string name, int bonus)
    {
        table.TryGetValue(wearer, out var mod);

        if (bonus <= 0)
        {
            if (mod != null)
            {
                mod.Remove();
                table.Remove(wearer);
            }

            return;
        }

        if (mod == null)
        {
            mod = new DefaultSkillMod(skill, name, true, bonus);
            wearer.AddSkillMod(mod);
            table[wearer] = mod;
        }
        else
        {
            mod.Value = bonus;
        }
    }

    // Glaukos: overrides the synced Resisting Spells bonus with a conditional value (e.g. while
    // below 50% HP), checked each HP-regen tick by RarityEffects.AdjustHitsRegenRate since there
    // is no per-mobile "HP changed" event to react to instantly.
    public static void BoostResistSkill(Mobile wearer, int value)
    {
        if (wearer != null && _resistSkillMods.TryGetValue(wearer, out var mod))
        {
            mod.Value = value;
        }
    }

    public static void ArmClauseBurst(Mobile wearer, ClauseType clause, TimeSpan duration)
    {
        if (wearer == null)
        {
            return;
        }

        _clauseBursts[(wearer, clause)] = Core.TickCount + (long)duration.TotalMilliseconds;

        // Every timed burst gets a buff-bar icon so the player can see the remaining duration
        // (2026-07-12 pass — previously only the four regen bursts had one). Timed icons
        // auto-expire with `duration`, so no removal bookkeeping is needed. Icons reuse
        // anachronistic buff art (buff bar intentionally enabled on T2A) and are chosen not to
        // collide with the mark/heal-block/frenzy/Bulwark/capstone/crit-ready icons already in
        // use (capstones hold Toughness/DeathStrike/InjectedStrike/Evasion/Block/Knockout).
        // The five spell-resist bursts share one icon deliberately — they raise the same stat,
        // and two running at once is a rare, purely cosmetic overlap.
        var (icon, label) = clause switch
        {
            ClauseType.DodgeRegenBurst              => (BuffIcon.Invigorate, "Dodge Surge: stamina regen up"),
            ClauseType.HpRegenBurstOnCritTaken      => (BuffIcon.GiftOfLife, "Regen Surge: health regen up"),
            ClauseType.HitHalvedRegenPulse          => (BuffIcon.GiftOfRenewal, "Regen Pulse: health/stamina/mana regen up"),
            ClauseType.RegenDoubleAfterPotion       => (BuffIcon.OrangePetals, "Elixir: regen doubled"),
            ClauseType.SpellDrBurstOnCritTaken      => (BuffIcon.MagicReflection, "Warded: spell resist doubled"),
            ClauseType.ParaResistBoostsSpellDr      => (BuffIcon.MagicReflection, "Warded: spell resist up"),
            ClauseType.LightningProcResistBurst     => (BuffIcon.MagicReflection, "Warded: spell resist up"),
            ClauseType.ManaLeechResistBurst         => (BuffIcon.MagicReflection, "Warded: spell resist up"),
            ClauseType.HitHalvedResistBurst         => (BuffIcon.MagicReflection, "Warded: spell resist up"),
            ClauseType.ParaResistBoostsResistSkill  => (BuffIcon.Warding, "Warded: Magic Resistance up"),
            ClauseType.ShrugFirstHitDrBurst         => (BuffIcon.ReactiveArmor, "Fortified: damage reduction up"),
            ClauseType.BlockGrantsDrBurst           => (BuffIcon.DefenseMastery, "Braced: damage reduction up"),
            ClauseType.OnKillDodgeDoubleDuration    => (BuffIcon.EssenceOfWind, "Fleet: dodge chance doubled"),
            ClauseType.LowHpDodgeBurst              => (BuffIcon.EssenceOfWind, "Fleet: dodge chance up"),
            ClauseType.ReflectBurstOnCritBlock      => (BuffIcon.CounterAttack, "Retribution: reflect doubled"),
            _                                       => (default(BuffIcon), null)
        };

        if (label != null)
        {
            BuffHelper.AddCustomBuff(wearer, icon, label, duration);
        }
    }

    public static bool IsClauseBurstActive(Mobile wearer, ClauseType clause) =>
        wearer != null && _clauseBursts.TryGetValue((wearer, clause), out var until) && Core.TickCount < until;

    // Klotho: arms a fresh burst on the first kill, or stacks (max `maxStacks`) if a prior burst
    // is still active. Returns the resulting stack count.
    public static int ArmOrStackKlothoBurst(Mobile wearer, TimeSpan duration, int maxStacks)
    {
        if (wearer == null)
        {
            return 0;
        }

        var now = Core.TickCount;
        var stacks = 1;

        if (_klothoStacks.TryGetValue(wearer, out var cur) && now < cur.ExpiryTick)
        {
            stacks = Math.Min(cur.Stacks + 1, maxStacks);
        }

        _klothoStacks[wearer] = (stacks, now + (long)duration.TotalMilliseconds);

        // Buff-bar readout so the on-kill stamina-regen stack (and its remaining time) is visible.
        // Refreshes on each kill; the label carries the current stack count.
        BuffHelper.AddCustomBuff(
            wearer, BuffIcon.Rampage,
            stacks > 1 ? $"Second Wind x{stacks}: stamina regen up" : "Second Wind: stamina regen up",
            duration
        );
        return stacks;
    }

    public static int GetKlothoStacks(Mobile wearer) =>
        wearer != null && _klothoStacks.TryGetValue(wearer, out var cur) && Core.TickCount < cur.ExpiryTick
            ? cur.Stacks
            : 0;

    // Marks a once-per-fight clause as spent; returns false (and does nothing) if already used
    // this fight. Reset naturally once CombatFxState considers the fight stale (30s idle).
    public static bool TryUseOncePerFight(Mobile wearer, ClauseType clause, bool fightIsFresh)
    {
        if (wearer == null)
        {
            return false;
        }

        var key = (wearer, clause);

        if (fightIsFresh)
        {
            _usedOncePerFight.Remove(key);
        }

        if (!_usedOncePerFight.Add(key))
        {
            return false;
        }

        return true;
    }

    public static void ArmSecondaryDeflect(Mobile wearer)
    {
        if (wearer != null)
        {
            _deflectSecondary.Add(wearer);
        }
    }

    public static bool ConsumeSecondaryDeflect(Mobile wearer) =>
        wearer != null && _deflectSecondary.Remove(wearer);

    public static void Evict(Mobile wearer)
    {
        if (wearer == null)
        {
            return;
        }

        if (_aggregates.TryGetValue(wearer, out var agg))
        {
            if (agg.HasCapstone)
            {
                BuffHelper.RemoveBuff(wearer, CapstoneIcon(agg.CapstoneMaterial));
            }

            if (agg.ResonanceOffense + agg.ResonanceDefense + agg.ResonanceUtility > 0)
            {
                BuffHelper.RemoveBuff(wearer, BuffIcon.ArcaneEmpowerment);
            }

            if (agg.HasDevotion)
            {
                BuffHelper.RemoveBuff(wearer, BuffIcon.Surge);
            }
        }

        _aggregates.Remove(wearer);
        _legendaries.Remove(wearer);
        _deflectSecondary.Remove(wearer);
        _klothoStacks.Remove(wearer);

        RemoveSkillMod(wearer, _resistSkillMods);
        RemoveSkillMod(wearer, _hidingSkillMods);
        RemoveSkillMod(wearer, _stealthSkillMods);
        RemoveSkillMod(wearer, _animalTamingSkillMods);

        List<ClauseType> staleBursts = null;

        foreach (var key in _clauseBursts.Keys)
        {
            if (key.Item1 == wearer)
            {
                (staleBursts ??= new List<ClauseType>()).Add(key.Item2);
            }
        }

        if (staleBursts != null)
        {
            foreach (var clause in staleBursts)
            {
                _clauseBursts.Remove((wearer, clause));
            }
        }

        List<ClauseType> staleOnce = null;

        foreach (var key in _usedOncePerFight)
        {
            if (key.Item1 == wearer)
            {
                (staleOnce ??= new List<ClauseType>()).Add(key.Item2);
            }
        }

        if (staleOnce != null)
        {
            foreach (var clause in staleOnce)
            {
                _usedOncePerFight.Remove((wearer, clause));
            }
        }
    }

    private static void RemoveSkillMod(Mobile wearer, Dictionary<Mobile, DefaultSkillMod> table)
    {
        if (table.TryGetValue(wearer, out var mod))
        {
            mod.Remove();
            table.Remove(wearer);
        }
    }
}
