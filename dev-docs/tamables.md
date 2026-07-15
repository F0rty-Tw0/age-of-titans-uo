# Tamables — 1-Follower Cap + Pantheon Pets & Mounts (v1, 2026-07-15)

The shard's taming lane. Design informed by `reference-shard-creatures.md §5` (pets live on their
own stat budget, never on dungeon-trash numbers). Every ladder dungeon offers a themed
reason to bring a tamer: **1 offensive pet + 1 rideable mount**, hue-matched to the family.

## The three rules

1. **One active creature.** `FollowersMax = 1` for players (staff keep 5). A tamed pet OR
   a summon — the pool is shared. Enforced at every acquisition gate (taming, summon
   spells, stable claim, pet transfer, hirelings, vendors).
2. **A ridden mount is free.** While its master rides it, a controlled mount costs 0
   follower slots; dismounted and following, it takes the slot. Ride your dungeon mount
   AND run one combat pet.
3. **Summons share the cap.** All T2A-reachable summons cost exactly 1 slot (Summon
   Creature, all four elementals, Summon Daemon — daemon was 5, elementals 2–4). A summon
   is *blocked* while a pet is active — stable or release first; nothing is auto-released.

**Deliberate edges:** taming while mounted then dismounting leaves you at 2/1 — allowed;
overflow only blocks *new* acquisitions (also grandfathers pre-cap multi-pet players).
Stable swap is the intended juggle: `AnimalTrainer` gives 2–5 stable slots by skill.

## Mechanics (where the code lives)

| Piece | File |
|---|---|
| Player cap (ctor + login clamp — value is serialized, so it re-clamps every login) | `Projects/UOContent/Mobiles/PlayerMobile.cs` |
| Counted-slots bookkeeping: `FollowersCounted` (non-serialized) + virtual `CountedControlSlots` in `AddFollowers`/`RemoveFollowers` | `Projects/UOContent/Mobiles/BaseCreature.cs` |
| Mount exemption: `CountedControlSlots => 0` while ridden by master; Rider-setter refund/re-charge; `AfterDeserialize` refunds the load-time charge (restart-while-mounted) | `Projects/UOContent/Mobiles/Animals/Mounts/BaseMount.cs` |
| Summon costs (spell `CheckCast` + creature `ControlSlots`, both = 1) | `Spells/Fifth/SummonCreature.cs`, `Spells/Eighth/*.cs`, `Mobiles/Monsters/Summons/*.cs` |

**Invariant:** `RemoveFollowers` subtracts what `AddFollowers` (or the mount hooks) actually
charged — never a recomputed value. Any new dynamic-slot mechanic must go through
`FollowersCounted`, not raw `ControlSlots` math.

## Roster (MinTameSkill ladder 45 → 98.7)

| Dungeon | Pet | Skill | Mount | Skill | Hue |
|---|---|---|---|---|---|
| Despise (Gaian, L4) | `GaianEarthbear` "an earthborn bear" (grizzly) | 55.1 | `GaianOrn` "a gaian orn" (desert ostard) | 45.1 | 0x972 ochre |
| Drowned Tholos (Tide, L4-5) | `TideBull` "a sea-born bull" | 59.1 | `TideSteed` "a tide-born steed" (horse 0xE2) | 47.1 | 0x847 / 0x481 |
| Deceit (Drowned, L5) | `DrownedHound` "a barrow hound" (dire wolf) | 65.1 | `DrownedCharger` (horse 0xCC) | 55.1 | 0x841 / 0x835 |
| Cinderworks (Cinder, L5-6) | `CinderHound` "a forge hound" (hellhound) | 71.1 | `CinderSteed` (horse 0xC8) | 63.1 | 0x654 ember |
| Shame (Brine, L6) | `BrineLynx` (panther) | 75.1 | `BrineOclock` "a brine oclock" (forest ostard) | 67.1 | 0x480 / 0x4F8 |
| Nemean Wildwood (Wyld, L6-7) | `WyldCub` "a nemean cub" (panther) | 82.1 | `WyldCourser` "a moon-marked courser" (horse 0xE4) | 77.1 | 0x501 / 0x486 |
| Destard (Drakon, L7) | `DrakonBroodling` (drake) | 87.1 | `DrakonZostrich` "a drakon zostrich" (frenzied ostard) | 80.1 | 0x501 hoard-gold |
| Hythloth (Tartarus, L8) | `TartarusHellcat` (panther) | 92.1 | `TartarusZostrich` (frenzied ostard) | 85.1 | 0x21 blood-hell |
| Stormcrown Aerie (Storm, L8-9) | `StormDrakeling` (drake) | 96.1 | `StormZostrich` "a storm zostrich" (frenzied ostard) | 90.1 | 0x480 / 0x481 |
| Stygian Deep (Stygian, L9-10) | `StygianWhelp` "a cerberus whelp" (hellhound) | 98.7 | `StygianNightmare` (horse 0xE4, void-black) | 95.1 | 0x453 / 0x455 |

Files live in the family's dungeon folder (`Mobiles/Dungeons/<Dungeon>/` and
`Mobiles/Dungeons/ClassicFive/<Dungeon>/`). **Mount bodies are strictly T2A** — horse
variants (0xC8/0xCC/0xE2/0xE4) and ostards (desert 0xD2 / forest 0xDB / frenzied 0xDA) only. Post-T2A
mount art (kirin, beetle, swamp dragon, skeletal/hell steed, unicorn, seahorse) does NOT
render on this shard's client — verified in-game 2026-07-15; do not reintroduce it.
Ostard-model species use shard fauna names: desert = **orn**, frenzied = **zostrich**,
forest = **oclock**. Poseidon's mount being a horse is lore-correct.

**Balance shape:** pet-budget stats (HP ~100–500, dmg ~8–20 by tier), `ControlSlots = 1`
everywhere, `FightMode.Aggressor`, no XP and no loot bags (pinned **0** in
`LevelConfig.MobLevelOverrides` — the bestiary guard test requires a pin for every
family-prefixed class; bag chance at level 0 is 0.0).

## Spawns

One "Tamables" spawner per dungeon JSON (`Distribution/Data/Spawns/uoml/felucca/*.json`):
count 2 (pet + mount, `maxCount` 1 each), respawn 10–20 min, placeholder coords offset
from the dungeon's entry spawner — relocate alongside the rest when real coords land.

## GM testing

Commands (`Commands/TameTestCommands.cs`): `[TameInfo` (slot bookkeeping inspector),
`[Tamables` (roster echo). Smoke script: `dev-docs/gm-testing-commands.md` § Taming.
