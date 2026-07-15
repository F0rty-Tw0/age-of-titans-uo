using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands;

// GM testing kit for the taming lane (dev-docs/tamables.md): the 1-follower cap, the
// free-ridden-mount rule, and the 20 pantheon tamables. [TameInfo exposes the otherwise
// invisible counted-slots bookkeeping; [Tamables echoes the roster + skill ladder.
public static class TameTestCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("TameInfo", AccessLevel.GameMaster, TameInfo_OnCommand);
        CommandSystem.Register("Tamables", AccessLevel.GameMaster, Tamables_OnCommand);
    }

    [Usage("TameInfo")]
    [Description(
        "Target a creature → taming stats + follower-slot bookkeeping (ControlSlots vs " +
        "FollowersCounted, rider state). Target a player → Followers/FollowersMax + every " +
        "controlled creature with its counted slots."
    )]
    private static void TameInfo_OnCommand(CommandEventArgs e)
    {
        e.Mobile.SendMessage("Target a creature or a player.");
        e.Mobile.BeginTarget(-1, false, TargetFlags.None, (from, targeted) =>
        {
            switch (targeted)
            {
                case BaseCreature bc:
                    {
                        from.SendMessage(
                            $"{bc.GetType().Name}: Tamable={bc.Tamable}, MinTameSkill={bc.MinTameSkill:F1}, " +
                            $"ControlSlots={bc.ControlSlots}, CountedControlSlots={bc.CountedControlSlots}, " +
                            $"FollowersCounted={bc.FollowersCounted}"
                        );
                        from.SendMessage(
                            $"Controlled={bc.Controlled}, Master={bc.ControlMaster?.RawName ?? bc.SummonMaster?.RawName ?? "none"}, " +
                            $"Summoned={bc.Summoned}, Stabled={bc.IsStabled}" +
                            (bc is BaseMount mount ? $", Rider={mount.Rider?.RawName ?? "none"}" : "")
                        );
                        break;
                    }
                case PlayerMobile pm:
                    {
                        from.SendMessage($"{pm.RawName}: Followers {pm.Followers}/{pm.FollowersMax}");

                        foreach (var follower in pm.AllFollowers)
                        {
                            if (follower is BaseCreature bc)
                            {
                                var state = bc is BaseMount m && m.Rider == pm ? "RIDDEN" :
                                    bc.IsStabled ? "stabled" : "active";
                                from.SendMessage(
                                    $"  {bc.GetType().Name} [{state}]: slots={bc.ControlSlots}, counted={bc.FollowersCounted}"
                                );
                            }
                        }

                        break;
                    }
                default:
                    {
                        from.SendMessage("Target a creature or a player.");
                        break;
                    }
            }
        });
    }

    [Usage("Tamables")]
    [Description("Lists the 20 pantheon tamables (dungeon, pet, mount, MinTameSkill ladder).")]
    private static void Tamables_OnCommand(CommandEventArgs e)
    {
        // Roster + ladder: dev-docs/tamables.md. Kept as data so the echo stays one line per dungeon.
        (string Dungeon, string Pet, double PetSkill, string Mount, double MountSkill)[] roster =
        {
            ("Despise (Gaian)", nameof(GaianEarthbear), 55.1, nameof(GaianOrn), 45.1),
            ("Drowned Tholos (Tide)", nameof(TideBull), 59.1, nameof(TideSteed), 47.1),
            ("Deceit (Drowned)", nameof(DrownedHound), 65.1, nameof(DrownedCharger), 55.1),
            ("Cinderworks (Cinder)", nameof(CinderHound), 71.1, nameof(CinderSteed), 63.1),
            ("Shame (Brine)", nameof(BrineLynx), 75.1, nameof(BrineOclock), 67.1),
            ("Nemean Wildwood (Wyld)", nameof(WyldCub), 82.1, nameof(WyldCourser), 77.1),
            ("Destard (Drakon)", nameof(DrakonBroodling), 87.1, nameof(DrakonZostrich), 80.1),
            ("Hythloth (Tartarus)", nameof(TartarusHellcat), 92.1, nameof(TartarusZostrich), 85.1),
            ("Stormcrown Aerie (Storm)", nameof(StormDrakeling), 96.1, nameof(StormZostrich), 90.1),
            ("Stygian Deep (Stygian)", nameof(StygianWhelp), 98.7, nameof(StygianNightmare), 95.1)
        };

        e.Mobile.SendMessage("Pantheon tamables — pet (skill) / mount (skill). Spawn with [Beast <class>:");

        foreach (var row in roster)
        {
            e.Mobile.SendMessage(
                $"  {row.Dungeon}: {row.Pet} ({row.PetSkill:F1}) / {row.Mount} ({row.MountSkill:F1})"
            );
        }
    }
}
