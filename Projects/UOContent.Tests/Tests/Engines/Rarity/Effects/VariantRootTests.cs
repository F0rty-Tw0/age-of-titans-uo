using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

public class VariantRootTests
{
    [Theory]
    [InlineData(VariantRoot.Zephyr, "zephyr")]
    [InlineData(VariantRoot.Phobos, "phobos")]
    [InlineData(VariantRoot.Stygian, "stygian")]
    [InlineData(VariantRoot.Aegis, "aegis")]
    [InlineData(VariantRoot.Arachne, "arachne")]
    public void GetDisplayName_IsLowercaseRoot(VariantRoot root, string expected)
    {
        Assert.Equal(expected, VariantRootInfo.GetDisplayName(root));
    }

    [Fact]
    public void GetBodyHue_CommonAndNoneAreUnhued()
    {
        Assert.Equal(0, VariantRootInfo.GetBodyHue(VariantRoot.Phobos, ItemRarity.Common));
        Assert.Equal(0, VariantRootInfo.GetBodyHue(VariantRoot.None, ItemRarity.Legendary));
    }

    [Fact]
    public void GetBodyHue_RampsOneShadePerRarity()
    {
        // Phobos base = 34 (Uncommon shade); each rarity adds one shade.
        Assert.Equal(34, VariantRootInfo.GetBodyHue(VariantRoot.Phobos, ItemRarity.Uncommon));
        Assert.Equal(35, VariantRootInfo.GetBodyHue(VariantRoot.Phobos, ItemRarity.Rare));
        Assert.Equal(36, VariantRootInfo.GetBodyHue(VariantRoot.Phobos, ItemRarity.Epic));
        Assert.Equal(37, VariantRootInfo.GetBodyHue(VariantRoot.Phobos, ItemRarity.Legendary));
    }

    [Fact]
    public void EveryRoot_HasDisplayNameAndHue()
    {
        for (var root = VariantRoot.Zephyr; root <= VariantRoot.Pnoe; root++)
        {
            Assert.False(string.IsNullOrEmpty(VariantRootInfo.GetDisplayName(root)));
            Assert.True(VariantRootInfo.GetBodyHue(root, ItemRarity.Legendary) > 0);
        }
    }

    [Fact]
    public void RootCount_MatchesLastMember()
    {
        Assert.Equal(VariantRootInfo.RootCount - 1, (int)VariantRoot.Pnoe);

        // The three backing arrays are private; VariantRootInfo's static ctor already throws
        // on any length mismatch against RootCount, so touching the type here (any call)
        // exercises that fail-fast check.
        Assert.False(string.IsNullOrEmpty(VariantRootInfo.GetDisplayName(VariantRoot.Pnoe)));
    }

    [Fact]
    public void GetStackGroup_GroupsSameFamilyRoots()
    {
        Assert.Equal(VariantRootInfo.GetStackGroup(VariantRoot.Polias), VariantRootInfo.GetStackGroup(VariantRoot.Aegis));
        Assert.Equal(VariantRootInfo.GetStackGroup(VariantRoot.Talarian), VariantRootInfo.GetStackGroup(VariantRoot.Ophis));
    }

    [Fact]
    public void GetStackGroup_DifferentFamiliesGetDifferentGroups()
    {
        Assert.NotEqual(VariantRootInfo.GetStackGroup(VariantRoot.Polias), VariantRootInfo.GetStackGroup(VariantRoot.Cyclopean));
    }
}
