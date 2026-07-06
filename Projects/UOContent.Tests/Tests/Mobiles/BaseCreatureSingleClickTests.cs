using System.Text;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Tests.Maps;
using Server.Tests.Network;
using Xunit;

namespace UOContent.Tests;

public class BaseCreatureSingleClickTests
{
    static BaseCreatureSingleClickTests()
    {
        Core.ApplicationAssembly = typeof(BaseCreatureSingleClickTests).Assembly;
        ServerConfiguration.Load(true);
        Core.LoopContext = new EventLoopContext();
        NetState.Configure();

        if (Map.Internal == null)
        {
            TestMapDefinitions.ConfigureTestMapDefinitions();
        }

        World.Configure();
        Timer.Init(0);
    }

    [Fact]
    public void OnSingleClick_EmitsLevelOverheadLine()
    {
        var creature = new TestCreature(World.NewMobile);
        creature.DefaultMobileInit();
        creature.HitsMaxSeed = 150; // -> MobLevelFromHits(150) == 3 (v2 bracket table)

        var player = new PlayerMobile(World.NewMobile);
        player.DefaultMobileInit();

        using var ns = PacketTestUtilities.CreateTestNetState();
        player.NetState = ns;
        ns.Mobile = player;

        creature.OnSingleClick(player);

        // PrivateOverheadMessage(..., ascii: true, ...) writes Latin1 text (packet 0x1C).
        var text = Encoding.Latin1.GetString(ns.SendBuffer.GetReadSpan());
        Assert.Contains("[lvl 3]", text);
    }

    private sealed class TestCreature : BaseCreature
    {
        // Serial ctor + DefaultMobileInit bypasses AI/NPCSpeeds setup (absent in tests).
        public TestCreature(Serial serial) : base(serial)
        {
        }
    }
}
