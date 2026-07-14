using System;
using System.Collections.Generic;

namespace Server.Mobiles;

// Shared ability plumbing for dungeon-ladder mobs (dev-docs/dungeon-ladder.md).
// Two patterns: a throttled damage aura driven from OnThink, and a capped add-spawn
// used by summoner elites/bosses. Both are deliberately non-serialized — auras and
// add tracking rebuild naturally after a restart.
public static class DungeonAbilities
{
    // Throttled adjacent-tile damage aura. Call from OnThink; nextPulse is a
    // non-serialized per-instance field. Only harms players and player pets.
    public static void AuraPulse(BaseCreature source, ref DateTime nextPulse, TimeSpan interval, int damage)
    {
        if (Core.Now < nextPulse || source.Deleted || !source.Alive || source.Map == null)
        {
            return;
        }

        nextPulse = Core.Now + interval;

        foreach (var m in source.Map.GetMobilesInRange(source.Location, 1))
        {
            var root = (m as BaseCreature)?.GetMaster() ?? m;

            if (m != source && m.Alive && root is PlayerMobile && source.CanBeHarmful(m))
            {
                source.DoHarmful(m);
                m.Damage(damage, source);
            }
        }
    }

    // Spawns one add next to owner unless cap of its previous adds are still alive.
    // adds is a non-serialized per-instance list; pruned in place. Caller clears the
    // list in OnAfterDelete (audit rule: null out refs).
    public static void TrySpawnAdd(BaseCreature owner, List<BaseCreature> adds, int cap, Func<BaseCreature> factory)
    {
        for (var i = adds.Count - 1; i >= 0; i--)
        {
            if (adds[i].Deleted || !adds[i].Alive)
            {
                adds.RemoveAt(i);
            }
        }

        if (adds.Count >= cap || owner.Map == null || owner.Deleted)
        {
            return;
        }

        var add = factory();
        // Summoned: no XP/fame/loot-bag awards (BaseCreature kill-award gate), dispellable,
        // no corpse on T2A — otherwise boss adds become a farmable reward font and orphaned
        // adds pile up after the owner dies.
        add.Summoned = true;
        add.Team = owner.Team;
        add.MoveToWorld(owner.Location, owner.Map);
        add.Combatant = owner.Combatant;
        adds.Add(add);
    }
}
