using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Tests;
using Xunit;

namespace UOContent.Tests;

// In the sequential collection: this class creates mobiles, so it must share the fixture's
// world boot. Its old hand-rolled cctor boot raced the shared UOContentFixture when run in
// parallel (TypeInitializationException out of World.Configure) and clobbered shared config
// (ServerConfiguration.Load) when serialized — TestServerInitializer is the process-wide,
// once-guarded boot both paths need.
[Collection("Sequential UOContent Tests")]
public class HostileMobileNotorietyTests
{
    static HostileMobileNotorietyTests()
    {
        TestServerInitializer.Initialize();
        NotorietyHandlers.Initialize();
    }

    [Fact]
    public void AttackOnSight_FightModeClosest_IsComputedAsMurderer()
    {
        var viewer = CreatePlayer();
        var target = CreateCreature(FightMode.Closest);

        var result = Notoriety.Handler!(viewer, target);

        Assert.Equal(Notoriety.Murderer, result);
    }

    [Fact]
    public void RetaliationOnly_FightModeAggressor_IsNotComputedAsMurderer()
    {
        var viewer = CreatePlayer();
        var target = CreateCreature(FightMode.Aggressor);

        var result = Notoriety.Handler!(viewer, target);

        Assert.NotEqual(Notoriety.Murderer, result);
    }

    private static NpcStub CreateCreature(FightMode mode)
    {
        var c = new NpcStub(World.NewMobile);
        c.DefaultMobileInit();
        c.Body = 0xC8; // Dog body: non-human, non-ghost.
        c.FightMode = mode;
        return c;
    }

    private static PlayerMobile CreatePlayer()
    {
        var p = new PlayerMobile(World.NewMobile);
        p.DefaultMobileInit();
        return p;
    }

    private sealed class NpcStub : BaseCreature
    {
        public NpcStub(Serial serial) : base(serial)
        {
        }
    }
}
