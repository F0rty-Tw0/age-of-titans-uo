using System;
using Server.Engines.LootBags;
using Server.Items;

namespace Server.Commands;

public static class LootTest
{
    public static void Configure()
    {
        CommandSystem.Register("LootTest", AccessLevel.GameMaster, LootTest_OnCommand);
    }

    [Usage("LootTest <level 0-10> [count]")]
    [Description("Creates loot bags of the given level filled via LootRoller, for testing the loot-bag system.")]
    private static void LootTest_OnCommand(CommandEventArgs e)
    {
        if (e.Length < 1)
        {
            e.Mobile.SendMessage("Usage: [LootTest <level 0-10> [count]");
            return;
        }

        var level = Math.Clamp(e.GetInt32(0), 0, 10);
        var count = e.Length >= 2 ? Math.Clamp(e.GetInt32(1), 1, 100) : 1;

        for (var i = 0; i < count; i++)
        {
            var bag = new LootBag(level);
            bag.DropItem(LootRoller.Roll(level, e.Mobile));

            if (e.Mobile.Backpack != null)
            {
                e.Mobile.Backpack.DropItem(bag);
            }
            else
            {
                bag.MoveToWorld(e.Mobile.Location, e.Mobile.Map);
            }
        }

        e.Mobile.SendMessage($"Created {count} level-{level} loot bags.");
    }
}
