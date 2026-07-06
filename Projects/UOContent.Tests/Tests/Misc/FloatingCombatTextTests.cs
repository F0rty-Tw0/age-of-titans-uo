using Server.Misc;
using Server.Network;
using Server.Tests.Maps;
using Xunit;

namespace Server.Tests.Network;
[CollectionDefinition("FloatingCombatText Tests", DisableParallelization = true)]
public class FloatingCombatTextTestCollection;

[Collection("FloatingCombatText Tests")]
public class FloatingCombatTextTests
{
    static FloatingCombatTextTests()
    {
        Core.ApplicationAssembly = typeof(FloatingCombatTextTests).Assembly;
        ServerConfiguration.Load(true);
        Core.LoopContext = new EventLoopContext();
        NetState.Configure();
        TestMapDefinitions.ConfigureTestMapDefinitions();
        World.Configure();
        Timer.Init(0);
    }

    [Fact]
    public void PlainIncomingDamageUsesIncomingDamageHue()
    {
        using var ns = PacketTestUtilities.CreateTestNetState();
        var target = CreateMobile(ns);

        FloatingCombatText.ClearContext();
        FloatingCombatText.ShowDamage(target, null, 7);

        var expected = new UnicodeMessage(
            target.Serial,
            target.Body,
            MessageType.Regular,
            0x490,
            3,
            "ENU",
            target.Name,
            "-7"
        ).Compile();

        AssertThat.Equal(ns.SendBuffer.GetReadSpan(), expected);
    }

    [Fact]
    public void PoisonIncomingDamageUsesPoisonHue()
    {
        using var ns = PacketTestUtilities.CreateTestNetState();
        var target = CreateMobile(ns);

        try
        {
            FloatingCombatText.SetPoisonContext();
            FloatingCombatText.ShowDamage(target, null, 7);
        }
        finally
        {
            FloatingCombatText.ClearContext();
        }

        var expected = new UnicodeMessage(
            target.Serial,
            target.Body,
            MessageType.Regular,
            0x3F,
            3,
            "ENU",
            target.Name,
            "-7 (Poison)"
        ).Compile();

        AssertThat.Equal(ns.SendBuffer.GetReadSpan(), expected);
    }

    [Fact]
    public void SpellIncomingDamageUsesSpellHue()
    {
        using var ns = PacketTestUtilities.CreateTestNetState();
        var target = CreateMobile(ns);

        try
        {
            FloatingCombatText.SetSpellContext("Explosion");
            FloatingCombatText.ShowDamage(target, null, 7);
        }
        finally
        {
            FloatingCombatText.ClearContext();
        }

        var expected = new UnicodeMessage(
            target.Serial,
            target.Body,
            MessageType.Regular,
            0x2B,
            3,
            "ENU",
            target.Name,
            "-7 (Explosion)"
        ).Compile();

        AssertThat.Equal(ns.SendBuffer.GetReadSpan(), expected);
    }

    private static Mobile CreateMobile(NetState ns)
    {
        var mobile = new Mobile((Serial)0x1024);
        mobile.DefaultMobileInit();
        mobile.NetState = ns;

        ns.Mobile = mobile;

        return mobile;
    }
}
