# Newbie Dungeon — Setup Guide

Operational runbook for standing up the Barrow of the Unremembered on a live
map. Design: `dev-docs/newbie-dungeon.md`. GM commands + smoke script:
`dev-docs/gm-testing-commands.md`. Per-mob stats: `dev-docs/newbie-dungeon-mobs.md`.

Everything ships with **placeholder coordinates** (Felucca x6700-6850,
y700-750, every entry marked `PLACEHOLDER COORDS`). The dungeon is not live
until you re-point those at a real map area and reboot.

## 1. Prerequisites (once per code change)

```bash
# compile-verify while the shard runs (never locks Distribution)
dotnet build Projects/UOContent/UOContent.csproj -p:OutDir=scratch-verify -p:SolutionDir=E:/age-of-titans-uo/

# serialization migrations (only after [SerializationGenerator] changes)
dotnet run --project Projects/BuildTool -- --action migrate

# full tests — shard must be STOPPED (Distribution DLLs lock)
MODERNUO_TEST_DATA_DIR='F:\UO' dotnet test Projects/UOContent.Tests/

# production build, then run from Distribution/
dotnet build
```

## 2. Pick the location

You need one contiguous map area with four zones, roughly:

| Zone | Suggested size | Mood |
|---|---|---|
| Entrance | ~20×20 | greeter + safe mouth |
| Trash L1 | ~35×40 | first kills |
| Trash L2 | ~35×40 | poison + speed lessons |
| Elite depth | ~45×50 | 3 elite chambers + sealed gate decor |

Walk the area in-game as GM and note the corner coordinates of each zone
(client status bar / `[props` on self shows position). Also pick:

- **GoLocation** — a tile INSIDE the entrance (the `[NewbieBarrow` teleport
  target).
- **EjectLocation** — a tile OUTSIDE the dungeon mouth (where over-level
  players get moved if they recall/teleport in). Do NOT put this inside any
  of the four zone rects — that reintroduces the eject-into-the-dungeon bug.

## 3. Regions — `Distribution/Data/regions.json`

Four `NewbieDungeonRegion` entries near the end of the file. Replace every
`Area` rect, the entrance `GoLocation`, and all four `EjectLocation` values.
Remove the `(PLACEHOLDER COORDS)` suffix from `Name` when done.

Per-entry knobs (all JSON-settable, no code):

| Field | What it does | Shipped values |
|---|---|---|
| `LightLevel` | ambient darkness, 0 day → 26 near-black | 6 / 12 / 20 / 26 ramp |
| `Music` | looping track, auto-swaps on zone cross | Approach / Cave01 / Dungeon2 / Death |
| `EntryFlash` | DarkFlash screen effect on entering the zone | true on L2 + Elite |
| `GoLocation` | `[NewbieBarrow` anchor | entrance only |
| `EjectLocation` | where gate-failers get moved | all four, same point |

Regions load at boot — **restart required** after editing.

Behavior baked into the region type (code, not config): player↔player harm
blocked (incl. fields, potions, pets), no criminal flags, corpse loot
owner-only, entry gate character level ≤ 3 (staff exempt).

## 4. Spawners — `Distribution/Data/Spawns/uoml/felucca/newbie-dungeon.json`

Six spawner objects; update each `location` to sit inside its matching zone
rect from step 3:

| Spawner | Contents | Count | Respawn |
|---|---|---|---|
| Entrance | NewbieFerryman (greeter) | 1 | 30s–1m |
| Trash L1 | BoneShade 5, GraveRat 4, CorpseCrawler 4, BoneBowman 2, BarrowBat 2 | 17 | 1–2 min |
| Trash L2 | GraveMiasma 4, RestlessArcher 3, Mourner 2, Chanter 1, GraveArcher 2, Wight 1 | 13 | 2 min |
| Elite ×3 | one spawner per named elite | 1 each | 15 min |

Spawn files also load at boot. Alternative: place spawners live as GM
(`[add Spawner` + gump) and skip the JSON — the JSON exists so the setup is
reproducible on a fresh world.

## 5. Boot & smoke

Start the shard, then run the GM smoke script from
`dev-docs/gm-testing-commands.md` § Leveling / newbie dungeon. Short form:

1. `[NewbieBarrow` → you land at the entrance; music + darkness change.
2. `[SetLevel 0` on a test player → mob tags read yellow (L1) / red (L2/L3).
3. Kill trash → XP messages; ~5-10% of kills drop a hued loot bag.
4. Kill an elite → guaranteed bag 2 + corpse sparkle + shimmer sound.
5. `[GiveXP` across level 4 → lightning bolt + Ferryman's Coin in backpack.
6. Walk the level-4 char back to the entrance → blocked at the border with
   the Ferryman line; recall inside → moved to EjectLocation.
7. Two test players: melee swing, fire/poison field, explosion potion, pet
   attack against each other → all blocked; no criminal flag; cross-loot a
   corpse → denied.

## 6. Optional dressing (all GM-placeable, no code)

- **Torches/braziers** — `[add Torch` etc. (BaseLight items) for warm pools
  in the entrance; sparse in L1 so the darkness ramp reads.
- **EffectController nodes** — `[add EffectController`, set via `[props`:
  `TriggerType = InRange`, sound-only drips/growls in corridors; a Sequenced
  self-looping Lightning node makes a chamber strobe. Serialized, survives
  restarts.
- **Blood + bones** — `[add Blood` near the L2→elite transition.
- **Sealed gate** — static portcullis/door props in the elite depth, no
  mechanics; it's the "future Hades content" promise.
- **Particle caveat** — `Effects.ParticleSupportType` defaults to `Detect`,
  which may withhold particle FX from ClassicUO clients (launch-checklist
  item in the design doc — verify in-game before relying on particle-based
  dressing; sounds/lights/screen-flashes are unaffected).

## 7. Tuning knobs (live-tune, no redesign)

| Knob | Where | Shipped |
|---|---|---|
| Bag drop % per mob level | `Engines/LootBags/LootBagConfig.cs` | global table (shard-wide — L1/L2 changes affect the whole world) |
| Bag rarity ceilings | `Engines/Rarity/RarityConfig.MaxRarityForBagLevel` | L0-1 Uncommon, L2-3 Rare |
| Elite HP/damage | each elite class in `Mobiles/NewbieDungeon/` | 170-240 HP — UNVALIDATED, tune vs a real level-3 char |
| Elite respawn | spawner JSON `minDelay`/`maxDelay` | 15 min |
| Entry gate level | `NewbieDungeonRegion.OnMoveInto`/`OnEnter` | ≤ 3 |
| Light/music/flash | regions.json | see §3 |
| Mob XP levels | `LevelConfig.MobLevelOverrides` pins | L1×3, L2×2, L3×3 |

## 8. Troubleshooting

- **Region ignored at boot** — `$type` must be exactly `NewbieDungeonRegion`
  and the type registered in `Regions/RegionJsonRegistration.cs` (it is);
  malformed JSON fails the whole file — validate with
  `python -c "import json; json.load(open('Distribution/Data/regions.json'))"`.
- **No spawns** — spawner `map` must be `Felucca` and `location` inside the
  region rect you actually walked to; check the spawner gump in-game.
- **Gate lets a level-4 in** — only staff bypass; check the character's real
  level with `[Level`. Ghosts and pets follow their owner's gate result.
- **No music change** — music only re-sends when the track differs from the
  previous region; identical `Music` values on adjacent zones = silence.
- **Elite drops two bags** — elites must keep `DropsLootBag => false`
  (NewbieElite base); a new elite subclass that overrides it back on gets the
  global roll AND the forced drop.

## 9. Quests — The Ferryman's Toll

A 5-quest chain given and turned in by the greeter `NewbieFerryman` (the
Ferryman's Shade). Once-per-character: the terminal link is `OneTimeOnly`, so
finishing it records the chain done and it is never offered again.
Definitions: `Projects/UOContent/Engines/ML Quests/Definitions/FerrymansToll.cs`.

| # | Quest | Objective | Reward |
|---|---|---|---|
| 1 | First Blood | kill 5 `NewbieBoneShade` (frail skeletons) | a pauper's grave-gift (filled level-0 loot bag) |
| 2 | Grave Goods | kill 8 `NewbieGraveMiasma` / `NewbieRestlessArcher` | a grave-gift (filled level-1 loot bag) |
| 3 | The Price of Passage | salvage 1 item at the Pantheon Altar | 10 Pantheon Ichor |
| 4 | Tempered in Shadow | upgrade 1 item at the Pantheon Altar | a grave-gift (filled level-1 loot bag) |
| 5 | The Road Down | reach character level 2 | a king's grave-gift (filled level-2 loot bag — once ever; elites stay the only repeatable Rare source) |

Enabling (all three, then **restart** — the engine reads the flag and the cfg
at boot):

1. `Distribution/Configuration/modernuo.json` →
   `"questSystem.enableMLQuests": "True"` (shipped flipped on).
2. `Distribution/Data/MLQuests.cfg` → the five
   `<Quest>\tNewbieFerryman` lines (tab-separated; shipped under the
   `# The Ferryman's Toll` heading).
3. The greeter spawns from the entrance spawner (§4) — no extra NPC wiring;
   `BaseCreature` is the quest giver.

**Session-only progress caveat.** The salvage, upgrade, and reach-level
objectives (quests 3–5) track progress as an in-memory flag, not saved state
(`ExtraDataType.None`). A server restart while one of those quests is active
drops the flag — the player must redo that quest's action (salvage/upgrade
again, or gain the level again) after the restart. The kill quests (1–2) use
the engine's serialized kill counter and survive a restart normally.
