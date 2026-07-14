# Classic Five — Enhancement Spec (v1, 2026-07-14)

> **IMPLEMENTED 2026-07-14** (uncommitted): 11 classes under
> `Projects/UOContent/Mobiles/Dungeons/ClassicFive/`, 11 LevelConfig pins (6 roster +
> 5 elites), additive spawn file
> `Distribution/Data/Spawns/uoml/felucca/classic-elites.json` at the REAL dungeon
> coords (not placeholders). Build clean, migrations generated. Post-review fix: brood
> venom moved from a ctor-set field to per-skin `HitPoison` overrides (ctor fields don't
> survive deserialization). Elite tuning UNVALIDATED — live pass needed.

Light-touch identity pass on five existing T2A/Felucca dungeons. No rebuild: each gets a
**level band**, a **roster audit** with minimal pins, **one named contested elite**, and
**one skin of a shared recolor family**. Everything consumes systems that already ship
(Leveling, LootBags/Rarity, MobLevelOverrides, the `DungeonElite` force-drop idiom).
Companion doc: `dev-docs/dungeon-ladder.md` (the five NEW domain dungeons).

Grounding: HP values confirmed from creature classes (`Projects/UOContent/Mobiles/**`,
`SetHits`). Levels from `LevelConfig.MobLevelFromHits` (≤65 L1, ≤100 L2, ≤160 L3, ≤240 L4,
≤380 L5, ≤550 L6, ≤720 L7, ≤950 L8). Bag ceilings from `RarityConfig` (bag 5–6 → Epic,
bag 7–9 → Legendary).

## Implementation corrections (applied on integration)

- **Shame elite renamed**: designer's "Kymopoleia" collides with the Drowned Tholos elite —
  Shame's elite ships as **Thaumas, the Wavebreaker**.
- **"ContestedElite" = `DungeonElite`** (already built,
  `Projects/UOContent/Mobiles/Dungeons/DungeonElite.cs`) — parameterized bag level,
  force-drop telegraph, anti-farm clamp included.
- **Whirlpool "slow"** (Thaumas) simplified to the cold AoE pulse only (`DungeonAbilities.
  AuraPulse`) — no slow-debuff machinery on T2A.
- **Spawners additive**: one new file
  `Distribution/Data/Spawns/uoml/felucca/classic-elites.json` (5 elite + 5 brood
  spawners at the REAL coords below) — stock dungeon spawn files untouched.

## Overview

| Dungeon | Band | Signature theme | Named elite | Elite pin / bag | Bag ceiling | Brood skin |
|---|---|---|---|---|---|---|
| Despise | **L4** | earth / giants / lizardmen | Enkelados, the Buried Giant | L5 / bag 5 | Epic | Gigas Adder (ochre) |
| Deceit | **L5** | undead (Hades lane) | Aiakos, the Drowned Judge | L6 / bag 6 | Epic | Grave Asp (bone) |
| Shame | **L6** | elementals / flooded | Thaumas, the Wavebreaker | L7 / bag 7 | Legendary | Brine Serpent (sea-green) |
| Destard | **L7** | dragons / drakes | Ladon, the Sleepless Wyrm | L8 / bag 8 | Legendary | Drakescale Python (crimson) |
| Hythloth | **L8** | daemons / gargoyles | Alastor, the Tormentor | L9 / bag 9 | Legendary | Ashen Basilisk (charred) |

Headline audit finding: **the bands are aspirational peaks, not roster averages.** Every
classic roster is internally spread (L1 fodder → L6–7 heavies). Destard and Shame have
genuine on-band anchors; Despise, Deceit, and especially Hythloth read under their labels
and lean on their marquee monster + named elite to earn the band. Pins fix only true
"caster/breath punches above HP" cases and anchor the marquee — they do **not** rebalance
honest melee, and nothing is despawned.

## Roster pins (LevelConfig.MobLevelOverrides — global, type-keyed)

| Type | Pin | Why |
|---|---|---|
| `BloodElemental` | 6 | tanky caster reading L5 on raw HP (Shame anchor) |
| `FireGargoyle` | 5 | fire caster reading L4 (Hythloth) |
| `Daemon` | 6 | caster reading L5 (Destard + Hythloth) |
| `Drake` | 6 | breath weapon reading L5 (Destard) |
| `Dragon` | 7 | marquee: breath + magery reading L6 (Destard anchor) |
| `Balron` | 8 | marquee: summons + casts reading L7 (Hythloth anchor) |

Skipped (designer marked optional, keeping the table lean): Shade/Spectre/Wraith → 2.

## Elites (all inherit `DungeonElite`; guaranteed bag = pin level; real coords)

### Enkelados, the Buried Giant — Despise (pin L5, bag 5)
Cyclops body, granite-ochre ~0x972. HP 300–360, dmg 14–20. Signature **"The earth
heaves!"** — OnGotMelee ~15%: small physical AoE around the giant + brief freeze (2 s) on
the attacker. Adds: 1–2 Lizardmen (Spartoi), capped. Spawn: Despise L3 OgreLord hall
(~5558, 824). Respawn ~20 min.

### Aiakos, the Drowned Judge — Deceit (pin L6, bag 6)
Lich body, drowned blue-green ~0x835. HP 380–450, caster AI. Signature — OnGaveMelee:
drains 10–20 mana and heals self (attrition lich; forces burst). Adds: 1–2 Spectres.
Spawn: Deceit lich room L3 (~5314, 748). Respawn ~20 min.

### Thaumas, the Wavebreaker — Shame (pin L7, bag 7)
Water Elemental body (land-capable), storm-teal ~0x4F8. HP 480–540. Signature —
**whirlpool pulse**: AuraPulse cold AoE every few seconds (punishes clumping). Adds: 1–2
SeaSerpents. Spawn: Shame flooded galleries / kraken pools (~5557, 222). Respawn ~25 min.

### Ladon, the Sleepless Wyrm — Destard (pin L8, bag 8)
Dragon body, hoard-gold ~0x501. HP 620–700, high resists. Signature — enhanced fire breath
(dragon breath path, boosted). Adds: 1–2 Drakes. Spawn: Destard deep roost (~5185, 1006).
Respawn ~30 min.

### Alastor, the Tormentor — Hythloth (pin L9, bag 9)
Balron body, blood-hell ~0x21. HP 780–900, caster + high resists. Signature — **fire-curse
burst**: OnGaveMelee ~20% flamestrike-style fire pop. Adds: 1–2 Imps. Spawn: Hythloth demon
sanctum (~6086, 178). Respawn ~30 min.

## The Brood of Echidna (shared recolor family)

One archetype, five skins — shared skeleton + venom bite + ONE dungeon-flavored bonus.
Echidna, mother of monsters, scattered her serpent-children across the world. Ambient
signature spice, NOT elites: no guaranteed bag, normal level-scaled bag chance. HP set
in-band-minus-one so the level tag is honest without pins.

Base: abstract `EchidnaBrood` (GiantSerpent body, `HitPoison` scaling by band, melee AI) +
five thin concretes (hue/name/HP + one bonus):

| Dungeon | Skin | Hue | HP level | Venom | One bonus |
|---|---|---|---|---|---|
| Despise | Gigas Adder | dull ochre | L3 | Lesser | tail-sweep stam drain (OnGaveMelee 20%: −12 stam) |
| Deceit | Grave Asp | bone-pale | L4 | Regular | lifedrain on hit (OnGaveMelee 20%: self-heal) |
| Shame | Brine Serpent | sea-green | L5 | Regular | cold breath (HasBreath cold) |
| Destard | Drakescale Python | crimson | L6 | Greater | weak fire breath |
| Hythloth | Ashen Basilisk | charred black-red | L7 | Deadly | fire aura (AuraPulse) |

## Mood (optional polish, config-only)

No region-type changes: classic dungeons already run `DungeonRegion`. A per-dungeon
`LightLevel` tint on the existing region JSON entries is a single-property edit if wanted
later. Not part of this pass.

## Anti-abuse

Gap curve zeroes vet XP already; `DungeonElite`'s built-in clamp additionally downgrades
the guaranteed bag when the killer is >2 levels above the elite's pin (matters for
Despise/Deceit; vacuous by Hythloth). Legendaries are NOT one-per-shard (2026-07-14:
"globally unique" = unique names/IDs in the registry, not instance caps; dupes drop and
Divine Resonance echoes them). Long contested respawns (20–30 min) cap throughput. No
level-gating — open world stays open. Watch item: bag 7–9 elites become camp targets; if
toxic post-launch, jitter the respawn window, don't gate.

## Implementation cost

11 new classes (5 elites + `EchidnaBrood` base + 5 skins — `DungeonElite` already exists),
6 LevelConfig pins + 5 elite pins (type-keyed on the new classes), 1 additive spawn JSON.
Verify at implementation: donor Body/BaseSoundID copied from donor classes, breath/aura
hooks as used by the ladder dungeons.
