using System;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;
using Server.Collections;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Text;

namespace Server.Engines.Rarity;

public static partial class RarityEffects
{
    // ---- Object property list -----------------------------------------------------------

    // Adds variant OPL lines below the item name. No base-shape subtitle: legendary names embed
    // the base shape since BuildLegendaryName ("Labrys Double Axe"), so a LabelNumber subtitle
    // would repeat it. Called after RaritySystem.AddRarityProperty.
    public static void AddVariantProperties(IPropertyList list, Item item)
    {
        if (item is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return;
        }

        // Approved layout (2026-07-11): [stats] -> [<myth tag>: effects] -> [lane/slot signature]
        // -> [legendary unique clause].
        if (item is BaseWeapon weapon)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = WeaponEffectTable.Get(root, rarity);

            list.Add(WeaponStatsLine(weapon, row));
            AddEffectsLines(list, root, row.IsEmpty ? null : CollectWeaponEffects(row));
            AddClauseLine(list, row.Signature, row.S1, row.S2, row.S3, weapon is BaseRanged, row.MarkBonusPct);
            AddLegendaryClauseLine(list, variant, weapon is BaseRanged, row.MarkBonusPct);
        }
        else if (item is BaseArmor armor)
        {
            var isShield = armor is BaseShield;
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = ArmorEffectTable.Get(root, rarity, isShield);

            list.Add(ArmorStatsLine(armor, row));

            // Set progress gets its own row on BOTH tooltip paths (user directive 2026-07-13);
            // the single-click armor budget still holds at exactly 5 (name+stats+set+signature+legendary).
            var set = SetLine(item);

            if (set != null)
            {
                list.Add(set);
            }

            // Option A milestone: armor (not shields) at Epic+ shows the (material x slot)
            // signature instead of the per-root one. Shields/sub-Epic keep the row's signature.
            var (signature, s1, s2, s3) = !isShield && rarity >= ItemRarity.Epic
                ? ArmorSlotSignatureTable.Get(armor.MaterialType, armor.BodyPosition)
                : (row.Signature, row.S1, row.S2, row.S3);

            AddClauseLine(list, signature, s1, s2, s3);
            AddLegendaryClauseLine(list, variant);
        }
        else if (item is BaseJewel or BaseClothing)
        {
            // Jewelry/clothing carry no damage/AR, so they have no stats line (framework §7).
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var displacing = item is BaseClothing displacingCloth && AccessoryEffectTable.IsDisplacingClothing(displacingCloth);
            var row = AccessoryEffectTable.Get(root, rarity, isClothing: item is BaseClothing, displacing);

            AddEffectsLines(list, root, row.IsEmpty ? null : CollectAccessoryEffects(row, item));
            AddLegendaryClauseLine(list, variant);
        }
    }

    // OPL effects block (user directive 2026-07-12): the theme's short myth tag as a header line
    // ("Ares:") followed by one bulleted line per effect ("• damage +10%"). The OPL has no line
    // cap, so each buff gets its own row; the single-click mirror keeps the compact comma-joined
    // form to respect the classic client's 5-line cap. Skipped when the theme prints nothing.
    private static void AddEffectsLines(IPropertyList list, VariantRoot root, List<string> parts)
    {
        if (parts == null || parts.Count == 0)
        {
            return;
        }

        list.Add(MythTagHeader(root));

        for (var i = 0; i < parts.Count; i++)
        {
            // Built ahead of the call so the string overload (single passthrough argument) is
            // used — an interpolated literal here would hit the handler overload, where bare
            // text ("• ") is a delimiter, not content (CLAUDE.md rule 14).
            var line = $"• {parts[i]}";
            list.Add(line);
        }
    }

    // ---- Single-click (pre-UOTD) mirror of the OPL lines above ---------------------------

    // T2A/pre-UOTD clients never see the OPL tooltip built by AddVariantProperties — only
    // single-click overhead text, capped at 5 lines per item by the classic client (oldest dropped
    // first, which would otherwise push the item's own name off the label). Mirrors the OPL content
    // via LabelTo instead of list.Add; the worst cases — weapon: name + stats + effects + signature
    // + legendary clause, armor: name + stats + set + signature + legendary clause — land at
    // exactly 5. Called from each BaseWeapon/BaseArmor/BaseJewel/BaseClothing
    // OnSingleClickPreUOTD after their existing label output.
    public static void LabelVariantDetails(Mobile from, Item item)
    {
        var lines = new List<string>(4);
        CollectSingleClickLines(item, lines);

        for (var i = 0; i < lines.Count; i++)
        {
            item.LabelTo(from, lines[i]);
        }
    }

    // The ordered, non-empty single-click lines a variant item emits below its name (which the
    // caller labels first). Internal + Mobile-free so the 5-line budget can be unit-tested without a
    // live client: name is 1 line, so this list must stay <= 4 for the classic cap to hold.
    internal static void CollectSingleClickLines(Item item, List<string> lines)
    {
        if (item is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return;
        }

        if (item is BaseWeapon weapon)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = WeaponEffectTable.Get(root, rarity);

            AddLine(lines, WeaponStatsLine(weapon, row));
            AddLine(lines, DurabilityLine(weapon, rarity));
            AddLine(lines, EffectsLine(root, row.IsEmpty ? null : BuildWeaponSummary(row)));
            AddLine(lines, SignatureClauseText(row.Signature, row.S1, row.S2, row.S3, weapon is BaseRanged, row.MarkBonusPct));
            AddLine(lines, LegendaryClauseText(variant, weapon is BaseRanged, row.MarkBonusPct));
        }
        else if (item is BaseArmor armor)
        {
            var isShield = armor is BaseShield;
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = ArmorEffectTable.Get(root, rarity, isShield);

            AddLine(lines, ArmorStatsLine(armor, row));
            AddLine(lines, SetLine(item)); // own line (user directive 2026-07-13); worst case is exactly 5 with name+stats+signature+legendary

            // Option A milestone: same slot-table redirect as AddVariantProperties above.
            var (signature, s1, s2, s3) = !isShield && rarity >= ItemRarity.Epic
                ? ArmorSlotSignatureTable.Get(armor.MaterialType, armor.BodyPosition)
                : (row.Signature, row.S1, row.S2, row.S3);

            AddLine(lines, CapFirst(ClauseText.Describe(signature, s1, s2, s3)));
            AddLine(lines, LegendaryClauseText(variant));
        }
        else if (item is BaseJewel or BaseClothing)
        {
            // No stats line (no damage/AR) and no lane signature on this path — just effects + unique.
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var displacing = item is BaseClothing displacingCloth && AccessoryEffectTable.IsDisplacingClothing(displacingCloth);
            var row = AccessoryEffectTable.Get(root, rarity, isClothing: item is BaseClothing, displacing);

            AddLine(lines, EffectsLine(root, row.IsEmpty ? null : BuildAccessorySummary(row, item)));
            AddLine(lines, LegendaryClauseText(variant));
        }
    }

    // Single-click effects line: the bare effect summary. Unlike the OPL, single-click drops the
    // myth tag entirely (user directive 2026-07-12 — the pantheon name is removed from the click
    // tooltip); the OPL subtitle/effects lines keep the tag for modern clients per framework §6.
    private static string EffectsLine(VariantRoot root, string effects) => effects;

    // Durability line (single-click): shown only below Legendary. Legendary variant items become
    // indestructible (Part B), so their tooltip omits durability — which also keeps the worst-case
    // legendary layout (name + stats + effects + signature + legendary) at the 5-line classic cap.
    private static string DurabilityLine(Item item, ItemRarity rarity)
    {
        if (rarity >= ItemRarity.Legendary)
        {
            return null;
        }

        var (hitPoints, maxHitPoints) = item switch
        {
            BaseWeapon weapon => (weapon.HitPoints, weapon.MaxHitPoints),
            BaseArmor armor   => (armor.HitPoints, armor.MaxHitPoints),
            _                 => (0, 0)
        };

        return hitPoints >= 0 && maxHitPoints > 0 ? $"Durability: {hitPoints}/{maxHitPoints}" : null;
    }

    private static string LegendaryClauseText(IVariantItem variant, bool ranged = false, int markBonusPct = 0) =>
        variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry)
            ? SignatureClauseText(entry.Clause, entry.P1, entry.P2, entry.P3, ranged, markBonusPct)
            : null;

    // Capitalized clause sentence for the single-click path, with the mark-family bonus injected.
    private static string SignatureClauseText(ClauseType clause, short s1, short s2, short s3, bool ranged, int markBonusPct)
    {
        (s1, s2, s3) = InjectMarkBonus(clause, s1, s2, s3, markBonusPct);
        return CapFirst(ClauseText.Describe(clause, s1, s2, s3, ranged));
    }

    private static void AddLine(List<string> lines, string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            lines.Add(text);
        }
    }

    // Framework §6 OPL order: name -> rarity line -> theme effects (above) -> unique clause
    // (here), always last. A lane signature (weapons/armor only) precedes the legendary's own
    // unique clause when both exist on the same item.
    private static void AddClauseLine(
        IPropertyList list, ClauseType clause, short s1, short s2, short s3, bool ranged = false, int markBonusPct = 0
    )
    {
        (s1, s2, s3) = InjectMarkBonus(clause, s1, s2, s3, markBonusPct);
        var text = ClauseText.Describe(clause, s1, s2, s3, ranged);

        if (!string.IsNullOrEmpty(text))
        {
            // Bulleted like the effect lines above (one power per row); built ahead of the call
            // so the string overload is used (rule 14 — see AddEffectsLines).
            var line = $"• {CapFirst(text)}";
            list.Add(line);
        }
    }

    private static void AddLegendaryClauseLine(IPropertyList list, IVariantItem variant, bool ranged = false, int markBonusPct = 0)
    {
        if (variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry))
        {
            AddClauseLine(list, entry.Clause, entry.P1, entry.P2, entry.P3, ranged, markBonusPct);
        }
    }


    // Mark-family clauses read a "+N% damage taken" value that lives on the weapon row (MarkBonusPct),
    // not in the clause params. Inject it into the clause's free p-slot before Describe runs so the
    // tooltip states the number instead of a value-less "for bonus damage". Only fills a slot the
    // clause leaves at 0, so a clause that already uses the slot for its own datum is untouched.
    // A lane with no mark bonus of its own falls back to the same DefaultMarkBonusPct the combat
    // dispatch applies, so the shown number always matches the number the mark inflicts.
    private static (short S1, short S2, short S3) InjectMarkBonus(
        ClauseType clause, short s1, short s2, short s3, int markBonusPct
    )
    {
        if (markBonusPct <= 0)
        {
            markBonusPct = DefaultMarkBonusPct;
        }

        switch (clause)
        {
            case ClauseType.MarkFirstHit:
            case ClauseType.MarkOnCrit:
            case ClauseType.PoisonTickDoubled:
                {
                    if (s1 == 0)
                    {
                        s1 = (short)markBonusPct;
                    }

                    break;
                }
            case ClauseType.MarkManaLeech:
            case ClauseType.MarkElemental:
            case ClauseType.MarkHealBlockFirstHit:
                {
                    if (s2 == 0)
                    {
                        s2 = (short)markBonusPct;
                    }

                    break;
                }
        }

        return (s1, s2, s3);
    }

    // ---- Stats line (line 2) + myth-tag prefix (line 3) --------------------------------------

    // A tooltip is built with no wielder, so the classic-era swing formula (BaseWeapon.GetDelay's
    // pre-AOS branch, 15000 / ((stam + 100) * speed)) is evaluated at a fixed reference stamina.
    // 100 is a round baseline that keeps this in step with GetDelay; the real in-fight interval
    // still scales with the wielder's actual stamina.
    private const int ReferenceStamina = 100;

    // Weapon stats line. weapon.MinDamage/MaxDamage now resolve the runtime rarity anchor for variant
    // weapons (framework §2/§7; 2026-07-11 directive) — the shown range is that anchor folded with the
    // theme's row.DamagePct bonus, exactly what combat rolls. weapon.Speed likewise resolves the base's
    // swing-seconds anchor, so SwingSeconds folds the wired SwingSpeedPct over that anchored base time.
    private static string WeaponStatsLine(BaseWeapon weapon, in WeaponEffectRow row)
    {
        var min = weapon.MinDamage + weapon.MinDamage * row.DamagePct / 100;
        var max = weapon.MaxDamage + weapon.MaxDamage * row.DamagePct / 100;
        var damage = min == max ? $"{min}" : $"{min}-{max}";
        var seconds = SwingSeconds(weapon, row.SwingSpeedPct);

        return seconds > 0 ? $"Damage: {damage}, Speed: {seconds:0.0}s" : $"Damage: {damage}";
    }

    private static double SwingSeconds(BaseWeapon weapon, int swingSpeedPct)
    {
        double speed = weapon.Speed;

        if (speed <= 0)
        {
            return 0;
        }

        var baseSeconds = 15000.0 / ((ReferenceStamina + 100) * speed);

        return baseSeconds * 100.0 / (100 + swingSpeedPct);
    }


    // The theme's short myth tag (framework §3) as the OPL effects-block header ("Ares:").
    // Tags are stored lowercase where they are concepts ("unbreakable") — capitalize uniformly so
    // god names and concept tags read the same way at line start ("Ares:", "Unbreakable:").
    private static string MythTagHeader(VariantRoot root) => $"{CapFirst(VariantRootInfo.GetMythTag(root))}:";

    // Stats line carries ALL lane effects, comma-joined (user directive 2026-07-13):
    // "12 Armor, Spell damage taken -8%, +5 Magic Resistance". Set progress is a separate
    // line on both tooltip paths.
    private static string ArmorStatsLine(BaseArmor armor, in ArmorEffectRow row)
    {
        var line = $"{(int)Math.Round(armor.ArmorRatingScaled)} Armor";
        var effects = BuildArmorSummary(row);

        return effects != null ? $"{line}, {effects}" : line;
    }
    private static string BuildArmorSummary(in ArmorEffectRow row) => JoinParts(CollectArmorEffects(row));

    private static string BuildAccessorySummary(in AccessoryEffectRow row, Item source) =>
        JoinParts(CollectAccessoryEffects(row, source));

    private static string BuildWeaponSummary(in WeaponEffectRow row) => JoinParts(CollectWeaponEffects(row));

    private static List<string> CollectArmorEffects(in ArmorEffectRow row)
    {
        var parts = new List<string>();

        // Bonus AR is NOT listed here: BaseArmor.ArmorRating already folds GetBonusArmorRating
        // in, so the "Armor: N" number the effects ride on displays it — a separate "armor +N"
        // part would double-report it.

        AddPct(parts, "damage reduction +", row.DrPct);

        // Shrug halves the incoming hit (RarityEffects.Defense: damage /= 2).
        AddPct(parts, "chance to shrug +", row.ShrugPct);

        if (row.ReflectPct != 0)
        {
            parts.Add($"reflects {row.ReflectPct}% damage back");
        }

        AddPct(parts, "chance to burn the attacker +", row.FlameProcPct);
        AddPct(parts, "health regen +", row.HpRegenPct);
        AddPct(parts, "healing received +", row.HealsReceivedPct);
        AddPct(parts, "spell damage taken -", row.SpellDrPct);
        AddPct(parts, "chance to resist paralyze +", row.ParaResistPct);

        // Flat skill points (Herkos/Tritonian ward lanes at Epic+), not a percent.
        if (row.ResistSkillBonus > 0)
        {
            parts.Add($"+{row.ResistSkillBonus} Magic Resistance");
        }
        // WeightReductionPct now models a carry-capacity boost folded into the wearer's MaxWeight
        // (user directive 2026-07-12), not a reduction of the piece's own weight.
        AddPct(parts, "carry capacity +", row.WeightReductionPct);
        AddPct(parts, "stamina regen +", row.StamRegenPct);
        AddPct(parts, "chance to dodge +", row.DodgePct);
        AddPct(parts, "chance to parry +", row.ParryPct);
        AddPct(parts, "damage reduction while parrying +", row.ParryDrPct);

        // Re-theme lane fields + previously unprinted bool effects.
        AddPct(parts, "chance to resist poison +", row.PoisonResistPct);

        if (row.OnKillStamPct != 0)
        {
            parts.Add($"on kill: restores {row.OnKillStamPct}% stamina");
        }

        if (row.OnKillHpPct != 0)
        {
            parts.Add($"on kill: restores {row.OnKillHpPct}% health");
        }

        if (row.HidingBonus > 0)
        {
            parts.Add($"+{row.HidingBonus} hiding");
        }

        if (row.AutoCure)
        {
            parts.Add("auto-cure");
        }

        if (row.ParryThorns)
        {
            parts.Add("parry thorns");
        }

        return CapEach(parts);
    }

    private static List<string> CollectAccessoryEffects(in AccessoryEffectRow row, Item source)
    {
        var parts = new List<string>();

        // The Olympian stat bonus is a flat point boost to the rolled stat (persisted on the jewel).
        if (row.StatBonus != 0)
        {
            var statName = source is BaseJewel jewel ? StatWord(jewel.OlympianStat) : "to a stat";
            parts.Add($"+{row.StatBonus} {statName}");
        }

        AddPct(parts, "chance on hit to call lightning +", row.LightningProcPct);
        AddPct(parts, "mana regen +", row.ManaRegenPct);
        AddPct(parts, "spell damage +", row.SpellDamagePct);
        AddPct(parts, "chance to leech mana on spells +", row.ManaLeechPct);
        AddPct(parts, "chance to shrug +", row.HitHalvedPct);
        AddPct(parts, "chance to reroll your misses +", row.MissRerollPct);

        if (row.HidingBonus != 0)
        {
            parts.Add($"hiding +{row.HidingBonus}");
        }

        if (row.StealthBonus != 0)
        {
            parts.Add($"stealth +{row.StealthBonus}");
        }

        AddPct(parts, "chance to resist poison +", row.PoisonResistPct);
        AddPct(parts, "all regen +", row.AllRegenPct);
        AddPct(parts, "potion effects +", row.PotionEffectPct);

        if (row.OnKillStamina != 0)
        {
            parts.Add($"on kill: restores {row.OnKillStamina} stamina");
        }

        if (row.OnKillHp != 0)
        {
            parts.Add($"on kill: restores {row.OnKillHp} health");
        }

        AddPct(parts, "karma gained +", row.KarmaGainPct);

        if (row.VendorPricePct != 0)
        {
            parts.Add($"vendor prices {row.VendorPricePct}% better");
        }

        AddPct(parts, "chance to enter frenzy when being hit +", row.FrenzyChancePct);
        if (row.StationaryRegenPct > 0)
        {
            var kind = row.StationaryAppliesMana ? "HP & mana regen" : "HP regen";
            parts.Add($"{kind} while standing still +{row.StationaryRegenPct}%");
        }
        AddPct(parts, "chance to dodge +", row.DodgePct);

        if (row.NightSight)
        {
            parts.Add("night sight");
        }

        return CapEach(parts);
    }

    private static string StatWord(StatType stat) => stat switch
    {
        StatType.Dex => "dexterity",
        StatType.Int => "intelligence",
        _            => "strength"
    };

    private static List<string> CollectWeaponEffects(in WeaponEffectRow row)
    {
        var parts = new List<string>();

        AddPct(parts, "attack speed +", row.SwingSpeedPct);
        AddPct(parts, "chance to hit +", row.HitChancePct);
        AddPct(parts, "chance to hit twice +", row.ExtraSwingPct);
        AddPct(parts, "damage +", row.DamagePct);
        AddPct(parts, "crit chance +", row.CritChancePct);
        AddPct(parts, "crit damage +", row.CritDamagePct);
        AddPct(parts, "chance to mark the target +", row.MarkChancePct);
        AddPct(parts, "chance to block +", row.BlockPct);
        AddPct(parts, "damage reduction while blocking +", row.BlockDrPct);
        AddPct(parts, "lifesteal ", row.LifestealPct);

        // Weapon-row "stamina regen" is really an on-hit leech (see WeaponEffectRow); the armor row
        // by the same name is real regen.
        AddPct(parts, "stamina leech ", row.StamRegenPct);

        // Re-theme lane fields (framework §4 menu).
        AddPct(parts, "splash damage ", row.SplashPct);

        if (row.ArmorPenPct != 0)
        {
            parts.Add($"penetrates {row.ArmorPenPct}% of armor");
        }

        AddPct(parts, "chance to stagger +", row.StaggerProcPct);
        AddPct(parts, "chance to poison +", row.PoisonApplyPct);
        AddPct(parts, "chance to leech mana +", row.ManaLeechPct);
        AddPct(
            parts,
            row.ElementalKind == 1 ? "chance on hit to call flame +" : "chance on hit to call lightning +",
            row.ElementalProcPct
        );
        AddPct(parts, "chance to block heals +", row.HealBlockProcPct);

        if (row.FirstHitBonusPct != 0)
        {
            parts.Add($"first hit deals +{row.FirstHitBonusPct}% damage");
        }

        if (row.NthHitBonusPct > 0)
        {
            parts.Add($"every {ClauseText.Ord(row.NthHitN)} hit deals +{row.NthHitBonusPct}% damage");
        }

        if (row.RampPerStackPct > 0)
        {
            parts.Add($"consecutive hits +{row.RampPerStackPct}% damage each (max {row.RampMaxStacks})");
        }

        if (row.OnKillStamPct != 0)
        {
            parts.Add($"on kill: restores {row.OnKillStamPct}% stamina");
        }

        if (row.DefenderStamDrainFlat > 0)
        {
            parts.Add($"drains {row.DefenderStamDrainFlat} stamina on hit");
        }

        // Worn-side utility (held-weapon passives — staves/fencing/archery lanes).
        AddPct(parts, "spell damage taken -", row.SpellDrPct);
        AddPct(parts, "mana regen +", row.ManaRegenPct);
        AddPct(parts, "chance to dodge +", row.DodgePct);
        AddPct(parts, "healing received +", row.HealsReceivedPct);

        if (row.ResistSkillBonus > 0)
        {
            parts.Add($"Magic Resistance +{row.ResistSkillBonus}");
        }

        if (row.AutoCure)
        {
            parts.Add("auto-cure");
        }

        return CapEach(parts);
    }

    // Capitalizes the first letter of a clause sentence ("every 3rd hit..." -> "Every 3rd hit...").
    // Null/empty or already-capital passes through unchanged.
    private static string CapFirst(string s) =>
        string.IsNullOrEmpty(s) || char.IsUpper(s[0]) ? s : $"{char.ToUpperInvariant(s[0])}{s.AsSpan(1)}";

    // Labels are authored in final casing (lowercase words) so the effects read as a lowercase
    // list under the capitalized myth-tag header. Cold display path — the per-part strings and
    // the click-path join are acceptable allocations (existing convention in this file).
    private static void AddPct(List<string> parts, string label, int pct)
    {
        if (pct != 0)
        {
            parts.Add($"{label}{pct}%");
        }
    }

    private static string JoinParts(List<string> parts) =>
        parts == null || parts.Count == 0 ? null : string.Join(", ", parts);

    // Capitalizes the first letter of every effect part in place ("chance to hit +8%" ->
    // "Chance to hit +8%"), so each phrase reads as its own titled entry both as an OPL bullet
    // and inside the comma-joined single-click line (user directive 2026-07-12).
    private static List<string> CapEach(List<string> parts)
    {
        for (var i = 0; i < parts.Count; i++)
        {
            parts[i] = CapFirst(parts[i]);
        }

        return parts;
    }

    // ---- Set tracking (how many pieces of this root are worn) -----------------------------

    // Returns a "Set (worn/threshold)" line for body armor, mirroring the P4 capstone gate in
    // WornEffectState.Rebuild exactly: Epic+ non-shield variant pieces of this MATERIAL (roots can
    // mix), threshold = the family's CapstoneThreshold (min(4, slots): chainmail 3, rest 4).
    // Null when the item is not worn, sub-Epic (it wouldn't count toward the set), or the
    // material has no capstone.
    private static string SetLine(Item item)
    {
        if (item is not BaseArmor armor || armor is BaseShield)
        {
            return null;
        }

        if (item.RootParent is not Mobile wearer)
        {
            return null;
        }

        if (item is not IVariantItem variant ||
            ResolveRootRarity(variant, ((IRarity)armor).Rarity).rarity < ItemRarity.Epic)
        {
            return null;
        }

        var material = armor.MaterialType;

        if (!FamilyRegistry.ArmorFamilyByMaterial.TryGetValue(material, out var family) ||
            family.CapstoneThreshold <= 0)
        {
            return null;
        }

        var count = 0;
        var wornItems = wearer.Items;

        for (var i = 0; i < wornItems.Count; i++)
        {
            if (wornItems[i] is not BaseArmor wornArmor || wornArmor is BaseShield ||
                wornArmor.MaterialType != material || wornArmor is not IVariantItem wornVariant)
            {
                continue;
            }

            if (ResolveRootRarity(wornVariant, ((IRarity)wornArmor).Rarity).rarity >= ItemRarity.Epic)
            {
                count++;
            }
        }

        // Wearing more pieces than the capstone needs (e.g. 5 plate on a 4-threshold) reads as
        // "5/4" — clamp so a complete set always shows exactly full.
        var shown = Math.Min(count, family.CapstoneThreshold);

        // "Grave-Chill (2/4): your hits heal-block the target" (user directive 2026-07-13).
        return $"{family.CapstoneName} ({shown}/{family.CapstoneThreshold}): {WornEffectState.CapstoneEffectText(material)}";
    }
 }
