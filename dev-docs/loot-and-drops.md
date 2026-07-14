# Loot & Drops — Canonical Reference

Purpose: the single source of truth for how a killed monster turns into gear on the Age of Titans
shard — drop chance, bag level, bag contents, rarity odds, and the per-area drop tables. Every
number here is grounded in code; when the code changes, update this doc.

**Sources of truth** (read these, not this doc, when the two disagree):

| What | Where |
|---|---|
| Drop chance per mob level | `Projects/UOContent/Engines/LootBags/LootBagConfig.cs` |
| Rarity + category weights, one-item-per-bag roll | `Projects/UOContent/Engines/LootBags/LootRoller.cs` |
| Rarity ceilings per bag level, tier names/hues | `Projects/UOContent/Engines/Rarity/RarityConfig.cs` |
| Global drop wiring (mob death → bag) | `Projects/UOContent/Mobiles/BaseCreature.cs` (`OnBeforeDeath`, ~`:3035`) |
| Elite/boss guaranteed drops | `Projects/UOContent/Mobiles/Dungeons/DungeonElite.cs` |
| Bag item (open behavior, announce) | `Projects/UOContent/Items/Containers/LootBag.cs` |
| Per-creature level + loot column | `dev-docs/beast-reference.md` (GENERATED) |
| Pantheon bag design (pending impl) | `dev-docs/itemization/30-pantheon-bags.md` |
| Itemization framework (law) | `dev-docs/itemization/00-framework.md` |

---

## 1. The chain {#the-chain}

A monster dies and, if it drops loot at all, produces a **loot bag** tagged with a level 0-10. The
bag holds exactly ONE item. The item's rarity and theme are rolled from the bag level. Nothing is
revealed until a player opens the bag.

```
mob dies → drop chance (by mob level) → bag level → 1 item (rarity → category → theme → base)
```

### 1a. Drop chance {#drop-chance}

Rolled at mob death. Convex ramp, back-loaded so L8-10 feel like the payoff
(`LootBagConfig.cs:14-19`).

| Mob level | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 |
|---|---|---|---|---|---|---|---|---|---|---|---|
| Drop chance | 0% | 10% | 13% | 16% | 20% | 24% | 29% | 34% | 39% | 44% | 50% |

L0 (trivial critters) never drop in the open world.

### 1b. Bag level {#bag-level}

The level stamped on the bag, which drives every downstream roll.

| Mob kind | Bag level | Source |
|---|---|---|
| Stock creature (default) | = mob XP level | `BaseCreature.LootBagLevel` → `LevelConfig.GetMobLevel` (`BaseCreature.cs:2971`) |
| Custom dungeon trash | pinned override, ≈ mob level − 1 | per-class `LootBagLevel =>` override |
| Open-world biome mobs | = mob level (no override) | stock default; beast-reference "level-scaled roll" |
| Elite | `EliteBagLevel`, ×`EliteBagCount` (1) | `DungeonElite.cs:21-24` |
| Boss | `EliteBagLevel`, ×`EliteBagCount` (2) | `DungeonElite.cs:24` overridden to 2 |
| Hades (Stygian Lord) | 1× bag 10 + 1× bag 9 | `StygianLord.cs:61, 84-91` |

Pets/controlled creatures never drop (`DropsLootBag => !Controlled`, `BaseCreature.cs:2967`).
Elites opt out of the global roll (`DropsLootBag => false`) and force their guaranteed bag(s)
instead (`DungeonElite.cs:18`).

**Elite anti-farm clamp** (`DungeonElite.cs:31-41`): if the killing blow lands from a player (or
their pet) more than 2 levels above the elite, the guaranteed drop downgrades to the normal
trash-style roll at the same bag level. Last-hit rule — a high-level friend finishing the kill
costs the group the guarantee.

**Hades is the only bag-10 source on the shard** (`StygianLord.cs:7`). Two bag-10s would mean two
guaranteed Legendaries per respawn, so his second bag is a 9.

### 1c. Bag contents — one item {#bag-contents}

`LootRoller.Roll(bagLevel)` builds exactly one item (`LootRoller.cs:151-159`). Roll order:
rarity → category → family → theme → base. Never any Commons from a bag (user directive
2026-07-08) — Uncommon is the absolute floor.

### 1d. Rarity weights {#rarity-weights}

Index = bag level, columns = tier weight. Verbatim from `LootRoller._rarityWeights`
(`LootRoller.cs:42-55`).

| Bag level | Common | Uncommon | Rare | Epic | Legendary |
|---|---|---|---|---|---|
| 0  | 0 | 100 | 0  | 0  | 0   |
| 1  | 0 | 100 | 0  | 0  | 0   |
| 2  | 0 | 80  | 20 | 0  | 0   |
| 3  | 0 | 65  | 35 | 0  | 0   |
| 4  | 0 | 50  | 42 | 8  | 0   |
| 5  | 0 | 0   | 85 | 15 | 0   |
| 6  | 0 | 0   | 75 | 25 | 0   |
| 7  | 0 | 0   | 66 | 30 | 4   |
| 8  | 0 | 0   | 55 | 39 | 6   |
| 9  | 0 | 0   | 0  | 90 | 10  |
| 10 | 0 | 0   | 0  | 0  | 100 |

Bag 10 is a **guaranteed Legendary** (user directive 2026-07-14); bag 9 is 90/10 Epic/Legendary.

### 1e. Category weights {#category-weights}

What kind of item the bag holds. Weights, not framework-fixed (`LootRoller.cs:58-64`).

| Category | Weapon | Armor | Shield | Jewelry | Clothing |
|---|---|---|---|---|---|
| Weight | 45 | 25 | 10 | 12 | 8 |

### 1f. Rarity ceilings {#rarity-ceilings}

A defensive second gate re-applied after the rarity roll (`RarityConfig._maxRarityByBagLevel`,
`RarityConfig.cs:34-47`). Ceilings are ≥ the weight-table floor at every level, so the clamp
never pushes a roll below its floor.

| Bag level | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 |
|---|---|---|---|---|---|---|---|---|---|---|---|
| Max rarity | Unc | Unc | Rare | Rare | Epic | Epic | Epic | Leg | Leg | Leg | Leg |

### 1g. Opening & the announce {#opening}

The bag shows no container gump. Double-clicking dumps its item into the finder's backpack and the
bag vanishes (`LootBag.cs:33-64`). The **world broadcast for top-tier finds fires here, at
bag-open** — when the finder actually sees the item — **never at mob-death roll time**
(`LootBag.cs:58-60`). An unlooted bag never spoils its contents. The broadcast threshold is the
`rarity.announceMinTier` config setting, **default Legendary** (`RaritySystem.cs:38`).

---

## 2. Per-area drop tables {#drop-tables}

Trash-bag ranges are the observed span of `LootBagLevel` overrides within each family (open-world
biomes use the stock default = mob level). Named elites/bosses list their guaranteed
`EliteBagLevel`; `×2` marks a boss (`EliteBagCount = 2`). Mob levels derive from
`dev-docs/beast-reference.md`.

### 2a. Barrow — Newbie dungeon {#barrow}

| Area | Prefix | Mob levels | Trash bags | Named elites (bag ×count) |
|---|---|---|---|---|
| Barrow | `Newbie` | 0-2 | 0-1 | Newbie Elite → **2** |

### 2b. Five Domains (L4-10 ladder) {#five-domains}

| Area | Prefix | Trash bags | Named elites / bosses (bag ×count) |
|---|---|---|---|
| Drowned Tholos | `Tide` | 1-4 | Tide Navarch → 5; Tide Warden → 5; Tide Herald → **5 ×2** |
| Cinderworks | `Cinder` | 4-5 | Cinder Cyclops → 6; Cinder Kedalion → 6; Cinder Heart → **6 ×2** |
| Nemean Wildwood | `Wyld` | 5-6 | Wyld Prokris → 7; Wyld Matriarch → 7; Wyld Stag → **7 ×2** |
| Stormcrown Aerie | `Storm` | 7-8 | Storm Father → **8 ×2**; Storm Chained → 9; Storm Ephialtes → 9 |
| Stygian Deep | `Stygian` | 8-9 | Rhadamanthys → 9; Charon → 9; Cerberus → 9; **Hades → 10 + 9** |

### 2c. Classic Five {#classic-five}

Roaming/shared elites (`Mobiles/Dungeons/ClassicFive/`): Enkelados → 5, Aiakos → 6, Thaumas → 7,
Ladon → 8, Alastor → 9.

| Dungeon | Prefix | Trash bags | Named elites (bag ×count) |
|---|---|---|---|
| Despise | `Gaian` | 2-4 | Chthonios → 4; Gaian Peloreus → 4 |
| Deceit | `Drowned` | 3-5 | Drowned Phrontis → 5; Minos → 5 |
| Shame | `Brine` | 4-6 | Glaukos → 6; Brine Ormenos → 6 |
| Destard | `Drakon` | 5-7 | Drakon Ismenos → 7; Pythios → 7 |
| Hythloth | `Tartarus` | 6-7 | Eurynomos → 8; Tartarus Menoetius → 8 |

### 2d. Gap dungeons (9) {#gap-dungeons}

Classic UO dungeons reskinned to Greek families.

| Dungeon (classic) | Prefix | Trash bags | Named elites (bag ×count) |
|---|---|---|---|
| Fire | `Pyre` | 4-6 | Pyre Phaethon → 7; Pyre Phlegyas → 8 |
| Ice | `Rime` | 3-6 | Rime Cheimon → 7; Rime Abaris → 8 |
| Khaldun | `Cursed` | 4-7 | Cursed Perses → 8; Cursed Aeetes → 9 |
| Solen Hive | `Myrmi` | 2-6 | Myrmi Menoitios → 7; Myrmi Myrmex → 7 |
| Terathan Keep | `Ophian` | 3-7 | Ophian Keto → 7; Ophian Poine → 8 |
| Orc Caves | `Lykai` | 1-5 | Lykai Mainalos → 6; Lykai Nyktimos → 6 |
| Covetous | `Argus` | 2-7 | Argus Midas → 7; Argus Erysichthon → 8 |
| Wrong | `Wayman` | 2-6 | Wayman Procrustes → 7; Wayman Periphetes → 8 |
| Painted Caves | `Pelasg` | 1-3 | Pelasg Pelasgos → 4; Pelasg Phoroneus → 4 |

### 2e. Open-world biomes (5) {#open-world}

Biome trash uses the stock default (bag = mob level; no `LootBagLevel` override).

| Biome | Prefix | Mob levels | Trash bags | Named elite (bag) |
|---|---|---|---|---|
| Grove | `Grove` | 2-3 | 2-3 | Silenos → 3 |
| Restless | `Restless` | 2-3 | 2-3 | Barrow-King → 3 |
| Shore | `Shore` | 3-4 | 3-4 | Karkinos → 4 |
| Peak | `Peak` | 4-5 | 4-5 | Talos → 5 |
| Mire | `Mire` | 5-6 | 5-6 | Fen-Mother → 6 |

### 2f. The Labors (6 named hunts) {#labors}

Single named boss hunts, no trash tier — each is one elite with a guaranteed bag
(`Mobiles/OpenWorld/Labors/`).

| Labor | Level | Bag |
|---|---|---|
| Ceryneian Hind | 4 | 4 |
| Calydonian Boar | 5 | 5 |
| Nemean Lion | 6 | 6 |
| Stymphalian Matriarch | 6 | 6 |
| Erymanthian Boar | 7 | 7 |
| Cretan Bull | 8 | 8 |

---

## 3. Pantheon layer (DESIGN — pending implementation) {#pantheon}

**Status: approved design 2026-07-14, NOT yet in code.** Full spec:
`dev-docs/itemization/30-pantheon-bags.md`.

Adds a *source* dimension to bag drops: every themed creature family serves one of 11 gods. A
themed mob's bag is marked with that god 70% of the time (30% generic), and a marked bag holds
ONLY that god's gear at every rarity tier — enabling targeted pantheon farming.

Key rules (from `30-pantheon-bags.md`):

- **70/30 split** for themed trash; **elites/bosses always drop the god's bag** (no 70/30).
- **Themed bags are 100% god-locked**, every rarity tier — no leaks. Generic bags roll exactly as
  today (§1 above, unchanged).
- **Rarity weights untouched** — bag 9 = 90/10 Epic/Legendary, bag 10 = 100% Legendary.
- **11 domains**, each with a flagship dungeon laddering to a bag-10 **avatar boss** = a
  guaranteed god-locked legendary farm route: Sea/Poseidon (Drowned Tholos), Forge/Hephaestus
  (Cinderworks), Hunt/Artemis (Nemean Wildwood), Sky/Zeus (Stormcrown Aerie), Underworld/Hades
  (Stygian Deep), Nature/Demeter (Despise), War/Ares (Hythloth), Night/Nyx (Khaldun),
  Wind/Hermes (Ice), Aegis/Athena (Covetous), Sun/Apollo (Fire).
- **God-sanctum expansion**: each flagship gains a sanctum wing extending its family ladder to
  L10 (~100+ new creatures), crowned by the avatar boss.
- Hades stays distinct: hardest avatar fight + his signature bonus bag 9.

The pantheon layer changes *where* legendaries come from, not the rarity odds or drop chances in
§1 — those tables remain authoritative.

---

## Unconfirmed / gaps

- **Trash-bag ranges** are the observed min/max of `LootBagLevel` overrides per family folder as
  of 2026-07-14; individual mobs within a family vary across that span. For exact per-mob values,
  see the beast-reference "Loot" column.
