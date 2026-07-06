using Server.Engines.Leveling;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Commands
{
    public static class LevelCommand
    {
        public static void Configure()
        {
            CommandSystem.Register("Level", AccessLevel.Player, Level_OnCommand);
            CommandSystem.Register("LevelGuide", AccessLevel.Player, LevelGuide_OnCommand);
        }

        [Usage("LevelGuide")]
        [Description("Opens the window explaining how the leveling system works.")]
        private static void LevelGuide_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                pm.SendGump(new LevelingPrimerGump());
            }
        }

        [Usage("Level")]
        [Description("Displays your current level, total experience, and experience remaining to the next level.")]
        private static void Level_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is not PlayerMobile pm)
            {
                return;
            }

            var level = LevelSystem.GetLevel(pm);
            var xp = LevelSystem.GetXP(pm);

            if (level >= LevelConfig.MaxLevel)
            {
                pm.SendMessage($"You are level {level} (maximum) with {xp} total experience.");
                return;
            }

            var remaining = LevelConfig.XPToReach(level + 1) - xp;

            pm.SendMessage($"You are level {level} with {xp} total experience.");
            pm.SendMessage($"You need {remaining} more experience to reach level {level + 1}.");
        }
    }
}
