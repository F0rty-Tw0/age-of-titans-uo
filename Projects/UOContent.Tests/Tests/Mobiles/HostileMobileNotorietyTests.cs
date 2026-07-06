using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Tests.Maps;
using Xunit;

namespace UOContent.Tests;

public class HostileMobileNotorietyTests
{
    static HostileMobileNotorietyTests()
    {
        Core.ApplicationAssembly = typeof(HostileMobileNotorietyTests).Assembly;
        Core.LoopContext = new EventLoopContext();
        Core.Expansion = Expansion.EJ;

        ServerConfiguration.Load(true);

        if (Map.Internal == null)
        {
            TestMapDefinitions.ConfigureTestMapDefinitions();
        }

        World.Configure();
        Timer.Init(0);
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
