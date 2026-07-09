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
            list.Add(item.LabelNumber); // base shape, e.g. "a double axe"
        }

        if (item is BaseWeapon)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = WeaponEffectTable.Get(root, rarity);

            if (!row.IsEmpty)
            {
                list.Add(BuildWeaponSummary(row));
            }

            AddClauseLine(list, row.Signature, row.S1, row.S2, row.S3);
            AddLegendaryClauseLine(list, variant);
        }
        else if (item is BaseArmor armor)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = ArmorEffectTable.Get(root, rarity, armor is BaseShield);

            if (!row.IsEmpty)
            {
                list.Add(BuildArmorSummary(row));
            }

            AddClauseLine(list, row.Signature, row.S1, row.S2, row.S3);
            AddLegendaryClauseLine(list, variant);
        }
        else if (item is BaseJewel or BaseClothing)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = AccessoryEffectTable.Get(root, rarity, isClothing: item is BaseClothing);

            if (!row.IsEmpty)
            {
                list.Add(BuildAccessorySummary(row));
            }

            AddLegendaryClauseLine(list, variant);
        }
    }

    // ---- Single-click (pre-UOTD) mirror of the OPL lines above ---------------------------

    // T2A/pre-UOTD clients never see the OPL tooltip built by AddVariantProperties — only
    // single-click overhead text, capped at ~5 lines per item by the classic client (oldest
    // dropped first, which would otherwise push the item's own name off the label). Mirrors the
    // OPL content (base shape / numeric summary / clause text) via LabelTo instead of list.Add,
    // but compressed to at most 2 lines: shape+summary merged on one line, signature clause +
    // legendary clause merged with "; " on the other. Called from each
    // BaseWeapon/BaseArmor/BaseJewel/BaseClothing OnSingleClickPreUOTD after their existing label
    // output.
    public static void LabelVariantDetails(Mobile from, Item item)
    {
        if (item is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return;
        }

        // Base shape text, e.g. "a halberd" — item.Name is already the legendary's proper noun
        // (e.g. "Klytios") at this point, so (unlike BuildRootName) it must NOT be consulted here;
        // LabelNumber is derived from ItemID alone and is unaffected by Name.
        var shape = variant.LegendaryId != 0 ? Localization.GetText(item.LabelNumber) : null;

        if (item is BaseWeapon)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = WeaponEffectTable.Get(root, rarity);

            LabelShapeSummaryLine(from, item, shape, row.IsEmpty ? null : BuildWeaponSummary(row));
            LabelCombinedClauseLine(from, item, variant, row.Signature, row.S1, row.S2, row.S3);
        }
        else if (item is BaseArmor armor)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = ArmorEffectTable.Get(root, rarity, armor is BaseShield);

            LabelShapeSummaryLine(from, item, shape, row.IsEmpty ? null : BuildArmorSummary(row));
            LabelCombinedClauseLine(from, item, variant, row.Signature, row.S1, row.S2, row.S3);
        }
        else if (item is BaseJewel or BaseClothing)
        {
            var (root, rarity) = ResolveRootRarity(variant, ((IRarity)item).Rarity);
            var row = AccessoryEffectTable.Get(root, rarity, isClothing: item is BaseClothing);

            LabelShapeSummaryLine(from, item, shape, row.IsEmpty ? null : BuildAccessorySummary(row));
            LabelLegendaryClauseLine(from, item, variant); // no signature lane on this path
        }
    }

    // Merges the legendary base-shape with the numeric effect summary into a single line
    // ("Halberd: lifesteal 10%") so a legendary item never spends 2 of its 5-line budget on
    // shape and summary separately. Non-legendary variants have no shape, so this degrades to
    // just the summary (or nothing, unchanged from before).
    private static void LabelShapeSummaryLine(Mobile from, Item item, string shape, string summary)
    {
        var text = shape == null ? summary : string.IsNullOrEmpty(summary) ? shape : $"{shape}: {summary}";
        LabelIfNotEmpty(from, item, text);
    }

    // Merges the lane signature's clause text with the legendary's own unique clause text ("; "
    // between when both exist) into a single line, instead of one line each.
    private static void LabelCombinedClauseLine(
        Mobile from, Item item, IVariantItem variant, ClauseType signature, short s1, short s2, short s3
    )
    {
        var signatureText = ClauseText.Describe(signature, s1, s2, s3);
        string legendaryText = null;

        if (variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry))
        {
            legendaryText = ClauseText.Describe(entry.Clause, entry.P1, entry.P2, entry.P3);
        }

        var text = !string.IsNullOrEmpty(signatureText) && !string.IsNullOrEmpty(legendaryText)
            ? $"{signatureText}; {legendaryText}"
            : string.IsNullOrEmpty(signatureText) ? legendaryText : signatureText;

        LabelIfNotEmpty(from, item, CapFirst(text));
    }

    private static void LabelLegendaryClauseLine(Mobile from, Item item, IVariantItem variant)
    {
        if (variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry))
        {
            LabelIfNotEmpty(from, item, CapFirst(ClauseText.Describe(entry.Clause, entry.P1, entry.P2, entry.P3)));
        }
    }

    private static void LabelIfNotEmpty(Mobile from, Item item, string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            item.LabelTo(from, text);
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

    private static string BuildArmorSummary(in ArmorEffectRow row)
    {
        var sb = ValueStringBuilder.Create();

        AppendPct(ref sb, "AR +", row.BonusAr);
        AppendPct(ref sb, "DR +", row.DrPct);
        AppendPct(ref sb, "shrug ", row.ShrugPct);
        AppendPct(ref sb, "reflect ", row.ReflectPct);
        AppendPct(ref sb, "flame proc ", row.FlameProcPct);
        AppendPct(ref sb, "HP regen +", row.HpRegenPct);
        AppendPct(ref sb, "heals +", row.HealsReceivedPct);
        AppendPct(ref sb, "spell DR ", row.SpellDrPct);
        AppendPct(ref sb, "para resist ", row.ParaResistPct);
        AppendPct(ref sb, "weight -", row.WeightReductionPct);
        AppendPct(ref sb, "stam regen +", row.StamRegenPct);
        AppendPct(ref sb, "dodge ", row.DodgePct);
        AppendPct(ref sb, "parry ", row.ParryPct);
        AppendPct(ref sb, "parry DR ", row.ParryDrPct);

        // Re-theme lane fields + previously unprinted bool effects.
        AppendPct(ref sb, "poison resist ", row.PoisonResistPct);
        AppendPct(ref sb, "on-kill stam ", row.OnKillStamPct);
        AppendPct(ref sb, "on-kill HP ", row.OnKillHpPct);

        if (row.HidingBonus > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"+{row.HidingBonus} hiding");
        }

        if (row.SelfRepair)
        {
            AppendSeparator(ref sb);
            sb.Append("Self-repair");
        }

        if (row.AutoCure)
        {
            AppendSeparator(ref sb);
            sb.Append("Auto-cure");
        }

        if (row.ParryThorns)
        {
            AppendSeparator(ref sb);
            sb.Append("Parry thorns");
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
        AppendPct(ref sb, "spell dmg +", row.SpellDamagePct);
        AppendPct(ref sb, "mana leech ", row.ManaLeechPct);
        AppendPct(ref sb, "hit halved ", row.HitHalvedPct);
        AppendPct(ref sb, "reroll miss ", row.MissRerollPct);
        AppendPct(ref sb, "Hiding +", row.HidingBonus);
        AppendPct(ref sb, "Stealth +", row.StealthBonus);
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

            sb.Append("Night sight");
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
        AppendPct(ref sb, "dmg +", row.DamagePct);
        AppendPct(ref sb, "crit +", row.CritChancePct);
        AppendPct(ref sb, "crit dmg +", row.CritDamagePct);
        AppendPct(ref sb, "mark ", row.MarkChancePct);
        AppendPct(ref sb, "block ", row.BlockPct);
        AppendPct(ref sb, "block DR ", row.BlockDrPct);
        AppendPct(ref sb, "lifesteal ", row.LifestealPct);
        AppendPct(ref sb, "stam regen +", row.StamRegenPct);

        // Re-theme lane fields (framework §4 menu).
        AppendPct(ref sb, "splash ", row.SplashPct);
        AppendPct(ref sb, "armor pen ", row.ArmorPenPct);
        AppendPct(ref sb, "stagger ", row.StaggerProcPct);
        AppendPct(ref sb, "poison ", row.PoisonApplyPct);
        AppendPct(ref sb, "mana leech ", row.ManaLeechPct);
        AppendPct(ref sb, "elem proc ", row.ElementalProcPct);
        AppendPct(ref sb, "heal-block ", row.HealBlockProcPct);
        AppendPct(ref sb, "first hit +", row.FirstHitBonusPct);

        if (row.NthHitBonusPct > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"Every {row.NthHitN}th +{row.NthHitBonusPct}%");
        }

        if (row.RampPerStackPct > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"Ramp +{row.RampPerStackPct}%/hit (max {row.RampMaxStacks})");
        }

        AppendPct(ref sb, "on-kill stam ", row.OnKillStamPct);

        if (row.DefenderStamDrainFlat > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"Drain {row.DefenderStamDrainFlat} stam");
        }

        // Worn-side utility (held-weapon passives — staves/fencing/archery lanes).
        AppendPct(ref sb, "spell DR ", row.SpellDrPct);
        AppendPct(ref sb, "mana regen +", row.ManaRegenPct);
        AppendPct(ref sb, "dodge ", row.DodgePct);
        AppendPct(ref sb, "heals recv +", row.HealsReceivedPct);

        if (row.ResistSkillBonus > 0)
        {
            AppendSeparator(ref sb);
            sb.Append($"+{row.ResistSkillBonus} resist");
        }

        if (row.AutoCure)
        {
            AppendSeparator(ref sb);
            sb.Append("Auto-cure");
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

        // Capitalize the first letter so each effect reads as its own capitalized item
        // ("Swing +5%", "Stam regen +5%"). Labels are non-empty; idempotent for already-caps ones.
        sb.Append(char.ToUpperInvariant(label[0]));
        sb.Append(label.AsSpan(1));
        sb.Append(number[..written]);
        sb.Append("%");
    }
}
