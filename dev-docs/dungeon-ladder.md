# Dungeon Ladder — Five Domains (design v1, 2026-07-14)

> **IMPLEMENTED 2026-07-14** (uncommitted): all 36 mob classes under
> `Projects/UOContent/Mobiles/Dungeons/<Dungeon>/`, `DungeonElite` + `DungeonAbilities`
> shared bases, 10 `ThemedDungeonRegion` entries in regions.json (PLACEHOLDER coords
> x6700-6850 / y900-1350 — GM re-points later), 5 spawn JSONs under
> `Distribution/Data/Spawns/uoml/felucca/`, 36 LevelConfig pins, `[GoDungeon <name>`
> GM teleport. Build clean, migrations generated. Post-review fixes: TideHerald walks on
> land (kraken donor was CantWalk), WyldPanther aggressive (donor was tame-passive).
> Elite/boss HP-vs-party tuning is UNVALIDATED — live pass needed, same as the barrow.

Level-tiered follow-up to the Barrow of the Unremembered (L0–3, `dev-docs/newbie-dungeon.md`).
Players graduate the barrow at L4 and climb this ladder to L10. Consumer of existing systems
only (Leveling gap-curve, Rarity loot bags, PantheonFx, `DungeonElite` force-drop idiom).
Owns no new engines. All mobs are hue-reskins of stock T2A bodies with one signature ability
+ at most one passive each.

Classic-dungeon enhancements (Despise/Deceit/Shame/Destard/Hythloth) live in
`dev-docs/classic-five-enhancements.md`.

## Overview

| # | Dungeon | Domain / God | Level band | Trash bag | Elite bag (forced) | Boss bag | Music | Light |
|---|---|---|---|---|---|---|---|---|
| 1 | The Drowned Tholos | Sea / Poseidon | L4–5 | bag3 (L4) / bag4 (L5) | bag5 | 2×bag5 | Cave01 → Approach | 14 → 18 |
| 2 | The Cinderworks | Forge / Hephaestus | L5–6 | bag4 (L5) / bag5 (L6) | bag6 | 2×bag6 | Dungeon2 → Dungeon9 | 20 → 16 (lava glow) |
| 3 | The Nemean Wildwood | Hunt / Artemis | L6–7 | bag5 (L6) / bag6 (L7) | bag7 | 2×bag7 | Forest_a → Jungle_a | 16 → 12 |
| 4 | The Stormcrown Aerie | Sky / Zeus | L8–9 | bag7 (L8) / bag8 (L9) | bag9 | 2×bag8 | Mountn_a → Dungeon9 | 8 → 12 (above the clouds) |
| 5 | The Stygian Deep | Underworld / Hades | L9–10 | bag8 (L9) / bag9 (L10) | bag9 | **bag10 + bag9** | Dungeon9 → Death | 26 → 28 (blackest) |

(Light scale: higher = darker, 0 = daylight — the v1 draft had two rows inverted; music
limited to real `MusicName` values. Regions shipped in `Distribution/Data/regions.json`,
two `ThemedDungeonRegion` zones per dungeon at placeholder coords x6700-6850 /
y900-1350.)

Bag convention inherited from the barrow (L1 mob → bag0): **trash bag = mob level − 1**
(`LootBagLevel` override per class). Elites force a guaranteed bag one tier above the
dungeon's top trash. Boss = two bags (bag level per table). **Bag 10 = GUARANTEED
Legendary** (user directive 2026-07-14; bag 9 took bag 10's old 90/10 Epic/Legendary
split) **and drops from the Hades boss and nowhere else** — the Sky boss is deliberately
held at bag9 so Hades stays the only bag-10 faucet. Hades drops 1×bag10 + 1×bag9 (not
2×bag10 — two guaranteed Legendaries per respawn is too hot). Legendaries are NOT
one-per-shard: "globally unique" means unique names/IDs, dupes drop and Divine Resonance
echoes them.

Entry flash: DarkFlash on descent into Sea and Hades (submersion / grave), LightFlash into
Forge, Hunt, Sky (glow / moonburst / lightning). Per-depth light ramp via region
`AlterLightLevel`, music via `Region.Music` (engine auto-swaps), same as barrow §6.2.

---

## 1. The Drowned Tholos — Sea / Poseidon (L4–5)

A colonnaded sea-temple that Poseidon's wrath dragged beneath the waves. Air-pockets and
green-lit flooded halls where the drowned garrison of a sunken city still walks its posts,
barnacle-crusted and furious that the living breathe. Teaches sustained pressure, cold/poison
management, and holding position against knockback near the water's edge.

| Class | Display | Donor body | Hue | Lvl | HP | Dmg | AI | Signature | Passive |
|---|---|---|---|---|---|---|---|---|---|
| `TideDrudge` | a drowned oarsman | Zombie (3) | drowned teal ~0x0847 | 4 | 120–160 | 6–9 | Melee | **Undertow** — on melee hit 20%: drain 12 stamina | BleedImmune |
| `TideBrinescale` | a brine serpent | Sea Serpent (150) | pale aqua ~0x0481 | 4 | 100–140 | 5–8 | Melee (VeryFast) | HitPoison Regular ("Venom of the Deep") | — |
| `TideHoplite` | a barnacled hoplite | Skeletal Knight (57) | verdigris bronze ~0x08A5 | 5 | 300–360 | 10–14 | Melee | **Phalanx** — pack instinct: +dmg while another hoplite adjacent | — |
| `TideMaw` | a whirling maw | Water Elemental (16) | deep blue ~0x0530 | 5 | 280–340 | 9–13 | Melee | **Riptide** — on hit 25%: knockback msg + 15 stam drain | *aura* Chilling Wake: 2 cold/tick to adjacent |
| `TideSentinel` | a coral sentinel | Water Elemental (16) | Poseidon blue ~0x0532 | 5 | 320–380 | 10–14 | Melee | HasBreath Cold ("Tidal Breath") | PoisonImmune |
| `TideWarden` *(elite)* | Kymopoleia, the Reefbound | Sea Serpent (150) | reef green-gold ~0x0851 | 5 | 360–380 | 12–16 | **Mage** | **Maelstrom Call** — OnGotMelee 15%: spawn 1 `TideBrinescale` (cap 2, dispel-vulnerable) | — |
| `TideHerald` *(BOSS)* | Aigaion, the Drowned Herald | Kraken (77) | abyssal ~0x0850 | 5 | 370–380 | 14–18 | Melee | **Sovereign's Undertow** — OnGaveMelee 30%: 20 stam drain + knockback; HasBreath Cold ("Deluge") | PoisonImmune + BleedImmune |

**Anti-abuse:** gap curve zeroes XP for L7+ visitors; guaranteed-bag clamp (see Shared
Systems). Boss guard: two `TideSentinel` patrol the flooded apse while Aigaion is despawned
(custodian). Respawn: elite 15 min, boss 30–45 min.

---

## 2. The Cinderworks — Forge / Hephaestus (L5–6)

Hephaestus's abandoned undermountain forge, its bellows still breathing without a smith.
Automaton laborers and slag-things toil in the heat, mistaking intruders for raw material to
be smelted. Lava-lit and lethal; teaches fire-resist gearing and bursting things down before
they get worse.

| Class | Display | Donor body | Hue | Lvl | HP | Dmg | AI | Signature | Passive |
|---|---|---|---|---|---|---|---|---|---|
| `CinderThrall` | a slag thrall | Ettin (152) | ash grey ~0x0964 | 5 | 300–350 | 10–14 | Melee | **Emberbrand** — on hit 20%: 12 mana drain | PoisonImmune |
| `CinderImp` | a forge imp | Imp (74) | flame ~0x0655 | 5 | 260–320 | 8–12 | **Mage** | HasBreath Fire ("Bellows Breath") | — |
| `CinderGargoyle` | a slag gargoyle | Gargoyle (4) | basalt ~0x0966 | 6 | 460–520 | 12–16 | Melee | **Molten Riposte** — OnGotMelee 20%: reflect 25% of hit as fire | — |
| `CinderAutomaton` | a bronze automaton | Golem (752) | bronze ~0x0798 | 6 | 500–550 | 13–17 | Melee (slow) | **Overheat** — *aura* below 30% HP: 3 fire/tick to adjacent (comeback) | Bleed+Poison immune (construct) |
| `CinderSentinel` | a forge sentinel | Fire Elemental (15) | Hephaestus orange ~0x0654 | 6 | 500–550 | 13–17 | Melee | HasBreath Fire ("Forge Breath") | PoisonImmune |
| `CinderCyclops` *(elite)* | Brontes, the Last Cyclops | Cyclops (75) | soot + ember ~0x0967 | 6 | 540–550 | 15–19 | Melee | **Hammerfall** — OnGaveMelee 25%: knockback + 18 stam drain | BleedImmune |
| `CinderHeart` *(BOSS)* | Kelmion, the Bellows-Heart | Daemon (9) | molten core ~0x0669 | 6 | 545–550 | 16–20 | **Mage** | **Reforge** — OnGotMelee 12%: self-heal leech 8% max HP; HasBreath Fire ("Pyroclasm") | PoisonImmune |

**Anti-abuse:** clamp. `CinderAutomaton`'s Overheat is the the reference shard comeback lever —
over-cautious players who chip it slowly get punished, rewarding commitment. Boss guard: two
`CinderSentinel`. Respawn: elite 15, boss 30–45.

---

## 3. The Nemean Wildwood — Hunt / Artemis (L6–7)

A moon-silvered old-growth thicket where Artemis's hunt never ends — but the quarry has
turned. Beasts wear the goddess's silver mark and stalk in coordinated packs; the huntresses
who trespassed long ago now run half-feral among them. Teaches pack control, heavy poison
discipline, and what it feels like to be the hunted.

| Class | Display | Donor body | Hue | Lvl | HP | Dmg | AI | Signature | Passive |
|---|---|---|---|---|---|---|---|---|---|
| `WyldHound` | a moonlit hound | Hellhound (98) | silver-grey ~0x0483 | 6 | 460–520 | 12–16 | Melee (Fast) | **Pack Hunt** — pack instinct (+dmg in packs) | — |
| `WyldStalker` | a thornclad stalker | Ratman (42) | mossy ~0x0844 | 6 | 440–500 | 11–15 | **Mage** | HitPoison Greater ("Nightshade Arrow") | — |
| `WyldPanther` | a bronze-maned panther | Panther | bronze ~0x0798 | 7 | 620–700 | 15–19 | Melee (VeryFast) | **Pounce** — OnGotMelee 15%: teleport-blink behind attacker | BleedImmune |
| `WyldPython` | a sacred python | Giant Serpent | emerald ~0x0851 | 7 | 600–680 | 14–18 | Melee | HitPoison Deadly ("Artemis's Bane") | PoisonImmune |
| `WyldSentinel` | a grove sentinel | Earth Elemental (14) | mossy green ~0x0851 | 7 | 640–720 | 15–19 | Melee | HasBreath Poison ("Pollen Breath") | PoisonImmune |
| `WyldMatriarch` *(elite)* | Atalanta, the Unbowed | Ophidian Matriarch (87) | silver ~0x0486 | 7 | 700–720 | 16–20 | **Mage** | **Coordinated Volley** — OnGotMelee 15%: spawn 1 `WyldHound` (cap 2, dispel-vulnerable) | — |
| `WyldStag` *(BOSS)* | Elaphos, the Golden-Horned | Great Hart (0xEA) | golden ~0x0501 | 7 | 710–720 | 17–21 | Melee (VeryFast) | **Endless Hunt** — OnGaveMelee 25%: "Marked Quarry" msg + 15 stam drain | BleedImmune + PoisonImmune |

**Anti-abuse:** clamp. Kill Atalanta first or the hound adds never stop — the "kill the
alpha" lesson (elite is a caster/summoner, not a bigger sack of HP: the the reference shard
"harder = different role" rule). Boss guard: two `WyldHound`. Respawn: elite 15, boss 45–60.

---

## 4. The Stormcrown Aerie — Sky / Zeus (L8–9)

A cyclopean mountain-peak fortress above the clouds, where the defeated Titans were chained —
and the storm set to guard them has grown teeth. Lightning-wreathed giants and living
tempests patrol the shattered ramparts; the air itself strikes. This is the gear wall before
the descent: teaches energy-resist gearing, burst, and surviving aura spike on big
single-target fights.

| Class | Display | Donor body | Hue | Lvl | HP | Dmg | AI | Signature | Passive |
|---|---|---|---|---|---|---|---|---|---|
| `StormHarpy` | a tempest harpy | Harpy (30) | storm violet ~0x0491 | 8 | 800–900 | 16–20 | Melee (Fast) | **Gale Buffet** — OnGaveMelee 20%: knockback + 15 stam drain; pack instinct | — |
| `StormWisp` | a charged wisp | Wisp (58) | electric blue ~0x0480 | 8 | 780–880 | 15–19 | **Mage** | HasBreath Energy ("Static Breath") | — |
| `StormTitan` | a chained titan | Cyclops (75) | storm grey ~0x0492 | 9 | 2200–2400 | 20–26 | Melee (slow) | **Thunderclap** — *aura* 4 energy/tick to adjacent | BleedImmune |
| `StormDrake` | a thunder drake | Drake (60) | crackling blue ~0x0480 | 9 | 2100–2350 | 19–24 | Melee | HasBreath Energy ("Levin Breath") | high energy resist |
| `StormSentinel` | a storm sentinel | Air Elemental (13) | Zeus white-gold ~0x0481 | 9 | 2200–2400 | 20–25 | Melee | HasBreath Energy ("Zeus's Breath") | PoisonImmune |
| `StormChained` *(elite)* | Enceladus, the Chainbreaker | Cyclops (75) | lightning-scarred ~0x0492 | 9 | 2350–2400 | 22–28 | Melee | **Titan's Wrath** — OnGotMelee 15%: reflect 30% as energy + knockback | BleedImmune |
| `StormFather` *(BOSS)* | Typhon, the Hundred-Storm | Dragon (12) | thunderhead ~0x0492 | 9 | 2380–2400 | 24–30 | **Mage** | **Hundred Hands** — OnGotMelee 12%: spawn 1 `StormWisp` (cap 2); HasBreath Energy ("Cataclysm") | PoisonImmune + BleedImmune |

**Anti-abuse:** clamp. The L7→L8 HP jump (Hunt ~720 → Storm 800+/2400) is the intended
progression wall — you cannot walk in undergeared from the Wildwood. Boss held at bag9 to
reserve bag10 for Hades. Boss guard: two `StormSentinel`. Respawn: elite 15, boss 45–60.

---

## 5. The Stygian Deep — Underworld / Hades (L9–10)

**The payoff of the barrow's sealed gate.** The returning hero descends past the very door
the level-0 player saw chained shut — katabasis completed, full circle. Beyond it: the five
rivers of the dead, the asphodel wastes, and Hades's own throne. The unransomed dead here
are ancient and mighty, and the deeper you go the closer to the crown. This is the
graduation exam — every earlier lesson at once.

| Class | Display | Donor body | Hue | Lvl | HP | Dmg | AI | Signature | Passive |
|---|---|---|---|---|---|---|---|---|---|
| `StygianShade` | an unransomed shade | Spectre (26) | void black ~0x0455 | 9 | 2100–2350 | 19–24 | **Mage** | **Grave Chill** — OnGaveMelee 20%: 15 mana drain | PoisonImmune |
| `StygianHound` | a hound of Dis | Hellhound (98) | hellfire black ~0x0453 | 9 | 2000–2300 | 18–23 | Melee (VeryFast) | HasBreath Fire ("Hellhound Breath"); pack instinct | — |
| `StygianReaper` | a soul-reaper | Lich (24) | bone-violet ~0x0454 | 10 | 2500–2800 | 22–28 | **Mage** | HitPoison Lethal ("Stygian Rot") + OnGaveMelee 15%: self-heal 8% | — |
| `StygianBoneLord` | a bronze bone lord | Bone Knight (57) | underworld bronze ~0x08A5 | 10 | 2600–2900 | 23–29 | Melee | **Deathless Ranks** — OnGotMelee 12%: spawn 1 `StygianShade` (cap 2); pack instinct | BleedImmune |
| `StygianSentinel` | a stygian sentinel | Fire Elemental (15) | Hades violet-black ~0x0454 | 10 | 2600–2900 | 22–28 | Melee | HasBreath Cold ("Cocytus Breath") | PoisonImmune |
| `StygianCharon` *(elite 1)* | Charon, Ferryman of the Deep | Headless One (31) | deep shadow ~0x0455 | 10 | 2800–3000 | 24–30 | **Mage** | **Ferryman's Toll** — OnGaveMelee 25%: 20 mana drain + knockback | PoisonImmune |
| `StygianCerberus` *(elite 2)* | Cerberus, Warden of the Gate | Hellhound (98) | black-fire ~0x0453 | 10 | 2900–3100 | 25–31 | Melee | **Threefold Maw** — OnGotMelee 15%: spawn 1 `StygianHound` (cap 2); HasBreath Fire | BleedImmune + PoisonImmune |
| `StygianLord` *(BOSS)* | Hades, Lord of the Unseen | Daemon (9) | imperial black-gold ~0x0489 | 10 | 3200–3600 | 26–34 | **Mage** | **Sovereign of the Dead** — OnGotMelee 12%: self-heal 10% + spawn 1 `StygianShade` (cap 3); HasBreath Cold ("Breath of the Grave") | PoisonImmune + BleedImmune |

**Callbacks (full-circle payoff):** Charon reuses the barrow `NewbieCharon`'s deep-shadow hue
(~0x0455) and Headless-One body; Cerberus is the grown "Warden of the Gate" from the barrow's
`NewbieHollowWarden`. The barrow foreshadowed Hermes/Ares/Hades via its three elites — this
dungeon cashes the Hades promise.

**Anti-abuse:** clamp (vacuous at L10 — nobody out-levels it). Hades is the game's only
bag-10 source. Two elites double as the boss's custodian guard while Hades is despawned.
Respawn: elites 15, boss 60 min.

---

## Shared Systems

### Signature variant — the Pantheon Sentinel line (the reference shard recolor trick)
One archetype, five recolors, zero new combat math. Base class `PantheonSentinel :
BaseCreature` holds the shared skeleton (elemental body, melee AI, PoisonImmune, mid-band HP
for its dungeon). Each dungeon subclass sets only **hue + breath type + level/HP band**.
Each Sentinel doubles as its dungeon's **boss custodian** (patrols the boss room while the
boss is on respawn), so the class earns its keep twice.

### Elite/boss base — `DungeonElite` (BUILT)
`Projects/UOContent/Mobiles/Dungeons/DungeonElite.cs`: `DropsLootBag => false`, abstract
`EliteBagLevel`, virtual `EliteBagCount` (bosses set 2), force-drop + corpse sparkle/shimmer
telegraph in `OnDeath`, **anti-farm clamp built in** (killer more than 2 levels above the
elite's pinned level → guaranteed drop downgrades to the global trash-style roll; last-hit
rule, tune later if it stings). `NewbieElite` now subclasses it (bag 2).

### Region — `ThemedDungeonRegion` (BUILT)
Mood-only: `LightLevel`, `EntryFlash`, music via base JSON prop. No gating, no PvP rules —
open world. Registered in `RegionJsonRegistration`.

### Spawner counts & respawns
- **Trash:** 8–14 per sub-tier zone, 1–3 min respawn. Two sub-tier zones per dungeon.
- **Sentinels:** 2–3 per dungeon, ~5 min; 2 placed as boss-room custodians.
- **Elites:** 1–2 spawn points, ~15 min (contested shared spawn, no instancing).
- **Boss:** 1 spawn point, 30–60 min (Sea/Forge 30–45, Hunt/Sky 45–60, Hades 60).

### Naming conventions
Class prefix per dungeon: `Tide*`, `Cinder*`, `Wyld*`, `Storm*`, `Stygian*` (barrow
`Newbie*` idiom). Trash = UO-lowercase display ("a barnacled hoplite"). Named elites/bosses
get `Name` + `Title`. Folder: `Projects/UOContent/Mobiles/Dungeons/<Dungeon>/` per dungeon.

### the reference shard patterns stolen (labelled)
1. **Recolor signature variant** — the Pantheon Sentinel line.
2. **Role-based resist tiers** — casters carry immunities/resists, melee stay low; by role
   not rank.
3. **Custodian boss-guard** — Sentinels patrol the boss room during respawn.
4. **Comeback mechanic** — `CinderAutomaton` Overheat aura below 30% HP.
5. **"Harder = different role, not bigger numbers"** — elites are summoners/casters/CC.
6. **Designed-in abuse counter** — add-spawns capped + dispel-vulnerable; guaranteed-bag
   clamp ships with the reward.

---

## Implementation-Cost Appendix

**Total: 36 mob classes** + shared bases (`DungeonElite` ✅ built, `PantheonSentinel`,
one aura-timer helper, one capped-add-spawn helper). Sea 7, Forge 7, Hunt 7, Sky 7,
Underworld 8.

- **Pure stat blocks** (existing overrides only — HitPoison, HasBreath, pack instinct,
  speed, immunities, AI): ~14 classes.
- **Aura via slow timer** (shared helper): `TideMaw`, `CinderAutomaton`, `StormTitan`.
- **Custom `OnGaveMeleeAttack`** (stam/mana drain, knockback msg, self-heal): ~9 classes.
- **Custom `OnGotMeleeAttack`** (reflect, capped add-spawn, blink, self-heal): ~10 classes.

**Impl caveats:** donor body IDs are suggestions — copy the donor mob's source file for
proven body/sound values. Level pins go in `LevelConfig.MobLevelOverrides`; HP values sit
inside each level's band by design (L4 ≤240, L5 ≤380, L6 ≤550, L7 ≤720, L8 ≤950, L9 ≤2400,
L10 above). Trash classes override `LootBagLevel => <mob level − 1>`. Placeholder region
coordinates like the barrow — user GM-places later. Music names must exist in `MusicName`
(verify Boss/StygianAbyss tracks; fall back to Death/Dungeon2 if missing on T2A client).
