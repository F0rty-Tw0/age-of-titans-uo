using System;
using Server.Engines.Spawners;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands;

// GM testing kit for the custom bestiary (dev-docs/beast-reference.md — all 368 creatures).
// [Beast spawns a single creature for inspection; [BeastSpawner drops a configured spawner.
public static class BeastCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("Beast", AccessLevel.GameMaster, Beast_OnCommand);
        CommandSystem.Register("BeastSpawner", AccessLevel.GameMaster, BeastSpawner_OnCommand);
    }

    private static Type ResolveCreature(Mobile from, string name)
    {
        var type = AssemblyHandler.FindTypeByName(name);

        if (type == null || !type.IsSubclassOf(typeof(BaseCreature)) || type.IsAbstract)
        {
            from.SendMessage($"'{name}' is not a spawnable creature class. Class names are in dev-docs/beast-reference.md.");
            return null;
        }

        return type;
    }

    [Usage("Beast <className>")]
    [Description(
        "Spawns a single creature of the given class at a targeted location, e.g. [Beast StygianLord. " +
        "Every class is listed in dev-docs/beast-reference.md."
    )]
    private static void Beast_OnCommand(CommandEventArgs e)
    {
        if (e.Length != 1)
        {
            e.Mobile.SendMessage("Usage: [Beast <className> — e.g. [Beast LaborNemeanLion");
            return;
        }

        var type = ResolveCreature(e.Mobile, e.GetString(0));

        if (type == null)
        {
            return;
        }

        e.Mobile.SendMessage("Target where the beast should spawn.");
        e.Mobile.BeginTarget(-1, true, TargetFlags.None, (from, targeted) =>
        {
            if (targeted is not IPoint3D p)
            {
                from.SendMessage("Invalid target.");
                return;
            }

            BaseCreature creature;

            try
            {
                creature = (BaseCreature)Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                from.SendMessage($"{type.Name} has no usable parameterless constructor — cannot spawn it.");
                return;
            }

            creature.MoveToWorld(new Point3D(p), from.Map);
            from.SendMessage($"Spawned {type.Name} (level {Engines.Leveling.LevelConfig.GetMobLevel(creature)}).");
        });
    }

    [Usage("BeastSpawner <className> [count=3] [respawnMinutes=5]")]
    [Description(
        "Creates a running Spawner for the given creature class at a targeted location, e.g. " +
        "[BeastSpawner PyreHound 4 10. HomeRange 10; tweak via [props on the spawner."
    )]
    private static void BeastSpawner_OnCommand(CommandEventArgs e)
    {
        if (e.Length < 1)
        {
            e.Mobile.SendMessage("Usage: [BeastSpawner <className> [count=3] [respawnMinutes=5]");
            return;
        }

        var type = ResolveCreature(e.Mobile, e.GetString(0));

        if (type == null)
        {
            return;
        }

        var count = Math.Clamp(e.Length > 1 ? e.GetInt32(1) : 3, 1, 50);
        var minutes = Math.Clamp(e.Length > 2 ? e.GetInt32(2) : 5, 1, 720);

        e.Mobile.SendMessage("Target where the spawner should be placed.");
        e.Mobile.BeginTarget(-1, true, TargetFlags.None, (from, targeted) =>
        {
            if (targeted is not IPoint3D p)
            {
                from.SendMessage("Invalid target.");
                return;
            }

            var spawner = new Spawner(
                count,
                TimeSpan.FromMinutes(minutes),
                TimeSpan.FromMinutes(minutes + 1),
                0,
                default,
                type.Name
            )
            {
                HomeRange = 10
            };

            spawner.MoveToWorld(new Point3D(p), from.Map);
            spawner.Respawn();
            from.SendMessage(
                $"Spawner placed: {type.Name} ×{count}, respawn {minutes}–{minutes + 1} min, home range 10. Edit with [props."
            );
        });
    }
}
