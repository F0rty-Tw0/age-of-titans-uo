using Server;
using Server.Engines.Rarity;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// P28 consecutive-hit ramp tracker (CombatFxState). Uses the shared world boot for PlayerMobile
// construction; the tracker itself only keys off the mobile instances and Core.TickCount.
[Collection("Sequential UOContent Tests")]
public class RampTrackerTests
{
    private static PlayerMobile CreateMobile()
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        return m;
    }

    [Fact]
    public void Ramp_IncrementsOnSameTarget_ResetsOnSwap_CapsAtMax_Evicts()
    {
        var attacker = CreateMobile();
        var t1 = CreateMobile();
        var t2 = CreateMobile();

        try
        {
            Assert.Equal(1, CombatFxState.RegisterRampHit(attacker, t1, 3, out var reached1));
            Assert.False(reached1);

            Assert.Equal(2, CombatFxState.RegisterRampHit(attacker, t1, 3, out var reached2));
            Assert.False(reached2);

            // Third same-target hit climbs to the cap and reports the transition once.
            Assert.Equal(3, CombatFxState.RegisterRampHit(attacker, t1, 3, out var reached3));
            Assert.True(reached3);

            // Further same-target hits stay clamped and do NOT re-report reaching max.
            Assert.Equal(3, CombatFxState.RegisterRampHit(attacker, t1, 3, out var reached4));
            Assert.False(reached4);

            // Swapping targets resets the ramp to 1.
            Assert.Equal(1, CombatFxState.RegisterRampHit(attacker, t2, 3, out var reached5));
            Assert.False(reached5);
            Assert.Equal(1, CombatFxState.GetRampStacks(attacker));

            CombatFxState.Evict(attacker);
            Assert.Equal(0, CombatFxState.GetRampStacks(attacker));
        }
        finally
        {
            CombatFxState.Evict(attacker);
            CombatFxState.Evict(t1);
            CombatFxState.Evict(t2);
            attacker.Delete();
            t1.Delete();
            t2.Delete();
        }
    }

    [Fact]
    public void Ramp_NullOrZeroMax_IsSafe()
    {
        var attacker = CreateMobile();
        var target = CreateMobile();

        try
        {
            // max <= 0 clamps to a single stack that never reports reaching max.
            Assert.Equal(1, CombatFxState.RegisterRampHit(attacker, target, 0, out var reached));
            Assert.False(reached);
            Assert.Equal(1, CombatFxState.RegisterRampHit(attacker, target, 0, out reached));
            Assert.False(reached);

            Assert.Equal(0, CombatFxState.RegisterRampHit(null, target, 3, out _));
            Assert.Equal(0, CombatFxState.RegisterRampHit(attacker, null, 3, out _));
        }
        finally
        {
            CombatFxState.Evict(attacker);
            CombatFxState.Evict(target);
            attacker.Delete();
            target.Delete();
        }
    }
}
