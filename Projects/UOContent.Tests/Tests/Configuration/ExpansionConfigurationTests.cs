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

            ServerConfiguration.Load(true);
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
            ServerConfiguration.Load(true);
            ExpansionConfiguration.Configure();
            ObjectPropertyList.Enabled = oplEnabled;
            IncomingEntityPackets.SingleClickProps = singleClickProps;
        }
    }
}
