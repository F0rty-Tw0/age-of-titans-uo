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

    // Adds variant OPL lines: for legendaries the base-shape subtitle (framework §6), and a
    // compact effect summary for weapon variants. Called after RaritySystem.AddRarityProperty.
    public static void AddVariantProperties(IPropertyList list, Item item)
    {
        if (item is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return;
        }

        if (variant.LegendaryId != 0)
        {
            list.Add(item.LabelNumber); // base shape subtitle, e.g. "a double axe" (framework §6)
        }

        // Approved layout (2026-07-11): [stats] -> [<myth tag> — effects] -> [lane/slot signature]
        // -> [legendary unique clause]. The OPL has no line cap, so the shape subtitle above stays
        // its own line; the single-click mirror (LabelVariantDetails) merges it to fit the 5-line cap.
        if (item is BaseWeapon weapon)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = WeaponEffectTable.Get(root, rarity);

            list.Add(WeaponStatsLine(weapon, row));
            AddEffectsLine(list, root, row.IsEmpty ? null : BuildWeaponSummary(row));
            AddClauseLine(list, row.Signature, row.S1, row.S2, row.S3);
            AddLegendaryClauseLine(list, variant);
        }
        else if (item is BaseArmor armor)
        {
            var isShield = armor is BaseShield;
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = ArmorEffectTable.Get(root, rarity, isShield);

            list.Add(ArmorStatsLine(armor));
            AddEffectsLine(list, root, row.IsEmpty ? null : BuildArmorSummary(row));

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

            AddEffectsLine(list, root, row.IsEmpty ? null : BuildAccessorySummary(row));
            AddLegendaryClauseLine(list, variant);
        }
    }

    // Effects line: the theme's numeric summary prefixed with its short myth tag (framework §3),
    // e.g. "Ares — damage +10%, crit chance +10%". Skipped when the theme has no printable effects.
    private static void AddEffectsLine(IPropertyList list, VariantRoot root, string effects)
    {
        var text = PrefixMythTag(root, effects);

        if (!string.IsNullOrEmpty(text))
        {
            list.Add(text);
        }
    }

    // ---- Single-click (pre-UOTD) mirror of the OPL lines above ---------------------------

    // T2A/pre-UOTD clients never see the OPL tooltip built by AddVariantProperties — only
    // single-click overhead text, capped at 5 lines per item by the classic client (oldest dropped
    // first, which would otherwise push the item's own name off the label). Mirrors the OPL content
    // via LabelTo instead of list.Add, but the legendary base-shape shares the effects line (rather
    // than taking its own) so the worst case — name + stats + effects + signature clause + legendary
    // clause — lands at exactly 5. Called from each BaseWeapon/BaseArmor/BaseJewel/BaseClothing
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
            AddLine(lines, EffectsLine(root, row.IsEmpty ? null : BuildWeaponSummary(row)));
            AddLine(lines, CapFirst(ClauseText.Describe(row.Signature, row.S1, row.S2, row.S3)));
            AddLine(lines, LegendaryClauseText(variant));
        }
        else if (item is BaseArmor armor)
        {
            var isShield = armor is BaseShield;
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = ArmorEffectTable.Get(root, rarity, isShield);

            AddLine(lines, ArmorStatsLine(armor));
            AddLine(lines, EffectsLine(root, row.IsEmpty ? null : BuildArmorSummary(row)));

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

            AddLine(lines, EffectsLine(root, row.IsEmpty ? null : BuildAccessorySummary(row)));
            AddLine(lines, LegendaryClauseText(variant));
        }
    }

    // Single-click effects line: the myth-tagged effect summary. The legendary base shape is NOT
    // shown on single-click (user directive 2026-07-11 — the item graphic already shows the shape);
    // the OPL subtitle line keeps it for modern clients per framework §6.
    private static string EffectsLine(VariantRoot root, string effects) => PrefixMythTag(root, effects);

    private static string LegendaryClauseText(IVariantItem variant) =>
        variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry)
            ? CapFirst(ClauseText.Describe(entry.Clause, entry.P1, entry.P2, entry.P3))
            : null;

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
    private static void AddClauseLine(IPropertyList list, ClauseType clause, short s1, short s2, short s3)
    {
        var text = ClauseText.Describe(clause, s1, s2, s3);

        if (!string.IsNullOrEmpty(text))
        {
            list.Add(CapFirst(text));
        }
    }

    private static void AddLegendaryClauseLine(IPropertyList list, IVariantItem variant)
    {
        if (variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry))
        {
            AddClauseLine(list, entry.Clause, entry.P1, entry.P2, entry.P3);
        }
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

        return seconds > 0 ? $"Damage {damage}, speed {seconds:0.0}s" : $"Damage {damage}";
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

    // Armor/shield effective rating — BaseArmor.ArmorRating already folds in the variant bonus AR
    // (RarityEffects.GetBonusArmorRating) and durability scaling, so it is the live effective value.
    private static string ArmorStatsLine(BaseArmor armor) => $"Armor {(int)Math.Round(armor.ArmorRating)}";

    // Prefixes the theme's short myth tag (framework §3) onto its effect summary. Cold display path,
    // so the single tag-join allocation is fine (mirrors the ClauseText/Build*Summary convention).
    // Tags are stored lowercase where they are concepts ("unbreakable") — capitalize uniformly so
    // god names and concept tags read the same way at line start ("Ares —", "Unbreakable —").
    private static string PrefixMythTag(VariantRoot root, string effects) =>
        string.IsNullOrEmpty(effects) ? null : $"{CapFirst(VariantRootInfo.GetMythTag(root))} — {effects}";

    private static string BuildArmorSummary(in ArmorEffectRow row)
    {
        var sb = ValueStringBuilder.Create();

        // Bonus AR is a flat armor value (folded into the "Armor N" stats line), not a percent — so
        // it uses a flat "+N" append, unlike the "%" effects below.
        if (row.BonusAr > 0)
        {
            sb.Append($"armor +{row.BonusAr}");
        }

        AppendPct(ref sb, "damage reduction +", row.DrPct);
        AppendPct(ref sb, "shrug ", row.ShrugPct);
        AppendPct(ref sb, "reflect ", row.ReflectPct);
        AppendPct(ref sb, "flame proc ", row.FlameProcPct);
        AppendPct(ref sb, "HP regen +", row.HpRegenPct);
        AppendPct(ref sb, "heals +", row.HealsReceivedPct);
        AppendPct(ref sb, "spell damage reduction ", row.SpellDrPct);
        AppendPct(ref sb, "paralyze resist ", row.ParaResistPct);
        AppendPct(ref sb, "weight -", row.WeightReductionPct);
        AppendPct(ref sb, "stamina regen +", row.StamRegenPct);
        AppendPct(ref sb, "dodge ", row.DodgePct);
        AppendPct(ref sb, "parry ", row.ParryPct);
        AppendPct(ref sb, "parry damage reduction ", row.ParryDrPct);

        // Re-theme lane fields + previously unprinted bool effects.
        AppendPct(ref sb, "poison resist ", row.PoisonResistPct);
        AppendPct(ref sb, "on-kill stamina ", row.OnKillStamPct);
        AppendPct(ref sb, "on-kill HP ", row.OnKillHpPct);

        if (row.HidingBonus > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"+{row.HidingBonus} hiding");
        }

        if (row.SelfRepair)
        {
            AppendSeparator(ref sb);
            sb.Append("self-repair");
        }

        if (row.AutoCure)
        {
            AppendSeparator(ref sb);
            sb.Append("auto-cure");
        }

        if (row.ParryThorns)
        {
            AppendSeparator(ref sb);
            sb.Append("parry thorns");
        }

        var result = sb.ToString();
        sb.Dispose();
        return result;
    }

    private static string BuildAccessorySummary(in AccessoryEffectRow row)
    {
        var sb = ValueStringBuilder.Create();

        AppendPct(ref sb, "stat +", row.StatBonus);
        AppendPct(ref sb, "lightning proc ", row.LightningProcPct);
        AppendPct(ref sb, "mana regen +", row.ManaRegenPct);
        AppendPct(ref sb, "spell damage +", row.SpellDamagePct);
        AppendPct(ref sb, "mana leech ", row.ManaLeechPct);
        AppendPct(ref sb, "hit halved ", row.HitHalvedPct);
        AppendPct(ref sb, "reroll miss ", row.MissRerollPct);
        AppendPct(ref sb, "hiding +", row.HidingBonus);
        AppendPct(ref sb, "stealth +", row.StealthBonus);
        AppendPct(ref sb, "poison resist ", row.PoisonResistPct);
        AppendPct(ref sb, "all regen +", row.AllRegenPct);
        AppendPct(ref sb, "potion effect +", row.PotionEffectPct);
        AppendPct(ref sb, "on-kill stamina +", row.OnKillStamina);
        AppendPct(ref sb, "on-kill HP +", row.OnKillHp);
        AppendPct(ref sb, "karma +", row.KarmaGainPct);
        AppendPct(ref sb, "better prices ", row.VendorPricePct);
        AppendPct(ref sb, "frenzy ", row.FrenzyChancePct);
        AppendPct(ref sb, "stationary regen +", row.StationaryRegenPct);
        AppendPct(ref sb, "dodge ", row.DodgePct);

        if (row.NightSight)
        {
            if (sb.Length > 0)
            {
                sb.Append(", ");
            }

            sb.Append("night sight");
        }

        var result = sb.ToString();
        sb.Dispose();
        return result;
    }

    private static string BuildWeaponSummary(in WeaponEffectRow row)
    {
        var sb = ValueStringBuilder.Create();

        AppendPct(ref sb, "swing +", row.SwingSpeedPct);
        AppendPct(ref sb, "hit +", row.HitChancePct);
        AppendPct(ref sb, "extra-swing ", row.ExtraSwingPct);
        AppendPct(ref sb, "damage +", row.DamagePct);
        AppendPct(ref sb, "crit +", row.CritChancePct);
        AppendPct(ref sb, "crit damage +", row.CritDamagePct);
        AppendPct(ref sb, "mark ", row.MarkChancePct);
        AppendPct(ref sb, "block ", row.BlockPct);
        AppendPct(ref sb, "block damage reduction ", row.BlockDrPct);
        AppendPct(ref sb, "lifesteal ", row.LifestealPct);
        AppendPct(ref sb, "stamina regen +", row.StamRegenPct);

        // Re-theme lane fields (framework §4 menu).
        AppendPct(ref sb, "splash ", row.SplashPct);
        AppendPct(ref sb, "armor penetration ", row.ArmorPenPct);
        AppendPct(ref sb, "stagger ", row.StaggerProcPct);
        AppendPct(ref sb, "poison ", row.PoisonApplyPct);
        AppendPct(ref sb, "mana leech ", row.ManaLeechPct);
        AppendPct(ref sb, row.ElementalKind == 1 ? "fire proc " : "lightning proc ", row.ElementalProcPct);
        AppendPct(ref sb, "heal-block ", row.HealBlockProcPct);
        AppendPct(ref sb, "first hit +", row.FirstHitBonusPct);

        if (row.NthHitBonusPct > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"every {row.NthHitN}th +{row.NthHitBonusPct}%");
        }

        if (row.RampPerStackPct > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"ramp +{row.RampPerStackPct}%/hit (max {row.RampMaxStacks})");
        }

        AppendPct(ref sb, "on-kill stamina ", row.OnKillStamPct);

        if (row.DefenderStamDrainFlat > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"drain {row.DefenderStamDrainFlat} stamina");
        }

        // Worn-side utility (held-weapon passives — staves/fencing/archery lanes).
        AppendPct(ref sb, "spell damage reduction ", row.SpellDrPct);
        AppendPct(ref sb, "mana regen +", row.ManaRegenPct);
        AppendPct(ref sb, "dodge ", row.DodgePct);
        AppendPct(ref sb, "heals received +", row.HealsReceivedPct);

        if (row.ResistSkillBonus > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"+{row.ResistSkillBonus} resist");
        }

        if (row.AutoCure)
        {
            AppendSeparator(ref sb);
            sb.Append("auto-cure");
        }

        var result = sb.ToString();
        sb.Dispose();
        return result;
    }

    // Capitalizes the first letter of a clause sentence ("every 3rd hit..." -> "Every 3rd hit...").
    // Null/empty or already-capital passes through unchanged.
    private static string CapFirst(string s) =>
        string.IsNullOrEmpty(s) || char.IsUpper(s[0]) ? s : $"{char.ToUpperInvariant(s[0])}{s.AsSpan(1)}";

    private static void AppendSeparator(ref ValueStringBuilder sb)
    {
        if (sb.Length > 0)
        {
            sb.Append(", ");
        }
    }

    private static void AppendPct(ref ValueStringBuilder sb, string label, int pct)
    {
        if (pct == 0)
        {
            return;
        }

        if (sb.Length > 0)
        {
            sb.Append(", ");
        }

        Span<char> number = stackalloc char[8];
        pct.TryFormat(number, out var written);

        // Labels are authored in final casing (lowercase words, acronyms like "AR"/"HP" kept) so
        // the effects read as a lowercase list after the capitalized myth tag ("Ares — damage +10%,
        // crit chance +10%"). Appended verbatim — no per-item capitalization.
        sb.Append(label);
        sb.Append(number[..written]);
        sb.Append("%");
    }
}
