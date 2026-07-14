using System;
using Server.Engines.LootBags;
using Server.Engines.Rarity;
using Server.Items;

namespace Server.Commands;

public static class LootTest
{
    public static void Configure()
    {
        CommandSystem.Register("LootTest", AccessLevel.GameMaster, LootTest_OnCommand);
        CommandSystem.Register("LootBag", AccessLevel.GameMaster, LootBag_OnCommand);
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
            bag.DropItem(LootRoller.Roll(level));

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

    [Usage("LootBag <level 0-10> [god|domain] [count]")]
    [Description("Creates pantheon-themed loot bags (god-locked contents), e.g. [LootBag 7 Poseidon 5. Omit the god for a generic bag.")]
    private static void LootBag_OnCommand(CommandEventArgs e)
    {
        if (e.Length < 1)
        {
            e.Mobile.SendMessage("Usage: [LootBag <level 0-10> [god|domain] [count]");
            return;
        }

        var level = Math.Clamp(e.GetInt32(0), 0, 10);
        PantheonDomain? domain = null;

        if (e.Length >= 2 && !TryParseDomain(e.GetString(1), out domain))
        {
            e.Mobile.SendMessage($"Unknown god/domain '{e.GetString(1)}'. Try a god (Poseidon) or a domain (Sea).");
            return;
        }

        var count = e.Length >= 3 ? Math.Clamp(e.GetInt32(2), 1, 100) : 1;

        for (var i = 0; i < count; i++)
        {
            var bag = new LootBag(level, domain);
            bag.DropItem(domain is { } d ? LootRoller.Roll(level, d) : LootRoller.Roll(level));

            if (e.Mobile.Backpack != null)
            {
                e.Mobile.Backpack.DropItem(bag);
            }
            else
            {
                bag.MoveToWorld(e.Mobile.Location, e.Mobile.Map);
            }
        }

        e.Mobile.SendMessage(domain is { } dom
            ? $"Created {count} level-{level} loot bags of {PantheonFx.GetPatronName(dom)}."
            : $"Created {count} generic level-{level} loot bags.");
    }

    // Accepts either the domain name (Sea, Underworld, ...) or the patron god (Poseidon, Hades, ...).
    private static bool TryParseDomain(string text, out PantheonDomain? domain)
    {
        if (Enum.TryParse<PantheonDomain>(text, true, out var byName))
        {
            domain = byName;
            return true;
        }

        for (var i = 0; i <= (int)PantheonDomain.Nature; i++)
        {
            var candidate = (PantheonDomain)i;

            if (text.InsensitiveEquals(PantheonFx.GetPatronName(candidate)))
            {
                domain = candidate;
                return true;
            }
        }

        domain = null;
        return false;
    }
}
