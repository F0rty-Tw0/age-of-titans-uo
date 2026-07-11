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
    public void ResetFight_ReArmsAttackerFirstHit()
    {
        var attacker = CreateMobile();

        try
        {
            // A fresh mobile's first registered hit is the first-of-fight; the next is not.
            CombatFxState.RegisterHit(attacker, out var first1);
            Assert.True(first1);
            CombatFxState.RegisterHit(attacker, out var first2);
            Assert.False(first2);

            // A kill ends the fight — the next foe engaged counts as a first-hit again. (This is the
            // bug fix: without ResetFight, killing one enemy and swinging at another within the 30s
            // window carried the dead fight's counter over, so first-hit never re-armed.)
            CombatFxState.ResetFight(attacker);

            CombatFxState.RegisterHit(attacker, out var first3);
            Assert.True(first3);
        }
        finally
        {
            CombatFxState.Evict(attacker);
            attacker.Delete();
        }
    }

    [Fact]
    public void RollbackHit_UndoesANoDamageSwing_ForFirstHitAndCadence()
    {
        var attacker = CreateMobile();

        try
        {
            // A parried first hit rolls back → the next connecting hit is first-of-fight again.
            var h1 = CombatFxState.RegisterHit(attacker, out var first1);
            Assert.Equal(1, h1);
            Assert.True(first1);

            CombatFxState.RollbackHit(attacker, h1);

            var h2 = CombatFxState.RegisterHit(attacker, out var first2);
            Assert.Equal(1, h2);
            Assert.True(first2);

            // A parried later hit rolls back the cadence counter → the Nth-hit position is not spent.
            var h3 = CombatFxState.RegisterHit(attacker, out _); // hitCount 2
            Assert.Equal(2, h3);
            CombatFxState.RollbackHit(attacker, h3);
            var h4 = CombatFxState.RegisterHit(attacker, out _);
            Assert.Equal(2, h4); // re-uses slot 2 rather than skipping to 3

            // Safety: rollback no-ops if the counter already advanced past the given swing (a nested
            // extra swing landed), so a hit that DID connect is never rewound.
            CombatFxState.RegisterHit(attacker, out _); // hitCount 3
            CombatFxState.RollbackHit(attacker, 2);     // stale — must not touch the counter
            var h5 = CombatFxState.RegisterHit(attacker, out _);
            Assert.Equal(4, h5);
        }
        finally
        {
            CombatFxState.Evict(attacker);
            attacker.Delete();
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
