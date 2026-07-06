using Server.Engines.Craft.T2A;
using Server.Network;

namespace Server
{
    public static class ExpansionConfiguration
    {
        public static void Configure()
        {
            Mobile.InsuranceEnabled = ServerConfiguration.GetSetting("insurance.enable", Core.AOS);
            ObjectPropertyList.Enabled = ServerConfiguration.GetSetting("opl.enable", Core.AOS);
            var visibleDamage = ServerConfiguration.GetSetting("visibleDamage", Core.AOS);
            Mobile.VisibleDamageType = visibleDamage ? VisibleDamageType.Related : VisibleDamageType.None;
            Mobile.GuildClickMessage = ServerConfiguration.GetSetting("guildClickMessage", !Core.AOS);
            Mobile.AsciiClickMessage = ServerConfiguration.GetSetting("asciiClickMessage", !Core.AOS);
            T2ACraftSystem.Enabled = ServerConfiguration.GetSetting("t2aCraftMenus", !Core.UOTD);

            Mobile.ActionDelay = ServerConfiguration.GetSetting("actionDelay", Core.AOS ? 1000 : 500);

            // T2A item-info labels use the old OnSingleClick path. Only route single-clicks
            // through OPL when explicitly enabled, or by default on true AOS cores.
            IncomingEntityPackets.SingleClickProps = ObjectPropertyList.Enabled &&
                                                    ServerConfiguration.GetSetting("opl.singleClickProps", Core.AOS);

            if (Core.AOS)
            {
                Mobile.AOSStatusHandler = AOS.GetStatus;
            }
        }
    }
}
