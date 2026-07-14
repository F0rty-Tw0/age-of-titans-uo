# Dungeon Ladder — Expanded Bestiary (roster ×5, design, 2026-07-14)

> **IMPLEMENTED 2026-07-14** (uncommitted): all 140 classes on disk under
> `Mobiles/Dungeons/<Dungeon>/`, spawners extended in the five dungeon JSONs, 140
> LevelConfig pins, build clean, migrations generated. Integration fixes: Stygian
> mini-boss renamed Minos → **Rhadamanthys** (classic-five already seats Minos in Deceit);
> 5 wisp-donor Aerie mobs un-passived (FightMode.Aggressor → aggressive). Enriches the five Domain dungeons of
> `dev-docs/dungeon-ladder.md` from ~7-8 creatures each to ~35. Adds **+28 new mobs per
> dungeon (140 total)**, the reference shard deep-keep style (`dev-docs/reference-shard-bestiary.md`,
> `dev-docs/reference-shard-creatures.md` §3): one theme × an HP curve × role-appropriate resists =
> a deep roster where **higher rank means a different role, not bigger numbers**. Most rows
> are **pure stat blocks** — no custom code, just stats/speed/AI/immunities/HitPoison/breath/
> pack-instinct, which the codebase already supports for free.

## How to read this

- **Donor class** — a stock T2A body that exists under `Projects/UOContent/Mobiles/` (all
  verified 2026-07-14). Copy the donor's source for proven `Body`/`BaseSoundID` values; the
  hue recolors it. Class names use the dungeon prefix (`Tide/Cinder/Wyld/Storm/Stygian`).
- **Immunities/Poison/Breath** — all zero-cost overrides. `HasBreath Energy`/`HasBreath
  Poison` are local one-class `FireBreath` reskins (established idiom — see
  `StormWisp.EnergyBreath`, `WyldSentinel`'s `PoisonBreath`); native breaths are
  Fire/Cold/Chaos (`MonsterAbilities`).
- **Ability** — `—` means pure stat block (the majority). A named ability is one entry from
  the shared toolbox: `OnGaveMeleeAttack` proc (stam/mana drain, flavor msg),
  `OnGotMeleeAttack` proc (reflect, blink, capped add via `DungeonAbilities.TrySpawnAdd`), or
  a damage aura (`DungeonAbilities.AuraPulse`). **≤8 custom-ability mobs per dungeon** (tally
  in the appendix); the rest are free.
- **Bag** — trash `LootBagLevel = mob level − 1`. Mini-boss inherits `DungeonElite`, drops the
  dungeon's existing **elite bag**, `EliteBagCount = 1`, respawn ~20-30 min.
- HP stays inside each level's band (`LevelConfig.MobLevelFromHits`): L4 ≤240, L5 ≤380,
  L6 ≤550, L7 ≤720, L8 ≤950, L9 ≤2400, L10 2400-3600 (kept under each boss).

---

## 1. The Drowned Tholos — Sea / Poseidon (L4-5) · +28

Existing bag: trash bag3 (L4)/bag4 (L5), elite bag5. Mini-boss → bag5.

### Core family expansion — the drowned garrison (15)
*The sunken city's marines, divers and reef-beasts still hold their posts; low resists, sustained pressure, cold/poison to manage.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `TideConscript` | a drowned conscript | Zombie | drowned teal 0x0847 | 4 | 110-150 | 6-9 | Melee | Slow | BleedImmune | — |
| `TideDiver` | a drowned diver | Ghoul | pale aqua 0x0481 | 4 | 120-160 | 7-10 | Melee | Fast | — | — |
| `TideRower` | a barnacled rower | Skeleton | verdigris 0x08A5 | 4 | 100-140 | 6-9 | Melee | Medium | — | — |
| `TideLeech` | a brine leech | Slime | deep blue 0x0530 | 4 | 90-130 | 5-8 | Melee | Slow | PoisonImmune | HitPoison Lesser |
| `TideEel` | a coiling eel | Snake | pale aqua 0x0481 | 4 | 100-140 | 6-9 | Melee | VeryFast | — | HitPoison Regular |
| `TideMarine` | a drowned marine | Skeletal Knight | verdigris 0x08A5 | 5 | 300-350 | 10-14 | Melee | Medium | — | — |
| `TidePikeman` | a barnacled pikeman | Bone Knight | verdigris 0x08A5 | 5 | 320-370 | 11-15 | Melee | Medium | BleedImmune; pack instinct | — |
| `TideVotary` | a drowned votary | Spectre | Poseidon blue 0x0532 | 5 | 280-330 | 9-13 | Mage | Medium | PoisonImmune | — |
| `TideHexer` | a drowned hexer | Skeletal Mage | Poseidon blue 0x0532 | 5 | 280-330 | 9-13 | Mage | Medium | PoisonImmune | — |
| `TideCoralguard` | a coral guard | Water Elemental | Poseidon blue 0x0532 | 5 | 320-380 | 11-15 | Melee | Slow | PoisonImmune | — |
| `TideAbyssEel` | a deep serpent | Deep Sea Serpent | deep blue 0x0530 | 5 | 320-380 | 11-15 | Melee | Medium | — | HasBreath Cold ("Brine Breath") |
| `TideReefserpent` | a reef serpent | Sea Serpent | reef green-gold 0x0851 | 5 | 300-360 | 10-14 | Melee | Fast | — | HitPoison Greater |
| `TideSpinecrab` | a spinecrab | Scorpion | deep blue 0x0530 | 5 | 300-350 | 10-14 | Melee | Slow | PoisonImmune | HitPoison Greater |
| `TideBloated` | a bloated drowned | Rotting Corpse | drowned teal 0x0847 | 5 | 340-380 | 12-16 | Melee | Slow | BleedImmune + PoisonImmune | — |
| `TideWraith` | a drowned wraith | Wraith | Poseidon blue 0x0532 | 5 | 280-330 | 9-13 | Mage | Fast | — | — |

### Themed block — the drowned crew of the *Pelagos* (9)
*A single wrecked trireme's company, ranked bosun to captain; the officers carry the drain/reflect procs.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `TidePelagosDeckhand` | a Pelagos deckhand | Zombie | drowned teal 0x0847 | 5 | 300-350 | 10-14 | Melee | Slow | BleedImmune | — |
| `TidePelagosLookout` | a Pelagos lookout | Ghoul | pale aqua 0x0481 | 5 | 280-330 | 9-13 | Melee | Fast | — | — |
| `TidePelagosHarpooner` | a Pelagos harpooner | Skeleton | pale aqua 0x0481 | 5 | 280-320 | 9-13 | Archer | Medium | — | — |
| `TidePelagosDrummer` | a Pelagos drummer | Skeleton | verdigris 0x08A5 | 5 | 280-320 | 9-13 | Melee | Medium | — | — |
| `TidePelagosNavigator` | the Pelagos navigator | Spectre | Poseidon blue 0x0532 | 5 | 280-330 | 9-13 | Mage | Medium | PoisonImmune | — |
| `TidePelagosOarmaster` | the Pelagos oarmaster | Bone Knight | verdigris 0x08A5 | 5 | 330-370 | 11-15 | Melee | Medium | BleedImmune; pack instinct | — |
| `TidePelagosBosun` | the Pelagos bosun | Skeletal Knight | verdigris 0x08A5 | 5 | 320-360 | 11-15 | Melee | Medium | — | **Lash of the Deep** — OnGaveMelee 20%: 12 stam drain |
| `TidePelagosBrineshade` | a Pelagos brineshade | Wraith | deep blue 0x0530 | 5 | 280-330 | 9-13 | Mage | Fast | — | **Salt Rot** — OnGaveMelee 20%: 12 mana drain |
| `TidePelagosCaptain` | the Pelagos captain | Skeletal Knight | reef green-gold 0x0851 | 5 | 340-375 | 12-16 | Melee | Medium | BleedImmune | **Broadside** — OnGotMelee 15%: knockback msg + 10 stam drain |

### Ambient / fodder (3)
*Reef vermin at the water's edge.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `TideJelly` | a drifting jelly | Slime | pale aqua 0x0481 | 4 | 80-110 | 4-7 | Melee | Slow | PoisonImmune | HitPoison Lesser |
| `TideCrabling` | a scuttling crab | Scorpion | deep blue 0x0530 | 4 | 90-120 | 5-8 | Melee | Slow | — | — |
| `TideGull` | a brine gull | Mongbat | pale aqua 0x0481 | 4 | 80-110 | 4-7 | Melee | Fast | — | — |

### Mini-boss (1)
*Between Kymopoleia (elite) and Aigaion (boss).*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `TideNavarch` | Nauplios, the Wreckwarden | Deep Sea Serpent | abyssal 0x0850 | 5 | 375-380 | 14-18 | Melee | Medium | BleedImmune | **Wreckwarden's Pull** — OnGaveMelee 25%: 18 stam drain + knockback msg; HasBreath Cold ("Undertow") |

---

## 2. The Cinderworks — Forge / Hephaestus (L5-6) · +28

Existing bag: trash bag4 (L5)/bag5 (L6), elite bag6. Mini-boss → bag6.

### Core family expansion — slag-things & forge laborers (15)
*Automaton labor and lava-born beasts mistaking intruders for stock; constructs carry the immunities, fire everywhere.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CinderStoker` | a slag stoker | Ogre | ash grey 0x0964 | 5 | 300-350 | 10-14 | Melee | Slow | PoisonImmune | — |
| `CinderHauler` | a bronze hauler | Troll | bronze 0x0798 | 5 | 300-350 | 10-14 | Melee | Medium | — | — |
| `CinderEmberling` | an emberling | Imp | flame 0x0655 | 5 | 260-310 | 8-12 | Mage | Fast | — | HasBreath Fire ("Cinder Breath") |
| `CinderSalamander` | a forge salamander | Lava Lizard | Hephaestus orange 0x0654 | 5 | 280-330 | 9-13 | Melee | Fast | PoisonImmune | HasBreath Fire |
| `CinderSlaghound` | a slag hound | Hell Hound | basalt 0x0966 | 5 | 280-330 | 9-13 | Melee | Fast | — | HasBreath Fire; pack instinct |
| `CinderScorial` | a cinder scorpion | Scorpion | soot 0x0967 | 5 | 260-310 | 8-12 | Melee | Slow | — | HitPoison Greater |
| `CinderMoth` | a molten mongrel | Mongbat | flame 0x0655 | 5 | 260-300 | 8-11 | Melee | Fast | — | — |
| `CinderGrelt` | a slag grotesque | Stone Gargoyle | basalt 0x0966 | 6 | 460-510 | 12-16 | Melee | Medium | Bleed + Poison immune | — |
| `CinderForgewright` | a forge wright | Golem | bronze 0x0798 | 6 | 500-550 | 13-17 | Melee | Slow | Bleed + Poison immune | — |
| `CinderPyreling` | a pyre gargoyle | Fire Gargoyle | flame 0x0655 | 6 | 460-520 | 12-16 | Mage | Fast | — | HasBreath Fire |
| `CinderMoltling` | a molten elemental | Fire Elemental | Hephaestus orange 0x0654 | 6 | 500-550 | 13-17 | Melee | Medium | PoisonImmune | HasBreath Fire |
| `CinderBronzeOgre` | a bronze ogre lord | Ogre Lord | bronze 0x0798 | 6 | 500-550 | 14-18 | Melee | Slow | — | — |
| `CinderSlagDaemon` | a slag daemon | Daemon | soot + ember 0x0967 | 6 | 500-550 | 13-17 | Mage | Medium | PoisonImmune | HasBreath Fire |
| `CinderKindler` | a forge kindler | Efreet | flame 0x0655 | 6 | 480-540 | 13-17 | Mage | Medium | — | HasBreath Fire |
| `CinderCrucible` | a crucible construct | Golem | Hephaestus orange 0x0654 | 6 | 500-550 | 13-17 | Melee | Slow | Bleed + Poison immune | **Radiant Heat** — aura: 3 fire/tick to adjacent |

### Themed block — the Kerykes bronze-servant line (9)
*Hephaestus's golden heralds, forged to serve and still tending a dead forge; the warden/smith/foreman carry procs.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CinderKeryx` | a bronze keryx | Golem | bronze 0x0798 | 6 | 500-550 | 13-17 | Melee | Slow | Bleed + Poison immune | — |
| `CinderKeryxCourier` | a keryx courier | Imp | bronze 0x0798 | 5 | 300-350 | 9-13 | Mage | VeryFast | — | — |
| `CinderKeryxHerald` | the keryx herald | Gargoyle | bronze 0x0798 | 6 | 470-520 | 12-16 | Mage | Medium | — | — |
| `CinderKeryxSentry` | a keryx sentry | Stone Gargoyle | bronze 0x0798 | 6 | 490-540 | 13-17 | Melee | Slow | Bleed + Poison immune | — |
| `CinderKeryxBellows` | a keryx bellows-tender | Fire Elemental | Hephaestus orange 0x0654 | 6 | 480-530 | 12-16 | Melee | Slow | PoisonImmune | HasBreath Fire |
| `CinderKeryxSapper` | a keryx sapper | Ettin | bronze 0x0798 | 6 | 500-550 | 13-17 | Melee | Medium | — | — |
| `CinderKeryxWarden` | a keryx warden | Stone Gargoyle | bronze 0x0798 | 6 | 490-540 | 13-17 | Melee | Medium | Bleed + Poison immune | **Molten Riposte** — OnGotMelee 20%: reflect 25% as fire |
| `CinderKeryxSmith` | a keryx smith | Golem | soot + ember 0x0967 | 6 | 500-550 | 13-17 | Melee | Slow | Bleed + Poison immune | **Emberbrand** — OnGaveMelee 20%: 12 mana drain |
| `CinderKeryxForeman` | the keryx foreman | Ogre Lord | bronze 0x0798 | 6 | 510-550 | 14-18 | Melee | Slow | — | **Hammerfall** — OnGaveMelee 25%: 15 stam drain + knockback msg |

### Ambient / fodder (3)
*Forge vermin in the slag pits.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CinderMothling` | a cinder moth | Mongbat | flame 0x0655 | 5 | 260-290 | 7-10 | Melee | Fast | — | — |
| `CinderSlagling` | a slagling | Slime | ash grey 0x0964 | 5 | 260-290 | 7-10 | Melee | Slow | Bleed + Poison immune | — |
| `CinderEmberrat` | an ember rat | Giant Rat | flame 0x0655 | 5 | 260-290 | 7-10 | Melee | Medium | — | — |

### Mini-boss (1)
*Between Brontes (elite) and Kelmion (boss).*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CinderKedalion` | Kedalion, the Bellows-Bound | Efreet | molten core 0x0669 | 6 | 545-550 | 15-19 | Mage | Medium | PoisonImmune | **Bellows-Wrath** — OnGaveMelee 25%: 15 mana drain; HasBreath Fire ("Pyre") |

---

## 3. The Nemean Wildwood — Hunt / Artemis (L6-7) · +28

Existing bag: trash bag5 (L6)/bag6 (L7), elite bag7. Mini-boss → bag7.

### Core family expansion — the moon-marked pack (15)
*Beasts wearing the goddess's silver mark, hunting in coordinated packs; heavy poison discipline, packs everywhere.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WyldWolf` | a moonlit wolf | Grey Wolf | silver-grey 0x0483 | 6 | 440-500 | 11-15 | Melee | Fast | — | pack instinct |
| `WyldLynx` | a silver lynx | Cougar | silver-grey 0x0483 | 6 | 440-490 | 11-15 | Melee | VeryFast | — | — |
| `WyldBoar` | a great boar | Boar | mossy 0x0844 | 6 | 460-510 | 12-16 | Melee | Fast | — | — |
| `WyldViper` | a grove viper | Snake | emerald 0x0851 | 6 | 440-490 | 11-15 | Melee | VeryFast | — | HitPoison Greater |
| `WyldStinger` | a thornscorpion | Scorpion | mossy 0x0844 | 6 | 440-490 | 11-15 | Melee | Slow | — | HitPoison Deadly |
| `WyldHarrier` | a moon harrier | Harpy | silver-grey 0x0483 | 6 | 440-490 | 11-15 | Melee | Fast | — | — |
| `WyldHart` | a silver hart | Great Hart | silver 0x0486 | 6 | 440-490 | 11-15 | Melee | Fast | — | — |
| `WyldGrizzly` | a moon-marked bear | Grizzly Bear | silver-grey 0x0483 | 7 | 620-680 | 15-19 | Melee | Fast | — | — |
| `WyldDirewolf` | a dire wolf of the hunt | Dire Wolf | silver 0x0486 | 7 | 620-680 | 15-19 | Melee | VeryFast | — | pack instinct |
| `WyldPuma` | a silverpelt panther | Panther | silver 0x0486 | 7 | 620-690 | 15-19 | Melee | VeryFast | BleedImmune | — |
| `WyldConstrictor` | a sacred constrictor | Giant Serpent | emerald 0x0851 | 7 | 620-690 | 15-19 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `WyldTreant` | a grove treant | Reaper | mossy green 0x0851 | 7 | 640-710 | 15-19 | Mage | Slow | PoisonImmune | HasBreath Poison ("Pollen Breath") |
| `WyldCorpser` | a thornwood corpser | Corpser | mossy 0x0844 | 7 | 620-690 | 15-19 | Melee | Slow | — | HitPoison Greater |
| `WyldSpider` | a silverweb spider | Giant Spider | silver-grey 0x0483 | 7 | 600-670 | 14-18 | Melee | Medium | — | HitPoison Deadly |
| `WyldNemean` | a nemean cougar | Cougar | bronze 0x0798 | 7 | 620-690 | 15-19 | Melee | VeryFast | — | — |

### Themed block — the feral Huntresses of the *Thiasos* (9)
*Trespasser-huntresses gone half-feral with their beasts; the stalker/houndmaster/matron carry blink/summon/drain.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WyldThiasosHuntress` | a Thiasos huntress | Evil Mage | silver 0x0486 | 7 | 600-660 | 14-18 | Archer | Medium | — | — |
| `WyldThiasosArcher` | a Thiasos archer | Evil Mage | mossy 0x0844 | 7 | 600-650 | 14-18 | Archer | Medium | — | — |
| `WyldThiasosWitch` | a Thiasos witch | Evil Mage | mossy green 0x0851 | 7 | 600-660 | 14-18 | Mage | Medium | PoisonImmune | HitPoison Deadly |
| `WyldThiasosPriestess` | the Thiasos priestess | Evil Mage | silver 0x0486 | 7 | 610-670 | 14-18 | Mage | Medium | PoisonImmune | — |
| `WyldThiasosHound` | a Thiasos hound | Hell Hound | silver-grey 0x0483 | 6 | 440-490 | 11-15 | Melee | Fast | — | pack instinct |
| `WyldThiasosPanther` | a Thiasos panther | Panther | silver 0x0486 | 7 | 620-690 | 15-19 | Melee | VeryFast | BleedImmune | — |
| `WyldThiasosStalker` | a Thiasos stalker | Evil Mage | silver-grey 0x0483 | 7 | 600-660 | 14-18 | Melee | Fast | — | **Vanish** — OnGotMelee 15%: blink behind attacker |
| `WyldThiasosHoundmaster` | the Thiasos houndmaster | Evil Mage | silver 0x0486 | 7 | 620-680 | 15-19 | Mage | Medium | — | **Loose the Pack** — OnGotMelee 15%: spawn 1 `WyldWolf` (cap 2, dispel-vulnerable) |
| `WyldThiasosMatron` | the Thiasos matron | Evil Mage | bronze 0x0798 | 7 | 640-700 | 15-19 | Mage | Medium | — | **Marked Quarry** — OnGaveMelee 20%: msg + 12 stam drain |

### Ambient / fodder (3)
*Grove critters at the tree line.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WyldFawn` | a moonlit fawn | Hind | silver-grey 0x0483 | 6 | 440-470 | 10-14 | Melee | Fast | — | — |
| `WyldSpiderling` | a silverweb spiderling | Giant Spider | silver-grey 0x0483 | 6 | 440-470 | 10-14 | Melee | Medium | — | HitPoison Greater |
| `WyldSprite` | a thorn sprite | Pixie | mossy green 0x0851 | 6 | 440-470 | 10-14 | Mage | VeryFast | — | — |

### Mini-boss (1)
*Between Atalanta (elite) and Elaphos (boss).*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WyldProkris` | Prokris, the Unerring | Grizzly Bear | silver 0x0486 | 7 | 710-720 | 16-20 | Melee | Fast | BleedImmune | **Never Misses** — OnGaveMelee 25%: knockback msg + 15 stam drain |

---

## 4. The Stormcrown Aerie — Sky / Zeus (L8-9) · +28

Existing bag: trash bag7 (L8)/bag8 (L9), elite bag9. Mini-boss → bag9.

### Core family expansion — the storm-wreathed guard (15)
*Lightning giants and living tempests on the shattered ramparts; the L8→L9 HP wall is the gear check. Energy-resist gearing.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `StormRoc` | a squall harpy | Harpy | storm violet 0x0491 | 8 | 800-880 | 16-20 | Melee | Fast | — | pack instinct |
| `StormGale` | a gale spirit | Air Elemental | Zeus white-gold 0x0481 | 8 | 800-900 | 16-20 | Melee | Fast | PoisonImmune | — |
| `StormSprite` | a levin wisp | Wisp | electric blue 0x0480 | 8 | 780-860 | 15-19 | Mage | VeryFast | — | HasBreath Energy |
| `StormGazer` | a storm gazer | Gazer | storm violet 0x0491 | 8 | 800-880 | 16-20 | Mage | Slow | — | — |
| `StormThunderharpy` | a thunder harpy | Stone Harpy | storm grey 0x0492 | 8 | 820-900 | 16-20 | Melee | Fast | — | — |
| `StormChainling` | a chained ettin | Ettin | storm grey 0x0492 | 8 | 820-920 | 17-21 | Melee | Slow | — | — |
| `StormEfreet` | a sky efreet | Efreet | electric blue 0x0480 | 8 | 800-880 | 16-20 | Mage | Medium | — | HasBreath Energy |
| `StormColossus` | a chained colossus | Cyclops | storm grey 0x0492 | 9 | 2200-2400 | 20-26 | Melee | Slow | BleedImmune | — |
| `StormTitanling` | a lesser titan | Titan | lightning-scarred 0x0492 | 9 | 2200-2400 | 20-25 | Mage | Medium | — | HasBreath Energy |
| `StormWyrm` | a levin wyrm | White Wyrm | crackling blue 0x0480 | 9 | 2150-2380 | 19-24 | Melee | Medium | high energy resist | HasBreath Energy |
| `StormElemental` | a living tempest | Air Elemental | Zeus white-gold 0x0481 | 9 | 2200-2400 | 20-25 | Melee | Fast | PoisonImmune | HasBreath Energy |
| `StormGargoyle` | a storm gargoyle | Gargoyle | storm violet 0x0491 | 9 | 2100-2350 | 19-24 | Mage | Fast | — | — |
| `StormEye` | a tempest eye | Elder Gazer | storm violet 0x0491 | 9 | 2200-2400 | 20-25 | Mage | Slow | — | — |
| `StormThunderdrake` | a thunderhead drake | Drake | thunderhead 0x0492 | 9 | 2100-2350 | 19-24 | Melee | Fast | high energy resist | HasBreath Energy |
| `StormOgreKing` | a storm-forged ogre lord | Ogre Lord | storm grey 0x0492 | 9 | 2250-2400 | 21-26 | Melee | Slow | BleedImmune | **Gale Buffet** — OnGaveMelee 20%: knockback msg + 15 stam drain |

### Themed block — the Anemoi wind-spirits (9)
*Living winds set to guard the chained Titans; the outrider/vortex/gale carry stam/aura/reflect.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `StormAnemoiDrift` | an anemoi drift | Air Elemental | Zeus white-gold 0x0481 | 8 | 800-880 | 16-20 | Melee | VeryFast | PoisonImmune | — |
| `StormAnemoiBreeze` | a wandering breeze | Wisp | Zeus white-gold 0x0481 | 8 | 780-850 | 15-19 | Mage | VeryFast | — | — |
| `StormAnemoiNorthwind` | a northwind spirit | Air Elemental | crackling blue 0x0480 | 9 | 2150-2380 | 19-24 | Melee | Fast | PoisonImmune | HasBreath Cold ("North Wind") |
| `StormAnemoiSquall` | a squall-spirit | Wisp | storm violet 0x0491 | 9 | 2100-2350 | 19-24 | Mage | VeryFast | — | HasBreath Energy |
| `StormAnemoiHerald` | the anemoi herald | Dark Wisp | storm violet 0x0491 | 9 | 2200-2400 | 20-25 | Mage | VeryFast | — | HasBreath Energy |
| `StormAnemoiTempest` | the anemoi tempest-lord | Titan | lightning-scarred 0x0492 | 9 | 2300-2400 | 21-26 | Mage | Medium | PoisonImmune | HasBreath Energy |
| `StormAnemoiOutrider` | an anemoi outrider | Harpy | storm grey 0x0492 | 8 | 820-900 | 16-20 | Melee | Fast | — | **Buffet** — OnGaveMelee 20%: knockback msg + 12 stam drain |
| `StormAnemoiVortex` | a howling vortex | Air Elemental | electric blue 0x0480 | 9 | 2150-2380 | 19-24 | Melee | Fast | PoisonImmune | **Static Field** — aura: 4 energy/tick to adjacent |
| `StormAnemoiGale` | a howling gale | Air Elemental | storm grey 0x0492 | 9 | 2200-2400 | 20-25 | Melee | Fast | PoisonImmune | **Windward Recoil** — OnGotMelee 15%: reflect 25% as energy |

### Ambient / fodder (3)
*Small charged flyers above the clouds.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `StormMoth` | a static moth | Mongbat | electric blue 0x0480 | 8 | 780-820 | 14-18 | Melee | Fast | — | — |
| `StormSparrow` | a lightning sparrow | Eagle | Zeus white-gold 0x0481 | 8 | 780-820 | 14-18 | Melee | VeryFast | — | — |
| `StormPuff` | a charged wispling | Wisp | electric blue 0x0480 | 8 | 780-820 | 14-18 | Mage | VeryFast | — | — |

### Mini-boss (1)
*Between Enceladus (elite) and Typhon (boss).*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `StormEphialtes` | Ephialtes, the Storm-Chained | Titan | lightning-scarred 0x0492 | 9 | 2380-2400 | 22-28 | Mage | Medium | BleedImmune + PoisonImmune | **Chain-Lash** — OnGotMelee 15%: reflect 30% as energy + knockback msg; HasBreath Energy |

---

## 5. The Stygian Deep — Underworld / Hades (L9-10) · +28

Existing bag: trash bag8 (L9)/bag9 (L10), elite bag9. Mini-boss → bag9.

### Core family expansion — the unransomed dead (15)
*The ancient dead of the five rivers, ranked deeper toward the crown; the graduation exam — every earlier lesson at once.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `StygianWight` | an unransomed wight | Skeleton | void black 0x0455 | 9 | 2000-2250 | 18-23 | Melee | Medium | — | — |
| `StygianGhoul` | a starving ghoul | Ghoul | bone-violet 0x0454 | 9 | 2000-2250 | 18-23 | Melee | Fast | — | — |
| `StygianMummy` | a bound mummy | Mummy | underworld bronze 0x08A5 | 9 | 2100-2350 | 19-24 | Melee | Slow | Bleed + Poison immune | — |
| `StygianBogle` | a mournful bogle | Bogle | void black 0x0455 | 9 | 2000-2250 | 18-23 | Mage | Medium | PoisonImmune | — |
| `StygianCorpse` | a bloated corpse | Rotting Corpse | bone-violet 0x0454 | 9 | 2100-2350 | 19-24 | Melee | Slow | BleedImmune | HitPoison Deadly |
| `StygianAsphodelKnight` | an asphodel knight | Skeletal Knight | underworld bronze 0x08A5 | 9 | 2100-2350 | 19-24 | Melee | Medium | — | — |
| `StygianStyxMage` | a shade of the styx | Skeletal Mage | Hades violet-black 0x0454 | 9 | 2000-2250 | 18-23 | Mage | Medium | — | — |
| `StygianWraith` | a river wraith | Wraith | void black 0x0455 | 9 | 2000-2250 | 18-23 | Mage | Fast | PoisonImmune | **Lethe Chill** — OnGaveMelee 20%: 15 mana drain |
| `StygianBoneKnight` | a bronze bone knight | Bone Knight | underworld bronze 0x08A5 | 10 | 2600-2900 | 23-29 | Melee | Medium | BleedImmune | — |
| `StygianLich` | an unhallowed lich | Lich | bone-violet 0x0454 | 10 | 2500-2800 | 22-28 | Mage | Medium | — | — |
| `StygianRevenant` | an asphodel revenant | Rotting Corpse | void black 0x0455 | 10 | 2600-2900 | 23-29 | Melee | Slow | BleedImmune | — |
| `StygianBoneColossus` | a bone colossus | Skeletal Dragon | underworld bronze 0x08A5 | 10 | 2800-3100 | 24-30 | Melee | Slow | Bleed + Poison immune | HasBreath Cold ("Grave Breath") |
| `StygianDishound` | a great hound of Dis | Hell Hound | hellfire black 0x0453 | 10 | 2500-2800 | 22-28 | Melee | VeryFast | — | HasBreath Fire; pack instinct |
| `StygianDaemon` | a stygian daemon | Daemon | Hades violet-black 0x0454 | 10 | 2700-3000 | 23-29 | Mage | Medium | PoisonImmune | HasBreath Cold |
| `StygianLichLord` | a lich of Dis | Lich Lord | Hades violet-black 0x0454 | 10 | 2600-2900 | 23-29 | Mage | Medium | PoisonImmune | **Grave Leech** — OnGaveMelee 15%: self-heal 8% max HP |

### Themed block — the Erinyes and the judged dead (9)
*The Furies' court and the damned they scourge; the erinys/scourge/arbiter carry drain/stam/summon.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `StygianDamned` | one of the damned | Zombie | void black 0x0455 | 9 | 2000-2250 | 18-23 | Melee | Slow | BleedImmune | — |
| `StygianWitness` | a silent witness | Bogle | void black 0x0455 | 9 | 2100-2350 | 19-24 | Mage | Medium | PoisonImmune | — |
| `StygianTormentor` | a tormentor shade | Wraith | bone-violet 0x0454 | 10 | 2500-2800 | 22-28 | Mage | VeryFast | — | — |
| `StygianCondemned` | a condemned shade | Shade | void black 0x0455 | 10 | 2500-2800 | 22-28 | Mage | Fast | PoisonImmune | — |
| `StygianJudge` | a judge of the dead | Lich | Hades violet-black 0x0454 | 10 | 2600-2900 | 23-29 | Mage | Medium | PoisonImmune | — |
| `StygianHarrower` | a harrower of souls | Headless One | hellfire black 0x0453 | 10 | 2600-2900 | 23-29 | Melee | Fast | — | — |
| `StygianErinys` | a winged erinys | Spectre | Hades violet-black 0x0454 | 10 | 2600-2900 | 23-29 | Mage | Fast | PoisonImmune | **Vengeance** — OnGaveMelee 20%: 15 mana drain |
| `StygianScourge` | a scourge of the furies | Headless One | hellfire black 0x0453 | 10 | 2600-2900 | 23-29 | Melee | Fast | — | **Flay** — OnGaveMelee 20%: flavor msg + 12 stam drain |
| `StygianArbiter` | the arbiter of asphodel | Lich Lord | Hades violet-black 0x0454 | 10 | 2700-3000 | 23-29 | Mage | Medium | PoisonImmune | **Summon the Judged** — OnGotMelee 12%: spawn 1 `StygianShade` (cap 2, dispel-vulnerable) |

### Ambient / fodder (3)
*Faint dead things adrift on the rivers.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `StygianWisp` | a wisp of lethe | Dark Wisp | void black 0x0455 | 9 | 2000-2200 | 17-22 | Mage | VeryFast | — | — |
| `StygianGraverat` | a grave rat | Giant Rat | bone-violet 0x0454 | 9 | 2000-2200 | 17-22 | Melee | Medium | — | — |
| `StygianShademoth` | a shade moth | Mongbat | void black 0x0455 | 9 | 2000-2200 | 17-22 | Melee | Fast | — | — |

### Mini-boss (1)
*Between Charon/Cerberus (elites) and Hades (boss).*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `StygianRhadamanthys` | Rhadamanthys, the Judge-King | Lich Lord | Hades violet-black 0x0454 | 10 | 3050-3150 | 25-31 | Mage | Medium | PoisonImmune + BleedImmune | **Sentence of the Dead** — OnGaveMelee 25%: 20 mana drain + knockback msg |

*(Renamed from "Minos" on integration — the classic-five bestiary already seats Minos as
Deceit's mini-boss; Rhadamanthys completes the three judges alongside Minos and Aiakos.)*

---

## Totals Appendix

| Dungeon | Core | Themed | Ambient | Mini-boss | **New total** | Custom-ability mobs (≤8) |
|---|---|---|---|---|---|---|
| Drowned Tholos | 15 | 9 | 3 | 1 | **28** | 4 — Bosun, Brineshade, Captain, Nauplios |
| Cinderworks | 15 | 9 | 3 | 1 | **28** | 5 — Crucible, Keryx Warden, Keryx Smith, Keryx Foreman, Kedalion |
| Nemean Wildwood | 15 | 9 | 3 | 1 | **28** | 4 — Thiasos Stalker, Houndmaster, Matron, Prokris |
| Stormcrown Aerie | 15 | 9 | 3 | 1 | **28** | 5 — Ogre King, Anemoi Outrider, Vortex, Gale, Ephialtes |
| Stygian Deep | 15 | 9 | 3 | 1 | **28** | 6 — Wraith, Lich Lord, Erinys, Scourge, Arbiter, Minos |
| **Total** | **75** | **45** | **15** | **5** | **140** | **24** |

**Custom-ability count** = mobs with an `OnGaveMeleeAttack`/`OnGotMeleeAttack` proc or a
damage aura. Breath, HitPoison, pack-instinct and immunities are free stat-block features and
are **not** counted. Every dungeon is at or under the 8-custom budget.

**Deviations from the brief:**
- Split as **15 core / 9 themed / 3 ambient / 1 mini-boss = 28** (brief said ~14/~9/~3/1 =
  27); one extra core member per dungeon lands the exact +28.
- Themed threads renamed to dodge legendary proper-noun collisions (verified against
  `dev-docs/itemization`): the sea crew is the ***Pelagos***; sky winds keep **Anemoi** as the
  faction but drop individual wind-god names (**Boreas/Notos/Euros/Zephyr** are all legendary
  axes), using descriptive displays instead. Mini-boss names **Nauplios / Kedalion / Prokris /
  Ephialtes / Minos** were each grep-checked clear of the legendary registry and existing boss
  classes (**Steropes, Arges, Pallas, Astraios, Alkyoneus, Porphyrion, Kallisto, Britomartis,
  Kyrene** were rejected as collisions).
- Wildwood Huntresses use the **Evil Mage** human body as donor (T2A has no distinct huntress
  body); AI is set per-row (Archer/Mage/Melee) independent of the donor's default.
