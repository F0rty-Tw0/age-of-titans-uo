# 06 — Fencing (Fencing)

**Derives from:** `00-framework.md` (budgets §2, pantheon §3, effect pattern §4, primitives §5,
balance rules §9, naming rules §10, ladder §7).
**Source:** base ratios/speeds fixed in framework §7 ("Fencing" ladder, 2026-07-07 approval) —
carried over unchanged. Legendary names and unique clauses composed for this pass, themed to the
Greek pantheon per framework §3/§10; effect text stays inside the primitive catalog (§5) only.

---

## 1. Base ladder & damage matrix

Damage = `ratio × D[rarity]`, D = (10, 13, 17, 22, 30). Speed = swing seconds (bigger =
slower). DPS columns are base values before speed effects.

| Base | Ratio | Speed | Common | Uncommon | Rare | Epic | Legendary | Leg. DPS |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Dagger | 0.50 | 2.05s | 5.0 | 6.5 | 8.5 | 11.0 | 15.0 | 7.32 |
| Kryss | 0.55 | 2.15s | 5.5 | 7.2 | 9.4 | 12.1 | 16.5 | 7.67 |
| War fork | 0.60 | 2.25s | 6.0 | 7.8 | 10.2 | 13.2 | 18.0 | 8.00 |
| Pitchfork | 0.65 | 2.35s | 6.5 | 8.5 | 11.1 | 14.3 | 19.5 | 8.30 |
| Short spear | 0.70 | 2.45s | 7.0 | 9.1 | 11.9 | 15.4 | 21.0 | 8.57 |
| Spear | 0.75 | 2.55s | 7.5 | 9.8 | 12.8 | 16.5 | 22.5 | 8.82 |

Commons are the plain base items: stock name, no hue, no effects.

## 2. Drop-variant themes — "serpent's tempo" (fencing-unique, shared across all 6 bases)

Names read `[root] [base] [rarity]` — e.g. `ios war fork [rare]`. Roots are fencing-only (framework
§3, 2026-07-07 re-theme): each lane's primary primitive and magnitudes are drawn from the framework
§4 menu, not copied from another family. Fencing's own identity leans on faster tempo than the
axe/sword families: Kentron and Aiolos keep the faster 3rd/4th-hit cadences (instead of 5th/6th),
reflecting Fencing's higher swing count in T2A.

| Root | Lane | Uncommon | Rare | Epic (+ signature) |
|---|---|---|---|---|
| **Aiolos** | flurry | +8% swing speed | +8% swing speed, +6% hit chance | +10% swing speed, +8% hit chance, 10% extra-swing proc — **every 7th hit grants an extra swing that may chain into one more** (cadence 5→7, 2026-07-11 DPS-sim pass: at 5 the lane measured ~+60% over the family pack) |
| **Ephodos** | lunge | +15% damage on the first hit of any fight (P14) | +20% first-hit damage, +8% crit chance | +25% first-hit damage, +10% crit chance — **the first hit of any fight is a guaranteed crit that also refunds its stamina cost** |
| **Ios** | venom | 8% poison apply chance (P10) | 10% poison apply chance, +6% hit chance | 12% poison apply chance, +8% hit chance — **poison severity tiers up (Lesser → Greater)** |
| **Kentron** | puncture | 10% armor penetration (P27) | 15% armor penetration, +6% hit chance | 20% armor penetration, +8% hit chance — **every 3rd hit fully ignores armor** |
| **Ophis** | evasion | +6% dodge/parry | +6% dodge/parry | +8% dodge/parry — **a successful dodge/parry opens a brief counter-window: your next hit crits (DodgeGrantsCounterWindow)** |

*Phase-3 wiring note (Ophis §2):* the "DR on a successful dodge/parry" is not a separate mechanic
— a dodge fully avoids the hit, so there is nothing to reduce. The rows therefore carry `DodgePct`
only (6/6/8), plus the Epic counter-window signature.

## 3. Legendaries (unique per base × theme)

Every legendary = **all Epic effects of its root** + the unique clause below. Damage from §1
Legendary column. Single-click shows the proper noun (`Pelion [legendary]`); base shape appears
in the tooltip. Namespace for this family (unchanged by the re-theme): spear-heroes of the
Trojan/Theban cycles, and mythic serpents/dragons of Greek myth.

**Lane bijection (old → new):** Zephyr→Aiolos · Phobos→Ephodos · Agrotera→Ios · Pallas→Ophis ·
Stygian→Kentron. Clause text below restates the **registry (code truth)**; several pre-retheme doc
lines had invented poison-tick/resist detail the registry never carried (flagged per line) — the
registry wording wins. Audit column: FIT = clause kept verbatim (reworded only where the old prose
had drifted, or where the delivery mechanic is simply relabeled — e.g. "block" reads as
"dodge/parry" under Ophis); SWAP = clause replaced (old → new).

### Aiolos line (was Zephyr — swift spear-companions, now the wind-lord's flurry)

Zephyr's mechanical identity (swing speed + extra swing) *is* Aiolos's new mechanical identity —
nothing here is foreign to the re-themed lane, so the whole line carries over unchanged.

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Dagger | **Balios** | guaranteed extra swing every 4th hit; each extra swing refunds its stamina cost | FIT |
| Kryss | **Kyknos** | guaranteed extra swing on the first hit of any fight; every follow-up swing this fight gains +5% hit chance (stacks to +20%) | FIT |
| War fork | **Asteropaios** | guaranteed extra swing every 4th hit; extra swings ignore 10% armor (P27) | FIT |
| Pitchfork | **Protesilaos** | guaranteed extra swing on the first hit of any fight; killing blow restores 25 stamina | FIT |
| Short spear | **Akamas** | guaranteed extra swing every 3rd hit; each extra swing leeches 5% stamina from the target | FIT |
| Spear | **Peleus** | guaranteed double strike every 4th hit; the second strike always lands (guaranteed hit) | FIT |

### Ephodos line (was Phobos — the opening lunge)

Ephodos's new identity is first-hit-of-fight bonuses, not crit-in-general — most of the old crit
riders still read fine as an assault-flavored crit package, but two entries are a plain guaranteed
first-hit crit with nothing else, which is exactly the lane's own Epic signature restated. Both
must swap.

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Dagger | **Parthenopaios** | crits deal double damage against full-HP targets | SWAP: CritFirstHit(0,0,0) → CritFullHpDouble(0, 0, 0) — plain "guaranteed crit on first hit" duplicates Ephodos' own Epic signature verbatim. P3 must stay 0: P3=1 re-adds a forced first-hit crit and restates the signature again (same trap as archery's Pandaros) |
| Kryss | **Kapaneus** | guaranteed crit every 4th hit; crits deal 10% splash (3 targets, P26) | FIT |
| War fork | **Tydeus** | guaranteed crit every 3rd hit; crit damage bonus doubles vs targets below 25% HP | FIT |
| Pitchfork | **Asios** | guaranteed crit every 4th hit; crits deal 15% splash (3 targets, P26) | FIT |
| Short spear | **Meleagros** | guaranteed crit every 4th hit; crits stagger the target (1s stun, §9) | SWAP: CritFirstHit(0,0,0) → CritStagger(4) — duplicates Ephodos' own Epic signature verbatim; re-paramed to an every-4th-hit cadence |
| Spear | **Pelion** | guaranteed crit every 3rd hit | SWAP: CritArmorPen(3, 20) → CritEveryN(3) — armor pen is Kentron's puncture fantasy now, and the every-3rd-hit cadence duplicated Kentron's own signature too closely on top of that |

### Ios line (was Agrotera — venomous serpents, mark re-read as venom)

Agrotera's mark-family riders carry no poison content in the *registry* — several old doc lines
had invented a poison-tick/resist detail that the code never implemented (flagged below). The
underlying debuff reads naturally as an envenomed strike either way, so every entry FITs — reworded
to venom flavor and corrected to what the registry actually grants.

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Dagger | **Amphisbaena** | a venomous strike guarantees a mark on the first hit of any fight | FIT (registry: `MarkFirstHit`, no extra params — old doc's "poison tick on every subsequent hit" was invented, not in code) |
| Kryss | **Delphyne** | guaranteed venom-mark on the first hit of any fight; marked targets take +25% damage from all sources | FIT |
| War fork | **Python** | every crit injects a venom-mark on the target | FIT (registry: `MarkOnCrit`, no extra params — old doc's "poison resist halved" was invented, not in code) |
| Pitchfork | **Lamia** | guaranteed venom-mark on the first hit; on the marked target's death the venom spreads to ≤3 nearby enemies | FIT |
| Short spear | **Typhon** | guaranteed venom-mark on the first hit of any fight; marked targets take +25% damage from all sources | FIT (registry: `MarkAllSources25(25)` only — old doc's "and a poison tick every 3rd hit" was invented, not in code) |
| Spear | **Hydra** | a venomous strike guarantees a mark on the first hit of any fight | FIT (registry: `MarkFirstHit`, no extra params — old doc's "25% chance to poison tick" was invented, not in code) |

### Ophis line (was Pallas — guardian serpents, block re-read as sway)

Pallas's block/reflect riders carry no content foreign to an evasion fantasy — a parry or a
sinuous dodge reads the same as a block mechanically. Whole line FITs, relabeled from "block" to
"dodge/parry."

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Dagger | **Aspis** | guaranteed dodge/parry vs the first hit of any fight; a dodged crit briefly stuns the attacker (1s stun, §9 immunity) | FIT |
| Kryss | **Ladon** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (1s stun, §9) | FIT |
| War fork | **Ekhion** | guaranteed dodge/parry vs the first hit of any fight; a dodged hit also applies a poison tick to the attacker (P10) | FIT |
| Pitchfork | **Kaineus** | guaranteed dodge/parry vs the first hit of any fight; every dodge/parry for the rest of that fight also reduces incoming damage by 10% | FIT |
| Short spear | **Kolchis** | the first hit taken each fight is reflected for 25% of its damage and staggers the attacker (1s stun, §9) | FIT |
| Spear | **Bellerophon** | guaranteed dodge/parry vs the first hit of any fight; a dodged crit reflects 30% of its damage back at the attacker | FIT |

### Kentron line (was Stygian — the sting bites through)

The heavy-swap line. Stygian's old lifesteal/stam-drain/on-kill-restore/mana-leech riders are all
foreign to puncture — Kentron carries zero drain identity. All six swap to armor-pen flavors.
**Phase-3 wiring correction:** the originally-drafted `NthHitFullArmorPen(N)` uniques (Sybaris,
Drakaina, Ophion) restated Kentron's own Epic signature *type* (`NthHitFullArmorPen`, every 3rd
hit) — the audit invariant is type-level, not cadence-level (the Pede precedent), so a different N
does not make them distinct. All six therefore use `CritArmorPen` (crit-gated partial pen), the
same distinct-from-signature vehicle the Rhaistes line uses.

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Dagger | **Ketos** | guaranteed crit every 4th hit; crits ignore 20% armor (P27) | SWAP: StamDrainOnCrit → CritArmorPen(4, 20) — stamina drain is foreign to puncture |
| Kryss | **Sybaris** | guaranteed crit every 5th hit; crits ignore 20% armor (P27) | SWAP: LifestealOnCrit → CritArmorPen(5, 20) — lifesteal foreign to puncture; CritArmorPen (not NthHitFullArmorPen == signature) |
| War fork | **Drakaina** | guaranteed crit every 6th hit; crits ignore 25% armor (P27) | SWAP: OnKillRestore(2) → CritArmorPen(6, 25) — on-kill restore foreign to puncture; CritArmorPen (not NthHitFullArmorPen == signature) |
| Pitchfork | **Ismenios** | guaranteed crit every 6th hit; crits ignore 25% armor (P27) | SWAP: LifestealOnCrit → CritArmorPen(6, 25) — lifesteal is foreign to puncture |
| Short spear | **Ophion** | guaranteed crit every 4th hit; crits ignore 15% armor (P27) | SWAP: OnKillRestore(1) → CritArmorPen(4, 15) — on-kill restore foreign to puncture; CritArmorPen (not NthHitFullArmorPen == signature) |
| Spear | **Kampe** | guaranteed crit every 6th hit; crits ignore 20% armor (P27) | SWAP: CritManaLeech(5, 10) → CritArmorPen(6, 20) — mana leech belongs to no fencing lane (fencing has no caster identity) |

## 4. Legendary registry (claimed names — review pass merges these)

*Re-theme note (2026-07-07):* all 30 names and ids are unchanged by the per-family root re-theme;
only each line's root (Zephyr→Aiolos, Phobos→Ephodos, Agrotera→Ios, Pallas→Ophis, Stygian→Kentron)
and the audited clauses above moved. Aiolos and Ophis carry over 12 of 12 clauses unchanged; Ios
carries over all 6 (reworded to venom, with invented poison detail corrected against the
registry); Ephodos swaps 2 of 6 (signature duplicates); Kentron swaps all 6 (its old Stygian
content was 100% drain-flavored, none of it puncture).

Balios, Kyknos, Asteropaios, Protesilaos, Akamas, Peleus, Parthenopaios, Kapaneus, Tydeus,
Asios, Meleagros, Pelion, Amphisbaena, Delphyne, Python, Lamia, Typhon, Hydra, Aspis, Ladon,
Ekhion, Kaineus, Kolchis, Bellerophon, Ketos, Sybaris, Drakaina, Ismenios, Ophion, Kampe. (30)

*Merge-pass note (2026-07-07):* 11 names re-assigned to resolve cross-family collisions — swords
keeps the Trojan core (Automedon→metal armor, Antilochos, Meriones, Idomeneus, Sarpedon,
Diomedes), staves keeps Amphiaraos, light armor keeps Echidna/Skylla/Khimaira. Fencing retains
Typhon, Hydra, Ladon, Kaineus (tightest myth fit here); Cetus corrected to the Greek Ketos.

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-base unique names, Uncommon→Epic, same damage anchors)
exist in the approved source spec and will be re-themed to mythic materials in the crafting pass.
Nothing here blocks them.
