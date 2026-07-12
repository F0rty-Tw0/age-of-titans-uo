using System;
using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

public class ClauseTextTests
{
    [Fact]
    public void EveryClauseType_ExceptNone_ProducesNonEmptyText()
    {
        foreach (var clause in Enum.GetValues<ClauseType>())
        {
            if (clause == ClauseType.None)
            {
                continue;
            }

            var text = ClauseText.Describe(clause, 5, 10, 3);

            Assert.False(string.IsNullOrEmpty(text), $"{clause} produced no clause text");
        }
    }

    [Fact]
    public void None_ProducesNoText()
    {
        Assert.Null(ClauseText.Describe(ClauseType.None, 0, 0, 0));
    }

    [Fact]
    public void NoLegendary_RendersAZerothCadence()
    {
        // A cadence clause left at N=0 used to print the nonsensical "every 0th ...". Every legendary
        // must degrade to a sensible phrase (e.g. Kalchas → "critical hits strike an additional time ...").
        foreach (var entry in LegendaryRegistry.Entries)
        {
            if (entry.Clause == ClauseType.None)
            {
                continue;
            }

            var text = ClauseText.Describe(entry.Clause, entry.P1, entry.P2, entry.P3);

            // " 0th" (space before) is a bare zero cadence; "10th"/"20th" have a digit before the 0.
            Assert.DoesNotContain(" 0th", text);
        }
    }

    [Fact]
    public void ExtraSwingClause_WithNoCadence_ReadsAsNaturalCrit()
    {
        var text = ClauseText.Describe(ClauseType.ExtraSwingManaLeech, 0, 8, 0);

        Assert.StartsWith("critical hits strike an additional time", text);
    }

    [Fact]
    public void CritExecuteUnder15_RendersTheEnginesEffectiveThreshold()
    {
        // The engine (RarityEffects.WeaponHit.cs) reads `p2 > 0 ? p2 : 15` as the low-HP execute
        // threshold. Memnon (id 74) sets P2=25, so the tooltip must read "25%", not a hardcoded
        // "15%" that drifts from the real in-game trigger. (The 25% example moved here from Tydeus
        // in the 2026-07-12 arming-group split — Tydeus's Ephodos crit signature forced its unique
        // off the crit lane.)
        Assert.True(LegendaryRegistry.TryGet(74, out var memnon));
        Assert.Equal("Memnon", memnon.Name);
        Assert.Equal(ClauseType.CritExecuteUnder15, memnon.Clause);
        Assert.Equal(25, memnon.P2);

        var text = ClauseText.Describe(memnon.Clause, memnon.P1, memnon.P2, memnon.P3);

        Assert.Contains("under 25% health", text);
        Assert.DoesNotContain("under 15% health", text);
    }

    [Fact]
    public void CritExecuteUnder15_WithNoLegendaryOverride_DefaultsTo15Percent()
    {
        var text = ClauseText.Describe(ClauseType.CritExecuteUnder15, 0, 0, 0);

        Assert.Contains("under 15% health", text);
    }

    [Fact]
    public void EveryLegendary_WithARealClause_ProducesNonEmptyText()
    {
        foreach (var entry in LegendaryRegistry.Entries)
        {
            if (entry.Clause == ClauseType.None)
            {
                continue;
            }

            var text = ClauseText.Describe(entry.Clause, entry.P1, entry.P2, entry.P3);

            Assert.False(string.IsNullOrEmpty(text), $"{entry.Name} ({entry.Clause}) produced no clause text");
        }
    }
}
