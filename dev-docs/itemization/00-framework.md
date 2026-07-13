# Itemization Framework — Age of Titans (Greek Mythos)

**Status:** Design master. Every family doc (`01-*.md` …) derives from this file. Numbers here are
the single source of truth — family docs may not invent ratios, budgets, or effect magnitudes.
**Scope:** Dropped items only. Crafting deferred (see §11). Implementation deferred (see §12).
**Era:** T2A custom shard. Stock-era purity is NOT a constraint for named variants; base item
shapes stay T2A-recognizable (one deliberate exception: ornate axe, kept per approved axe spec).

---

## 1. Design pillars

1. **One enchantment, many shapes — within a family.** A named variant (e.g. *Phobos*) is the
   same effect package on every base in its family — only the damage/AR number changes, driven
   by the base's ratio. Learnable: players read "phobos" and know what it does before they read
   the tooltip. Families do NOT share roots (2026-07-07 directive): each weapon family, each
   armor material, and shields carry their own five — a root is also a statement of what you're
   holding.
2. **Clean linear ladders.** Bases inside a family climb weakest→strongest in even ratio/speed
   steps. No ties, no reversals (approved departure from real-UO stats — axe spec, 2026-07-07).
3. **Legendaries are individuals.** Every base × theme combination at Legendary gets a unique
   proper-noun name from Greek myth and one unique bonus clause. Names never repeat, anywhere.
4. **Offense scales steep, defense scales gentle.** Weapon damage grows ~3× Common→Legendary
   (softened `D` curve, §2 — 2026-07-11); armor AR grows only ~2.2×. High-tier fights stay lethal; mixed-tier
   PvP stays winnable. Deliberate asymmetry — do not "fix" it by inflating AR.
5. **Effects compose from the primitive catalog (§5) only.** If an effect isn't in the catalog,
   it doesn't go on an item — extend the catalog first (one engine hook, many items).

## 2. Rarity ladder

Engine: `Projects/UOContent/Engines/Rarity/` — `ItemRarity` (Common..Legendary),
`IRarity` on `BaseWeapon`/`BaseArmor`/`BaseClothing`/`BaseJewel`, name suffix + OPL line wired,
`RarityConfig.MaxRarityForBagLevel` ceilings coded. (Confirmed: files read 2026-07-07.)

| Rarity | Item level | Budget | Damage anchor `D` | Armor anchor `A` | Shield anchor `S` |
|---|---:|---:|---:|---:|---:|
| Common | 1 | 10 | 10 | 12 | 8 |
| Uncommon | 2 | 12 | 13 | 14 | 10 |
| Rare | 4 | 16 | 17 | 17 | 12 |
| Epic | 6 | 21 | 22 | 21 | 15 |
| Legendary | 10 | 37 | 30 | 26 | 20 |

- **Damage anchor `D` (2026-07-11 directive):** softened curve `(10, 13, 17, 22, 30)` —
  supersedes the original 11× anchors `(10, 15.6, 27.2, 46.2, 111)`; base ratios and the ladder
  structure (§7) are unchanged.
- **Weapon damage** = `base ratio × D[rarity]`, round to 1 decimal (verified: battle axe
  0.75×17 = 12.75 ✓). Live items apply this at runtime — nothing is serialized, so retuning `D`
  or a ratio retro-adjusts every dropped variant weapon. Combat rolls the anchor ±10% (min ≈0.9×,
  max ≈1.1×, average on the anchor).
- **DPS** = damage / swing seconds (base, before speed effects).
- **Armor piece AR** = `material ratio × slot weight × A[rarity]`, round to integer.
- **Shield AR** = `shield ratio × S[rarity]`.
- Full 10-step budget curve (future crafted tiers / boss items): 10/12/14/16/18/21/24/28/32/37.
  Dropped rarities anchor at levels 1/2/4/6/10 only.
- **Item level is drop-pacing metadata only — no wear gate.** (Recommended; tunable. Hard level
  gates feel non-T2A; power is gated by drop distribution instead, §8.)

## 3. Pantheon registry — themes, roots, hues

Five roots per **weapon family**, per **armor material**, and for **shields** (user directive
2026-07-07: themes are unique per item type, including their Epic and Legendary powers — no root
appears on two families). The **root** is the drop-variant name ("rare phoibos longsword").
Roots are globally unique across every category AND may not duplicate any legendary proper noun —
grep this directory before adding one. Rarity display hues (labels/announce) come from
`RarityConfig`; item body hues follow the hue ramp rule below. Body-hue runs are placeholders
until the in-client hue pass (§12.4) — interim, each root borrows the legacy run closest to its
lane's vibe.

Legacy shared roots retired 2026-07-07: Polias / Cyclopean / Paean / Tritonian / Talarian remain
in the `VariantRoot` enum for old-save decode only and are remapped on load.

### Weapons — five roots per family

**Axes** (exemplar — the original approved spec; gods and mechanics unchanged):

| Theme | Myth | Root | Mechanical identity |
|---|---|---|---|
| Speed | Hermes | **Zephyr** | swing speed, hit chance, extra swings |
| Damage | Ares | **Phobos** | flat damage, crit chance/damage |
| Mark | Artemis | **Agrotera** | Huntress' Mark debuff (+dmg taken), poison |
| Defense | Athena | **Pallas** | parry/block, damage reduction on block, thorns |
| Drain | Hades | **Stygian** | lifesteal, stamina leech/regen, execute synergy |

**Swords — "the hero's duel"**

| Theme | Myth | Root | Mechanical identity | Epic signature |
|---|---|---|---|---|
| Precision | Apollo the radiant | **Phoibos** | hit chance + crit chance | first hit of each fight always crits |
| Riposte | Athena Areia | **Areia** | block + DR on block | a block guarantees next-hit crit |
| Wrath | the Wrath of Achilles | **Menis** | stacking dmg per consecutive hit, one target (P28) | burst splash at max stacks |
| Glory | aristeia, battle-glory | **Aristeia** | on-kill restores (P23) | on-kill: full stam + brief crit window |
| Bleed | haima, spilt blood | **Haima** | "gash" poison tick on crit + small lifesteal | bleeding targets take +dmg from wielder |

**Maces — "storm & anvil"**

| Theme | Myth | Root | Mechanical identity | Epic signature |
|---|---|---|---|---|
| Quake | Poseidon Ennosigaios | **Ennosigaios** | splash damage (P26) | every 5th hit fires a full-power splash burst |
| Concussion | kataigis, the tempest | **Kataigis** | stagger procs (P13, §9.2 caps) | crits stagger |
| Sunder | rhaistes, the smasher | **Rhaistes** | armor pen (P27) + durability dmg (P22) | every 4th hit ignores AR |
| Anvil | eryma, the bulwark | **Eryma** | block + DR + thorns (P11/P12) | after a block, next swing +dmg |
| Exhaust | kamatos, toil | **Kamatos** | stam leech + attacker stam drain (P7) | bonus dmg vs low-stam targets |

**Polearms — "the reaping line"**

| Theme | Myth | Root | Mechanical identity | Epic signature |
|---|---|---|---|---|
| Reap | theristes, the reaper | **Theristes** | cleave splash (P26) | splash on every crit |
| Impale | the sarisa pike | **Sarisa** | armor pen (P27) + crit dmg | first hit of fight: guaranteed crit w/ pen |
| Hold | the phalanx line | **Phalanx** | block + thorns (P11/P12) | block grants brief DR |
| Momentum | Horme, daimon of onslaught | **Horme** | every-Nth-hit escalating dmg (P15) | extra swing at cadence |
| Toll | zophos, nether-gloom | **Zophos** | lifesteal + on-kill restore (P6/P23) | lifesteal ×2 vs low-HP targets |

**Staves — "the seer's rod"**

| Theme | Myth | Root | Mechanical identity | Epic signature |
|---|---|---|---|---|
| Siphon | Empousa, the drainer | **Empousa** | mana leech (P8) | crit = mana burst leech |
| Storm | prester, fire-wind | **Prester** | elemental proc (P24) | proc chance up, alternates fire/lightning |
| Ward | Apollo Alexikakos | **Alexikakos** | spell DR + Resist bonus (P20/P19) | first spell each fight heavily DR'd |
| Oracle | manteia, prophecy | **Manteia** | mana regen + skill bonus (P17/P19) | heals-received + auto-cure tick |
| Curse | baskania, the evil eye | **Baskania** | heal-block procs (P25, §9.3 gated) | crits heal-block |

**Fencing — "serpent's tempo"**

| Theme | Myth | Root | Mechanical identity | Epic signature |
|---|---|---|---|---|
| Venom | ios, venom | **Ios** | poison apply chance (P10) | poison tier up |
| Lunge | ephodos, the assault | **Ephodos** | first-hit-of-fight bonuses (P14) | first hit guaranteed crit + stam refund |
| Flurry | Aiolos, lord of winds | **Aiolos** | swing speed + extra swing (P1/P5) | extra-swing chain proc |
| Puncture | kentron, the sting | **Kentron** | armor pen + hit chance (P27/P3) | every 3rd hit ignores AR |
| Evasion | ophis, the serpent | **Ophis** | dodge/parry (worn-side) | dodge opens counter window |

**Archery — "the far mark"**

| Theme | Myth | Root | Mechanical identity | Epic signature |
|---|---|---|---|---|
| Deadeye | Apollo Hekatos | **Hekatos** | crit chance + crit dmg (P4) | first shot of fight always crits |
| Volley | belos, the missile | **Belos** | splash (P26) | every 4th shot splashes |
| Pin | pede, the fetter | **Pede** | stagger proc (P13, §9.2 caps) | crits pin |
| Toxin | toxikon, arrow-poison | **Toxikon** | poison apply chance (P10) | poisoned targets are marked |
| Warden | skopos, the watcher | **Skopos** | hit chance + defensive block (P3/P11) | block grants next-shot crit |

### Armor — five roots per material

Lanes remix the armor primitive pool (P11/P12/P16/P17/P19/P20/P21/P22/P24 + dodge); distinctness
comes from the mix, the myth set, and the unique Epic riders chosen in the family docs.

**Leather — "the nymph's hide"**

| Theme | Myth | Root | Mechanical identity |
|---|---|---|---|
| Mending | the naiads | **Naias** | HP regen + heals received |
| Briar | the dryads | **Dryas** | thorns + poison resist |
| Stride | the oreads | **Oreias** | weight reduction + stam regen |
| Balm | Melissa, honey-nymph | **Melissa** | auto-cure + heals received |
| Startle | Pan's panic | **Panika** | dodge |

**Studded — "the hunter's brand"**

| Theme | Myth | Root | Mechanical identity |
|---|---|---|---|
| Chase | kynegis, the huntress | **Kynegis** | dodge + stam regen |
| Bramble | batos, the briar | **Batos** | thorns |
| Bear | Arkas the bear-son | **Arkas** | bonus AR + shrug |
| Deer | elaphis, the hind | **Elaphis** | weight reduction + dodge |
| Shadow | skia, shade | **Skia** | poison resist + Hiding skill bonus |

**Bone — "the grave-warden"**

| Theme | Myth | Root | Mechanical identity |
|---|---|---|---|
| Phantom | Melinoe, ghost-bringer | **Melinoe** | dodge + para resist |
| Blessed Death | Makaria, Hades' daughter | **Makaria** | on-kill restores |
| Tomb | tymbos, the barrow | **Tymbos** | bonus AR + DR |
| Death-ward | the nekyia rite | **Nekyia** | spell DR |
| Grave-thorns | katachthon, of-the-underworld | **Katachthon** | thorns/reflect |

**Ring — "the hoplite's kit"**

| Theme | Myth | Root | Mechanical identity |
|---|---|---|---|
| Line-shield | the hoplites | **Hoplites** | bonus AR + block |
| Formation | taxis, battle order | **Taxis** | DR + para resist |
| March | dromos, the march | **Dromos** | stam regen + weight reduction |
| War-belt | zoster, the girdle | **Zoster** | durability + self-repair |
| Valiant | alkimos, the valiant | **Alkimos** | shrug |

**Chain — "the watchful wall"**

| Theme | Myth | Root | Mechanical identity |
|---|---|---|---|
| Guard | phylax, the guard | **Phylax** | bonus AR + shrug |
| Unsleeping | egregoros, the wakeful | **Egregoros** | para resist + first-hit shrug |
| Wall | teichos, the wall | **Teichos** | DR |
| The Chain | halysis, the chain itself | **Halysis** | durability + self-repair |
| Fortress | phrourion, the stronghold | **Phrourion** | spell DR |

**Plate — "the forged colossus"**

| Theme | Myth | Root | Mechanical identity |
|---|---|---|---|
| Adamant | adamas, the unbreakable | **Adamas** | top bonus AR + DR |
| Kiln | kaminos, the forge-kiln | **Kaminos** | flame proc + reflect |
| Colossus | the kolossos | **Kolossos** | shrug + stun/para resist |
| Panoply | panoplia, full armor | **Panoplia** | bonus AR + durability |
| Unwearying | akamatos, tireless | **Akamatos** | self-repair + HP regen |

### Shields — "the aegis line"

| Theme | Myth | Root | Mechanical identity |
|---|---|---|---|
| Parry | the aegis itself | **Aegis** (kept) | parry chance + DR on parry + parry thorns |
| Counter | amyntor, the defender | **Amyntor** | reflect |
| Breakwater | probolos, the breakwater | **Probolos** | DR on block |
| Barrier | herkos, the fence of war | **Herkos** | spell DR |
| Second Wind | pnoe, breath | **Pnoe** | stam/HP regen on parry |

### Jewelry (ring, bracelet, necklace, earrings)

| Theme | God | Root | Mechanical identity | Hue family |
|---|---|---|---|---|
| Might | Zeus | **Olympian** | stat bonuses (Str/Dex/Int), lightning proc | storm-gold (flagship 2213) |
| Sorcery | Hecate | **Hecatean** | mana regen, mana leech, spell damage | violet run ~896 area |
| Fortune | Tyche | **Tychean** | beneficial procs, durability saves, loot QoL | gold-green, tune |
| Night | Nyx | **Nyxian** | hiding/stealth skill, night sight, poison resist | near-black purple, tune |
| Harvest | Demeter | **Demetrian** | all-regen (HP/stam/mana small), food/heal bonus | wheat-green, tune |

### Clothing

| Theme | God | Root | Mechanical identity | Hue family |
|---|---|---|---|---|
| Victory | Nike | **Laurel** | on-kill heal/stam refund | gold-green, tune |
| Charm | Aphrodite | **Charis** | karma gain, vendor prices, taming/barding QoL | rose, tune |
| Frenzy | Dionysus | **Maenad** | random short combat buff on being struck | wine-purple, tune |
| Hearth | Hestia | **Hestian** | regen while stationary/at home | warm ember, tune |
| Web | Arachne | **Arachne** | dodge chance, poison resist | grey-web, tune |

**Hue ramp rule:** UO standard hues come in 5-shade runs. Assign each root one run; rarity picks
the shade — Uncommon = 2nd shade … Legendary = 5th (most saturated). Commons stay unhued.
Verify run direction (dark→bright) in-client before committing hue numbers. Until the in-client
pass, every new root borrows the legacy run closest to its lane's vibe (placeholders).

## 4. Effect scaling pattern (per rarity)

Identical structure to the approved axe spec:

| Rarity | Effect package |
|---|---|
| Common | none |
| Uncommon | 1 theme effect, small |
| Rare | Uncommon effect (sometimes bumped) + 1 support effect |
| Epic | bumped primary + support + 1 signature proc |
| Legendary | all Epic effects + **one unique named clause** (per base × theme) |

**Legendary base-package rule:** on weapons, the Legendary base package *equals* the Epic package
(the damage anchor carries rarity scaling). On armor, shields, jewelry and clothing relics —
families whose §2 tables list an explicit Legendary column — that column is a step *above* Epic
by design, then the unique clause is added on top. Both are intentional; do not reconcile one
into the other.

Lanes are family-unique (§3), so the framework no longer centralizes per-root weapon tables.
Family docs hold each lane's Uncommon/Rare/Epic table — but they may not invent values: every
number is drawn from the **magnitude menu** below (per-primitive standard steps, derived from
the approved axe-spec scale). Axes keep their original values, restated in `01-axes.md`.
A lane's table = primary primitive at Uncommon → +1 support at Rare → bumps + the lane's Epic
signature clause at Epic (the 1 / 2 / 3 / 3+unique pattern above, unchanged).

| Primitive (always-on unless noted) | Uncommon | Rare | Epic |
|---|---|---|---|
| Swing speed % (P1) | 8 | 8 | 10 |
| Damage % (P2) | 8 | 8 | 10 |
| Hit chance % (P3) | 6 | 6–8 | 8 |
| Crit chance % (P4) | — | 8 | 10 |
| Crit damage % (P4) | — | — | 20 |
| Extra-swing proc % (P5) | — | — | 10 |
| Lifesteal % (P6) | 6 | 6 | 8 |
| Stam/mana leech or regen % (P7/P8) | 6 | 6–8 | 8–10 |
| Mark chance % / mark bonus % (P9) | 6 / 8 | 8 / 10 | 10 / 14 |
| Poison apply % (P10) | 8 | 10 | 12 (tier up only where the lane says so) |
| Block % / DR-on-block % (P11) | 6 / — | 6 / 8 | 8 / 12 |
| Stagger proc %, 1s (P13, §9.2) | 4 | 6 | 8 |
| First-hit-of-fight bonus dmg % (P14) | 15 | 20 | 25 |
| Every-Nth-hit bonus dmg % (P15) | +15 @ 5th | +20 @ 5th | +25 @ 4th |
| Elemental proc % (P24) | 6 | 8 | 10 |
| Heal-block proc % (P25, §9.3) | — | 6 | 8 |
| Splash % of damage, ≤3 targets (P26) | 8 | 10 | 12 |
| Armor pen % (P27) | 10 | 15 | 20 |
| Consecutive-hit ramp (P28) | +2%/stack, max 5 | +3%/stack, max 5 | +3%/stack, max 6 |
| On-kill restore, % max stam (P23) | 10 | 15 | 20 |
| Weapon worn-side utility: spell DR % / mana regen % / dodge % | 2 / 8 / 2 | 3 / 12 / 3 | 5 / 18 / 4 |

Menu values are placeholder-tunable (README watch list). Armor/shield lanes keep the magnitude
scales of the retired shared tables — family docs restate them per material; jewelry/clothing
docs unchanged. Clothing magnitudes run at ~50% of jewelry's.

## 5. Effect primitive catalog (engine contract)

Everything below is implementable in ModernUO single-threaded game loop with one hook each.
**Only these primitives may appear on items.** (Feasibility inferred from BaseWeapon/BaseArmor
hook structure; each needs its engine task before first use.)

| # | Primitive | Engine hook |
|---|---|---|
| P1 | Swing speed % | `BaseWeapon.GetDelay` |
| P2 | Damage % | weapon damage computation |
| P3 | Hit chance % | `BaseWeapon.CheckHit` |
| P4 | Crit chance % / crit damage % | custom roll in `OnHit` |
| P5 | Extra-swing proc (lands at 65% damage — 2026-07-11 balance pass: a full-strength free hit measured +25-60% family DPS on the cadence lanes; the scalar prices the class, cadences/procs unchanged) | `OnHit` → next-swing time credit |
| P6 | Lifesteal % | `OnHit` heal attacker |
| P7 | Stamina leech / stam regen | `OnHit` / regen rate hook |
| P8 | Mana leech / mana regen | `OnHit` / regen rate hook |
| P9 | Huntress' Mark debuff | timer + marked-by table (+% dmg taken from marker) |
| P10 | Poison apply chance | `OnHit` → `Poison.Lesser+` |
| P11 | Block % + DR-on-block | `AbsorbDamage` |
| P12 | Thorns (reflect) | `AbsorbDamage` |
| P13 | Brief stun proc | paralyze ≤2s, PvP diminishing (§9) |
| P14 | First-hit-of-fight trigger | per-combat flag, reset on combatant change |
| P15 | Every-Nth-hit counter | per-weapon counter |
| P16 | Bonus AR | `BaseArmor` rating virtual |
| P17 | HP/stam/mana regen (worn) | Mobile regen hooks |
| P18 | Stat bonus (Str/Dex/Int) | `StatMod` (exists in core) |
| P19 | Skill bonus | `SkillMod` (exists in core) |
| P20 | Magic resist % / spell DR | resist/damage hooks |
| P21 | Weight reduction | item weight |
| P22 | Durability / self-repair | `IDurability` (exists) |
| P23 | On-kill trigger (heal/stam/mana restore) | kill event |
| P24 | Elemental proc (lightning/fire FX + damage) | spell FX util + flat damage |
| P25 | Heal-block debuff | capped 3s, custom debuff |
| P26 | Splash damage | `GetMobilesInRange`, target cap 3 |
| P27 | Armor penetration % | AR bypass in damage calc |
| P28 | Consecutive-hit ramp (stacking dmg vs one target; resets on target swap / fight lapse) | per-attacker tracker in `CombatFxState` |

**P27 status note (2026-07-07):** pen was spec'd but never wired — `CritArmorPen` only forced
crit cadence, so six legendaries (Enyo, Chrysaor, Mimas, Steropes, Pelion, Alkon) silently
no-op'd their pen rider. Wiring lands with the re-theme engine pass and retro-fixes them.

**Banned (client-enforced or no system exists):** movement speed, cooldown reduction/reset,
luck stat. Approved substitutions when adapting older spec text:
movement speed → stamina refund (P7/P23); cooldown reset/refresh → full stam+mana restore on
kill (P23); "immune to disarm" → dropped (no disarm special in T2A).

## 6. Naming grammar

- Single-click: `[root] [base name] [rarity-suffix]` → `phobos battle axe [rare]`
  (suffix comes from `RarityConfig.GetSuffix`, do not bake it into `Name`).
- Legendary: `[ProperNoun] [BaseName] [rarity-suffix]` → `Labrys Double Axe [legendary]`
  (BuildLegendaryName embeds the base shape in the name — revised 2026-07-13, was
  `Labrys [legendary]` + OPL subtitle; no separate subtitle line anymore).
- OPL order: name → rarity line (engine) → theme effects → unique clause (legendary).
- Commons keep stock names, no root, no hue.

## 7. Base ladders (all families — fixed here, do not re-derive)

Legendary DPS parity target across melee families: **8.70 ±3%** at top base (recomputed for
`D[Legendary]=30`; outlier %s are scale-invariant, so they carry over unchanged). Deliberate outliers:
maces −1.5% (paid back by crush identity), archery −16% (ranged safety tax), staves −8%
(caster utility budget).

### Weapons — `ratio / swing seconds` (Legendary DPS in parens)

**Axes** (Swordsmanship) — per approved spec:
hatchet .65/2.75 (7.09) · axe .70/2.85 (7.37) · battle axe .75/2.95 (7.63) ·
double axe .80/3.05 (7.87) · executioner's axe .85/3.15 (8.10) · two-handed axe .90/3.25 (8.31) ·
large battle axe .95/3.35 (8.51) · ornate axe 1.00/3.45 (8.70)

**Swords** (Swordsmanship):
butcher knife .60/2.55 (7.06) · cleaver .65/2.65 (7.36) · cutlass .70/2.75 (7.64) ·
scimitar .75/2.85 (7.89) · katana .80/2.95 (8.14) · broadsword .85/3.05 (8.36) ·
longsword .90/3.15 (8.57) · viking sword .95/3.25 (8.77)

**Polearms** (Swordsmanship):
bardiche .90/3.25 (8.31) · halberd .98/3.40 (8.65)

**Maces** (Mace Fighting):
club .70/2.90 (7.24) · mace .75/3.00 (7.50) · maul .80/3.10 (7.74) · war axe .85/3.20 (7.97) ·
hammer pick .90/3.30 (8.18) · war mace .95/3.40 (8.38) · war hammer 1.00/3.50 (8.57)

**Staves** (Mace Fighting, caster hybrid):
quarter staff .65/2.70 (7.22) · gnarled staff .72/2.85 (7.58) · black staff .80/3.00 (8.00)

**Fencing**:
dagger .50/2.05 (7.32) · kryss .55/2.15 (7.67) · war fork .60/2.25 (8.00) ·
pitchfork .65/2.35 (8.30) · short spear .70/2.45 (8.57) · spear .75/2.55 (8.82)

**Archery**:
bow .70/3.10 (6.77) · crossbow .80/3.40 (7.06) · heavy crossbow .90/3.70 (7.30)

### Armor — material ratio × slot weight

Materials: leather .65 · studded .72 · bone .79 · ring .86 · chain .93 · plate 1.00
Slots: chest 1.00 · legs .70 · helm .50 · arms .40 · gorget .25 · gloves .25 (suit sum 3.10)
Sanity: Common plate suit AR = 1.00 × 3.10 × 12 ≈ 37 (≈ stock T2A plate — inferred, verify
against live values); Legendary plate suit ≈ 81.

### Shields — ratio

buckler .60 · wooden shield .68 · wooden kite .76 · metal shield .84 · metal kite .92 · heater 1.00

### Jewelry / clothing

No ladder — slot-based. Jewelry slots: ring, bracelet, necklace, earrings. Clothing pieces
(curated, bonus-bearing): robe, cloak, doublet, tunic, sash, kilt, skirt, hat family (pick 4–6
shapes), pants (long/short), boots. Full effect budget goes to effects (no damage/AR share). Hats
and cloth pants occupy armor layers (helm/pants) and so run at 2× the other clothing magnitudes —
see `21-clothing.md` §1.

## 8. Distribution

- Loot-bag levels 0–10; rarity ceilings already coded in `RarityConfig.MaxRarityForBagLevel`
  (L0–1 → Uncommon max, L2–3 → Rare, L4–6 → Epic, L7+ → Legendary).
- **Exactly one item per bag** (user directive 2026-07-07). A bag is one roll, no fillers.
- Roll order: rarity → family → theme (uniform 1/5) → base.
- **Category weights** (implemented in `LootRoller`, placeholder — tune live): weapons 45 ·
  armor 25 · shields 10 · jewelry 12 · clothing 8; family uniform within category.
- **Base pick**: target index t = round(bagLevel/10 × (N−1)); weight(i) = max(1, 4 − |i − t|).
- **Rarity weights per bag level** (proposal, tune live):

| Bag lvl | Common | Uncommon | Rare | Epic | Legendary |
|---:|---:|---:|---:|---:|---:|
| 0–1 | 0 | 100 | — | — | — |
| 2 | 0 | 80 | 20 | — | — |
| 3 | 0 | 65 | 35 | — | — |
| 4 | 0 | 50 | 42 | 8 | — |
| 5 | 0 | — | 85 | 15 | — |
| 6 | 0 | — | 75 | 25 | — |
| 7 | 0 | — | 66 | 30 | 4 |
| 8 | 0 | — | 55 | 39 | 6 |
| 9 | 0 | — | 40 | 52 | 8 |
| 10 | 0 | — | — | 90 | 10 |

User directive 2026-07-08: bags never drop Common. Floors: L0-4 Uncommon, L5-9 Rare, L10
Epic-only. Weights above L1 remain live-tuning placeholders.

- **Base pick within family:** weight ∝ closeness of base index to bag level scaled onto the
  ladder (weak bases common in low bags, top bases dominate high bags); all bases possible at all
  levels. Exact curve is an implementation detail.
- **Legendary** finds broadcast via `RaritySystem.Announce` (user directive 2026-07-07; the
  `rarity.announceMinTier` setting can lower it back to Epic without a code change).

## 9. Balance rules (hard)

1. Within-family Legendary DPS spread ≤ ~25% bottom→top base; cross-family per §7 targets.
2. Stun procs: ≤2s, and a struck target is stun-immune for 10s after any item stun (PvP).
3. Heal-block: ≤3s, never on every plain hit — it must be gated (crit-only, first-hit-of-fight,
   or proc chance).
4. Same god theme worn on multiple accessories: strongest applies fully, the rest at 50%.
5. Weapon + armor + jewelry of the same god is allowed (build identity) — but audit any
   cross-item combo that exceeds the single-item Epic magnitude by >2× at review time.
6. Splash/AoE: 3 targets max, no chaining.
7. Any "guaranteed X on first hit of fight" resets only when combat fully ends (30s out of
   combat), not on target swap.
8. **Shared defensive pools** (hard ceilings across everything worn — armor + shield + clothing
   together, enforced via rule 4's diminishing stack): bonus AR ≤ 15 · damage reduction ≤ 12% ·
   shrug ≤ 20% · thorns/reflect ≤ 25% · HP regen ≤ 60% · spell DR ≤ 18% · dodge ≤ 12%. Tune live.

## 10. Legendary naming rules

- One unique proper noun per base × theme, globally unique across every family doc.
- **Roots share the same namespace**: a root name may not duplicate any legendary proper noun
  (and vice versa) — grep the whole directory before claiming either.
- Namespace: Greek myth — figures, epithets, monsters, rivers, artifacts, places. Transliterate
  Greek (k over c where natural: *Kerberos* fine, *Cerberus* fine — pick one per name, no dupes).
- Iconic myth objects go to their natural base (e.g. **Labrys** = double axe, **Harpe** = a sword,
  **Keraunos** = a mace/thrown-bolt analog). Family docs claim names in a "Legendary registry"
  table; the review pass (README) merges them and rejects collisions.
- Unique clauses compose from §5 primitives only, one clause per legendary.

## 11. Crafting (deferred — pointers only)

- Crafted variants exist in the approved axe spec: per-base unique names, Uncommon→Epic (no
  Legendary), same damage anchors. Family docs OMIT crafted tables for now.
- When picked up: T2A crafting reference lives at `dev-docs/t2a-crafting.md`; crafted names stay
  unique per base (mortal smiths don't share god enchantments) — mythic material names
  (adamant, orichalcum, Cretan bronze) are the suggested naming lane.

## 12. Implementation phasing (future, not this pass)

1. Effect-primitive engine (§5) — one system, serialization-safe, OPL lines per CLAUDE.md rules.
2. Variant data tables (root × rarity × family) — data-driven, not 500 item classes.
3. Loot roller (§8) wiring into loot-bag levels + `Announce`.
4. Hue pass in-client.

## File map

| File | Contents |
|---|---|
| `00-framework.md` | this file |
| `01-axes.md` | axes (exemplar — canonical format for all family docs) |
| `02-swords.md` | 8 blades |
| `03-polearms.md` | bardiche, halberd |
| `04-maces.md` | 7 maces incl. war axe |
| `05-staves.md` | 3 staves |
| `06-fencing.md` | 6 fencing weapons |
| `07-archery.md` | 3 bows |
| `10-armor-metal.md` | ring/chain/plate sets + metal helms |
| `11-armor-light.md` | leather/studded/bone sets + light helms |
| `12-shields.md` | 6 shields |
| `20-jewelry.md` | 4 jewelry slots |
| `21-clothing.md` | curated bonus-bearing clothing |
| `README.md` | index + merged legendary registry |
