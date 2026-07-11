# 05 — Staves (Mace Fighting, Caster Hybrid)

**Derives from:** `00-framework.md` (budgets §2, themes §3, effect pattern §4, primitives §5, base
ladder §7, balance rules §9). Format mirrors `01-axes.md` (canonical family doc structure).

---

## 1. Base ladder & damage matrix

Damage = `ratio × D[rarity]`, D = (10, 13, 17, 22, 30), rounded to 1 decimal. Speed = swing
seconds (bigger = slower). Leg. DPS is computed from the unrounded legendary damage value, 2
decimals — this reproduces the three Leg. DPS figures fixed in framework §7 exactly.

| Base | Ratio | Speed | Common | Uncommon | Rare | Epic | Legendary | Leg. DPS |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Quarter staff | 0.65 | 2.70s | 6.5 | 8.5 | 11.1 | 14.3 | 19.5 | 7.22 |
| Gnarled staff | 0.72 | 2.85s | 7.2 | 9.4 | 12.2 | 15.8 | 21.6 | 7.58 |
| Black staff | 0.80 | 3.00s | 8.0 | 10.4 | 13.6 | 17.6 | 24.0 | 8.00 |

Commons are the plain base items: stock name, no hue, no effects.

**Family identity — the caster lean.** Staves run ~8% under melee DPS parity (framework §7:
8.70 ±3% target at top base; black staff lands at 8.00, ≈8% under). The gap is deliberate, not a
shortfall: every staff drop-variant, regardless of root, additionally carries a flat mana-regen
rider on top of its theme package — **+6% (Uncommon), +8% (Rare), +12% (Epic and Legendary)** mana
regen. Framework §7 explicitly sanctions staves as a DPS outlier for this reason; the missing
damage is spent on caster sustain instead.

## 2. Drop-variant themes — "the seer's rod" (staves-unique, shared across all 3 bases)

Names read `[root] [base] [rarity]` — e.g. `prester gnarled staff [rare]`. Roots are staves-only
(framework §3, 2026-07-07 re-theme): each lane's primary primitive and magnitudes are drawn from
the framework §4 menu, not copied from another family. The universal mana-regen rider (§1) stacks
underneath every row below. Staves' worn-side utility values (spell DR% / mana regen% / dodge% on
a held weapon) come from the menu's "weapon worn-side utility" row (2/8/2, 3/12/3, 5/18/4) —
Alexikakos draws its spell-DR column from there, Manteia draws its mana-regen column from there.

| Root | Lane | Uncommon | Rare | Epic (+ signature) |
|---|---|---|---|---|
| **Empousa** | siphon | 6% mana leech (P8) | 8% mana leech, +8% crit chance | 10% mana leech, +10% crit chance — **crits trigger a burst mana-leech (double the normal %)** |
| **Prester** | storm | 6% elemental proc chance (P24) | 8% elemental proc, +8% crit chance | 10% elemental proc, +10% crit chance — **no event signature; the "proc chance up" is the 6→8→10% ramp, fixed to lightning** |
| **Manteia** | oracle | 8% mana regen (worn-side row) | 12% mana regen, +6% hit chance | 18% mana regen, +8% hit chance, boosted heals-received + auto-cure tick — **no event signature (numeric row fields)** |
| **Alexikakos** | ward | 2% spell DR (worn-side row) | 3% spell DR, +6% hit chance | 5% spell DR, +8% hit chance, +5 Resisting Spells (P19) — **the first spell cast against you each fight is heavily DR'd** |
| **Baskania** | curse | +6% lifesteal | +6% lifesteal, 6% heal-block proc chance (P25, §9.3 — no Uncommon value on the menu, so the proc itself doesn't unlock until Rare) | +8% lifesteal, 8% heal-block proc chance — **crits heal-block the target** |

| Rider (all roots) | Uncommon | Rare | Epic / Legendary |
|---|---|---|---|
| **Mana regen** | +6% | +8% | +12% |

*Phase-3 wiring notes (05-staves §2):* Prester's "alternates fire/lightning" has no engine hook —
`ElementalKind` is a fixed element with no alternation flag — so the proc is served as fixed
lightning, and the "proc chance up" is captured by the numeric `ElementalProcPct` ramp (no event
signature). Manteia's "+5 Meditation" is dropped: the only wired skill-mod field is Resisting
Spells (`ResistSkillBonus`), which is Alexikakos' identity; Manteia's Epic capstone is the numeric
`HealsReceivedPct` + `AutoCure` fields (no event signature). The universal §1 mana-regen rider is
folded into each root's `ManaRegenPct` (so Manteia's Epic reads 18 lane + 12 universal = 30%).

## 3. Legendaries (unique per base × theme)

3 bases × 5 themes = 15 legendaries. Every legendary = **all Epic effects of its root** + **the
mana-regen rider at Epic/Legendary strength (+12%)** + one unique clause below. Damage from §1
Legendary column. Clause text below restates the **registry (code truth)**; several pre-retheme
doc lines had invented poison/effect detail the registry never carried (flagged per line) — the
registry wording wins. Audit column: FIT = clause kept verbatim (reworded only where the old prose
had drifted from the registry); SWAP = clause replaced (old → new).

**Lane bijection (old → new):** Zephyr→Empousa · Phobos→Prester · Agrotera→Manteia ·
Pallas→Alexikakos · Stygian→Baskania.

Namespace for this family (unchanged by the re-theme): Greek seers, sorcerers, and mystics only —
no wind-spirits, war-daimones, or Olympian epithets (those belong to other weapon families).

### Empousa line (was Zephyr — mana drawn from a shade's grasp)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Quarter staff | **Teiresias** | guaranteed extra swing every 5th hit; each extra swing also leeches 5% of the target's mana to the wielder (P8) | FIT |
| Gnarled staff | **Kalchas** | guaranteed extra swing on the first hit of any fight; that swing also leeches 8% of the target's mana (P8) | SWAP: ExtraSwingElemental(0, lightning) → ExtraSwingManaLeech(0, 8) — elemental proc belongs to Prester's storm fantasy, not Empousa's siphon |
| Black staff | **Amphiaraos** | guaranteed extra swing every 5th hit; that swing also leeches 10% of the target's mana (P8) | SWAP: ExtraSwingHealBlock(5, 3) → ExtraSwingManaLeech(5, 10) — heal-block belongs to Baskania's curse fantasy |

### Prester line (was Phobos — the storm's circle)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Quarter staff | **Kassandra** | guaranteed elemental (lightning) proc on the first hit of any fight, and on every natural crit thereafter (P24) | SWAP: CritManaLeech(0, 8, 1) → CritElemental(0, 0, 1) — mana leech is Empousa's fantasy, not Prester's storm |
| Gnarled staff | **Mopsos** | guaranteed crit every 5th hit; crits carry a fire proc for flat damage (P24) | FIT |
| Black staff | **Melampos** | guaranteed crit every 6th hit; crits carry a lightning proc for flat damage (P24) | SWAP: CritHealBlock(6, 3) → CritElemental(6, 0) — heal-block belongs to Baskania's curse fantasy |

### Manteia line (was Agrotera — the oracle's mercy)

None of Agrotera's old mark-family riders (mana leech / elemental / heal-block) carry any
mana-regen, healing, or skill-bonus content — Manteia's oracle fantasy is entirely new for this
base tier, so all three swap to the Paean auto-cure family (already in the ClauseType catalog,
reused here rather than invented fresh).

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Quarter staff | **Medeia** | the wielder's auto-cure tick also restores 10% stamina and mana | SWAP: MarkManaLeech(5) → AutoCureRestoresStamMana(10) — mana leech is Empousa's fantasy |
| Gnarled staff | **Kirke** | the wielder's auto-cure tick also restores 8% missing HP | SWAP: MarkElemental(lightning) → AutoCureRestoresHpPct(8) — elemental proc is Prester's fantasy |
| Black staff | **Phineus** | once per fight, dropping below 30% HP triggers a free cure and restores 15% HP | SWAP: MarkHealBlockFirstHit(3) → LowHpEmergencyCure(30, 15) — heal-block is Baskania's fantasy |

### Alexikakos line (was Pallas — the ward that turns evil aside)

Same situation as Manteia: Pallas's old block-family riders carry mana-leech/elemental/heal-block
content, none of it ward/spell-DR flavored. Swaps pull from the Tritonian ward-rider family
(already in the catalog for armor's Poseidon lane — same "averter of evil" fantasy).

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Quarter staff | **Orpheus** | resisting a paralyze raises spell DR to 10% for 5s (P19/P20) | SWAP: BlockManaLeech(10) → ParaResistBoostsSpellDr(10, 5) — mana leech is Empousa's fantasy |
| Gnarled staff | **Polyeidos** | spell DR doubles for 6s after taking a crit | SWAP: BlockElemental(fire) → SpellDrBurstOnCritTaken(6) — elemental proc is Prester's fantasy |
| Black staff | **Helenos** | the first paralyze/stun attempt of any fight automatically fails | SWAP: ReflectHealBlock(20, 3) → FirstParaAutoFails — heal-block is Baskania's fantasy, and a flat reflect-% doesn't showcase the ward/spell-DR identity either |

### Baskania line (was Stygian — the evil eye lingers)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Quarter staff | **Manto** | guaranteed mark on every crit; marked targets are heal-blocked for 3s (P25 cap) | SWAP: CritManaLeech(5, 8) → MarkHealBlock(3) — mana leech is Empousa's fantasy |
| Gnarled staff | **Idmon** | guaranteed mark on the first hit of any fight; the mark heal-blocks its target for 3s (P25 cap) | SWAP: CritElemental(5, lightning) → MarkHealBlockFirstHit(3) — elemental proc is Prester's fantasy |
| Black staff | **Theoklymenos** | the first hit landed each fight heal-blocks its target for 3s (P25 cap) | SWAP: CritHealBlock(5, 3) → HealBlockOnFirstHitLanded(3) — duplicates Baskania's own Epic signature ("crits heal-block") verbatim; re-paramed to a first-hit-landed trigger instead of every natural crit |

## 4. Legendary registry (claimed names — review pass merges these)

*Re-theme note (2026-07-07):* all 15 names and ids are unchanged by the per-family root re-theme;
only each line's root (Zephyr→Empousa, Phobos→Prester, Agrotera→Manteia, Pallas→Alexikakos,
Stygian→Baskania) and the audited clauses above moved. 10 of 15 clauses swapped — the old base-tier
rider pattern (mana leech / elemental / heal-block, keyed by base) survives largely intact under
Empousa and Prester (the two lanes whose new fantasy overlaps that pattern) but had to be fully
replaced under Manteia and Alexikakos (two brand-new caster fantasies with no old-line analog) and
partly replaced under Baskania (its own signature duplicate).

Teiresias, Kalchas, Amphiaraos, Kassandra, Mopsos, Melampos, Medeia, Kirke, Phineus, Orpheus,
Polyeidos, Helenos, Manto, Idmon, Theoklymenos. (15)

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-base unique names, Uncommon→Epic, same damage anchors)
exist in the approved source spec and will be re-themed to mythic materials in the crafting pass.
Nothing here blocks them.
