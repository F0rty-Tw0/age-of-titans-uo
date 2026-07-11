using Server.Gumps;
using Server.Network;

namespace Server.Engines.Rarity;

// Hub menu for the Pantheon altar: dispatches to the legendary offering, salvage, and upgrade
// flows. Layout is static (cached per type); only the carried-ichor footer is per-instance.
public class PantheonAltarGump : StaticGump<PantheonAltarGump>
{
    private const int ButtonOffering = 1;
    private const int ButtonSalvage = 2;
    private const int ButtonUpgrade = 3;

    public override bool Singleton => true;

    private readonly Mobile _from;
    private readonly PantheonAltar _altar;

    // Private — DisplayTo is the only entry point (empty-gump rule: validate before construct).
    private PantheonAltarGump(Mobile from, PantheonAltar altar) : base(100, 100)
    {
        _from = from;
        _altar = altar;
    }

    public static void DisplayTo(Mobile from, PantheonAltar altar)
    {
        if (from?.NetState == null || !from.Alive || altar?.Deleted != false ||
            !from.InRange(altar.GetWorldLocation(), 3))
        {
            return;
        }

        from.SendGump(new PantheonAltarGump(from, altar));
    }

    protected override void BuildLayout(ref StaticGumpBuilder builder)
    {
        builder.SetNoResize();

        builder.AddPage();
        builder.AddBackground(0, 0, 420, 280, 5054);
        builder.AddAlphaRegion(10, 10, 400, 260);

        builder.AddHtml(20, 18, 380, 30, "<CENTER><BASEFONT size=6 color=#FFD700>ALTAR OF THE TWELVE</BASEFONT></CENTER>");

        builder.AddButton(25, 62, 4005, 4007, ButtonOffering);
        builder.AddHtml(65, 60, 335, 22, "<BASEFONT color=#FFFFFF>Legendary Offering</BASEFONT>");
        builder.AddHtml(65, 80, 335, 22, "<BASEFONT size=4 color=#CCCCCC>Trade two relics of one god for another of that domain.</BASEFONT>");

        builder.AddButton(25, 117, 4005, 4007, ButtonSalvage);
        builder.AddHtml(65, 115, 335, 22, "<BASEFONT color=#FFFFFF>Salvage</BASEFONT>");
        builder.AddHtml(65, 135, 335, 22, "<BASEFONT size=4 color=#CCCCCC>Destroy an uncommon, rare, or epic item for ichor.</BASEFONT>");

        builder.AddButton(25, 172, 4005, 4007, ButtonUpgrade);
        builder.AddHtml(65, 170, 335, 22, "<BASEFONT color=#FFFFFF>Upgrade</BASEFONT>");
        builder.AddHtml(65, 190, 335, 22, "<BASEFONT size=4 color=#CCCCCC>Spend ichor to raise a themed item one tier, up to epic.</BASEFONT>");

        builder.AddHtmlPlaceholder(20, 240, 380, 22, "ichorCount");
    }

    protected override void BuildStrings(ref GumpStringsBuilder builder)
    {
        var carried = _from.Backpack?.GetAmount(typeof(PantheonIchor)) ?? 0;
        builder.SetHtmlText("ichorCount", $"Ichor carried: {carried}", "#FFD700");
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        var from = sender.Mobile;

        // Same gate as DisplayTo — the player may have died/walked away while the gump sat open.
        if (info.ButtonID == 0 || from == null || !from.Alive || _altar?.Deleted != false ||
            !from.InRange(_altar.GetWorldLocation(), 3))
        {
            return;
        }

        switch (info.ButtonID)
        {
            case ButtonOffering:
                {
                    _altar.BeginLegendaryOffering(from);
                    break;
                }
            case ButtonSalvage:
                {
                    _altar.BeginSalvage(from);
                    break;
                }
            case ButtonUpgrade:
                {
                    _altar.BeginUpgrade(from);
                    break;
                }
        }
    }
}
