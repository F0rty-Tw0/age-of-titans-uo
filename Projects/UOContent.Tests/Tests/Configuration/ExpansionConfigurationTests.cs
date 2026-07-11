using Server;
using Server.Network;
using Xunit;

namespace UOContent.Tests;

[Collection("Sequential UOContent Tests")]
public class ExpansionConfigurationTests
{
    [Fact]
    public void Configure_DoesNotEnableSingleClickPropsWhenOnlyOplIsEnabledBelowAos()
    {
        var expansion = Core.Expansion;
        var oplEnabled = ObjectPropertyList.Enabled;
        var singleClickProps = IncomingEntityPackets.SingleClickProps;

        try
        {
            Core.Expansion = Expansion.T2A;
            ObjectPropertyList.Enabled = false;
            IncomingEntityPackets.SingleClickProps = false;

            // NOT a bare Load(true): that drops the boot-added data directory and breaks any
            // pathfinding test that lazily loads a map sector afterwards.
            Server.Tests.TestServerInitializer.ReloadConfiguration();
            ServerConfiguration.SetSetting("opl.enable", true);

            ExpansionConfiguration.Configure();

            Assert.True(ObjectPropertyList.Enabled);
            Assert.False(IncomingEntityPackets.SingleClickProps);
        }
        finally
        {
            Core.Expansion = expansion;
            // Reset the mocked settings and re-derive the globals Configure() touched
            // (Mobile.*, T2ACraftSystem) before restoring the captured values.
            // NOT a bare Load(true): that drops the boot-added data directory and breaks any
            // pathfinding test that lazily loads a map sector afterwards.
            Server.Tests.TestServerInitializer.ReloadConfiguration();
            ExpansionConfiguration.Configure();
            ObjectPropertyList.Enabled = oplEnabled;
            IncomingEntityPackets.SingleClickProps = singleClickProps;
        }
    }
}
