# Open-World Bestiary — Biome Families + The Labors (design, 2026-07-14)

> **IMPLEMENTED 2026-07-14** (uncommitted): 30 biome classes in `Mobiles/OpenWorld/`,
> 6 Labors in `Mobiles/OpenWorld/Labors/` (all `DungeonElite`, guaranteed bag = level,
> 60-120 min timers at real landmarks), spawn files `open-world-families.json` (20
> spawners, real coords; Shore marked APPROX — verify sand tiles in-game) + `labors.json`.
> 36 LevelConfig pins, build clean, migrations generated, trap sweep clean.

> Design doc. Companion to `dev-docs/dungeon-ladder-bestiary.md` and
> `dev-docs/classic-five-bestiary.md` (same table format, same toolbox, same hard rules).
> This spec enriches the **open overworld** — the felucca wilderness outside the dungeons —
> with **five biome families (~6 each, 30 total)** that spawn alongside the stock overworld
> fauna, plus **The Labors: 6 named roaming world-hunt elites** at fixed, thematically-fitting
> landmarks.
>
> The open world is a **gentle band** (L2-6): families span 2 levels each, sit below the
> dungeon ladder, and are where a fresh graduate of the Barrow first meets leveled content in
> the wild. The Labors are the exception — L4-8 contested elites that reward a guaranteed bag.

## How to read this

Same conventions as the dungeon bestiaries:

- **Donor class** — a stock T2A body verified present under `Projects/UOContent/Mobiles/`
  (all checked 2026-07-14). Copy the donor's `Body`/`BaseSoundID`; the hue recolors it. Class
  names use the biome prefix (`Grove/Peak/Mire/Restless/Shore`) or `Labor` for the hunts.
- **Immunities/Poison/Breath** — zero-cost overrides. `HitPoison`, pack instinct, and
  immunities are free stat-block features (**not** counted against the custom-ability budget).
- **Ability** — `—` means pure stat block (the majority). A named ability is one entry from
  the shared toolbox: `OnGaveMeleeAttack` proc (stam/mana drain + flavor msg),
  `OnGotMeleeAttack` proc (reflect / blink / capped add via `DungeonAbilities.TrySpawnAdd`), or
  a damage aura (`DungeonAbilities.AuraPulse`). Reused plumbing only — no new systems.
- **Loot / bag** — **open-world trash carry NO `LootBagLevel` override.** They ride the
  natural mob-level → bag mapping and drop normal loot packs. Each gets a
  `LevelConfig.MobLevelOverrides` **pin** at its **Lvl** so the `[lvl N]` tag, XP gap, and
  derived bag stay stable while HP is tuned (HP is kept in-band anyway, so the pin is honest).
- **The Labors** are `DungeonElite` subclasses: guaranteed bag = **their level**
  (`EliteBagLevel = Lvl`, `EliteBagCount = 1`), the built-in **over-level clamp** throttles
  farming by high-level players, and each respawns on a **60-120 min** timer.
- HP stays inside each level's band (`LevelConfig.MobLevelFromHits`): L2 ≤100, L3 ≤160,
  L4 ≤240, L5 ≤380, L6 ≤550, L7 ≤720, L8 ≤950.

---

## 1. Groves — forests · fey / satyr-touched (L2-3) · `Grove*`

*Artemis's untamed wood: deer and wolves wearing a fey glamour, and the satyrs who pipe them
into a fury. The gentlest leveled content in the game — a graduate's first taste of the wild.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `GroveStag` | a grove stag | Great Hart | forest green 0x844 | 2 | 80-100 | 5-8 | Melee | Fast | — | — |
| `GroveThornwolf` | a thornwolf | Grey Wolf | mossy 0x844 | 2 | 85-100 | 5-8 | Melee | Fast | — | pack instinct |
| `GroveBriarboar` | a briar boar | Boar | mossy 0x844 | 3 | 130-155 | 7-11 | Melee | Fast | — | — |
| `GroveFaun` | a grove faun | Satyr | forest green 0x851 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `GroveNettleback` | a nettleback spider | Giant Spider | mossy green 0x851 | 3 | 125-150 | 7-10 | Melee | Medium | — | HitPoison Lesser |
| `GrovePiper` | a satyr piper | Satyr | dusk violet 0x491 | 3 | 135-160 | 7-11 | Melee | Medium | — | **Beguiling Pipes** — OnGaveMelee 20%: 8 stam drain + msg |

Custom: 1 (Piper). Pure stat blocks: 5.

---

## 2. Peaks — mountains · bronze-age giant-kin / rocs (L4-5) · `Peak*`

*The high crags where the old giant-blood lingers: bronze-skinned ettins and cyclopes, and
rocs riding the updrafts. The step up from the Groves — the first real melee walls in the
open world.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PeakRoc` | a mountain roc | Harpy | slate grey 0x492 | 4 | 200-235 | 12-16 | Melee | Fast | — | — |
| `PeakTur` | a wild tur | Grizzly Bear | slate 0x455 | 4 | 190-225 | 11-15 | Melee | Fast | — | — |
| `PeakBronzeEttin` | a bronze-age ettin | Ettin | bronze 0x798 | 4 | 210-238 | 12-16 | Melee | Medium | — | — |
| `PeakCragOgre` | a crag ogre | Ogre | granite 0x455 | 5 | 320-370 | 13-18 | Melee | Slow | — | — |
| `PeakCyclops` | a bronze cyclops | Cyclops | bronze 0x798 | 5 | 330-380 | 14-19 | Melee | Medium | BleedImmune | — |
| `PeakThunderroc` | a thunder roc | Harpy | storm grey 0x492 | 5 | 320-360 | 13-18 | Melee | Fast | — | **Downdraft** — OnGaveMelee 20%: knockback msg + 12 stam drain |

Custom: 1 (Thunderroc). Pure stat blocks: 5.

---

## 3. Mire — swamps · Lernaean hydra-spawn / marsh horrors (L5-6) · `Mire*`

*The Hydra's brood seeped out of the fens: serpent-spawn that split when struck, bog horrors,
and poison everywhere. The open world's poison-discipline check.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MireAlligator` | a mire alligator | Alligator | murk 0x844 | 5 | 300-350 | 12-17 | Melee | Medium | — | — |
| `MireSkulker` | a mire skulker | Lizardman | murk 0x844 | 5 | 290-330 | 11-16 | Melee | Fast | — | — |
| `MireToad` | a bloated marsh toad | Giant Toad | sickly green 0x851 | 5 | 290-340 | 11-16 | Melee | Slow | PoisonImmune | HitPoison Regular |
| `MireSerpentspawn` | a lernaean spawn | Giant Serpent | swamp green 0x851 | 5 | 310-360 | 13-18 | Melee | Medium | PoisonImmune | HitPoison Greater |
| `MireBogthing` | a bog horror | Bog Thing | murk 0x844 | 6 | 470-520 | 15-20 | Melee | Slow | PoisonImmune | — |
| `MireHydraspawn` | a hissing hydra-spawn | Giant Serpent | venom violet 0x491 | 6 | 480-540 | 15-20 | Melee | Medium | PoisonImmune | **Sever & Split** — OnGotMelee 12%: `TrySpawnAdd` 1 `Snake` (cap 2, dispel-vulnerable); HitPoison Deadly |

Custom: 1 (Hydraspawn). Pure stat blocks: 5.

---

## 4. Restless — graveyards · the unburied dead (L2-3) · `Restless*`

*The overworld graveyards, echoing the Barrow of the Unremembered: the dead who never got
their coin for the ferryman, clawing up from unmarked ground. Low band — where a barrow
graduate keeps their footing before the dungeons.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RestlessSkeleton` | a restless skeleton | Skeleton | bone 0x835 | 2 | 80-100 | 5-8 | Melee | Medium | — | — |
| `RestlessZombie` | an unburied zombie | Zombie | grave teal 0x847 | 2 | 85-100 | 5-8 | Melee | Slow | BleedImmune | — |
| `RestlessGhoul` | a graveyard ghoul | Ghoul | pallid 0x481 | 3 | 130-155 | 7-11 | Melee | Fast | — | — |
| `RestlessShade` | a mourning shade | Spectre | void grey 0x455 | 3 | 125-150 | 7-10 | Mage | Medium | — | — |
| `RestlessWight` | a barrow wight | Skeleton | grave violet 0x454 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `RestlessWraith` | a keening wraith | Wraith | void grey 0x455 | 3 | 130-160 | 7-11 | Mage | Fast | — | **Grave Chill** — OnGaveMelee 20%: 8 mana drain + msg |

Custom: 1 (Wraith). Pure stat blocks: 5. *(Class names avoid the existing `RestlessSoul` — the
only stock `Restless*` type.)*

---

## 5. Shore — coasts · siren-touched / tide beasts (L3-4) · `Shore*`

*The tide-line: sirens luring off the rocks, giant crabs, reef serpents. Spans the Groves↔Peaks
gap for coastal levelers.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ShoreCrab` | a giant shore crab | Scorpion | tide blue 0x530 | 3 | 130-155 | 7-11 | Melee | Slow | — | — |
| `ShoreTideeel` | a tide eel | Snake | pale aqua 0x481 | 3 | 125-150 | 7-10 | Melee | VeryFast | — | HitPoison Regular |
| `ShoreReefserpent` | a reef serpent | Giant Serpent | reef green 0x851 | 4 | 205-238 | 12-16 | Melee | Medium | — | — |
| `ShoreSiren` | a shoal siren | Harpy | siren teal 0x481 | 4 | 200-235 | 11-16 | Melee | Fast | — | — |
| `ShoreBrineling` | a brineling | Water Elemental | tide blue 0x530 | 4 | 210-238 | 11-16 | Melee | Slow | PoisonImmune | — |
| `ShoreLuresiren` | a luring siren | Harpy | siren violet 0x491 | 4 | 205-238 | 12-16 | Melee | Fast | — | **Siren's Call** — OnGaveMelee 20%: 10 stam drain + msg |

Custom: 1 (Luresiren). Pure stat blocks: 5. *(Reef serpent uses the land-walking `Giant Serpent`
body, not `Sea Serpent` — `SeaSerpent` is `CantWalk` and needed a walk-fix in the classic-five
build. Sidestepped here.)*

---

## 6. The Labors — named roaming world-hunts (L4-8) · `Labor*`

*Heracles's six beasts, loose in the wilds. Each is a single named `DungeonElite` at a fixed
real landmark, contested (open-world, no instancing), with a guaranteed bag = its level. The
over-level clamp is the anti-abuse lever; long respawns keep them scarce.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability | Location | Respawn |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `LaborCeryneianHind` | the Ceryneian Hind | Great Hart | golden 0x486 | 4 | 230-240 | 12-16 | Melee | VeryFast | — | **Bounding Flight** — OnGotMelee 25%: blink 6-8 tiles from attacker (a chase fight) | golden glades, Lost Lands [4552,1447] | 60 min |
| `LaborCalydonianBoar` | the Calydonian Boar | Boar | tusked russet 0x21E | 5 | 365-380 | 13-18 | Melee | Fast | — | **Tusk Sweep** — OnGaveMelee 20%: knockback msg + 12 stam drain | the wilds E of Yew [846,1071] | 60 min |
| `LaborNemeanLion` | the Nemean Lion | Cougar | tawny bronze 0x798 | 6 | 530-550 | 15-20 | Melee | Fast | BleedImmune | **Impervious Hide** — OnGotMelee 20%: reflect 25% of the blow, no debuff | the crag dens W of Britain [1055,1442] | 90 min |
| `LaborStymphalianHarpy` | the Stymphalian Matriarch | Harpy | brass-feather 0x8A5 | 6 | 530-550 | 15-20 | Melee | Fast | — | **Roused Flock** — OnGotMelee 15%: `TrySpawnAdd` 1 `Harpy` (cap 3, dispel-vulnerable) | the reed-lake S of Britain [1915,2385] | 90 min |
| `LaborErymanthianBoar` | the Erymanthian Boar | Boar | frost-hide 0x481 | 7 | 690-720 | 17-22 | Melee | Fast | — | **Goring Charge** — OnGaveMelee 25%: knockback msg + 15 stam drain | the high peaks N of Britain [1749,474] | 90 min |
| `LaborCretanBull` | the Cretan Bull | Bull | maddened red 0x21 | 8 | 900-950 | 20-25 | Melee | Fast | BleedImmune | **Maddened Charge** — OnGaveMelee 25%: knockback msg + 18 stam drain | the pastures E of Britain [2512,1066] | 120 min |

Custom: 6 (all six carry one signature — elites earn it). `Snake`/`Harpy` adds ride existing
`TrySpawnAdd`, dispel-vulnerable, capped.

**Name safety** — `Nemean`, `Cretan`, `Stymphal`, `Erymanthos` appear in
`dev-docs/itemization` only as legendary **item** display names, never as mob classes; all six
`Labor*` **class** names are collision-free (grep-verified). Sharing a proper noun between a
legendary weapon and an open-world beast is intended flavor (the `WyldNemean` precedent). The
Stymphalian hunt is displayed **the Stymphalian Matriarch** to distinguish the mob from the
`Stymphal*` archery legendary.

---

## Spawn-wiring appendix (real overworld coords)

Additive spawners into a new sibling file under `Distribution/Data/Spawns/shared/felucca/`
(e.g. `open-world-biomes.json`), placed **alongside** the stock fauna already at these coords
(so the wilds feel populated, not replaced). Coords pulled from `Outdoors.json`, `WildLife.json`,
`Graveyards.json`; biome identified by the stock mobs already spawning there.

| Family | Real coords (biome anchor) | Stock neighbors that mark the biome |
|---|---|---|
| **Groves** (forest) | [846,1071] E of Yew · [1666,1290] NW of Britain · [1695,809] N woods · [2512,1066] E of Britain | Boar, Cougar, Panther, Black/Grizzly Bear, Great Hart |
| **Peaks** (mountain) | [1055,1442] W-of-Britain mts · [741,1838] SW peaks · [1474,1297] · [1749,474] N peaks | Ettin, Gazer, Harpy, Ogre, Troll, Air Elemental |
| **Mire** (swamp) | [1915,2385] swamp S of Britain · [2079,1018] Bog near Britain · [2963,3617] southern fen · [2354,3503] Trinsic-area swamp | Alligator, Giant Serpent, Lizardman, Bog Thing, Slime, Snake |
| **Restless** (graveyard) | [2758,867] Vesper/Minoc gy · [1369,1475] Britain gy · [2438,1100] Cove gy · [1285,3731] Trinsic gy · [3407,2652] | Spectre, Wraith, Shade, Skeleton, Zombie, Lich |
| **Shore** (coast) | Beach tiles adjacent to [1285,3731] Trinsic coast, the Britain harbor edge, and Vesper waterline | *(no stock beach spawner to copy — see note)* |

> **Shore caveat (honest gap):** the felucca spawn files hold **open-ocean** SeaLife spawners
> (z ≈ -5, deep water) and inland fauna, but **no temperate beach-edge** spawner to lift coords
> from directly. Shore spawners must be **hand-placed at sand tiles (z ≈ 0) at the water's
> edge** near the coastal landmarks above. This is the one biome that needs manual coord
> placement at build time rather than copy-from-stock.

### The Labors — spawn coords (single-point spawners, `count = 1`)

| Labor | Coord | Landmark | Respawn |
|---|---|---|---|
| the Ceryneian Hind | [4552,1447] | golden glades, Lost Lands | 60 min |
| the Calydonian Boar | [846,1071] | the wilds E of Yew | 60 min |
| the Nemean Lion | [1055,1442] | the crag dens W of Britain | 90 min |
| the Stymphalian Matriarch | [1915,2385] | the reed-lake S of Britain | 90 min |
| the Erymanthian Boar | [1749,474] | the high peaks N of Britain | 90 min |
| the Cretan Bull | [2512,1066] | the pastures E of Britain | 120 min |

---

## Totals appendix

| Section | Classes | Custom-ability | Pure stat blocks |
|---|---|---|---|
| Groves (L2-3) | 6 | 1 | 5 |
| Peaks (L4-5) | 6 | 1 | 5 |
| Mire (L5-6) | 6 | 1 | 5 |
| Restless (L2-3) | 6 | 1 | 5 |
| Shore (L3-4) | 6 | 1 | 5 |
| **Biome families** | **30** | **5** | **25** |
| The Labors (L4-8) | 6 | 6 | 0 |
| **Total** | **36** | **11** | **25** |

**Custom-ability tally (11):** GrovePiper, PeakThunderroc, MireHydraspawn, RestlessWraith,
ShoreLuresiren (biomes, 5) + all 6 Labors. Every custom uses only the reused toolbox
(`OnGaveMeleeAttack` / `OnGotMeleeAttack` procs, `DungeonAbilities.TrySpawnAdd`) — no new
plumbing. Breath, `HitPoison`, pack instinct, and immunities are free and uncounted.

**Loot model:** biome trash = **no `LootBagLevel` override**, natural level→bag mapping, normal
loot packs; each pinned in `LevelConfig.MobLevelOverrides` at its **Lvl**. Labors =
`DungeonElite`, `EliteBagLevel = Lvl`, `EliteBagCount = 1`, over-level clamp on.

**Donor bodies** — all verified present under `Projects/UOContent/Mobiles/` (2026-07-14):
Great Hart, Grey Wolf, Boar, Satyr, Giant Spider, Harpy, Grizzly Bear, Ettin, Ogre, Cyclops,
Alligator, Lizardman, Giant Toad, Giant Serpent, Bog Thing, Skeleton, Zombie, Ghoul, Spectre,
Wraith, Scorpion, Snake, Water Elemental, Cougar, Bull. No `Roc` class exists at T2A — rocs use
the `Harpy` body upsized/rehued (`PeakRoc`, `PeakThunderroc`, `LaborStymphalianHarpy`).

---

# Expansion to 50+ per family (+45 each, design 2026-07-14)

> Deep-population pass. Brings each of the **five biome families** from 6 → **51** members
> (**+45 each, 225 new classes**) so the open world reads as a living wilderness. **The Labors
> are untouched** (they stay 6 named world-hunts). Existing tables above are unchanged;
> everything here is additive.
>
> Same conventions, same toolbox, same hard rules. Every new class is **open-world trash → NO
> `LootBagLevel` override** (natural mob-level → bag; one `LevelConfig.MobLevelOverrides` pin per
> class at its **Lvl**), HP kept in-band, and a **pure stat block wherever possible** — at this
> size **~90% carry no custom code at all**. Each family adds only a handful of custom-ability
> mobs (two themed lieutenants + the mini-boss); with the 1 existing custom that is **4-5 per
> family — well under the ≤8 budget**. Breath, `HitPoison`, pack instinct, immunities stay free.
>
> **Per-family shape (45 = 28 core + 12 themed [two sub-factions × 6] + 4 ambient + 1 mini-boss).**
> Core spreads 6-8 role lines across the band; the two sub-factions give each biome two distinct
> "camps"; the mini-boss is a single `DungeonElite` (bag = its level, `EliteBagCount = 1`,
> over-level clamp on, **30-60 min** respawn) at a real landmark. Bands hold: Grove/Restless
> L2-3, Peak L4-5, Mire L5-6, Shore L3-4.

---

## 1E. Groves — Expansion (+45) · `Grove*` (L2-3)

*Artemis's untamed wood, teeming: grazers and their predators, the deeper fey, spiders and
serpents — plus two fey camps: the drunken **Kōmos** satyr revel and the thorn-fey **Bramble
Court**.*

### Core expansion — beasts, fey, spiders & serpents (28)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `GroveDoe` | a grove doe | Hind | forest green 0x844 | 2 | 80-95 | 5-8 | Melee | Fast | — | — |
| `GroveFawn` | a dappled fawn | Hind | mossy 0x851 | 2 | 80-90 | 5-7 | Melee | VeryFast | — | — |
| `GroveElk` | a grove elk | Great Hart | forest green 0x844 | 3 | 135-160 | 7-11 | Melee | Fast | — | — |
| `GroveWhitehart` | a white hart | Great Hart | forest green 0x851 | 3 | 140-160 | 7-11 | Melee | Fast | — | — |
| `GroveRazorback` | a razorback boar | Boar | mossy 0x851 | 3 | 135-160 | 7-11 | Melee | Fast | — | — |
| `GroveTusker` | a young tusker | Boar | forest green 0x844 | 2 | 85-100 | 5-8 | Melee | Fast | — | — |
| `GroveSowthing` | a bristled sow | Pig | mossy 0x851 | 2 | 80-95 | 5-8 | Melee | Medium | — | — |
| `GroveFangwolf` | a fang wolf | Timber Wolf | mossy 0x851 | 2 | 85-100 | 5-8 | Melee | Fast | — | pack instinct |
| `GroveHuntwolf` | a hunt wolf | Grey Wolf | forest green 0x844 | 3 | 130-155 | 7-11 | Melee | Fast | — | pack instinct |
| `GroveGreywood` | a greywood wolf | Grey Wolf | mossy 0x851 | 2 | 85-100 | 5-8 | Melee | Fast | — | pack instinct |
| `GrovePanther` | a grove panther | Panther | mossy 0x851 | 3 | 130-155 | 7-11 | Melee | VeryFast | — | — |
| `GroveLynx` | a bracken lynx | Cougar | forest green 0x844 | 2 | 85-100 | 5-8 | Melee | VeryFast | — | — |
| `GroveMossbear` | a moss-pelt bear | Black Bear | mossy 0x851 | 3 | 140-160 | 7-11 | Melee | Medium | — | — |
| `GroveShaggybear` | a shaggy bear | Brown Bear | forest green 0x844 | 3 | 140-160 | 7-11 | Melee | Medium | — | — |
| `GroveApe` | a grove ape | Gorilla | mossy 0x851 | 3 | 130-155 | 7-11 | Melee | Fast | — | — |
| `GroveSatyr` | a wood satyr | Satyr | forest green 0x851 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `GroveDryad` | a grove dryad | Pixie | forest green 0x851 | 3 | 125-150 | 7-10 | Mage | Fast | — | — |
| `GroveSprite` | a thorn sprite | Pixie | dusk violet 0x491 | 2 | 80-95 | 5-8 | Mage | VeryFast | — | — |
| `GroveWisp` | a grove wisp | Wisp | forest green 0x844 | 3 | 125-150 | 7-10 | Mage | VeryFast | — | — |
| `GroveWillowisp` | a willow wisp | Wisp | dusk violet 0x491 | 2 | 80-95 | 5-8 | Mage | VeryFast | — | — |
| `GroveWebspinner` | a grove webspinner | Giant Spider | mossy 0x851 | 2 | 85-100 | 5-8 | Melee | Medium | — | HitPoison Lesser |
| `GroveThornspider` | a thornback spider | Giant Spider | forest green 0x844 | 3 | 130-155 | 7-11 | Melee | Medium | — | HitPoison Regular |
| `GroveStinger` | a bracken stinger | Scorpion | mossy 0x851 | 3 | 130-155 | 7-11 | Melee | Slow | — | HitPoison Regular |
| `GroveViper` | a bracken viper | Snake | mossy 0x851 | 2 | 80-95 | 5-8 | Melee | Fast | — | HitPoison Regular |
| `GroveAdder` | a green adder | Snake | forest green 0x844 | 3 | 130-150 | 7-10 | Melee | Fast | — | HitPoison Greater |
| `GroveThornvine` | a thorn creeper | Corpser | forest green 0x851 | 3 | 135-160 | 7-11 | Melee | Slow | — | — |
| `GroveSaplingreaper` | a sapling reaper | Reaper | forest green 0x851 | 3 | 135-160 | 7-11 | Mage | Slow | — | — |
| `GroveForestrat` | a forest rat | Giant Rat | mossy 0x851 | 2 | 80-90 | 5-7 | Melee | Medium | — | — |

### Themed sub-faction A — the Kōmos revel (6)
*A roving satyr revel-band; the reveler drains, the hornmaster calls in more fauns.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `GroveKomosFaun` | a kōmos faun | Satyr | dusk violet 0x491 | 2 | 85-100 | 5-8 | Melee | Medium | — | — |
| `GroveKomosDancer` | a kōmos dancer | Satyr | forest green 0x851 | 3 | 130-155 | 7-11 | Melee | Fast | — | — |
| `GroveKomosBacchant` | a kōmos bacchant | Satyr | dusk violet 0x491 | 3 | 135-160 | 7-11 | Melee | Medium | — | — |
| `GroveKomosGoatling` | a kōmos goatling | Goat | mossy 0x851 | 2 | 80-95 | 5-8 | Melee | Fast | — | — |
| `GroveKomosReveler` | a kōmos reveler | Satyr | dusk violet 0x491 | 3 | 135-160 | 7-11 | Melee | Medium | — | **Wine-Madness** — OnGaveMelee 20%: 8 stam drain + msg |
| `GroveKomosHornmaster` | the kōmos hornmaster | Satyr | dusk violet 0x491 | 3 | 150-160 | 8-11 | Melee | Fast | — | **Reveler's Horn** — OnGotMelee 15%: `TrySpawnAdd` 1 `GroveKomosFaun` (cap 2, dispel-vulnerable) |

### Themed sub-faction B — the Bramble Court (6)
*A thorn-fey court of dryads and treant-kin; the queen ensnares.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `GroveBrambleNymph` | a bramble nymph | Pixie | dusk violet 0x491 | 3 | 125-150 | 7-10 | Mage | Fast | — | — |
| `GroveBrambleThornling` | a thornling | Pixie | mossy 0x851 | 2 | 80-95 | 5-8 | Mage | VeryFast | — | — |
| `GroveBrambleHound` | a bramble hound | Timber Wolf | dusk violet 0x491 | 3 | 130-155 | 7-11 | Melee | Fast | — | pack instinct |
| `GroveBrambleTreant` | a bramble treant | Reaper | forest green 0x851 | 3 | 150-160 | 7-11 | Mage | Slow | — | — |
| `GroveBrambleWarden` | a bramble warden | Corpser | dusk violet 0x491 | 3 | 150-160 | 7-11 | Melee | Slow | — | HitPoison Regular |
| `GroveBrambleQueen` | the bramble queen | Pixie | dusk violet 0x491 | 3 | 155-160 | 8-11 | Mage | Fast | — | **Thornsnare** — OnGaveMelee 20%: 8 stam drain + msg |

### Ambient (4)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `GroveThrush` | a grove thrush | Chicken | forest green 0x844 | 2 | 80-90 | 4-7 | Melee | Fast | — | — |
| `GroveLunamoth` | a luna moth | Mongbat | dusk violet 0x491 | 2 | 80-90 | 4-7 | Melee | Fast | — | — |
| `GroveHare` | a grove hare | Rabbit | mossy 0x851 | 2 | 80-88 | 4-6 | Melee | VeryFast | — | — |
| `GroveFinch` | a grove finch | Chicken | forest green 0x851 | 2 | 80-88 | 4-6 | Melee | VeryFast | — | — |

### Mini-boss (1)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability | Location | Respawn |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `GroveSilenos` | Silenos, the Reveler-King | Satyr | dusk violet 0x491 | 3 | 155-160 | 8-11 | Melee | Fast | — | **Maddening Pipes** — OnGaveMelee 25%: 12 stam drain + knockback msg | the piping-glade NW of Britain [1666,1290] | 30-60 min |

Custom (new): 4 — Reveler, Hornmaster, Bramble Queen, Silenos. Family total (with existing Piper): **5**.

---

## 2E. Peaks — Expansion (+45) · `Peak*` (L4-5)

*The high crags, crowded: giant-kin, mountain fauna, rocs and stone-eyes — plus two camps: the
Talos-kin **Bronze Watch** and the cyclops clan, the **Get of Polyphemos**.*

### Core expansion — giant-kin, fauna, rocs, gazers & stone (28)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PeakEttin` | a crag ettin | Ettin | slate 0x455 | 4 | 200-238 | 12-16 | Melee | Medium | — | — |
| `PeakStoneEttin` | a stone-hide ettin | Ettin | granite 0x455 | 5 | 320-370 | 13-18 | Melee | Slow | — | — |
| `PeakTwoheadEttin` | a two-head crag ettin | Ettin | slate grey 0x492 | 5 | 330-375 | 14-18 | Melee | Slow | — | — |
| `PeakOgre` | a mountain ogre | Ogre | granite 0x455 | 4 | 210-238 | 12-16 | Melee | Slow | — | — |
| `PeakOgreBrute` | an ogre brute | Ogre | slate 0x455 | 5 | 330-380 | 14-19 | Melee | Slow | — | — |
| `PeakOgrelord` | a crag ogre lord | Ogre Lord | granite 0x455 | 5 | 340-380 | 14-19 | Melee | Slow | BleedImmune | — |
| `PeakCyclopsYoung` | a young cyclops | Cyclops | slate 0x455 | 4 | 210-238 | 12-16 | Melee | Medium | — | — |
| `PeakOldCyclops` | an old cyclops | Cyclops | slate 0x455 | 5 | 330-380 | 14-19 | Melee | Medium | BleedImmune | — |
| `PeakBear` | a mountain bear | Grizzly Bear | slate 0x455 | 4 | 200-235 | 11-15 | Melee | Fast | — | — |
| `PeakBoulderbear` | a boulder bear | Grizzly Bear | granite 0x455 | 5 | 320-360 | 13-18 | Melee | Medium | — | — |
| `PeakFrostbear` | a frost bear | Polar Bear | slate grey 0x492 | 5 | 320-360 | 13-18 | Melee | Medium | — | — |
| `PeakRam` | a horned ram | Mountain Goat | slate 0x455 | 4 | 190-225 | 11-15 | Melee | Fast | — | — |
| `PeakLion` | a mountain lion | Cougar | slate 0x455 | 4 | 195-230 | 11-15 | Melee | VeryFast | — | — |
| `PeakSnowcat` | a snow leopard | Snow Leopard | slate grey 0x492 | 4 | 195-230 | 11-15 | Melee | VeryFast | — | — |
| `PeakCragwolf` | a crag wolf | Grey Wolf | slate grey 0x492 | 4 | 190-225 | 11-15 | Melee | Fast | — | pack instinct |
| `PeakSnowwolf` | a snow wolf | White Wolf | slate grey 0x492 | 5 | 320-355 | 13-18 | Melee | Fast | — | pack instinct |
| `PeakRocFledgling` | a roc fledgling | Harpy | slate grey 0x492 | 4 | 200-235 | 12-16 | Melee | Fast | — | — |
| `PeakStormroc` | a squall roc | Harpy | storm grey 0x492 | 5 | 320-360 | 13-18 | Melee | Fast | — | — |
| `PeakGriffonharpy` | a crag griffon | Harpy | granite 0x455 | 5 | 330-360 | 13-18 | Melee | Fast | — | — |
| `PeakGazerling` | a crag gazer larva | Gazer Larva | slate 0x455 | 4 | 190-225 | 11-15 | Mage | Slow | — | — |
| `PeakStonegazer` | a crag gazer | Gazer | granite 0x455 | 5 | 320-370 | 13-18 | Mage | Slow | — | — |
| `PeakElderGazer` | an elder crag gazer | Elder Gazer | granite 0x455 | 5 | 340-380 | 14-19 | Mage | Slow | — | — |
| `PeakCliffgargoyle` | a cliff gargoyle | Gargoyle | slate grey 0x492 | 5 | 320-360 | 13-18 | Mage | Fast | — | — |
| `PeakStoneling` | a stone elemental | Earth Elemental | granite 0x455 | 4 | 210-238 | 12-16 | Melee | Slow | BleedImmune | — |
| `PeakGraniteGolem` | a granite golem | Golem | granite 0x455 | 5 | 330-380 | 13-18 | Melee | Slow | Bleed + Poison immune | — |
| `PeakRockhound` | a rock hound | Hell Hound | slate 0x455 | 4 | 195-230 | 11-15 | Melee | Fast | — | pack instinct |
| `PeakStonetroll` | a stone troll | Frost Troll | granite 0x455 | 4 | 210-238 | 12-16 | Melee | Medium | — | — |
| `PeakCragtroll` | a crag troll | Frost Troll | slate 0x455 | 5 | 330-375 | 14-18 | Melee | Medium | — | — |

### Themed sub-faction A — the Bronze Watch (6)
*Talos-kin bronze automatons; the warden reflects, the captain hammers.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PeakBronzeWatchman` | a bronze watchman | Golem | bronze 0x798 | 4 | 210-238 | 12-16 | Melee | Slow | Bleed + Poison immune | — |
| `PeakBronzeSentry` | a bronze sentry | Stone Gargoyle | bronze 0x798 | 5 | 330-380 | 13-18 | Melee | Medium | Bleed + Poison immune | — |
| `PeakBronzeArcher` | a bronze archer | Gargoyle | bronze 0x798 | 5 | 320-360 | 13-18 | Archer | Medium | — | — |
| `PeakBronzeMastiff` | a bronze mastiff | Hell Hound | bronze 0x798 | 4 | 200-235 | 11-15 | Melee | Fast | — | pack instinct |
| `PeakBronzeWarden` | a bronze warden | Golem | bronze 0x798 | 5 | 340-380 | 14-19 | Melee | Slow | Bleed + Poison immune | **Molten Riposte** — OnGotMelee 20%: reflect 25% of the blow |
| `PeakBronzeCaptain` | the bronze watch-captain | Ogre Lord | bronze 0x798 | 5 | 350-380 | 15-19 | Melee | Slow | BleedImmune | **Hammerfall** — OnGaveMelee 25%: 15 stam drain + knockback msg |

### Themed sub-faction B — the Get of Polyphemos (6)
*A cyclops herding-clan; the chieftain hurls stone.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PeakGetShepherd` | a cyclops shepherd | Ettin | slate grey 0x492 | 4 | 210-238 | 12-16 | Melee | Slow | — | — |
| `PeakGetHerdsman` | a cyclops herdsman | Cyclops | slate grey 0x492 | 5 | 330-370 | 14-18 | Melee | Medium | BleedImmune | — |
| `PeakGetBoulderthrow` | a boulder-thrower | Ettin | granite 0x455 | 5 | 330-375 | 14-18 | Archer | Slow | — | — |
| `PeakGetRam` | the clan's great ram | Mountain Goat | granite 0x455 | 5 | 320-355 | 13-18 | Melee | Fast | — | — |
| `PeakGetElder` | a cyclops elder | Cyclops | granite 0x455 | 5 | 340-380 | 14-19 | Melee | Medium | BleedImmune | — |
| `PeakGetChieftain` | the Get chieftain | Cyclops | bronze 0x798 | 5 | 355-380 | 15-19 | Melee | Medium | BleedImmune | **Stone Hurl** — OnGaveMelee 25%: knockback msg + 12 stam drain |

### Ambient (4)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PeakCondor` | a mountain condor | Eagle | slate grey 0x492 | 4 | 190-210 | 10-14 | Melee | VeryFast | — | — |
| `PeakEaglet` | a crag eaglet | Eagle | granite 0x455 | 4 | 190-210 | 10-14 | Melee | VeryFast | — | — |
| `PeakPika` | a crag pika | Giant Rat | slate 0x455 | 4 | 190-210 | 10-14 | Melee | Medium | — | — |
| `PeakWildgoat` | a wild mountain goat | Mountain Goat | slate 0x455 | 4 | 190-210 | 10-14 | Melee | Fast | — | — |

### Mini-boss (1)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability | Location | Respawn |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `PeakTalos` | Talos, the Bronze Warden | Cyclops | bronze 0x798 | 5 | 375-380 | 15-20 | Melee | Slow | Bleed + Poison immune | **Bronze Fist** — OnGaveMelee 25%: 15 stam drain + knockback msg | the bronze cairn, W-of-Britain mts [1474,1297] | 30-60 min |

Custom (new): 4 — Bronze Warden, Bronze Captain, Get Chieftain, Talos. Family total (with existing Thunderroc): **5**.
*Name note:* `Talos` is also a legendary armor display name in `dev-docs/itemization`; sharing a
proper noun between a legendary item and a beast is intended flavor (the `WyldNemean`/`LaborNemeanLion`
precedent). `PeakTalos` is collision-free as a **class** (grep-verified). `Argos`/`Argus` was
**rejected** — already taken by the Argus dungeon roster.

---

## 3E. Mire — Expansion (+45) · `Mire*` (L5-6)

*The Hydra's fen, seething: reptiles, amphibians, bog horrors, poison elementals and vermin —
plus two camps: the human **Cult of Lerna** and the lizardman **Scaled Host**.*

### Core expansion — reptiles, amphibians, horrors, elementals & vermin (28)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MireCroc` | a fen crocodile | Alligator | murk 0x844 | 5 | 300-350 | 12-17 | Melee | Medium | — | — |
| `MireGreatgator` | a great mire gator | Alligator | swamp green 0x851 | 6 | 470-520 | 15-20 | Melee | Medium | — | — |
| `MireBlackgator` | a black-scale gator | Alligator | murk 0x844 | 6 | 460-510 | 15-20 | Melee | Medium | — | — |
| `MireLizardman` | a fen lizardman | Lizardman | murk 0x844 | 5 | 290-330 | 11-16 | Melee | Fast | — | — |
| `MireLizardBrute` | a fen lizard brute | Lizardman | swamp green 0x851 | 6 | 470-520 | 15-20 | Melee | Medium | — | — |
| `MireBogserpent` | a bog serpent | Giant Serpent | swamp green 0x851 | 5 | 310-360 | 13-18 | Melee | Medium | PoisonImmune | HitPoison Greater |
| `MireCoilserpent` | a coil serpent | Giant Serpent | venom violet 0x491 | 6 | 480-540 | 15-20 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `MireAdder` | a marsh adder | Snake | sickly green 0x851 | 5 | 290-330 | 11-16 | Melee | Fast | — | HitPoison Greater |
| `MireBlacksnake` | a black fen snake | Snake | murk 0x844 | 5 | 290-330 | 11-16 | Melee | VeryFast | — | HitPoison Greater |
| `MireToadspawn` | a warty toad | Giant Toad | sickly green 0x851 | 5 | 290-340 | 11-16 | Melee | Slow | PoisonImmune | HitPoison Regular |
| `MireBloattoad` | a bloated toad | Giant Toad | murk 0x844 | 6 | 460-510 | 15-20 | Melee | Slow | PoisonImmune | HitPoison Greater |
| `MireFenhorror` | a fen horror | Bog Thing | murk 0x844 | 6 | 470-520 | 15-20 | Melee | Slow | PoisonImmune | — |
| `MireBoghulk` | a bog hulk | Bog Thing | swamp green 0x851 | 6 | 480-540 | 15-20 | Melee | Slow | PoisonImmune | — |
| `MireMuckElemental` | a muck elemental | Water Elemental | murk 0x844 | 5 | 320-370 | 12-17 | Melee | Slow | PoisonImmune | — |
| `MireOozeElemental` | a poison ooze | Poison Elemental | sickly green 0x851 | 6 | 480-540 | 15-20 | Mage | Slow | PoisonImmune | — |
| `MireAcidThing` | an acid elemental | Acid Elemental | sickly green 0x851 | 6 | 470-520 | 15-20 | Melee | Slow | PoisonImmune | — |
| `MireSlimer` | a mire slime | Slime | sickly green 0x851 | 5 | 290-330 | 11-16 | Melee | Slow | PoisonImmune | HitPoison Regular |
| `MireLeech` | a giant bog leech | Slime | murk 0x844 | 5 | 290-330 | 11-16 | Melee | Slow | PoisonImmune | HitPoison Greater |
| `MireStranglevine` | a strangle-vine | Corpser | swamp green 0x851 | 6 | 460-510 | 15-20 | Melee | Slow | — | HitPoison Greater |
| `MireBogreaper` | a bog reaper | Reaper | swamp green 0x851 | 6 | 480-540 | 15-20 | Mage | Slow | PoisonImmune | HasBreath Poison ("Pollen Breath") |
| `MireStinger` | a marsh stinger | Scorpion | sickly green 0x851 | 5 | 290-330 | 11-16 | Melee | Slow | — | HitPoison Deadly |
| `MireSwampspider` | a swamp spider | Giant Spider | murk 0x844 | 5 | 290-330 | 11-16 | Melee | Medium | — | HitPoison Greater |
| `MireWidow` | a fen widow | Giant Black Widow | murk 0x844 | 6 | 470-510 | 15-20 | Melee | Medium | — | HitPoison Deadly |
| `MireRatman` | a fen ratman | Ratman | murk 0x844 | 5 | 290-330 | 11-16 | Melee | Fast | — | — |
| `MireRatmage` | a fen ratman shaman | Ratman Mage | swamp green 0x851 | 5 | 290-330 | 11-16 | Mage | Fast | — | — |
| `MireDrowned` | a fen-drowned corpse | Rotting Corpse | murk 0x844 | 6 | 460-510 | 15-20 | Melee | Slow | Bleed + Poison immune | HitPoison Greater |
| `MireHornbeast` | a fen gaman | Gaman | murk 0x844 | 5 | 300-350 | 12-17 | Melee | Medium | — | — |
| `MireBloodfly` | a fen bloodfly | Mongbat | sickly green 0x851 | 5 | 290-320 | 11-15 | Melee | Fast | — | — |

### Themed sub-faction A — the Cult of Lerna (6)
*Hydra-worshippers and their brood; the hierophant drains.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MireLernaThrall` | a Lerna thrall | Zombie | swamp green 0x851 | 5 | 300-350 | 11-16 | Melee | Slow | PoisonImmune | — |
| `MireLernaCultist` | a Lerna cultist | Evil Mage | swamp green 0x851 | 5 | 290-330 | 11-16 | Mage | Medium | — | — |
| `MireLernaZealot` | a Lerna zealot | Evil Mage | venom violet 0x491 | 6 | 470-520 | 15-20 | Mage | Medium | PoisonImmune | HitPoison Deadly |
| `MireLernaBrute` | a Lerna enforcer | Lizardman | venom violet 0x491 | 6 | 480-520 | 15-20 | Melee | Medium | — | — |
| `MireLernaServant` | a Lerna hydra-servant | Giant Serpent | venom violet 0x491 | 6 | 480-540 | 15-20 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `MireLernaHierophant` | the Lerna hierophant | Evil Mage Lord | venom violet 0x491 | 6 | 490-540 | 15-20 | Mage | Medium | PoisonImmune | **Venom Rite** — OnGaveMelee 20%: 12 mana drain + msg |

### Themed sub-faction B — the Scaled Host (6)
*A lizardman war-tribe of the deep fen; the war-king rallies its hunters.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MireScaledHunter` | a scaled hunter | Lizardman | swamp green 0x851 | 5 | 290-330 | 11-16 | Melee | Fast | — | — |
| `MireScaledSpear` | a scaled spearman | Lizardman | murk 0x844 | 5 | 300-350 | 12-17 | Melee | Medium | — | — |
| `MireScaledArcher` | a scaled archer | Lizardman | sickly green 0x851 | 5 | 290-330 | 11-16 | Archer | Medium | — | — |
| `MireScaledShaman` | a scaled shaman | Lizardman | venom violet 0x491 | 6 | 470-520 | 15-20 | Mage | Medium | PoisonImmune | — |
| `MireScaledBrute` | a scaled brute | Lizardman | venom violet 0x491 | 6 | 480-520 | 15-20 | Melee | Medium | — | — |
| `MireScaledKing` | the scaled war-king | Lizardman | venom violet 0x491 | 6 | 490-540 | 15-20 | Melee | Medium | — | **Scaled Fury** — OnGotMelee 15%: `TrySpawnAdd` 1 `MireScaledHunter` (cap 2, dispel-vulnerable) |

### Ambient (4)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MireHeron` | a marsh heron | Crane | murk 0x844 | 5 | 290-320 | 10-14 | Melee | Fast | — | — |
| `MireBullfrog` | a mire bullfrog | Bullfrog | sickly green 0x851 | 5 | 290-320 | 10-14 | Melee | Slow | PoisonImmune | — |
| `MireDragonfly` | a bog dragonfly | Mongbat | swamp green 0x851 | 5 | 290-320 | 10-14 | Melee | Fast | — | — |
| `MireMudrat` | a mud rat | Giant Rat | murk 0x844 | 5 | 290-320 | 10-14 | Melee | Medium | — | — |

### Mini-boss (1)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability | Location | Respawn |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `MireFenmother` | the Fen-Mother | Giant Serpent | venom violet 0x491 | 6 | 540-550 | 16-20 | Melee | Medium | PoisonImmune | **Sever & Split** — OnGotMelee 15%: `TrySpawnAdd` 1 `Snake` (cap 3, dispel-vulnerable); HitPoison Deadly | the southern fen [2963,3617] | 30-60 min |

Custom (new): 3 — Hierophant, Scaled War-King, Fen-Mother. Family total (with existing Hydraspawn): **4**.

---

## 4E. Restless — Expansion (+45) · `Restless*` (L2-3)

*The unburied dead, in force: skeletons, corpses, ghouls, spectres and grave vermin — plus two
camps: the lost **Mourner's Cortège** and the armored **Barrow Legion**.*

### Core expansion — bones, corpses, ghouls, spectres & vermin (28)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RestlessBones` | a heap of bones | Skeleton | bone 0x835 | 2 | 80-100 | 5-8 | Melee | Medium | — | — |
| `RestlessRattler` | a rattling skeleton | Skeleton | grave teal 0x847 | 2 | 80-100 | 5-8 | Melee | Fast | — | — |
| `RestlessLegionnaire` | a fallen legionnaire | Skeleton | grave violet 0x454 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `RestlessBonewalker` | a shambling bone-walker | Skeleton | grave teal 0x847 | 3 | 130-155 | 7-11 | Melee | Slow | — | — |
| `RestlessBoneguard` | a bone guard | Skeletal Knight | bone 0x835 | 3 | 145-160 | 7-11 | Melee | Medium | — | — |
| `RestlessBonearcher` | a bone archer | Skeleton | bone 0x835 | 3 | 130-155 | 7-11 | Archer | Medium | — | — |
| `RestlessBonemage` | a bone conjurer | Skeletal Mage | grave violet 0x454 | 3 | 130-155 | 7-11 | Mage | Medium | — | — |
| `RestlessGravebound` | a grave-bound warrior | Bone Knight | grave violet 0x454 | 3 | 150-160 | 7-11 | Melee | Medium | BleedImmune | — |
| `RestlessRotling` | a rotting corpse | Zombie | grave teal 0x847 | 2 | 85-100 | 5-8 | Melee | Slow | BleedImmune | — |
| `RestlessHusk` | a withered husk | Zombie | bone 0x835 | 2 | 85-100 | 5-8 | Melee | Slow | BleedImmune | — |
| `RestlessGravedigger` | an unburied gravedigger | Zombie | pallid 0x481 | 3 | 130-155 | 7-11 | Melee | Slow | BleedImmune | — |
| `RestlessCadaver` | a bloated cadaver | Rotting Corpse | grave teal 0x847 | 3 | 140-160 | 7-11 | Melee | Slow | BleedImmune | — |
| `RestlessGnawer` | a gnawing ghoul | Ghoul | pallid 0x481 | 2 | 85-100 | 5-8 | Melee | Fast | — | — |
| `RestlessGhast` | a graveyard ghast | Ghoul | grave violet 0x454 | 3 | 130-155 | 7-11 | Melee | Fast | — | — |
| `RestlessFeaster` | a corpse feaster | Ghoul | grave teal 0x847 | 3 | 135-155 | 7-11 | Melee | Fast | — | — |
| `RestlessSpecter` | a pale specter | Spectre | void grey 0x455 | 3 | 125-150 | 7-10 | Mage | Medium | — | — |
| `RestlessPhantom` | a grave phantom | Shade | void grey 0x455 | 3 | 125-150 | 7-10 | Mage | Fast | — | — |
| `RestlessBogle` | a mournful bogle | Bogle | grave violet 0x454 | 3 | 125-150 | 7-10 | Mage | Medium | — | — |
| `RestlessGloom` | a graveyard gloom | Spectre | grave teal 0x847 | 2 | 80-95 | 5-8 | Mage | Medium | — | — |
| `RestlessWailer` | a wailing wraith | Wraith | void grey 0x455 | 3 | 130-155 | 7-11 | Mage | Fast | — | — |
| `RestlessBanshee` | a keening banshee | Wraith | grave violet 0x454 | 3 | 135-160 | 7-11 | Mage | Fast | — | — |
| `RestlessMummy` | a bound mummy | Mummy | pallid 0x481 | 3 | 145-160 | 7-11 | Melee | Slow | Bleed + Poison immune | — |
| `RestlessBoundone` | a rag-bound dead | Mummy | grave teal 0x847 | 3 | 145-160 | 7-11 | Melee | Slow | Bleed + Poison immune | — |
| `RestlessHeadless` | a headless dead | Headless One | pallid 0x481 | 2 | 85-100 | 5-8 | Melee | Fast | — | — |
| `RestlessGraverat` | a grave rat | Giant Rat | grave teal 0x847 | 2 | 80-95 | 5-8 | Melee | Medium | — | — |
| `RestlessBonerat` | a bone-gnawing rat | Giant Rat | bone 0x835 | 2 | 80-95 | 5-8 | Melee | Medium | — | — |
| `RestlessCarrionbat` | a carrion bat | Mongbat | void grey 0x455 | 2 | 80-95 | 5-8 | Melee | Fast | — | — |
| `RestlessVampirebat` | a crypt vampire bat | Vampire Bat | grave violet 0x454 | 3 | 130-150 | 7-10 | Melee | Fast | — | — |

### Themed sub-faction A — the Mourner's Cortège (6)
*A lost funeral procession; the sexton chills, the psychopomp raises the dead.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RestlessMourner` | a veiled mourner | Spectre | grave violet 0x454 | 3 | 130-155 | 7-11 | Mage | Medium | — | — |
| `RestlessPallbearer` | a spectral pallbearer | Zombie | void grey 0x455 | 3 | 140-160 | 7-11 | Melee | Slow | BleedImmune | — |
| `RestlessKeener` | a keening widow | Spectre | pallid 0x481 | 3 | 130-155 | 7-11 | Mage | Fast | — | — |
| `RestlessBellringer` | a corpse bellringer | Skeleton | grave teal 0x847 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `RestlessSexton` | the cortège sexton | Skeleton | grave violet 0x454 | 3 | 150-160 | 7-11 | Mage | Medium | — | **Grave Chill** — OnGaveMelee 20%: 8 mana drain + msg |
| `RestlessPsychopomp` | the mourners' psychopomp | Wraith | void grey 0x455 | 3 | 150-160 | 8-11 | Mage | Fast | — | **Lantern of the Dead** — OnGotMelee 15%: `TrySpawnAdd` 1 `RestlessGnawer` (cap 2, dispel-vulnerable) |

### Themed sub-faction B — the Barrow Legion (6)
*Armored dead soldiers still in formation; the marshal rallies the ranks.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RestlessLegionSoldier` | a barrow legionary | Skeleton | grave violet 0x454 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `RestlessLegionKnight` | a barrow knight | Skeletal Knight | grave violet 0x454 | 3 | 150-160 | 7-11 | Melee | Medium | — | — |
| `RestlessLegionPikeman` | a barrow pikeman | Bone Knight | pallid 0x481 | 3 | 150-160 | 7-11 | Melee | Medium | BleedImmune | — |
| `RestlessLegionArcher` | a barrow archer | Skeleton | grave teal 0x847 | 3 | 130-155 | 7-11 | Archer | Medium | — | — |
| `RestlessLegionStandard` | a barrow standard-bearer | Skeletal Knight | grave teal 0x847 | 3 | 150-160 | 7-11 | Melee | Medium | — | — |
| `RestlessLegionMarshal` | the barrow marshal | Bone Knight | grave violet 0x454 | 3 | 155-160 | 8-11 | Melee | Medium | BleedImmune | **Rally the Fallen** — OnGotMelee 15%: `TrySpawnAdd` 1 `RestlessLegionSoldier` (cap 2, dispel-vulnerable) |

### Ambient (4)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RestlessCrow` | a graveyard crow | Eagle | void grey 0x455 | 2 | 80-90 | 4-7 | Melee | VeryFast | — | — |
| `RestlessGravemoth` | a grave moth | Mongbat | grave violet 0x454 | 2 | 80-90 | 4-7 | Melee | Fast | — | — |
| `RestlessCryptbat` | a crypt bat | Mongbat | void grey 0x455 | 2 | 80-90 | 4-7 | Melee | Fast | — | — |
| `RestlessBonefinch` | a graveyard finch | Chicken | bone 0x835 | 2 | 80-88 | 4-6 | Melee | Fast | — | — |

### Mini-boss (1)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability | Location | Respawn |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `RestlessBarrowking` | the Barrow-King | Skeletal Knight | grave violet 0x454 | 3 | 155-160 | 8-11 | Melee | Medium | — | **Call the Unburied** — OnGotMelee 15%: `TrySpawnAdd` 1 `RestlessBones` (cap 3, dispel-vulnerable) | the Cove barrow-ground [2438,1100] | 30-60 min |

Custom (new): 4 — Sexton, Psychopomp, Legion Marshal, Barrow-King. Family total (with existing Wraith): **5**.
*(All class names avoid the stock `RestlessSoul`, as the base doc notes.)*

---

## 5E. Shore — Expansion (+45) · `Shore*` (L3-4)

*The tide-line, crowded: crabs, eels, sirens, tide-things and fish-folk — plus two camps: the
drowned **Wreckers** and the fish-folk **Deep Court**.*

### Core expansion — crustaceans, eels, sirens, tide-things & fish-folk (28)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ShoreRockcrab` | a rock crab | Scorpion | tide blue 0x530 | 3 | 130-155 | 7-11 | Melee | Slow | — | — |
| `ShoreKingcrab` | a king crab | Scorpion | reef green 0x851 | 4 | 205-238 | 12-16 | Melee | Slow | — | — |
| `ShoreSpinecrab` | a spine crab | Scorpion | tide blue 0x530 | 4 | 205-238 | 12-16 | Melee | Slow | — | HitPoison Regular |
| `ShoreSandcrawler` | a sand crawler | Scorpion | pale aqua 0x481 | 3 | 125-150 | 7-10 | Melee | Slow | — | — |
| `ShoreHermit` | a giant hermit crab | Scorpion | reef green 0x851 | 3 | 130-155 | 7-11 | Melee | Slow | — | — |
| `ShoreBarnacleback` | a barnacle-back crab | Scorpion | reef green 0x851 | 4 | 210-238 | 12-16 | Melee | Slow | BleedImmune | — |
| `ShoreMoray` | a moray eel | Snake | pale aqua 0x481 | 4 | 205-238 | 12-16 | Melee | VeryFast | — | HitPoison Greater |
| `ShoreReefviper` | a reef viper | Snake | reef green 0x851 | 4 | 200-235 | 11-16 | Melee | Fast | — | HitPoison Greater |
| `ShoreSpineeel` | a spined eel | Snake | tide blue 0x530 | 3 | 125-150 | 7-10 | Melee | VeryFast | — | HitPoison Regular |
| `ShoreSeaSerpent` | a coastal serpent | Giant Serpent | tide blue 0x530 | 4 | 210-238 | 12-16 | Melee | Medium | — | — |
| `ShoreBrineserpent` | a brine serpent | Giant Serpent | reef green 0x851 | 4 | 210-238 | 12-16 | Melee | Medium | — | HitPoison Greater |
| `ShoreHarpy` | a shore harpy | Harpy | siren teal 0x481 | 3 | 130-155 | 7-11 | Melee | Fast | — | — |
| `ShoreShrieker` | a shrieking siren | Harpy | siren violet 0x491 | 4 | 205-238 | 12-16 | Melee | Fast | — | — |
| `ShoreSeaharpy` | a sea harpy | Harpy | tide blue 0x530 | 4 | 200-235 | 11-16 | Melee | Fast | — | — |
| `ShoreBrinespawn` | a brine spawn | Water Elemental | tide blue 0x530 | 3 | 130-155 | 7-11 | Melee | Slow | PoisonImmune | — |
| `ShoreTidal` | a tidal elemental | Water Elemental | tide blue 0x530 | 4 | 210-238 | 11-16 | Melee | Slow | PoisonImmune | — |
| `ShoreCoralthing` | a coral horror | Water Elemental | reef green 0x851 | 4 | 210-238 | 11-16 | Melee | Slow | PoisonImmune | — |
| `ShoreSeaslime` | a tidal slime | Slime | reef green 0x851 | 3 | 125-150 | 7-10 | Melee | Slow | PoisonImmune | HitPoison Regular |
| `ShoreUrchin` | a giant sea urchin | Slime | reef green 0x851 | 3 | 125-150 | 7-10 | Melee | Slow | PoisonImmune | HitPoison Greater |
| `ShoreReefstalker` | a reef stalker | Lizardman | reef green 0x851 | 3 | 130-155 | 7-11 | Melee | Fast | — | — |
| `ShoreDeepone` | a deep-one raider | Lizardman | tide blue 0x530 | 4 | 205-238 | 12-16 | Melee | Fast | — | — |
| `ShoreReefspear` | a reef spearman | Lizardman | pale aqua 0x481 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `ShoreSeal` | a bull seal | Walrus | tide blue 0x530 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `ShoreSeacow` | a lumbering sea-cow | Walrus | pale aqua 0x481 | 4 | 205-235 | 11-15 | Melee | Slow | — | — |
| `ShoreGull` | a great gull | Mongbat | pale aqua 0x481 | 3 | 125-150 | 7-10 | Melee | Fast | — | — |
| `ShoreShorerat` | a shore rat | Giant Rat | tide blue 0x530 | 3 | 125-150 | 7-10 | Melee | Medium | — | — |
| `ShoreSeahag` | a sea hag | Evil Mage | siren teal 0x481 | 4 | 205-235 | 11-16 | Mage | Medium | — | — |
| `ShoreTidewitch` | a tide witch | Evil Mage | siren violet 0x491 | 4 | 205-235 | 11-16 | Mage | Medium | — | — |

### Themed sub-faction A — the Wreckers (6)
*A drowned wreck-cult; the wreck-witch lures, the wreck-captain hooks.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ShoreWrecker` | a drowned wrecker | Zombie | tide blue 0x530 | 4 | 205-238 | 12-16 | Melee | Slow | BleedImmune | — |
| `ShoreDrownedSailor` | a drowned sailor | Skeleton | pale aqua 0x481 | 3 | 130-155 | 7-11 | Melee | Medium | — | — |
| `ShoreReaver` | a wreck reaver | Ghoul | tide blue 0x530 | 4 | 205-238 | 12-16 | Melee | Fast | — | — |
| `ShoreLampbearer` | a false-light lurer | Zombie | pale aqua 0x481 | 3 | 130-155 | 7-11 | Melee | Slow | BleedImmune | — |
| `ShoreWreckWitch` | a wreck-witch | Evil Mage | siren violet 0x491 | 4 | 205-238 | 12-16 | Mage | Medium | — | **False Beacon** — OnGaveMelee 20%: 10 stam drain + msg |
| `ShoreWreckLord` | the wreck-captain | Skeleton | siren violet 0x491 | 4 | 230-238 | 12-16 | Melee | Medium | BleedImmune | **Wrecker's Hook** — OnGotMelee 15%: knockback msg + 10 stam drain |

### Themed sub-faction B — the Deep Court (6)
*A nereid-and-fish-folk court of the offshore reef; the prince drags foes under.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ShoreDeepNereid` | a deep nereid | Harpy | siren teal 0x481 | 4 | 205-235 | 11-16 | Melee | Fast | — | — |
| `ShoreDeepGuard` | a deep-court guard | Lizardman | tide blue 0x530 | 4 | 210-238 | 12-16 | Melee | Medium | — | — |
| `ShoreDeepOracle` | a deep-court oracle | Evil Mage | siren teal 0x481 | 4 | 205-235 | 11-16 | Mage | Medium | — | — |
| `ShoreDeepTideguard` | a tide-guard of the deep | Water Elemental | tide blue 0x530 | 4 | 210-238 | 11-16 | Melee | Slow | PoisonImmune | — |
| `ShoreDeepHound` | a deep-court hound | Hell Hound | tide blue 0x530 | 4 | 200-235 | 11-15 | Melee | Fast | — | pack instinct |
| `ShoreDeepPrince` | the prince of the deep | Lizardman | siren violet 0x491 | 4 | 230-238 | 12-16 | Melee | Medium | — | **Undertow** — OnGaveMelee 20%: 10 stam drain + msg |

### Ambient (4)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ShoreSandpiper` | a sandpiper | Eagle | pale aqua 0x481 | 3 | 125-145 | 7-10 | Melee | VeryFast | — | — |
| `ShoreCormorant` | a cormorant | Eagle | tide blue 0x530 | 3 | 125-145 | 7-10 | Melee | VeryFast | — | — |
| `ShoreSandflea` | a sand flea | Mongbat | tide blue 0x530 | 3 | 125-145 | 7-10 | Melee | Fast | — | — |
| `ShoreFiddler` | a fiddler crab | Scorpion | pale aqua 0x481 | 3 | 125-145 | 7-10 | Melee | Slow | — | — |

### Mini-boss (1)
| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability | Location | Respawn |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `ShoreKarkinos` | the Karkinos | Scorpion | reef green 0x851 | 4 | 235-240 | 13-16 | Melee | Slow | BleedImmune | **Crushing Claw** — OnGaveMelee 25%: 14 stam drain + knockback msg | APPROX — the Trinsic coast reef [1285,3731] (sand tiles at water's edge) | 30-60 min |

Custom (new): 4 — Wreck-Witch, Wreck-Lord, Deep Prince, Karkinos. Family total (with existing Luresiren): **5**.
*(Reef bodies keep the land-walking `Giant Serpent`, not `Sea Serpent`, per the base-doc walk-fix note.)*

---

## Expansion spawn-wiring appendix (real overworld coords)

Additive spawners alongside the existing biome spawners (same `open-world-biomes.json` sibling
pattern). Coords **grep-verified 2026-07-14** against
`Distribution/Data/Spawns/shared/felucca/{Outdoors,WildLife,LostLands,Graveyards}.json` unless
marked. The 45 new members per family mostly **ride the same spawners** as the base six (added to
`entries`); the rows below give **8-10 anchors per family** so the larger roster spreads out.

| Family | Real coords (8-10 anchors) | Source / marker |
|---|---|---|
| **Groves** (forest) | [1666,1290] · [1695,809] · [846,1071] · [1635,3253] · [1387,3109] · [1827,3429] · [2091,3413] · [1595,3045] · [5347,3449] · [5570,3153] | `Outdoors`/`LostLands` — Gorilla/Panther/GreatHart/Bear/Boar clusters (mainland Trinsic-jungle belt + Lost Lands wood) |
| **Peaks** (mountain) | [809,1615] (own `PeakBronzeEttin` spawner) · [1055,1442] · [741,1838] · [1474,1297] · [1749,474] · [5558,824] (Lost Lands crag) · [6119,219] · [5986,203] (Hythloth-approach gazer crags) | `Outdoors` own spawner + base anchors + `LostLands`/dungeon-mouth gazer/ogre spawns |
| **Mire** (swamp) | [1915,2385] · [2079,1018] · [2963,3617] · [2354,3503] · [6052,1463] · [6090,1483] · [6087,1443] · [6108,1471] (Britain-sewer fen gators) | base anchors (mainland fen) + `BritainSewer` gator spawns; mainland gator/serpent overworld was rerouted by the P2 era pass (honest gap) |
| **Restless** (graveyard) | [4545,1317] · [2758,867] · [722,1119] · [2438,1100] · [1369,1475] · [3407,2652] · [1285,3731] | `Graveyards.json` — **already host `Restless*`** (P1 swap); the 45 new members just join the `entries` |
| **Shore** (coast) | APPROX — sand tiles (z ≈ 0) at the water's edge near [1285,3731] Trinsic coast, [1695,809]-area N harbor, Britain harbor, Vesper waterline | no stock beach spawner exists (base-doc caveat stands) — hand-place at sand tiles |

> **Honest gaps (unchanged from the base doc):** (1) **Peak & Mire** mainland Ettin/Ogre/gator
> *overworld* spawners were removed/rerouted by the earlier era-mismatch pass; their extra
> anchors mix base-doc mainland points with real `LostLands`/`BritainSewer` spawns, and the
> family's *own* wired spawners (`open-world-families.json`) remain the real seed points.
> (2) **Shore** still has no stock beach-edge spawner; its coords stay **APPROX** and need manual
> sand-tile placement.

### Mini-boss spawn coords (single-point, `count = 1`, `DungeonElite`, bag = level)

| Mini-boss | Coord | Landmark | Respawn |
|---|---|---|---|
| Silenos, the Reveler-King | [1666,1290] | the piping-glade NW of Britain | 30-60 min |
| Talos, the Bronze Warden | [1474,1297] | the bronze cairn, W-of-Britain mts | 30-60 min |
| the Fen-Mother | [2963,3617] | the southern fen | 30-60 min |
| the Barrow-King | [2438,1100] | the Cove barrow-ground | 30-60 min |
| the Karkinos | [1285,3731] (APPROX) | the Trinsic coast reef | 30-60 min |

---

## Expansion totals appendix

| Family (band) | Core | Themed (2×6) | Ambient | Mini-boss | **New** | Was | **Now** | Custom (new / family total) |
|---|---|---|---|---|---|---|---|---|
| Groves (L2-3) | 28 | 12 | 4 | 1 | **45** | 6 | **51** | 4 / 5 |
| Peaks (L4-5) | 28 | 12 | 4 | 1 | **45** | 6 | **51** | 4 / 5 |
| Mire (L5-6) | 28 | 12 | 4 | 1 | **45** | 6 | **51** | 3 / 4 |
| Restless (L2-3) | 28 | 12 | 4 | 1 | **45** | 6 | **51** | 4 / 5 |
| Shore (L3-4) | 28 | 12 | 4 | 1 | **45** | 6 | **51** | 4 / 5 |
| **Biome families** | **140** | **60** | **20** | **5** | **225** | **30** | **255** | **19 / 24** |

The Labors stay **6** (untouched). Open-world grand total: **255 biome + 6 Labors = 261**.

**Custom-ability tally (19 new, 24 with the base six):** each family carries two themed
lieutenants (a drain proc + a `TrySpawnAdd`/reflect) plus the mini-boss (drain + knockback) —
Mire runs one fewer (its two summon/drain roles collapse into the Hierophant + Scaled King).
Every custom is on the reused toolbox (`OnGaveMeleeAttack` / `OnGotMeleeAttack` / `TrySpawnAdd`),
**no new plumbing**. Each family sits at **4-5 custom, under the ≤8 budget**; ~90% of the 225 new
classes are pure stat blocks. `HasBreath Poison` on `MireBogreaper` is the established one-class
`FireBreath` reskin (`WyldTreant` precedent) and is **free/uncounted**, as are all `HitPoison`,
pack instinct and immunities.

**Loot model (unchanged):** every new biome class is **open-world trash — NO `LootBagLevel`
override**; natural mob-level → bag mapping, normal loot packs, one `LevelConfig.MobLevelOverrides`
pin per class at its **Lvl**. Mini-bosses are `DungeonElite` (`EliteBagLevel = Lvl`,
`EliteBagCount = 1`, over-level clamp on, 30-60 min respawn).

**Donor bodies (all grep-verified present 2026-07-14):** adds, on top of the base-doc set —
Hind, Timber Wolf, Panther, Cougar, Black Bear, Brown Bear, Polar Bear, White Wolf, Snow Leopard,
Gorilla, Pixie, Wisp, Goat, Pig, Chicken, Rabbit, Mongbat, Mountain Goat, Ogre Lord, Gazer,
Gazer Larva, Elder Gazer, Gargoyle, Stone Gargoyle, Golem, Earth Elemental, Frost Troll,
Hell Hound, Eagle, Giant Rat, Slime, Corpser, Reaper, Ratman, Ratman Mage, Rotting Corpse,
Skeleton, Skeletal Knight, Skeletal Mage, Bone Knight, Bogle, Mummy, Shade, Wraith, Headless One,
Vampire Bat, Evil Mage, Evil Mage Lord, Water Elemental, Poison Elemental, Acid Elemental,
Giant Black Widow, Giant Spider, Scorpion, Snake, Giant Serpent, Alligator, Lizardman, Giant Toad,
Bullfrog, Bog Thing, Gaman, Walrus, Crane. No `Roc` class at T2A — rocs reuse `Harpy`.
