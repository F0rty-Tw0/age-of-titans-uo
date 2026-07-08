# Itemization — Age of Titans (Greek Mythos)

Design-doc set for the shard's full dropped-item economy. **`00-framework.md` is law** — budgets,
pantheon roots, effect primitives (P1–P27), family ladders, distribution, balance rules all live
there; family docs only instantiate it. `01-axes.md` is the canonical format every family doc
mirrors (it is the user-approved axe spec re-themed Greek, numbers unchanged).

Crafting is deferred by scope decision (framework §11). Implementation is phased (framework §12):
effect engine → data tables → loot roller → hue pass. Nothing ships until the primitive engine
exists.

## Files

| File | Family | Bases/pieces | Legendaries |
|---|---|---:|---:|
| `00-framework.md` | system master | — | — |
| `01-axes.md` | axes (Swordsmanship) | 8 | 40 |
| `02-swords.md` | blades (Swordsmanship) | 8 | 40 |
| `03-polearms.md` | bardiche/halberd | 2 | 10 |
| `04-maces.md` | maces incl. war axe | 7 | 35 |
| `05-staves.md` | staves (caster hybrid) | 3 | 15 |
| `06-fencing.md` | fencing | 6 | 30 |
| `07-archery.md` | bows | 3 | 15 |
| `10-armor-metal.md` | ring/chain/plate | 3 materials | 15 |
| `11-armor-light.md` | leather/studded/bone | 3 materials | 15 |
| `12-shields.md` | shields | 6 | 30 |
| `20-jewelry.md` | ring/bracelet/necklace/earrings | 4 slots | 20 |
| `21-clothing.md` | bonus-bearing cloth | 12 shapes | 5 relics |
| | | **total** | **270** |

**Name-uniqueness guard:** all 270 legendary proper nouns are globally unique (verified
2026-07-07 by parsing every `| **Name** |` table row across the set — zero duplicates). **Roots
live in the same namespace**: the 64 re-theme roots were grep-verified against all legendary
names + legacy roots (2026-07-07, zero exact collisions). Before adding any new named item OR
root, grep this directory for the name first. Accepted near-misses (signed off): Phoibos~Phoibe,
Adamas~Adamastos, Akamatos~Akamas, Hekatos~Hecatean, Eryma~Erymanthos, Aristeia~Aristaios,
Phylax~Phylakos — all cross-family, none the same myth figure.

## The root system (one enchantment, many shapes — per family)

Roots are unique per weapon family / armor material / shields (2026-07-07 directive — framework
§3 holds the full registry with lane mechanics and Epic signatures). Legacy shared armor roots
(Polias/Cyclopean/Paean/Tritonian/Talarian) are retired: enum-decode-only, remapped on load.

| Family | Roots |
|---|---|
| Axes (unchanged) | Zephyr (speed) · Phobos (damage) · Agrotera (mark) · Pallas (defense) · Stygian (drain) |
| Swords | Phoibos (precision) · Areia (riposte) · Menis (wrath-ramp) · Aristeia (glory) · Haima (bleed) |
| Maces | Ennosigaios (quake) · Kataigis (concussion) · Rhaistes (sunder) · Eryma (anvil) · Kamatos (exhaust) |
| Polearms | Theristes (reap) · Sarisa (impale) · Phalanx (hold) · Horme (momentum) · Zophos (toll) |
| Staves | Empousa (siphon) · Prester (storm) · Alexikakos (ward) · Manteia (oracle) · Baskania (curse) |
| Fencing | Ios (venom) · Ephodos (lunge) · Aiolos (flurry) · Kentron (puncture) · Ophis (evasion) |
| Archery | Hekatos (deadeye) · Belos (volley) · Pede (pin) · Toxikon (toxin) · Skopos (warden) |
| Leather | Naias · Dryas · Oreias · Melissa · Panika |
| Studded | Kynegis · Batos · Arkas · Elaphis · Skia |
| Bone | Melinoe · Makaria · Tymbos · Nekyia · Katachthon |
| Ring | Hoplites · Taxis · Dromos · Zoster · Alkimos |
| Chain | Phylax · Egregoros · Teichos · Halysis · Phrourion |
| Plate | Adamas · Kaminos · Kolossos · Panoplia · Akamatos |
| Shields | Aegis (kept) · Amyntor · Probolos · Herkos · Pnoe |
| Jewelry (unchanged) | Olympian (Zeus) · Hecatean (Hecate) · Tychean (Tyche) · Nyxian (Nyx) · Demetrian (Demeter) |
| Clothing (unchanged) | Laurel (Nike) · Charis (Aphrodite) · Maenad (Dionysos) · Hestian (Hestia) · Arachne |

Legendary namespace lanes (collision prevention for future additions): axes = winds/harpies +
war-daimones + Artemis' circle + Athena epithets + underworld rivers · swords = Trojan heroes +
Perseid cycle · polearms = Gigantes · maces = Cyclopes/Hecatoncheires/forge · staves = seers ·
fencing = spear-heroes + serpents · archery = archers/arrow-myths · metal armor = city-kings +
forged guardians · light armor = beast-hides + nymphs · shields = shield-bearers/bulwarks ·
jewelry = celestials/primordials · clothing = Fates/weavers.

## Merge-pass decisions log (2026-07-07)

Cross-family name collisions were resolved by "tightest myth fit keeps the name"; each affected
doc carries a local note. Renames applied:

- **Swords** (11): Automedon→Xanthos, Penthesileia→Neoptolemos, Teukros→Kephalos,
  Pandaros→Peirithoos, Philoktetes→Melanion, Podaleirios→Menestheus, Machaon→Eurypylos,
  Glaukos→Deiphobos, Patroklos→Iphitos, Akrisios→Agamemnon, Proitos→Aigisthos.
- **Fencing** (11): Automedon→Balios, Antilochos→Kyknos, Meriones→Asteropaios, Idomeneus→Akamas,
  Sarpedon→Asios, Diomedes→Kolchis, Amphiaraos→Parthenopaios, Echidna→Lamia, Skylla→Sybaris,
  Chimaira→Ekhion, Cetus→Ketos.
- **Shields** (5): Kekrops→Asklepios, Erechtheus→Boutes, Sthenelos→Palaimon, Kaineus→Abderos,
  Ladon→Myrtilos.
- **Maces** (2): Typhon→Thyella, Enkelados→Kabeiros. **Jewelry** (2): Chaos→Khaos, Hemera→Moros.
  **Archery** (1): Hydra→Lerna. **Metal armor** (1): Polyidos→Iapyx.

Balance retunes at merge: metal armor **Podaleirios** and shields **Asklepios** on-kill heals
trimmed from full HP to 50%-missing / 25%-max respectively (they were the only two full-heal
clauses in the set). Framework updates: Legendary base-package rule codified (§4), heal-block
gating clarified (§9.3), numeric shared defensive pools added (§9.8).

**Known accepted quirks:** `01-axes.md` repeats identical clauses up to 5× within a theme line —
sanctioned source-spec fidelity (the approved axe spec did the same); all other families hold a
≤3 repeat cap. Transliteration is tolerant (Achilles vs Kekrops-style Greek) as long as no two
names refer to the same figure.

## Balance watch list (tune live)

1. Hue numbers everywhere are placeholders — in-client eyeball pass required (framework §3 ramp).
2. Drop-weight table (framework §8) unvalidated against real farm rates.
3. Archery's −16% ranged tax and maces' crush numbers (+25% armor durability dmg, −2 stam per
   hit) are first-guess values.
4. Clothing relic **Lachesis** (+15 Animal Taming) is the strongest skill item in the set.
5. Offense scales ~11× Common→Legendary, armor ~2.2× — intentional (framework pillar 4); expect
   fast TTK at top-end PvP and tune stuns/dodge pools first, not AR.
