# Beast Reference — every custom creature (GENERATED 2026-07-14, post-review)

> Generated from class sources + `LevelConfig.MobLevelOverrides` — ground truth; guard tests
> in `UOContent.Tests/Tests/Engines/Leveling/BestiaryGuardTests.cs` enforce pin coverage +
> bag/pin agreement. Spawn one: `[Beast <class>` · spawner: `[BeastSpawner <class> [count] [min]`.

**Totals: 962 creatures, 63 named elites/bosses. All spawnable, all pinned (test-enforced), zero duplicate display names.**

- **Newbie Barrow** — 15 creatures
- **Argus** — 35 creatures
- **Cinderworks** — 35 creatures
- **Classic Five — elites & brood** — 10 creatures
- **Classic Five — Deceit** — 33 creatures
- **Classic Five — Despise** — 33 creatures
- **Classic Five — Destard** — 33 creatures
- **Classic Five — Hythloth** — 33 creatures
- **Classic Five — Shame** — 33 creatures
- **Cursed** — 35 creatures
- **DrownedTholos** — 35 creatures
- **Lykai** — 35 creatures
- **Myrmi** — 35 creatures
- **NemeanWildwood** — 35 creatures
- **Ophian** — 35 creatures
- **Pelasg** — 35 creatures
- **Pyre** — 35 creatures
- **Rime** — 35 creatures
- **StormcrownAerie** — 35 creatures
- **StygianDeep** — 36 creatures
- **Wayman** — 35 creatures
- **Open World — biomes** — 255 creatures
- **Open World — The Labors** — 6 creatures

## Newbie Barrow (15)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `NewbieBarrowBat` | a barrow bat | 1 | 22–32 | 1–3 | Melee | 39 | 0x0455 | — | — | bag 0 |
| `NewbieBoneBowman` | a bone bowman | 1 | 30–45 | 2–4 | Archer | Utility.RandomList | 0x0482 | poison-immune, bleed-immune | — | bag 0 |
| `NewbieBoneShade` | a frail skeleton | 1 | 25–35 | 2–4 | Melee | Utility.RandomList | 0x03B2 | poison-immune, bleed-immune | — | bag 0 |
| `NewbieCorpseCrawler` | a shambling corpse | 1 | 30–45 | 2–5 | Melee | 3 | — | poison-immune, bleed-immune | — | bag 0 |
| `NewbieGraveRat` | a barrow rat | 1 | 20–30 | 1–3 | Melee | 0xD7 | 0x0481 | — | — | bag 0 |
| `NewbieChanter` | a barrow chanter | 2 | 66–80 | 2–5 | Mage | 148 | 0x047E | poison-immune, bleed-immune | — | bag 1 |
| `NewbieGraveArcher` | a grave archer | 2 | 66–82 | 3–6 | Archer | Utility.RandomList | 0x0385 | poison-immune, bleed-immune | — | bag 1 |
| `NewbieGraveMiasma` | a grave-touched ghoul | 2 | 70–90 | 3–6 | Melee | 153 | 0x0851 | venom Lesser, poison-immune, bleed-immune | — | bag 1 |
| `NewbieMourner` | a forgotten mourner | 2 | 60–78 | 2–5 | Mage | 26 | 0x0455 | poison-immune, bleed-immune | — | bag 1 |
| `NewbieRestlessArcher` | a restless dead | 2 | 66–85 | 3–6 | Melee | Utility.RandomList | 0x0385 | poison-immune, bleed-immune | — | bag 1 |
| `NewbieWight` | an unremembered wight | 2 | 90–100 | 4–7 | Melee | 153 | 0x0455 | poison-immune, bleed-immune | — | bag 1 |
| `NewbieCharon` | the Boatless Ferryman | 3 | 180–220 | 8–14 | Melee | 31 | 0x0455 | — | — | elite bag None |
| `NewbieFallenChampion` | **Anax, the Unyielded** | 3 | 200–240 | 10–16 | Melee | 57 | 0x0021 | bleed-immune | — | elite bag None |
| `NewbieHollowWarden` | the Warden of the Gate | 3 | 170–200 | 7–12 | Mage | 148 | 0x08A5 | poison-immune, bleed-immune | — | elite bag None |
| `NewbieFerryman` | **the Ferryman's Shade** | · | — | — | Animal | 31 | 0x0455 | — | — | level-scaled roll |

## Argus (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `ArgusEye` | a watching eye | 3 | 150–160 | 8–11 | Mage | 22 | 0x0486 | — | — | bag 2 |
| `ArgusGilded` | a gilded thrall | 3 | 130–160 | 7–10 | Melee | 31 | 0x0479 | — | — | bag 2 |
| `ArgusGoldrat` | a gilded rat | 3 | 130–155 | 6–9 | Melee | 0xD7 | 0x0479 | — | — | bag 2 |
| `ArgusMote` | a gazing mote | 3 | 130–160 | 7–10 | Mage | 778 | 0x0486 | — | — | bag 2 |
| `ArgusMoteling` | a drifting mote | 3 | 130–155 | 6–9 | Mage | 778 | 0x0486 | — | — | bag 2 |
| `ArgusOoze` | a gilded ooze | 3 | 130–160 | 6–9 | Melee | 51 | 0x0479 | poison-immune | — | bag 2 |
| `ArgusScryeye` | a scrying eye | 3 | 130–160 | 7–10 | Mage | 778 | 0x0486 | — | — | bag 2 |
| `ArgusSnare` | a vault snare | 3 | 130–160 | 7–10 | Melee | 8 | 0x08A5 | poison-immune | — | bag 2 |
| `ArgusThrall` | a hoard-thrall | 3 | 130–160 | 7–10 | Melee | 31 | 0x0479 | — | — | bag 2 |
| `ArgusVaultmoth` | a vault moth | 3 | 130–155 | 6–9 | Melee | 39 | 0x08A5 | — | — | bag 2 |
| `ArgusCoinwraith` | a coin wraith | 4 | 200–240 | 8–11 | Mage | 26 | 0x0479 | — | — | bag 3 |
| `ArgusDeadhunter` | a bound treasure-hunter | 4 | 200–240 | 8–11 | Melee | Utility.RandomList | 0x08A5 | poison-immune, bleed-immune | — | bag 3 |
| `ArgusGildedhound` | a gilded hound | 4 | 200–240 | 8–11 | Melee | 98 | 0x0479 | pack | — | bag 3 |
| `ArgusHarpy` | a vault harpy | 4 | 200–240 | 8–11 | Melee | 30 | 0x0798 | — | — | bag 3 |
| `ArgusStoneEye` | a stone-eyed harpy | 4 | 200–240 | 8–11 | Melee | 73 | 0x08A5 | — | — | bag 3 |
| `ArgusTelchineApprentice` | a Telchine apprentice | 4 | 200–235 | 8–11 | Mage | 778 | 0x08A5 | — | — | bag 3 |
| `ArgusVaultguard` | a vault guardian | 4 | 200–240 | 8–11 | Melee | 147 | 0x08A5 | bleed-immune | — | bag 3 |
| `ArgusGildspider` | a gilded vault-spider | 5 | 320–360 | 10–14 | Melee | 11 | 0x0798 | venom Deadly, poison-immune | — | bag 4 |
| `ArgusHoardknight` | a hoard-bound knight | 5 | 320–360 | 10–14 | Melee | 57 | 0x08A5 | bleed-immune | — | bag 4 |
| `ArgusTelchineSmith` | a Telchine gild-smith | 5 | 320–360 | 10–14 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | bag 4 |
| `ArgusTelchineWarden` | a Telchine vault-warden | 5 | 320–360 | 10–14 | Melee | 73 | 0x08A5 | — | — | bag 4 |
| `ArgusVaultspider` | a vault spider | 5 | 320–360 | 10–14 | Melee | 11 | 0x0486 | venom Deadly, poison-immune | — | bag 4 |
| `ArgusWatcher` | a hundred-eyed watcher | 5 | 320–360 | 10–14 | Mage | 22 | 0x0486 | — | — | bag 4 |
| `ArgusGoldwyrm` | a gold-cursed wyrm | 6 | 460–520 | 12–16 | Melee | 62 | 0x0479 | breath:FireBreath | — | bag 5 |
| `ArgusHoardmage` | a hoard-cursed lich | 6 | 460–510 | 12–16 | Mage | 24 | 0x0486 | poison-immune, bleed-immune | — | bag 5 |
| `ArgusOverseer` | an all-seeing overseer | 6 | 460–510 | 12–16 | Mage | 22 | 0x0479 | — | when-struck proc | bag 5 |
| `ArgusTelchineGuard` | a Telchine bronze-guard | 6 | 460–520 | 12–16 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | bag 5 |
| `ArgusTelchineHexer` | a Telchine blight-hexer | 6 | 460–510 | 12–16 | Mage | 24 | 0x0486 | poison-immune, bleed-immune | — | bag 5 |
| `ArgusTelchineSeer` | a Telchine evil-eye seer | 6 | 460–510 | 12–16 | Mage | 22 | 0x0486 | — | — | bag 5 |
| `ArgusEyetyrant` | an eye-tyrant | 7 | 600–660 | 14–18 | Mage | 22 | 0x0486 | — | — | bag 6 |
| `ArgusMidas` | **Midas, the Gilded** | 7 | 700–720 | 16–22 | Mage | 79 | 0x0479 | poison-immune, bleed-immune | on-hit proc | elite bag 7 |
| `ArgusTelchineBlight` | a Telchine blight-caster | 7 | 600–660 | 14–18 | Mage | 22 | 0x0479 | poison-immune | aura | bag 6 |
| `ArgusTelchineGildmage` | a Telchine gild-mage | 7 | 610–670 | 14–18 | Mage | 24 | 0x0479 | poison-immune, bleed-immune | on-hit proc | bag 6 |
| `ArgusErysichthon` | **Erysichthon, the Ever-Hungering** | 8 | 900–950 | 18–24 | Melee | Utility.RandomList | 0x0479 | bleed-immune, breath:FireBreath | on-hit proc | elite bag 8 |
| `ArgusTelchineOverwarden` | the Telchine over-warden | 8 | 900–950 | 18–24 | Mage | 22 | 0x0479 | — | when-struck proc | bag 7 |

## Cinderworks (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `CinderEmberling` | an emberling | 5 | 260–310 | 8–12 | Mage | 74 | 0x0655 | breath:FireBreath | — | bag 4 |
| `CinderEmberrat` | an ember rat | 5 | 260–290 | 7–10 | Melee | 0xD7 | 0x0655 | — | — | bag 4 |
| `CinderHauler` | a bronze hauler | 5 | 300–350 | 10–14 | Melee | Utility.RandomList | 0x0798 | — | — | bag 4 |
| `CinderImp` | a forge imp | 5 | 260–320 | 8–12 | Mage | 74 | 0x0655 | breath:FireBreath | — | bag 4 |
| `CinderKeryxCourier` | a keryx courier | 5 | 300–350 | 9–13 | Mage | 74 | 0x0798 | — | — | bag 4 |
| `CinderMoth` | a molten mongrel | 5 | 260–300 | 8–11 | Melee | 39 | 0x0655 | — | — | bag 4 |
| `CinderMothling` | a cinder moth | 5 | 260–290 | 7–10 | Melee | 39 | 0x0655 | — | — | bag 4 |
| `CinderSalamander` | a forge salamander | 5 | 280–330 | 9–13 | Melee | 0xCE | 0x0654 | poison-immune, breath:FireBreath | — | bag 4 |
| `CinderScorial` | a cinder scorpion | 5 | 260–310 | 8–12 | Melee | 48 | 0x0967 | venom Greater | — | bag 4 |
| `CinderSlaghound` | a slag hound | 5 | 280–330 | 9–13 | Melee | 98 | 0x0966 | breath:FireBreath, pack | — | bag 4 |
| `CinderSlagling` | a slagling | 5 | 260–290 | 7–10 | Melee | 51 | 0x0964 | poison-immune, bleed-immune | — | bag 4 |
| `CinderStoker` | a slag stoker | 5 | 300–350 | 10–14 | Melee | 1 | 0x0964 | poison-immune | — | bag 4 |
| `CinderThrall` | a slag thrall | 5 | 300–350 | 10–14 | Melee | 18 | 0x0964 | poison-immune | on-hit proc | bag 4 |
| `CinderAutomaton` | a bronze automaton | 6 | 500–550 | 13–17 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | aura | bag 5 |
| `CinderBronzeOgre` | a bronze ogre lord | 6 | 500–550 | 14–18 | Melee | 83 | 0x0798 | — | — | bag 5 |
| `CinderCrucible` | a crucible construct | 6 | 500–550 | 13–17 | Melee | 752 | 0x0654 | poison-immune, bleed-immune | aura | bag 5 |
| `CinderCyclops` | **Brontes, the Last Cyclops** | 6 | 540–550 | 15–19 | Melee | 75 | 0x0967 | bleed-immune | on-hit proc | elite bag 6 |
| `CinderForgewright` | a forge wright | 6 | 500–550 | 13–17 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | bag 5 |
| `CinderGargoyle` | a slag gargoyle | 6 | 460–520 | 12–16 | Melee | 4 | 0x0966 | — | when-struck proc | bag 5 |
| `CinderGrelt` | a slag grotesque | 6 | 460–510 | 12–16 | Melee | 67 | 0x0966 | poison-immune, bleed-immune | — | bag 5 |
| `CinderHeart` | **Kelmion, the Bellows-Heart** | 6 | 545–550 | 16–20 | Mage | 9 | 0x0669 | poison-immune, breath:FireBreath | when-struck proc | elite bag 6 ×2 |
| `CinderKedalion` | **Kedalion, the Bellows-Bound** | 6 | 545–550 | 15–19 | Mage | 131 | 0x0669 | poison-immune, breath:FireBreath | on-hit proc | elite bag 6 |
| `CinderKeryx` | a bronze keryx | 6 | 500–550 | 13–17 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | bag 5 |
| `CinderKeryxBellows` | a keryx bellows-tender | 6 | 480–530 | 12–16 | Melee | 15 | 0x0654 | poison-immune, breath:FireBreath | — | bag 5 |
| `CinderKeryxForeman` | the keryx foreman | 6 | 510–550 | 14–18 | Melee | 83 | 0x0798 | — | on-hit proc | bag 5 |
| `CinderKeryxHerald` | the keryx herald | 6 | 470–520 | 12–16 | Mage | 4 | 0x0798 | — | — | bag 5 |
| `CinderKeryxSapper` | a keryx sapper | 6 | 500–550 | 13–17 | Melee | 18 | 0x0798 | — | — | bag 5 |
| `CinderKeryxSentry` | a keryx sentry | 6 | 490–540 | 13–17 | Melee | 67 | 0x0798 | poison-immune, bleed-immune | — | bag 5 |
| `CinderKeryxSmith` | a keryx smith | 6 | 500–550 | 13–17 | Melee | 752 | 0x0967 | poison-immune, bleed-immune | on-hit proc | bag 5 |
| `CinderKeryxWarden` | a keryx warden | 6 | 490–540 | 13–17 | Melee | 67 | 0x0798 | poison-immune, bleed-immune | when-struck proc | bag 5 |
| `CinderKindler` | a forge kindler | 6 | 480–540 | 13–17 | Mage | 131 | 0x0655 | breath:FireBreath | — | bag 5 |
| `CinderMoltling` | a molten elemental | 6 | 500–550 | 13–17 | Melee | 15 | 0x0654 | poison-immune, breath:FireBreath | — | bag 5 |
| `CinderPyreling` | a pyre gargoyle | 6 | 460–520 | 12–16 | Mage | 130 | 0x0655 | breath:FireBreath | — | bag 5 |
| `CinderSentinel` | a forge sentinel | 6 | 500–550 | 13–17 | Melee | 15 | 0x0654 | poison-immune, breath:FireBreath | — | bag 5 |
| `CinderSlagDaemon` | a slag daemon | 6 | 500–550 | 13–17 | Mage | 9 | 0x0967 | poison-immune, breath:FireBreath | — | bag 5 |

## Classic Five — elites & brood (10)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `Enkelados` | **Enkelados, the Buried Giant** | 5 | 300–360 | 14–20 | Melee | 75 | 0x0972 | — | when-struck proc, spawns adds | elite bag 5 |
| `Aiakos` | **Aiakos, the Drowned Judge** | 6 | 380–450 | 20–26 | Mage | 24 | 0x0835 | poison-immune, bleed-immune | on-hit proc, when-struck proc, spawns adds | elite bag 6 |
| `Thaumas` | **Thaumas, the Wavebreaker** | 7 | 480–540 | 15–20 | Mage | 16 | 0x04F8 | bleed-immune | when-struck proc, aura, spawns adds | elite bag 7 |
| `Ladon` | **Ladon, the Sleepless Wyrm** | 8 | 620–700 | 18–24 | Mage | Utility.RandomList | 0x0501 | breath:LadonBreath | when-struck proc, spawns adds | elite bag 8 |
| `Alastor` | **Alastor, the Tormentor** | 9 | 780–900 | 24–30 | Mage | 40 | 0x0021 | poison-immune | on-hit proc, when-struck proc, spawns adds | elite bag 9 |
| `AshenBasilisk` | — | · | — | — | — | — | — | venom Deadly | aura | level-scaled roll |
| `BrineSerpent` | — | · | — | — | — | — | — | venom Regular, breath:ColdBreath | — | level-scaled roll |
| `DrakescalePython` | — | · | — | — | — | — | — | venom Greater, breath:FireBreath | — | level-scaled roll |
| `GigasAdder` | — | · | — | — | — | — | — | venom Lesser | on-hit proc | level-scaled roll |
| `GraveAsp` | — | · | — | — | — | — | — | venom Regular | on-hit proc | level-scaled roll |

## Classic Five — Deceit (33)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `DrownedBrackishHulk` | a brackish hulk | 4 | 215–225 | 10–15 | Melee | 155 | 0x0830 | poison-immune, bleed-immune | — | bag 3 |
| `DrownedBrinerot` | a mass of brine-rot | 4 | 185–205 | 9–14 | Melee | 51 | 0x0830 | poison-immune, bleed-immune | — | bag 3 |
| `DrownedDead` | a drowned dead | 4 | 195–205 | 11–16 | Melee | 3 | 0x0835 | poison-immune, bleed-immune | — | bag 3 |
| `DrownedFloater` | a drowned floater | 4 | 185–205 | 10–15 | Melee | 153 | 0x0830 | bleed-immune | — | bag 3 |
| `DrownedGraveEel` | a grave eel | 4 | 180–200 | 9–14 | Melee | 52 | 0x0841 | venom Regular, bleed-immune | — | bag 3 |
| `DrownedHaunt` | a drowned haunt | 4 | 200–220 | 10–15 | Mage | 26 | 0x0835 | bleed-immune | — | bag 3 |
| `DrownedMournling` | a mournling | 4 | 185–195 | 9–14 | Melee | 153 | 0x0835 | bleed-immune | — | bag 3 |
| `DrownedRower` | a drowned rower | 4 | 180–200 | 10–15 | Melee | Utility.RandomList | 0x0841 | bleed-immune | — | bag 3 |
| `DrownedSailor` | a drowned sailor | 4 | 190–210 | 11–16 | Melee | 3 | 0x0835 | poison-immune, bleed-immune | — | bag 3 |
| `DrownedShade` | a drowned shade | 4 | 205–215 | 10–15 | Mage | 26 | 0x0835 | bleed-immune | — | bag 3 |
| `DrownedSilt` | a silt-choked corpse | 4 | 210–230 | 11–16 | Melee | 155 | 0x0830 | poison-immune, bleed-immune | — | bag 3 |
| `DrownedBanshee` | a drowned banshee | 5 | 320–340 | 13–18 | Mage | 26 | 0x0841 | bleed-immune | — | bag 4 |
| `DrownedKeen` | a keening dead | 5 | 330–350 | 14–19 | Mage | 153 | 0x0847 | bleed-immune | — | bag 4 |
| `DrownedLamentor` | a drowned lamentor | 5 | 355–365 | 14–19 | Mage | 153 | 0x0847 | bleed-immune | when-struck proc | bag 4 |
| `DrownedLegionary` | a drowned legionary | 5 | 335–345 | 15–20 | Melee | 147 | 0x0841 | poison-immune, bleed-immune | — | bag 4 |
| `DrownedMarine` | a sea-taken marine | 5 | 330–350 | 15–20 | Melee | 147 | 0x0841 | poison-immune, bleed-immune | — | bag 4 |
| `DrownedNecromage` | a drowned necromage | 5 | 320–340 | 13–18 | Mage | 148 | 0x0835 | poison-immune, bleed-immune | — | bag 4 |
| `DrownedNostosArcher` | a Nostoi archer | 5 | 290–320 | 12–17 | Archer | Utility.RandomList | 0x0830 | bleed-immune | — | bag 4 |
| `DrownedNostosBosun` | the Nostoi bosun | 5 | 330–355 | 14–19 | Melee | 147 | 0x0841 | poison-immune, bleed-immune | on-hit proc | bag 4 |
| `DrownedNostosCurser` | a Nostoi curser | 5 | 320–345 | 13–18 | Mage | 148 | 0x0847 | poison-immune, bleed-immune | on-hit proc | bag 4 |
| `DrownedNostosDeckhand` | a Nostoi deckhand | 5 | 300–330 | 12–17 | Melee | 3 | 0x0835 | poison-immune, bleed-immune | — | bag 4 |
| `DrownedNostosNavigator` | the Nostoi navigator | 5 | 300–330 | 12–17 | Mage | 26 | 0x0835 | poison-immune, bleed-immune | — | bag 4 |
| `DrownedNostosOarsman` | a Nostoi oarsman | 5 | 290–320 | 12–17 | Melee | Utility.RandomList | 0x0841 | bleed-immune | — | bag 4 |
| `DrownedOathbreaker` | a drowned oathbreaker | 5 | 370–380 | 17–22 | Melee | 57 | 0x0847 | poison-immune, bleed-immune | on-hit proc | bag 4 |
| `DrownedReaver` | a drowned reaver | 5 | 350–370 | 16–21 | Melee | 57 | 0x0847 | poison-immune, bleed-immune | — | bag 4 |
| `DrownedSaltMummy` | a salt-crusted mummy | 5 | 340–360 | 15–20 | Melee | 154 | 0x0830 | poison-immune, bleed-immune | — | bag 4 |
| `DrownedTidewraith` | a tide-wraith | 5 | 320–340 | 13–18 | Mage | 26 | 0x0835 | poison-immune, bleed-immune | — | bag 4 |
| `DrownedWailer` | a drowned wailer | 5 | 325–335 | 13–18 | Mage | 26 | 0x0841 | bleed-immune | — | bag 4 |
| `Minos` | **Minos, the Pale Arbiter** | 5 | 370–380 | 14–19 | Mage | 24 | 0x0835 | poison-immune, bleed-immune | when-struck proc, spawns adds | elite bag 5 |
| `DrownedDeepGaunt` | a deep gaunt | 6 | 460–490 | 16–21 | Melee | 57 | 0x0847 | poison-immune, bleed-immune | — | bag 5 |
| `DrownedHarbinger` | a drowned harbinger | 6 | 470–500 | 16–21 | Melee | 147 | 0x0830 | poison-immune, bleed-immune | — | bag 5 |
| `DrownedPhrontis` | **Phrontis, the Unreturned** | 6 | 490–510 | 16–21 | Melee | 57 | 0x0835 | poison-immune, bleed-immune | when-struck proc, spawns adds | elite bag 5 |
| `DrownedWrecklich` | a wreck-lich | 6 | 460–490 | 15–20 | Mage | 24 | 0x0835 | poison-immune, bleed-immune | — | bag 5 |

## Classic Five — Despise (33)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `GaianClayServitor` | a clay servitor | 3 | 145–155 | 8–13 | Melee | 752 | 0x0972 | poison-immune, bleed-immune | — | bag 2 |
| `GaianClayborn` | a gaian clayborn | 3 | 150–160 | 11–16 | Melee | 1 | 0x09C2 | — | — | bag 2 |
| `GaianClodling` | a gaian clodling | 3 | 130–145 | 8–13 | Melee | 42 | 0x0972 | — | — | bag 2 |
| `GaianDustadder` | a dust-adder | 3 | 115–135 | 6–11 | Melee | 52 | 0x09C2 | venom Lesser | — | bag 2 |
| `GaianFurrowborn` | a furrow-born | 3 | 140–155 | 9–14 | Melee | Utility.RandomList | 0x09C4 | — | — | bag 2 |
| `GaianRootcrawler` | a root-crawler | 3 | 135–150 | 8–13 | Melee | 28 | 0x09C2 | venom Lesser | — | bag 2 |
| `GaianRubblecrawler` | a rubble-crawler | 3 | 120–140 | 7–12 | Melee | 51 | 0x0972 | poison-immune, bleed-immune | — | bag 2 |
| `GaianSownSeed` | a sown seed | 3 | 115–125 | 6–11 | Melee | 0x15 | 0x09C2 | poison-immune | — | bag 2 |
| `GaianSpartos` | a gaian spartos | 3 | 145–155 | 9–14 | Melee | Utility.RandomList | 0x0972 | — | — | bag 2 |
| `Chthonios` | **Chthonios, the First-Sown** | 4 | 230–240 | 14–20 | Melee | 75 | 0x0972 | poison-immune | when-struck proc, spawns adds | elite bag 4 |
| `GaianBoulderback` | a boulder-back | 4 | 220–238 | 13–18 | Melee | 1 | 0x09C2 | — | — | bag 3 |
| `GaianClaymason` | a clay mason | 4 | 205–225 | 12–17 | Melee | 752 | 0x0972 | poison-immune, bleed-immune | — | bag 3 |
| `GaianDelver` | a gaian delver | 4 | 195–215 | 12–17 | Melee | Utility.RandomList | 0x0455 | — | — | bag 3 |
| `GaianEarthbloodChampion` | a gaian earthblood champion | 4 | 228–238 | 14–19 | Melee | 18 | 0x0972 | pack | — | bag 3 |
| `GaianEarthwyrm` | a gaian earth-wyrm | 4 | 195–215 | 11–16 | Melee | 21 | 0x09C2 | venom Regular, poison-immune | — | bag 3 |
| `GaianGegenesDigger` | a gegenes digger | 4 | 210–230 | 12–17 | Melee | Utility.RandomList | 0x09C2 | — | — | bag 3 |
| `GaianGegenesThrall` | a gegenes thrall | 4 | 220–238 | 13–18 | Melee | 18 | 0x09C2 | — | — | bag 3 |
| `GaianGraniteAdder` | a granite adder | 4 | 200–220 | 12–17 | Melee | 48 | 0x0455 | venom Greater, poison-immune | — | bag 3 |
| `GaianPhalangite` | a gaian phalangite | 4 | 208–218 | 13–18 | Melee | 18 | 0x0455 | — | — | bag 3 |
| `GaianQuarrybrute` | a quarry brute | 4 | 215–235 | 13–18 | Melee | 18 | 0x0455 | — | — | bag 3 |
| `GaianSlinger` | a gaian slinger | 4 | 190–210 | 11–16 | Archer | 42 | 0x09C4 | — | — | bag 3 |
| `GaianSpearborn` | a gaian spearborn | 4 | 195–205 | 12–17 | Melee | Utility.RandomList | 0x09C4 | — | — | bag 3 |
| `GaianStoneshaper` | a gaian stoneshaper | 4 | 190–210 | 11–16 | Mage | 4 | 0x0455 | — | — | bag 3 |
| `GaianTitanWard` | a gaian titan-ward | 4 | 226–236 | 13–18 | Melee | 1 | 0x0455 | — | when-struck proc | bag 3 |
| `GaianClayColossus` | a clay colossus | 5 | 320–350 | 15–20 | Melee | 75 | 0x0972 | — | — | bag 4 |
| `GaianGegenesElder` | a gegenes elder | 5 | 350–370 | 16–21 | Melee | 83 | 0x0455 | pack | — | bag 4 |
| `GaianGegenesHurler` | a gegenes hurler | 5 | 320–350 | 15–20 | Archer | 75 | 0x0455 | — | — | bag 4 |
| `GaianGegenesShaker` | a gegenes earth-shaker | 5 | 330–360 | 15–20 | Melee | 14 | 0x0455 | poison-immune | on-hit proc | bag 4 |
| `GaianGegenesWarden` | a gegenes warden | 5 | 340–365 | 15–20 | Melee | 67 | 0x0455 | poison-immune, bleed-immune | when-struck proc | bag 4 |
| `GaianMountainborn` | a mountain-born | 5 | 330–360 | 15–20 | Melee | 83 | 0x0455 | — | — | bag 4 |
| `GaianPeloreus` | **Peloreus, the Unearthed** | 5 | 365–380 | 16–21 | Melee | 76 | 0x0972 | poison-immune | when-struck proc, spawns adds | elite bag 4 |
| `GaianStonewarden` | a stone warden | 5 | 300–330 | 14–19 | Melee | 67 | 0x0455 | poison-immune, bleed-immune | — | bag 4 |
| `GaianTerraGorger` | a terra-gorger | 5 | 310–340 | 14–19 | Melee | 14 | 0x09C2 | poison-immune | — | bag 4 |

## Classic Five — Destard (33)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `DrakonAcolyte` | a drakon acolyte | 6 | 485–495 | 14–19 | Melee | 86 | 0x0501 | — | — | bag 5 |
| `DrakonAshviper` | an ash viper | 6 | 465–485 | 12–17 | Melee | 52 | 0x066D | venom Regular | — | bag 5 |
| `DrakonBroodviper` | a drakon brood-viper | 6 | 465–485 | 13–18 | Melee | 0x15 | 0x066D | venom Regular | — | bag 5 |
| `DrakonHatchling` | a drakon hatchling | 6 | 470–490 | 15–20 | Melee | Utility.RandomList | 0x0501 | — | — | bag 5 |
| `DrakonLavaAdder` | a lava adder | 6 | 470–490 | 14–19 | Melee | 90 | 0x0489 | breath:FireBreath | — | bag 5 |
| `DrakonOphidianSpear` | a drakon spear | 6 | 480–500 | 14–19 | Melee | 86 | 0x0501 | — | — | bag 5 |
| `DrakonScaleHound` | a scale-hound | 6 | 495–505 | 14–19 | Melee | 730 | 0x0501 | — | — | bag 5 |
| `DrakonScaleraptor` | a scale-raptor | 6 | 490–510 | 14–19 | Melee | 730 | 0x0501 | — | — | bag 5 |
| `DrakonScalerat` | a scale-rat | 6 | 460–480 | 12–17 | Melee | 0xD7 | 0x0501 | — | — | bag 5 |
| `DrakonSerpentling` | a drakon serpentling | 6 | 465–475 | 13–18 | Melee | 0x15 | 0x066D | venom Regular | — | bag 5 |
| `DrakonWhelp` | a drakon whelp | 6 | 475–485 | 15–20 | Melee | Utility.RandomList | 0x0501 | — | — | bag 5 |
| `DrakonDrakeling` | a drakon drakeling | 7 | 660–680 | 18–23 | Melee | Utility.RandomList | 0x066D | — | — | bag 6 |
| `DrakonEmberdrake` | an ember drake | 7 | 665–685 | 19–24 | Melee | Utility.RandomList | 0x0489 | breath:FireBreath | — | bag 6 |
| `DrakonFirewyrm` | a drakon fire-wyrm | 7 | 700–720 | 18–23 | Mage | Utility.RandomList | 0x0489 | breath:FireBreath | — | bag 6 |
| `DrakonFlamespeaker` | a drakon flamespeaker | 7 | 645–655 | 15–20 | Mage | 85 | 0x0489 | — | on-hit proc | bag 6 |
| `DrakonFlamewing` | a drakon flamewing | 7 | 690–700 | 19–24 | Melee | Utility.RandomList | 0x0489 | breath:FireBreath | — | bag 6 |
| `DrakonIsmenianAugur` | an Ismenian augur | 7 | 640–660 | 15–20 | Mage | 85 | 0x066D | — | on-hit proc | bag 6 |
| `DrakonIsmenianCoil` | an Ismenian coil | 7 | 640–660 | 15–20 | Melee | 0x15 | 0x0489 | venom Greater | — | bag 6 |
| `DrakonIsmenianDrake` | an Ismenian drake | 7 | 665–685 | 18–23 | Melee | Utility.RandomList | 0x0489 | breath:FireBreath | — | bag 6 |
| `DrakonIsmenianFang` | an Ismenian fang | 7 | 645–665 | 16–21 | Melee | 86 | 0x066D | — | — | bag 6 |
| `DrakonIsmenianWard` | an Ismenian ward | 7 | 655–675 | 17–22 | Melee | 86 | 0x0489 | — | — | bag 6 |
| `DrakonIsmenianWyrm` | an Ismenian wyrm | 7 | 645–670 | 18–23 | Melee | 62 | 0x0501 | venom Greater | on-hit proc, when-struck proc | bag 6 |
| `DrakonIsmenos` | **Ismenos, the Hoard-Coiled** | 7 | 705–720 | 18–23 | Mage | 103 | 0x0489 | breath:FireBreath | when-struck proc, spawns adds | elite bag 7 |
| `DrakonMatron` | a drakon matron | 7 | 700–720 | 17–22 | Mage | 103 | 0x066D | — | — | bag 6 |
| `DrakonOphidianAvenger` | a drakon avenger | 7 | 650–670 | 17–22 | Melee | 86 | 0x066D | — | — | bag 6 |
| `DrakonOphidianSeer` | a drakon seer | 7 | 640–660 | 15–20 | Mage | 85 | 0x0501 | — | — | bag 6 |
| `DrakonSerpentGuard` | a drakon serpent-guard | 7 | 655–675 | 17–22 | Melee | 86 | 0x0489 | — | — | bag 6 |
| `DrakonWyrmling` | a drakon wyrmling | 7 | 635–645 | 18–23 | Melee | 62 | 0x066D | venom Greater | — | bag 6 |
| `DrakonWyvern` | a drakon wyvern | 7 | 640–660 | 18–23 | Melee | 62 | 0x066D | venom Greater | — | bag 6 |
| `DrakonZealot` | a drakon zealot | 7 | 655–665 | 17–22 | Melee | 86 | 0x066D | — | — | bag 6 |
| `Pythios` | **Pythios, the Cult-Hierarch** | 7 | 705–715 | 18–23 | Mage | 103 | 0x0489 | — | when-struck proc, spawns adds | elite bag 7 |
| `DrakonGreatDrake` | a drakon great-drake | 8 | 850–890 | 20–25 | Mage | Utility.RandomList | 0x066D | breath:FireBreath | — | bag 7 |
| `DrakonHoardWyrm` | a hoard wyrm | 8 | 820–860 | 19–24 | Melee | Utility.RandomBool | 0x0501 | breath:ColdBreath | — | bag 7 |

## Classic Five — Hythloth (33)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `TartarusAshhound` | an ash-hound | 7 | 640–655 | 14–19 | Melee | 98 | 0x0455 | pack | — | bag 6 |
| `TartarusChaosbrand` | a chaos-brand | 7 | 640–660 | 15–20 | Melee | 792 | 0x0022 | — | — | bag 6 |
| `TartarusCinderImp` | a cinder imp | 7 | 635–650 | 15–20 | Mage | 74 | 0x0022 | — | — | bag 6 |
| `TartarusEfreet` | a pit-efreet | 7 | 645–665 | 15–20 | Mage | 131 | 0x0022 | breath:FireBreath | — | bag 6 |
| `TartarusEmberwing` | an ember-wing gargoyle | 7 | 645–665 | 15–20 | Mage | 130 | 0x0022 | breath:FireBreath | — | bag 6 |
| `TartarusGargoyle` | a tartarus gargoyle | 7 | 645–655 | 15–20 | Mage | 4 | 0x0455 | — | — | bag 6 |
| `TartarusGargoyleLord` | a tartarus gargoyle-lord | 7 | 645–665 | 15–20 | Mage | 4 | 0x0455 | — | — | bag 6 |
| `TartarusGoreling` | a goreling | 7 | 650–670 | 16–21 | Melee | 305 | 0x0021 | poison-immune, bleed-immune | — | bag 6 |
| `TartarusHellbrand` | a hellbrand | 7 | 635–645 | 15–20 | Melee | 792 | 0x0022 | — | — | bag 6 |
| `TartarusImp` | a tartarus imp | 7 | 635–645 | 16–21 | Mage | 74 | 0x0021 | — | — | bag 6 |
| `TartarusImpling` | a tartarus impling | 7 | 635–655 | 16–21 | Mage | 74 | 0x0021 | — | — | bag 6 |
| `TartarusPyrehound` | a pyre-hound | 7 | 640–660 | 15–20 | Melee | 98 | 0x0022 | breath:FireBreath, pack | — | bag 6 |
| `TartarusSoulgorger` | a soulgorger | 7 | 650–660 | 16–21 | Melee | 305 | 0x0021 | poison-immune, bleed-immune | — | bag 6 |
| `TartarusTitanWarden` | a titan-warden | 7 | 650–680 | 15–20 | Melee | 18 | 0x0022 | — | — | bag 6 |
| `Eurynomos` | **Eurynomos, the Corpse-Eater** | 8 | 930–940 | 20–26 | Mage | 0x310 | 0x0021 | poison-immune | aura | elite bag 8 |
| `TartarusBrimstone` | a brimstone daemon | 8 | 850–870 | 20–25 | Mage | 9 | 0x0022 | breath:FireBreath | — | bag 7 |
| `TartarusDaemonspawn` | a tartarus daemonspawn | 8 | 840–860 | 20–25 | Mage | 9 | 0x0021 | — | — | bag 7 |
| `TartarusEnforcer` | a tartarus enforcer | 8 | 815–825 | 18–23 | Mage | 0x2F2 | 0x0022 | — | — | bag 7 |
| `TartarusFiend` | a tartarus fiend | 8 | 845–855 | 20–25 | Mage | 9 | 0x0021 | — | — | bag 7 |
| `TartarusHexfiend` | a hex-fiend | 8 | 815–835 | 18–23 | Mage | 0x310 | 0x0021 | — | — | bag 7 |
| `TartarusMenoetius` | **Menoetius, the Chained** | 8 | 930–950 | 21–26 | Mage | 76 | 0x0021 | poison-immune, breath:EnergyBreath | when-struck proc, spawns adds | elite bag 8 |
| `TartarusRimefiend` | a rime-fiend | 8 | 890–910 | 21–26 | Mage | 43 | 0x0455 | breath:ColdBreath | — | bag 7 |
| `TartarusScourgewing` | a scourge-wing | 8 | 820–840 | 18–23 | Mage | 0x2F2 | 0x0022 | — | — | bag 7 |
| `TartarusSlatewarden` | a slate warden | 8 | 875–895 | 19–24 | Melee | 67 | 0x0455 | poison-immune | — | bag 7 |
| `TartarusSoulflayer` | a soulflayer | 8 | 855–875 | 20–25 | Melee | 305 | 0x0022 | poison-immune, bleed-immune | — | bag 7 |
| `TartarusStonewrath` | a stonewrath | 8 | 875–885 | 19–24 | Melee | 67 | 0x0455 | poison-immune | when-struck proc | bag 7 |
| `TartarusSuccubus` | a pit-succubus | 8 | 830–850 | 19–24 | Mage | 149 | 0x0021 | — | — | bag 7 |
| `TartarusTitanBreaker` | a titan-breaker | 8 | 900–930 | 21–26 | Mage | 76 | 0x0022 | breath:EnergyBreath | on-hit proc | bag 7 |
| `TartarusTitanColossus` | a titan colossus | 8 | 900–930 | 21–26 | Melee | 75 | 0x0455 | — | — | bag 7 |
| `TartarusTitanJailer` | a titan-jailer | 8 | 830–855 | 19–24 | Mage | 0x2F2 | 0x0022 | — | when-struck proc | bag 7 |
| `TartarusTitanShackled` | a shackled titan | 8 | 880–910 | 20–25 | Melee | 83 | 0x0455 | pack | — | bag 7 |
| `TartarusTitanThrall` | a chained thrall | 8 | 870–900 | 19–24 | Melee | 75 | 0x0455 | — | — | bag 7 |
| `TartarusTormentFiend` | a torment-fiend | 8 | 895–905 | 21–26 | Mage | 43 | 0x0022 | — | on-hit proc | bag 7 |

## Classic Five — Shame (33)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `BrineDeepcoil` | **a deep-coil serpent** | 5 | 315–340 | 13–18 | Melee | 150 | 0x04F2 | venom Greater | — | bag 4 |
| `BrineFrost` | **a brine frost** | 5 | 300–320 | 12–17 | Melee | 163 | 0x0480 | bleed-immune | — | bag 4 |
| `BrineGale` | **a brine gale** | 5 | 305–315 | 12–17 | Mage | 13 | 0x04F8 | bleed-immune | — | bag 4 |
| `BrineJelly` | **a stinging jelly** | 5 | 295–315 | 11–16 | Melee | 51 | 0x0480 | — | — | bag 4 |
| `BrineLeechEel` | **a leech-eel** | 5 | 315–325 | 12–17 | Melee | 150 | 0x04F2 | — | — | bag 4 |
| `BrinePetrel` | **a storm-petrel** | 5 | 290–310 | 11–16 | Melee | 5 | 0x0481 | — | — | bag 4 |
| `BrineReefcrab` | **a reef crab** | 5 | 300–320 | 12–17 | Melee | 48 | 0x04F2 | venom Greater | — | bag 4 |
| `BrineSeep` | **a brine seep** | 5 | 295–305 | 11–16 | Melee | 163 | 0x0480 | bleed-immune | — | bag 4 |
| `BrineSpray` | **a brine spray** | 5 | 295–315 | 13–18 | Mage | 16 | 0x04F8 | bleed-immune | — | bag 4 |
| `BrineSpume` | **a brine spume** | 5 | 295–305 | 13–18 | Mage | 16 | 0x04F8 | bleed-immune | — | bag 4 |
| `BrineSquallwind` | **a squall-wind** | 5 | 300–320 | 12–17 | Mage | 13 | 0x0481 | bleed-immune | — | bag 4 |
| `BrineStormHarpy` | **a storm-harpy** | 5 | 305–325 | 12–17 | Melee | 30 | 0x0481 | bleed-immune | — | bag 4 |
| `BrineAbyssal` | **an abyssal serpent** | 6 | 490–510 | 16–21 | Melee | 150 | 0x04F2 | breath:ColdBreath | — | bag 5 |
| `BrineCrusher` | **a brine crusher** | 6 | 530–545 | 17–22 | Melee | 77 | 0x04F8 | — | — | bag 5 |
| `BrineCyclone` | **a brine cyclone** | 6 | 480–500 | 15–20 | Mage | 13 | 0x0481 | bleed-immune | — | bag 5 |
| `BrineGlacier` | **a brine glacier** | 6 | 490–510 | 17–22 | Mage | 161 | 0x0480 | bleed-immune | — | bag 5 |
| `BrineMaelstrom` | **a brine maelstrom** | 6 | 530–540 | 18–23 | Mage | 161 | 0x0480 | bleed-immune | aura | bag 5 |
| `BrineRiptide` | **a brine riptide** | 6 | 475–495 | 16–21 | Mage | 16 | 0x04F2 | bleed-immune | — | bag 5 |
| `BrineSquallHarrier` | **a squall-harrier** | 6 | 515–525 | 15–20 | Melee | 30 | 0x0481 | bleed-immune | when-struck proc | bag 5 |
| `BrineSquallHawk` | **a squall-hawk** | 6 | 500–520 | 15–20 | Melee | 73 | 0x0481 | bleed-immune | — | bag 5 |
| `BrineSurge` | **a brine surge** | 6 | 475–485 | 16–21 | Mage | 16 | 0x04F2 | bleed-immune | — | bag 5 |
| `BrineTelchinAdept` | **a telchine adept** | 6 | 480–500 | 15–20 | Mage | 16 | 0x04F8 | bleed-immune | — | bag 5 |
| `BrineTelchinBrinesmith` | **a telchine brine-smith** | 6 | 490–515 | 16–21 | Mage | 161 | 0x0480 | bleed-immune | when-struck proc | bag 5 |
| `BrineTelchinDrowner` | **a telchine drowner** | 6 | 490–510 | 16–21 | Melee | 150 | 0x04F2 | venom Greater | — | bag 5 |
| `BrineTelchinGaler` | **a telchine galer** | 6 | 475–495 | 14–19 | Mage | 13 | 0x0481 | bleed-immune | — | bag 5 |
| `BrineTelchinTideward` | **a telchine tideward** | 6 | 510–530 | 16–21 | Melee | 77 | 0x04F2 | — | on-hit proc | bag 5 |
| `BrineTempest` | **a brine tempest** | 6 | 495–505 | 15–20 | Mage | 13 | 0x0481 | bleed-immune | — | bag 5 |
| `BrineTideElemental` | **a tide elemental** | 6 | 510–530 | 16–21 | Melee | 16 | 0x04F2 | bleed-immune | — | bag 5 |
| `Glaukos` | **Glaukos, the Brine-Shepherd** | 6 | 535–545 | 16–21 | Mage | 16 | 0x04F8 | bleed-immune | when-struck proc, spawns adds | elite bag 6 |
| `BrineHailspite` | **a hailspite** | 7 | 620–655 | 17–22 | Mage | 161 | 0x0480 | bleed-immune | — | bag 6 |
| `BrineOrmenos` | **Ormenos, the Tide-Smith** | 7 | 700–720 | 18–23 | Mage | 77 | 0x04F8 | breath:ColdBreath | on-hit proc | elite bag 6 |
| `BrineTelchinStormcaller` | **a telchine stormcaller** | 7 | 620–650 | 17–22 | Mage | 13 | 0x04F8 | bleed-immune, breath:ColdBreath | — | bag 6 |
| `BrineWaterlord` | **a brine water-lord** | 7 | 640–680 | 18–23 | Mage | 16 | 0x04F8 | bleed-immune | — | bag 6 |

## Cursed (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `CursedDelver` | a cursed delver | 5 | 320–360 | 10–14 | Melee | Utility.RandomList | 0x0842 | — | — | bag 4 |
| `CursedDigger` | a cursed digger | 5 | 320–360 | 10–14 | Melee | 3 | 0x0842 | bleed-immune | — | bag 4 |
| `CursedGravecaller` | a grave-caller | 5 | 320–360 | 10–14 | Mage | 148 | 0x0486 | — | — | bag 4 |
| `CursedGravehound` | a barrow hound | 5 | 300–350 | 9–13 | Melee | 98 | 0x0455 | pack | — | bag 4 |
| `CursedGraverat` | a dig-site rat | 5 | 280–310 | 8–11 | Melee | 0xD7 | 0x0842 | — | — | bag 4 |
| `CursedGraveslime` | a grave slime | 5 | 280–310 | 8–11 | Melee | 51 | 0x0851 | poison-immune, bleed-immune | — | bag 4 |
| `CursedLampadNovice` | a Lampad novice | 5 | 320–360 | 10–14 | Mage | 0x190 | 0x0486 | — | — | bag 4 |
| `CursedPallbearer` | a cursed pallbearer | 5 | 320–360 | 10–14 | Melee | 3 | 0x0842 | bleed-immune | — | bag 4 |
| `CursedShade` | a barrow shade | 5 | 300–350 | 9–13 | Melee | 0xA8 | 0x0455 | — | — | bag 4 |
| `CursedTombmoth` | a tomb moth | 5 | 280–310 | 8–11 | Melee | 39 | 0x0455 | — | — | bag 4 |
| `CursedWight` | a dig-wight | 5 | 320–360 | 10–14 | Melee | Utility.RandomList | 0x0851 | — | — | bag 4 |
| `CursedAccursed` | one of the accursed | 6 | 460–510 | 12–16 | Mage | 0x190 | 0x0851 | poison-immune | — | bag 5 |
| `CursedArmour` | an animate spectral armour | 6 | 460–510 | 12–16 | Melee | 637 | 0x0AA8 | poison-immune, bleed-immune | — | bag 5 |
| `CursedBoneguard` | a cursed bone-guard | 6 | 460–520 | 12–16 | Melee | 57 | 0x0842 | bleed-immune | — | bag 5 |
| `CursedBonemagi` | a bone hierophant | 6 | 460–510 | 12–16 | Mage | 148 | 0x0842 | — | — | bag 5 |
| `CursedGheist` | a cairn geist | 6 | 460–510 | 12–16 | Mage | 26 | 0x0AA8 | poison-immune | — | bag 5 |
| `CursedGravemage` | a barrow magus | 6 | 460–510 | 12–16 | Mage | 24 | 0x0486 | — | — | bag 5 |
| `CursedGravewurm` | a cairn wurm | 6 | 460–510 | 12–16 | Melee | 0x15 | 0x0851 | venom Deadly, poison-immune | — | bag 5 |
| `CursedLampadShade` | a Lampad shade | 6 | 460–510 | 12–16 | Melee | 0xA8 | 0x0455 | — | — | bag 5 |
| `CursedLampadTorch` | a Lampad torchbearer | 6 | 460–510 | 12–16 | Mage | 0x190 | 0x0842 | — | — | bag 5 |
| `CursedLampadWarden` | a Lampad grave-warden | 6 | 460–520 | 12–16 | Melee | 57 | 0x0851 | bleed-immune | — | bag 5 |
| `CursedNecromancer` | a Hecate necromancer | 6 | 460–510 | 12–16 | Mage | 148 | 0x0486 | — | — | bag 5 |
| `CursedSentinel` | a bound sentinel | 6 | 460–520 | 12–16 | Melee | 57 | 0x0851 | bleed-immune | — | bag 5 |
| `CursedDreadknight` | a warded dread-knight | 7 | 600–660 | 14–18 | Melee | 147 | 0x0AA8 | bleed-immune | — | bag 6 |
| `CursedLampadCrossroads` | a crossroads lampad | 7 | 610–670 | 14–18 | Mage | 0x190 | 0x0486 | poison-immune | aura | bag 6 |
| `CursedLampadHex` | a Lampad hexer | 7 | 600–660 | 14–18 | Mage | 0x190 | 0x0AA8 | — | — | bag 6 |
| `CursedLampadPyre` | a Lampad pyre-witch | 7 | 600–660 | 14–18 | Mage | 0x190 | 0x0486 | venom Deadly | — | bag 6 |
| `CursedLampadWailer` | a Lampad wailer | 7 | 600–660 | 14–18 | Mage | 26 | 0x0455 | — | on-hit proc | bag 6 |
| `CursedRevenant` | a warded revenant | 7 | 600–660 | 14–18 | Melee | 147 | 0x0AA8 | bleed-immune | when-struck proc | bag 6 |
| `CursedSummoner` | a crossroads summoner | 7 | 610–670 | 14–18 | Mage | 0x190 | 0x0486 | — | when-struck proc, spawns adds | bag 6 |
| `CursedZealot` | a Hecate zealot | 7 | 600–660 | 14–18 | Mage | 0x190 | 0x0486 | venom Deadly | — | bag 6 |
| `CursedHierophant` | a Hecate hierophant | 8 | 900–950 | 18–24 | Mage | 24 | 0x0486 | — | — | bag 7 |
| `CursedLampadMatron` | the Lampad lantern-matron | 8 | 900–950 | 18–24 | Mage | 78 | 0x0486 | poison-immune | when-struck proc, spawns adds | bag 7 |
| `CursedPerses` | **Perses, the Grave-Titan** | 8 | 900–950 | 19–25 | Mage | 78 | 0x0486 | poison-immune, bleed-immune | on-hit proc | elite bag 8 |
| `CursedAeetes` | **Aeetes, the Buried Oracle** | 9 | 2200–2400 | 20–26 | Mage | 78 | 0x0486 | poison-immune, bleed-immune | on-hit proc | elite bag 9 |

## DrownedTholos (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `TideCrabling` | a scuttling crab | 2 | 90–120 | 5–8 | Melee | 48 | 0x0530 | — | — | bag 1 |
| `TideGull` | a brine gull | 2 | 80–110 | 4–7 | Melee | 39 | 0x0481 | — | — | bag 1 |
| `TideJelly` | a drifting jelly | 2 | 80–110 | 4–7 | Melee | 51 | 0x0481 | venom Lesser, poison-immune | — | bag 1 |
| `TideBrinescale` | a brinescale serpent | 4 | 100–140 | 5–8 | Melee | 150 | 0x0481 | venom Regular | — | bag 3 |
| `TideConscript` | a drowned conscript | 4 | 110–150 | 6–9 | Melee | 3 | 0x0847 | bleed-immune | — | bag 3 |
| `TideDiver` | a drowned diver | 4 | 120–160 | 7–10 | Melee | 153 | 0x0481 | — | — | bag 3 |
| `TideDrudge` | a drowned oarsman | 4 | 120–160 | 6–9 | Melee | 3 | 0x0847 | bleed-immune | on-hit proc | bag 3 |
| `TideEel` | a coiling eel | 4 | 100–140 | 6–9 | Melee | 52 | 0x0481 | venom Regular | — | bag 3 |
| `TideLeech` | a brine leech | 4 | 90–130 | 5–8 | Melee | 51 | 0x0530 | venom Lesser, poison-immune | — | bag 3 |
| `TideRower` | a barnacled rower | 4 | 100–140 | 6–9 | Melee | 50 | 0x08A5 | bleed-immune | — | bag 3 |
| `TideAbyssEel` | a deep serpent | 5 | 320–380 | 11–15 | Melee | 150 | 0x0530 | breath:ColdBreath | — | bag 4 |
| `TideBloated` | a bloated drowned | 5 | 340–380 | 12–16 | Melee | 155 | 0x0847 | poison-immune, bleed-immune | — | bag 4 |
| `TideCoralguard` | a coral guard | 5 | 320–380 | 11–15 | Melee | 16 | 0x0532 | poison-immune | — | bag 4 |
| `TideHerald` | **Aigaion, the Drowned Herald** | 5 | 370–380 | 14–18 | Melee | 77 | 0x0850 | poison-immune, bleed-immune, breath:ColdBreath | on-hit proc | elite bag 5 ×2 |
| `TideHexer` | a drowned hexer | 5 | 280–330 | 9–13 | Mage | 148 | 0x0532 | poison-immune, bleed-immune | — | bag 4 |
| `TideHoplite` | a barnacled hoplite | 5 | 300–360 | 10–14 | Melee | 57 | 0x08A5 | bleed-immune, pack | — | bag 4 |
| `TideMarine` | a drowned marine | 5 | 300–350 | 10–14 | Melee | 57 | 0x08A5 | bleed-immune | — | bag 4 |
| `TideMaw` | a whirling maw | 5 | 280–340 | 9–13 | Melee | 16 | 0x0530 | — | on-hit proc, aura | bag 4 |
| `TideNavarch` | **Nauplios, the Wreckwarden** | 5 | 375–380 | 14–18 | Melee | 150 | 0x0850 | bleed-immune, breath:ColdBreath | on-hit proc | elite bag 5 |
| `TidePelagosBosun` | the Pelagos bosun | 5 | 320–360 | 11–15 | Melee | 57 | 0x08A5 | — | on-hit proc | bag 4 |
| `TidePelagosBrineshade` | a Pelagos brineshade | 5 | 280–330 | 9–13 | Mage | 26 | 0x0530 | — | on-hit proc | bag 4 |
| `TidePelagosCaptain` | the Pelagos captain | 5 | 340–375 | 12–16 | Melee | 57 | 0x0851 | bleed-immune | on-hit proc, when-struck proc | bag 4 |
| `TidePelagosDeckhand` | a Pelagos deckhand | 5 | 300–350 | 10–14 | Melee | 3 | 0x0847 | bleed-immune | — | bag 4 |
| `TidePelagosDrummer` | a Pelagos drummer | 5 | 280–320 | 9–13 | Melee | 50 | 0x08A5 | bleed-immune | — | bag 4 |
| `TidePelagosHarpooner` | a Pelagos harpooner | 5 | 280–320 | 9–13 | Archer | 50 | 0x0481 | bleed-immune | — | bag 4 |
| `TidePelagosLookout` | a Pelagos lookout | 5 | 280–330 | 9–13 | Melee | 153 | 0x0481 | — | — | bag 4 |
| `TidePelagosNavigator` | the Pelagos navigator | 5 | 280–330 | 9–13 | Mage | 26 | 0x0532 | poison-immune | — | bag 4 |
| `TidePelagosOarmaster` | the Pelagos oarmaster | 5 | 330–370 | 11–15 | Melee | 147 | 0x08A5 | bleed-immune, pack | — | bag 4 |
| `TidePikeman` | a barnacled pikeman | 5 | 320–370 | 11–15 | Melee | 147 | 0x08A5 | bleed-immune, pack | — | bag 4 |
| `TideReefserpent` | a reef serpent | 5 | 300–360 | 10–14 | Melee | 150 | 0x0851 | venom Greater | — | bag 4 |
| `TideSentinel` | a coral sentinel | 5 | 320–380 | 10–14 | Melee | 16 | 0x0532 | poison-immune, breath:ColdBreath | — | bag 4 |
| `TideSpinecrab` | a spinecrab | 5 | 300–350 | 10–14 | Melee | 48 | 0x0530 | venom Greater, poison-immune | — | bag 4 |
| `TideVotary` | a drowned votary | 5 | 280–330 | 9–13 | Mage | 26 | 0x0532 | poison-immune | — | bag 4 |
| `TideWarden` | **Kymopoleia, the Reefbound** | 5 | 360–380 | 12–16 | Mage | 150 | 0x0851 | — | when-struck proc, spawns adds | elite bag 5 |
| `TideWraith` | a drowned wraith | 5 | 280–330 | 9–13 | Mage | 26 | 0x0532 | — | — | bag 4 |

## Lykai (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `LykaiBat` | a warren bat | 2 | 80–110 | 5–8 | Melee | 39 | 0x0483 | — | — | bag 1 |
| `LykaiCur` | a caves cur | 2 | 80–110 | 5–8 | Melee | 23 | 0x0964 | pack | — | bag 1 |
| `LykaiGrub` | a cave grub | 2 | 80–110 | 4–7 | Melee | 51 | 0x0964 | poison-immune | — | bag 1 |
| `LykaiRatling` | a cave ratling | 2 | 80–110 | 5–8 | Melee | 0xD7 | 0x0964 | — | — | bag 1 |
| `LykaiScout` | an Arcadian scout | 2 | 90–120 | 6–9 | Melee | 42 | 0x0483 | — | — | bag 1 |
| `LykaiWhelp` | a wolf-cult whelp | 2 | 90–120 | 6–9 | Melee | 42 | 0x0483 | — | — | bag 1 |
| `LykaiWretch` | a caves wretch | 2 | 80–110 | 5–8 | Melee | 0xD7 | 0x0964 | — | — | bag 1 |
| `LykaiForager` | an Arcadian forager | 3 | 130–160 | 7–10 | Melee | 17 | 0x0844 | pack | — | bag 2 |
| `LykaiHound` | a Lykaian hound | 3 | 130–160 | 7–10 | Melee | 23 | 0x0483 | pack | — | bag 2 |
| `LykaiHunter` | an Arcadian hunter | 3 | 130–160 | 7–10 | Archer | 17 | 0x0964 | — | — | bag 2 |
| `LykaiLykaonidWhelp` | a Lykaonid whelp | 3 | 130–160 | 7–10 | Melee | 23 | 0x0483 | pack | — | bag 2 |
| `LykaiRaider` | an Arcadian raider | 3 | 130–160 | 7–10 | Melee | 17 | 0x0844 | pack | — | bag 2 |
| `LykaiSkirmisher` | an Arcadian javelineer | 3 | 130–160 | 7–10 | Melee | 182 | 0x0844 | — | — | bag 2 |
| `LykaiSnare` | a cave snare | 3 | 130–160 | 7–10 | Melee | 8 | 0x0964 | — | — | bag 2 |
| `LykaiTracker` | a Lykaian tracker | 3 | 130–160 | 7–10 | Melee | Utility.RandomList | 0x0483 | pack | — | bag 2 |
| `LykaiBloodrunner` | an Arcadian blood-runner | 4 | 200–240 | 8–11 | Melee | 182 | 0x0021 | — | — | bag 3 |
| `LykaiHowler` | a Lykaian howler | 4 | 200–235 | 8–11 | Melee | 23 | 0x0483 | pack | — | bag 3 |
| `LykaiLykaonidHunter` | a Lykaonid hunter | 4 | 200–240 | 8–11 | Melee | 17 | 0x0021 | pack | — | bag 3 |
| `LykaiLykaonidSkin` | a Lykaonid skin-changer | 4 | 200–235 | 8–11 | Melee | 23 | 0x0964 | pack | — | bag 3 |
| `LykaiMauler` | an Arcadian mauler | 4 | 200–240 | 8–11 | Melee | 189 | 0x0844 | — | — | bag 3 |
| `LykaiShaman` | a wolf-cult shaman | 4 | 200–235 | 8–11 | Mage | 140 | 0x0021 | — | — | bag 3 |
| `LykaiWitchdoctor` | a wolf-cult witch-doctor | 4 | 200–235 | 8–11 | Mage | 140 | 0x0021 | — | — | bag 3 |
| `LykaiBrute` | an Arcadian brute | 5 | 320–360 | 11–15 | Melee | 189 | 0x0844 | — | — | bag 4 |
| `LykaiLykaonidBrute` | a Lykaonid brute-prince | 5 | 320–360 | 11–15 | Melee | 189 | 0x0483 | — | — | bag 4 |
| `LykaiLykaonidShaman` | a Lykaonid blood-shaman | 5 | 320–360 | 11–15 | Mage | 140 | 0x0021 | — | — | bag 4 |
| `LykaiLykaonidStalker` | a Lykaonid stalker | 5 | 320–360 | 11–15 | Melee | Utility.RandomList | 0x0964 | — | on-hit proc | bag 4 |
| `LykaiRavener` | a Lykaian ravener | 5 | 320–360 | 11–15 | Melee | 23 | 0x0964 | pack | — | bag 4 |
| `LykaiReaver` | an Arcadian reaver | 5 | 320–360 | 11–15 | Melee | 189 | 0x0021 | — | — | bag 4 |
| `LykaiWarleader` | an Arcadian warleader | 5 | 320–360 | 11–15 | Melee | 138 | 0x0021 | pack | when-struck proc, spawns adds | bag 4 |
| `LykaiChieftain` | an Arcadian chieftain | 6 | 480–530 | 13–17 | Melee | 138 | 0x0844 | — | — | bag 5 |
| `LykaiLykaonidHoundmaster` | a Lykaonid hound-master | 6 | 460–510 | 12–16 | Melee | 138 | 0x0483 | pack | when-struck proc | bag 5 |
| `LykaiLykaonidPriest` | a Lykaonid moon-priest | 6 | 460–510 | 12–16 | Mage | 138 | 0x0021 | — | when-struck proc, spawns adds | bag 5 |
| `LykaiLykaonidPrince` | a Lykaonid wolf-prince | 6 | 480–530 | 13–17 | Melee | 189 | 0x0483 | bleed-immune, pack | — | bag 5 |
| `LykaiMainalos` | **Mainalos, the Old Wolf** | 6 | 540–550 | 14–18 | Melee | 138 | 0x0483 | bleed-immune, pack | when-struck proc, spawns adds | elite bag 6 |
| `LykaiNyktimos` | **Nyktimos, the Wolf-Crowned** | 6 | 540–550 | 14–18 | Melee | 189 | 0x0483 | bleed-immune, pack | on-hit proc | elite bag 6 |

## Myrmi (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `MyrmiAiakidRunner` | an Aiakid runner | 3 | 130–160 | 7–10 | Melee | 71 | 0x0798 | pack | — | bag 2 |
| `MyrmiDrone` | a myrmex drone | 3 | 130–160 | 7–10 | Melee | 71 | 0x0964 | pack | — | bag 2 |
| `MyrmiForager` | a myrmex forager | 3 | 130–160 | 7–10 | Melee | 805 | 0x0798 | — | — | bag 2 |
| `MyrmiGatherer` | a myrmex gatherer | 3 | 130–160 | 7–10 | Melee | 805 | 0x0798 | pack | — | bag 2 |
| `MyrmiGnat` | a nest gnat | 3 | 110–140 | 6–9 | Melee | 39 | 0x0798 | — | — | bag 2 |
| `MyrmiGrub` | a myrmex grub | 3 | 110–140 | 6–9 | Melee | 51 | 0x0798 | poison-immune | — | bag 2 |
| `MyrmiMite` | a nest mite | 3 | 110–140 | 6–9 | Melee | 0xD7 | 0x0964 | — | — | bag 2 |
| `MyrmiSpitter` | a myrmex spitter | 3 | 130–160 | 7–10 | Melee | 71 | 0x0021 | venom Greater | — | bag 2 |
| `MyrmiTunneler` | a myrmex tunneler | 3 | 130–160 | 7–10 | Melee | 787 | 0x0964 | — | — | bag 2 |
| `MyrmiAiakidShield` | an Aiakid shield-ant | 4 | 200–240 | 8–11 | Melee | 806 | 0x0021 | pack | — | bag 3 |
| `MyrmiAiakidSpear` | an Aiakid spear-ant | 4 | 200–240 | 8–11 | Melee | 70 | 0x0966 | pack | — | bag 3 |
| `MyrmiAntlion` | a myrmex antlion | 4 | 200–235 | 8–11 | Melee | 787 | 0x0798 | venom Greater | — | bag 3 |
| `MyrmiBurrower` | a myrmex burrower | 4 | 200–235 | 8–11 | Melee | 787 | 0x0966 | — | — | bag 3 |
| `MyrmiChitinguard` | a myrmex chitin-guard | 4 | 200–240 | 8–11 | Melee | 806 | 0x0966 | pack | — | bag 3 |
| `MyrmiPikebug` | a myrmex pikeguard | 4 | 200–240 | 8–11 | Melee | 70 | 0x0966 | pack | — | bag 3 |
| `MyrmiRaidbug` | a myrmex raidbug | 4 | 200–240 | 8–11 | Melee | 70 | 0x0021 | pack | — | bag 3 |
| `MyrmiSoldier` | a myrmex soldier | 4 | 200–240 | 8–11 | Melee | 806 | 0x0966 | pack | — | bag 3 |
| `MyrmiStinger` | a myrmex stinger | 4 | 200–235 | 8–11 | Melee | 48 | 0x0798 | venom Deadly | — | bag 3 |
| `MyrmiAiakidLancer` | an Aiakid lancer | 5 | 320–360 | 10–14 | Melee | 70 | 0x0798 | venom Deadly | — | bag 4 |
| `MyrmiAiakidPhalanx` | an Aiakid phalangite | 5 | 320–360 | 10–14 | Melee | 152 | 0x0966 | poison-immune, pack | — | bag 4 |
| `MyrmiSoldierRed` | a red myrmex soldier | 5 | 320–360 | 10–14 | Melee | 782 | 0x0021 | pack | — | bag 4 |
| `MyrmiVenomspur` | a myrmex venomspur | 5 | 320–360 | 10–14 | Melee | 782 | 0x0021 | venom Deadly | — | bag 4 |
| `MyrmiWarden` | a myrmex hive-warden | 5 | 320–360 | 10–14 | Melee | 70 | 0x0798 | — | when-struck proc, spawns adds | bag 4 |
| `MyrmiWarspur` | a myrmex warspur | 5 | 320–360 | 10–14 | Melee | 70 | 0x0966 | venom Deadly | — | bag 4 |
| `MyrmiAiakidGoad` | an Aiakid goad-warden | 6 | 460–510 | 12–16 | Melee | 152 | 0x0798 | poison-immune | when-struck proc, spawns adds | bag 5 |
| `MyrmiAiakidSpearcaller` | an Aiakid spear-caller | 6 | 480–530 | 13–17 | Mage | 72 | 0x0966 | poison-immune | on-hit proc | bag 5 |
| `MyrmiAiakidVenomcaster` | an Aiakid venom-caster | 6 | 460–520 | 12–16 | Mage | 72 | 0x0021 | venom Deadly, poison-immune | — | bag 5 |
| `MyrmiAvenger` | a myrmex avenger | 6 | 460–510 | 12–16 | Melee | 152 | 0x0021 | poison-immune, pack | — | bag 5 |
| `MyrmiBroodguard` | a myrmex broodguard | 6 | 480–530 | 13–17 | Melee | 807 | 0x0966 | poison-immune | — | bag 5 |
| `MyrmiBroodpriest` | a myrmex brood-priest | 6 | 460–520 | 12–16 | Mage | 72 | 0x0798 | poison-immune | — | bag 5 |
| `MyrmiHivelord` | a myrmex hive-lord | 6 | 480–530 | 13–17 | Melee | 807 | 0x0966 | poison-immune | — | bag 5 |
| `MyrmiRavager` | a myrmex ravager | 6 | 460–510 | 12–16 | Melee | 152 | 0x0021 | poison-immune, pack | — | bag 5 |
| `MyrmiAiakidMarshal` | **the Aiakid war-marshal** | 7 | 700–720 | 16–20 | Melee | 152 | 0x0021 | poison-immune, bleed-immune, pack | when-struck proc | bag 6 |
| `MyrmiMenoitios` | **Menoitios, the Ant-Marshal** | 7 | 700–720 | 16–20 | Mage | 72 | 0x0966 | venom Deadly, poison-immune, bleed-immune | when-struck proc, spawns adds | elite bag 7 |
| `MyrmiMyrmex` | **Myrmex, the Brood-Mother** | 7 | 700–720 | 16–20 | Mage | 72 | 0x0798 | venom Deadly, poison-immune, bleed-immune | when-struck proc, spawns adds | elite bag 7 |

## NemeanWildwood (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `WyldBoar` | a great boar | 6 | 460–510 | 12–16 | Melee | 0x122 | 0x0844 | — | — | bag 5 |
| `WyldFawn` | a moonlit fawn | 6 | 440–470 | 10–14 | Melee | 0xED | 0x0483 | — | — | bag 5 |
| `WyldHarrier` | a moon harrier | 6 | 440–490 | 11–15 | Melee | 30 | 0x0483 | — | — | bag 5 |
| `WyldHart` | a silver hart | 6 | 440–490 | 11–15 | Melee | 0xEA | 0x0486 | — | — | bag 5 |
| `WyldHound` | a moonlit hound | 6 | 460–520 | 12–16 | Melee | 98 | 0x0483 | pack | — | bag 5 |
| `WyldLynx` | a silver lynx | 6 | 440–490 | 11–15 | Melee | 63 | 0x0483 | — | — | bag 5 |
| `WyldSpiderling` | a silverweb spiderling | 6 | 440–470 | 10–14 | Melee | 28 | 0x0483 | venom Greater | — | bag 5 |
| `WyldSprite` | a thorn sprite | 6 | 440–470 | 10–14 | Mage | 128 | 0x0851 | — | — | bag 5 |
| `WyldStalker` | a thornclad stalker | 6 | 440–500 | 11–15 | Mage | 42 | 0x0844 | venom Greater | — | bag 5 |
| `WyldStinger` | a thornscorpion | 6 | 440–490 | 11–15 | Melee | 48 | 0x0844 | venom Deadly | — | bag 5 |
| `WyldThiasosHound` | a Thiasos hound | 6 | 440–490 | 11–15 | Melee | 98 | 0x0483 | pack | — | bag 5 |
| `WyldViper` | a grove viper | 6 | 440–490 | 11–15 | Melee | 52 | 0x0851 | venom Greater | — | bag 5 |
| `WyldWolf` | a moonlit wolf | 6 | 440–500 | 11–15 | Melee | Utility.RandomList | 0x0483 | pack | — | bag 5 |
| `WyldConstrictor` | a sacred constrictor | 7 | 620–690 | 15–19 | Melee | 0x15 | 0x0851 | venom Deadly, poison-immune | — | bag 6 |
| `WyldCorpser` | a thornwood corpser | 7 | 620–690 | 15–19 | Melee | 8 | 0x0844 | venom Greater | — | bag 6 |
| `WyldDirewolf` | a dire wolf of the hunt | 7 | 620–680 | 15–19 | Melee | 23 | 0x0486 | pack | — | bag 6 |
| `WyldGrizzly` | a moon-marked bear | 7 | 620–680 | 15–19 | Melee | 212 | 0x0483 | — | — | bag 6 |
| `WyldMatriarch` | **Atalanta, the Unbowed** | 7 | 700–720 | 16–20 | Mage | 87 | 0x0486 | — | when-struck proc, spawns adds | elite bag 7 |
| `WyldNemean` | a nemean cougar | 7 | 620–690 | 15–19 | Melee | 63 | 0x0798 | — | — | bag 6 |
| `WyldPanther` | a bronze-maned panther | 7 | 620–700 | 15–19 | Melee | 0xD6 | 0x0798 | bleed-immune | when-struck proc | bag 6 |
| `WyldProkris` | **Prokris, the Unerring** | 7 | 710–720 | 16–20 | Melee | 212 | 0x0486 | bleed-immune | on-hit proc | elite bag 7 |
| `WyldPuma` | a silverpelt panther | 7 | 620–690 | 15–19 | Melee | 0xD6 | 0x0486 | bleed-immune | — | bag 6 |
| `WyldPython` | a sacred python | 7 | 600–680 | 14–18 | Melee | 0x15 | 0x0851 | venom Deadly, poison-immune | — | bag 6 |
| `WyldSentinel` | a grove sentinel | 7 | 640–720 | 15–19 | Melee | 14 | 0x0851 | poison-immune, breath:PoisonBreath | — | bag 6 |
| `WyldSpider` | a silverweb spider | 7 | 600–670 | 14–18 | Melee | 28 | 0x0483 | venom Deadly | — | bag 6 |
| `WyldStag` | **Elaphos, the Golden-Horned** | 7 | 710–720 | 17–21 | Melee | 0xEA | 0x0501 | poison-immune, bleed-immune | on-hit proc | elite bag 7 ×2 |
| `WyldThiasosArcher` | a Thiasos archer | 7 | 600–650 | 14–18 | Archer | 0x190 | 0x0844 | — | — | bag 6 |
| `WyldThiasosHoundmaster` | the Thiasos houndmaster | 7 | 620–680 | 15–19 | Mage | 0x190 | 0x0486 | — | when-struck proc, spawns adds | bag 6 |
| `WyldThiasosHuntress` | a Thiasos huntress | 7 | 600–660 | 14–18 | Archer | 0x190 | 0x0486 | — | — | bag 6 |
| `WyldThiasosMatron` | the Thiasos matron | 7 | 640–700 | 15–19 | Mage | 0x190 | 0x0798 | — | on-hit proc | bag 6 |
| `WyldThiasosPanther` | a Thiasos panther | 7 | 620–690 | 15–19 | Melee | 0xD6 | 0x0486 | bleed-immune | — | bag 6 |
| `WyldThiasosPriestess` | the Thiasos priestess | 7 | 610–670 | 14–18 | Mage | 0x190 | 0x0486 | poison-immune | — | bag 6 |
| `WyldThiasosStalker` | a Thiasos stalker | 7 | 600–660 | 14–18 | Melee | 0x190 | 0x0483 | — | when-struck proc | bag 6 |
| `WyldThiasosWitch` | a Thiasos witch | 7 | 600–660 | 14–18 | Mage | 0x190 | 0x0851 | venom Deadly, poison-immune | — | bag 6 |
| `WyldTreant` | a grove treant | 7 | 640–710 | 15–19 | Mage | 47 | 0x0851 | poison-immune, breath:PoisonBreath | — | bag 6 |

## Ophian (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `OphianAcolyte` | an ophian acolyte | 4 | 200–235 | 8–11 | Mage | 85 | 0x0847 | venom Greater | — | bag 3 |
| `OphianAsp` | a temple asp | 4 | 190–220 | 7–10 | Melee | 52 | 0x0851 | venom Greater | — | bag 3 |
| `OphianCoilrat` | a nest rat | 4 | 190–220 | 7–10 | Melee | 0xD7 | 0x0798 | — | — | bag 3 |
| `OphianCrawler` | an ophian scale-crawler | 4 | 200–240 | 8–11 | Melee | 0x15 | 0x0847 | venom Greater | — | bag 3 |
| `OphianHatchling` | an ophian hatchling | 4 | 190–230 | 7–10 | Melee | 52 | 0x0851 | venom Greater | — | bag 3 |
| `OphianOphiteAcolyte` | an Ophite acolyte | 4 | 200–235 | 8–11 | Mage | 85 | 0x0847 | venom Greater | — | bag 3 |
| `OphianScale` | an ophian scaleguard | 4 | 200–240 | 8–11 | Melee | 86 | 0x0851 | venom Greater | — | bag 3 |
| `OphianScuttler` | a scale scuttler | 4 | 190–220 | 7–10 | Melee | 48 | 0x0847 | venom Greater | — | bag 3 |
| `OphianWarden` | an ophian scale-warden | 4 | 200–240 | 8–11 | Melee | 86 | 0x0798 | venom Greater | — | bag 3 |
| `OphianBasilisk` | an ophian basilisk | 5 | 320–360 | 10–14 | Melee | 0x15 | 0x0847 | venom Deadly, poison-immune | — | bag 4 |
| `OphianConstrictor` | a temple constrictor | 5 | 320–360 | 10–14 | Melee | 0x15 | 0x0851 | venom Deadly, poison-immune | — | bag 4 |
| `OphianKnight` | an ophian serpent-knight | 5 | 320–360 | 10–14 | Melee | 86 | 0x0798 | venom Deadly, poison-immune | — | bag 4 |
| `OphianLancer` | an ophian coil-lancer | 5 | 320–360 | 10–14 | Melee | 86 | 0x0453 | venom Deadly, poison-immune | — | bag 4 |
| `OphianOphiteScaleward` | an Ophite scale-ward | 5 | 320–360 | 10–14 | Melee | 86 | 0x0851 | venom Deadly | — | bag 4 |
| `OphianOphiteSerpentguard` | an Ophite serpent-guard | 5 | 320–360 | 10–14 | Melee | 86 | 0x0798 | venom Deadly, poison-immune | — | bag 4 |
| `OphianPriest` | an ophian venom-priest | 5 | 320–360 | 10–14 | Mage | 85 | 0x0851 | venom Deadly, poison-immune | — | bag 4 |
| `OphianReaver` | an ophian reaver | 5 | 320–360 | 10–14 | Melee | 86 | 0x0453 | venom Deadly | — | bag 4 |
| `OphianVenomancer` | an ophian venomancer | 5 | 320–360 | 10–14 | Mage | 85 | 0x0851 | venom Deadly | — | bag 4 |
| `OphianArchpriest` | an ophian archpriest | 6 | 460–520 | 12–16 | Mage | 85 | 0x0851 | poison-immune | on-hit proc | bag 5 |
| `OphianColossus` | a scaled colossus | 6 | 460–510 | 12–16 | Melee | 86 | 0x0798 | poison-immune | — | bag 5 |
| `OphianConstrictorGreat` | a great constrictor | 6 | 460–510 | 12–16 | Melee | 0x15 | 0x0453 | venom Deadly, poison-immune | — | bag 5 |
| `OphianMount` | an ophian coil-mount | 6 | 460–510 | 12–16 | Melee | 116 | 0x0453 | breath:FireBreath | — | bag 5 |
| `OphianOphiteConstrictor` | an Ophite temple-constrictor | 6 | 460–510 | 12–16 | Melee | 0x15 | 0x0453 | venom Deadly, poison-immune | — | bag 5 |
| `OphianOphiteMystic` | an Ophite coil-mystic | 6 | 460–520 | 12–16 | Mage | 85 | 0x0847 | poison-immune | — | bag 5 |
| `OphianOphiteOracle` | an Ophite venom-oracle | 6 | 460–520 | 12–16 | Mage | 85 | 0x0851 | venom Deadly, poison-immune | — | bag 5 |
| `OphianScaledbrute` | a scaled brute | 6 | 460–510 | 12–16 | Melee | 86 | 0x0847 | poison-immune | — | bag 5 |
| `OphianSerpentmage` | an ophian serpent-mage | 6 | 460–520 | 12–16 | Mage | 85 | 0x0851 | poison-immune | — | bag 5 |
| `OphianBroodmother` | an ophian brood-mother | 7 | 600–660 | 14–18 | Mage | 87 | 0x0851 | venom Deadly, poison-immune | — | bag 6 |
| `OphianDrakon` | an ophian drakon | 7 | 620–680 | 15–19 | Melee | 62 | 0x0847 | poison-immune, breath:OphianVenomBreath | — | bag 6 |
| `OphianKeto` | **Keto, the Scaled Matron** | 7 | 700–720 | 16–20 | Mage | 87 | 0x0851 | venom Deadly, poison-immune, bleed-immune | on-hit proc | elite bag 7 |
| `OphianOphiteEnvenomer` | an Ophite envenomer | 7 | 600–660 | 14–18 | Mage | 85 | 0x0851 | venom Deadly, poison-immune | on-hit proc | bag 6 |
| `OphianOphiteWarden` | an Ophite high-warden | 7 | 610–670 | 15–19 | Melee | 86 | 0x0798 | poison-immune | when-struck proc | bag 6 |
| `OphianTitanspawn` | an ophian titan-spawn | 7 | 620–680 | 15–19 | Mage | 87 | 0x0453 | venom Deadly, poison-immune | — | bag 6 |
| `OphianOphiteHierophant` | the Ophite high-hierophant | 8 | 900–950 | 18–24 | Mage | 87 | 0x0851 | venom Deadly, poison-immune, bleed-immune | when-struck proc, spawns adds | bag 7 |
| `OphianPoine` | **Poine, the Argive Coil** | 8 | 900–950 | 18–24 | Mage | 87 | 0x0453 | venom Deadly, poison-immune, bleed-immune | on-hit proc | elite bag 8 |

## Pelasg (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `PelasgCavebat` | a cave bat | 2 | 80–100 | 4–7 | Melee | 39 | 0x0964 | — | — | bag 1 |
| `PelasgCreeper` | a painted creeper | 2 | 80–110 | 5–8 | Melee | 8 | 0x0964 | — | — | bag 1 |
| `PelasgGrub` | a painted grub | 2 | 80–100 | 4–7 | Melee | 51 | 0x0798 | poison-immune | — | bag 1 |
| `PelasgGrubber` | a cave grubber | 2 | 80–110 | 5–8 | Melee | 0xD7 | 0x0844 | — | — | bag 1 |
| `PelasgKnapper` | a Pelasgian stone-knapper | 2 | 90–110 | 6–9 | Melee | 267 | 0x0844 | — | — | bag 1 |
| `PelasgLelexForager` | a Leleges forager | 2 | 90–110 | 6–9 | Melee | 267 | 0x0964 | — | — | bag 1 |
| `PelasgToadling` | a spotted toadling | 2 | 80–100 | 4–7 | Melee | 80 | 0x0844 | — | — | bag 1 |
| `PelasgVermin` | a cave vermin | 2 | 80–110 | 5–8 | Melee | 0xD7 | 0x0844 | — | — | bag 1 |
| `PelasgWhelp` | a Pelasgian whelp | 2 | 90–110 | 6–9 | Melee | 267 | 0x0798 | — | — | bag 1 |
| `PelasgApeman` | a painted ape-man | 3 | 130–160 | 7–10 | Melee | 0x1D | 0x0967 | — | — | bag 2 |
| `PelasgCaveviper` | a painted viper | 3 | 130–160 | 7–10 | Melee | 52 | 0x0844 | venom Greater | — | bag 2 |
| `PelasgDaubed` | a daubed hunter | 3 | 130–160 | 7–10 | Melee | 17 | 0x0844 | pack | — | bag 2 |
| `PelasgForager` | a Pelasgian forager | 3 | 130–160 | 7–10 | Melee | 267 | 0x0798 | — | — | bag 2 |
| `PelasgHunter` | a Pelasgian hunter | 3 | 130–160 | 7–10 | Archer | 267 | 0x0844 | — | — | bag 2 |
| `PelasgLelexHunter` | a Leleges hunter | 3 | 130–160 | 7–10 | Archer | 17 | 0x0964 | pack | — | bag 2 |
| `PelasgLelexKnapper` | a Leleges flint-knapper | 3 | 130–160 | 7–10 | Melee | 267 | 0x0844 | pack | — | bag 2 |
| `PelasgLelexToadherd` | a Leleges toad-herd | 3 | 130–160 | 7–10 | Melee | 80 | 0x0798 | venom Greater | — | bag 2 |
| `PelasgSlinger` | a Pelasgian slinger | 3 | 130–160 | 7–10 | Archer | 267 | 0x0964 | — | — | bag 2 |
| `PelasgSpearman` | a Pelasgian spearman | 3 | 130–160 | 7–10 | Melee | 267 | 0x0798 | pack | — | bag 2 |
| `PelasgToadherd` | a cave-toad | 3 | 130–160 | 7–10 | Melee | 80 | 0x0964 | venom Regular | — | bag 2 |
| `PelasgTorchbearer` | a Pelasgian torchbearer | 3 | 130–160 | 7–10 | Melee | 267 | 0x0798 | — | — | bag 2 |
| `PelasgTrapper` | a Pelasgian trapper | 3 | 130–160 | 7–10 | Melee | 182 | 0x0964 | — | — | bag 2 |
| `PelasgBrute` | a Pelasgian brute | 4 | 200–240 | 8–11 | Melee | 189 | 0x0844 | — | — | bag 3 |
| `PelasgCavebear` | a painted cave-bear | 4 | 200–240 | 8–11 | Melee | 212 | 0x0967 | — | — | bag 3 |
| `PelasgLelexAper` | a Leleges ape-keeper | 4 | 200–240 | 8–11 | Melee | 0x1D | 0x0967 | — | — | bag 3 |
| `PelasgLelexAugur` | a Leleges bone-augur | 4 | 200–235 | 8–11 | Mage | 140 | 0x0798 | — | on-hit proc | bag 3 |
| `PelasgLelexBrute` | a Leleges cave-brute | 4 | 200–240 | 8–11 | Melee | 189 | 0x0964 | — | — | bag 3 |
| `PelasgLelexMother` | the Leleges clan-mother | 4 | 200–240 | 9–12 | Mage | 267 | 0x0967 | — | when-struck proc, spawns adds | bag 3 |
| `PelasgLelexTrapmaster` | a Leleges trap-master | 4 | 200–240 | 8–11 | Melee | 182 | 0x0844 | — | when-struck proc | bag 3 |
| `PelasgOchremage` | a Pelasgian ochre-shaman | 4 | 200–235 | 8–11 | Mage | 140 | 0x0798 | — | — | bag 3 |
| `PelasgPelasgos` | **Pelasgos, the Earth-Born** | 4 | 235–240 | 10–14 | Melee | 267 | 0x0967 | bleed-immune | when-struck proc, spawns adds | elite bag 4 |
| `PelasgPhoroneus` | **Phoroneus, the First-Cut** | 4 | 235–240 | 10–14 | Melee | 267 | 0x0967 | bleed-immune | on-hit proc | elite bag 4 |
| `PelasgShaman` | a Pelasgian bone-shaman | 4 | 200–235 | 8–11 | Mage | 267 | 0x0964 | — | — | bag 3 |
| `PelasgWarchief` | a Pelasgian war-chief | 4 | 200–240 | 9–12 | Melee | 189 | 0x0967 | — | — | bag 3 |
| `PelasgWarhunter` | a Pelasgian war-hunter | 4 | 200–240 | 8–11 | Archer | 17 | 0x0798 | pack | — | bag 3 |

## Pyre (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `PyreAshgull` | an ash gull | 5 | 280–310 | 8–11 | Melee | 5 | 0x0021 | — | — | bag 4 |
| `PyreAsp` | a cinder asp | 5 | 300–340 | 10–14 | Melee | 52 | 0x0655 | venom Greater | — | bag 4 |
| `PyreCharhound` | a charred hound | 5 | 300–350 | 10–14 | Melee | 98 | 0x0021 | breath:FireBreath, pack | — | bag 4 |
| `PyreCindermoth` | a pyre moth | 5 | 280–310 | 8–11 | Melee | 39 | 0x0655 | — | — | bag 4 |
| `PyreCinderrat` | a pyre rat | 5 | 280–310 | 8–11 | Melee | 0xD7 | 0x0655 | — | — | bag 4 |
| `PyreCoalwalker` | a coalwalking shade | 5 | 300–350 | 10–14 | Melee | Utility.RandomList | 0x0453 | — | — | bag 4 |
| `PyreCrawler` | a lava crawler | 5 | 300–350 | 10–14 | Melee | 0xCE | 0x0021 | poison-immune | — | bag 4 |
| `PyreEmberling` | a living ember | 5 | 280–330 | 9–13 | Mage | 74 | 0x0655 | breath:FireBreath | — | bag 4 |
| `PyreEmberwing` | an emberwing harpy | 5 | 300–350 | 10–14 | Melee | 30 | 0x0655 | — | — | bag 4 |
| `PyreHound` | an ashen hound | 5 | 300–350 | 10–14 | Melee | 98 | 0x0453 | breath:FireBreath, pack | — | bag 4 |
| `PyreKaminosAcolyte` | a Kaminoi acolyte | 5 | 300–350 | 10–14 | Mage | 148 | 0x0655 | bleed-immune | — | bag 4 |
| `PyreKaminosStoker` | a Kaminoi stoker | 5 | 320–360 | 11–15 | Melee | 1 | 0x0021 | poison-immune | — | bag 4 |
| `PyreScorchling` | a scorchling | 5 | 300–350 | 10–14 | Melee | 0xCE | 0x0655 | poison-immune | — | bag 4 |
| `PyreSlaggrub` | a slag grub | 5 | 280–310 | 8–11 | Melee | 51 | 0x0669 | poison-immune, bleed-immune | — | bag 4 |
| `PyreSlagling` | a slag imp | 5 | 280–330 | 9–13 | Mage | 74 | 0x0669 | breath:FireBreath | — | bag 4 |
| `PyreAshwraith` | an ash wraith | 6 | 460–510 | 12–16 | Mage | 26 | 0x0453 | — | — | bag 5 |
| `PyreCoalgeist` | a coal geist | 6 | 460–510 | 12–16 | Mage | 148 | 0x0026 | bleed-immune | — | bag 5 |
| `PyreEfreet` | a flamewind efreet | 6 | 480–540 | 13–17 | Mage | 131 | 0x0655 | breath:FireBreath | — | bag 5 |
| `PyreKaminosBrand` | a Kaminoi brand-bearer | 6 | 460–520 | 12–16 | Melee | 130 | 0x0669 | breath:FireBreath | — | bag 5 |
| `PyreKaminosEmbermonk` | a Kaminoi ember-monk | 6 | 460–520 | 12–16 | Mage | 131 | 0x0655 | breath:FireBreath | — | bag 5 |
| `PyreKaminosScald` | a Kaminoi scald-priest | 6 | 460–510 | 12–16 | Mage | 24 | 0x0026 | bleed-immune | — | bag 5 |
| `PyreMagmaton` | a magma elemental | 6 | 460–520 | 12–16 | Melee | 15 | 0x0669 | poison-immune | — | bag 5 |
| `PyreScoria` | a scoria gargoyle | 6 | 460–520 | 12–16 | Mage | 130 | 0x0021 | breath:FireBreath | — | bag 5 |
| `PyreSerpent` | a molten serpent | 6 | 480–530 | 12–16 | Melee | 90 | 0x0669 | venom Deadly | — | bag 5 |
| `PyreServitor` | a river-fire elemental | 6 | 460–520 | 12–16 | Melee | 15 | 0x0026 | poison-immune | aura | bag 5 |
| `PyreUnburnt` | an unburnt shade | 6 | 460–510 | 12–16 | Mage | 148 | 0x0453 | bleed-immune | — | bag 5 |
| `PyreBrandbeast` | a brand-beast | 7 | 620–680 | 15–19 | Melee | 9 | 0x0655 | poison-immune | — | bag 6 |
| `PyreDaemon` | a cinder daemon | 7 | 640–700 | 15–19 | Mage | 9 | 0x0669 | poison-immune, breath:FireBreath | — | bag 6 |
| `PyreKaminosAnvilguard` | a Kaminoi anvil-guard | 7 | 620–680 | 15–19 | Melee | 9 | 0x0669 | poison-immune | — | bag 6 |
| `PyreKaminosCindercaller` | a Kaminoi cinder-caller | 7 | 610–670 | 15–19 | Mage | 24 | 0x0655 | bleed-immune | when-struck proc, spawns adds | bag 6 |
| `PyreKaminosEmberbrand` | a Kaminoi emberbrand | 7 | 620–680 | 15–19 | Mage | 131 | 0x0026 | — | on-hit proc | bag 6 |
| `PyreKaminosForgemaster` | the Kaminoi forgemaster | 7 | 640–700 | 16–20 | Melee | 9 | 0x0669 | poison-immune, breath:FireBreath | on-hit proc | bag 6 |
| `PyrePhaethon` | **Phaethon, the Sky-Scorcher** | 7 | 700–720 | 16–22 | Mage | 9 | 0x0669 | poison-immune, bleed-immune, breath:FireBreath | on-hit proc | elite bag 7 |
| `PyrePyromancer` | an unburnt pyromancer | 7 | 620–680 | 15–19 | Mage | 24 | 0x0026 | poison-immune, bleed-immune | on-hit proc | bag 6 |
| `PyrePhlegyas` | **Phlegyas, the Unquenched** | 8 | 900–950 | 18–24 | Mage | 9 | 0x0669 | poison-immune, bleed-immune, breath:FireBreath | on-hit proc | elite bag 8 |

## Rime (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `RimeArcher` | a rime-bound archer | 4 | 200–235 | 8–11 | Archer | 0x8E | 0x0AF3 | — | — | bag 3 |
| `RimeBoar` | a tundra boar | 4 | 200–240 | 8–11 | Melee | 0x122 | 0x0485 | — | — | bag 3 |
| `RimeBoreadScout` | a Boread scout | 4 | 200–240 | 8–11 | Melee | 30 | 0x0AF3 | — | — | bag 3 |
| `RimeCrone` | a rime crone | 4 | 200–235 | 8–11 | Mage | 148 | 0x0AF3 | breath:ColdBreath | — | bag 3 |
| `RimeGrub` | a permafrost grub | 4 | 190–230 | 7–10 | Melee | 51 | 0x0B0F | poison-immune, bleed-immune | — | bag 3 |
| `RimeHowler` | a frost howler | 4 | 200–235 | 8–11 | Melee | Utility.RandomList | 0x047E | pack | — | bag 3 |
| `RimeMoth` | a frost moth | 4 | 190–220 | 7–10 | Melee | 39 | 0x0B0F | — | — | bag 3 |
| `RimeOoze` | a frost ooze | 4 | 190–230 | 7–10 | Melee | 94 | 0x0B0F | poison-immune, bleed-immune | — | bag 3 |
| `RimeSerpent` | a glacial serpent | 4 | 200–240 | 8–11 | Melee | 89 | 0x0AF3 | venom Regular | — | bag 3 |
| `RimeShaman` | a rime-bound shaman | 4 | 200–235 | 8–11 | Mage | 0x8F | 0x0AF3 | breath:ColdBreath | — | bag 3 |
| `RimeSparrow` | a frost sparrow | 4 | 190–220 | 7–10 | Melee | 5 | 0x0AF3 | — | — | bag 3 |
| `RimeSpider` | a hoarfrost spider | 4 | 200–235 | 8–11 | Melee | 20 | 0x047E | venom Greater, pack | — | bag 3 |
| `RimeStalker` | a frostfang stalker | 4 | 190–230 | 7–10 | Melee | 52 | 0x0B0F | — | — | bag 3 |
| `RimeThrall` | a rime-bound thrall | 4 | 200–240 | 8–11 | Melee | 42 | 0x047E | — | — | bag 3 |
| `RimeVole` | a snow vole | 4 | 190–220 | 7–10 | Melee | 0xD7 | 0x047E | — | — | bag 3 |
| `RimeBoreadArcher` | a Boread frost-archer | 5 | 300–350 | 10–14 | Archer | 0x8E | 0x047E | — | — | bag 4 |
| `RimeBoreadHoundmaster` | a Boread hound-master | 5 | 320–360 | 10–14 | Mage | 0x8F | 0x0485 | — | — | bag 4 |
| `RimeBoreadRider` | a Boread wind-rider | 5 | 320–360 | 11–15 | Melee | 30 | 0x0B0F | — | — | bag 4 |
| `RimeElemental` | a snowbound elemental | 5 | 320–370 | 10–14 | Melee | 163 | 0x0485 | poison-immune, bleed-immune, breath:ColdBreath | — | bag 4 |
| `RimeReaver` | a rime-bound reaver | 5 | 320–360 | 10–14 | Melee | 147 | 0x0485 | — | — | bag 4 |
| `RimeWight` | a frostbound wight | 5 | 320–360 | 10–14 | Mage | 26 | 0x0AF3 | poison-immune | — | bag 4 |
| `RimeBoreadLancer` | a Boread frost-lancer | 6 | 480–530 | 13–17 | Melee | 135 | 0x0485 | — | — | bag 5 |
| `RimeBoreadOutrider` | a Boread outrider | 6 | 480–530 | 13–17 | Melee | 30 | 0x0B0F | — | on-hit proc | bag 5 |
| `RimeBoreadShaman` | a Boread storm-shaman | 6 | 480–530 | 13–17 | Mage | 43 | 0x0AF3 | breath:ColdBreath | — | bag 5 |
| `RimeFiend` | a boreal fiend | 6 | 480–530 | 13–17 | Mage | 43 | 0x0B0F | breath:ColdBreath | — | bag 5 |
| `RimeGiant` | a rime frost-giant | 6 | 480–540 | 13–17 | Melee | 55 | 0x0485 | — | — | bag 5 |
| `RimeGolem` | a hoarfrost golem | 6 | 480–540 | 13–17 | Melee | 752 | 0x047E | poison-immune, bleed-immune | — | bag 5 |
| `RimeMauler` | a glacier mauler | 6 | 480–530 | 13–17 | Melee | 55 | 0x0485 | — | — | bag 5 |
| `RimeSeer` | a hoarfrost seer | 6 | 460–510 | 12–16 | Mage | 24 | 0x0AF3 | — | — | bag 5 |
| `RimeBoreadGale` | the Boread gale-caller | 7 | 610–670 | 15–19 | Mage | 43 | 0x0485 | — | on-hit proc | bag 6 |
| `RimeBoreadHerald` | the Boread herald | 7 | 640–700 | 16–20 | Melee | 135 | 0x047E | bleed-immune | when-struck proc, spawns adds | bag 6 |
| `RimeCheimon` | **Cheimon, the Deep-Winter** | 7 | 700–720 | 16–22 | Melee | Utility.RandomBool | 0x0AF3 | poison-immune, bleed-immune, breath:ColdBreath | on-hit proc | elite bag 7 |
| `RimeColossus` | a frostbound colossus | 7 | 620–680 | 15–19 | Melee | 135 | 0x047E | bleed-immune | — | bag 6 |
| `RimeWarlord` | a hoarfrost warlord | 7 | 620–680 | 15–19 | Melee | 135 | 0x0485 | bleed-immune | on-hit proc | bag 6 |
| `RimeAbaris` | **Abaris, the Hoarfrost Herald** | 8 | 900–950 | 18–24 | Melee | Utility.RandomBool | 0x047E | bleed-immune, breath:ColdBreath | on-hit proc | elite bag 8 |

## StormcrownAerie (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `StormAnemoiBreeze` | a wandering breeze | 8 | 780–850 | 15–19 | Mage | 58 | 0x0481 | — | — | bag 7 |
| `StormAnemoiDrift` | an anemoi drift | 8 | 800–880 | 16–20 | Melee | 13 | 0x0481 | poison-immune | — | bag 7 |
| `StormAnemoiOutrider` | an anemoi outrider | 8 | 820–900 | 16–20 | Melee | 30 | 0x0492 | — | on-hit proc | bag 7 |
| `StormChainling` | a chained ettin | 8 | 820–920 | 17–21 | Melee | 18 | 0x0492 | — | — | bag 7 |
| `StormEfreet` | a sky efreet | 8 | 800–880 | 16–20 | Mage | 131 | 0x0480 | breath:EnergyBreath | — | bag 7 |
| `StormGale` | a gale spirit | 8 | 800–900 | 16–20 | Melee | 13 | 0x0481 | poison-immune | — | bag 7 |
| `StormGazer` | a storm gazer | 8 | 800–880 | 16–20 | Mage | 22 | 0x0491 | — | — | bag 7 |
| `StormHarpy` | a tempest harpy | 8 | 800–900 | 16–20 | Melee | 30 | 0x0491 | pack | on-hit proc | bag 7 |
| `StormMoth` | a static moth | 8 | 780–820 | 14–18 | Melee | 39 | 0x0480 | — | — | bag 7 |
| `StormPuff` | a charged wispling | 8 | 780–820 | 14–18 | Mage | 58 | 0x0480 | — | — | bag 7 |
| `StormRoc` | a squall harpy | 8 | 800–880 | 16–20 | Melee | 30 | 0x0491 | pack | — | bag 7 |
| `StormSparrow` | a lightning sparrow | 8 | 780–820 | 14–18 | Melee | 5 | 0x0481 | — | — | bag 7 |
| `StormSprite` | a levin wisp | 8 | 780–860 | 15–19 | Mage | 58 | 0x0480 | breath:EnergyBreath | — | bag 7 |
| `StormThunderharpy` | a thunder harpy | 8 | 820–900 | 16–20 | Melee | 73 | 0x0492 | — | — | bag 7 |
| `StormWisp` | a charged wisp | 8 | 780–880 | 15–19 | Mage | 58 | 0x0480 | breath:EnergyBreath | — | bag 7 |
| `StormAnemoiGale` | a howling gale | 9 | 2200–2400 | 20–25 | Melee | 13 | 0x0492 | poison-immune | when-struck proc | bag 8 |
| `StormAnemoiHerald` | the anemoi herald | 9 | 2200–2400 | 20–25 | Mage | 165 | 0x0491 | breath:EnergyBreath | — | bag 8 |
| `StormAnemoiNorthwind` | a northwind spirit | 9 | 2150–2380 | 19–24 | Melee | 13 | 0x0480 | poison-immune, breath:ColdBreath | — | bag 8 |
| `StormAnemoiSquall` | a squall-spirit | 9 | 2100–2350 | 19–24 | Mage | 58 | 0x0491 | breath:EnergyBreath | — | bag 8 |
| `StormAnemoiTempest` | the anemoi tempest-lord | 9 | 2300–2400 | 21–26 | Mage | 76 | 0x0492 | poison-immune, breath:EnergyBreath | — | bag 8 |
| `StormAnemoiVortex` | a howling vortex | 9 | 2150–2380 | 19–24 | Melee | 13 | 0x0480 | poison-immune | aura | bag 8 |
| `StormChained` | **Enceladus, the Chainbreaker** | 9 | 2350–2400 | 22–28 | Melee | 75 | 0x0492 | bleed-immune | when-struck proc | elite bag 9 |
| `StormColossus` | a chained colossus | 9 | 2200–2400 | 20–26 | Melee | 75 | 0x0492 | bleed-immune | — | bag 8 |
| `StormDrake` | a thunder drake | 9 | 2100–2350 | 19–24 | Melee | Utility.RandomList | 0x0480 | breath:EnergyBreath | — | bag 8 |
| `StormElemental` | a living tempest | 9 | 2200–2400 | 20–25 | Melee | 13 | 0x0481 | poison-immune, breath:EnergyBreath | — | bag 8 |
| `StormEphialtes` | **Ephialtes, the Storm-Chained** | 9 | 2380–2400 | 22–28 | Mage | 76 | 0x0492 | poison-immune, bleed-immune, breath:EnergyBreath | when-struck proc | elite bag 9 |
| `StormEye` | a tempest eye | 9 | 2200–2400 | 20–25 | Mage | 22 | 0x0491 | — | — | bag 8 |
| `StormFather` | **Typhon, the Hundred-Storm** | 9 | 2380–2400 | 24–30 | Mage | 12 | 0x0492 | poison-immune, bleed-immune, breath:EnergyBreath | when-struck proc, spawns adds | elite bag 8 ×2 |
| `StormGargoyle` | a storm gargoyle | 9 | 2100–2350 | 19–24 | Mage | 4 | 0x0491 | — | — | bag 8 |
| `StormOgreKing` | a storm-forged ogre lord | 9 | 2250–2400 | 21–26 | Melee | 83 | 0x0492 | bleed-immune | on-hit proc | bag 8 |
| `StormSentinel` | a storm sentinel | 9 | 2200–2400 | 20–25 | Melee | 13 | 0x0481 | poison-immune, breath:EnergyBreath | — | bag 8 |
| `StormThunderdrake` | a thunderhead drake | 9 | 2100–2350 | 19–24 | Melee | Utility.RandomList | 0x0492 | breath:EnergyBreath | — | bag 8 |
| `StormTitan` | a chained titan | 9 | 2200–2400 | 20–26 | Melee | 75 | 0x0492 | bleed-immune | aura | bag 8 |
| `StormTitanling` | a lesser titan | 9 | 2200–2400 | 20–25 | Mage | 76 | 0x0492 | breath:EnergyBreath | — | bag 8 |
| `StormWyrm` | a levin wyrm | 9 | 2150–2380 | 19–24 | Melee | Utility.RandomBool | 0x0480 | breath:EnergyBreath | — | bag 8 |

## StygianDeep (36)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `StygianAsphodelKnight` | an asphodel knight | 9 | 2100–2350 | 19–24 | Melee | 147 | 0x08A5 | — | — | bag 8 |
| `StygianBogle` | a mournful bogle | 9 | 2000–2250 | 18–23 | Mage | 153 | 0x0455 | poison-immune | — | bag 8 |
| `StygianCorpse` | a bloated corpse | 9 | 2100–2350 | 19–24 | Melee | 155 | 0x0454 | venom Deadly, bleed-immune | — | bag 8 |
| `StygianDamned` | one of the damned | 9 | 2000–2250 | 18–23 | Melee | 3 | 0x0455 | bleed-immune | — | bag 8 |
| `StygianGhoul` | a starving ghoul | 9 | 2000–2250 | 18–23 | Melee | 153 | 0x0454 | — | — | bag 8 |
| `StygianGraverat` | a rat of Dis | 9 | 2000–2200 | 17–22 | Melee | 0xD7 | 0x0454 | — | — | bag 8 |
| `StygianHound` | a hound of Dis | 9 | 2000–2300 | 18–23 | Melee | 98 | 0x0453 | breath:FireBreath, pack | — | bag 8 |
| `StygianMummy` | a bound mummy | 9 | 2100–2350 | 19–24 | Melee | 154 | 0x08A5 | poison-immune, bleed-immune | — | bag 8 |
| `StygianShade` | an unransomed shade | 9 | 2100–2350 | 19–24 | Mage | 26 | 0x0455 | poison-immune, bleed-immune | on-hit proc | bag 8 |
| `StygianShademoth` | a shade moth | 9 | 2000–2200 | 17–22 | Melee | 39 | 0x0455 | — | — | bag 8 |
| `StygianStyxMage` | a shade of the styx | 9 | 2000–2250 | 18–23 | Mage | 148 | 0x0454 | — | — | bag 8 |
| `StygianWight` | an unransomed wight | 9 | 2000–2250 | 18–23 | Melee | Utility.RandomList | 0x0455 | — | — | bag 8 |
| `StygianWisp` | a wisp of lethe | 9 | 2000–2200 | 17–22 | Mage | 165 | 0x0455 | — | — | bag 8 |
| `StygianWitness` | a silent witness | 9 | 2100–2350 | 19–24 | Mage | 153 | 0x0455 | poison-immune | — | bag 8 |
| `StygianWraith` | a river wraith | 9 | 2000–2250 | 18–23 | Mage | 26 | 0x0455 | poison-immune | on-hit proc | bag 8 |
| `StygianArbiter` | the arbiter of asphodel | 10 | 2700–3000 | 23–29 | Mage | 79 | 0x0454 | poison-immune | when-struck proc, spawns adds | bag 9 |
| `StygianBoneColossus` | a bone colossus | 10 | 2800–3100 | 24–30 | Melee | 104 | 0x08A5 | poison-immune, bleed-immune, breath:ColdBreath | — | bag 9 |
| `StygianBoneKnight` | a bronze bone knight | 10 | 2600–2900 | 23–29 | Melee | 57 | 0x08A5 | bleed-immune | — | bag 9 |
| `StygianBoneLord` | a bronze bone lord | 10 | 2600–2900 | 23–29 | Melee | 57 | 0x08A5 | bleed-immune, pack | when-struck proc, spawns adds | bag 9 |
| `StygianCerberus` | **Cerberus, Warden of the Gate** | 10 | 2900–3100 | 25–31 | Melee | 98 | 0x0453 | poison-immune, bleed-immune, breath:FireBreath | when-struck proc, spawns adds | elite bag 9 |
| `StygianCharon` | **Charon, Ferryman of the Deep** | 10 | 2800–3000 | 24–30 | Mage | 31 | 0x0455 | poison-immune | on-hit proc | elite bag 9 |
| `StygianCondemned` | a condemned shade | 10 | 2500–2800 | 22–28 | Mage | 26 | 0x0455 | poison-immune | — | bag 9 |
| `StygianDaemon` | a stygian daemon | 10 | 2700–3000 | 23–29 | Mage | 9 | 0x0454 | poison-immune, breath:ColdBreath | — | bag 9 |
| `StygianDishound` | a great hound of Dis | 10 | 2500–2800 | 22–28 | Melee | 98 | 0x0453 | breath:FireBreath, pack | — | bag 9 |
| `StygianErinys` | a winged erinys | 10 | 2600–2900 | 23–29 | Mage | 26 | 0x0454 | poison-immune | on-hit proc | bag 9 |
| `StygianHarrower` | a harrower of souls | 10 | 2600–2900 | 23–29 | Melee | 31 | 0x0453 | — | — | bag 9 |
| `StygianJudge` | a judge of the dead | 10 | 2600–2900 | 23–29 | Mage | 24 | 0x0454 | poison-immune | — | bag 9 |
| `StygianLich` | an unhallowed lich | 10 | 2500–2800 | 22–28 | Mage | 24 | 0x0454 | — | — | bag 9 |
| `StygianLichLord` | a lich of Dis | 10 | 2600–2900 | 23–29 | Mage | 79 | 0x0454 | poison-immune | on-hit proc | bag 9 |
| `StygianLord` | **Hades, Lord of the Unseen** | 10 | 3200–3600 | 26–34 | Mage | 9 | 0x0489 | poison-immune, bleed-immune, breath:ColdBreath | when-struck proc, spawns adds | elite bag 10 |
| `StygianReaper` | a soul-reaper | 10 | 2500–2800 | 22–28 | Mage | 24 | 0x0454 | venom Lethal, bleed-immune | on-hit proc | bag 9 |
| `StygianRevenant` | an asphodel revenant | 10 | 2600–2900 | 23–29 | Melee | 155 | 0x0455 | bleed-immune | — | bag 9 |
| `StygianRhadamanthys` | **Rhadamanthys, the Judge-King** | 10 | 3050–3150 | 25–31 | Mage | 79 | 0x0454 | poison-immune, bleed-immune | on-hit proc | elite bag 9 |
| `StygianScourge` | a scourge of the furies | 10 | 2600–2900 | 23–29 | Melee | 31 | 0x0453 | — | on-hit proc | bag 9 |
| `StygianSentinel` | a stygian sentinel | 10 | 2600–2900 | 22–28 | Melee | 15 | 0x0454 | poison-immune, breath:ColdBreath | — | bag 9 |
| `StygianTormentor` | a tormentor shade | 10 | 2500–2800 | 22–28 | Mage | 26 | 0x0454 | — | — | bag 9 |

## Wayman (35)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `WaymanBrigand` | a Wayman brigand | 3 | 150–190 | 8–11 | Melee | 0x191 | 0x0844 | — | — | bag 2 |
| `WaymanCrow` | a gallows crow | 3 | 130–155 | 6–9 | Melee | 39 | 0x0000 | — | — | bag 2 |
| `WaymanCurhound` | a mangy cur | 3 | 130–155 | 6–9 | Melee | 23 | 0x0844 | pack | — | bag 2 |
| `WaymanCutpurse` | a road cutpurse | 3 | 130–160 | 7–10 | Melee | 0x191 | 0x0844 | — | — | bag 2 |
| `WaymanFootpad` | a road footpad | 3 | 130–160 | 7–10 | Melee | 0x191 | 0x0844 | — | — | bag 2 |
| `WaymanRat` | a gutter rat | 3 | 130–155 | 6–9 | Melee | 0xD7 | 0x0964 | — | — | bag 2 |
| `WaymanSlinger` | a Wayman slinger | 3 | 130–160 | 7–10 | Archer | 0x191 | 0x0964 | — | — | bag 2 |
| `WaymanBowman` | a Wayman bowman | 4 | 200–240 | 8–11 | Archer | 764 | 0x0964 | — | — | bag 3 |
| `WaymanCudgeler` | a Wayman cudgeler | 4 | 200–240 | 8–11 | Melee | 764 | 0x0798 | — | — | bag 3 |
| `WaymanIsthmianArcher` | an Isthmian pine-archer | 4 | 200–240 | 8–11 | Archer | 764 | 0x0964 | — | — | bag 3 |
| `WaymanIsthmianCutthroat` | an Isthmian cutthroat | 4 | 200–240 | 8–11 | Melee | 0x191 | 0x0844 | — | — | bag 3 |
| `WaymanRuffian` | a Wayman ruffian | 4 | 200–240 | 8–11 | Melee | 764 | 0x0844 | — | — | bag 3 |
| `WaymanThug` | a Wayman thug | 4 | 200–240 | 8–11 | Melee | 764 | 0x0964 | — | — | bag 3 |
| `WaymanWaylayer` | a Wayman waylayer | 4 | 200–240 | 8–11 | Archer | 764 | 0x0844 | — | — | bag 3 |
| `WaymanEnforcer` | a Wayman enforcer | 5 | 320–360 | 10–14 | Melee | 766 | 0x0964 | — | — | bag 4 |
| `WaymanGearhound` | a clockwork hound | 5 | 320–360 | 10–14 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | bag 4 |
| `WaymanHedgewizard` | a Wayman hedge-wizard | 5 | 320–360 | 10–14 | Mage | 765 | — | — | — | bag 4 |
| `WaymanIsthmianHexer` | an Isthmian hedge-hexer | 5 | 320–360 | 10–14 | Mage | 765 | 0x0000 | — | — | bag 4 |
| `WaymanIsthmianReaver` | an Isthmian reaver | 5 | 320–360 | 10–14 | Melee | 764 | 0x0844 | — | — | bag 4 |
| `WaymanRoadwitch` | a Wayman road-witch | 5 | 320–360 | 10–14 | Mage | 765 | 0x0000 | — | — | bag 4 |
| `WaymanArtificer` | a Talos artificer | 6 | 480–530 | 13–17 | Mage | 400 | 0x0798 | — | when-struck proc | bag 5 |
| `WaymanAutomaton` | a lesser talos | 6 | 460–520 | 12–16 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | bag 5 |
| `WaymanBrigadier` | a Wayman brigadier | 6 | 480–530 | 13–17 | Melee | 766 | 0x0844 | — | — | bag 5 |
| `WaymanBronzeguard` | a bronze sentinel | 6 | 460–520 | 12–16 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | bag 5 |
| `WaymanCaptain` | the Wayman captain | 6 | 480–530 | 13–17 | Melee | 766 | 0x0964 | — | when-struck proc | bag 5 |
| `WaymanCogwright` | a Talos cogwright | 6 | 480–530 | 13–17 | Mage | 400 | 0x0798 | — | — | bag 5 |
| `WaymanIsthmianAutomaton` | an Isthmian bronze-thug | 6 | 460–520 | 12–16 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | bag 5 |
| `WaymanIsthmianBravo` | an Isthmian bravo | 6 | 460–510 | 12–16 | Melee | 766 | 0x0964 | — | — | bag 5 |
| `WaymanIsthmianPinebender` | an Isthmian pine-bender | 6 | 480–530 | 13–17 | Melee | 766 | 0x0844 | — | on-hit proc | bag 5 |
| `WaymanIronclad` | an ironclad automaton | 7 | 600–660 | 14–18 | Melee | 752 | 0x0964 | poison-immune, bleed-immune | — | bag 6 |
| `WaymanIsthmianBedwright` | an Isthmian bed-wright | 7 | 600–660 | 14–18 | Mage | 400 | 0x0964 | — | on-hit proc | bag 6 |
| `WaymanIsthmianCaptain` | the Isthmian toll-captain | 7 | 620–680 | 15–19 | Melee | 766 | 0x0798 | — | when-struck proc, spawns adds | bag 6 |
| `WaymanProcrustes` | **Procrustes, the Stretcher** | 7 | 700–720 | 16–22 | Melee | 752 | 0x0964 | poison-immune, bleed-immune | on-hit proc | elite bag 7 |
| `WaymanWarlord` | the Wayman road-warlord | 7 | 620–680 | 15–19 | Melee | 766 | 0x0798 | — | — | bag 6 |
| `WaymanPeriphetes` | **Periphetes, the Bronze-Cudgel** | 8 | 900–950 | 18–24 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | on-hit proc | elite bag 8 |

## Open World — biomes (255)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `GroveBrambleThornling` | a thornling | 2 | 80–95 | 5–8 | Mage | 128 | 0x0851 | — | — | level-scaled roll |
| `GroveDoe` | a grove doe | 2 | 80–95 | 5–8 | Melee | 0xED | 0x0844 | — | — | level-scaled roll |
| `GroveFangwolf` | a fang wolf | 2 | 85–100 | 5–8 | Melee | 225 | 0x0851 | pack | — | level-scaled roll |
| `GroveFawn` | a dappled fawn | 2 | 80–90 | 5–7 | Melee | 0xED | 0x0851 | — | — | level-scaled roll |
| `GroveFinch` | a grove finch | 2 | 80–88 | 4–6 | Melee | 0xD0 | 0x0851 | — | — | level-scaled roll |
| `GroveForestrat` | a forest rat | 2 | 80–90 | 5–7 | Melee | 0xD7 | 0x0851 | — | — | level-scaled roll |
| `GroveGreywood` | a greywood wolf | 2 | 85–100 | 5–8 | Melee | Utility.RandomList | 0x0851 | pack | — | level-scaled roll |
| `GroveHare` | a grove hare | 2 | 80–88 | 4–6 | Melee | 205 | 0x0851 | — | — | level-scaled roll |
| `GroveKomosFaun` | a kōmos faun | 2 | 85–100 | 5–8 | Melee | 271 | 0x0491 | — | — | level-scaled roll |
| `GroveKomosGoatling` | a kōmos goatling | 2 | 80–95 | 5–8 | Melee | 0xD1 | 0x0851 | — | — | level-scaled roll |
| `GroveLunamoth` | a luna moth | 2 | 80–90 | 4–7 | Melee | 39 | 0x0491 | — | — | level-scaled roll |
| `GroveLynx` | a bracken lynx | 2 | 85–100 | 5–8 | Melee | 63 | 0x0844 | — | — | level-scaled roll |
| `GroveSowthing` | a bristled sow | 2 | 80–95 | 5–8 | Melee | 0xCB | 0x0851 | — | — | level-scaled roll |
| `GroveSprite` | a briar sprite | 2 | 80–95 | 5–8 | Mage | 128 | 0x0491 | — | — | level-scaled roll |
| `GroveStag` | a grove stag | 2 | 80–100 | 5–8 | Melee | 0xEA | 0x0844 | — | — | level-scaled roll |
| `GroveThornwolf` | a thornwolf | 2 | 85–100 | 5–8 | Melee | Utility.RandomList | 0x0844 | pack | — | level-scaled roll |
| `GroveThrush` | a grove thrush | 2 | 80–90 | 4–7 | Melee | 0xD0 | 0x0844 | — | — | level-scaled roll |
| `GroveTusker` | a young tusker | 2 | 85–100 | 5–8 | Melee | 0x122 | 0x0844 | — | — | level-scaled roll |
| `GroveViper` | a bracken viper | 2 | 80–95 | 5–8 | Melee | 52 | 0x0851 | venom Regular | — | level-scaled roll |
| `GroveWebspinner` | a grove webspinner | 2 | 85–100 | 5–8 | Melee | 28 | 0x0851 | venom Lesser | — | level-scaled roll |
| `GroveWillowisp` | a willow wisp | 2 | 80–95 | 5–8 | Mage | 58 | 0x0491 | — | — | level-scaled roll |
| `RestlessBonefinch` | a graveyard finch | 2 | 80–88 | 4–6 | Melee | 0xD0 | 0x0835 | — | — | level-scaled roll |
| `RestlessBonerat` | a bone-gnawing rat | 2 | 80–95 | 5–8 | Melee | 0xD7 | 0x0835 | — | — | level-scaled roll |
| `RestlessBones` | a heap of bones | 2 | 80–100 | 5–8 | Melee | Utility.RandomList | 0x0835 | — | — | level-scaled roll |
| `RestlessCarrionbat` | a carrion bat | 2 | 80–95 | 5–8 | Melee | 39 | 0x0455 | — | — | level-scaled roll |
| `RestlessCrow` | a graveyard crow | 2 | 80–90 | 4–7 | Melee | 5 | 0x0455 | — | — | level-scaled roll |
| `RestlessCryptbat` | a crypt bat | 2 | 80–90 | 4–7 | Melee | 39 | 0x0455 | — | — | level-scaled roll |
| `RestlessGloom` | a graveyard gloom | 2 | 80–95 | 5–8 | Mage | 26 | 0x0847 | bleed-immune | — | level-scaled roll |
| `RestlessGnawer` | a gnawing ghoul | 2 | 85–100 | 5–8 | Melee | 153 | 0x0481 | — | — | level-scaled roll |
| `RestlessGravemoth` | a grave moth | 2 | 80–90 | 4–7 | Melee | 39 | 0x0454 | — | — | level-scaled roll |
| `RestlessGraverat` | a grave rat | 2 | 80–95 | 5–8 | Melee | 0xD7 | 0x0847 | — | — | level-scaled roll |
| `RestlessHeadless` | a headless dead | 2 | 85–100 | 5–8 | Melee | 31 | 0x0481 | — | — | level-scaled roll |
| `RestlessHusk` | a withered husk | 2 | 85–100 | 5–8 | Melee | 3 | 0x0835 | bleed-immune | — | level-scaled roll |
| `RestlessRattler` | a rattling skeleton | 2 | 80–100 | 5–8 | Melee | Utility.RandomList | 0x0847 | — | — | level-scaled roll |
| `RestlessRotling` | a rotting corpse | 2 | 85–100 | 5–8 | Melee | 3 | 0x0847 | bleed-immune | — | level-scaled roll |
| `RestlessSkeleton` | a restless skeleton | 2 | 80–100 | 5–8 | Melee | Utility.RandomList | 0x0835 | — | — | level-scaled roll |
| `RestlessZombie` | an unburied zombie | 2 | 85–100 | 5–8 | Melee | 3 | 0x0847 | bleed-immune | — | level-scaled roll |
| `GroveAdder` | a green adder | 3 | 130–150 | 7–10 | Melee | 52 | 0x0844 | venom Greater | — | level-scaled roll |
| `GroveApe` | a grove ape | 3 | 130–155 | 7–11 | Melee | 0x1D | 0x0851 | — | — | level-scaled roll |
| `GroveBrambleHound` | a bramble hound | 3 | 130–155 | 7–11 | Melee | 225 | 0x0491 | pack | — | level-scaled roll |
| `GroveBrambleNymph` | a bramble nymph | 3 | 125–150 | 7–10 | Mage | 128 | 0x0491 | — | — | level-scaled roll |
| `GroveBrambleQueen` | the bramble queen | 3 | 155–160 | 8–11 | Mage | 128 | 0x0491 | — | on-hit proc | level-scaled roll |
| `GroveBrambleTreant` | a bramble treant | 3 | 150–160 | 7–11 | Mage | 47 | 0x0851 | — | — | level-scaled roll |
| `GroveBrambleWarden` | a bramble warden | 3 | 150–160 | 7–11 | Melee | 8 | 0x0491 | venom Regular | — | level-scaled roll |
| `GroveBriarboar` | a briar boar | 3 | 130–155 | 7–11 | Melee | 0x122 | 0x0844 | — | — | level-scaled roll |
| `GroveDryad` | a grove dryad | 3 | 125–150 | 7–10 | Mage | 128 | 0x0851 | — | — | level-scaled roll |
| `GroveElk` | a grove elk | 3 | 135–160 | 7–11 | Melee | 0xEA | 0x0844 | — | — | level-scaled roll |
| `GroveFaun` | a grove faun | 3 | 130–155 | 7–11 | Melee | 271 | 0x0851 | — | — | level-scaled roll |
| `GroveHuntwolf` | a hunt wolf | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0844 | pack | — | level-scaled roll |
| `GroveKomosBacchant` | a kōmos bacchant | 3 | 135–160 | 7–11 | Melee | 271 | 0x0491 | — | — | level-scaled roll |
| `GroveKomosDancer` | a kōmos dancer | 3 | 130–155 | 7–11 | Melee | 271 | 0x0851 | — | — | level-scaled roll |
| `GroveKomosHornmaster` | the kōmos hornmaster | 3 | 150–160 | 8–11 | Melee | 271 | 0x0491 | — | when-struck proc, spawns adds | level-scaled roll |
| `GroveKomosReveler` | a kōmos reveler | 3 | 135–160 | 7–11 | Melee | 271 | 0x0491 | — | on-hit proc | level-scaled roll |
| `GroveMossbear` | a moss-pelt bear | 3 | 140–160 | 7–11 | Melee | 211 | 0x0851 | — | — | level-scaled roll |
| `GroveNettleback` | a nettleback spider | 3 | 125–150 | 7–10 | Melee | 28 | 0x0851 | venom Lesser | — | level-scaled roll |
| `GrovePanther` | a grove panther | 3 | 130–155 | 7–11 | Melee | 0xD6 | 0x0851 | — | — | level-scaled roll |
| `GrovePiper` | a satyr piper | 3 | 135–160 | 7–11 | Melee | 271 | 0x0491 | — | on-hit proc | level-scaled roll |
| `GroveRazorback` | a razorback boar | 3 | 135–160 | 7–11 | Melee | 0x122 | 0x0851 | — | — | level-scaled roll |
| `GroveSaplingreaper` | a sapling reaper | 3 | 135–160 | 7–11 | Mage | 47 | 0x0851 | — | — | level-scaled roll |
| `GroveSatyr` | a wood satyr | 3 | 130–155 | 7–11 | Melee | 271 | 0x0851 | — | — | level-scaled roll |
| `GroveShaggybear` | a shaggy bear | 3 | 140–160 | 7–11 | Melee | 167 | 0x0844 | — | — | level-scaled roll |
| `GroveSilenos` | Silenos, the Reveler-King | 3 | 155–160 | 8–11 | Melee | 271 | 0x0491 | — | on-hit proc | elite bag 3 |
| `GroveStinger` | a bracken stinger | 3 | 130–155 | 7–11 | Melee | 48 | 0x0851 | venom Regular | — | level-scaled roll |
| `GroveThornspider` | a thornback spider | 3 | 130–155 | 7–11 | Melee | 28 | 0x0844 | venom Regular | — | level-scaled roll |
| `GroveThornvine` | a thorn creeper | 3 | 135–160 | 7–11 | Melee | 8 | 0x0851 | — | — | level-scaled roll |
| `GroveWhitehart` | a white hart | 3 | 140–160 | 7–11 | Melee | 0xEA | 0x0851 | — | — | level-scaled roll |
| `GroveWisp` | a grove wisp | 3 | 125–150 | 7–10 | Mage | 58 | 0x0844 | — | — | level-scaled roll |
| `RestlessBanshee` | a keening banshee | 3 | 135–160 | 7–11 | Mage | 26 | 0x0454 | bleed-immune | — | level-scaled roll |
| `RestlessBarrowking` | **the Barrow-King** | 3 | 155–160 | 8–11 | Melee | 147 | 0x0454 | — | when-struck proc, spawns adds | elite bag 3 |
| `RestlessBellringer` | a corpse bellringer | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0847 | — | — | level-scaled roll |
| `RestlessBogle` | a weeping bogle | 3 | 125–150 | 7–10 | Mage | 153 | 0x0454 | — | — | level-scaled roll |
| `RestlessBonearcher` | a bone archer | 3 | 130–155 | 7–11 | Archer | Utility.RandomList | 0x0835 | — | — | level-scaled roll |
| `RestlessBoneguard` | a bone guard | 3 | 145–160 | 7–11 | Melee | 147 | 0x0835 | — | — | level-scaled roll |
| `RestlessBonemage` | a bone conjurer | 3 | 130–155 | 7–11 | Mage | 148 | 0x0454 | — | — | level-scaled roll |
| `RestlessBonewalker` | a shambling bone-walker | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0847 | — | — | level-scaled roll |
| `RestlessBoundone` | a rag-bound dead | 3 | 145–160 | 7–11 | Melee | 154 | 0x0847 | poison-immune, bleed-immune | — | level-scaled roll |
| `RestlessCadaver` | a bloated cadaver | 3 | 140–160 | 7–11 | Melee | 155 | 0x0847 | bleed-immune | — | level-scaled roll |
| `RestlessFeaster` | a corpse feaster | 3 | 135–155 | 7–11 | Melee | 153 | 0x0847 | — | — | level-scaled roll |
| `RestlessGhast` | a graveyard ghast | 3 | 130–155 | 7–11 | Melee | 153 | 0x0454 | — | — | level-scaled roll |
| `RestlessGhoul` | a graveyard ghoul | 3 | 130–155 | 7–11 | Melee | 153 | 0x0481 | bleed-immune | — | level-scaled roll |
| `RestlessGravebound` | a grave-bound warrior | 3 | 150–160 | 7–11 | Melee | 57 | 0x0454 | bleed-immune | — | level-scaled roll |
| `RestlessGravedigger` | an unburied gravedigger | 3 | 130–155 | 7–11 | Melee | 3 | 0x0481 | bleed-immune | — | level-scaled roll |
| `RestlessKeener` | a keening widow | 3 | 130–155 | 7–11 | Mage | 26 | 0x0481 | bleed-immune | — | level-scaled roll |
| `RestlessLegionArcher` | a barrow archer | 3 | 130–155 | 7–11 | Archer | Utility.RandomList | 0x0847 | — | — | level-scaled roll |
| `RestlessLegionKnight` | a barrow knight | 3 | 150–160 | 7–11 | Melee | 147 | 0x0454 | — | — | level-scaled roll |
| `RestlessLegionMarshal` | the barrow marshal | 3 | 155–160 | 8–11 | Melee | 57 | 0x0454 | bleed-immune | when-struck proc, spawns adds | level-scaled roll |
| `RestlessLegionPikeman` | a barrow pikeman | 3 | 150–160 | 7–11 | Melee | 57 | 0x0481 | bleed-immune | — | level-scaled roll |
| `RestlessLegionSoldier` | a barrow legionary | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0454 | — | — | level-scaled roll |
| `RestlessLegionStandard` | a barrow standard-bearer | 3 | 150–160 | 7–11 | Melee | 147 | 0x0847 | — | — | level-scaled roll |
| `RestlessLegionnaire` | a fallen legionnaire | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0454 | — | — | level-scaled roll |
| `RestlessMourner` | a veiled mourner | 3 | 130–155 | 7–11 | Mage | 26 | 0x0454 | bleed-immune | — | level-scaled roll |
| `RestlessMummy` | a shrouded mummy | 3 | 145–160 | 7–11 | Melee | 154 | 0x0481 | poison-immune, bleed-immune | — | level-scaled roll |
| `RestlessPallbearer` | a spectral pallbearer | 3 | 140–160 | 7–11 | Melee | 3 | 0x0455 | bleed-immune | — | level-scaled roll |
| `RestlessPhantom` | a grave phantom | 3 | 125–150 | 7–10 | Mage | 26 | 0x0455 | bleed-immune | — | level-scaled roll |
| `RestlessPsychopomp` | the mourners' psychopomp | 3 | 150–160 | 8–11 | Mage | 26 | 0x0455 | bleed-immune | when-struck proc, spawns adds | level-scaled roll |
| `RestlessSexton` | the cortège sexton | 3 | 150–160 | 7–11 | Mage | Utility.RandomList | 0x0454 | — | on-hit proc | level-scaled roll |
| `RestlessShade` | a mourning shade | 3 | 125–150 | 7–10 | Mage | 26 | 0x0455 | bleed-immune | — | level-scaled roll |
| `RestlessSpecter` | a pale specter | 3 | 125–150 | 7–10 | Mage | 26 | 0x0455 | bleed-immune | — | level-scaled roll |
| `RestlessVampirebat` | a crypt vampire bat | 3 | 130–150 | 7–10 | Melee | 317 | 0x0454 | — | — | level-scaled roll |
| `RestlessWailer` | a wailing wraith | 3 | 130–155 | 7–11 | Mage | 26 | 0x0455 | bleed-immune | — | level-scaled roll |
| `RestlessWight` | a barrow wight | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0454 | — | — | level-scaled roll |
| `RestlessWraith` | a keening wraith | 3 | 130–160 | 7–11 | Mage | 26 | 0x0455 | bleed-immune | on-hit proc | level-scaled roll |
| `ShoreBrinespawn` | a brine spawn | 3 | 130–155 | 7–11 | Melee | 16 | 0x0530 | poison-immune | — | level-scaled roll |
| `ShoreCormorant` | a cormorant | 3 | 125–145 | 7–10 | Melee | 5 | 0x0530 | — | — | level-scaled roll |
| `ShoreCrab` | a giant shore crab | 3 | 130–155 | 7–11 | Melee | 48 | 0x0530 | — | — | level-scaled roll |
| `ShoreDrownedSailor` | a wrecked sailor | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0481 | — | — | level-scaled roll |
| `ShoreFiddler` | a fiddler crab | 3 | 125–145 | 7–10 | Melee | 48 | 0x0481 | — | — | level-scaled roll |
| `ShoreGull` | a great gull | 3 | 125–150 | 7–10 | Melee | 39 | 0x0481 | — | — | level-scaled roll |
| `ShoreHarpy` | a shore harpy | 3 | 130–155 | 7–11 | Melee | 30 | 0x0481 | — | — | level-scaled roll |
| `ShoreHermit` | a giant hermit crab | 3 | 130–155 | 7–11 | Melee | 48 | 0x0851 | — | — | level-scaled roll |
| `ShoreLampbearer` | a false-light lurer | 3 | 130–155 | 7–11 | Melee | 3 | 0x0481 | bleed-immune | — | level-scaled roll |
| `ShoreReefspear` | a reef spearman | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0481 | — | — | level-scaled roll |
| `ShoreReefstalker` | a reef stalker | 3 | 130–155 | 7–11 | Melee | Utility.RandomList | 0x0851 | — | — | level-scaled roll |
| `ShoreRockcrab` | a rock crab | 3 | 130–155 | 7–11 | Melee | 48 | 0x0530 | — | — | level-scaled roll |
| `ShoreSandcrawler` | a sand crawler | 3 | 125–150 | 7–10 | Melee | 48 | 0x0481 | — | — | level-scaled roll |
| `ShoreSandflea` | a sand flea | 3 | 125–145 | 7–10 | Melee | 39 | 0x0530 | — | — | level-scaled roll |
| `ShoreSandpiper` | a sandpiper | 3 | 125–145 | 7–10 | Melee | 5 | 0x0481 | — | — | level-scaled roll |
| `ShoreSeal` | a bull seal | 3 | 130–155 | 7–11 | Melee | 0xDD | 0x0530 | — | — | level-scaled roll |
| `ShoreSeaslime` | a tidal slime | 3 | 125–150 | 7–10 | Melee | 51 | 0x0851 | venom Regular, poison-immune | — | level-scaled roll |
| `ShoreShorerat` | a shore rat | 3 | 125–150 | 7–10 | Melee | 0xD7 | 0x0530 | — | — | level-scaled roll |
| `ShoreSpineeel` | a spined eel | 3 | 125–150 | 7–10 | Melee | 52 | 0x0530 | venom Regular | — | level-scaled roll |
| `ShoreTideeel` | a tide eel | 3 | 125–150 | 7–10 | Melee | 52 | 0x0481 | venom Regular | — | level-scaled roll |
| `ShoreUrchin` | a giant sea urchin | 3 | 125–150 | 7–10 | Melee | 51 | 0x0851 | venom Greater, poison-immune | — | level-scaled roll |
| `PeakBear` | a mountain bear | 4 | 200–235 | 11–15 | Melee | 212 | 0x0455 | — | — | level-scaled roll |
| `PeakBronzeEttin` | a bronze-age ettin | 4 | 210–238 | 12–16 | Melee | 18 | 0x0798 | — | — | level-scaled roll |
| `PeakBronzeMastiff` | a bronze mastiff | 4 | 200–235 | 11–15 | Melee | 98 | 0x0798 | pack | — | level-scaled roll |
| `PeakBronzeWatchman` | a bronze watchman | 4 | 210–238 | 12–16 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | — | level-scaled roll |
| `PeakCondor` | a mountain condor | 4 | 190–210 | 10–14 | Melee | 5 | 0x0492 | — | — | level-scaled roll |
| `PeakCragwolf` | a crag wolf | 4 | 190–225 | 11–15 | Melee | 25 | 0x0492 | pack | — | level-scaled roll |
| `PeakCyclopsYoung` | a young cyclops | 4 | 210–238 | 12–16 | Melee | 75 | 0x0455 | — | — | level-scaled roll |
| `PeakEaglet` | a crag eaglet | 4 | 190–210 | 10–14 | Melee | 5 | 0x0455 | — | — | level-scaled roll |
| `PeakEttin` | a crag ettin | 4 | 200–238 | 12–16 | Melee | 18 | 0x0455 | — | — | level-scaled roll |
| `PeakGazerling` | a crag gazer larva | 4 | 190–225 | 11–15 | Mage | 778 | 0x0455 | — | — | level-scaled roll |
| `PeakGetShepherd` | a cyclops shepherd | 4 | 210–238 | 12–16 | Melee | 18 | 0x0492 | — | — | level-scaled roll |
| `PeakLion` | a mountain lion | 4 | 195–230 | 11–15 | Melee | 63 | 0x0455 | — | — | level-scaled roll |
| `PeakOgre` | a mountain ogre | 4 | 210–238 | 12–16 | Melee | 1 | 0x0455 | — | — | level-scaled roll |
| `PeakPika` | a crag pika | 4 | 190–210 | 10–14 | Melee | 0xD7 | 0x0455 | — | — | level-scaled roll |
| `PeakRam` | a horned ram | 4 | 190–225 | 11–15 | Melee | 88 | 0x0455 | — | — | level-scaled roll |
| `PeakRoc` | a mountain roc | 4 | 200–235 | 12–16 | Melee | 30 | 0x0492 | — | — | level-scaled roll |
| `PeakRocFledgling` | a roc fledgling | 4 | 200–235 | 12–16 | Melee | 30 | 0x0492 | — | — | level-scaled roll |
| `PeakRockhound` | a rock hound | 4 | 195–230 | 11–15 | Melee | 98 | 0x0455 | pack | — | level-scaled roll |
| `PeakSnowcat` | a snow leopard | 4 | 195–230 | 11–15 | Melee | 64 | 0x0492 | — | — | level-scaled roll |
| `PeakStoneling` | a stone elemental | 4 | 210–238 | 12–16 | Melee | 14 | 0x0455 | bleed-immune | — | level-scaled roll |
| `PeakStonetroll` | a stone troll | 4 | 210–238 | 12–16 | Melee | 55 | 0x0455 | — | — | level-scaled roll |
| `PeakTur` | a wild tur | 4 | 190–225 | 11–15 | Melee | 212 | 0x0455 | — | — | level-scaled roll |
| `PeakWildgoat` | a wild mountain goat | 4 | 190–210 | 10–14 | Melee | 88 | 0x0455 | — | — | level-scaled roll |
| `ShoreBarnacleback` | a barnacle-back crab | 4 | 210–238 | 12–16 | Melee | 48 | 0x0851 | bleed-immune | — | level-scaled roll |
| `ShoreBrineling` | a brineling | 4 | 210–238 | 11–16 | Melee | 16 | 0x0530 | poison-immune | — | level-scaled roll |
| `ShoreBrineserpent` | a shoal serpent | 4 | 210–238 | 12–16 | Melee | 0x15 | 0x0851 | venom Greater | — | level-scaled roll |
| `ShoreCoralthing` | a coral horror | 4 | 210–238 | 11–16 | Melee | 16 | 0x0851 | poison-immune | — | level-scaled roll |
| `ShoreDeepGuard` | a deep-court guard | 4 | 210–238 | 12–16 | Melee | Utility.RandomList | 0x0530 | — | — | level-scaled roll |
| `ShoreDeepHound` | a deep-court hound | 4 | 200–235 | 11–15 | Melee | 98 | 0x0530 | pack | — | level-scaled roll |
| `ShoreDeepNereid` | a deep nereid | 4 | 205–235 | 11–16 | Melee | 30 | 0x0481 | — | — | level-scaled roll |
| `ShoreDeepOracle` | a deep-court oracle | 4 | 205–235 | 11–16 | Mage | 0x190 | 0x0481 | — | — | level-scaled roll |
| `ShoreDeepPrince` | the prince of the deep | 4 | 230–238 | 12–16 | Melee | Utility.RandomList | 0x0491 | — | on-hit proc | level-scaled roll |
| `ShoreDeepTideguard` | a tide-guard of the deep | 4 | 210–238 | 11–16 | Melee | 16 | 0x0530 | poison-immune | — | level-scaled roll |
| `ShoreDeepone` | a deep-one raider | 4 | 205–238 | 12–16 | Melee | Utility.RandomList | 0x0530 | — | — | level-scaled roll |
| `ShoreKarkinos` | the Karkinos | 4 | 235–240 | 13–16 | Melee | 48 | 0x0851 | bleed-immune | on-hit proc | elite bag 4 |
| `ShoreKingcrab` | a king crab | 4 | 205–238 | 12–16 | Melee | 48 | 0x0851 | — | — | level-scaled roll |
| `ShoreLuresiren` | a luring siren | 4 | 205–238 | 12–16 | Melee | 30 | 0x0491 | — | on-hit proc | level-scaled roll |
| `ShoreMoray` | a moray eel | 4 | 205–238 | 12–16 | Melee | 52 | 0x0481 | venom Greater | — | level-scaled roll |
| `ShoreReaver` | a wreck reaver | 4 | 205–238 | 12–16 | Melee | 153 | 0x0530 | — | — | level-scaled roll |
| `ShoreReefserpent` | a cove serpent | 4 | 205–238 | 12–16 | Melee | 0x15 | 0x0851 | — | — | level-scaled roll |
| `ShoreReefviper` | a reef viper | 4 | 200–235 | 11–16 | Melee | 52 | 0x0851 | venom Greater | — | level-scaled roll |
| `ShoreSeaSerpent` | a coastal serpent | 4 | 210–238 | 12–16 | Melee | 0x15 | 0x0530 | — | — | level-scaled roll |
| `ShoreSeacow` | a lumbering sea-cow | 4 | 205–235 | 11–15 | Melee | 0xDD | 0x0481 | — | — | level-scaled roll |
| `ShoreSeahag` | a sea hag | 4 | 205–235 | 11–16 | Mage | 0x190 | 0x0481 | — | — | level-scaled roll |
| `ShoreSeaharpy` | a sea harpy | 4 | 200–235 | 11–16 | Melee | 30 | 0x0530 | — | — | level-scaled roll |
| `ShoreShrieker` | a shrieking siren | 4 | 205–238 | 12–16 | Melee | 30 | 0x0491 | — | — | level-scaled roll |
| `ShoreSiren` | a shoal siren | 4 | 200–235 | 11–16 | Melee | 30 | 0x0481 | — | — | level-scaled roll |
| `ShoreSpinecrab` | a spine crab | 4 | 205–238 | 12–16 | Melee | 48 | 0x0530 | venom Regular | — | level-scaled roll |
| `ShoreTidal` | a tidal elemental | 4 | 210–238 | 11–16 | Melee | 16 | 0x0530 | poison-immune | — | level-scaled roll |
| `ShoreTidewitch` | a tide witch | 4 | 205–235 | 11–16 | Mage | 0x190 | 0x0491 | — | — | level-scaled roll |
| `ShoreWreckLord` | the wreck-captain | 4 | 230–238 | 12–16 | Melee | Utility.RandomList | 0x0491 | bleed-immune | when-struck proc | level-scaled roll |
| `ShoreWreckWitch` | a wreck-witch | 4 | 205–238 | 12–16 | Mage | 0x190 | 0x0491 | — | on-hit proc | level-scaled roll |
| `ShoreWrecker` | a drowned wrecker | 4 | 205–238 | 12–16 | Melee | 3 | 0x0530 | bleed-immune | — | level-scaled roll |
| `MireAdder` | a marsh adder | 5 | 290–330 | 11–16 | Melee | 52 | 0x0851 | venom Greater | — | level-scaled roll |
| `MireAlligator` | a mire alligator | 5 | 300–350 | 12–17 | Melee | 0xCA | 0x0844 | — | — | level-scaled roll |
| `MireBlacksnake` | a black fen snake | 5 | 290–330 | 11–16 | Melee | 52 | 0x0844 | venom Greater | — | level-scaled roll |
| `MireBloodfly` | a fen bloodfly | 5 | 290–320 | 11–15 | Melee | 39 | 0x0851 | — | — | level-scaled roll |
| `MireBogserpent` | a bog serpent | 5 | 310–360 | 13–18 | Melee | 0x15 | 0x0851 | venom Greater, poison-immune | — | level-scaled roll |
| `MireBullfrog` | a mire bullfrog | 5 | 290–320 | 10–14 | Melee | 81 | 0x0851 | poison-immune | — | level-scaled roll |
| `MireCroc` | a fen crocodile | 5 | 300–350 | 12–17 | Melee | 0xCA | 0x0844 | — | — | level-scaled roll |
| `MireDragonfly` | a bog dragonfly | 5 | 290–320 | 10–14 | Melee | 39 | 0x0851 | — | — | level-scaled roll |
| `MireHeron` | a marsh heron | 5 | 290–320 | 10–14 | Melee | 254 | 0x0844 | — | — | level-scaled roll |
| `MireHornbeast` | a fen gaman | 5 | 300–350 | 12–17 | Melee | 248 | 0x0844 | — | — | level-scaled roll |
| `MireLeech` | a giant bog leech | 5 | 290–330 | 11–16 | Melee | 51 | 0x0844 | venom Greater, poison-immune | — | level-scaled roll |
| `MireLernaCultist` | a Lerna cultist | 5 | 290–330 | 11–16 | Mage | 0x190 | 0x0851 | — | — | level-scaled roll |
| `MireLernaThrall` | a Lerna thrall | 5 | 300–350 | 11–16 | Melee | 3 | 0x0851 | poison-immune, bleed-immune | — | level-scaled roll |
| `MireLizardman` | a fen lizardman | 5 | 290–330 | 11–16 | Melee | Utility.RandomList | 0x0844 | — | — | level-scaled roll |
| `MireMuckElemental` | a muck elemental | 5 | 320–370 | 12–17 | Melee | 16 | 0x0844 | poison-immune | — | level-scaled roll |
| `MireMudrat` | a mud rat | 5 | 290–320 | 10–14 | Melee | 0xD7 | 0x0844 | — | — | level-scaled roll |
| `MireRatmage` | a fen ratman shaman | 5 | 290–330 | 11–16 | Mage | 0x8F | 0x0851 | — | — | level-scaled roll |
| `MireRatman` | a fen ratman | 5 | 290–330 | 11–16 | Melee | 42 | 0x0844 | — | — | level-scaled roll |
| `MireScaledArcher` | a scaled archer | 5 | 290–330 | 11–16 | Archer | Utility.RandomList | 0x0851 | — | — | level-scaled roll |
| `MireScaledHunter` | a scaled hunter | 5 | 290–330 | 11–16 | Melee | Utility.RandomList | 0x0851 | — | — | level-scaled roll |
| `MireScaledSpear` | a scaled spearman | 5 | 300–350 | 12–17 | Melee | Utility.RandomList | 0x0844 | — | — | level-scaled roll |
| `MireSerpentspawn` | a lernaean spawn | 5 | 310–360 | 13–18 | Melee | 0x15 | 0x0851 | venom Greater, poison-immune | — | level-scaled roll |
| `MireSkulker` | a mire skulker | 5 | 290–330 | 11–16 | Melee | Utility.RandomList | 0x0844 | — | — | level-scaled roll |
| `MireSlimer` | a mire slime | 5 | 290–330 | 11–16 | Melee | 51 | 0x0851 | venom Regular, poison-immune | — | level-scaled roll |
| `MireStinger` | a marsh stinger | 5 | 290–330 | 11–16 | Melee | 48 | 0x0851 | venom Deadly | — | level-scaled roll |
| `MireSwampspider` | a swamp spider | 5 | 290–330 | 11–16 | Melee | 28 | 0x0844 | venom Greater | — | level-scaled roll |
| `MireToad` | a bloated marsh toad | 5 | 290–340 | 11–16 | Melee | 80 | 0x0851 | venom Regular, poison-immune | — | level-scaled roll |
| `MireToadspawn` | a warty toad | 5 | 290–340 | 11–16 | Melee | 80 | 0x0851 | venom Regular, poison-immune | — | level-scaled roll |
| `PeakBoulderbear` | a boulder bear | 5 | 320–360 | 13–18 | Melee | 212 | 0x0455 | — | — | level-scaled roll |
| `PeakBronzeArcher` | a bronze archer | 5 | 320–360 | 13–18 | Archer | 4 | 0x0798 | — | — | level-scaled roll |
| `PeakBronzeCaptain` | the bronze watch-captain | 5 | 350–380 | 15–19 | Melee | 83 | 0x0798 | bleed-immune | on-hit proc | level-scaled roll |
| `PeakBronzeSentry` | a bronze sentry | 5 | 330–380 | 13–18 | Melee | 67 | 0x0798 | poison-immune, bleed-immune | — | level-scaled roll |
| `PeakBronzeWarden` | a bronze warden | 5 | 340–380 | 14–19 | Melee | 752 | 0x0798 | poison-immune, bleed-immune | when-struck proc | level-scaled roll |
| `PeakCliffgargoyle` | a cliff gargoyle | 5 | 320–360 | 13–18 | Mage | 4 | 0x0492 | — | — | level-scaled roll |
| `PeakCragOgre` | a crag ogre | 5 | 320–370 | 13–18 | Melee | 1 | 0x0455 | — | — | level-scaled roll |
| `PeakCragtroll` | a crag troll | 5 | 330–375 | 14–18 | Melee | 55 | 0x0455 | — | — | level-scaled roll |
| `PeakCyclops` | a bronze cyclops | 5 | 330–380 | 14–19 | Melee | 75 | 0x0798 | bleed-immune | — | level-scaled roll |
| `PeakElderGazer` | an elder crag gazer | 5 | 340–380 | 14–19 | Mage | 22 | 0x0455 | — | — | level-scaled roll |
| `PeakFrostbear` | a frost bear | 5 | 320–360 | 13–18 | Melee | 213 | 0x0492 | — | — | level-scaled roll |
| `PeakGetBoulderthrow` | a boulder-thrower | 5 | 330–375 | 14–18 | Archer | 18 | 0x0455 | — | — | level-scaled roll |
| `PeakGetChieftain` | the Get chieftain | 5 | 355–380 | 15–19 | Melee | 75 | 0x0798 | bleed-immune | on-hit proc | level-scaled roll |
| `PeakGetElder` | a cyclops elder | 5 | 340–380 | 14–19 | Melee | 75 | 0x0455 | bleed-immune | — | level-scaled roll |
| `PeakGetHerdsman` | a cyclops herdsman | 5 | 330–370 | 14–18 | Melee | 75 | 0x0492 | bleed-immune | — | level-scaled roll |
| `PeakGetRam` | the clan's great ram | 5 | 320–355 | 13–18 | Melee | 88 | 0x0455 | — | — | level-scaled roll |
| `PeakGraniteGolem` | a granite golem | 5 | 330–380 | 13–18 | Melee | 752 | 0x0455 | poison-immune, bleed-immune | — | level-scaled roll |
| `PeakGriffonharpy` | a crag griffon | 5 | 330–360 | 13–18 | Melee | 30 | 0x0455 | — | — | level-scaled roll |
| `PeakOgreBrute` | an ogre brute | 5 | 330–380 | 14–19 | Melee | 1 | 0x0455 | — | — | level-scaled roll |
| `PeakOgrelord` | a crag ogre lord | 5 | 340–380 | 14–19 | Melee | 83 | 0x0455 | bleed-immune | — | level-scaled roll |
| `PeakOldCyclops` | an old cyclops | 5 | 330–380 | 14–19 | Melee | 75 | 0x0455 | bleed-immune | — | level-scaled roll |
| `PeakSnowwolf` | a snow wolf | 5 | 320–355 | 13–18 | Melee | 34 | 0x0492 | pack | — | level-scaled roll |
| `PeakStoneEttin` | a stone-hide ettin | 5 | 320–370 | 13–18 | Melee | 18 | 0x0455 | — | — | level-scaled roll |
| `PeakStonegazer` | a crag gazer | 5 | 320–370 | 13–18 | Mage | 22 | 0x0455 | — | — | level-scaled roll |
| `PeakStormroc` | a squall roc | 5 | 320–360 | 13–18 | Melee | 30 | 0x0492 | — | — | level-scaled roll |
| `PeakTalos` | Talos, the Bronze Warden | 5 | 375–380 | 15–20 | Melee | 75 | 0x0798 | poison-immune, bleed-immune | on-hit proc | elite bag 5 |
| `PeakThunderroc` | a thunder roc | 5 | 320–360 | 13–18 | Melee | 30 | 0x0492 | — | on-hit proc | level-scaled roll |
| `PeakTwoheadEttin` | a two-head crag ettin | 5 | 330–375 | 14–18 | Melee | 18 | 0x0492 | — | — | level-scaled roll |
| `MireAcidThing` | an acid elemental | 6 | 470–520 | 15–20 | Melee | 0x9E | 0x0851 | poison-immune | — | level-scaled roll |
| `MireBlackgator` | a black-scale gator | 6 | 460–510 | 15–20 | Melee | 0xCA | 0x0844 | — | — | level-scaled roll |
| `MireBloattoad` | a bloated toad | 6 | 460–510 | 15–20 | Melee | 80 | 0x0844 | venom Greater, poison-immune | — | level-scaled roll |
| `MireBoghulk` | a bog hulk | 6 | 480–540 | 15–20 | Melee | 780 | 0x0851 | poison-immune | — | level-scaled roll |
| `MireBogreaper` | a bog reaper | 6 | 480–540 | 15–20 | Mage | 47 | 0x0851 | poison-immune, breath:PoisonBreath | — | level-scaled roll |
| `MireBogthing` | a bog horror | 6 | 470–520 | 15–20 | Melee | 780 | 0x0844 | poison-immune | — | level-scaled roll |
| `MireCoilserpent` | a coil serpent | 6 | 480–540 | 15–20 | Melee | 0x15 | 0x0491 | venom Deadly, poison-immune | — | level-scaled roll |
| `MireDrowned` | a fen-drowned corpse | 6 | 460–510 | 15–20 | Melee | 155 | 0x0844 | venom Greater, poison-immune, bleed-immune | — | level-scaled roll |
| `MireFenhorror` | a fen horror | 6 | 470–520 | 15–20 | Melee | 780 | 0x0844 | poison-immune | — | level-scaled roll |
| `MireFenmother` | the Fen-Mother | 6 | 540–550 | 16–20 | Melee | 0x15 | 0x0491 | venom Deadly, poison-immune | when-struck proc, spawns adds | elite bag 6 |
| `MireGreatgator` | a great mire gator | 6 | 470–520 | 15–20 | Melee | 0xCA | 0x0851 | — | — | level-scaled roll |
| `MireHydraspawn` | a hissing hydra-spawn | 6 | 480–540 | 15–20 | Melee | 0x15 | 0x0491 | venom Deadly, poison-immune | when-struck proc, spawns adds | level-scaled roll |
| `MireLernaBrute` | a Lerna enforcer | 6 | 480–520 | 15–20 | Melee | Utility.RandomList | 0x0491 | — | — | level-scaled roll |
| `MireLernaHierophant` | the Lerna hierophant | 6 | 490–540 | 15–20 | Mage | 0x190 | 0x0491 | poison-immune | on-hit proc | level-scaled roll |
| `MireLernaServant` | a Lerna hydra-servant | 6 | 480–540 | 15–20 | Melee | 0x15 | 0x0491 | venom Deadly, poison-immune | — | level-scaled roll |
| `MireLernaZealot` | a Lerna zealot | 6 | 470–520 | 15–20 | Mage | 0x190 | 0x0491 | venom Deadly, poison-immune | — | level-scaled roll |
| `MireLizardBrute` | a fen lizard brute | 6 | 470–520 | 15–20 | Melee | Utility.RandomList | 0x0851 | — | — | level-scaled roll |
| `MireOozeElemental` | a poison ooze | 6 | 480–540 | 15–20 | Mage | 162 | 0x0851 | poison-immune | — | level-scaled roll |
| `MireScaledBrute` | a mire brute | 6 | 480–520 | 15–20 | Melee | Utility.RandomList | 0x0491 | — | — | level-scaled roll |
| `MireScaledKing` | the scaled war-king | 6 | 490–540 | 15–20 | Melee | Utility.RandomList | 0x0491 | — | when-struck proc, spawns adds | level-scaled roll |
| `MireScaledShaman` | a scaled shaman | 6 | 470–520 | 15–20 | Mage | Utility.RandomList | 0x0491 | poison-immune | — | level-scaled roll |
| `MireStranglevine` | a strangle-vine | 6 | 460–510 | 15–20 | Melee | 8 | 0x0851 | venom Greater | — | level-scaled roll |
| `MireWidow` | a fen widow | 6 | 470–510 | 15–20 | Melee | 0x9D | 0x0844 | venom Deadly | — | level-scaled roll |

## Open World — The Labors (6)

| Class | Display | Lvl | HP | Dmg | AI | Body | Hue | Traits | Abilities | Loot |
|---|---|---|---|---|---|---|---|---|---|---|
| `LaborCeryneianHind` | the Ceryneian Hind | 4 | 230–240 | 12–16 | Melee | 0xEA | 0x0486 | — | when-struck proc | elite bag 4 |
| `LaborCalydonianBoar` | the Calydonian Boar | 5 | 365–380 | 13–18 | Melee | 0x122 | 0x021E | — | on-hit proc | elite bag 5 |
| `LaborNemeanLion` | the Nemean Lion | 6 | 530–550 | 15–20 | Melee | 63 | 0x0798 | bleed-immune | when-struck proc | elite bag 6 |
| `LaborStymphalianHarpy` | the Stymphalian Matriarch | 6 | 530–550 | 15–20 | Melee | 30 | 0x08A5 | — | when-struck proc, spawns adds | elite bag 6 |
| `LaborErymanthianBoar` | the Erymanthian Boar | 7 | 690–720 | 17–22 | Melee | 0x122 | 0x0481 | — | on-hit proc | elite bag 7 |
| `LaborCretanBull` | the Cretan Bull | 8 | 900–950 | 20–25 | Melee | 0xE9 | 0x0021 | bleed-immune | on-hit proc | elite bag 8 |
## Pantheon Tamables (20) — added 2026-07-15, post-generation

Taming lane (`dev-docs/tamables.md`): 1 offensive pet + 1 rideable mount per ladder
dungeon. All `Tamable`, `ControlSlots 1`, pinned level **0** (no XP, no loot bag),
pet-budget stats. Sorted by MinTameSkill.

| Class | Display | Tame | HP | Dmg | Kind | Body | Hue |
|---|---|---|---|---|---|---|---|
| `GaianOrn` | a gaian orn | 45.1 | 85–105 | 5–8 | mount (desert ostard) | 0xD2 | 0x0972 |
| `TideSteed` | a tide-born steed | 47.1 | 95–115 | 6–9 | mount (horse) | 0xE2 | 0x0481 |
| `GaianEarthbear` | an earthborn bear | 55.1 | 110–130 | 8–12 | pet (grizzly) | 212 | 0x0972 |
| `DrownedCharger` | a drowned charger | 55.1 | 110–140 | 7–10 | mount (horse) | 0xCC | 0x0835 |
| `TideBull` | a sea-born bull | 59.1 | 130–150 | 9–13 | pet (bull) | 0xE8/0xE9 | 0x0847 |
| `CinderSteed` | a cinder steed | 63.1 | 140–170 | 8–11 | mount (horse) | 0xC8 | 0x0654 |
| `DrownedHound` | a barrow hound | 65.1 | 150–180 | 10–14 | pet (dire wolf) | 23 | 0x0841 |
| `BrineOclock` | a brine oclock | 67.1 | 160–190 | 9–12 | mount (forest ostard) | 0xDB | 0x04F8 |
| `CinderHound` | a forge hound | 71.1 | 190–230 | 11–15 | pet (hellhound) | 98 | 0x0654 |
| `BrineLynx` | a brine lynx | 75.1 | 220–260 | 12–16 | pet (panther) | 0xD6 | 0x0480 |
| `WyldCourser` | a moon-marked courser | 77.1 | 190–220 | 10–13 | mount (horse) | 0xE4 | 0x0486 |
| `DrakonZostrich` | a drakon zostrich | 80.1 | 210–250 | 11–14 | mount (frenzied ostard) | 0xDA | 0x0501 |
| `WyldCub` | a nemean cub | 82.1 | 260–310 | 13–17 | pet (panther) | 0xD6 | 0x0501 |
| `TartarusZostrich` | a tartarus zostrich | 85.1 | 240–290 | 12–15 | mount (frenzied ostard) | 0xDA | 0x0021 |
| `DrakonBroodling` | a drakon broodling | 87.1 | 300–350 | 14–18 | pet (drake) | 60/61 | 0x0501 |
| `StormZostrich` | a storm zostrich | 90.1 | 280–330 | 12–16 | mount (frenzied ostard) | 0xDA | 0x0481 |
| `TartarusHellcat` | a tartarus hellcat | 92.1 | 350–400 | 15–19 | pet (panther) | 0xD6 | 0x0021 |
| `StygianNightmare` | a stygian nightmare | 95.1 | 320–380 | 13–17 | mount (horse) | 0xE4 | 0x0455 |
| `StormDrakeling` | a storm drakeling | 96.1 | 400–460 | 16–20 | pet (drake) | 60/61 | 0x0480 |
| `StygianWhelp` | a cerberus whelp | 98.7 | 460–500 | 17–20 | pet (hellhound) | 98 | 0x0453 |
