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
    public void ReflectDamagePairsWithFirstStatusRestFloatSeparately()
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

        // The reflect number pairs with its label ("-1 Reflect"); the stun floats on its own line.
        // Both lines are over the ATTACKER, seen by both parties. Effects are never merged.
        var reflectLine = new UnicodeMessage(
            attacker.Serial,
            attacker.Body,
            MessageType.Regular,
            FloatingCombatText.DebuffHue,
            3,
            "ENU",
            attacker.Name,
            "-1 Reflect"
        ).Compile();

        var stunLine = new UnicodeMessage(
            attacker.Serial,
            attacker.Body,
            MessageType.Regular,
            FloatingCombatText.DebuffHue,
            3,
            "ENU",
            attacker.Name,
            "Stunned"
        ).Compile();

        var sentToAttacker = attackerNs.SendBuffer.GetReadSpan();
        AssertThat.Equal(sentToAttacker[..reflectLine.Length], reflectLine);
        AssertThat.Equal(sentToAttacker[reflectLine.Length..], stunLine);

        var sentToDefender = defenderNs.SendBuffer.GetReadSpan();
        AssertThat.Equal(sentToDefender[..reflectLine.Length], reflectLine);
        AssertThat.Equal(sentToDefender[reflectLine.Length..], stunLine);
    }

    [Fact]
    public void TwoNumberedRetaliationsEachPairWithOwnLabelNoSum()
    {
        using var attackerNs = PacketTestUtilities.CreateTestNetState();
        using var defenderNs = PacketTestUtilities.CreateTestNetState();
        var attacker = CreateMobile(attackerNs, 0x1024);
        var defender = CreateMobile(defenderNs, 0x1025);

        FloatingCombatText.ClearContext();
        FloatingCombatText.BeginHit(defender, attacker);
        // Attacker takes two separate retaliation procs: 5 reflected, then an 88-damage stun.
        FloatingCombatText.ShowDamage(attacker, defender, 5);
        FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Reflect");
        FloatingCombatText.ShowDamage(attacker, defender, 88);
        FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Stunned");
        FloatingCombatText.EndHit();

        // Each proc keeps its own number+label — never summed into "-93".
        var reflectLine = new UnicodeMessage(
            attacker.Serial,
            attacker.Body,
            MessageType.Regular,
            FloatingCombatText.DebuffHue,
            3,
            "ENU",
            attacker.Name,
            "-5 Reflect"
        ).Compile();

        var stunLine = new UnicodeMessage(
            attacker.Serial,
            attacker.Body,
            MessageType.Regular,
            FloatingCombatText.DebuffHue,
            3,
            "ENU",
            attacker.Name,
            "-88 Stunned"
        ).Compile();

        var sent = attackerNs.SendBuffer.GetReadSpan();
        AssertThat.Equal(sent[..reflectLine.Length], reflectLine);
        AssertThat.Equal(sent[reflectLine.Length..], stunLine);
    }

    [Fact]
    public void DefenderHitPairsFirstStatusRestFloatSeparately()
    {
        using var attackerNs = PacketTestUtilities.CreateTestNetState();
        using var defenderNs = PacketTestUtilities.CreateTestNetState();
        var attacker = CreateMobile(attackerNs, 0x1024);
        var defender = CreateMobile(defenderNs, 0x1025);

        FloatingCombatText.ClearContext();
        FloatingCombatText.BeginHit(defender, attacker);
        // Main hit lands for 88 and applies a stun plus a poison to the defender.
        FloatingCombatText.ShowDamage(defender, attacker, 88);
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Stunned");
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Poisoned");
        FloatingCombatText.EndHit();

        // "-88 Stunned" is the hit line (number + first status); "Poisoned" floats on its own line.
        var hitLine = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            0x490, // defender sees the incoming-damage hue for their own line
            3,
            "ENU",
            defender.Name,
            "-88 Stunned"
        ).Compile();

        var poisonLine = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            FloatingCombatText.DebuffHue,
            3,
            "ENU",
            defender.Name,
            "Poisoned"
        ).Compile();

        var sent = defenderNs.SendBuffer.GetReadSpan();
        AssertThat.Equal(sent[..hitLine.Length], hitLine);
        AssertThat.Equal(sent[hitLine.Length..], poisonLine);
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
    public void MissedExtraSwingFloatsSeparately()
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

        // "-15 Extra Swing" is the hit line; the whiffed bonus swing's "Miss" floats on its own
        // line (a damage number next to "Miss" would read as a contradiction).
        var hitLine = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            0x490, // defender sees the incoming-damage hue for their own line
            3,
            "ENU",
            defender.Name,
            "-15 Extra Swing"
        ).Compile();

        var missLine = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            FloatingCombatText.MissHue,
            3,
            "ENU",
            defender.Name,
            "Miss"
        ).Compile();

        var sent = defenderNs.SendBuffer.GetReadSpan();
        AssertThat.Equal(sent[..hitLine.Length], hitLine);
        AssertThat.Equal(sent[hitLine.Length..], missLine);
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

    [Fact]
    public void NestedExtraSwingOwnsItsDamageAndStun()
    {
        using var attackerNs = PacketTestUtilities.CreateTestNetState();
        using var defenderNs = PacketTestUtilities.CreateTestNetState();
        var attacker = CreateMobile(attackerNs, 0x1024);
        var defender = CreateMobile(defenderNs, 0x1025);

        FloatingCombatText.ClearContext();
        FloatingCombatText.BeginHit(defender, attacker);                    // main swing
        FloatingCombatText.ShowDamage(defender, attacker, 25);
        FloatingCombatText.BeginHit(defender, attacker);                    // re-entrant extra swing
        FloatingCombatText.ShowDamage(defender, attacker, 1);
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Stunned"); // proc of the extra swing
        FloatingCombatText.EndHit();                                        // -> "-1 Stunned"
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, FloatingCombatText.ExtraSwingLabel);
        FloatingCombatText.EndHit();                                        // -> "-25 Extra Swing"

        var nested = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            0x490, // defender sees the incoming-damage hue for their own line
            3,
            "ENU",
            defender.Name,
            "-1 Stunned"
        ).Compile();

        var main = new UnicodeMessage(
            defender.Serial,
            defender.Body,
            MessageType.Regular,
            0x490,
            3,
            "ENU",
            defender.Name,
            "-25 Extra Swing"
        ).Compile();

        var sent = defenderNs.SendBuffer.GetReadSpan();
        AssertThat.Equal(sent[..nested.Length], nested);
        AssertThat.Equal(sent[nested.Length..], main);
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
