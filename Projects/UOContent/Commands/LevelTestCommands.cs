using Server.Engines.Leveling;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;

namespace Server.Commands;

// GM testing kit for the leveling system and the newbie dungeon (dev-docs/newbie-dungeon.md).
// The production XP path (LevelSystem.AwardXP) exempts staff, so testing a level directly needs
// its own entry point — see LevelSystem.SetLevel.
public static class LevelTestCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("SetLevel", AccessLevel.GameMaster, SetLevel_OnCommand);
        CommandSystem.Register("GiveXP", AccessLevel.GameMaster, GiveXP_OnCommand);
        CommandSystem.Register("NewbieBarrow", AccessLevel.GameMaster, NewbieBarrow_OnCommand);
    }

    [Usage("SetLevel <0-10>")]
    [Description(
        "Targets a player and sets their level directly (XP snapped to that level's threshold). " +
        "Raising replays every level-up crossed (stat top-up, caps, the level-4 coin/bolt grant); " +
        "lowering just resets the level and caps."
    )]
    private static void SetLevel_OnCommand(CommandEventArgs e)
    {
        if (e.Length != 1 || !int.TryParse(e.GetString(0), out var level) || level < 0 || level > LevelConfig.MaxLevel)
        {
            e.Mobile.SendMessage($"Usage: [SetLevel <0-{LevelConfig.MaxLevel}>");
            return;
        }

        e.Mobile.SendMessage("Target the player to set their level.");
        e.Mobile.BeginTarget(-1, false, TargetFlags.None, (from, targeted) =>
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage("That is not a player.");
                return;
            }

            LevelSystem.SetLevel(pm, level);
            from.SendMessage($"{pm.Name} is now level {level}.");
        });
    }

    [Usage("GiveXP <amount>")]
    [Description(
        "Targets a player and awards them XP via the production AwardXP path. Must target a " +
        "Player-access character — staff targets no-op by design."
    )]
    private static void GiveXP_OnCommand(CommandEventArgs e)
    {
        if (e.Length != 1 || !int.TryParse(e.GetString(0), out var amount) || amount <= 0)
        {
            e.Mobile.SendMessage("Usage: [GiveXP <amount>");
            return;
        }

        e.Mobile.SendMessage("Target the player to award XP.");
        e.Mobile.BeginTarget(-1, false, TargetFlags.None, (from, targeted) =>
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage("That is not a player.");
                return;
            }

            LevelSystem.AwardXP(pm, amount);
            from.SendMessage($"Awarded {amount} XP to {pm.Name} (no-ops if they are staff-access).");
        });
    }

    [Usage("NewbieBarrow")]
    [Description("Teleports you to the newbie dungeon's entrance GoLocation (dev-docs/newbie-dungeon.md).")]
    private static void NewbieBarrow_OnCommand(CommandEventArgs e)
    {
        var from = e.Mobile;

        foreach (var region in Region.Regions)
        {
            if (region is NewbieDungeonRegion && region.GoLocation != Point3D.Zero)
            {
                from.MoveToWorld(region.GoLocation, region.Map);
                from.SendMessage("Teleported to the Barrow entrance.");
                return;
            }
        }

        from.SendMessage("No newbie dungeon region with a GoLocation found.");
    }
}
