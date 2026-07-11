# 07 — Archery (Archery)

**Derives from:** `00-framework.md` (budgets §2, themes §3, effect pattern §4, primitives §5,
ladder §7). Format mirrors `01-axes.md` (canonical format for all family docs).
**Source:** base ratios/speeds per framework §7 "Archery" (exact, do not re-derive). Drop-variant
theme table (§2 below) uses archery-exclusive roots per the 2026-07-07 per-family re-theme
(framework §3) — magnitudes still come from the framework §4 menu, but the roots and lane
identities no longer copy `01-axes.md` verbatim.

---

## 1. Base ladder & damage matrix

Damage = `ratio × D[rarity]`, D = (10, 13, 17, 22, 30). Speed = swing seconds (bigger =
slower). DPS columns are base values before speed effects.

| Base | Ratio | Speed | Common | Uncommon | Rare | Epic | Legendary | Leg. DPS |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Bow | 0.70 | 3.10s | 7.0 | 9.1 | 11.9 | 15.4 | 21.0 | 6.77 |
| Crossbow | 0.80 | 3.40s | 8.0 | 10.4 | 13.6 | 17.6 | 24.0 | 7.06 |
| Heavy crossbow | 0.90 | 3.70s | 9.0 | 11.7 | 15.3 | 19.8 | 27.0 | 7.30 |

Commons are the plain base items: stock name, no hue, no effects.

**Family identity — the ranged safety tax.** Top-of-ladder heavy crossbow lands at 7.30 Legendary
DPS against the cross-family melee parity target of 8.70 (framework §7) — 7.30 / 8.70 ≈ 0.84, i.e.
**~16% under melee parity**, exactly the outlier the framework calls out. This is deliberate, not a
gap to close: archery buys its damage back in range and safety (no reach disadvantage, no counter
from parry/block-on-approach), so its DPS ceiling sits below swords/axes on purpose. Do not raise
archery's ratios or shrink its swing seconds to "catch up" — that decision was made at the
framework level (§7) and is out of scope for this doc to revisit.

## 2. Drop-variant themes — "the far mark" (archery-unique, shared across all 3 bases)

Names read `[root] [base] [rarity]` — e.g. `hekatos crossbow [rare]`. One effect package per root
per rarity; only the damage number differs by base. Roots are archery-only (framework §3,
2026-07-07 re-theme); magnitudes drawn from the framework §4 menu. Epic adds the lane's signature
clause. Hues: placeholder runs until the in-client pass.

| Root | Lane | Uncommon | Rare | Epic (+ signature) |
|---|---|---|---|---|
| **Hekatos** | deadeye | +6% hit chance | +6–8% hit chance, +8% crit chance | +8% hit chance, +10% crit chance, +20% crit damage — **the first shot of any fight is a guaranteed crit** |
| **Belos** | volley | 8% splash on hit (≤3 targets, P26) | 10% splash on hit, +8% damage | 12% splash on hit, +10% damage — **every 4th shot also fires a guaranteed, full-power splash burst** |
| **Toxikon** | toxin | 8% poison apply chance (P10) | 10% poison apply chance, +6% hit chance | 12% poison apply chance, +8% hit chance — **poisoned targets are marked, taking +14% damage from you while poisoned** |
| **Skopos** | warden | +6% hit chance, +6% block | +8% hit chance, +6% block, 8% DR on block | +8% hit chance, +8% block, 12% DR on block — **a successful block guarantees your next shot crits** |
| **Pede** | pin | 4% stagger proc, 1s (P13, §9.2 caps) | 6% stagger proc, +8% damage | 8% stagger proc, +10% damage — **your crits also stagger the target (1s stun, §9.2 caps apply)** |

**Skopos at range reads as deflection, not a riposte.** The block/DR-on-block numbers above apply
identically while the bow or crossbow is held and drawn — Athena's ward doesn't care what's in your
hands. A melee-only thorns/reflect follow-up would need you standing next to what you just blocked,
so it's absent here; instead Skopos converts a block directly into offense at range (the "next-shot
crit" signature above) — archery's Skopos legendaries (§3) are written pure block/DR/stun/crit-on-
block, no reflect clause, to keep this consistent.

## 3. Legendaries (unique per base × theme)

Every legendary = **all Epic effects of its root** + the unique clause below. Damage from §1
Legendary column. Single-click shows the proper noun (`Teukros [legendary]`); base shape appears in
the tooltip. Namespace for this family: archers and arrow-myths (not the theme gods themselves).

**Names and ids are frozen** — the 2026-07-07 re-theme re-points each line to the archery-unique
roots via the lane bijection below and re-audits clauses against the new lane fantasy. Clause text
below restates the **registry (code truth)**; a few pre-retheme doc lines had drifted from the
registry — the registry wording wins. Audit column: FIT = clause kept verbatim (registry-true
wording); SWAP = clause replaced (old → new).

**Lane bijection (old → new):** Zephyr→Belos · Phobos→Hekatos · Agrotera→Toxikon · Pallas→Skopos ·
Stygian→Pede.

### Hekatos line (was Phobos — killing shots)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bow | **Teukros** | the first hit of any fight is a guaranteed crit that also refunds its stamina cost | SWAP: `CritFirstHit` → `CritFirstHitStamRefund` — bare "guaranteed crit on the first hit" is now the Hekatos Epic signature itself; every Hekatos item already has it, so the old clause added no unique value. **Phase-3 flag:** `CritFirstHitStamRefund` is engine-designated signature-only — the stamina-refund half fires only from the signature slot, not a legendary's unique-clause slot, so as Teukros' unique it currently yields a redundant first-hit crit. Kept per doc (it clears the type-level invariant vs the Hekatos `CritFirstHit` signature); recommend either the engine honour the unique slot for this clause, or a re-pick, in a later pass |
| Crossbow | **Pandaros** | your crits deal double damage against full-HP targets | SWAP: `CritFullHpDouble(0,0,1)` → `CritFullHpDouble(0,0,0)` — the `P3=1` "also first hit" component forces a guaranteed crit on the opening shot, duplicating the Hekatos signature; dropped, keeping only the double-damage-vs-full-HP rider |
| Heavy crossbow | **Alkon** | guaranteed crit every 5th hit; crits ignore 10% armor (P27) | FIT |

### Belos line (was Zephyr — swift arrows)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bow | **Skythes** | guaranteed extra shot on the first hit of any fight | FIT (registry: `ExtraSwingFirstHit` carries no stamina-refund parameter; the old doc's "refunds its stamina cost" was drift — dropped) |
| Crossbow | **Molpadia** | guaranteed extra shot every 5th hit | FIT (registry: `ExtraSwingEveryN` carries no mark parameter; the old doc's "applies Huntress' Mark at half bonus" was drift — dropped) |
| Heavy crossbow | **Stymphalia** | guaranteed extra shot every 5th hit; the extra shot splits into a 10% splash across up to 3 targets | FIT (restored the registry's 10% splash magnitude, which the old doc omitted) |

### Toxikon line (was Agrotera — the hunt made mark)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bow | **Skamandrios** | guaranteed mark on the first hit of any fight; marked targets take +25% damage from all sources | FIT |
| Crossbow | **Nessos** | guaranteed mark on the first hit of any fight; your poison ticks on marked targets are doubled | FIT |
| Heavy crossbow | **Penthesileia** | guaranteed mark on the first hit of any fight; up to 3 nearby allies of the target are marked at half bonus | FIT |

Toxikon inherits the Agrotera mark-clause set almost intact — the bijection keeps a mark-flavored
line on a mark-flavored root, so none of these needed swapping; only the root name and the lane's
new signature (poison → mark, rather than first-hit → mark) changed.

### Skopos line (was Pallas — deflection at range)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bow | **Philoktetes** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) | FIT |
| Crossbow | **Kheiron** | guaranteed block vs the first hit of any fight; each block restores 10% stamina (P7) | FIT (registry: `BlockRestoreStam(10)` is a flat 10%; the old doc's "restores stamina equal to its DR" was drift — corrected) |
| Heavy crossbow | **Kydon** | guaranteed block vs the first hit of any fight; each block also drains 3 stamina from the attacker | SWAP: `BlockNextShotCrit` → `BlockDrainStam(3)` — "a block guarantees your next shot crits" is now the Skopos Epic signature itself; swapped to a block-triggered stamina drain so the Warden still punishes the attacker without restating the signature |

### Pede line (was Stygian — the draining arrow)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Bow | **Toxeus** | guaranteed crit every 6th hit (the lane's signature then pins it) | SWAP: `OnKillRestore(2)` → `CritEveryN(6)` — on-kill restore is foreign to Pede's pin identity. NOT `CritStagger`: that is the Pede Epic signature's own clause type (invariant: unique ≠ signature) and the signature already staggers every crit, so the forced-crit cadence IS the unique value |
| Crossbow | **Lerna** | guaranteed crit every 5th hit (the lane's signature then pins it) | SWAP: `LifestealOnCrit` → `CritEveryN(5)` — lifesteal belongs to Zophos' drain fantasy; cadence only, the stagger comes from the lane signature |
| Heavy crossbow | **Krotos** | guaranteed crit every 4th hit (the lane's signature then pins it) | SWAP: `LifestealOnCrit` → `CritEveryN(4)` — lifesteal foreign to Pede; heavy crossbow gets the tightest cadence as the strongest base |

## 4. Legendary registry (claimed names — review pass merges these)

*Re-theme note (2026-07-07):* all 15 names and ids are unchanged by the per-family root re-theme;
only each line's root and the audited clauses above moved.

Skythes, Molpadia, Stymphalia, Teukros, Pandaros, Alkon, Skamandrios, Nessos, Penthesileia,
Philoktetes, Kheiron, Kydon, Toxeus, Lerna, Krotos. (15)

*Merge-pass note (2026-07-07):* Hydra renamed to Lerna (the hydra's swamp) — fencing owns the
monster itself; the venom-soaked-arrow reference survives intact.

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-base unique names, Uncommon→Epic, same damage anchors)
will be re-themed to mythic materials in the crafting pass. Nothing here blocks them.
