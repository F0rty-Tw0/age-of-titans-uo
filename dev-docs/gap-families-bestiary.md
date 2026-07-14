# Gap-Families Bestiary — Age of Titans (rosters v1, 2026-07-14)

> Full rosters for the **9 gap families** stubbed in `dev-docs/bestiary-master.md` §6 — the
> world areas whose stock spawns still had no Greek-themed replacement. Each family here is a
> complete stat-block table in the `dev-docs/dungeon-ladder-bestiary.md` format, built to the
> **REPLACE→NEW niche contract** in `dev-docs/spawn-migration-map.md` §D (every stock type a
> family owns is covered by a member within ±1 of the stock level).
>
> These are the nine **dungeon-resident** families (Fire, Ice, Khaldun, Solen/Terathan swarm,
> Terathan Keep serpents, Orc Caves, Covetous, Wrong, Painted Caves). They are the third
> changeover phase (`spawn-migration-map.md` §F Phase 3) and each is the **first elite draw**
> its dungeon has ever had — the named elite is the new reason to run the place.

## How to read this

- **Donor class** — a stock T2A body verified present under `Projects/UOContent/Mobiles/`
  (all 60+ donors grep-checked 2026-07-14). Copy the donor's `Body`/`BaseSoundID`; the hue
  recolors it. Class names use the family prefix.
- **Immunities/Poison/Breath** — zero-cost overrides. `HasBreath Cold`/`Fire` are native
  (`MonsterAbilities`); `HasBreath Cold ("Frost Breath")` is a local one-class `FireBreath`
  reskin (established `StormWisp`/`WyldSentinel` idiom). Breath, `HitPoison`, pack instinct
  and immunities are **free** and do not count against the custom budget.
- **Ability** — `—` = pure stat block (the majority). A named ability is one toolbox entry:
  `OnGaveMeleeAttack` proc (stam/mana drain, flavor msg), `OnGotMeleeAttack` proc (reflect /
  blink / capped add via `DungeonAbilities.TrySpawnAdd`), or a damage aura
  (`DungeonAbilities.AuraPulse`). **≤3 custom-ability mobs per family** (tally in appendix).
- **Bag (trash)** — every family here is dungeon-resident, so trash overrides
  `LootBagLevel = mob level − 1` (the barrow idiom). The **named elite** carries `DungeonElite`,
  opts out of the trash roll, and force-drops a guaranteed bag `EliteBagLevel = its own level`
  (`EliteBagCount = 1`), with the built-in over-level anti-farm clamp.
- HP stays inside each level's band (`LevelConfig.MobLevelFromHits`): L2 ≤100, L3 ≤160,
  L4 ≤240, L5 ≤380, L6 ≤550, L7 ≤720, L8 ≤950, L9 ≤2400.

---

## 6.1 The Pyre — Fire dungeon · `Pyre*` · Phlegethon / Prometheus's stolen flame (L5–8)

The burning river **Pyriphlegethon** has broken up through the lava halls, and the shades of
the **unburnt** — those denied a funeral pyre — walk its coals beside river-fire elementals and
Titan-flame beasts. This is raw conflagration, not Hephaestus's disciplined forge: Prometheus's
theft left to run wild. The Pyre owns the **Fire dungeon** and the fire lane leaking into
**Trinsic Passage**. *Trash `LootBagLevel = level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PyreCinderrat` | an ember rat | Giant Rat | ember 0x0655 | 5 | 280-310 | 8-11 | Melee | Medium | — | — |
| `PyreCrawler` | a lava crawler | Lava Lizard | ashen red 0x0021 | 5 | 300-350 | 10-14 | Melee | Slow | PoisonImmune | — |
| `PyreAsp` | a cinder asp | Lava Snake | ember 0x0655 | 5 | 300-340 | 10-14 | Melee | Fast | — | HitPoison Greater |
| `PyreHound` | an ashen hound | Hell Hound | hellfire black 0x0453 | 5 | 300-350 | 10-14 | Melee | Fast | — | HasBreath Fire; pack instinct |
| `PyreEmberling` | a living ember | Imp | ember 0x0655 | 5 | 280-330 | 9-13 | Mage | Fast | — | HasBreath Fire |
| `PyreServitor` | a river-fire elemental | Fire Elemental | pyre crimson 0x0026 | 6 | 460-520 | 12-16 | Melee | Medium | PoisonImmune | **River-Glare** — aura: 3 fire/tick to adjacent |
| `PyreSerpent` | a molten serpent | Lava Serpent | molten core 0x0669 | 6 | 480-530 | 12-16 | Melee | Medium | — | HitPoison Deadly |
| `PyreUnburnt` | an unburnt shade | Skeletal Mage | hellfire black 0x0453 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `PyreEfreet` | a flamewind efreet | Efreet | ember 0x0655 | 6 | 480-540 | 13-17 | Mage | Medium | — | HasBreath Fire |
| `PyrePyromancer` | an unburnt pyromancer | Lich | pyre crimson 0x0026 | 7 | 620-680 | 15-19 | Mage | Medium | — | **Scald** — OnGaveMelee 20%: 12 mana drain + flavor msg |
| `PyreDaemon` | a cinder daemon | Daemon | molten core 0x0669 | 7 | 640-700 | 15-19 | Mage | Medium | PoisonImmune | HasBreath Fire |

**Elite — `PyrePhlegyas` · Phlegyas, the Unquenched** (Tartarus's arsonist king, chained in
the river he set alight)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PyrePhlegyas` | Phlegyas, the Unquenched | Daemon | molten core 0x0669 | 8 | 900-950 | 18-24 | Mage | Medium | PoisonImmune + BleedImmune | **Rivers of Fire** — OnGaveMelee 25%: 18 mana drain + knockback msg; HasBreath Fire · `DungeonElite` bag8 |

---

## 6.2 The Rimehold — Ice dungeon · `Rime*` · Boreas & Khione / the Hyperborean cold (L4–8)

The north wind **Boreas** and the snow-maiden **Khione** hold the Ice dungeon as a frozen
antechamber to the world's edge. The ratman tribes that denned here froze into **rime-bound
thralls**, and the cold itself grew teeth — frost-oozes, glacial serpents, and a hoarfrost
warlord. Cold breath and glass-skirmisher serpents carry the theme; the frost-giant tank
anchors the top ranks. The Rimehold owns the whole **Ice dungeon**. *Trash `LootBagLevel =
level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RimeThrall` | a rime-bound thrall | Ratman | hoarfrost white 0x047E | 4 | 200-240 | 8-11 | Melee | Medium | — | — |
| `RimeArcher` | a rime-bound archer | Ratman Archer | glacier blue 0x0AF3 | 4 | 200-235 | 8-11 | Archer | Medium | — | — |
| `RimeShaman` | a rime-bound shaman | Ratman Mage | glacier blue 0x0AF3 | 4 | 200-235 | 8-11 | Mage | Medium | — | HasBreath Cold ("Frost Breath") |
| `RimeStalker` | a frostfang stalker | Ice Snake | rime cyan 0x0B0F | 4 | 190-230 | 7-10 | Melee | VeryFast | — | — |
| `RimeSerpent` | a glacial serpent | Ice Serpent | glacier blue 0x0AF3 | 4 | 200-240 | 8-11 | Melee | VeryFast | — | HitPoison Regular |
| `RimeSpider` | a hoarfrost spider | Frost Spider | hoarfrost white 0x047E | 4 | 200-235 | 8-11 | Melee | Medium | — | HitPoison Greater |
| `RimeOoze` | a frost ooze | Frost Ooze | rime cyan 0x0B0F | 4 | 190-230 | 7-10 | Melee | Slow | Bleed + Poison immune | — |
| `RimeElemental` | a snowbound elemental | Snow Elemental | frostbite 0x0485 | 5 | 320-370 | 10-14 | Melee | Medium | PoisonImmune | HasBreath Cold |
| `RimeGiant` | a rime frost-giant | Frost Troll | deep frost 0x0485 | 6 | 480-540 | 13-17 | Melee | Slow | — | — |
| `RimeFiend` | a boreal fiend | Ice Fiend | rime cyan 0x0B0F | 6 | 480-530 | 13-17 | Mage | Medium | — | HasBreath Cold |
| `RimeWarlord` | a hoarfrost warlord | Arctic Ogre Lord | deep frost 0x0485 | 7 | 620-680 | 15-19 | Melee | Slow | BleedImmune | **Northwind Crush** — OnGaveMelee 20%: knockback msg + 15 stam drain |

**Elite — `RimeAbaris` · Abaris, the Hoarfrost Herald** (the Hyperborean priest who rode
Apollo's arrow, frozen into the seat of the north)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RimeAbaris` | Abaris, the Hoarfrost Herald | White Wyrm | hoarfrost white 0x047E | 8 | 900-950 | 18-24 | Melee | Medium | high cold resist; BleedImmune | **Killing Frost** — OnGaveMelee 25%: 18 mana drain + flavor msg; HasBreath Cold · `DungeonElite` bag8 |

---

## 6.3 The Accursed Dig — Khaldun · `Cursed*` · Hecate / the doomed expedition (L5–9)

A doomed excavation woke something buried, and its diggers are **cursed to guard it** forever.
A death-cult of **Hecate** — crossroads witchcraft, restless shades, the buried oracle they
feed — has grown around the seal. Undead-heavy but a *living cult + a curse*, deliberately
apart from the Stygian Deep (the underworld proper): these dead still dig. The Accursed Dig owns
**Khaldun** (its named questline NPCs fold in only if the quest is retired — see
`spawn-migration-map.md` §C). *Trash `LootBagLevel = level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CursedShade` | a barrow shade | Shadow Fiend | void 0x0455 | 5 | 300-350 | 9-13 | Melee | Fast | — | — |
| `CursedDigger` | a cursed digger | Zombie | corpselight 0x0842 | 5 | 320-360 | 10-14 | Melee | Slow | BleedImmune | — |
| `CursedDelver` | a cursed delver | Skeleton | corpselight 0x0842 | 5 | 320-360 | 10-14 | Melee | Medium | — | — |
| `CursedSentinel` | a bound sentinel | Bone Knight | ectoplasm green 0x0851 | 6 | 460-520 | 12-16 | Melee | Medium | BleedImmune | — |
| `CursedArmour` | an animate spectral armour | Spectral Armour | spectral cyan 0x0AA8 | 6 | 460-510 | 12-16 | Melee | Slow | Bleed + Poison immune | — |
| `CursedBonemagi` | a bone hierophant | Bone Magi | corpselight 0x0842 | 6 | 460-510 | 12-16 | Mage | Slow | — | — |
| `CursedNecromancer` | a Hecate necromancer | Skeletal Mage | hex violet 0x0486 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `CursedAccursed` | one of the accursed | Cursed | ectoplasm green 0x0851 | 6 | 460-510 | 12-16 | Mage | Medium | PoisonImmune | — |
| `CursedZealot` | a Hecate zealot | Evil Mage | hex violet 0x0486 | 7 | 600-660 | 14-18 | Mage | Medium | — | HitPoison Deadly |
| `CursedRevenant` | a warded revenant | Skeletal Knight | spectral cyan 0x0AA8 | 7 | 600-660 | 14-18 | Melee | Medium | BleedImmune | **Grave Ward** — OnGotMelee 20%: reflect 25% as cold |
| `CursedSummoner` | a crossroads summoner | Evil Mage | hex violet 0x0486 | 7 | 610-670 | 14-18 | Mage | Medium | — | **Call the Buried** — OnGotMelee 15%: spawn 1 `CursedShade` (cap 2, dispel-vulnerable) |

**Elite — `CursedAeetes` · Aeetes, the Buried Oracle** (the Hecate-cult king-seer the dig
woke, the thing the diggers are bound to guard)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CursedAeetes` | Aeetes, the Buried Oracle | Ancient Lich | hex violet 0x0486 | 9 | 2200-2400 | 20-26 | Mage | Medium | PoisonImmune + BleedImmune | **Hecate's Sentence** — OnGaveMelee 25%: 20 mana drain + knockback msg · `DungeonElite` bag9 |

---

## 6.4 The Myrmex Nest — Solen Hive + Terathan swarm · `Myrmi*` · Zeus / the Myrmidons (L3–7)

Zeus made the **Myrmidons** from ants for **Aiakos** (already our Deceit elite — a clean
bloodline tie). The Solen hive and the Terathan swarm are recast as one **myrmex ant-nation**:
foragers, chitin soldiers, venom-spurred raiders, and a brood-mother who calls the swarm. **Pack
instinct** everywhere and a **queen-summoner** elite are the signature. The Nest takes the
insect side of the contested **Terathan Keep** (the serpent side is `Ophian*` §6.5) plus the
whole **Solen Hive**. *Trash `LootBagLevel = level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MyrmiForager` | a myrmex forager | Black Solen Worker | amber 0x0798 | 3 | 130-160 | 7-10 | Melee | Medium | — | — |
| `MyrmiDrone` | a myrmex drone | Terathan Drone | drone grey 0x0964 | 3 | 130-160 | 7-10 | Melee | Medium | — | pack instinct |
| `MyrmiSoldier` | a myrmex soldier | Black Solen Warrior | chitin black 0x0966 | 4 | 200-240 | 8-11 | Melee | Fast | — | pack instinct |
| `MyrmiPikebug` | a myrmex pikeguard | Terathan Warrior | chitin black 0x0966 | 4 | 200-240 | 8-11 | Melee | Medium | — | pack instinct |
| `MyrmiAntlion` | a myrmex antlion | Ant Lion | amber 0x0798 | 4 | 200-235 | 8-11 | Melee | Slow | — | HitPoison Greater |
| `MyrmiVenomspur` | a myrmex venomspur | Red Solen Warrior | soldier crimson 0x0021 | 5 | 320-360 | 10-14 | Melee | Fast | — | HitPoison Deadly |
| `MyrmiBroodguard` | a myrmex broodguard | Black Solen Queen | chitin black 0x0966 | 6 | 480-530 | 13-17 | Melee | Medium | PoisonImmune | — |
| `MyrmiAvenger` | a myrmex avenger | Terathan Avenger | soldier crimson 0x0021 | 6 | 460-510 | 12-16 | Melee | Fast | PoisonImmune | pack instinct |
| `MyrmiWarden` | a myrmex hive-warden | Terathan Warrior | amber 0x0798 | 5 | 320-360 | 10-14 | Melee | Medium | — | **Swarm Call** — OnGotMelee 15%: spawn 1 `MyrmiDrone` (cap 2, dispel-vulnerable) |

**Elite — `MyrmiMyrmex` · Myrmex, the Brood-Mother** (the ant-nymph Athena cursed into the
first ant; mother of the whole nation)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MyrmiMyrmex` | Myrmex, the Brood-Mother | Terathan Matriarch | amber 0x0798 | 7 | 700-720 | 16-20 | Mage | Medium | PoisonImmune + BleedImmune | **Spawn the Swarm** — OnGotMelee 12%: spawn 1 `MyrmiDrone` (cap 3, dispel-vulnerable); HitPoison Deadly · `DungeonElite` bag7 |

---

## 6.5 The Coiled Sanctum — Terathan Keep serpents · `Ophian*` · Ophion & Echidna (L4–9)

The serpent-men who share the Keep are the cult of **Ophion**, the elder serpent-titan who
ruled before the gods, and the brood of **Echidna** — snake-priests, scaled knights, coil-mounts
and a scaled matron. **Poison discipline** is the through-line, tying to the existing
`EchidnaBrood` recolor flavor. The Coiled Sanctum takes the **serpent side of Terathan Keep**
(the insect side is `Myrmi*` §6.4). *Trash `LootBagLevel = level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `OphianScale` | an ophian scaleguard | Ophidian Warrior | venom green 0x0851 | 4 | 200-240 | 8-11 | Melee | Medium | — | HitPoison Greater |
| `OphianAcolyte` | an ophian acolyte | Ophidian Mage | jaundice 0x0847 | 4 | 200-235 | 8-11 | Mage | Medium | — | HitPoison Greater |
| `OphianReaver` | an ophian reaver | Ophidian Warrior | viper black 0x0453 | 5 | 320-360 | 10-14 | Melee | Fast | — | HitPoison Deadly |
| `OphianKnight` | an ophian serpent-knight | Ophidian Knight | scale bronze 0x0798 | 5 | 320-360 | 10-14 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `OphianPriest` | an ophian venom-priest | Ophidian Mage | venom green 0x0851 | 5 | 320-360 | 10-14 | Mage | Medium | PoisonImmune | HitPoison Deadly |
| `OphianConstrictor` | a sacred constrictor | Giant Serpent | venom green 0x0851 | 5 | 320-360 | 10-14 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `OphianScaledbrute` | a scaled brute | Ophidian Knight | jaundice 0x0847 | 6 | 460-510 | 12-16 | Melee | Slow | PoisonImmune | — |
| `OphianMount` | an ophian coil-mount | Nightmare | viper black 0x0453 | 6 | 460-510 | 12-16 | Melee | Fast | — | HasBreath Fire |
| `OphianArchpriest` | an ophian archpriest | Ophidian Archmage | venom green 0x0851 | 6 | 460-520 | 12-16 | Mage | Medium | PoisonImmune | **Ophion's Venom** — OnGaveMelee 20%: 12 mana drain + flavor msg |

**Elite — `OphianKeto` · Keto, the Scaled Matron** (Ceto, the primordial sea-serpent mother of
monsters — Echidna's own dam)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `OphianKeto` | Keto, the Scaled Matron | Ophidian Matriarch | venom green 0x0851 | 7 | 700-720 | 16-20 | Mage | Medium | PoisonImmune + BleedImmune | **Coil and Constrict** — OnGaveMelee 25%: knockback msg + 15 stam drain; HitPoison Deadly · `DungeonElite` bag7 |

---

## 6.6 The Arcadian Warband — Orc Caves · `Lykai*` · Lykaon / the wolf-cult (L2–6)

**Lykaon**, the Arcadian king cursed into a wolf for serving Zeus human flesh, leads a savage
cult of wild men and their dire packs. The orcs are recast as **feral Arcadian raiders**, the
dire wolves as the cult's hounds, with a shaman and a war-leader over them. **Pack instinct** and
skirmisher wolves define it. The Warband owns the **Orc Caves** and the overworld **orc/rat
camps** that convert to it (`spawn-migration-map.md` §D.13). *Trash `LootBagLevel = level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `LykaiScout` | an Arcadian scout | Ratman | wolf grey 0x0483 | 2 | 90-120 | 6-9 | Melee | Fast | — | — |
| `LykaiWretch` | a caves wretch | Giant Rat | ash 0x0964 | 2 | 80-110 | 5-8 | Melee | Medium | — | — |
| `LykaiRaider` | an Arcadian raider | Orc | raider hide 0x0844 | 3 | 130-160 | 7-10 | Melee | Medium | — | pack instinct |
| `LykaiHound` | a Lykaian hound | Dire Wolf | wolf grey 0x0483 | 3 | 130-160 | 7-10 | Melee | Fast | — | pack instinct |
| `LykaiSkirmisher` | an Arcadian javelineer | Orc Bomber | raider hide 0x0844 | 3 | 130-160 | 7-10 | Melee | Fast | — | — |
| `LykaiSnare` | a cave snare | Corpser | ash 0x0964 | 3 | 130-160 | 7-10 | Melee | Slow | — | — |
| `LykaiShaman` | a wolf-cult shaman | Orcish Mage | blood 0x0021 | 4 | 200-235 | 8-11 | Mage | Medium | — | — |
| `LykaiBrute` | an Arcadian brute | Orc Brute | raider hide 0x0844 | 5 | 320-360 | 11-15 | Melee | Slow | — | — |
| `LykaiWarleader` | an Arcadian warleader | Orcish Lord | blood 0x0021 | 5 | 320-360 | 11-15 | Melee | Medium | — | **Howl of the Pack** — OnGotMelee 15%: spawn 1 `LykaiHound` (cap 2, dispel-vulnerable); pack instinct |

**Elite — `LykaiNyktimos` · Nyktimos, the Wolf-Crowned** (Lykaon's son, first to wear the
pelt-crown of the cursed line)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `LykaiNyktimos` | Nyktimos, the Wolf-Crowned | Orc Brute | wolf grey 0x0483 | 6 | 540-550 | 14-18 | Melee | Fast | BleedImmune | **Rend and Run** — OnGaveMelee 25%: knockback msg + 15 stam drain; pack instinct · `DungeonElite` bag6 |

---

## 6.7 The Hundred-Eyed Vault — Covetous · `Argus*` · Argus Panoptes / the hoard-guard (L3–8)

The hundred-eyed giant **Argus Panoptes** guards a **gold-cursed hoard**; his lesser eyes (the
gazers) drift the halls, and everything the greed drew in — bound treasure-hunters, vault spiders,
a hoard-cursed lich, and the ever-hungering wyrm at the bottom — has been set to watch. The
**all-seeing eye** is the motif (the gazer lane) and the **cursed hoard** the reward flavor. The
Vault owns **Covetous**. *Trash `LootBagLevel = level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ArgusMote` | a gazing mote | Gazer Larva | eye violet 0x0486 | 3 | 130-160 | 7-10 | Mage | Slow | — | — |
| `ArgusThrall` | a hoard-thrall | Headless One | cursed gold 0x0479 | 3 | 130-160 | 7-10 | Melee | Fast | — | — |
| `ArgusOoze` | a gilded ooze | Slime | cursed gold 0x0479 | 3 | 130-160 | 6-9 | Melee | Slow | PoisonImmune | — |
| `ArgusSnare` | a vault snare | Corpser | verdigris 0x08A5 | 3 | 130-160 | 7-10 | Melee | Slow | — | — |
| `ArgusEye` | a watching eye | Gazer | eye violet 0x0486 | 3 | 150-160 | 8-11 | Mage | Slow | — | — |
| `ArgusHarpy` | a vault harpy | Harpy | hoard amber 0x0798 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `ArgusStoneEye` | a stone-eyed harpy | Stone Harpy | verdigris 0x08A5 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `ArgusDeadhunter` | a bound treasure-hunter | Skeleton | verdigris 0x08A5 | 4 | 200-240 | 8-11 | Melee | Medium | — | — |
| `ArgusVaultspider` | a vault spider | Dread Spider | eye violet 0x0486 | 5 | 320-360 | 10-14 | Melee | Medium | — | HitPoison Deadly |
| `ArgusHoardmage` | a hoard-cursed lich | Lich | eye violet 0x0486 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `ArgusOverseer` | an all-seeing overseer | Elder Gazer | cursed gold 0x0479 | 6 | 460-510 | 12-16 | Mage | Slow | — | **Hundred Eyes** — OnGotMelee 15%: reflect 20% as energy |

**Elite — `ArgusErysichthon` · Erysichthon, the Ever-Hungering** (cursed with insatiable
hunger for felling a sacred grove; devoured his hoard, then himself)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ArgusErysichthon` | Erysichthon, the Ever-Hungering | Dragon | cursed gold 0x0479 | 8 | 900-950 | 18-24 | Melee | Medium | BleedImmune | **Devour the Hoard** — OnGaveMelee 25%: 18 stam drain + self-heal 8% max HP + knockback msg; HasBreath Fire · `DungeonElite` bag8 |

---

## 6.8 The Wayman's Toll — Wrong · `Wayman*` · Theseus's road-villains + a lesser Talos (L3–8)

The prison-fort of Wrong is held by a **robber-band** in the mold of the road-villains **Theseus**
slew — Sciron, Sinis, Procrustes — with a captured **automaton** (a lesser **Talos**) and its
artificer as the boss pair. A rare **human-body** family (bandits, thugs, a hedge-wizard) plus a
single **bronze-construct lane**. The Toll owns **Wrong** and the overworld **Brigand** that
converts to it. *Trash `LootBagLevel = level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WaymanCutpurse` | a road cutpurse | Brigand | roadworn brown 0x0844 | 3 | 130-160 | 7-10 | Melee | Medium | — | — |
| `WaymanBrigand` | a Wayman brigand | Brigand | roadworn brown 0x0844 | 3 | 150-190 | 8-11 | Melee | Medium | — | — |
| `WaymanThug` | a Wayman thug | Juka Warrior | iron grey 0x0964 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `WaymanWaylayer` | a Wayman waylayer | Juka Warrior | roadworn brown 0x0844 | 4 | 200-240 | 8-11 | Archer | Medium | — | — |
| `WaymanHedgewizard` | a Wayman hedge-wizard | Juka Mage | drab 0x0000 | 5 | 320-360 | 10-14 | Mage | Medium | — | — |
| `WaymanAutomaton` | a bronze automaton | Golem | bronze 0x0798 | 6 | 460-520 | 12-16 | Melee | Slow | Bleed + Poison immune | — |
| `WaymanCaptain` | the Wayman captain | Juka Lord | iron grey 0x0964 | 6 | 480-530 | 13-17 | Melee | Medium | — | **Cutthroat's Order** — OnGotMelee 15%: flavor msg + 12 stam drain |
| `WaymanArtificer` | a Talos artificer | Golem Controller | bronze 0x0798 | 6 | 480-530 | 13-17 | Mage | Medium | — | **Reforge** — OnGotMelee 15%: reflect 25% as physical |

**Elite — `WaymanPeriphetes` · Periphetes, the Bronze-Cudgel** (Corynetes, Hephaestus's
club-bearing son whom Theseus slew — the lesser Talos woken whole)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WaymanPeriphetes` | Periphetes, the Bronze-Cudgel | Golem | bronze 0x0798 | 8 | 900-950 | 18-24 | Melee | Slow | Bleed + Poison immune | **Cudgel Fall** — OnGaveMelee 25%: knockback msg + 18 stam drain · `DungeonElite` bag8 |

---

## 6.9 The Painted Deep — Painted Caves · `Pelasg*` · the Pelasgians / cave-folk (L2–4, small)

The pre-Greek **Pelasgians**, the aboriginal cave-dwellers — primitive, territorial, and led by
their **first-cut** ancestor. The smallest gap family: troglodyte foragers, a bone-shaman, and
the chieftain, over cave vermin. **Foldable into `Lykai*` §6.6** if a standalone family isn't
worth the spawn edit. The Painted Deep owns the **Painted Caves**. *Trash `LootBagLevel =
level − 1`.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PelasgVermin` | a cave vermin | Giant Rat | clay brown 0x0844 | 2 | 80-110 | 5-8 | Melee | Medium | — | — |
| `PelasgForager` | a Pelasgian forager | Troglodyte | ochre 0x0798 | 3 | 130-160 | 7-10 | Melee | Medium | — | — |
| `PelasgHunter` | a Pelasgian hunter | Troglodyte | clay brown 0x0844 | 3 | 130-160 | 7-10 | Archer | Medium | — | — |
| `PelasgShaman` | a Pelasgian bone-shaman | Troglodyte | cave grey 0x0964 | 4 | 200-235 | 8-11 | Mage | Medium | — | — |

**Elite — `PelasgPhoroneus` · Phoroneus, the First-Cut** (the Argive first-man who first
brought fire to mortals — the cave-folk's primordial chieftain)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PelasgPhoroneus` | Phoroneus, the First-Cut | Troglodyte | hide 0x0967 | 4 | 235-240 | 10-14 | Melee | Fast | BleedImmune | **First Fire** — OnGaveMelee 25%: knockback msg + 12 stam drain · `DungeonElite` bag4 |

---

## Totals Appendix

| # | Family | Prefix | Band | Members | Elite | **Total** | Custom-ability (≤3) |
|---|---|---|---|---|---|---|---|
| 6.1 | The Pyre | `Pyre*` | L5–8 | 11 | Phlegyas | **12** | 3 — Servitor (aura), Pyromancer, Phlegyas |
| 6.2 | The Rimehold | `Rime*` | L4–8 | 11 | Abaris | **12** | 2 — Warlord, Abaris |
| 6.3 | The Accursed Dig | `Cursed*` | L5–9 | 11 | Aeetes | **12** | 3 — Revenant, Summoner, Aeetes |
| 6.4 | The Myrmex Nest | `Myrmi*` | L3–7 | 9 | Myrmex | **10** | 2 — Warden, Myrmex |
| 6.5 | The Coiled Sanctum | `Ophian*` | L4–7 | 9 | Keto | **10** | 2 — Archpriest, Keto |
| 6.6 | The Arcadian Warband | `Lykai*` | L2–6 | 9 | Nyktimos | **10** | 2 — Warleader, Nyktimos |
| 6.7 | The Hundred-Eyed Vault | `Argus*` | L3–8 | 11 | Erysichthon | **12** | 2 — Overseer, Erysichthon |
| 6.8 | The Wayman's Toll | `Wayman*` | L3–8 | 8 | Periphetes | **9** | 3 — Captain, Artificer, Periphetes |
| 6.9 | The Painted Deep | `Pelasg*` | L2–4 | 4 | Phoroneus | **5** | 1 — Phoroneus |
| | **Total** | | | **83** | **9** | **92** | **20** |

**Custom-ability count** = mobs with an `OnGaveMeleeAttack`/`OnGotMeleeAttack` proc or a damage
aura. Breath, `HitPoison`, pack-instinct and immunities are free stat-block features and are
**not** counted. Every family is at or under the 3-custom budget.

**Niche coverage (migration contract).** Every `REPLACE→NEW:Fam.role` row in
`spawn-migration-map.md` §D.7–D.16 is covered by a member within ±1 of the stock level: Fire
(§D.8) + Trinsic fire (§D.14) → Pyre; Ice (§D.9) → Rime; Khaldun (§D.10) → Cursed; Solen +
Terathan insects (§D.11) → Myrmi; Terathan Keep serpents (§D.12) → Ophian; Orc Caves + camps
(§D.13) → Lykai; Covetous (§D.7) → Argus; Wrong (§D.15) → Wayman; Painted Caves (§D.16) → Pelasg.

**Collision checks performed (2026-07-14).**
1. **Donor classes** — all 60+ donors grep-verified present under `Projects/UOContent/Mobiles/`
   (`class <Donor> :`). Two name corrections applied: `OphidianArchmage` (not `…ArchMage`); no
   `orcscout` body exists, so `LykaiScout` uses the **Ratman** donor.
2. **New class names** — no existing class under `Mobiles/**` begins with any of the nine
   prefixes (`grep "class (Pyre|Rime|Cursed|Myrmi|Ophian|Lykai|Argus|Wayman|Pelasg)…"` → zero
   hits); all 92 class names are new.
3. **Elite proper nouns** — grep-checked against `Mobiles/**` and `dev-docs/itemization/**`.
   **`Lamia` was rejected** (reserved in `06-fencing.md`'s legendary serpent-name pool alongside
   Delphyne, Python, Aspis, Amphisbaena, Ladon, Typhon, Hydra) → the Ophian matriarch became
   **`Keto`**. The remaining eight — **Phlegyas, Abaris, Aeetes, Myrmex, Nyktimos, Erysichthon,
   Periphetes, Phoroneus** — are collision-free in both trees. Patron/flavor gods (Boreas,
   Khione, Hecate, Lykaon, Argus, Ophion, Echidna, Prometheus) appear in prose only, never as
   class names.

---

# Expansion to 35 — the delta rosters (v2, 2026-07-14)

> Brings every gap family from its v1 stub up to the **35-roster shard standard**
> (`dev-docs/dungeon-ladder-bestiary.md`): the v1 ranks + named elite stay as the family core,
> and each family gains a **themed sub-faction block (~9)**, **core in-fill**, **ambient/fodder
> (3)** and **one new named mini-boss** to reach ~35. These are new subsections only — the v1
> tables above are untouched. Same rules as v1: donors are verified T2A bodies, HP stays in
> band, trash `LootBagLevel = level − 1`, mini-boss carries `DungeonElite` and force-drops
> `EliteBagLevel = its own level`. **Custom-ability budget is per-family across v1 + v2 (≤8)**;
> each family adds **4 new** procs/auras here, all within budget (tally in the v2 appendix).
> Breath/`HitPoison`/pack-instinct/immunities remain **free**.

---

## 6.1e The Pyre — Expansion (+23) · the Kaminoi kiln-priests

Prometheus's wild flame drew a priesthood of its own — the **Kaminoi**, kiln-tenders who feed
the river-fire and forge-brand the unburnt into new servants. *Trash `LootBagLevel = level − 1`.*

### Core expansion — the coalwalk host (10)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PyreCoalwalker` | a coalwalking shade | Skeleton | hellfire black 0x0453 | 5 | 300-350 | 10-14 | Melee | Medium | — | — |
| `PyreScorchling` | a scorchling | Lava Lizard | ember 0x0655 | 5 | 300-350 | 10-14 | Melee | Fast | PoisonImmune | — |
| `PyreEmberwing` | an emberwing harpy | Harpy | ember 0x0655 | 5 | 300-350 | 10-14 | Melee | Fast | — | — |
| `PyreSlagling` | a slag imp | Imp | molten core 0x0669 | 5 | 280-330 | 9-13 | Mage | Fast | — | HasBreath Fire |
| `PyreCharhound` | a charred hound | Hell Hound | ashen red 0x0021 | 5 | 300-350 | 10-14 | Melee | Fast | — | HasBreath Fire; pack instinct |
| `PyreMagmaton` | a magma elemental | Fire Elemental | molten core 0x0669 | 6 | 460-520 | 12-16 | Melee | Slow | PoisonImmune | — |
| `PyreAshwraith` | an ash wraith | Wraith | hellfire black 0x0453 | 6 | 460-510 | 12-16 | Mage | Fast | — | — |
| `PyreCoalgeist` | a coal geist | Skeletal Mage | pyre crimson 0x0026 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `PyreScoria` | a scoria gargoyle | Fire Gargoyle | ashen red 0x0021 | 6 | 460-520 | 12-16 | Mage | Fast | — | HasBreath Fire |
| `PyreBrandbeast` | a brand-beast | Daemon | ember 0x0655 | 7 | 620-680 | 15-19 | Melee | Medium | PoisonImmune | — |

### Themed block — the Kaminoi kiln-priests (9)
*They tend the eternal kiln; the cinder-caller, emberbrand and forgemaster carry the procs.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PyreKaminosAcolyte` | a Kaminoi acolyte | Skeletal Mage | ember 0x0655 | 5 | 300-350 | 10-14 | Mage | Medium | — | — |
| `PyreKaminosStoker` | a Kaminoi stoker | Ogre | ashen red 0x0021 | 5 | 320-360 | 11-15 | Melee | Slow | PoisonImmune | — |
| `PyreKaminosEmbermonk` | a Kaminoi ember-monk | Efreet | ember 0x0655 | 6 | 460-520 | 12-16 | Mage | Medium | — | HasBreath Fire |
| `PyreKaminosBrand` | a Kaminoi brand-bearer | Fire Gargoyle | molten core 0x0669 | 6 | 460-520 | 12-16 | Melee | Fast | — | HasBreath Fire |
| `PyreKaminosScald` | a Kaminoi scald-priest | Lich | pyre crimson 0x0026 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `PyreKaminosAnvilguard` | a Kaminoi anvil-guard | Daemon | molten core 0x0669 | 7 | 620-680 | 15-19 | Melee | Medium | PoisonImmune | — |
| `PyreKaminosCindercaller` | a Kaminoi cinder-caller | Lich | ember 0x0655 | 7 | 610-670 | 15-19 | Mage | Medium | — | **Kindle the Unburnt** — OnGotMelee 15%: spawn 1 `PyreEmberling` (cap 2, dispel-vulnerable) |
| `PyreKaminosEmberbrand` | a Kaminoi emberbrand | Efreet | pyre crimson 0x0026 | 7 | 620-680 | 15-19 | Mage | Medium | — | **Emberbrand** — OnGaveMelee 20%: 14 mana drain + flavor msg |
| `PyreKaminosForgemaster` | the Kaminoi forgemaster | Daemon | molten core 0x0669 | 7 | 640-700 | 16-20 | Melee | Medium | PoisonImmune | **Anvil-Fall** — OnGaveMelee 25%: knockback msg + 15 stam drain; HasBreath Fire |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PyreCindermoth` | a cinder moth | Mongbat | ember 0x0655 | 5 | 280-310 | 8-11 | Melee | Fast | — | — |
| `PyreAshgull` | an ash gull | Eagle | ashen red 0x0021 | 5 | 280-310 | 8-11 | Melee | VeryFast | — | — |
| `PyreSlaggrub` | a slag grub | Slime | molten core 0x0669 | 5 | 280-310 | 8-11 | Melee | Slow | Bleed + Poison immune | — |

### Mini-boss (1) — between the core and Phlegyas (elite L8)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PyrePhaethon` | Phaethon, the Sky-Scorcher | Daemon | molten core 0x0669 | 7 | 700-720 | 16-22 | Mage | Medium | PoisonImmune + BleedImmune | **Chariot Fall** — OnGaveMelee 25%: 18 mana drain + knockback msg; HasBreath Fire · `DungeonElite` bag7 |

---

## 6.2e The Rimehold — Expansion (+23) · the Boread wind-riders

Boreas's winged sons, the **Boreads**, ride the blizzard on frost-mounts, driving the cold south
ahead of the hoarfrost warlord. *Trash `LootBagLevel = level − 1`.*

### Core expansion — the deepening frost (10)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RimeCrone` | a rime crone | Skeletal Mage | glacier blue 0x0AF3 | 4 | 200-235 | 8-11 | Mage | Medium | — | HasBreath Cold ("Frost Breath") |
| `RimeHowler` | a frost howler | Grey Wolf | hoarfrost white 0x047E | 4 | 200-235 | 8-11 | Melee | Fast | — | pack instinct |
| `RimeGrub` | a permafrost grub | Slime | rime cyan 0x0B0F | 4 | 190-230 | 7-10 | Melee | Slow | Bleed + Poison immune | — |
| `RimeBoar` | a tundra boar | Boar | frostbite 0x0485 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `RimeReaver` | a rime-bound reaver | Skeletal Knight | deep frost 0x0485 | 5 | 320-360 | 10-14 | Melee | Medium | — | — |
| `RimeWight` | a frostbound wight | Wraith | glacier blue 0x0AF3 | 5 | 320-360 | 10-14 | Mage | Fast | PoisonImmune | — |
| `RimeGolem` | a hoarfrost golem | Golem | hoarfrost white 0x047E | 6 | 480-540 | 13-17 | Melee | Slow | Bleed + Poison immune | — |
| `RimeMauler` | a glacier mauler | Frost Troll | deep frost 0x0485 | 6 | 480-530 | 13-17 | Melee | Slow | — | — |
| `RimeSeer` | a hoarfrost seer | Lich | glacier blue 0x0AF3 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `RimeColossus` | a frostbound colossus | Arctic Ogre Lord | hoarfrost white 0x047E | 7 | 620-680 | 15-19 | Melee | Slow | BleedImmune | — |

### Themed block — the Boread wind-riders (9)
*Winged riders of the north wind; the outrider, gale-caller and herald carry buffet/drain/summon.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RimeBoreadScout` | a Boread scout | Harpy | glacier blue 0x0AF3 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `RimeBoreadArcher` | a Boread frost-archer | Ratman Archer | hoarfrost white 0x047E | 5 | 300-350 | 10-14 | Archer | Medium | — | — |
| `RimeBoreadRider` | a Boread wind-rider | Harpy | rime cyan 0x0B0F | 5 | 320-360 | 11-15 | Melee | Fast | — | — |
| `RimeBoreadHoundmaster` | a Boread hound-master | Ratman Mage | frostbite 0x0485 | 5 | 320-360 | 10-14 | Mage | Medium | — | — |
| `RimeBoreadLancer` | a Boread frost-lancer | Arctic Ogre Lord | deep frost 0x0485 | 6 | 480-530 | 13-17 | Melee | Medium | — | — |
| `RimeBoreadShaman` | a Boread storm-shaman | Ice Fiend | glacier blue 0x0AF3 | 6 | 480-530 | 13-17 | Mage | Medium | — | HasBreath Cold |
| `RimeBoreadOutrider` | a Boread outrider | Harpy | rime cyan 0x0B0F | 6 | 480-530 | 13-17 | Melee | Fast | — | **North Buffet** — OnGaveMelee 20%: knockback msg + 12 stam drain |
| `RimeBoreadGale` | a Boread gale-caller | Ice Fiend | frostbite 0x0485 | 7 | 610-670 | 15-19 | Mage | Medium | — | **Whiteout** — OnGaveMelee 20%: 14 mana drain + flavor msg |
| `RimeBoreadHerald` | the Boread herald | Arctic Ogre Lord | hoarfrost white 0x047E | 7 | 640-700 | 16-20 | Melee | Slow | BleedImmune | **Rally the Wind** — OnGotMelee 15%: spawn 1 `RimeBoreadRider` (cap 2, dispel-vulnerable) |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RimeSparrow` | a frost sparrow | Eagle | glacier blue 0x0AF3 | 4 | 190-220 | 7-10 | Melee | VeryFast | — | — |
| `RimeVole` | a snow vole | Giant Rat | hoarfrost white 0x047E | 4 | 190-220 | 7-10 | Melee | Medium | — | — |
| `RimeMoth` | a frost moth | Mongbat | rime cyan 0x0B0F | 4 | 190-220 | 7-10 | Melee | Fast | — | — |

### Mini-boss (1) — between the core and Abaris (elite L8)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `RimeCheimon` | Cheimon, the Deep-Winter | White Wyrm | glacier blue 0x0AF3 | 7 | 700-720 | 16-22 | Melee | Medium | PoisonImmune + BleedImmune | **Deep Freeze** — OnGaveMelee 25%: 18 stam drain + knockback msg; HasBreath Cold · `DungeonElite` bag7 |

---

## 6.3e The Accursed Dig — Expansion (+23) · the Lampad torchbearers

Hecate's underworld torch-nymphs, the **Lampades**, drift the dig with crossroads-fire, lighting
the way for the buried oracle's court. *Trash `LootBagLevel = level − 1`.*

### Core expansion — the deepening curse (10)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CursedGravecaller` | a grave-caller | Skeletal Mage | hex violet 0x0486 | 5 | 320-360 | 10-14 | Mage | Medium | — | — |
| `CursedPallbearer` | a cursed pallbearer | Zombie | corpselight 0x0842 | 5 | 320-360 | 10-14 | Melee | Slow | BleedImmune | — |
| `CursedGravehound` | a barrow hound | Hell Hound | void 0x0455 | 5 | 300-350 | 9-13 | Melee | Fast | — | pack instinct |
| `CursedWight` | a dig-wight | Skeleton | ectoplasm green 0x0851 | 5 | 320-360 | 10-14 | Melee | Medium | — | — |
| `CursedGheist` | a cairn geist | Spectre | spectral cyan 0x0AA8 | 6 | 460-510 | 12-16 | Mage | Fast | PoisonImmune | — |
| `CursedBoneguard` | a cursed bone-guard | Bone Knight | corpselight 0x0842 | 6 | 460-520 | 12-16 | Melee | Medium | BleedImmune | — |
| `CursedGravemage` | a barrow magus | Lich | hex violet 0x0486 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `CursedGravewurm` | a cairn wurm | Giant Serpent | ectoplasm green 0x0851 | 6 | 460-510 | 12-16 | Melee | Slow | PoisonImmune | HitPoison Deadly |
| `CursedDreadknight` | a warded dread-knight | Skeletal Knight | spectral cyan 0x0AA8 | 7 | 600-660 | 14-18 | Melee | Medium | BleedImmune | — |
| `CursedHierophant` | a Hecate hierophant | Lich | hex violet 0x0486 | 8 | 900-950 | 18-24 | Mage | Medium | — | — |

### Themed block — the Lampad torchbearers (9)
*Crossroads torch-nymphs; the crossroads-lampad, wailer and lantern-matron carry aura/drain/summon.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CursedLampadNovice` | a Lampad novice | Evil Mage | hex violet 0x0486 | 5 | 320-360 | 10-14 | Mage | Medium | — | — |
| `CursedLampadTorch` | a Lampad torchbearer | Evil Mage | corpselight 0x0842 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `CursedLampadShade` | a Lampad shade | Shadow Fiend | void 0x0455 | 6 | 460-510 | 12-16 | Melee | Fast | — | — |
| `CursedLampadWarden` | a Lampad grave-warden | Bone Knight | ectoplasm green 0x0851 | 6 | 460-520 | 12-16 | Melee | Medium | BleedImmune | — |
| `CursedLampadPyre` | a Lampad pyre-witch | Evil Mage | hex violet 0x0486 | 7 | 600-660 | 14-18 | Mage | Medium | — | HitPoison Deadly |
| `CursedLampadHex` | a Lampad hexer | Evil Mage | spectral cyan 0x0AA8 | 7 | 600-660 | 14-18 | Mage | Medium | — | — |
| `CursedLampadCrossroads` | a crossroads lampad | Cursed | hex violet 0x0486 | 7 | 610-670 | 14-18 | Mage | Medium | PoisonImmune | **Crossroads Fire** — aura: 3 fire/tick to adjacent |
| `CursedLampadWailer` | a Lampad wailer | Wraith | void 0x0455 | 7 | 600-660 | 14-18 | Mage | Fast | — | **Hex-Drain** — OnGaveMelee 20%: 14 mana drain + flavor msg |
| `CursedLampadMatron` | the Lampad lantern-matron | Ancient Lich | hex violet 0x0486 | 8 | 900-950 | 18-24 | Mage | Medium | PoisonImmune | **Light the Dead** — OnGotMelee 15%: spawn 1 `CursedShade` (cap 2, dispel-vulnerable) |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CursedGraverat` | a grave rat | Giant Rat | corpselight 0x0842 | 5 | 280-310 | 8-11 | Melee | Medium | — | — |
| `CursedTombmoth` | a tomb moth | Mongbat | void 0x0455 | 5 | 280-310 | 8-11 | Melee | Fast | — | — |
| `CursedGraveslime` | a grave slime | Slime | ectoplasm green 0x0851 | 5 | 280-310 | 8-11 | Melee | Slow | Bleed + Poison immune | — |

### Mini-boss (1) — between the core and Aeetes (elite L9)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `CursedPerses` | Perses, the Grave-Titan | Ancient Lich | hex violet 0x0486 | 8 | 900-950 | 19-25 | Mage | Medium | PoisonImmune + BleedImmune | **Titan's Sentence** — OnGaveMelee 25%: 18 mana drain + knockback msg · `DungeonElite` bag8 |

---

## 6.4e The Myrmex Nest — Expansion (+25) · the Aiakid war-brood

The eldest of the nest are the **Aiakid war-brood** — the ant-soldiers Zeus made into the
Myrmidons for Aiakos (our Deceit elite), reverted to chitin and marching for the Brood-Mother.
*Trash `LootBagLevel = level − 1`.*

### Core expansion — the swarming ranks (12)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MyrmiGatherer` | a myrmex gatherer | Black Solen Worker | amber 0x0798 | 3 | 130-160 | 7-10 | Melee | Medium | — | pack instinct |
| `MyrmiTunneler` | a myrmex tunneler | Ant Lion | drone grey 0x0964 | 3 | 130-160 | 7-10 | Melee | Slow | — | — |
| `MyrmiSpitter` | a myrmex spitter | Terathan Drone | soldier crimson 0x0021 | 3 | 130-160 | 7-10 | Melee | Medium | — | HitPoison Greater |
| `MyrmiRaidbug` | a myrmex raidbug | Terathan Warrior | soldier crimson 0x0021 | 4 | 200-240 | 8-11 | Melee | Fast | — | pack instinct |
| `MyrmiChitinguard` | a myrmex chitin-guard | Black Solen Warrior | chitin black 0x0966 | 4 | 200-240 | 8-11 | Melee | Medium | — | pack instinct |
| `MyrmiStinger` | a myrmex stinger | Scorpion | amber 0x0798 | 4 | 200-235 | 8-11 | Melee | Slow | — | HitPoison Deadly |
| `MyrmiBurrower` | a myrmex burrower | Ant Lion | chitin black 0x0966 | 4 | 200-235 | 8-11 | Melee | Slow | — | — |
| `MyrmiSoldierRed` | a red myrmex soldier | Red Solen Warrior | soldier crimson 0x0021 | 5 | 320-360 | 10-14 | Melee | Fast | — | pack instinct |
| `MyrmiWarspur` | a myrmex warspur | Terathan Warrior | chitin black 0x0966 | 5 | 320-360 | 10-14 | Melee | Medium | — | HitPoison Deadly |
| `MyrmiRavager` | a myrmex ravager | Terathan Avenger | soldier crimson 0x0021 | 6 | 460-510 | 12-16 | Melee | Fast | PoisonImmune | pack instinct |
| `MyrmiHivelord` | a myrmex hive-lord | Black Solen Queen | chitin black 0x0966 | 6 | 480-530 | 13-17 | Melee | Medium | PoisonImmune | — |
| `MyrmiBroodpriest` | a myrmex brood-priest | Terathan Matriarch | amber 0x0798 | 6 | 460-520 | 12-16 | Mage | Medium | PoisonImmune | — |

### Themed block — the Aiakid war-brood (9)
*The Myrmidon-ants of Aiakos's line; the goad-warden, spear-caller and marshal carry summon/drain/reflect.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MyrmiAiakidRunner` | an Aiakid runner | Terathan Drone | amber 0x0798 | 3 | 130-160 | 7-10 | Melee | Fast | — | pack instinct |
| `MyrmiAiakidSpear` | an Aiakid spear-ant | Terathan Warrior | chitin black 0x0966 | 4 | 200-240 | 8-11 | Melee | Medium | — | pack instinct |
| `MyrmiAiakidShield` | an Aiakid shield-ant | Black Solen Warrior | soldier crimson 0x0021 | 4 | 200-240 | 8-11 | Melee | Medium | — | pack instinct |
| `MyrmiAiakidLancer` | an Aiakid lancer | Terathan Warrior | amber 0x0798 | 5 | 320-360 | 10-14 | Melee | Fast | — | HitPoison Deadly |
| `MyrmiAiakidPhalanx` | an Aiakid phalangite | Terathan Avenger | chitin black 0x0966 | 5 | 320-360 | 10-14 | Melee | Medium | PoisonImmune | pack instinct |
| `MyrmiAiakidVenomcaster` | an Aiakid venom-caster | Terathan Matriarch | soldier crimson 0x0021 | 6 | 460-520 | 12-16 | Mage | Medium | PoisonImmune | HitPoison Deadly |
| `MyrmiAiakidGoad` | an Aiakid goad-warden | Terathan Avenger | amber 0x0798 | 6 | 460-510 | 12-16 | Melee | Fast | PoisonImmune | **Swarm Goad** — OnGotMelee 15%: spawn 1 `MyrmiAiakidRunner` (cap 2, dispel-vulnerable) |
| `MyrmiAiakidSpearcaller` | an Aiakid spear-caller | Terathan Matriarch | chitin black 0x0966 | 6 | 480-530 | 13-17 | Mage | Medium | PoisonImmune | **Venom Spray** — OnGaveMelee 20%: 12 mana drain + flavor msg |
| `MyrmiAiakidMarshal` | the Aiakid war-marshal | Terathan Avenger | soldier crimson 0x0021 | 7 | 700-720 | 16-20 | Melee | Fast | PoisonImmune + BleedImmune | **Chitin Wall** — OnGotMelee 15%: reflect 25% as poison; pack instinct |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MyrmiGrub` | a myrmex grub | Slime | amber 0x0798 | 3 | 110-140 | 6-9 | Melee | Slow | PoisonImmune | — |
| `MyrmiMite` | a nest mite | Giant Rat | drone grey 0x0964 | 3 | 110-140 | 6-9 | Melee | Medium | — | — |
| `MyrmiGnat` | a nest gnat | Mongbat | amber 0x0798 | 3 | 110-140 | 6-9 | Melee | Fast | — | — |

### Mini-boss (1) — beside Myrmex (elite L7), the swarm's field-marshal

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `MyrmiMenoitios` | Menoitios, the Ant-Marshal | Terathan Matriarch | chitin black 0x0966 | 7 | 700-720 | 16-20 | Mage | Medium | PoisonImmune + BleedImmune | **Marshal the Swarm** — OnGotMelee 12%: spawn 1 `MyrmiAiakidRunner` (cap 3, dispel-vulnerable); HitPoison Deadly · `DungeonElite` bag7 |

---

## 6.5e The Coiled Sanctum — Expansion (+25) · the Ophite hierophants

The serpent-cult's inner circle, the **Ophite** hierophants, keep Ophion's old rites — venom-
sacraments and coil-oracles — beneath the Keep, waiting to wake the elder serpent.
*Trash `LootBagLevel = level − 1`.*

### Core expansion — the coiled ranks (12)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `OphianHatchling` | an ophian hatchling | Snake | venom green 0x0851 | 4 | 190-230 | 7-10 | Melee | VeryFast | — | HitPoison Greater |
| `OphianCrawler` | an ophian scale-crawler | Giant Serpent | jaundice 0x0847 | 4 | 200-240 | 8-11 | Melee | Medium | — | HitPoison Greater |
| `OphianWarden` | an ophian scale-warden | Ophidian Warrior | scale bronze 0x0798 | 4 | 200-240 | 8-11 | Melee | Medium | — | HitPoison Greater |
| `OphianVenomancer` | an ophian venomancer | Ophidian Mage | venom green 0x0851 | 5 | 320-360 | 10-14 | Mage | Medium | — | HitPoison Deadly |
| `OphianLancer` | an ophian coil-lancer | Ophidian Knight | viper black 0x0453 | 5 | 320-360 | 10-14 | Melee | Fast | PoisonImmune | HitPoison Deadly |
| `OphianBasilisk` | an ophian basilisk | Giant Serpent | jaundice 0x0847 | 5 | 320-360 | 10-14 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `OphianColossus` | a scaled colossus | Ophidian Knight | scale bronze 0x0798 | 6 | 460-510 | 12-16 | Melee | Slow | PoisonImmune | — |
| `OphianSerpentmage` | an ophian serpent-mage | Ophidian Archmage | venom green 0x0851 | 6 | 460-520 | 12-16 | Mage | Medium | PoisonImmune | — |
| `OphianConstrictorGreat` | a great constrictor | Giant Serpent | viper black 0x0453 | 6 | 460-510 | 12-16 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `OphianBroodmother` | an ophian brood-mother | Ophidian Matriarch | venom green 0x0851 | 7 | 600-660 | 14-18 | Mage | Medium | PoisonImmune | HitPoison Deadly |
| `OphianDrakon` | an ophian drakon | Wyvern | jaundice 0x0847 | 7 | 620-680 | 15-19 | Melee | Fast | PoisonImmune | HasBreath Poison ("Venom Breath") |
| `OphianTitanspawn` | an ophian titan-spawn | Ophidian Matriarch | viper black 0x0453 | 7 | 620-680 | 15-19 | Mage | Medium | PoisonImmune | HitPoison Deadly |

### Themed block — the Ophite hierophants (9)
*Ophion's priesthood; the envenomer, high-warden and hierophant carry drain/reflect/summon.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `OphianOphiteAcolyte` | an Ophite acolyte | Ophidian Mage | jaundice 0x0847 | 4 | 200-235 | 8-11 | Mage | Medium | — | HitPoison Greater |
| `OphianOphiteScaleward` | an Ophite scale-ward | Ophidian Warrior | venom green 0x0851 | 5 | 320-360 | 10-14 | Melee | Medium | — | HitPoison Deadly |
| `OphianOphiteSerpentguard` | an Ophite serpent-guard | Ophidian Knight | scale bronze 0x0798 | 5 | 320-360 | 10-14 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `OphianOphiteOracle` | an Ophite venom-oracle | Ophidian Mage | venom green 0x0851 | 6 | 460-520 | 12-16 | Mage | Medium | PoisonImmune | HitPoison Deadly |
| `OphianOphiteConstrictor` | an Ophite temple-constrictor | Giant Serpent | viper black 0x0453 | 6 | 460-510 | 12-16 | Melee | Medium | PoisonImmune | HitPoison Deadly |
| `OphianOphiteMystic` | an Ophite coil-mystic | Ophidian Archmage | jaundice 0x0847 | 6 | 460-520 | 12-16 | Mage | Medium | PoisonImmune | — |
| `OphianOphiteEnvenomer` | an Ophite envenomer | Ophidian Archmage | venom green 0x0851 | 7 | 600-660 | 14-18 | Mage | Medium | PoisonImmune | **Ophion's Gift** — OnGaveMelee 20%: 14 mana drain + flavor msg; HitPoison Deadly |
| `OphianOphiteWarden` | an Ophite high-warden | Ophidian Knight | scale bronze 0x0798 | 7 | 610-670 | 15-19 | Melee | Medium | PoisonImmune | **Coil Riposte** — OnGotMelee 20%: reflect 25% as poison |
| `OphianOphiteHierophant` | the Ophite high-hierophant | Ophidian Matriarch | venom green 0x0851 | 8 | 900-950 | 18-24 | Mage | Medium | PoisonImmune + BleedImmune | **Wake the Coil** — OnGotMelee 15%: spawn 1 `OphianConstrictor` (cap 2, dispel-vulnerable); HitPoison Deadly |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `OphianAsp` | a temple asp | Snake | venom green 0x0851 | 4 | 190-220 | 7-10 | Melee | VeryFast | — | HitPoison Greater |
| `OphianScuttler` | a scale scuttler | Scorpion | jaundice 0x0847 | 4 | 190-220 | 7-10 | Melee | Slow | — | HitPoison Greater |
| `OphianCoilrat` | a nest rat | Giant Rat | scale bronze 0x0798 | 4 | 190-220 | 7-10 | Melee | Medium | — | — |

### Mini-boss (1) — the L8 deep capstone above Keto (elite L7), using the L4-9 headroom

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `OphianPoine` | Poine, the Argive Coil | Ophidian Matriarch | viper black 0x0453 | 8 | 900-950 | 18-24 | Mage | Medium | PoisonImmune + BleedImmune | **Serpent's Toll** — OnGaveMelee 25%: 18 mana drain + knockback msg; HitPoison Deadly · `DungeonElite` bag8 |

---

## 6.6e The Arcadian Warband — Expansion (+25) · the Lykaonid wolf-sons

The fifty sons of **Lykaon**, cursed with their father, run the caves as the **Lykaonid** brood —
wolf-crowned princes each leading a pack of the changed. *Trash `LootBagLevel = level − 1`.*

### Core expansion — the feral warband (12)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `LykaiCur` | a caves cur | Dire Wolf | ash 0x0964 | 2 | 80-110 | 5-8 | Melee | Fast | — | pack instinct |
| `LykaiWhelp` | a wolf-cult whelp | Ratman | wolf grey 0x0483 | 2 | 90-120 | 6-9 | Melee | Fast | — | — |
| `LykaiForager` | an Arcadian forager | Orc | raider hide 0x0844 | 3 | 130-160 | 7-10 | Melee | Medium | — | pack instinct |
| `LykaiHunter` | an Arcadian hunter | Orc | ash 0x0964 | 3 | 130-160 | 7-10 | Archer | Medium | — | — |
| `LykaiTracker` | a Lykaian tracker | Grey Wolf | wolf grey 0x0483 | 3 | 130-160 | 7-10 | Melee | VeryFast | — | pack instinct |
| `LykaiMauler` | an Arcadian mauler | Orc Brute | raider hide 0x0844 | 4 | 200-240 | 8-11 | Melee | Slow | — | — |
| `LykaiHowler` | a Lykaian howler | Dire Wolf | wolf grey 0x0483 | 4 | 200-235 | 8-11 | Melee | Fast | — | pack instinct |
| `LykaiWitchdoctor` | a wolf-cult witch-doctor | Orcish Mage | blood 0x0021 | 4 | 200-235 | 8-11 | Mage | Medium | — | — |
| `LykaiBloodrunner` | an Arcadian blood-runner | Orc Bomber | blood 0x0021 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `LykaiRavener` | a Lykaian ravener | Dire Wolf | ash 0x0964 | 5 | 320-360 | 11-15 | Melee | VeryFast | — | pack instinct |
| `LykaiReaver` | an Arcadian reaver | Orc Brute | blood 0x0021 | 5 | 320-360 | 11-15 | Melee | Medium | — | — |
| `LykaiChieftain` | an Arcadian chieftain | Orcish Lord | raider hide 0x0844 | 6 | 480-530 | 13-17 | Melee | Medium | — | — |

### Themed block — the Lykaonid wolf-sons (9)
*Lykaon's cursed sons; the stalker, moon-priest and hound-master carry drain/summon/reflect.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `LykaiLykaonidWhelp` | a Lykaonid whelp | Dire Wolf | wolf grey 0x0483 | 3 | 130-160 | 7-10 | Melee | Fast | — | pack instinct |
| `LykaiLykaonidHunter` | a Lykaonid hunter | Orc | blood 0x0021 | 4 | 200-240 | 8-11 | Melee | Medium | — | pack instinct |
| `LykaiLykaonidSkin` | a Lykaonid skin-changer | Dire Wolf | ash 0x0964 | 4 | 200-235 | 8-11 | Melee | VeryFast | — | pack instinct |
| `LykaiLykaonidShaman` | a Lykaonid blood-shaman | Orcish Mage | blood 0x0021 | 5 | 320-360 | 11-15 | Mage | Medium | — | — |
| `LykaiLykaonidBrute` | a Lykaonid brute-prince | Orc Brute | wolf grey 0x0483 | 5 | 320-360 | 11-15 | Melee | Slow | — | — |
| `LykaiLykaonidStalker` | a Lykaonid stalker | Grey Wolf | ash 0x0964 | 5 | 320-360 | 11-15 | Melee | VeryFast | — | **Throat-Rip** — OnGaveMelee 20%: 12 stam drain + flavor msg |
| `LykaiLykaonidPriest` | a Lykaonid moon-priest | Orcish Lord | blood 0x0021 | 6 | 460-510 | 12-16 | Mage | Medium | — | **Call the Pack** — OnGotMelee 15%: spawn 1 `LykaiHound` (cap 2, dispel-vulnerable) |
| `LykaiLykaonidHoundmaster` | a Lykaonid hound-master | Orcish Lord | wolf grey 0x0483 | 6 | 460-510 | 12-16 | Melee | Medium | — | **Pelt-Ward** — OnGotMelee 20%: reflect 20% as physical; pack instinct |
| `LykaiLykaonidPrince` | a Lykaonid wolf-prince | Orc Brute | wolf grey 0x0483 | 6 | 480-530 | 13-17 | Melee | Fast | BleedImmune | pack instinct |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `LykaiRatling` | a cave ratling | Giant Rat | ash 0x0964 | 2 | 80-110 | 5-8 | Melee | Medium | — | — |
| `LykaiBat` | a cave bat | Mongbat | wolf grey 0x0483 | 2 | 80-110 | 5-8 | Melee | Fast | — | — |
| `LykaiGrub` | a cave grub | Slime | ash 0x0964 | 2 | 80-110 | 4-7 | Melee | Slow | PoisonImmune | — |

### Mini-boss (1) — beside Nyktimos (elite L6), the pack's grey elder

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `LykaiMainalos` | Mainalos, the Old Wolf | Orcish Lord | wolf grey 0x0483 | 6 | 540-550 | 14-18 | Melee | Fast | BleedImmune | **Elder Howl** — OnGotMelee 15%: spawn 1 `LykaiHound` (cap 3, dispel-vulnerable); pack instinct · `DungeonElite` bag6 |

---

## 6.7e The Hundred-Eyed Vault — Expansion (+23) · the Telchine hoard-sorcerers

The greed drew the **Telchines** — the envious sea-smiths whose evil eye blights what it covets —
down into the vault to hoard and to ward, forging cursed gold for Argus's hundred eyes.
*Trash `LootBagLevel = level − 1`.*

### Core expansion — the hoard-wardens (10)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ArgusGilded` | a gilded thrall | Headless One | cursed gold 0x0479 | 3 | 130-160 | 7-10 | Melee | Fast | — | — |
| `ArgusScryeye` | a scrying eye | Gazer Larva | eye violet 0x0486 | 3 | 130-160 | 7-10 | Mage | Slow | — | — |
| `ArgusCoinwraith` | a coin wraith | Wraith | cursed gold 0x0479 | 4 | 200-240 | 8-11 | Mage | Fast | — | — |
| `ArgusVaultguard` | a vault guardian | Skeletal Knight | verdigris 0x08A5 | 4 | 200-240 | 8-11 | Melee | Medium | — | — |
| `ArgusGildedhound` | a gilded hound | Hell Hound | cursed gold 0x0479 | 4 | 200-240 | 8-11 | Melee | Fast | — | pack instinct |
| `ArgusWatcher` | a hundred-eyed watcher | Gazer | eye violet 0x0486 | 5 | 320-360 | 10-14 | Mage | Slow | — | — |
| `ArgusHoardknight` | a hoard-bound knight | Bone Knight | verdigris 0x08A5 | 5 | 320-360 | 10-14 | Melee | Medium | BleedImmune | — |
| `ArgusGildspider` | a gilded vault-spider | Dread Spider | hoard amber 0x0798 | 5 | 320-360 | 10-14 | Melee | Medium | — | HitPoison Deadly |
| `ArgusGoldwyrm` | a gold-cursed wyrm | Wyvern | cursed gold 0x0479 | 6 | 460-520 | 12-16 | Melee | Fast | — | HasBreath Fire |
| `ArgusEyetyrant` | an eye-tyrant | Elder Gazer | eye violet 0x0486 | 7 | 600-660 | 14-18 | Mage | Slow | — | — |

### Themed block — the Telchine hoard-sorcerers (9)
*Envious metal-sorcerers; the blight-caster, gild-mage and over-warden carry aura/drain/reflect.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ArgusTelchineApprentice` | a Telchine apprentice | Gazer Larva | verdigris 0x08A5 | 4 | 200-235 | 8-11 | Mage | Slow | — | — |
| `ArgusTelchineSmith` | a Telchine gild-smith | Golem | hoard amber 0x0798 | 5 | 320-360 | 10-14 | Melee | Slow | Bleed + Poison immune | — |
| `ArgusTelchineWarden` | a Telchine vault-warden | Stone Harpy | verdigris 0x08A5 | 5 | 320-360 | 10-14 | Melee | Fast | — | — |
| `ArgusTelchineSeer` | a Telchine evil-eye seer | Gazer | eye violet 0x0486 | 6 | 460-510 | 12-16 | Mage | Slow | — | — |
| `ArgusTelchineHexer` | a Telchine blight-hexer | Lich | eye violet 0x0486 | 6 | 460-510 | 12-16 | Mage | Medium | — | — |
| `ArgusTelchineGuard` | a Telchine bronze-guard | Golem | hoard amber 0x0798 | 6 | 460-520 | 12-16 | Melee | Slow | Bleed + Poison immune | — |
| `ArgusTelchineBlight` | a Telchine blight-caster | Elder Gazer | cursed gold 0x0479 | 7 | 600-660 | 14-18 | Mage | Slow | — | **Evil Eye** — aura: 3 energy/tick to adjacent |
| `ArgusTelchineGildmage` | a Telchine gild-mage | Lich | cursed gold 0x0479 | 7 | 610-670 | 14-18 | Mage | Medium | — | **Covet** — OnGaveMelee 20%: 14 mana drain + flavor msg |
| `ArgusTelchineOverwarden` | the Telchine over-warden | Elder Gazer | cursed gold 0x0479 | 8 | 900-950 | 18-24 | Mage | Slow | — | **Hundred-Eyed Ward** — OnGotMelee 15%: reflect 25% as energy |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ArgusMoteling` | a drifting mote | Gazer Larva | eye violet 0x0486 | 3 | 130-155 | 6-9 | Mage | Slow | — | — |
| `ArgusGoldrat` | a gilded rat | Giant Rat | cursed gold 0x0479 | 3 | 130-155 | 6-9 | Melee | Medium | — | — |
| `ArgusVaultmoth` | a vault moth | Mongbat | verdigris 0x08A5 | 3 | 130-155 | 6-9 | Melee | Fast | — | — |

### Mini-boss (1) — between the core and Erysichthon (elite L8)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `ArgusMidas` | Midas, the Gilded | Lich Lord | cursed gold 0x0479 | 7 | 700-720 | 16-22 | Mage | Medium | PoisonImmune + BleedImmune | **Golden Touch** — OnGaveMelee 25%: 18 mana drain + knockback msg · `DungeonElite` bag7 |

---

## 6.8e The Wayman's Toll — Expansion (+26) · the Isthmian wreckers

The road-band swelled with the ghosts of the **Isthmian** wreckers — the highway-killers Theseus
put down at the isthmus — who run the Toll's outer gates with cudgel, pine-bow and iron bed.
*Trash `LootBagLevel = level − 1`.*

### Core expansion — the toll-gang (13)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WaymanFootpad` | a road footpad | Brigand | roadworn brown 0x0844 | 3 | 130-160 | 7-10 | Melee | Fast | — | — |
| `WaymanSlinger` | a Wayman slinger | Brigand | iron grey 0x0964 | 3 | 130-160 | 7-10 | Archer | Medium | — | — |
| `WaymanRuffian` | a Wayman ruffian | Juka Warrior | roadworn brown 0x0844 | 4 | 200-240 | 8-11 | Melee | Medium | — | — |
| `WaymanBowman` | a Wayman bowman | Juka Warrior | iron grey 0x0964 | 4 | 200-240 | 8-11 | Archer | Medium | — | — |
| `WaymanCudgeler` | a Wayman cudgeler | Juka Warrior | bronze 0x0798 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `WaymanRoadwitch` | a Wayman road-witch | Juka Mage | drab 0x0000 | 5 | 320-360 | 10-14 | Mage | Medium | — | — |
| `WaymanEnforcer` | a Wayman enforcer | Juka Lord | iron grey 0x0964 | 5 | 320-360 | 10-14 | Melee | Medium | — | — |
| `WaymanGearhound` | a clockwork hound | Golem | bronze 0x0798 | 5 | 320-360 | 10-14 | Melee | Fast | Bleed + Poison immune | — |
| `WaymanBronzeguard` | a bronze sentinel | Golem | bronze 0x0798 | 6 | 460-520 | 12-16 | Melee | Slow | Bleed + Poison immune | — |
| `WaymanBrigadier` | a Wayman brigadier | Juka Lord | roadworn brown 0x0844 | 6 | 480-530 | 13-17 | Melee | Medium | — | — |
| `WaymanCogwright` | a Talos cogwright | Golem Controller | bronze 0x0798 | 6 | 480-530 | 13-17 | Mage | Medium | — | — |
| `WaymanIronclad` | an ironclad automaton | Golem | iron grey 0x0964 | 7 | 600-660 | 14-18 | Melee | Slow | Bleed + Poison immune | — |
| `WaymanWarlord` | the Wayman road-warlord | Juka Lord | bronze 0x0798 | 7 | 620-680 | 15-19 | Melee | Medium | — | — |

### Themed block — the Isthmian wreckers (9)
*The road-villain ghosts; the pine-bender, bed-wright and toll-captain carry procs.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WaymanIsthmianCutthroat` | an Isthmian cutthroat | Brigand | roadworn brown 0x0844 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `WaymanIsthmianArcher` | an Isthmian pine-archer | Juka Warrior | iron grey 0x0964 | 4 | 200-240 | 8-11 | Archer | Medium | — | — |
| `WaymanIsthmianReaver` | an Isthmian reaver | Juka Warrior | roadworn brown 0x0844 | 5 | 320-360 | 10-14 | Melee | Medium | — | — |
| `WaymanIsthmianHexer` | an Isthmian hedge-hexer | Juka Mage | drab 0x0000 | 5 | 320-360 | 10-14 | Mage | Medium | — | — |
| `WaymanIsthmianBravo` | an Isthmian bravo | Juka Lord | iron grey 0x0964 | 6 | 460-510 | 12-16 | Melee | Medium | — | — |
| `WaymanIsthmianAutomaton` | an Isthmian bronze-thug | Golem | bronze 0x0798 | 6 | 460-520 | 12-16 | Melee | Slow | Bleed + Poison immune | — |
| `WaymanIsthmianPinebender` | an Isthmian pine-bender | Juka Lord | roadworn brown 0x0844 | 6 | 480-530 | 13-17 | Melee | Medium | — | **Pine-Snap** — OnGaveMelee 20%: knockback msg + 12 stam drain |
| `WaymanIsthmianBedwright` | an Isthmian bed-wright | Golem Controller | iron grey 0x0964 | 7 | 600-660 | 14-18 | Mage | Medium | — | **Rack and Ruin** — OnGaveMelee 20%: 14 stam drain + flavor msg |
| `WaymanIsthmianCaptain` | the Isthmian toll-captain | Juka Lord | bronze 0x0798 | 7 | 620-680 | 15-19 | Melee | Medium | — | **Toll Collected** — OnGotMelee 15%: spawn 1 `WaymanCutpurse` (cap 2, dispel-vulnerable) |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WaymanCurhound` | a mangy cur | Dire Wolf | roadworn brown 0x0844 | 3 | 130-155 | 6-9 | Melee | Fast | — | — |
| `WaymanRat` | a gutter rat | Giant Rat | iron grey 0x0964 | 3 | 130-155 | 6-9 | Melee | Medium | — | — |
| `WaymanCrow` | a gallows crow | Mongbat | drab 0x0000 | 3 | 130-155 | 6-9 | Melee | Fast | — | — |

### Mini-boss (1) — between the core and Periphetes (elite L8)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `WaymanProcrustes` | Procrustes, the Stretcher | Golem | iron grey 0x0964 | 7 | 700-720 | 16-22 | Melee | Slow | Bleed + Poison immune | **Fit to the Bed** — OnGaveMelee 25%: 18 stam drain + knockback msg · `DungeonElite` bag7 |

---

## 6.9e The Painted Deep — Expansion (+30) · the Leleges cave-clan

The Painted Deep runs deeper than one clan. Below Phoroneus's Pelasgians den the **Leleges** — a
rival aboriginal cave-people of stone-knappers, toad-herders and bone-augurs — and the two clans
feud over the painted galleries. (Largest delta: 30 new bodies held inside the L2-4 band.)
*Trash `LootBagLevel = level − 1`.*

### Core expansion — the painted clans (17)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PelasgGrubber` | a cave grubber | Giant Rat | clay brown 0x0844 | 2 | 80-110 | 5-8 | Melee | Medium | — | — |
| `PelasgCreeper` | a painted creeper | Corpser | cave grey 0x0964 | 2 | 80-110 | 5-8 | Melee | Slow | — | — |
| `PelasgWhelp` | a Pelasgian whelp | Troglodyte | ochre 0x0798 | 2 | 90-110 | 6-9 | Melee | Medium | — | — |
| `PelasgKnapper` | a Pelasgian stone-knapper | Troglodyte | clay brown 0x0844 | 2 | 90-110 | 6-9 | Melee | Fast | — | — |
| `PelasgSpearman` | a Pelasgian spearman | Troglodyte | ochre 0x0798 | 3 | 130-160 | 7-10 | Melee | Medium | — | pack instinct |
| `PelasgSlinger` | a Pelasgian slinger | Troglodyte | cave grey 0x0964 | 3 | 130-160 | 7-10 | Archer | Medium | — | — |
| `PelasgDaubed` | a daubed hunter | Orc | clay brown 0x0844 | 3 | 130-160 | 7-10 | Melee | Medium | — | pack instinct |
| `PelasgTorchbearer` | a Pelasgian torchbearer | Troglodyte | ochre 0x0798 | 3 | 130-160 | 7-10 | Melee | Fast | — | — |
| `PelasgTrapper` | a Pelasgian trapper | Orc Bomber | cave grey 0x0964 | 3 | 130-160 | 7-10 | Melee | Fast | — | — |
| `PelasgApeman` | a painted ape-man | Gorilla | hide 0x0967 | 3 | 130-160 | 7-10 | Melee | Fast | — | — |
| `PelasgToadherd` | a cave-toad | Giant Toad | cave grey 0x0964 | 3 | 130-160 | 7-10 | Melee | Slow | — | HitPoison Regular |
| `PelasgCaveviper` | a painted viper | Snake | clay brown 0x0844 | 3 | 130-160 | 7-10 | Melee | VeryFast | — | HitPoison Greater |
| `PelasgBrute` | a Pelasgian brute | Orc Brute | clay brown 0x0844 | 4 | 200-240 | 8-11 | Melee | Slow | — | — |
| `PelasgWarhunter` | a Pelasgian war-hunter | Orc | ochre 0x0798 | 4 | 200-240 | 8-11 | Archer | Medium | — | pack instinct |
| `PelasgOchremage` | a Pelasgian ochre-shaman | Orcish Mage | ochre 0x0798 | 4 | 200-235 | 8-11 | Mage | Medium | — | — |
| `PelasgCavebear` | a painted cave-bear | Grizzly Bear | hide 0x0967 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `PelasgWarchief` | a Pelasgian war-chief | Orc Brute | hide 0x0967 | 4 | 200-240 | 9-12 | Melee | Medium | — | — |

### Themed block — the Leleges cave-clan (9)
*The rival stone-folk; the bone-augur, trap-master and clan-mother carry drain/knockback/summon.*

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PelasgLelexForager` | a Leleges forager | Troglodyte | cave grey 0x0964 | 2 | 90-110 | 6-9 | Melee | Medium | — | — |
| `PelasgLelexKnapper` | a Leleges flint-knapper | Troglodyte | clay brown 0x0844 | 3 | 130-160 | 7-10 | Melee | Medium | — | pack instinct |
| `PelasgLelexHunter` | a Leleges hunter | Orc | cave grey 0x0964 | 3 | 130-160 | 7-10 | Archer | Medium | — | pack instinct |
| `PelasgLelexToadherd` | a Leleges toad-herd | Giant Toad | ochre 0x0798 | 3 | 130-160 | 7-10 | Melee | Slow | — | HitPoison Greater |
| `PelasgLelexAper` | a Leleges ape-keeper | Gorilla | hide 0x0967 | 4 | 200-240 | 8-11 | Melee | Fast | — | — |
| `PelasgLelexBrute` | a Leleges cave-brute | Orc Brute | cave grey 0x0964 | 4 | 200-240 | 8-11 | Melee | Slow | — | — |
| `PelasgLelexAugur` | a Leleges bone-augur | Orcish Mage | ochre 0x0798 | 4 | 200-235 | 8-11 | Mage | Medium | — | **Bone-Rattle** — OnGaveMelee 20%: 10 stam drain + flavor msg |
| `PelasgLelexTrapmaster` | a Leleges trap-master | Orc Bomber | clay brown 0x0844 | 4 | 200-240 | 8-11 | Melee | Fast | — | **Deadfall** — OnGotMelee 15%: knockback msg + 8 stam drain |
| `PelasgLelexMother` | the Leleges clan-mother | Troglodyte | hide 0x0967 | 4 | 200-240 | 9-12 | Mage | Medium | — | **Rouse the Clan** — OnGotMelee 15%: spawn 1 `PelasgLelexForager` (cap 2, dispel-vulnerable) |

### Ambient / fodder (3)

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PelasgCavebat` | a cave bat | Mongbat | cave grey 0x0964 | 2 | 80-100 | 4-7 | Melee | Fast | — | — |
| `PelasgGrub` | a painted grub | Slime | ochre 0x0798 | 2 | 80-100 | 4-7 | Melee | Slow | PoisonImmune | — |
| `PelasgToadling` | a spotted toadling | Giant Toad | clay brown 0x0844 | 2 | 80-100 | 4-7 | Melee | Slow | — | — |

### Mini-boss (1) — beside Phoroneus (elite L4), the clans' earth-born forefather

| Class | Display | Donor class | Hue | Lvl | HP | Dmg | AI | Speed | Immunities/Poison/Breath | Ability |
|---|---|---|---|---|---|---|---|---|---|---|
| `PelasgPelasgos` | Pelasgos, the Earth-Born | Troglodyte | hide 0x0967 | 4 | 235-240 | 10-14 | Melee | Fast | BleedImmune | **Sons of the Soil** — OnGotMelee 15%: spawn 1 `PelasgWhelp` (cap 2, dispel-vulnerable) · `DungeonElite` bag4 |

---

## Expansion Totals Appendix (v2)

| # | Family | Prefix | v1 | Core+ | Themed | Ambient | Mini-boss | **New (v2)** | **Cumulative** | New custom (≤5) |
|---|---|---|---|---|---|---|---|---|---|---|
| 6.1e | The Pyre | `Pyre*` | 12 | 10 | 9 | 3 | 1 | **23** | **35** | 4 — Cindercaller, Emberbrand, Forgemaster, Phaethon |
| 6.2e | The Rimehold | `Rime*` | 12 | 10 | 9 | 3 | 1 | **23** | **35** | 4 — Outrider, Gale, Herald, Cheimon |
| 6.3e | The Accursed Dig | `Cursed*` | 12 | 10 | 9 | 3 | 1 | **23** | **35** | 4 — Crossroads(aura), Wailer, Matron, Perses |
| 6.4e | The Myrmex Nest | `Myrmi*` | 10 | 12 | 9 | 3 | 1 | **25** | **35** | 4 — Goad, Spearcaller, Marshal, Menoitios |
| 6.5e | The Coiled Sanctum | `Ophian*` | 10 | 12 | 9 | 3 | 1 | **25** | **35** | 4 — Envenomer, Warden, Hierophant, Poine |
| 6.6e | The Arcadian Warband | `Lykai*` | 10 | 12 | 9 | 3 | 1 | **25** | **35** | 4 — Stalker, Priest, Houndmaster, Mainalos |
| 6.7e | The Hundred-Eyed Vault | `Argus*` | 12 | 10 | 9 | 3 | 1 | **23** | **35** | 4 — Blight(aura), Gildmage, Overwarden, Midas |
| 6.8e | The Wayman's Toll | `Wayman*` | 9 | 13 | 9 | 3 | 1 | **26** | **35** | 4 — Pinebender, Bedwright, Captain, Procrustes |
| 6.9e | The Painted Deep | `Pelasg*` | 5 | 17 | 9 | 3 | 1 | **30** | **35** | 4 — Augur, Trapmaster, Mother, Pelasgos |
| | **Total** | | **92** | **106** | **81** | **27** | **9** | **223** | **315** | **36** |

**Custom-ability budget (per family, v1 + v2 ≤ 8).** Pyre 3+4=7 · Rime 2+4=6 · Cursed 3+4=7 ·
Myrmi 2+4=6 · Ophian 2+4=6 · Lykai 2+4=6 · Argus 2+4=6 · Wayman 3+4=7 · Pelasg 1+4=5. Every
family is at or under 8. Custom = an `OnGaveMeleeAttack`/`OnGotMeleeAttack` proc or a
`DungeonAbilities.AuraPulse`; breath, `HitPoison`, pack-instinct and immunities stay free. Two
auras added (`CursedLampadCrossroads` fire, `ArgusTelchineBlight` energy) match the demonstrated
`AuraPulse` idiom; all summons are capped and `dispel-vulnerable` via `DungeonAbilities.TrySpawnAdd`.

**Structure.** Each family follows the shard standard: v1 ranks + named elite = the core, plus a
themed sub-faction (~9), core in-fill, ambient/fodder (3) and one new mini-boss to land 35. Mini-
bosses carry `DungeonElite` and force-drop `EliteBagLevel = own level` (bag7/8 mostly; Lykai bag6,
Pelasg bag4); all other v2 rows are trash at `LootBagLevel = level − 1`.

**Collision checks (v2, 2026-07-14).**
1. **New proper nouns** — grep-verified clear in `Mobiles/**` and `dev-docs/itemization/**`:
   **Kaminoi, Phaethon, Boread, Cheimon, Lampad(es), Perses, Aiakid, Menoitios, Ophite, Poine,
   Lykaonid, Mainalos, Telchine, Midas, Isthmian, Procrustes, Leleges, Pelasgos.** Rejected as
   reserved legendaries/mobs and replaced: **Kadmos** (armor), **Peleus** (fencing), **Empousa**
   (staves), **Panoptai** (shields), **Kampe** & **Sybaris** (fencing), **Telamon** (shields),
   **Myrmidon** (ML mobs — kept to prose only). Patron gods (Boreas, Hecate, Ophion, Lykaon,
   Argus, Prometheus, Aiakos) appear in prose; Aiakos is referenced as the existing Deceit elite.
2. **New class names** — all ~223 use the nine family prefixes and are distinct from every v1
   class in their family (fresh role words; e.g. `MyrmiSoldierRed` ≠ v1 `MyrmiSoldier`,
   `OphianConstrictorGreat` ≠ v1 `OphianConstrictor`, which summons reuse on purpose).
3. **Donors** — every donor is drawn from the v1/dungeon-ladder verified pool, plus five bodies
   re-confirmed present this pass: **Gorilla, Giant Toad, Wyvern, Lich Lord, Giant Serpent**
   (`class X` grep under `Projects/UOContent/Mobiles/`). AI/speed are per-row overrides.
