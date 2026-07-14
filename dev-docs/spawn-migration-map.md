# Spawn Migration Map — Age of Titans (changeover plan v1, 2026-07-14)

> The complete verdict table for replacing **all hostile spawns** with Greek-themed creatures
> while leaving the neutral world intact. Input: the 595 distinct spawned types extracted from
> every `Distribution/Data/Spawns/{uoml,shared,uoml}/felucca` file
> (`scratchpad/spawn-inventory.tsv`). Companion to `dev-docs/bestiary-master.md` (the taxonomy
> + gap-family stubs referenced as `NEW:<Family>.<role>` below).
>
> **Counts are TYPE counts, not spawn counts.** The inventory's number column is total
> `maxCount` across files; a type in six files is one row here. Level parity uses the
> `LevelConfig.MobLevelFromHits` HP heuristic for stock types; a replacement must land within
> **±1 level** of the stock type so area difficulty holds.

## Verdict legend

| Verdict | Meaning |
|---|---|
| **KEEP-NEUTRAL** | reagents, crops, items, vendors/townsfolk, farm animals, natural wildlife — untouched |
| **OURS-KEEP** | already one of our ~277 custom creatures (spawned via the `uoml` family files) |
| **REPLACE → `Class`** | stock hostile covered by an existing custom class — swap directly |
| **REPLACE → `NEW:Fam.role`** | needs a gap-family member (design stub in bestiary-master §6) |
| **FLAG** | era-mismatch, water-only, or quest/event-bound — decide before touching |

---

## A. KEEP-NEUTRAL (untouched)

Terse — grouped. Do **not** open town/vendor files. Every row below stays as-is.

| Group | Types (representative) | Source files |
|---|---|---|
| Reagents (8) | BlackPearl, Bloodmoss, SpidersSilk, SulfurousAsh, Garlic, Ginseng, Nightshade, MandrakeRoot | shared/Reagents.json |
| Crops (6) | FarmableWheat, FarmableCabbage, FarmableCarrot, FarmableTurnip, FarmableCotton, FarmableOnion | shared/FelCropsLS.json |
| Treasure (5) | TreasureChestLevel1–4, treasurelevel1h | shared (dungeon + towns) |
| Vendors & guildmasters (~70) | Banker, Provisioner, Armorer, Weaponsmith, Mage, Tailor, Blacksmith, Healer, Cook, Tinker, Scribe, Bowyer, all `*Guildmaster`, Fisherman, InnKeeper, TavernKeeper, Waiter, Barkeeper, Rancher, Farmer, AnimalTrainer, Beekeeper, Artist, HarborMaster, bodysculptor, dockmaster, boatpainter, hirepeasant, hiresailor, hirefighter, Merchant, Peasant, Noble, Bard, Minter, Cobbler, Weaver, Butcher, Baker, Herbalist, Carpenter, Architect, RealEstateBroker, Mapmaker, Tanner, Furtrader, Jeweler, CustomHairstylist, BrideGroom | shared/Vendors.json, uoml/Vendors.json |
| Townsfolk & hirelings (~30) | TownCrier, SeekerOfAdventure, EscortableMage, WanderingHealer, escortablewanderinghealer, OrderGuard, ChaosGuard, all `Hire*` (Bard/Fighter/Mage/Ranger/Sailor/Thief/Paladin/Beggar/…), the named Khaldun-area townsfolk (Alelle, Daelas, athialon, Olaeni, aneen, Bolaevin, Aluniol, Rebinil, abbein, taellia, vicaie, mallew, jothan, alethanian, Tyeelor) | shared/TownsPeople.json, shared/TownsLife.json, Vendors |
| Décor / gatherable | MiniatureMushroom | shared/TownsLife.json |
| Farm animals (L0-pinned) | Sheep, Horse, Pig, Goat, MountainGoat, Cow, Bull, Boar, Chicken, Dog, Cat, Hind, GreatHart, Rabbit, JackRabbit, Llama, RidableLlama, ForestOstard, DesertOstard | Outdoors/WildLife/LostLands/TownsLife |
| Passive wildlife | Bird, TropicalBird, Eagle, Gorilla, Walrus, Ferret, Squirrel | Outdoors/WildLife/LostLands |
| Sea fauna (passive) | Dolphin, SeaHorse | shared/SeaLife.json |

**Predator wildlife — KEEP-NEUTRAL but FLAGGED for your overrule** (natural fauna, but they
already award XP on the HP curve — `LevelConfig` intentionally leaves predators leveled). If you
want the *whole* natural world converted, these are the borderline set:

| Predators (keep unless you say otherwise) | Level (HP) |
|---|---|
| GreyWolf, TimberWolf, WhiteWolf, DireWolf, SnowLeopard, Cougar, Panther | L2–3 |
| BrownBear, BlackBear, GrizzlyBear, PolarBear | L3 |
| FrenziedOstard | L3 |

> `DireWolf` also appears in the Orc Caves lane — there it converts (see §D.6, the Lykaian
> hounds); as free overworld fauna it stays.

---

## B. OURS-KEEP (already themed — do not touch)

Every type sourced **solely from a `uoml/<family>.json` file** is one of our ~277 custom
creatures and stays. This is the bulk of the high-row-number inventory. By family file:

| Family file | Prefix / rows | Verdict |
|---|---|---|
| uoml/newbie-dungeon.json | `Newbie*` (BoneShade, GraveRat, CorpseCrawler, GraveMiasma, RestlessArcher, Charon, FallenChampion, HollowWarden, Ferryman) | OURS-KEEP |
| uoml/open-world-families.json | `Grove*` `Peak*` `Mire*` `Restless*` `Shore*` | OURS-KEEP |
| uoml/labors.json | `Labor*` (6 hunts) | OURS-KEEP |
| uoml/drowned-tholos.json | `Tide*` | OURS-KEEP |
| uoml/cinderworks.json | `Cinder*` | OURS-KEEP |
| uoml/nemean-wildwood.json | `Wyld*` | OURS-KEEP |
| uoml/stormcrown-aerie.json | `Storm*` | OURS-KEEP |
| uoml/stygian-deep.json | `Stygian*` | OURS-KEEP |
| uoml/despise-gaian.json | `Gaian*`, Chthonios | OURS-KEEP |
| uoml/deceit-drowned.json | `Drowned*`, Minos | OURS-KEEP |
| uoml/shame-brine.json | `Brine*`, Glaukos | OURS-KEEP |
| uoml/destard-drakon.json | `Drakon*`, Pythios | OURS-KEEP |
| uoml/hythloth-tartarus.json | `Tartarus*`, Eurynomos | OURS-KEEP |
| uoml/classic-elites.json | Enkelados, Aiakos, Thaumas, Ladon, Alastor + Echidna brood (GigasAdder, GraveAsp, **BrineSerpent**, DrakescalePython, AshenBasilisk) | OURS-KEEP |

> **Name collision watch:** `BrineSerpent` in `classic-elites.json` is OUR Echidna-brood skin,
> not a stock type — keep it. The `WyldHound / CinderThrall / TideDrudge / NewbieBoneShade /
> RestlessSkeleton / StormHarpy / StygianShade` rows in the inventory are just the
> first-listed member of each family file; all OURS-KEEP.
>
> `WyldHound` also carries a `uoml/nemean-wildwood.json` source with a stock-looking name
> only by coincidence — it is our class. No stock `WyldHound` exists.

**≈270 rows OURS-KEEP.**

---

## C. FLAG — decide before touching

| Type | Area | Why flagged | Recommendation |
|---|---|---|---|
| **All Prism of Light** — Wisp, CrystalWisp, CrystalLatticeSeeker, CrystalVortex, CrystalDaemon, ShadowWisp, CrystalHydra, CrystalSeaSerpent, UnfrozenMummy, Protector, ShadowWyrm, IceElemental, IceSnake, Ferret | shared/PrismOfLight.json | ML/SE-era area on a T2A shard | disable area or theme last (bestiary-master §6.10) |
| **All Palace of Paroxysmus** — PlagueSpawn, PlagueBeast, PlagueBeastLord, CorrosiveSlime, Putrefier, InterredGrizzle, Moloch, ChaosDaemon, Succubus, AcidElemental, PoisonElemental, EarthElemental, Daemon | shared/PalaceOfParoxysmus.json | ML-era area | disable or theme last |
| **All Blighted Grove** — Thrasher, Coil, Tangle, Saliva, Abscess, Hydra, InsaneDryad, WhippingVine, SwampTentacle, BogThing, Bogling, Corpser, Reaper, Harpy, Changeling, GiantSerpent, Alligator, Snake, GiantToad, SilverSerpent | shared/BlightedGrove.json | ML-era area | disable, or convert the swamp lane to `Mire*` if kept |
| **Sanctuary set-pieces** — Szavetra, MougGuur, Chiikkaha, Doppleganger, Changeling, GargoyleEnforcer, GargoyleDestroyer | shared/Sanctuary.json | ML savage/juka content | disable or theme with a later pass |
| Wisp | Outdoors, PrismOfLight | classically neutral-ambiguous fauna | KEEP as neutral unless you want it hostile-themed |
| Kraken, SeaSerpent, DeepSeaSerpent | SeaLife, Shame | water-only (deep ocean spawners) | leave; ocean is out of the land changeover (Poseidon's own) |
| HarrowerTentacles, TavaraSewel, GrimmochDrummel, LysanderGathenwale, MorgBergen | Khaldun | quest/event-bound named (the Khaldun questline) | fold into `Cursed*` §D.3 only if the quest is retired; else keep |
| Jwilson | Outdoors | named unique / test spawn | verify before touching |
| Titan (overworld), Lich/LichLord (overworld graveyards) | Outdoors/Graveyards | HP-level outliers >1 above the local biome band | leave as rare overworld anchors, or add a family elite later |

---

## D. REPLACE — by world area

Existing custom classes named exactly; gap-family members as `NEW:Fam.role` (roster TBD,
bestiary-master §6). Level column = stock HP-level → replacement level (must be within ±1).

### D.1 Graveyards → `Restless*` (existing, L2–3)
File: shared/Graveyards.json (+ overworld graveyard spawns).

| Stock | Area | Replacement | Lvl stock→new | Note |
|---|---|---|---|---|
| Skeleton | Graveyards | RestlessSkeleton | 2→2 | ✓ |
| Zombie | Graveyards | RestlessZombie | 2→2 | ✓ |
| Ghoul | Graveyards | RestlessGhoul | 3→3 | ✓ |
| Spectre | Graveyards | RestlessShade | 3→3 | ✓ |
| Shade | Graveyards | RestlessShade | 3→3 | ✓ |
| Wraith | Graveyards | RestlessWraith | 3→3 | ✓ |
| Mummy | Graveyards | RestlessWight | 4→3 | −1, in tolerance |
| Lich, LichLord | Graveyards | FLAG | 5/7 | overworld boss outlier — leave or add a Restless elite later |

### D.2 Deceit → `Drowned*` (existing, L4–5)
File: shared/Deceit.json. Deceit is the Hades-lane undead dungeon; the Classic-Five `Drowned*`
family already spawns here additively — this swaps the stock lane onto it.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Skeleton | DrownedDead | 2→4 | band lift (Deceit is L5); use DrownedMournling (L4) for fodder |
| Zombie | DrownedDead | 2→4 | " |
| Ghoul | DrownedMournling | 3→4 | −1 ok |
| Spectre | DrownedShade | 3→4 | ✓ |
| Wraith | DrownedWailer | 3→5 | skirmisher caster |
| BoneKnight, SkeletalKnight | DrownedLegionary / DrownedOathbreaker | 4/5→5 | ✓ |
| SkeletalMage, BoneMagi | DrownedWailer / DrownedLamentor | 3→5 | caster lane |
| Mummy | DrownedBrackishHulk | 4→4 | ✓ |
| Lich | Minos (mini-boss) or DrownedLamentor | 5→5 | ✓ |
| PoisonElemental, FireElemental (Deceit) | route to `Rime*`/`Pyre*` or Drowned caster | 5→5 | elemental outliers |

### D.3 Despise → `Gaian*` (existing, L3–4)
File: shared/Despise.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Lizardman | GaianSpartos | 3→3 | ✓ the Spartoi core |
| Ettin | GaianPhalangite / GaianEarthbloodChampion | 4→4 | ✓ |
| Ogre | GaianClayborn / GaianTitanWard | 4→3/4 | ✓ |
| OgreLord | Chthonios (elite) or GaianTitanWard | 6→4 | band-cap; Despise is L4 |
| Troll | GaianPhalangite | 4→4 | ✓ |
| Cyclops | GaianEarthbloodChampion | 5→4 | −1 ok |
| EarthElemental | GaianClayServitor | 4→3 | −1 ok |
| AcidElemental | FLAG (era/Paroxysmus lane) | 4 | — |

### D.4 Shame → `Brine*` (existing, L5–6)
File: shared/Shame.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| WaterElemental | BrineSpume | 4→5 | ✓ |
| AirElemental | BrineGale | 4→5 | ✓ |
| SnowElemental | BrineSeep | 4→5 | ✓ |
| IceElemental | BrineMaelstrom | 4→6 | tank caster |
| BloodElemental | Glaukos (elite) — kept as Shame anchor (pinned L6) | 6→6 | already OURS-adjacent |
| FireElemental | route to `Pyre*` | 5→— | fire outlier |
| PoisonElemental, AcidElemental | FLAG / `Mire*` | 5 | — |
| Kraken, SeaSerpent | FLAG (water-only) | — | ocean |
| Scorpion | BrineSeep / ShoreCrab | 2→5 | fodder lift |
| DullCopperElemental | BrineSurge | 4→6 | ore-elemental reskin |
| EvilMage, EvilMageLord | route to `Cursed*` or Brine caster | 3/4 | human caster outlier |

### D.5 Destard → `Drakon*` (existing, L6–7)
File: shared/Destard.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Drake | DrakonWhelp | 6→6 | ✓ |
| Dragon | Pythios (elite) or DrakonFlamewing | 7→7 | ✓ |
| Wyvern | DrakonWyrmling | 5→7 | band lift (Destard is L7) |
| GiantSerpent | DrakonSerpentling | 3→6 | serpent-man lane |
| WaterElemental | route to `Brine*` | 4→— | outlier |
| FireElemental | route to `Pyre*` | 5→— | outlier |
| EvilMage | DrakonFlamespeaker | 3→7 | cult caster |
| AncientWyrm, ShadowWyrm | FLAG (boss outliers) | 9 | Destard deep-roost bosses — leave or make a Drakon boss |

### D.6 Hythloth → `Tartarus*` (existing, L7–8)
File: shared/Hythloth.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Imp | TartarusImp | 2→7 | band lift (Hythloth is L8) |
| Gargoyle | TartarusGargoyle | 3→7 | ✓ |
| StoneGargoyle | TartarusStonewrath | 4→8 | ✓ |
| FireGargoyle | TartarusFiend | 5→8 | ✓ |
| Daemon | TartarusFiend | 6→8 | ✓ |
| Balron | Eurynomos (elite) / Alastor | 8→8 | ✓ |
| HellHound | route to `Pyre*` | 3→— | fire outlier or TartarusSoulgorger |
| GazerLarva, Gazer, ElderGazer | route to `Argus*` | 1/3/6 | eye lane → Covetous family |

### D.7 Covetous → `NEW:Argus*` (gap 6.7, L3–9)
File: shared/Covetous.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Gazer | NEW:Argus.eye | 3→3 | the all-seeing motif |
| GazerLarva | NEW:Argus.spawn | 1→3 | −band, fodder |
| ElderGazer | NEW:Argus.overseer | 6→6 | ✓ |
| Harpy, StoneHarpy | NEW:Argus.harpy | 3/4→4 | vault flyers |
| Corpser | NEW:Argus.snare | 3→3 | vault plant |
| HeadlessOne | NEW:Argus.thrall | 1→3 | fodder |
| Skeleton, Zombie, Spectre, Shade, Wraith, Mummy, RottingCorpse | NEW:Argus.deadhunter (undead treasure-hunters) | 1–4→4 | ✓ |
| GiantSpider, DreadSpider | NEW:Argus.vaultspider | 3/5→5 | ✓ |
| Slime | NEW:Argus.ooze | 1→3 | fodder |
| Lich | NEW:Argus.hoardmage | 5→6 | caster |
| Drake, Dragon | NEW:Argus.hoardwyrm | 6/7→7 | hoard-guardian dragon |
| WaterElemental | route to `Brine*` | 4→— | outlier |

### D.8 Fire → `NEW:Pyre*` (gap 6.1, L5–8)
Files: shared/Fire.json (+ Trinsic Passage fire, §D.14).

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| FireElemental | NEW:Pyre.elemental | 5→5 | ✓ |
| HellHound, HellCat | NEW:Pyre.hound | 3→5 | pack skirmisher |
| LavaSnake, LavaLizard | NEW:Pyre.crawler | 2→5 | fodder lift |
| LavaSerpent | NEW:Pyre.serpent | 3→6 | ✓ |
| Efreet | NEW:Pyre.efreet | 5→6 | caster |
| Daemon (Fire) | NEW:Pyre.daemon | 6→7 | ✓ |
| SkeletalMage, EvilMage, EvilMageLord (Fire) | NEW:Pyre.cultist | 3/4→6 | the unburnt cult |
| Lich, LichLord (Fire) | NEW:Pyre.pyromancer | 5/7→7 | caster |
| GiantSerpent, GiantRat, Slime (Fire) | NEW:Pyre.crawler / fodder | 1–3→5 | fodder |

### D.9 Ice → `NEW:Rime*` (gap 6.2, L4–9)
File: shared/Ice.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Ratman, RatmanArcher, RatmanMage | NEW:Rime.thrall / .archer / .shaman | 3/3/4→4 | rime-bound tribe |
| IceSnake, IceSerpent | NEW:Rime.serpent | 2/3→4 | skirmisher |
| SnowElemental, IceElemental | NEW:Rime.elemental | 4→5 | ✓ |
| FrostOoze | NEW:Rime.ooze | 2→4 | fodder |
| FrostSpider | NEW:Rime.spider | 3→4 | ✓ |
| FrostTroll | NEW:Rime.giant | 5→6 | tank |
| IceFiend | NEW:Rime.fiend | 6→6 | caster |
| ArcticOgreLord | NEW:Rime.warlord | 7→7 | mini-boss |
| WhiteWyrm | NEW:Rime.wyrm | 8→8 | boss |

### D.10 Khaldun → `NEW:Cursed*` (gap 6.3, L5–9)
File: shared/Khaldun.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Skeleton, Zombie | NEW:Cursed.digger | 2→5 | the cursed expedition |
| BoneKnight, SkeletalKnight | NEW:Cursed.sentinel | 4/5→6 | ✓ |
| BoneMagi, SkeletalMage | NEW:Cursed.necromancer | 3→6 | caster |
| SpectralArmour | NEW:Cursed.armour | 4→6 | ✓ |
| ShadowFiend | NEW:Cursed.shade | 2→5 | fodder |
| KhaldunZealot, KhaldunSummoner | NEW:Cursed.zealot / .summoner | 4→7 | Hecate's cult |
| Cursed | NEW:Cursed.accursed | 3→6 | ✓ |
| AncientLich | NEW:Cursed.hierophant | 9→9 | boss |
| HarrowerTentacles, Khaldun named NPCs | FLAG (quest) | — | see §C |

### D.11 Solen Hive + Terathan swarm → `NEW:Myrmi*` (gap 6.4, L3–7)
Files: shared/SolenHive.json, shared/TerathanKeep.json (insect lane).

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| BlackSolenWorker | NEW:Myrmi.worker | 2→3 | ✓ |
| BlackSolenWarrior, RedSolenInfiltratorWarrior | NEW:Myrmi.warrior | 3→4 | pack instinct |
| BlackSolenQueen, RedSolenInfiltratorQueen | NEW:Myrmi.queen | 5→6 | summoner mini-boss |
| AntLion | NEW:Myrmi.antlion | 3→4 | ✓ |
| Beetle | NEW:Myrmi.drone | 2→3 | fodder |
| TerathanDrone | NEW:Myrmi.drone | 3→3 | ✓ |
| TerathanWarrior | NEW:Myrmi.warrior | 4→4 | ✓ |
| TerathanAvenger | NEW:Myrmi.avenger | 5→6 | ✓ |
| TerathanMatriarch | NEW:Myrmi.queen | 6→6 | ✓ |

### D.12 Terathan Keep serpents → `NEW:Ophian*` (gap 6.5, L4–9)
File: shared/TerathanKeep.json (serpent lane).

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| OphidianWarrior | NEW:Ophian.warrior | 4→4 | ✓ |
| OphidianKnight | NEW:Ophian.knight | 5→5 | ✓ |
| OphidianMage | NEW:Ophian.priest | 5→5 | caster |
| OphidianArchmage | NEW:Ophian.archpriest | 6→6 | ✓ |
| OphidianMatriarch | NEW:Ophian.matriarch | 6→7 | mini-boss |
| Nightmare | NEW:Ophian.mount | 5→6 | ✓ |
| Balron | route to `Tartarus*` (Eurynomos-tier) or Ophian boss | 8→8 | Keep boss |
| Dragon, Drake | route to `Drakon*` | 6/7→— | dragon garrison |

### D.13 Orc Caves → `NEW:Lykai*` (gap 6.6, L2–6)
File: shared/OrcCaves.json (+ overworld orc/rat camps).

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Orc | NEW:Lykai.raider | 3→3 | ✓ |
| OrcishMage | NEW:Lykai.shaman | 4→4 | caster |
| OrcishLord, OrcCaptain | NEW:Lykai.warleader | 4→5 | ✓ |
| OrcBomber | NEW:Lykai.skirmisher | 3→3 | ✓ |
| OrcBrute | NEW:Lykai.brute | 4→5 | tank |
| orcscout | NEW:Lykai.scout | 2→2 | ✓ |
| OrcCamp, RatCamp (Outdoors) | NEW:Lykai spawn-group | — | camp reskin |
| DireWolf | NEW:Lykai.hound | 3→3 | the cult's pack |
| EarthElemental | route to `Gaian*` | 4→— | outlier |
| Corpser, GiantRat | NEW:Lykai fodder | 1/3→3 | ✓ |

### D.14 Trinsic Passage → `NEW:Pyre*` (gap 6.1) + Britain Sewer → `Mire*`

| Stock | Area | Replacement | Lvl | Note |
|---|---|---|---|---|
| FireElemental | Trinsic Passage | NEW:Pyre.elemental | 5→5 | ✓ |
| LavaSnake, LavaLizard | Trinsic Passage | NEW:Pyre.crawler | 2→5 | fodder lift |
| HellCat | Trinsic Passage | NEW:Pyre.hound | 3→5 | ✓ |
| DreadSpider | Trinsic Passage | route to `Argus*`/`Mire*` | 5→5 | spider |
| Rat, SewerRat | Britain Sewer | MireSkulker / fodder | 0/3→5 | sewer vermin (or KEEP as ambient) |
| Alligator, BullFrog | Britain Sewer | MireAlligator / MireToad | 3/2→5 | ✓ |
| GiantRat | Britain Sewer | fodder | 3→— | KEEP-ambient option |

### D.15 Wrong → `NEW:Wayman*` (gap 6.8, L3–8)
File: shared/Wrong.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Brigand | NEW:Wayman.brigand | 3→3 | ✓ (also overworld Brigand) |
| JukaWarrior | NEW:Wayman.thug | 4→4 | ✓ |
| JukaLord | NEW:Wayman.captain | 6→6 | mini-boss |
| JukaMage | NEW:Wayman.hedgewizard | 5→5 | caster |
| Golem | NEW:Wayman.automaton (lesser Talos) | 5→6 | construct |
| GolemController | NEW:Wayman.artificer | 5→6 | boss pair |

### D.16 Painted Caves → `NEW:Pelasg*` (gap 6.9, L2–4, foldable)
File: shared/PaintedCaves.json.

| Stock | Replacement | Lvl | Note |
|---|---|---|---|
| Troglodyte | NEW:Pelasg.caveman | 3→3 | ✓ |
| Grobu, Lurg | NEW:Pelasg.chieftain (named leaders) | 4→4 | mini-boss pair |
| Rat, Dog, Cat, GiantRat | fodder / KEEP-ambient | 0–3 | vermin |

### D.17 Overworld (Outdoors / WildLife / Lost Lands) — by creature type
The roaming hostiles convert by **type wherever they spawn**, routed to the biome family or
existing themed class that matches their nature. (Biome families already spawn here additively;
this swaps the stock type onto its themed equivalent.)

| Stock | Nature | Replacement | Lvl stock→new | Note |
|---|---|---|---|---|
| Orc, OrcishMage, OrcishLord, OrcCaptain, OrcBomber, Savage, SavageRider, SavageShaman | humanoid warband | NEW:Lykai.* | 3–4 | wild-man raiders |
| Brigand | human bandit | NEW:Wayman.brigand | 3→3 | ✓ |
| Ettin, Troll, Ogre | giant-kin | PeakBronzeEttin / PeakCragOgre | 4→4/5 | ✓ |
| OgreLord, Cyclops | greater giant | PeakCyclops | 5/6→5 | ✓ |
| Titan | titan | FLAG (outlier) | 7 | rare anchor — leave or Peak elite later |
| Harpy | flyer | PeakRoc | 3→4 | ✓ |
| StoneHarpy | flyer | PeakThunderroc | 4→5 | ✓ |
| Gazer, ElderGazer | eye | NEW:Argus.eye / .overseer | 3/6 | eye lane |
| Lizardman | reptile humanoid | GaianSpartos (dry) / MireSkulker (swamp) | 3→3/5 | context |
| Ratman | rat humanoid | NEW:Lykai.rat | 3→3 | wretch |
| HeadlessOne, Mongbat, StrongMongbat | weak fodder | GroveThornwolf / Restless fodder | 1–2 | nearest biome fodder |
| Reaper, Corpser, Bogling, BogThing, SwampTentacle, WhippingVine | plant/swamp | MireBogthing / MireHydraspawn | 2–4→5/6 | swamp lane |
| Snake, GiantSerpent, SilverSerpent | serpent | MireSerpentspawn / ShoreReefserpent | 1–4→5 | ✓ |
| GiantSpider, SpittingSpider | spider | GroveNettleback | 3→3 | ✓ |
| Scorpion | arachnid | ShoreCrab / GroveNettleback | 2→3 | ✓ |
| Alligator | reptile | MireAlligator | 3→5 | ✓ |
| GiantToad, BullFrog | amphibian | MireToad | 2/4→5 | ✓ |
| Imp | fiendling | NEW:Pyre.imp | 2→5 | fire lane |
| HellHound | fiend hound | NEW:Pyre.hound | 3→5 | ✓ |
| Gargoyle, StoneGargoyle | gargoyle | route to `Tartarus*` / `Argus*` | 3/4 | context |
| Daemon, Succubus | daemon | route to `Tartarus*` / `Pyre*` | 6 | fiend lane |
| Dragon, Drake, Wyvern, SwampDragon | dragon | route to `Drakon*` | 5–7 | dragon lane |
| Lich, LichLord | undead caster | FLAG (overworld outlier) | 5/7 | rare anchor |
| AirElemental | air | route to `Storm*` (StormGale) | 4→8 | band lift; or Rime |
| EarthElemental | earth | GaianClayServitor | 4→3 | ✓ |
| WaterElemental | water | ShoreBrineling / BrineSpume | 4→4/5 | ✓ |
| FireElemental | fire | NEW:Pyre.elemental | 5→5 | ✓ |
| Slime | ooze | nearest-area fodder | 1 | context |
| Wisp | fey | KEEP-neutral / FLAG | — | ambiguous |
| Ophidian* (Lost Lands) | serpent-men | NEW:Ophian.* | 4–6 | ✓ |
| Terathan* (Lost Lands) | insectoid | NEW:Myrmi.* | 3–6 | ✓ |
| FrostTroll, IceSnake, IceSerpent, SnowElemental (Lost Lands/Outdoors) | frost | NEW:Rime.* | 2–5 | frost lane |
| LavaSnake, LavaLizard, LavaSerpent, Efreet, HellCat (Lost Lands) | fire | NEW:Pyre.* | 2–5 | fire lane |
| StoneGargoyle, StoneHarpy (Lost Lands/Covetous) | stone | NEW:Argus.* / Peak | 4 | context |
| Cyclops, FrostTroll, ArcticOgreLord | giant | Peak / Rime | 5–7 | ✓ |
| Wyvern, Nightmare, Balron (Lost Lands/Keep) | high fiend/dragon | Drakon / Ophian / Tartarus | 5–8 | context |

---

## E. Verdict summary (type counts)

| Verdict | ~Types | Share |
|---|---|---|
| KEEP-NEUTRAL | ~140 | reagents, crops, treasure, vendors, townsfolk, farm + passive wildlife, flagged predators |
| OURS-KEEP | ~270 | the `uoml` family files (~277 classes minus dupes) |
| REPLACE → existing class | ~55 | Graveyards, Deceit, Despise, Shame, Destard, Hythloth lanes + overworld biome routing |
| REPLACE → NEW gap-family | ~75 | Fire, Ice, Khaldun, Solen/Terathan, Terathan Keep, Orc Caves, Covetous, Wrong, Painted Caves |
| FLAG | ~55 | Prism/Paroxysmus/Blighted Grove/Sanctuary (era), water-only, quest/event, HP outliers |
| **Total** | **~595** | — |

> The three REPLACE-adjacent buckets overlap where a type spawns in several areas (e.g. Skeleton
> maps differently in Graveyards vs Deceit vs Covetous vs Khaldun); counts are per unique type
> at its dominant area, so the columns are indicative, not a clean partition of 595.

---

## F. Phased changeover plan (lowest-risk order)

| Phase | Scope | Status |
|---|---|---|
| Phase 1 | Graveyards + Classic Five | ✓ EXECUTED 2026-07-14 |
| Phase 2 | Overworld routing | ✓ EXECUTED 2026-07-14 |
| Phase 3 | Gap families | ✓ EXECUTED 2026-07-14 |
| Phase 4 | Era policy (Blighted Grove/Paroxysmus/Prism/Sanctuary → `Spawns/disabled/felucca/`) | ✓ EXECUTED 2026-07-14 |

Convert **existing-class lanes first** (zero new content — pure spawn edits), then the
**gap families** as each roster ships, then the **era-mismatch** decision last.

**Phase 1 — Graveyards + the enhanced Classic Five (existing classes, no new code).**
Files: `Graveyards.json`, `Deceit.json`, `Despise.json`, `Shame.json`, `Destard.json`,
`Hythloth.json`. Every replacement is a class we already built and already spawn additively.
Lowest risk, immediate themed coverage of six areas. *Gate:* confirm each stock→custom swap
holds the area's HP band (±1) in-game.

**Phase 2 — Overworld type-routing (existing biome/themed classes).**
Files: `Outdoors.json`, `WildLife.json`, `LostLands.json`. Swap roaming hostiles onto the biome
families (`Grove/Peak/Mire/Restless/Shore`) and Classic-Five families per §D.17. Higher blast
radius (huge files, many coords) but still no new content. *Gate:* keep predator wildlife and
farm animals untouched; verify the KEEP set survives the pass.

**Phase 3 — Gap families, one dungeon at a time (new rosters).**
Order by size/impact: `Pyre*` (Fire) → `Rime*` (Ice) → `Argus*` (Covetous) → `Lykai*` (Orc
Caves) → `Cursed*` (Khaldun) → `Myrmi*` + `Ophian*` (the two Terathan/Solen lanes together) →
`Wayman*` (Wrong) → `Pelasg*` (Painted Caves, or fold into Lykai). Each ships as a roster
(bestiary-master §6 → full stat blocks → `LevelConfig` pins → spawn-file swap). *Gate per
family:* grep every member name against `dev-docs/itemization` + `Mobiles/**` before build.

**Phase 4 — Era-mismatch decision (policy call, then act).**
Prism of Light, Palace of Paroxysmus, Blighted Grove, Sanctuary. Decide: disable the areas, or
theme them in a dedicated post-T2A pass. No spawn edits until the era policy is set.

**Untouched throughout:** all town/vendor files, `Reagents.json`, `FelCropsLS.json`, treasure
spawners, SeaLife (ocean is Poseidon's own — separate call), and every `uoml` family file
(already ours).
