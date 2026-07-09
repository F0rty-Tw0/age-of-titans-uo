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

    [Fact]
    public void RestoreFormatsAmountAndUnitOverSelf()
    {
        using var ns = PacketTestUtilities.CreateTestNetState();
        var self = CreateMobile(ns);

        FloatingCombatText.ShowRestore(self, 'S', 15);

        var expected = new UnicodeMessage(
            self.Serial,
            self.Body,
            MessageType.Regular,
            FloatingCombatText.StamHue,
            3,
            "ENU",
            self.Name,
            "+15 Stam"
        ).Compile();

        AssertThat.Equal(ns.SendBuffer.GetReadSpan(), expected);
    }

    [Fact]
    public void RestoreOfZeroShowsNothing()
    {
        using var ns = PacketTestUtilities.CreateTestNetState();
        var self = CreateMobile(ns);

        FloatingCombatText.ShowRestore(self, 'M', 0);

        Assert.Equal(0, ns.SendBuffer.GetReadSpan().Length);
    }

    [Fact]
    public void SelfStatusFloatsOverSelfOnly()
    {
        using var ns = PacketTestUtilities.CreateTestNetState();
        var self = CreateMobile(ns);

        FloatingCombatText.ShowSelfStatus(self, "Frenzy");

        var expected = new UnicodeMessage(
            self.Serial,
            self.Body,
            MessageType.Regular,
            FloatingCombatText.BuffHue,
            3,
            "ENU",
            self.Name,
            "Frenzy"
        ).Compile();

        AssertThat.Equal(ns.SendBuffer.GetReadSpan(), expected);
    }

    [Fact]
    public void OffensiveStatusFloatsOverTargetForBothParties()
    {
        using var attackerNs = PacketTestUtilities.CreateTestNetState();
        using var defenderNs = PacketTestUtilities.CreateTestNetState();
        var attacker = CreateMobile(attackerNs, 0x1024);
        var defender = CreateMobile(defenderNs, 0x1025);

        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Stunned", FloatingCombatText.DebuffHue);

        // Both the attacker and the defender see the label floating over the DEFENDER.
        var expected = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            FloatingCombatText.DebuffHue,
            3,
            "ENU",
            defender.Name,
            "Stunned"
        ).Compile();

        AssertThat.Equal(defenderNs.SendBuffer.GetReadSpan(), expected);
        AssertThat.Equal(attackerNs.SendBuffer.GetReadSpan(), expected);
    }

    [Fact]
    public void ReflectDamageAndStatusesShareOneAttackerLine()
    {
        using var attackerNs = PacketTestUtilities.CreateTestNetState();
        using var defenderNs = PacketTestUtilities.CreateTestNetState();
        var attacker = CreateMobile(attackerNs, 0x1024);
        var defender = CreateMobile(defenderNs, 0x1025);

        FloatingCombatText.ClearContext();
        FloatingCombatText.BeginHit(defender, attacker);
        // Hit fully absorbed (no defender damage), then reflected back onto the attacker with a stun.
        FloatingCombatText.ShowDamage(attacker, defender, 1);
        FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Reflect");
        FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Stunned");
        FloatingCombatText.EndHit();

        // One combined line over the ATTACKER, seen by both parties.
        var expected = new UnicodeMessage(
            attacker.Serial,
            attacker.Body,
            MessageType.Regular,
            FloatingCombatText.DebuffHue,
            3,
            "ENU",
            attacker.Name,
            "-1 Reflect Stunned"
        ).Compile();

        AssertThat.Equal(attackerNs.SendBuffer.GetReadSpan(), expected);
        AssertThat.Equal(defenderNs.SendBuffer.GetReadSpan(), expected);
    }

    [Fact]
    public void AttackerStatusWithoutReflectDamageEmitsLabelsOnly()
    {
        using var attackerNs = PacketTestUtilities.CreateTestNetState();
        using var defenderNs = PacketTestUtilities.CreateTestNetState();
        var attacker = CreateMobile(attackerNs, 0x1024);
        var defender = CreateMobile(defenderNs, 0x1025);

        FloatingCombatText.ClearContext();
        FloatingCombatText.BeginHit(defender, attacker);
        FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Stunned");
        FloatingCombatText.EndHit();

        var expected = new UnicodeMessage(
            attacker.Serial,
            attacker.Body,
            MessageType.Regular,
            FloatingCombatText.DebuffHue,
            3,
            "ENU",
            attacker.Name,
            "Stunned"
        ).Compile();

        AssertThat.Equal(attackerNs.SendBuffer.GetReadSpan(), expected);
        AssertThat.Equal(defenderNs.SendBuffer.GetReadSpan(), expected);
    }

    [Fact]
    public void MissedExtraSwingFoldsIntoOneTail()
    {
        using var attackerNs = PacketTestUtilities.CreateTestNetState();
        using var defenderNs = PacketTestUtilities.CreateTestNetState();
        var attacker = CreateMobile(attackerNs, 0x1024);
        var defender = CreateMobile(defenderNs, 0x1025);

        FloatingCombatText.ClearContext();
        FloatingCombatText.BeginHit(defender, attacker);
        // Main hit lands for 15, then the bonus swing whiffs: OnMiss adds "Miss",
        // DoExtraSwing adds "Extra Swing" (raw order is Miss then Extra Swing).
        FloatingCombatText.ShowDamage(defender, attacker, 15);
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, FloatingCombatText.MissLabel);
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, FloatingCombatText.ExtraSwingLabel);
        FloatingCombatText.EndHit();

        var expected = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            0x490, // defender sees the incoming-damage hue for their own line
            3,
            "ENU",
            defender.Name,
            "-15 Extra Swing Miss"
        ).Compile();

        AssertThat.Equal(defenderNs.SendBuffer.GetReadSpan(), expected);
    }

    [Fact]
    public void TwoExtraSwingsCollapseToMultiplier()
    {
        using var attackerNs = PacketTestUtilities.CreateTestNetState();
        using var defenderNs = PacketTestUtilities.CreateTestNetState();
        var attacker = CreateMobile(attackerNs, 0x1024);
        var defender = CreateMobile(defenderNs, 0x1025);

        FloatingCombatText.ClearContext();
        FloatingCombatText.BeginHit(defender, attacker);
        FloatingCombatText.ShowDamage(defender, attacker, 30);
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, FloatingCombatText.ExtraSwingLabel);
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, FloatingCombatText.ExtraSwingLabel);
        FloatingCombatText.EndHit();

        var expected = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            0x490, // defender sees the incoming-damage hue for their own line
            3,
            "ENU",
            defender.Name,
            "-30 Extra Swing x2"
        ).Compile();

        AssertThat.Equal(defenderNs.SendBuffer.GetReadSpan(), expected);
    }

    private static Mobile CreateMobile(NetState ns, uint serial = 0x1024)
    {
        var mobile = new Mobile((Serial)serial);
        mobile.DefaultMobileInit();
        mobile.NetState = ns;

        ns.Mobile = mobile;

        return mobile;
    }
}
