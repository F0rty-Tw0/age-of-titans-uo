using Server.Gumps;

namespace Server.Engines.Leveling;

public class LevelingPrimerGump : StaticGump<LevelingPrimerGump>
{
    private const string Body =
        "Kill monsters to earn experience. Big monsters give more. Tiny monsters give none.<br><br>" +
        "When you level up, your Strength, Dexterity, and Intelligence go up.<br><br>" +
        "Put a stat arrow UP to choose where points go. If no arrows are UP, points are shared.<br><br>" +
        "No stat can go over 200.<br><br>" +
        "You can still train stats yourself. If you train early, your next level just gives fewer points.<br><br>" +
        "Each level also lets your skills go higher, up to 100 at level 5.<br><br>" +
        "After level 5, levels do not give more stats. Skills and gear make you stronger.<br><br>" +
        "Say <B>[level</B> to see your progress. Say <B>[levelguide</B> to see this again.";

    public override bool Singleton => true;

    public LevelingPrimerGump() : base(50, 50)
    {
    }

    protected override void BuildLayout(ref StaticGumpBuilder builder)
    {
        builder.AddPage();
        builder.AddBackground(0, 0, 440, 370, 5054);
        builder.AddAlphaRegion(10, 10, 420, 350);

        builder.AddHtml(15, 15, 410, 22, "How Leveling Works", color: "#FFC000", align: TextAlignment.Center);
        builder.AddHtml(15, 45, 410, 275, Body, background: false, scrollbar: true);

        builder.AddButton(195, 330, 4005, 4007, 0);
        builder.AddHtml(230, 332, 100, 20, "Got it", color: "#FFFFFF");
    }
}
