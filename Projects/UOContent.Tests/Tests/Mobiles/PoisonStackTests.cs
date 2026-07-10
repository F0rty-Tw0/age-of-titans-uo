using Server;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// Stackable-poison engine model (plan PP1-PP3): append/cap/cure-all/area-refresh/expiry-shrink.
// Tick DAMAGE math (the merged PoisonTimer) is not driven here — timers never fire without the
// game loop — so the merged-tick sum stays on the in-game checklist.
[Collection("Sequential UOContent Tests")]
public class PoisonStackTests
{
    static PoisonStackTests()
    {
        // The curated test fixture doesn't invoke every static Configure; poisons register here.
        if (Poison.Poisons.Count == 0)
        {
            PoisonKinds.Configure();
        }
    }

    private static PlayerMobile CreatePlayerMobile(Point3D location)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }

    [Fact]
    public void ApplyPoison_AppendsStacks_MirrorTracksStrongest()
    {
        var player = CreatePlayerMobile(new Point3D(4760, 600, 0));

        try
        {
            Assert.Equal(ApplyPoisonResult.Poisoned, player.ApplyPoison(null, Poison.Lesser));
            Assert.Equal(ApplyPoisonResult.Poisoned, player.ApplyPoison(null, Poison.Greater));

            Assert.Equal(2, player.PoisonStacks.Count);
            Assert.True(player.Poisoned);
            Assert.Equal(Poison.Greater, player.Poison); // mirror = strongest active stack
            Assert.NotNull(player.PoisonTimer);
        }
        finally
        {
            player.Poison = null;
            player.Delete();
        }
    }

    [Fact]
    public void ApplyPoison_SixthApplication_IgnoredAtCap()
    {
        var player = CreatePlayerMobile(new Point3D(4762, 600, 0));

        try
        {
            for (var i = 0; i < Mobile.MaxPoisonStacks; i++)
            {
                Assert.Equal(ApplyPoisonResult.Poisoned, player.ApplyPoison(null, Poison.Lesser));
            }

            Assert.Equal(ApplyPoisonResult.HigherPoisonActive, player.ApplyPoison(null, Poison.Lethal));
            Assert.Equal(Mobile.MaxPoisonStacks, player.PoisonStacks.Count);
        }
        finally
        {
            player.Poison = null;
            player.Delete();
        }
    }

    [Fact]
    public void CurePoison_ClearsEveryStack()
    {
        var player = CreatePlayerMobile(new Point3D(4764, 600, 0));

        try
        {
            player.ApplyPoison(null, Poison.Lesser);
            player.ApplyPoison(null, Poison.Regular);
            player.ApplyPoison(null, Poison.Greater);

            Assert.True(player.CurePoison(player));

            Assert.Empty(player.PoisonStacks);
            Assert.False(player.Poisoned);
            Assert.Null(player.Poison);
            Assert.Null(player.PoisonTimer);
        }
        finally
        {
            player.Delete();
        }
    }

    [Fact]
    public void AreaRefresh_ReArmsExistingStack_InsteadOfStacking()
    {
        var player = CreatePlayerMobile(new Point3D(4766, 600, 0));

        try
        {
            Assert.Equal(ApplyPoisonResult.Poisoned, player.ApplyPoison(null, Poison.Lesser, refreshOnly: true));
            Assert.Single(player.PoisonStacks);

            player.PoisonStacks[0].TicksElapsed = 3; // pretend some ticks passed

            // Same poison from an area pulse: re-arm, not append.
            Assert.Equal(ApplyPoisonResult.Poisoned, player.ApplyPoison(null, Poison.Lesser, refreshOnly: true));
            Assert.Single(player.PoisonStacks);
            Assert.Equal(0, player.PoisonStacks[0].TicksElapsed);

            // Different poison with no matching stack still appends.
            Assert.Equal(ApplyPoisonResult.Poisoned, player.ApplyPoison(null, Poison.Greater, refreshOnly: true));
            Assert.Equal(2, player.PoisonStacks.Count);
        }
        finally
        {
            player.Poison = null;
            player.Delete();
        }
    }

    [Fact]
    public void StackExpiry_ShrinksSet_AndMirrorRecomputes()
    {
        var player = CreatePlayerMobile(new Point3D(4768, 600, 0));

        try
        {
            player.ApplyPoison(null, Poison.Greater);
            player.ApplyPoison(null, Poison.Lesser);
            Assert.Equal(Poison.Greater, player.Poison);

            player.OnPoisonStackExpired(player.PoisonStacks[0]); // the Greater stack runs out

            Assert.Single(player.PoisonStacks);
            Assert.Equal(Poison.Lesser, player.Poison); // mirror drops to the surviving stack
        }
        finally
        {
            player.Poison = null;
            player.Delete();
        }
    }

    [Fact]
    public void DirectPoisonAssignment_KeepsLegacyReplaceAllSemantics()
    {
        var player = CreatePlayerMobile(new Point3D(4770, 600, 0));

        try
        {
            player.ApplyPoison(null, Poison.Lesser);
            player.ApplyPoison(null, Poison.Lesser);
            player.ApplyPoison(null, Poison.Lesser);

            player.Poison = Poison.Regular; // legacy set: replaces the whole stack set

            Assert.Single(player.PoisonStacks);
            Assert.Equal(Poison.Regular, player.Poison);

            player.Poison = null; // legacy clear

            Assert.Empty(player.PoisonStacks);
            Assert.False(player.Poisoned);
        }
        finally
        {
            player.Delete();
        }
    }
}
