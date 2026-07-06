using Server.Misc;
using Xunit;

namespace UOContent.Tests;

public class SkillGainCurveTests
{
    [Theory]
    [InlineData(0.0, 0.90)]
    [InlineData(30.0, 0.90)]
    [InlineData(59.9, 0.90)]
    [InlineData(60.0, 0.75)] // step
    [InlineData(80.0, 0.75)]
    [InlineData(94.9, 0.75)]
    [InlineData(95.0, 0.50)] // step
    [InlineData(100.0, 0.50)]
    [InlineData(120.0, 0.50)]
    public void GainChance_MatchesBands(double baseSkill, double expected)
    {
        Assert.Equal(expected, SkillCheck.GainChance(baseSkill));
    }
}
