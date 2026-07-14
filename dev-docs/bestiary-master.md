# Bestiary Master — Age of Titans (taxonomy v1, 2026-07-14)

> The single master taxonomy for every hostile creature on the shard. Adapts the UO
> Outlands creature **system** (`dev-docs/outlands-creatures.md`, `dev-docs/outlands-bestiary.md`)
> to our own **1–10 mob-level ladder** (`Projects/UOContent/Engines/Leveling/LevelConfig.cs`).
> Absorbs the custom creatures built (942 as of 2026-07-14 — barrow, open world, Classic Five,
> Five Domains, and the gap families, all since implemented and expanded to full 35-per-family
> rosters; see beast-reference.md for the authoritative list). The changeover itself lives in
> `dev-docs/spawn-migration-map.md`; this doc is the *what exists / what's missing* half.
>
> **Design stubs only** for the gap families — names, prefix, concept, band, size, patron,
> and which stock types they replace. Full rosters come later, one family at a time, each
> grep-checked against `dev-docs/itemization` and existing classes at build time.

---

## 1. The system, ported from Outlands

Outlands runs its whole bestiary off **one derived number** (Difficulty → Gold ×10 → all
rewards). We already have that shape: our **mob level (1–10)** is the single knob, derived from
HP (`MobLevelFromHits`) or hand-pinned (`MobLevelOverrides`), and it drives XP
(`BaseMobXP = level × 100`), the overhead tag hue, the XP gap multiplier, and the loot-bag
tier. Retune a creature's HP or pin and everything downstream moves with it. No separate loot
tables anywhere — same philosophy as Outlands, same as our rarity D-values.

Five Outlands principles carry over verbatim (all five are already in use across the built
rosters — see `dev-docs/dungeon-ladder.md` §"Outlands patterns stolen"):

1. **Role-first stat model.** A creature is a *role*, not a stat sheet: **Melee** (low resist,
   plain block), **Caster** (Mage AI, carries the immunities/high resist), **Skirmisher**
   (fast, poison / hit-and-run, low HP), **Tank** (high HP+armor, slow, immunities). Role is
   expressed with the free tools the engine already gives us: `AI` type, `Speed`,
   `HitPoison`, breath, pack instinct, and immunity flags.
2. **Harder = a different role, not bigger numbers.** Climbing a family's ranks means swapping
   role (melee → caster → summoner), not just adding HP. Elites are summoners/casters/CC, not
   fatter melee — kill-the-alpha, not out-tank-the-sack.
3. **Resist by role, not by rank.** Casters and constructs carry the immunities and high
   resist at every tier; glass melee stay low even when their HP is high. Never blanket-buff
   resist with level.
4. **One signature ability per creature.** At most one active proc (`OnGaveMeleeAttack` /
   `OnGotMeleeAttack` / aura) plus at most one passive. Breath, `HitPoison`, pack instinct and
   immunities are *free* and don't count against the budget. Most roster members are pure stat
   blocks.
5. **Recolor variants.** One archetype × a hue × a reflavored signature = a new dungeon's worth
   of creature for zero new combat math (the `PantheonSentinel` line, the `EchidnaBrood` line).

**The family is the unit of content.** One theme + the HP curve + role-appropriate resists +
per-rank names = a 15–35-creature roster from an afternoon of tuning. That is how the shard
already reached ~276 creatures, and how the gap families in §6 will be filled.

### What we deliberately do NOT port
- **Separate wild/tamed stat budgets** — no taming content yet; revisit if pets ship.
- **Paragon overlay / participation-scaled raid bosses** — Outlands' two elite systems. We use
  the simpler `DungeonElite` force-drop + over-level clamp instead. Paragons are a future
  option, not in this taxonomy.
- **Progressive per-hour loot, weekly region/dungeon rotation, Black Goods, Antiquities** —
  reward-economy layers we may borrow later; out of scope here.

---

## 2. The ladder (our tiers replace Outlands Difficulty)

Source of truth: `LevelConfig.MobLevelFromHits` and `MobLevelOverrides`. HP ceiling per band is
the design contract every roster is tuned inside.

| Lvl | HP ceiling | Stock exemplars | Role feel | Trash bag* |
|---|---|---|---|---|
| **0** | pinned only | farm/ambient (sheep, bird, rabbit) | non-combat, gray tag, 0 XP | none |
| **1** | ≤65 | mongbat, giant rat, slime, headless | first safe kills | bag 0 |
| **2** | ≤100 | zombie, skeleton, wolves | fodder | bag 1 |
| **3** | ≤160 | orc, ratman, low elementals | early trash | bag 2 |
| **4** | ≤240 | ogre, troll, lich | first melee walls | bag 3 |
| **5** | ≤380 | ore elementals, elder gazer, efreet | mid | bag 4 |
| **6** | ≤550 | drake, daemon | upper-mid | bag 5 |
| **7** | ≤720 | titan, blood elemental, phoenix | heavy | bag 6 |
| **8** | ≤950 | dragon, white wyrm, ogre lord | gear wall | bag 7 |
| **9** | ≤2400 | balron, ancient wyrm, hydra | endgame trash | bag 8 |
| **10** | >2400 | custom bosses only | graduation | bag 9 (Hades only: bag 10) |

\* Two bag conventions coexist (both honest): **dungeon trash** overrides `LootBagLevel =
level − 1` (barrow idiom); **open-world trash** carries no override and rides the natural
level→bag mapping. Casters/breath-users get a `MobLevelOverrides` **pin** so their tag/XP/bag
stay put while HP is tuned; melee that read honestly on HP need no pin.

**Role signals per band are set by role, not tier:** Mage-AI casters carry `PoisonImmune` (and
`BleedImmune` on constructs/undead) at every level; skirmishers run Fast/VeryFast with
`HitPoison`; tanks run Slow with high HP + immunities; plain melee stay bare.

---

## 3. Reward rules (the loot contract)

- **Trash bag = level** (via the two conventions above). Rarity of a bag climbs with its
  number; `RarityConfig` sets bag 5–6 → Epic ceiling, bag 7–9 → Legendary ceiling.
- **Elites & bosses = `DungeonElite`**: opt out of the trash roll, **force-drop a guaranteed
  bag** (`EliteBagLevel`), telegraphed with a corpse sparkle/shimmer. Bosses set
  `EliteBagCount = 2`. Built-in **over-level clamp**: a killer more than 2 levels above the
  elite's pin downgrades the guaranteed bag to a trash-style roll (anti-farm).
- **Bag 10 = Hades only.** `StygianLord` (the Stygian Deep boss) is the shard's *only* bag-10
  faucet, dropping 1×bag10 + 1×bag9. The Sky boss is deliberately held at bag 9 to keep Hades
  unique. Every other elite/boss tops out at bag 9.
- **Legendaries are NOT one-per-shard.** "Globally unique" means unique names/IDs in the
  registry; duplicates still drop and Divine Resonance echoes them.

---

## 4. The built families (≈276 creatures, absorbed)

Organized by **where they live**, low band to high. Each entry: prefix · patron · band ·
members · the stock types it already stands in for. Full stat blocks live in the cited docs.

### 4.1 Newbie tier — the Barrow of the Unremembered (L0–3) · `Newbie*`
*Patron seed: Hades (the sealed deeper gate).* `dev-docs/newbie-dungeon-mobs.md`.
The tutorial dungeon: 14 combat + 1 greeter (expanded to 15 total 2026-07-14). Foreshadows
Hermes/Ares/Hades via its three elites (Charon → Fallen Champion → Hollow Warden). Stands in
for graveyard fodder (skeleton, zombie, ghoul, giant rat) in the starter zone.

| Rank | Members |
|---|---|
| L1 trash | NewbieBoneShade, NewbieGraveRat, NewbieCorpseCrawler, NewbieBoneBowman, NewbieBarrowBat |
| L2 trash | NewbieGraveMiasma, NewbieMourner, NewbieChanter, NewbieGraveArcher, NewbieWight, NewbieRestlessArcher |
| L3 elites | NewbieCharon, NewbieFallenChampion, NewbieHollowWarden |
| prop | NewbieFerryman (greeter, blessed) |

### 4.2 Open world — five biome families + the Labors (L2–8)
*The gentle band a barrow graduate meets in the wild, spawned alongside stock fauna.*
`dev-docs/open-world-bestiary.md`. Each family is 6 members over 2 levels; the Labors are 6
named roaming `DungeonElite` hunts.

| Family | Prefix | Patron / myth | Band | Replaces (stock) |
|---|---|---|---|---|
| Groves | `Grove*` | Artemis — fey-touched wood | L2–3 | forest deer/wolves/boar/satyr/spider |
| Peaks | `Peak*` | bronze-age giant-kin / rocs | L4–5 | ettin, ogre, cyclops, harpy, gazer |
| Mire | `Mire*` | Lernaean hydra-spawn | L5–6 | alligator, giant serpent, lizardman, bog thing, slime |
| Restless | `Restless*` | the unburied dead | L2–3 | overworld skeleton, zombie, ghoul, spectre, wraith |
| Shore | `Shore*` | siren-touched tide-beasts | L3–4 | coastal crab/snake/serpent/harpy |
| The Labors | `Labor*` | Heracles's six beasts | L4–8 | fixed-landmark world-hunt elites (no stock type) |

### 4.3 Classic Five — enhanced existing dungeons (L4–8)
*Light-touch identity pass: each classic dungeon gets a band, a themed family, a named elite,
and one Echidna-brood serpent skin.* `dev-docs/classic-five-enhancements.md` +
`dev-docs/classic-five-bestiary.md`. Families spawn **alongside** the untouched stock roster.

| Dungeon | Prefix | Patron / myth | Band | Named elite | Replaces (stock lane) |
|---|---|---|---|---|---|
| Despise | `Gaian*` | Gaia — the Spartoi (sown men) | L4 | Chthonios / Enkelados | lizardman, ettin, ogre, earth elemental |
| Deceit | `Drowned*` | Hades — the sea-taken dead | L5 | Minos / Aiakos | skeleton, zombie, spectre, wraith, lich |
| Shame | `Brine*` | Poseidon — storm-brood elementals | L6 | Glaukos / Thaumas | water/air/ice/snow elementals |
| Destard | `Drakon*` | Ladon — the dragon-cult | L7 | Pythios / Ladon | drake, wyvern, ophidian, dragon |
| Hythloth | `Tartarus*` | the Tartarus daemons | L8 | Eurynomos / Alastor | daemon, gargoyle, imp, balron |

**Shared recolor line — the Brood of Echidna** (`EchidnaBrood` base): Gigas Adder (Despise),
Grave Asp (Deceit), Brine Serpent (Shame), Drakescale Python (Destard), Ashen Basilisk
(Hythloth). One serpent archetype, five hues, one dungeon-flavored bonus each — the canonical
recolor-variant proof.

### 4.4 Five Domains — the new dungeon ladder (L4–10)
*The graduation climb from barrow to Hades's throne.* `dev-docs/dungeon-ladder.md` (36 core) +
`dev-docs/dungeon-ladder-bestiary.md` (+140 expansion). Each dungeon: ~35 creatures split core
family / themed sub-block / ambient fodder / mini-boss / elite / boss.

| Dungeon | Prefix | Patron | Band | Elite → Boss | Themed sub-block |
|---|---|---|---|---|---|
| The Drowned Tholos | `Tide*` | Poseidon (Sea) | L4–5 | Kymopoleia → Aigaion | the crew of the *Pelagos* |
| The Cinderworks | `Cinder*` | Hephaestus (Forge) | L5–6 | Brontes → Kelmion | the Kerykes bronze-servants |
| The Nemean Wildwood | `Wyld*` | Artemis (Hunt) | L6–7 | Atalanta → Elaphos | the huntresses of the *Thiasos* |
| The Stormcrown Aerie | `Storm*` | Zeus (Sky) | L8–9 | Enceladus → Typhon | the Anemoi wind-spirits |
| The Stygian Deep | `Stygian*` | Hades (Underworld) | L9–10 | Charon + Cerberus → **Hades** | the Erinyes and the judged dead |

**Shared recolor line — the Pantheon Sentinel** (`PantheonSentinel` base): one elemental-body
melee skeleton, five hues + five breath types, each doubling as its dungeon's boss-room
custodian. The five-dungeon recolor proof.

**The full-circle payoff:** the Stygian Deep is the barrow's sealed gate opened — Charon reuses
`NewbieCharon`'s hue/body, Cerberus is the grown `NewbieHollowWarden`. Hades (`StygianLord`) is
the only bag-10 source on the shard.

### 4.5 Cross-cutting roster totals (built)

| Group | Classes | Doc |
|---|---|---|
| Barrow (Newbie) | 14 combat + 1 greeter | newbie-dungeon-mobs.md |
| Open world biomes (51 × 5) | 255 | open-world-bestiary.md (design v1 — rosters in beast-reference.md) |
| The Labors | 6 | open-world-bestiary.md |
| Five Domains (35×4 + 36) | 176 | dungeon-ladder.md + dungeon-ladder-bestiary.md |
| Classic Five (33×5 + 11 elites/brood) | 176 | classic-five-enhancements.md + classic-five-bestiary.md |
| Gap families (35 × 9) | 315 | gap-families-bestiary.md (design v1 — rosters in beast-reference.md) |
| **Total built** | **942** (excl. greeter) | beast-reference.md (GENERATED, authoritative) |

---

## 5. Coverage map — where the world is themed

| World area (stock spawn file) | Themed by | Status |
|---|---|---|
| Starter graveyard / barrow | `Newbie*` | ✅ built |
| Overworld forests / mountains / swamps / graveyards / coasts | `Grove/Peak/Mire/Restless/Shore*` | ✅ built (additive) |
| Despise / Deceit / Shame / Destard / Hythloth | `Gaian/Drowned/Brine/Drakon/Tartarus*` | ✅ built (additive) |
| Five new domain dungeons | `Tide/Cinder/Wyld/Storm/Stygian*` | ✅ built |
| **Fire** | — | ❌ GAP (§6.1) |
| **Ice** | — | ❌ GAP (§6.2) |
| **Khaldun** | — | ❌ GAP (§6.3) |
| **Solen Hive + Terathan insectoids** | — | ❌ GAP (§6.4) |
| **Terathan Keep ophidians** | — | ❌ GAP (§6.5) |
| **Orc Caves** | — | ❌ GAP (§6.6) |
| **Covetous** | — | ❌ GAP (§6.7) |
| **Wrong** | — | ❌ GAP (§6.8) |
| **Painted Caves** | — | ❌ GAP (§6.9, small) |
| Sanctuary, Blighted Grove, Palace of Paroxysmus, Prism of Light | — | ⚠️ ERA-MISMATCH (§6.10) |

---

## 6. Gap families (design stubs)

Each stub: **prefix · patron · concept · band · ~size · replaces**. Sizes assume the family
unit — a ~6–15-member roster split by the standard core/themed/ambient/mini-boss shape. All
names below are **provisional**; the family prefix is chosen collision-free against existing mob
classes, and every individual member must be grep-checked against `dev-docs/itemization` and
`Mobiles/**` at roster time (the `Wyld*`/Nemean precedent: sharing a proper noun with a
legendary item is fine flavor; sharing a *class name* is not).

Ideas are lifted from the Outlands bestiary (`outlands-bestiary.md`) and reflavored to Greek
myth — Outlands' Inferno/Winterlands/Kraul-Hive/Ossuary rosters are the raw material.

### 6.1 Fire dungeon — `Pyre*` · Phlegethon / the fire-Titan's pit
The burning river Pyriphlegethon has broken through into the Fire dungeon's lava halls;
fire-things and the shades of the unburnt walk the coals. Distinct from the Cinderworks
(Hephaestus's *forge* — craft, not raw fire): this is elemental conflagration and Titan-fire,
closer to Prometheus's stolen flame left to run wild.
**Band L5–8 · ~10–12 members.** Replaces: FireElemental, HellHound, HellCat, LavaSnake,
LavaLizard, LavaSerpent, Efreet, fire Daemon, fire-lane EvilMage/SkeletalMage/Lich, Slime,
GiantSerpent (Fire) — and the fire types leaking into **Trinsic Passage** (FireElemental,
LavaSnake, LavaLizard, HellCat).

### 6.2 Ice dungeon — `Rime*` · Boreas & Khione / the Hyperborean cold
The north wind Boreas and the snow-maiden Khione hold the Ice dungeon as a frozen antechamber
to the world's edge; ratman tribes froze into rime-bound thralls, and the cold itself has
teeth. Cold immunities carry the theme; skirmisher snakes and tank frost-giants anchor the
ranks.
**Band L4–9 · ~12–14 members.** Replaces: Ratman/RatmanArcher/RatmanMage, IceSnake, IceSerpent,
SnowElemental, IceElemental, FrostTroll, IceFiend, ArcticOgreLord, WhiteWyrm, FrostOoze,
FrostSpider.

### 6.3 Khaldun — `Cursed*` (or `Hekate*`) · Hecate / the accursed expedition
A doomed dig woke something buried; its diggers are cursed to guard it, and a death-cult of
Hecate — witchcraft, crossroads, restless shades — feeds the seal. Undead-heavy but *living
cult + curse*, deliberately distinct from the Stygian Deep (the underworld proper). Absorbs the
existing Khaldun named NPCs as set-dressing bosses.
**Band L5–9 · ~12–14 members.** Replaces: ShadowFiend, KhaldunZealot, KhaldunSummoner,
SpectralArmour, Cursed, AncientLich, BoneMagi, SkeletalMage, Khaldun Skeleton/Zombie/BoneKnight/
SkeletalKnight; the Khaldun named (TavaraSewel, GrimmochDrummel, LysanderGathenwale, MorgBergen)
become the cult's captured/corrupted archaeologists. HarrowerTentacles → FLAG (event-bound).

### 6.4 Solen Hive + Terathan insectoids — `Myrmi*` · Zeus / the Myrmidons (ant-folk)
Zeus made the Myrmidons from ants for Aiakos (already our Deceit elite — a clean tie). The Solen
hive and the Terathan swarm are recast as a single myrmex ant-nation: workers, warriors, a
brood-queen. Pack instinct and a queen-summoner mini-boss are the signature.
**Band L3–7 · ~10 members.** Replaces: BlackSolenWorker/Warrior/Queen, RedSolenInfiltrator
Warrior/Queen, AntLion, Beetle (Solen), and the Terathan insectoids TerathanDrone/Warrior/
Avenger/Matriarch.

### 6.5 Terathan Keep ophidians — `Ophian*` · Ophion & Echidna / the serpent-cult
The serpent-men who share the Keep are the cult of Ophion, the elder serpent-titan, and
Echidna's brood — snake-priests, scaled knights, a matriarch. Poison discipline is the through
-line; ties to the existing `EchidnaBrood` recolor flavor. (Terathan Keep is a contested two
-faction den: `Myrmi*` §6.4 takes the insect side, `Ophian*` takes the serpent side.)
**Band L4–9 · ~10 members.** Replaces: OphidianWarrior/Knight/Mage/Matriarch/Archmage, and the
Keep's Nightmare, Balron, Dragon, Drake garrison.

### 6.6 Orc Caves — `Lykai*` · Lykaon / the Arcadian wolf-warband
Lykaon, the Arcadian king cursed into a wolf, leads a savage wolf-cult of wild men and their
dire packs. Orcs recast as feral Arcadian raiders; the dire wolves are the cult's hounds; a
shaman-caster and a war-captain lead. Pack instinct + skirmisher wolves define it.
**Band L2–6 · ~10 members.** Replaces: Orc, OrcishMage, OrcishLord, OrcCaptain, OrcBomber,
OrcBrute, orcscout, OrcCamp, DireWolf, and the Caves' EarthElemental / Corpser / GiantRat.
(Overworld orc/rat *camps* — OrcCamp, RatCamp — convert here too.)

### 6.7 Covetous — `Argus*` (or `Panopt*`) · Argus Panoptes / the hoard-guard
The hundred-eyed giant Argus guards a gold-cursed hoard; his lesser eyes (the gazers) drift the
halls, and everything the greed drew in — undead treasure-hunters, spiders in the vaults,
a hoard-drake — has been bound to watch. The all-seeing eye is the motif (gazer lane), the
cursed hoard the reward flavor.
**Band L3–9 · ~12 members.** Replaces: Gazer, GazerLarva, ElderGazer, StoneHarpy, Harpy,
Corpser, HeadlessOne, Covetous Skeleton/Zombie/Spectre/Shade/Wraith/Mummy/RottingCorpse,
GiantSpider, DreadSpider, Slime, Lich, Drake, Dragon, WaterElemental (Covetous).

### 6.8 Wrong — `Wayman*` · Theseus's road-villains / the bandit-fortress + Talos
The prison-fort of Wrong is held by a robber-band in the mold of the road-villains Theseus
slew (Sciron, Sinis, Procrustes), with a captured automaton (a lesser Talos) and its handler as
the boss pair. Human bandits (a rare human-body family) + one construct lane.
**Band L3–8 · ~9 members.** Replaces: Brigand, JukaWarrior, JukaLord, JukaMage, Golem,
GolemController. (Overworld Brigand converts here too.)

### 6.9 Painted Caves — `Pelasg*` · the Pelasgians / aboriginal cave-folk (small)
The pre-Greek aboriginal cave-dwellers, primitive and territorial. Smallest gap — three
troglodyte types + vermin. **Could fold into `Lykai*` §6.6** if a standalone family isn't worth
it. **Band L2–4 · ~4 members.** Replaces: Troglodyte, Grobu, Lurg (named leaders), Painted-Caves
GiantRat/Rat.

### 6.10 Era-mismatch areas — FLAG, do not theme yet
Sanctuary (ML savage/juka: Szavetra, MougGuur, Chiikkaha, Doppleganger, Changeling,
Gargoyle Enforcer/Destroyer), **Blighted Grove**, **Palace of Paroxysmus**, **Prism of Light**
are AOS/ML/SE-era content on a T2A shard. Their spawns (plague beasts, crystal creatures, acid
elementals, insane dryads, Hydra/CrystalHydra, ShadowWyrm) are **anachronistic**. Recommend:
decide era policy first (disable the areas, or theme them last as a separate pass). No gap
family proposed until that call is made — see `dev-docs/spawn-migration-map.md` FLAG table.

### Gap-family summary

| # | Family | Prefix | Band | ~Size | Area |
|---|---|---|---|---|---|
| 6.1 | Phlegethon fire | `Pyre*` | L5–8 | 10–12 | Fire (+ Trinsic Passage) |
| 6.2 | Boreal cold | `Rime*` | L4–9 | 12–14 | Ice |
| 6.3 | Hecate's accursed | `Cursed*` | L5–9 | 12–14 | Khaldun |
| 6.4 | Myrmidon ant-folk | `Myrmi*` | L3–7 | 10 | Solen Hive + Terathan swarm |
| 6.5 | Ophion serpent-cult | `Ophian*` | L4–9 | 10 | Terathan Keep (serpents) |
| 6.6 | Lykaian wolf-warband | `Lykai*` | L2–6 | 10 | Orc Caves (+ overworld orc/rat camps) |
| 6.7 | Panoptes hoard-guard | `Argus*` | L3–9 | 12 | Covetous |
| 6.8 | Wayman brigands | `Wayman*` | L3–8 | 9 | Wrong |
| 6.9 | Pelasgian cave-folk | `Pelasg*` | L2–4 | 4 | Painted Caves (foldable) |

**~9 gap families, ~90 future creatures** to reach full themed coverage of the T2A hostile
world (era-mismatch areas excluded pending an era-policy call).

---

## 7. Reward economy recap (stack order)

1. **Base:** mob level → XP (`level × 100` × gap multiplier) + loot-bag tier.
2. **Bag tier by source:** trash = level (two conventions); elite/boss = guaranteed
   `EliteBagLevel`; **bag 10 = Hades alone**.
3. **Over-level clamp:** killer >2 levels above an elite's pin → guaranteed bag downgrades
   (anti-farm), built into `DungeonElite`.
4. **Rarity ceilings:** bag 5–6 → Epic, bag 7–9 → Legendary, bag 10 → guaranteed Legendary.
5. **Divine Resonance** echoes duplicate legendaries (no instance caps).

Future layers to consider (Outlands, not yet ours): Paragon overlay, participation-scaled raid
bosses with threshold looting rights, progressive per-hour loot, weekly region/dungeon rotation.
