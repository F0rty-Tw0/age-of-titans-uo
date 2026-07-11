# 03 — Polearms (Swordsmanship)

**Derives from:** `00-framework.md` (budgets §2, themes §3, effect pattern §4, primitives §5,
ladder §7). Format mirrors `01-axes.md` §1–§5 (canonical).
**Bases:** bardiche, halberd — the smallest base ladder in the weapon roster (2 bases). Legendary
clauses lean into the polearm's reach/sweep identity where natural (splash, P26, ≤3 targets).

---

## 1. Base ladder & damage matrix

Damage = `ratio × D[rarity]`, D = (10, 13, 17, 22, 30). Speed = swing seconds (bigger =
slower). DPS columns are base values before speed effects.

| Base | Ratio | Speed | Common | Uncommon | Rare | Epic | Legendary | Leg. DPS |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Bardiche | 0.90 | 3.25s | 9.0 | 11.7 | 15.3 | 19.8 | 27.0 | 8.31 |
| Halberd | 0.98 | 3.40s | 9.8 | 12.7 | 16.7 | 21.6 | 29.4 | 8.65 |

Commons are the plain base items: stock name, no hue, no effects.

## 2. Drop-variant themes — "the reaping line" (polearm-unique, shared across both bases)

Names read `[root] [base] [rarity]` — e.g. `theristes bardiche [rare]`. One effect package per
root per rarity; only the damage number differs by base. Roots are polearm-only (framework §3,
2026-07-07 re-theme); magnitudes drawn from the framework §4 menu. Epic adds the lane's signature
clause. Hues: placeholder runs until the in-client pass.

| Root | Lane | Uncommon | Rare | Epic (+ signature) |
|---|---|---|---|---|
| **Theristes** | reap | 8% splash on hit (≤3 targets, P26) | 10% splash on hit, +8% damage | 12% splash on hit, +10% damage — **every crit also splashes, guaranteed, at full effect** |
| **Sarisa** | impale | +10% armor pen (P27) | +15% armor pen, +8% crit chance | +20% armor pen, +10% crit chance, +20% crit damage — **the first hit of any fight is a guaranteed crit that carries the full pen bonus** |
| **Horme** | momentum | +15% damage @ 5th hit (P15) | +20% damage @ 5th hit, +6% hit chance | +25% damage @ 4th hit, +8% hit chance — **that same hit also lands as a guaranteed extra swing** |
| **Phalanx** | hold | +6% block | +6% block, 8% DR on block | +8% block, 12% DR on block — **a successful block grants a brief window of bonus damage reduction** |
| **Zophos** | toll | +6% lifesteal | +6% lifesteal, on-kill restore 15% max stam (P23) | +8% lifesteal, on-kill restore 20% max stam — **lifesteal doubles vs targets below 30% HP** |

Splash/ramp/lifesteal effects reset or cap per framework §9 (splash ≤3 targets, ramp resets on
target swap, stun procs ≤2s with 10s immunity). Signature clauses layer onto the bumped Epic
package, per framework §4.

## 3. Legendaries (unique per base × theme)

Every legendary = **all Epic effects of its root** + the unique clause below. Damage from §1
Legendary column. Single-click shows the proper noun (`Porphyrion [legendary]`); base shape
appears in the tooltip. Namespace for this family: the **Gigantes** (Giants) and the
**Gigantomachy** — the earthborn brood who stormed Olympus and fell to the gods.

**Names and ids are frozen** — the 2026-07-07 re-theme re-points each line to the polearm-unique
roots via the lane bijection below and re-audits clauses against the new lane fantasy. Clause text
below restates the **registry (code truth)**; a few pre-retheme doc lines had drifted from the
registry — the registry wording wins. Audit column: FIT = clause kept verbatim (registry-true
wording); SWAP = clause replaced (old → new).

**Lane bijection (old → new):** Zephyr→Theristes · Phobos→Sarisa · Agrotera→Horme · Pallas→Phalanx
· Stygian→Zophos.

### Theristes line (was Zephyr — the giants' rout)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bardiche | **Hippolytos** | guaranteed extra swing every 5th hit; the extra swing sweeps for 10% splash damage (3 targets) | FIT |
| Halberd | **Thoon** | guaranteed extra swing on the first hit of any fight | FIT (registry: `ExtraSwingFirstHit` carries no stamina-refund parameter; the old doc's "refunds its stamina cost" was drift — dropped) |

### Sarisa line (was Phobos — the giants' assault)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bardiche | **Mimas** | guaranteed crit every 6th hit; crits ignore 10% armor (P27) | FIT |
| Halberd | **Porphyrion** | guaranteed crit every 5th hit; crits ignore 15% armor (P27) | SWAP: `CritSplash(5,15,3)` → `CritArmorPen(5,15)` — splash is Theristes' territory now, foreign to Sarisa's impale/armor-pen fantasy; also drops the old doc's "doubled against targets below 15% HP" flourish, which was never in the registry parameters |

### Horme line (was Agrotera — the hunted giants)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bardiche | **Gration** | guaranteed extra swing every 5th hit, which chains into one additional free swing | SWAP: `MarkNearbyAllies(3)` → `ExtraSwingChain(5)` — mark is foreign to Horme's momentum fantasy; a chained swing builds on the same cadence without just restating the bare "extra swing at cadence" signature |
| Halberd | **Polybotes** | every 4th hit ignores armor completely | SWAP: `MarkAllSources25(25)` → `NthHitFullArmorPen(4)` — mark is foreign to Horme; a full-pen finisher at the cadence reads as the crescendo of building momentum |

### Phalanx line (was Pallas — giant-slayer's shield-arm)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bardiche | **Enkelados** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (1s stun, §9) | FIT |
| Halberd | **Eurytos** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) | FIT |

### Zophos line (was Stygian — the fallen giants)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bardiche | **Alkyoneus** | guaranteed lifesteal proc on every crit | FIT (registry: `LifestealOnCrit` carries no on-kill parameter; the old doc's appended "on-kill: full stamina restore" was drift — dropped) |
| Halberd | **Klytios** | on-kill: full stamina and mana restore | FIT (registry: `OnKillRestore(2)` only; the old doc's appended "lifesteal ×2 vs targets below 30% HP" restates the root's Epic-tier base package, not a unique addition — dropped for clarity) |

## 4. Legendary registry (claimed names — review pass merges these)

*Re-theme note (2026-07-07):* all 10 names and ids are unchanged by the per-family root re-theme;
only each line's root and the audited clauses above moved.

Hippolytos, Thoon, Mimas, Porphyrion, Gration, Polybotes, Enkelados, Eurytos, Alkyoneus, Klytios.
(10)

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-base unique names, Uncommon→Epic, same damage anchors)
exist in the approved source spec and will be re-themed to mythic materials in the crafting pass.
Nothing here blocks them.
