# Classic Five — Expanded Bestiary (+45)

> **IMPLEMENTED 2026-07-14** (uncommitted): all 45 classes on disk under
> `Mobiles/Dungeons/ClassicFive/<Dungeon>/`, 5 additive spawn JSONs at real dungeon
> coords, 45 LevelConfig pins, build clean, migrations generated. Integration fixes:
> BrineLeechEel walks on land (SeaSerpent donor was CantWalk); two Tartarus mobs lost
> AOS-era `GetWeaponAbility` overrides copied from ML donors (meaningless on T2A);
> GaianEarthbloodChampion's pack bonus skipped (no fitting PackInstinct enum value —
> plain stat block).

> Design doc, 2026-07-14. Companion to `dev-docs/classic-five-enhancements.md` (the five
> dungeons, bands, existing named elites + Brood skins). This spec adds **+9 new creatures
> per dungeon (45 total)** that spawn **alongside** the stock classic mobs and the already-
> built elites/brood — it does not replace anything.

## How to read this

Each dungeon gets, on top of its existing 1 named elite + 1 Brood serpent:

- **One themed family (~6)** — 2–3 ranks × 2 role lines, mostly pure stat-block reskins.
  Max 2 family members carry a custom-code ability.
- **2 support/ambient** — pure stat blocks, level = band−1.
- **1 mini-boss** — named, `DungeonElite` subclass, guaranteed bag = **band level** (one
  below the existing named elite's bag).

**Hard rules honored** (mirror the ladder): stock T2A body reskins only, donor class
verified present under `Projects/UOContent/Mobiles/`; ≤1 signature + ≤1 passive each;
**max 3 of 9 per dungeon** carry custom code (marked ⚙); levels inside the band (HP
ceilings L3≤160 / L4≤240 / L5≤380 / L6≤550 / L7≤720 / L8≤950); class prefixes greppable;
trash `LootBagLevel = level − 1`, mini-boss `DungeonElite`.

**All new mobs are pinned** via `LevelConfig.MobLevelOverrides` to the **Lvl** shown (HP is
kept in-band anyway so the tag is honest without leaning on the pin). Toolbox reused from
`DungeonAbilities` (`AuraPulse`, `TrySpawnAdd`), `MonsterAbility` breath (see `LadonBreath`),
and the `OnGaveMeleeAttack` / `OnGotMeleeAttack` procs already used by the elites/brood.

Names avoid: the five god-roots (Zephyr, Phobos, Agrotera, Pallas, Stygian), the existing
elites (Enkelados, Aiakos, Thaumas, Ladon, Alastor), the reserved Tholos elite
(Kymopoleia), and every class already under `Mobiles/Dungeons/**`.

---

## Despise — L4 · Gaian* · Gaia's earthborn (the Spartoi)

Cadmus sowed the dragon's teeth and armed men rose from the furrows. Gaia keeps sowing:
clay-skinned Spartoi claw up out of Despise's earthen halls, unfinished and furious.

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| GaianSpartos | a gaian spartos | Lizardman | 0x972 ochre | 3 | 145–155 | 9–14 | Melee | Fast | — | — |
| GaianSpearborn | a gaian spearborn | Lizardman | 0x9C4 clay-red | 4 | 195–205 | 12–17 | Melee | Fast | — | — |
| GaianEarthbloodChampion | a gaian earthblood champion | Ettin | 0x972 ochre | 4 | 228–238 | 14–19 | Melee | Medium | — | ⚙ pack instinct: +25% melee dmg while ≥1 Gaian kin within 3 tiles (passive) |
| GaianClayborn | a gaian clayborn | Ogre | 0x9C2 dun | 3 | 150–160 | 11–16 | Melee | Slow | — | — |
| GaianPhalangite | a gaian phalangite | Ettin | 0x455 granite | 4 | 208–218 | 13–18 | Melee | Medium | — | — |
| GaianTitanWard | a gaian titan-ward | Ogre | 0x455 granite | 4 | 226–236 | 13–18 | Melee | Slow | — | ⚙ stone skin: OnGotMelee 15% reflect ~20% of the blow, no debuff (passive) |
| GaianSownSeed | a sown seed | GiantSerpent | 0x9C2 dun | 3 | 115–125 | 6–11 | Melee | Slow | poison immune (unhatched, inert) | — |
| GaianClayServitor | a clay servitor | Golem | 0x972 ochre | 3 | 145–155 | 8–13 | Melee | Slow | poison + bleed immune | — |
| **Chthonios** | **Chthonios, the First-Sown** | Cyclops | 0x972 ochre | 4 | 230–240 | 14–20 | Melee | Medium | poison immune | ⚙ **Sow the teeth**: OnGotMelee 15% → `TrySpawnAdd` 1–2 `Lizardman` (cap 2). `DungeonElite`, EliteBagLevel **4** |

Family customs: 2 (Champion, Titan-Ward). Dungeon total: 3 ⚙.

---

## Deceit — L5 · Drowned* · the drowned dead (Hades' court)

Deceit's undead are recast as the sea-taken dead of the Greek underworld — sailors and
oathbreakers dragged down and marched back up, brine still pouring from their ribs.

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| DrownedDead | a drowned dead | Zombie | 0x835 drowned teal | 4 | 195–205 | 11–16 | Melee | Slow | poison immune | — |
| DrownedLegionary | a drowned legionary | SkeletalKnight | 0x841 verdigris | 5 | 335–345 | 15–20 | Melee | Medium | poison immune | — |
| DrownedOathbreaker | a drowned oathbreaker | BoneKnight | 0x847 bone-green | 5 | 370–380 | 17–22 | Melee | Medium | poison immune | ⚙ grave hunger: OnGaveMelee 20% self-heal for ~⅓ of dmg dealt (passive) |
| DrownedShade | a drowned shade | Spectre | 0x835 drowned teal | 4 | 205–215 | 10–15 | Mage | Fast | — | — |
| DrownedWailer | a drowned wailer | Wraith | 0x841 verdigris | 5 | 325–335 | 13–18 | Mage | Fast | — | — |
| DrownedLamentor | a drowned lamentor | Bogle | 0x847 bone-green | 5 | 355–365 | 14–19 | Mage | Fast | — | ⚙ ebb away: OnGotMelee 15% blink 4–6 tiles from the attacker (passive) |
| DrownedMournling | a mournling | Ghoul | 0x835 drowned teal | 4 | 185–195 | 9–14 | Melee | Fast | — | — |
| DrownedBrackishHulk | a brackish hulk | RottingCorpse | 0x830 silt | 4 | 215–225 | 10–15 | Melee | Slow | poison immune | — |
| **Minos** | **Minos, the Pale Arbiter** | Lich | 0x835 drowned teal | 5 | 370–380 | 14–19 | Mage | Medium | poison immune | ⚙ **The dead rise**: OnGotMelee 15% → `TrySpawnAdd` 1–2 `Skeleton` (cap 2). `DungeonElite`, EliteBagLevel **5** |

Family customs: 2 (Oathbreaker, Lamentor). Dungeon total: 3 ⚙.

---

## Shame — L6 · Brine* · Poseidon's storm-brood (elementals)

Shame's flooded galleries churn with the Earth-Shaker's get: water and storm given angry
shape. (Names avoid the existing `BrineSerpent` Brood skin.)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| BrineSpume | a brine spume | WaterElemental | 0x4F8 storm-teal | 5 | 295–305 | 13–18 | Mage | Slow | can swim | — |
| BrineSurge | a brine surge | WaterElemental | 0x4F2 deep-teal | 6 | 475–485 | 16–21 | Mage | Medium | can swim | — |
| BrineMaelstrom | a brine maelstrom | IceElemental | 0x480 pale-cyan | 6 | 530–540 | 18–23 | Mage | Medium | cold immune | ⚙ undertow: `AuraPulse` cold, 3 dmg / 2s, adjacent only (passive) |
| BrineGale | a brine gale | AirElemental | 0x4F8 storm-teal | 5 | 305–315 | 12–17 | Mage | Fast | — | — |
| BrineTempest | a brine tempest | AirElemental | 0x481 squall-grey | 6 | 495–505 | 15–20 | Mage | VeryFast | — | — |
| BrineSquallHarrier | a squall-harrier | Harpy | 0x481 squall-grey | 6 | 515–525 | 15–20 | Melee | VeryFast | — | ⚙ gale-slap: OnGotMelee 15% knock the attacker back 2 tiles (blink, passive) |
| BrineSeep | a brine seep | SnowElemental | 0x480 pale-cyan | 5 | 295–305 | 11–16 | Melee | Slow | cold immune | — |
| BrineLeechEel | a leech-eel | SeaSerpent | 0x4F2 deep-teal | 5 | 315–325 | 12–17 | Melee | Slow | can swim | — |
| **Glaukos** | **Glaukos, the Brine-Shepherd** | WaterElemental | 0x4F8 storm-teal | 6 | 535–545 | 16–21 | Mage | Medium | can swim | ⚙ **Call the deep**: OnGotMelee 15% → `TrySpawnAdd` 1–2 `SeaSerpent` (cap 2). `DungeonElite`, EliteBagLevel **6** |

Family customs: 2 (Maelstrom, Squall-Harrier). Dungeon total: 3 ⚙.

---

## Destard — L7 · Drakon* · Ladon's dragon-cult (dragons/drakes)

The sleepless wyrm keeps a cult: half brood-drakes hatched under his coils, half serpent-
men who tend the hoard and burn for him.

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| DrakonWhelp | a drakon whelp | Drake | 0x501 hoard-gold | 6 | 475–485 | 15–20 | Melee | Medium | — | — |
| DrakonWyrmling | a drakon wyrmling | Wyvern | 0x66D crimson | 7 | 635–645 | 18–23 | Melee | Fast | HitPoison (Greater) | — |
| DrakonFlamewing | a drakon flamewing | Drake | 0x489 ember | 7 | 690–700 | 19–24 | Melee | Medium | fire breath | ⚙ weak fire breath (`MonsterAbility`, `FireBreath` at ~0.10 scalar — see `LadonBreath`) |
| DrakonAcolyte | a drakon acolyte | OphidianWarrior | 0x501 hoard-gold | 6 | 485–495 | 14–19 | Melee | Fast | — | — |
| DrakonZealot | a drakon zealot | OphidianKnight | 0x66D crimson | 7 | 655–665 | 17–22 | Melee | Medium | — | — |
| DrakonFlamespeaker | a drakon flamespeaker | OphidianMage | 0x489 ember | 7 | 645–655 | 15–20 | Mage | Medium | — | ⚙ ember-word: OnGaveMelee 20% fire pop, 10–16 dmg (passive) |
| DrakonServpentling | a drakon serpentling | GiantSerpent | 0x66D crimson | 6 | 465–475 | 13–18 | Melee | Fast | HitPoison (Regular) | — |
| DrakonScaleHound | a scale-hound | Raptor | 0x501 hoard-gold | 6 | 495–505 | 14–19 | Melee | VeryFast | — | — |
| **Pythios** | **Pythios, the Cult-Hierarch** | SerpentineDragon | 0x489 ember | 7 | 705–715 | 18–23 | Mage | Medium | — | ⚙ **Rally the brood**: OnGotMelee 15% → `TrySpawnAdd` 1–2 `Drake` (cap 2). `DungeonElite`, EliteBagLevel **7** |

Family customs: 2 (Flamewing, Flamespeaker). Dungeon total: 3 ⚙. (Note: class spelled
`DrakonServpentling` to avoid the near-duplicate `DrakescalePython`/`WyldPython` greps —
rename to `DrakonSerpentling` at impl if preferred; keep the grep unique either way.)

---

## Hythloth — L8 · Tartarus* · the Tartarus daemons (daemons/gargoyles)

The deepest pit. Tartarus's brood — torment-fiends and animate gargoyle-wardens — boil up
into Hythloth's demon sanctum around Alastor.

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| TartarusImp | a tartarus imp | Imp | 0x21 blood-hell | 7 | 635–645 | 16–21 | Mage | VeryFast | — | — |
| TartarusFiend | a tartarus fiend | Daemon | 0x21 blood-hell | 8 | 845–855 | 20–25 | Mage | Medium | — | — |
| TartarusTormentFiend | a torment-fiend | IceFiend | 0x22 ashen-red | 8 | 895–905 | 21–26 | Mage | Medium | — | ⚙ sear: OnGaveMelee 20% fire pop, 15–22 dmg (passive, à la Alastor, weaker) |
| TartarusGargoyle | a tartarus gargoyle | Gargoyle | 0x455 slate | 7 | 645–655 | 15–20 | Mage | Medium | — | — |
| TartarusEnforcer | a tartarus enforcer | GargoyleEnforcer | 0x22 ashen-red | 8 | 815–825 | 18–23 | Mage | Fast | — | — |
| TartarusStonewrath | a stonewrath | StoneGargoyle | 0x455 slate | 8 | 875–885 | 19–24 | Melee | Slow | poison immune | ⚙ stone skin: OnGotMelee 15% reflect ~20% of the blow (passive) |
| TartarusSoulgorger | a soulgorger | GoreFiend | 0x21 blood-hell | 7 | 650–660 | 16–21 | Melee | Fast | poison immune | — |
| TartarusHellbrand | a hellbrand | ChaosDaemon | 0x22 ashen-red | 7 | 635–645 | 15–20 | Melee | Fast | — | — |
| **Eurynomos** | **Eurynomos, the Corpse-Eater** | ArcaneDaemon | 0x21 blood-hell | 8 | 930–940 | 20–26 | Mage | Medium | poison immune | ⚙ **Pit-heat**: `AuraPulse` fire, 5 dmg / 2s, adjacent only. `DungeonElite`, EliteBagLevel **8** |

Family customs: 2 (Torment-Fiend, Stonewrath). Dungeon total: 3 ⚙.

---

## Totals appendix

| Dungeon | Prefix | Band | New mobs | Family | Support | Mini-boss | Custom-ability count (≤3) | Mini-boss bag |
|---|---|---|---|---|---|---|---|---|
| Despise | Gaian* | L4 | 9 | 6 | 2 | Chthonios | 3 | 4 |
| Deceit | Drowned* | L5 | 9 | 6 | 2 | Minos | 3 | 5 |
| Shame | Brine* | L6 | 9 | 6 | 2 | Glaukos | 3 | 6 |
| Destard | Drakon* | L7 | 9 | 6 | 2 | Pythios | 3 | 7 |
| Hythloth | Tartarus* | L8 | 9 | 6 | 2 | Eurynomos | 3 | 8 |
| **Total** | — | — | **45** | **30** | **10** | **5** | **15** | — |

**Ability budget**: 15 custom-code mobs of 45 (3 per dungeon, at the cap). Breakdown per
dungeon — 2 family + 1 mini-boss. 30 family + support members are pure stat-block reskins.

**Reused toolbox only** (no new plumbing): `DungeonAbilities.AuraPulse` (Maelstrom,
Eurynomos), `DungeonAbilities.TrySpawnAdd` (Chthonios, Minos, Glaukos, Pythios),
`MonsterAbility`/`FireBreath` (Flamewing), `OnGaveMeleeAttack` procs (Oathbreaker heal,
Flamespeaker + Torment-Fiend fire pop), `OnGotMeleeAttack` procs (Titan-Ward + Stonewrath
reflect, Lamentor + Squall-Harrier blink), passive pack instinct (Earthblood Champion),
`HitPoison` (Drakon Wyrmling/Serpentling), and stock immunities (golem/undead poison,
elemental cold, corpse bleed).

**Spawn wiring** (impl, out of scope here): additive to
`Distribution/Data/Spawns/uoml/felucca/classic-elites.json` (or a sibling file) at each
dungeon's real coords; mini-boss ~20 min respawn (one tier below the existing elites'
20–30 min). All 45 need `LevelConfig.MobLevelOverrides` pins at the **Lvl** column value.

---

# Expansion to 35 (+24 per dungeon · +120 total)

> Design, 2026-07-14. **Appends** to everything above — does not touch the +45 tables or the
> `classic-five-enhancements.md` elites/brood. Brings each Classic Five family from ~11
> (9 from the +45 spec incl. its mini-boss + 1 named elite + 1 Brood serpent) to **~35**.
>
> Follows the shard roster standard (`dev-docs/dungeon-ladder-bestiary.md`): one theme × an HP
> curve × role-appropriate resists, where **higher rank = a different role, not bigger numbers**.
> Per dungeon: **15 core ranks + 7 themed sub-faction (incl. 1 named mini-boss) + 2 ambient = 24**.
> Levels stay **band ± 1** so the five dungeons keep their gradient. Most rows are **pure stat
> blocks**; breath / `HitPoison` / pack-instinct / immunities are free and uncounted.

## How to read this (same rules as above)

- **Donor class** — stock T2A body under `Projects/UOContent/Mobiles/` (all 24 donors verified
  present 2026-07-14). Hue recolors it; class names keep the dungeon prefix.
- **Ability** — `—` = pure stat block (the majority). `⚙` marks a custom-code proc from the
  reused toolbox only (`OnGaveMelee`/`OnGotMelee` drain/reflect/blink, `DungeonAbilities.
  TrySpawnAdd`, `AuraPulse`, `MonsterAbility` breath). **New customs ≤ 5 per dungeon** (existing
  3 + new 3 = 6, under the ≤8 ceiling). Breath (`HasBreath`), `HitPoison`, pack instinct and
  immunities are **free** and not tallied.
- **Bag** — trash `LootBagLevel = Lvl − 1`; each new mini-boss inherits `DungeonElite`,
  `EliteBagCount = 1`, guaranteed bag = the dungeon's band level (one under the existing named
  elite), ~20 min respawn.
- **HP** stays inside `LevelConfig.MobLevelFromHits` (L3 ≤160, L4 ≤240, L5 ≤380, L6 ≤550,
  L7 ≤720, L8 ≤950). All 120 want a `MobLevelOverrides` pin at the **Lvl** shown.
- **Name safety** — all 120 class names + 5 mini-boss proper nouns grep-checked clear against
  `Mobiles/**` and `dev-docs/itemization/**` (2026-07-14). Rejected on collision:
  *Aeetes/Aietes* (CursedAeetes mob + pin), *Kolchis* / *Delphyne* (fencing legendaries) —
  Destard's brood is the **Ismenian** line led by **Ismenos** instead.

---

## Despise — L4 · Gaian* (band L3-5)

**Sub-faction: the Gegenes.** Gaia's eldest children — the earth-born giants Zeus once buried —
wake beneath Despise and heave up through the Spartoi's furrows, older and larger than the sown
men, dragging beast-thralls and stone-wardens up with them.

### Core expansion — the risen host (15)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| GaianClodling | a gaian clodling | Ratman | 0x972 ochre | 3 | 130–145 | 8–13 | Melee | Fast | — | — |
| GaianFurrowborn | a furrow-born | Lizardman | 0x9C4 clay-red | 3 | 140–155 | 9–14 | Melee | Fast | — | — |
| GaianRootcrawler | a root-crawler | GiantSpider | 0x9C2 dun | 3 | 135–150 | 8–13 | Melee | Medium | HitPoison (Lesser) | — |
| GaianSlinger | a gaian slinger | Ratman | 0x9C4 clay-red | 4 | 190–210 | 11–16 | Archer | Medium | — | — |
| GaianDelver | a gaian delver | Troll | 0x455 granite | 4 | 195–215 | 12–17 | Melee | Medium | — | — |
| GaianQuarrybrute | a quarry brute | Ettin | 0x455 granite | 4 | 215–235 | 13–18 | Melee | Medium | — | — |
| GaianBoulderback | a boulder-back | Ogre | 0x9C2 dun | 4 | 220–238 | 13–18 | Melee | Slow | — | — |
| GaianClaymason | a clay mason | Golem | 0x972 ochre | 4 | 205–225 | 12–17 | Melee | Slow | poison + bleed immune | — |
| GaianEarthwyrm | a gaian earth-wyrm | GiantSerpent | 0x9C2 dun | 4 | 195–215 | 11–16 | Melee | Medium | poison immune; HitPoison (Regular) | — |
| GaianStoneshaper | a gaian stoneshaper | Gargoyle | 0x455 granite | 4 | 190–210 | 11–16 | Mage | Medium | — | — |
| GaianGraniteAdder | a granite adder | Scorpion | 0x455 granite | 4 | 200–220 | 12–17 | Melee | Slow | poison immune; HitPoison (Greater) | — |
| GaianStonewarden | a stone warden | StoneGargoyle | 0x455 granite | 5 | 300–330 | 14–19 | Melee | Slow | poison + bleed immune | — |
| GaianClayColossus | a clay colossus | Cyclops | 0x972 ochre | 5 | 320–350 | 15–20 | Melee | Slow | — | — |
| GaianMountainborn | a mountain-born | OgreLord | 0x455 granite | 5 | 330–360 | 15–20 | Melee | Slow | — | — |
| GaianTerraGorger | a terra-gorger | EarthElemental | 0x9C2 dun | 5 | 310–340 | 14–19 | Melee | Slow | poison immune | — |

### Themed sub-faction — the Gegenes (7)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| GaianGegenesThrall | a gegenes thrall | Ettin | 0x9C2 dun | 4 | 220–238 | 13–18 | Melee | Medium | — | — |
| GaianGegenesDigger | a gegenes digger | Troll | 0x9C2 dun | 4 | 210–230 | 12–17 | Melee | Medium | — | — |
| GaianGegenesHurler | a gegenes hurler | Cyclops | 0x455 granite | 5 | 320–350 | 15–20 | Archer | Slow | — | — |
| GaianGegenesShaker | a gegenes earth-shaker | EarthElemental | 0x455 granite | 5 | 330–360 | 15–20 | Melee | Slow | poison immune | ⚙ tremor: OnGaveMelee 20% knockback msg + 15 stam drain |
| GaianGegenesWarden | a gegenes warden | StoneGargoyle | 0x455 granite | 5 | 340–365 | 15–20 | Melee | Slow | poison + bleed immune | ⚙ stone skin: OnGotMelee 15% reflect ~20% of the blow |
| GaianGegenesElder | a gegenes elder | OgreLord | 0x455 granite | 5 | 350–370 | 16–21 | Melee | Slow | pack instinct | — |
| **GaianPeloreus** | **Peloreus, the Unearthed** | Titan | 0x972 ochre | 5 | 365–380 | 16–21 | Melee | Medium | poison immune | ⚙ **Heave the earth**: OnGotMelee 15% → `TrySpawnAdd` 1–2 `Ettin` (cap 2). `DungeonElite`, EliteBagLevel **4** |

### Ambient (2)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| GaianRubblecrawler | a rubble-crawler | Slime | 0x972 ochre | 3 | 120–140 | 7–12 | Melee | Slow | poison + bleed immune | — |
| GaianDustadder | a dust-adder | Snake | 0x9C2 dun | 3 | 115–135 | 6–11 | Melee | Fast | HitPoison (Lesser) | — |

New customs: 3 (Shaker, Warden, Peloreus). Dungeon total incl. existing: 6 ⚙.

---

## Deceit — L5 · Drowned* (band L4-6)

**Sub-faction: the Nostoi.** The *nostos* is the sea-voyage home; the Nostoi are the crews who
never finished one — sailors lost on the long return, dragged up under a drowned officer's colors
to work Deceit's flooded halls one last watch.

### Core expansion — the sunken garrison (15)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| DrownedSailor | a drowned sailor | Zombie | 0x835 drowned teal | 4 | 190–210 | 11–16 | Melee | Slow | poison immune | — |
| DrownedFloater | a drowned floater | Ghoul | 0x830 silt | 4 | 185–205 | 10–15 | Melee | Fast | — | — |
| DrownedRower | a drowned rower | Skeleton | 0x841 verdigris | 4 | 180–200 | 10–15 | Melee | Medium | — | — |
| DrownedSilt | a silt-choked corpse | RottingCorpse | 0x830 silt | 4 | 210–230 | 11–16 | Melee | Slow | poison + bleed immune | — |
| DrownedHaunt | a drowned haunt | Spectre | 0x835 drowned teal | 4 | 200–220 | 10–15 | Mage | Fast | — | — |
| DrownedMarine | a drowned marine | SkeletalKnight | 0x841 verdigris | 5 | 330–350 | 15–20 | Melee | Medium | poison immune | — |
| DrownedReaver | a drowned reaver | BoneKnight | 0x847 bone-green | 5 | 350–370 | 16–21 | Melee | Medium | poison immune | — |
| DrownedSaltMummy | a salt-crusted mummy | Mummy | 0x830 silt | 5 | 340–360 | 15–20 | Melee | Slow | poison + bleed immune | — |
| DrownedNecromage | a drowned necromage | SkeletalMage | 0x835 drowned teal | 5 | 320–340 | 13–18 | Mage | Medium | poison immune | — |
| DrownedBanshee | a drowned banshee | Wraith | 0x841 verdigris | 5 | 320–340 | 13–18 | Mage | Fast | — | — |
| DrownedKeen | a keening dead | Bogle | 0x847 bone-green | 5 | 330–350 | 14–19 | Mage | Medium | — | — |
| DrownedTidewraith | a tide-wraith | Shade | 0x835 drowned teal | 5 | 320–340 | 13–18 | Mage | Fast | poison immune | — |
| DrownedWrecklich | a wreck-lich | Lich | 0x835 drowned teal | 6 | 460–490 | 15–20 | Mage | Medium | poison immune | — |
| DrownedDeepGaunt | a deep gaunt | BoneKnight | 0x847 bone-green | 6 | 460–490 | 16–21 | Melee | Medium | poison immune | — |
| DrownedHarbinger | a drowned harbinger | SkeletalKnight | 0x830 silt | 6 | 470–500 | 16–21 | Melee | Medium | poison immune | — |

### Themed sub-faction — the Nostoi (7)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| DrownedNostosDeckhand | a Nostoi deckhand | Zombie | 0x835 drowned teal | 5 | 300–330 | 12–17 | Melee | Slow | poison immune | — |
| DrownedNostosOarsman | a Nostoi oarsman | Skeleton | 0x841 verdigris | 5 | 290–320 | 12–17 | Melee | Medium | — | — |
| DrownedNostosArcher | a Nostoi archer | Skeleton | 0x830 silt | 5 | 290–320 | 12–17 | Archer | Medium | — | — |
| DrownedNostosNavigator | the Nostoi navigator | Spectre | 0x835 drowned teal | 5 | 300–330 | 12–17 | Mage | Medium | poison immune | — |
| DrownedNostosBosun | the Nostoi bosun | SkeletalKnight | 0x841 verdigris | 5 | 330–355 | 14–19 | Melee | Medium | poison immune | ⚙ **Lash of the deep**: OnGaveMelee 20% → 12 stam drain |
| DrownedNostosCurser | a Nostoi curser | BoneMagi | 0x847 bone-green | 5 | 320–345 | 13–18 | Mage | Medium | poison immune | ⚙ **Salt rot**: OnGaveMelee 20% → 12 mana drain |
| **DrownedPhrontis** | **Phrontis, the Unreturned** | BoneKnight | 0x835 drowned teal | 6 | 490–510 | 16–21 | Melee | Medium | poison immune | ⚙ **The tide reclaims**: OnGotMelee 15% → `TrySpawnAdd` 1–2 `Spectre` (cap 2). `DungeonElite`, EliteBagLevel **5** |

### Ambient (2)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| DrownedBrinerot | a mass of brine-rot | Slime | 0x830 silt | 4 | 185–205 | 9–14 | Melee | Slow | poison + bleed immune | — |
| DrownedGraveEel | a grave eel | Snake | 0x841 verdigris | 4 | 180–200 | 9–14 | Melee | Fast | HitPoison (Regular) | — |

New customs: 3 (Bosun, Curser, Phrontis). Dungeon total incl. existing: 6 ⚙.
(Phrontis spawns `Spectre`, not `Skeleton` — kept distinct from Minos.)

---

## Shame — L6 · Brine* (band L5-7)

**Sub-faction: the Telchines.** Rhodes' exiled sea-sorcerers — webbed, dog-headed smiths who
forge storms and blight the tide. Driven out for their malice, they have claimed Shame's flooded
forges and work brine and frost into weapons no drowned man should hold.

### Core expansion — the storm-brood (15)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| BrineSpray | a brine spray | WaterElemental | 0x4F8 storm-teal | 5 | 295–315 | 13–18 | Mage | Slow | can swim | — |
| BrineFrost | a brine frost | SnowElemental | 0x480 pale-cyan | 5 | 300–320 | 12–17 | Melee | Medium | cold immune | — |
| BrineSquallwind | a squall-wind | AirElemental | 0x481 squall-grey | 5 | 300–320 | 12–17 | Mage | Fast | — | — |
| BrineRiptide | a brine riptide | WaterElemental | 0x4F2 deep-teal | 6 | 475–495 | 16–21 | Mage | Medium | can swim | — |
| BrineGlacier | a brine glacier | IceElemental | 0x480 pale-cyan | 6 | 490–510 | 17–22 | Mage | Medium | cold immune | — |
| BrineCyclone | a brine cyclone | AirElemental | 0x481 squall-grey | 6 | 480–500 | 15–20 | Mage | VeryFast | — | — |
| BrineDeepcoil | a deep-coil serpent | SeaSerpent | 0x4F2 deep-teal | 5 | 315–340 | 13–18 | Melee | Medium | can swim; HitPoison (Greater) | — |
| BrineAbyssal | an abyssal serpent | DeepSeaSerpent | 0x4F2 deep-teal | 6 | 490–510 | 16–21 | Melee | Medium | HasBreath Cold ("Undertow") | — |
| BrineCrusher | a brine crusher | Kraken | 0x4F8 storm-teal | 6 | 530–545 | 17–22 | Melee | Slow | can swim | — |
| BrineStormHarpy | a storm-harpy | Harpy | 0x481 squall-grey | 5 | 305–325 | 12–17 | Melee | Fast | — | — |
| BrineSquallHawk | a squall-hawk | StoneHarpy | 0x481 squall-grey | 6 | 500–520 | 15–20 | Melee | Fast | — | — |
| BrineReefcrab | a reef crab | Scorpion | 0x4F2 deep-teal | 5 | 300–320 | 12–17 | Melee | Slow | poison immune; HitPoison (Greater) | — |
| BrineTideElemental | a tide elemental | WaterElemental | 0x4F2 deep-teal | 6 | 510–530 | 16–21 | Melee | Slow | can swim | — |
| BrineHailspite | a hailspite | IceElemental | 0x480 pale-cyan | 7 | 620–655 | 17–22 | Mage | Medium | cold immune | — |
| BrineWaterlord | a brine water-lord | WaterElemental | 0x4F8 storm-teal | 7 | 640–680 | 18–23 | Mage | Medium | can swim | — |

### Themed sub-faction — the Telchines (7)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| BrineTelchinAdept | a telchine adept | WaterElemental | 0x4F8 storm-teal | 6 | 480–500 | 15–20 | Mage | Slow | can swim | — |
| BrineTelchinGaler | a telchine galer | AirElemental | 0x481 squall-grey | 6 | 475–495 | 14–19 | Mage | VeryFast | — | — |
| BrineTelchinDrowner | a telchine drowner | SeaSerpent | 0x4F2 deep-teal | 6 | 490–510 | 16–21 | Melee | Medium | HitPoison (Greater) | — |
| BrineTelchinBrinesmith | a telchine brine-smith | IceElemental | 0x480 pale-cyan | 6 | 490–515 | 16–21 | Mage | Medium | cold immune | ⚙ **Rime riposte**: OnGotMelee 15% reflect ~25% as cold |
| BrineTelchinTideward | a telchine tideward | Kraken | 0x4F2 deep-teal | 6 | 510–530 | 16–21 | Melee | Slow | can swim | ⚙ **Undertow grip**: OnGaveMelee 20% → 12 stam drain |
| BrineTelchinStormcaller | a telchine stormcaller | AirElemental | 0x4F8 storm-teal | 7 | 620–650 | 17–22 | Mage | Fast | HasBreath Cold ("Sleet") | — |
| **BrineOrmenos** | **Ormenos, the Tide-Smith** | Kraken | 0x4F8 storm-teal | 7 | 700–720 | 18–23 | Mage | Medium | can swim; HasBreath Cold ("Maelstrom") | ⚙ **Break the deep**: OnGaveMelee 25% → 15 mana drain. `DungeonElite`, EliteBagLevel **6** |

### Ambient (2)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| BrineJelly | a stinging jelly | Slime | 0x480 pale-cyan | 5 | 295–315 | 11–16 | Melee | Slow | poison immune | — |
| BrinePetrel | a storm-petrel | Eagle | 0x481 squall-grey | 5 | 290–310 | 11–16 | Melee | Fast | — | — |

New customs: 3 (Brinesmith, Tideward, Ormenos). Dungeon total incl. existing: 6 ⚙.
(Ormenos drains + breathes cold instead of `TrySpawnAdd` — kept distinct from Glaukos.)

---

## Destard — L7 · Drakon* (band L6-8)

**Sub-faction: the Ismenian brood.** The dragon of Ares that guarded the spring of Ismenos at
Thebes — whose teeth Cadmus sowed to raise the first Spartoi — left a serpent-lineage as sleepless
as Ladon himself. They coil the deepest hoard-vaults, half drake and half war-serpent.

### Core expansion — the cult's brood (15)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| DrakonHatchling | a drakon hatchling | Drake | 0x501 hoard-gold | 6 | 470–490 | 15–20 | Melee | Medium | — | — |
| DrakonBroodviper | a drakon brood-viper | GiantSerpent | 0x66D crimson | 6 | 465–485 | 13–18 | Melee | Fast | HitPoison (Regular) | — |
| DrakonOphidianSpear | a drakon spear | OphidianWarrior | 0x501 hoard-gold | 6 | 480–500 | 14–19 | Melee | Fast | — | — |
| DrakonScaleraptor | a scale-raptor | Raptor | 0x501 hoard-gold | 6 | 490–510 | 14–19 | Melee | VeryFast | — | — |
| DrakonLavaAdder | a lava adder | LavaSerpent | 0x489 ember | 6 | 470–490 | 14–19 | Melee | Medium | HasBreath Fire | — |
| DrakonOphidianAvenger | a drakon avenger | OphidianKnight | 0x66D crimson | 7 | 650–670 | 17–22 | Melee | Medium | — | — |
| DrakonOphidianSeer | a drakon seer | OphidianMage | 0x501 hoard-gold | 7 | 640–660 | 15–20 | Mage | Medium | — | — |
| DrakonDrakeling | a drakon drakeling | Drake | 0x66D crimson | 7 | 660–680 | 18–23 | Melee | Medium | — | — |
| DrakonWyvern | a drakon wyvern | Wyvern | 0x66D crimson | 7 | 640–660 | 18–23 | Melee | Fast | HitPoison (Greater) | — |
| DrakonEmberdrake | an ember drake | Drake | 0x489 ember | 7 | 665–685 | 19–24 | Melee | Medium | HasBreath Fire | — |
| DrakonSerpentGuard | a drakon serpent-guard | OphidianKnight | 0x489 ember | 7 | 655–675 | 17–22 | Melee | Medium | — | — |
| DrakonMatron | a drakon matron | SerpentineDragon | 0x66D crimson | 7 | 700–720 | 17–22 | Mage | Medium | — | — |
| DrakonFirewyrm | a drakon fire-wyrm | Dragon | 0x489 ember | 7 | 700–720 | 18–23 | Mage | Medium | HasBreath Fire | — |
| DrakonHoardWyrm | a hoard wyrm | WhiteWyrm | 0x501 hoard-gold | 8 | 820–860 | 19–24 | Melee | Medium | HasBreath Cold | — |
| DrakonGreatDrake | a drakon great-drake | Dragon | 0x66D crimson | 8 | 850–890 | 20–25 | Mage | Medium | HasBreath Fire | — |

### Themed sub-faction — the Ismenian brood (7)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| DrakonIsmenianCoil | an Ismenian coil | GiantSerpent | 0x489 ember | 7 | 640–660 | 15–20 | Melee | Medium | HitPoison (Greater) | — |
| DrakonIsmenianFang | an Ismenian fang | OphidianWarrior | 0x66D crimson | 7 | 645–665 | 16–21 | Melee | Fast | — | — |
| DrakonIsmenianWard | an Ismenian ward | OphidianKnight | 0x489 ember | 7 | 655–675 | 17–22 | Melee | Medium | — | — |
| DrakonIsmenianDrake | an Ismenian drake | Drake | 0x489 ember | 7 | 665–685 | 18–23 | Melee | Medium | HasBreath Fire | — |
| DrakonIsmenianAugur | an Ismenian augur | OphidianMage | 0x66D crimson | 7 | 640–660 | 15–20 | Mage | Medium | — | ⚙ **Venom-word**: OnGaveMelee 20% → 12 mana drain |
| DrakonIsmenianWyrm | an Ismenian wyrm | Wyvern | 0x501 hoard-gold | 7 | 645–670 | 18–23 | Melee | Fast | HitPoison (Greater) | ⚙ **Tail-lash**: OnGotMelee 15% knock the attacker back 2 tiles |
| **DrakonIsmenos** | **Ismenos, the Hoard-Coiled** | SerpentineDragon | 0x489 ember | 7 | 705–720 | 18–23 | Mage | Medium | HasBreath Fire | ⚙ **Coil and crush**: OnGotMelee 15% → `TrySpawnAdd` 1–2 `GiantSerpent` (cap 2). `DungeonElite`, EliteBagLevel **7** |

### Ambient (2)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| DrakonAshviper | an ash viper | Snake | 0x66D crimson | 6 | 465–485 | 12–17 | Melee | Fast | HitPoison (Regular) | — |
| DrakonScalerat | a scale-rat | GiantRat | 0x501 hoard-gold | 6 | 460–480 | 12–17 | Melee | Medium | — | — |

New customs: 3 (Augur, Wyrm, Ismenos). Dungeon total incl. existing: 6 ⚙.
(Ismenos spawns `GiantSerpent`, not `Drake` — kept distinct from Pythios.)

---

## Hythloth — L8 · Tartarus* (band L7-9)

**Sub-faction: the Chained Titans.** Below the demon sanctum the first Titans still lie where
Zeus flung them, straining at adamantine bonds. Their thrashings crack Hythloth's floor and loose
Titan-thralls and their gargoyle jailers into the pit around Alastor.

### Core expansion — the pit's brood (15)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| TartarusImpling | a tartarus impling | Imp | 0x21 blood-hell | 7 | 635–655 | 16–21 | Mage | VeryFast | — | — |
| TartarusPyrehound | a pyre-hound | HellHound | 0x22 ashen-red | 7 | 640–660 | 15–20 | Melee | Fast | HasBreath Fire; pack instinct | — |
| TartarusEmberwing | an ember-wing gargoyle | FireGargoyle | 0x22 ashen-red | 7 | 645–665 | 15–20 | Mage | Fast | HasBreath Fire | — |
| TartarusGoreling | a goreling | GoreFiend | 0x21 blood-hell | 7 | 650–670 | 16–21 | Melee | Fast | poison immune | — |
| TartarusChaosbrand | a chaos-brand | ChaosDaemon | 0x22 ashen-red | 7 | 640–660 | 15–20 | Melee | Fast | — | — |
| TartarusEfreet | a pit-efreet | Efreet | 0x22 ashen-red | 7 | 645–665 | 15–20 | Mage | Medium | HasBreath Fire | — |
| TartarusGargoyleLord | a tartarus gargoyle-lord | Gargoyle | 0x455 slate | 7 | 645–665 | 15–20 | Mage | Medium | — | — |
| TartarusDaemonspawn | a tartarus daemonspawn | Daemon | 0x21 blood-hell | 8 | 840–860 | 20–25 | Mage | Medium | — | — |
| TartarusRimefiend | a rime-fiend | IceFiend | 0x455 slate | 8 | 890–910 | 21–26 | Mage | Medium | HasBreath Cold | — |
| TartarusHexfiend | a hex-fiend | ArcaneDaemon | 0x21 blood-hell | 8 | 815–835 | 18–23 | Mage | Fast | — | — |
| TartarusScourgewing | a scourge-wing | GargoyleEnforcer | 0x22 ashen-red | 8 | 820–840 | 18–23 | Mage | Fast | — | — |
| TartarusSlatewarden | a slate warden | StoneGargoyle | 0x455 slate | 8 | 875–895 | 19–24 | Melee | Slow | poison immune | — |
| TartarusSuccubus | a pit-succubus | Succubus | 0x21 blood-hell | 8 | 830–850 | 19–24 | Mage | Medium | — | — |
| TartarusBrimstone | a brimstone daemon | Daemon | 0x22 ashen-red | 8 | 850–870 | 20–25 | Mage | Medium | HasBreath Fire | — |
| TartarusSoulflayer | a soulflayer | GoreFiend | 0x22 ashen-red | 8 | 855–875 | 20–25 | Melee | Fast | poison immune | — |

### Themed sub-faction — the Chained Titans (7)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| TartarusTitanThrall | a chained thrall | Cyclops | 0x455 slate | 8 | 870–900 | 19–24 | Melee | Slow | — | — |
| TartarusTitanShackled | a shackled titan | OgreLord | 0x455 slate | 8 | 880–910 | 20–25 | Melee | Slow | pack instinct | — |
| TartarusTitanWarden | a titan-warden | Ettin | 0x22 ashen-red | 7 | 650–680 | 15–20 | Melee | Medium | — | — |
| TartarusTitanColossus | a titan colossus | Cyclops | 0x455 slate | 8 | 900–930 | 21–26 | Melee | Slow | — | — |
| TartarusTitanJailer | a titan-jailer | GargoyleEnforcer | 0x22 ashen-red | 8 | 830–855 | 19–24 | Mage | Fast | — | ⚙ **Warding flame**: OnGotMelee 15% reflect ~25% as fire |
| TartarusTitanBreaker | a titan-breaker | Titan | 0x22 ashen-red | 8 | 900–930 | 21–26 | Mage | Medium | HasBreath Energy ("Levin") | ⚙ **Thunder-fist**: OnGaveMelee 20% knockback msg + 15 stam drain |
| **TartarusMenoetius** | **Menoetius, the Chained** | Titan | 0x21 blood-hell | 8 | 930–950 | 21–26 | Mage | Medium | poison immune; HasBreath Energy ("Levin") | ⚙ **Rage of the pit**: OnGotMelee 15% → `TrySpawnAdd` 1–2 `Imp` (cap 2). `DungeonElite`, EliteBagLevel **8** |

### Ambient (2)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities / Poison / Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| TartarusCinderImp | a cinder imp | Imp | 0x22 ashen-red | 7 | 635–650 | 15–20 | Mage | VeryFast | — | — |
| TartarusAshhound | an ash-hound | HellHound | 0x455 slate | 7 | 640–655 | 14–19 | Melee | Fast | — | — |

New customs: 3 (Jailer, Breaker, Menoetius). Dungeon total incl. existing: 6 ⚙.
(Menoetius spawns `Imp` + breathes energy — kept distinct from Eurynomos's fire aura.)

---

## Expansion totals appendix

| Dungeon | Prefix | Band | Core | Sub-faction | Ambient | New mini-boss | **New total** | New ⚙ (≤5) | +Existing ⚙ | Cumulative roster |
|---|---|---|---|---|---|---|---|---|---|---|
| Despise | Gaian* | L4 (L3-5) | 15 | 6 + Peloreus | 2 | Peloreus (bag 4) | **24** | 3 | 6 | ~35 |
| Deceit | Drowned* | L5 (L4-6) | 15 | 6 + Phrontis | 2 | Phrontis (bag 5) | **24** | 3 | 6 | ~35 |
| Shame | Brine* | L6 (L5-7) | 15 | 6 + Ormenos | 2 | Ormenos (bag 6) | **24** | 3 | 6 | ~35 |
| Destard | Drakon* | L7 (L6-8) | 15 | 6 + Ismenos | 2 | Ismenos (bag 7) | **24** | 3 | 6 | ~35 |
| Hythloth | Tartarus* | L8 (L7-9) | 15 | 6 + Menoetius | 2 | Menoetius (bag 8) | **24** | 3 | 6 | ~35 |
| **Total** | — | — | **75** | **35** | **10** | **5** | **120** | **15** | **30** | **~175** |

**Cumulative per dungeon** = 11 existing (9 from the +45 spec incl. its mini-boss + Enkelados-tier
elite + Brood serpent) + 24 = **35**. Every dungeon lands 3 new ⚙ (2 sub-faction procs + 1
mini-boss), so combined with the 3 existing customs each sits at **6 ⚙, under the ≤8 ceiling**.

**Reused toolbox only** (no new plumbing): `DungeonAbilities.TrySpawnAdd` (Peloreus→Ettin,
Phrontis→Spectre, Ismenos→GiantSerpent, Menoetius→Imp), `OnGaveMelee` drains (Nostoi Bosun/Curser
stam+mana, Telchine Tideward stam, Ormenos mana, Ismenian Augur mana, Gegenes Shaker + Titan
Breaker stam-with-knockback), `OnGotMelee` reflect/blink (Gegenes Warden + Telchine Brinesmith +
Titan Jailer reflect, Ismenian Wyrm 2-tile knockback), plus free features: `HasBreath`
Fire/Cold/Energy local reskins, `HitPoison`, pack instinct, and stock immunities (golem/undead
poison, elemental cold, corpse bleed).

**Spawn wiring** (impl, out of scope): additive to the existing
`Distribution/Data/Spawns/uoml/felucca/classic-elites.json` (or a sibling) at each dungeon's real
coords; all 120 need `LevelConfig.MobLevelOverrides` pins at the **Lvl** column. New mini-bosses
respawn ~20 min. Two integration notes for the builder, mirroring the +45 pass: the SeaSerpent/
DeepSeaSerpent/Kraken donors are `CantWalk` — Shame's `BrineCrusher`/`BrineTelchinTideward`/
`BrineOrmenos` want the land-walk fix already applied to `BrineLeechEel`; and any AOS-era
`GetWeaponAbility` overrides on the ML donors (LavaSerpent, GoreFiend, ArcaneDaemon, Succubus)
are meaningless on T2A — drop them.
</content>
