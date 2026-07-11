using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Server.Engines.Rarity;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

// Throwaway golden-snapshot harness for the Phase 2 FamilyDefinition refactor (see task brief).
// Dumps every rarity-engine table through its stable public facade (plus reflection for the two
// non-public capstone/type-map members) to a text file, in canonical order. Run before the
// refactor -> golden-before.txt; run after -> golden-after.txt; diff must be byte-identical.
// Set RARITY_GOLDEN_PATH to choose the output file. Delete this file once the refactor lands.
public class RarityGoldenSnapshotTests
{
    [Fact]
    public void DumpGoldenSnapshot()
    {
        var path = Environment.GetEnvironmentVariable("RARITY_GOLDEN_PATH");

        if (string.IsNullOrEmpty(path))
        {
            return; // no-op in the normal suite; only runs when explicitly pointed at a file
        }

        var sb = new StringBuilder();

        DumpVariantRoots(sb);
        DumpWeaponTable(sb);
        DumpArmorTable(sb);
        DumpAccessoryTable(sb);
        DumpClothingDisplacingTable(sb);
        DumpSlotSignatures(sb);
        DumpLegendaries(sb);
        DumpWeaponFamilyMap(sb);
        DumpCapstones(sb);

        File.WriteAllText(path, sb.ToString());
    }

    private static readonly ItemRarity[] _rarities =
    {
        ItemRarity.Common, ItemRarity.Uncommon, ItemRarity.Rare, ItemRarity.Epic, ItemRarity.Legendary
    };

    private static void DumpVariantRoots(StringBuilder sb)
    {
        sb.AppendLine("== VARIANT ROOTS ==");

        foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
        {
            sb.Append((int)root).Append('\t').Append(root).Append('\t')
                .Append('"').Append(VariantRootInfo.GetDisplayName(root)).Append('"').Append('\t')
                .Append("stackGroup=").Append(VariantRootInfo.GetStackGroup(root)).Append('\t')
                .Append("hues=");

            foreach (var rarity in _rarities)
            {
                sb.Append(VariantRootInfo.GetBodyHue(root, rarity)).Append(',');
            }

            sb.AppendLine();
        }

        sb.Append("RootCount=").Append(VariantRootInfo.RootCount)
            .Append(" StackGroupCount=").Append(VariantRootInfo.StackGroupCount).AppendLine();
    }

    private static void DumpWeaponTable(StringBuilder sb)
    {
        sb.AppendLine("== WEAPON EFFECT TABLE ==");

        foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
        {
            foreach (var rarity in _rarities)
            {
                var r = WeaponEffectTable.Get(root, rarity);

                if (r.IsEmpty)
                {
                    continue;
                }

                sb.Append((int)root).Append(':').Append(rarity).Append('\t')
                    .Append("Swing=").Append(r.SwingSpeedPct)
                    .Append(" Hit=").Append(r.HitChancePct)
                    .Append(" ExtraSwing=").Append(r.ExtraSwingPct)
                    .Append(" Dmg=").Append(r.DamagePct)
                    .Append(" Crit=").Append(r.CritChancePct)
                    .Append(" CritDmg=").Append(r.CritDamagePct)
                    .Append(" Mark=").Append(r.MarkChancePct)
                    .Append(" MarkBonus=").Append(r.MarkBonusPct)
                    .Append(" MarkPoison=").Append(r.MarkPoisonTick)
                    .Append(" Block=").Append(r.BlockPct)
                    .Append(" BlockDr=").Append(r.BlockDrPct)
                    .Append(" BlockThorns=").Append(r.BlockThorns)
                    .Append(" Lifesteal=").Append(r.LifestealPct)
                    .Append(" StamRegen=").Append(r.StamRegenPct)
                    .Append(" LifestealExec=").Append(r.LifestealExecute)
                    .Append(" Splash=").Append(r.SplashPct)
                    .Append(" ArmorPen=").Append(r.ArmorPenPct)
                    .Append(" Stagger=").Append(r.StaggerProcPct)
                    .Append(" PoisonApply=").Append(r.PoisonApplyPct)
                    .Append(" PoisonTier=").Append(r.PoisonTier)
                    .Append(" ManaLeech=").Append(r.ManaLeechPct)
                    .Append(" Elemental=").Append(r.ElementalProcPct)
                    .Append(" ElemKind=").Append(r.ElementalKind)
                    .Append(" HealBlock=").Append(r.HealBlockProcPct)
                    .Append(" NthHitBonus=").Append(r.NthHitBonusPct)
                    .Append(" NthHitN=").Append(r.NthHitN)
                    .Append(" Ramp=").Append(r.RampPerStackPct)
                    .Append(" RampMax=").Append(r.RampMaxStacks)
                    .Append(" FirstHit=").Append(r.FirstHitBonusPct)
                    .Append(" OnKillStam=").Append(r.OnKillStamPct)
                    .Append(" DefStamDrain=").Append(r.DefenderStamDrainFlat)
                    .Append(" SpellDr=").Append(r.SpellDrPct)
                    .Append(" ManaRegen=").Append(r.ManaRegenPct)
                    .Append(" Dodge=").Append(r.DodgePct)
                    .Append(" ResistSkill=").Append(r.ResistSkillBonus)
                    .Append(" HealsRecv=").Append(r.HealsReceivedPct)
                    .Append(" AutoCure=").Append(r.AutoCure)
                    .Append(" Sig=").Append(r.Signature)
                    .Append(" S1=").Append(r.S1).Append(" S2=").Append(r.S2).Append(" S3=").Append(r.S3)
                    .AppendLine();
            }
        }
    }

    private static void DumpArmorTable(StringBuilder sb)
    {
        sb.AppendLine("== ARMOR EFFECT TABLE ==");

        foreach (var isShield in new[] { false, true })
        {
            foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
            {
                foreach (var rarity in _rarities)
                {
                    var r = ArmorEffectTable.Get(root, rarity, isShield);

                    if (r.IsEmpty)
                    {
                        continue;
                    }

                    sb.Append(isShield ? "shield " : "armor ")
                        .Append((int)root).Append(':').Append(rarity).Append('\t')
                        .Append("BonusAr=").Append(r.BonusAr)
                        .Append(" Dr=").Append(r.DrPct)
                        .Append(" Shrug=").Append(r.ShrugPct)
                        .Append(" Reflect=").Append(r.ReflectPct)
                        .Append(" SelfRepair=").Append(r.SelfRepair)
                        .Append(" Flame=").Append(r.FlameProcPct)
                        .Append(" HpRegen=").Append(r.HpRegenPct)
                        .Append(" HealsRecv=").Append(r.HealsReceivedPct)
                        .Append(" AutoCure=").Append(r.AutoCure)
                        .Append(" SpellDr=").Append(r.SpellDrPct)
                        .Append(" ParaResist=").Append(r.ParaResistPct)
                        .Append(" ResistSkill=").Append(r.ResistSkillBonus)
                        .Append(" WeightRed=").Append(r.WeightReductionPct)
                        .Append(" StamRegen=").Append(r.StamRegenPct)
                        .Append(" Dodge=").Append(r.DodgePct)
                        .Append(" Parry=").Append(r.ParryPct)
                        .Append(" ParryDr=").Append(r.ParryDrPct)
                        .Append(" ParryThorns=").Append(r.ParryThorns)
                        .Append(" PoisonResist=").Append(r.PoisonResistPct)
                        .Append(" Hiding=").Append(r.HidingBonus)
                        .Append(" OnKillStam=").Append(r.OnKillStamPct)
                        .Append(" OnKillHp=").Append(r.OnKillHpPct)
                        .Append(" Sig=").Append(r.Signature)
                        .Append(" S1=").Append(r.S1).Append(" S2=").Append(r.S2).Append(" S3=").Append(r.S3)
                        .AppendLine();
                }
            }
        }
    }

    private static void DumpAccessoryTable(StringBuilder sb)
    {
        sb.AppendLine("== ACCESSORY EFFECT TABLE ==");

        foreach (var isClothing in new[] { false, true })
        {
            foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
            {
                foreach (var rarity in _rarities)
                {
                    var r = AccessoryEffectTable.Get(root, rarity, isClothing);

                    if (r.IsEmpty)
                    {
                        continue;
                    }

                    sb.Append(isClothing ? "cloth " : "jewel ")
                        .Append((int)root).Append(':').Append(rarity).Append('\t')
                        .Append("Stat=").Append(r.StatBonus)
                        .Append(" Lightning=").Append(r.LightningProcPct)
                        .Append(" ManaRegen=").Append(r.ManaRegenPct)
                        .Append(" SpellDmg=").Append(r.SpellDamagePct)
                        .Append(" ManaLeech=").Append(r.ManaLeechPct)
                        .Append(" HitHalved=").Append(r.HitHalvedPct)
                        .Append(" MissReroll=").Append(r.MissRerollPct)
                        .Append(" Hiding=").Append(r.HidingBonus)
                        .Append(" Stealth=").Append(r.StealthBonus)
                        .Append(" NightSight=").Append(r.NightSight)
                        .Append(" PoisonResist=").Append(r.PoisonResistPct)
                        .Append(" AllRegen=").Append(r.AllRegenPct)
                        .Append(" Potion=").Append(r.PotionEffectPct)
                        .Append(" OnKillStam=").Append(r.OnKillStamina)
                        .Append(" OnKillHp=").Append(r.OnKillHp)
                        .Append(" Karma=").Append(r.KarmaGainPct)
                        .Append(" Vendor=").Append(r.VendorPricePct)
                        .Append(" FrenzyChance=").Append(r.FrenzyChancePct)
                        .Append(" FrenzyDmg=").Append(r.FrenzyDamagePct)
                        .Append(" FrenzySwing=").Append(r.FrenzySwingPct)
                        .Append(" StationaryRegen=").Append(r.StationaryRegenPct)
                        .Append(" StationaryMana=").Append(r.StationaryAppliesMana)
                        .Append(" Dodge=").Append(r.DodgePct)
                        .AppendLine();
                }
            }
        }
    }

    private static void DumpClothingDisplacingTable(StringBuilder sb)
    {
        sb.AppendLine("== CLOTHING DISPLACING EFFECT TABLE ==");

        foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
        {
            foreach (var rarity in _rarities)
            {
                var r = AccessoryEffectTable.Get(root, rarity, isClothing: true, displacing: true);

                if (r.IsEmpty)
                {
                    continue;
                }

                sb.Append("clothdisp ")
                    .Append((int)root).Append(':').Append(rarity).Append('\t')
                    .Append("Stat=").Append(r.StatBonus)
                    .Append(" Lightning=").Append(r.LightningProcPct)
                    .Append(" ManaRegen=").Append(r.ManaRegenPct)
                    .Append(" SpellDmg=").Append(r.SpellDamagePct)
                    .Append(" ManaLeech=").Append(r.ManaLeechPct)
                    .Append(" HitHalved=").Append(r.HitHalvedPct)
                    .Append(" MissReroll=").Append(r.MissRerollPct)
                    .Append(" Hiding=").Append(r.HidingBonus)
                    .Append(" Stealth=").Append(r.StealthBonus)
                    .Append(" NightSight=").Append(r.NightSight)
                    .Append(" PoisonResist=").Append(r.PoisonResistPct)
                    .Append(" AllRegen=").Append(r.AllRegenPct)
                    .Append(" Potion=").Append(r.PotionEffectPct)
                    .Append(" OnKillStam=").Append(r.OnKillStamina)
                    .Append(" OnKillHp=").Append(r.OnKillHp)
                    .Append(" Karma=").Append(r.KarmaGainPct)
                    .Append(" Vendor=").Append(r.VendorPricePct)
                    .Append(" FrenzyChance=").Append(r.FrenzyChancePct)
                    .Append(" FrenzyDmg=").Append(r.FrenzyDamagePct)
                    .Append(" FrenzySwing=").Append(r.FrenzySwingPct)
                    .Append(" StationaryRegen=").Append(r.StationaryRegenPct)
                    .Append(" StationaryMana=").Append(r.StationaryAppliesMana)
                    .Append(" Dodge=").Append(r.DodgePct)
                    .AppendLine();
            }
        }
    }

    private static void DumpSlotSignatures(StringBuilder sb)
    {
        sb.AppendLine("== ARMOR SLOT SIGNATURE TABLE ==");

        for (var material = 0; material < ArmorSlotSignatureTable.MaterialCount; material++)
        {
            for (var slot = 0; slot < 7; slot++)
            {
                var cell = ArmorSlotSignatureTable.Get((ArmorMaterialType)material, (ArmorBodyType)slot);

                if (cell.Signature == ClauseType.None)
                {
                    continue;
                }

                sb.Append((ArmorMaterialType)material).Append('/').Append((ArmorBodyType)slot).Append('\t')
                    .Append(cell.Signature).Append(" S1=").Append(cell.S1)
                    .Append(" S2=").Append(cell.S2).Append(" S3=").Append(cell.S3).AppendLine();
            }
        }
    }

    private static void DumpLegendaries(StringBuilder sb)
    {
        sb.AppendLine("== LEGENDARY REGISTRY ==");

        var entries = new List<LegendaryEntry>(LegendaryRegistry.Entries);
        entries.Sort((a, b) => a.Id.CompareTo(b.Id));

        foreach (var e in entries)
        {
            sb.Append(e.Id).Append('\t')
                .Append(e.Name).Append('\t')
                .Append("root=").Append((int)e.Root).Append('(').Append(e.Root).Append(')')
                .Append(" family=").Append(e.Family)
                .Append(" base=").Append(e.BaseIndex)
                .Append(" clause=").Append(e.Clause)
                .Append(" P1=").Append(e.P1).Append(" P2=").Append(e.P2).Append(" P3=").Append(e.P3)
                .Append(" hue=").Append(e.Hue)
                .AppendLine();
        }

        sb.Append("EntryCount=").Append(entries.Count).AppendLine();
    }

    private static void DumpWeaponFamilyMap(StringBuilder sb)
    {
        sb.AppendLine("== WEAPON FAMILY MAP (root->family) ==");

        foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
        {
            if (WeaponFamilyMap.TryGetRootFamily(root, out var family))
            {
                sb.Append((int)root).Append('(').Append(root).Append(")\tfamily=").Append(family).AppendLine();
            }
        }

        sb.AppendLine("== WEAPON FAMILY MAP (type->family) ==");

        var field = typeof(WeaponFamilyMap).GetField("_typeToFamily", BindingFlags.NonPublic | BindingFlags.Static);
        var map = (Dictionary<Type, byte>)field!.GetValue(null)!;
        var pairs = new List<KeyValuePair<Type, byte>>(map);
        pairs.Sort((a, b) => string.CompareOrdinal(a.Key.FullName, b.Key.FullName));

        foreach (var pair in pairs)
        {
            sb.Append(pair.Key.Name).Append("\tfamily=").Append(pair.Value).AppendLine();
        }
    }

    private static void DumpCapstones(StringBuilder sb)
    {
        sb.AppendLine("== CAPSTONE METADATA ==");

        var threshold = typeof(WornEffectState).GetMethod(
            "CapstoneThreshold", BindingFlags.NonPublic | BindingFlags.Static
        );

        foreach (var material in Enum.GetValues<ArmorMaterialType>())
        {
            var t = (int)threshold!.Invoke(null, new object[] { material })!;
            var name = WornEffectState.CapstoneName(material);
            var icon = WornEffectState.CapstoneIcon(material);

            sb.Append(material).Append('\t')
                .Append("threshold=").Append(t.ToString(CultureInfo.InvariantCulture))
                .Append(" name=\"").Append(name).Append('"')
                .Append(" icon=").Append(icon).AppendLine();
        }
    }
}
