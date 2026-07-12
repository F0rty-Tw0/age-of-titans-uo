using System.Collections.Generic;
using Server.Engines.Rarity;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

// Single-click overhead text for T2A/pre-UOTD clients is hard-capped at 5 lines (name + 4). These
// pin the approved layout (2026-07-11) and, most importantly, that no variant weapon path emits a
// 6th line. Item construction needs the shared world fixture.
[Collection("Sequential UOContent Tests")]
public class RarityTooltipTests
{
    [Fact]
    public void SingleClickLines_WorstCaseLegendaryWeapon_FitsFiveLineCap()
    {
        // Hektor (id 50) is a Phoibos cleaver: its Legendary row carries a lane signature
        // (CritFirstHit) AND its own unique clause (CritSplash), so it exercises every rarity line —
        // stats, myth-tagged effects (with the base shape merged in), signature clause, unique
        // clause. That is the worst case: 4 rarity lines + the item's name = exactly the 5-line cap.
        var item = new Cleaver();

        try
        {
            RarityEffects.ApplyLegendary(item, 50);

            var lines = new List<string>();
            RarityEffects.CollectSingleClickLines(item, lines);

            Assert.Equal(4, lines.Count); // + the name the caller labels first == 5, the classic cap

            // Legendary items are indestructible (Part B), so no durability line — which is exactly
            // what keeps the worst case (stats + effects + signature + legendary) at the 4-line budget.
            Assert.DoesNotContain(lines, l => l.StartsWith("Durability:"));
            Assert.StartsWith("Damage:", lines[0]);                 // stats line (legendary => no durability line)
            // Effects line: bare summary, no myth tag (pantheon name removed from click, 2026-07-12)
            // and no base shape (2026-07-11).
            Assert.DoesNotContain(" — ", lines[1]);
            Assert.DoesNotContain("cleaver", lines[1]);
            Assert.False(string.IsNullOrWhiteSpace(lines[1]));       // effects summary
            Assert.False(string.IsNullOrWhiteSpace(lines[2]));       // lane signature clause
            Assert.False(string.IsNullOrWhiteSpace(lines[3]));       // legendary unique clause
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void SingleClickLines_NonLegendaryVariantWeapon_HasNoShapeAndFitsCap()
    {
        // A plain rare variant: name + stats + durability + bare effects (no myth tag, no shape).
        var item = new Katana();

        try
        {
            RarityEffects.ApplyVariant(item, VariantRoot.Phobos, ItemRarity.Rare);

            var lines = new List<string>();
            RarityEffects.CollectSingleClickLines(item, lines);

            Assert.True(lines.Count <= 4, $"emitted {lines.Count} rarity lines, over the 4-line budget");
            Assert.StartsWith("Damage:", lines[0]);
            // Sub-legendary weapon shows a durability line; the pantheon name is gone from the click
            // tooltip (2026-07-12) so no line carries the "Ares — " myth tag.
            Assert.Contains(lines, l => l.StartsWith("Durability:"));
            Assert.DoesNotContain(lines, l => l.Contains(" — "));
            Assert.DoesNotContain(lines, l => l.StartsWith("Ares"));
        }
        finally
        {
            item.Delete();
        }
    }
}
