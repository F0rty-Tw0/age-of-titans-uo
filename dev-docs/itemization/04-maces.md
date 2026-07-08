# 04 — Maces (Mace Fighting)

**Derives from:** `00-framework.md` (budgets §2, pantheon §3, effect pattern §4, primitives §5,
ladder §7). Mirrors the canonical format set by `01-axes.md`. All damage, speed, and Legendary
DPS numbers are the framework §7 ladder, unchanged — this doc does not re-derive them. Effect
text follows the same primitive substitutions as the axe spec (no movement speed, no cooldowns,
no disarm-immunity).

---

## 1. Base ladder & damage matrix

Damage = `ratio × D[rarity]`, D = (10, 15.6, 27.2, 46.2, 111). Speed = swing seconds (bigger =
slower). DPS columns are base values before speed effects.

| Base | Ratio | Speed | Common | Uncommon | Rare | Epic | Legendary | Leg. DPS |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Club | 0.70 | 2.90s | 7.0 | 10.9 | 19.0 | 32.3 | 77.7 | 26.79 |
| Mace | 0.75 | 3.00s | 7.5 | 11.7 | 20.4 | 34.7 | 83.2 | 27.75 |
| Maul | 0.80 | 3.10s | 8.0 | 12.5 | 21.8 | 37.0 | 88.8 | 28.65 |
| War axe | 0.85 | 3.20s | 8.5 | 13.3 | 23.1 | 39.3 | 94.3 | 29.48 |
| Hammer pick | 0.90 | 3.30s | 9.0 | 14.0 | 24.5 | 41.6 | 99.9 | 30.27 |
| War mace | 0.95 | 3.40s | 9.5 | 14.8 | 25.8 | 43.9 | 105.4 | 31.01 |
| War hammer | 1.00 | 3.50s | 10.0 | 15.6 | 27.2 | 46.2 | 111.0 | 31.71 |

Commons are the plain base items: stock name, no hue, no theme/legendary effects — the crush
identity below still applies; it is baseline, not a theme perk.

**Family identity — crush (all rarities, all bases).** Maces sit under the 32.2 cross-family
melee Legendary DPS parity target (top base, war hammer: 31.71 vs 32.2, ≈1.52% short — framework
§7 sanctions this shortfall for the family). The gap is paid back by a standing perk present on
every mace, at every rarity from Common up, independent of root or theme: **every mace hit also
deals +25% durability damage to the target's worn armor piece and drains 2 stamina from the
target.** This does not scale by rarity, is not part of the §2 effect-package ladder, and always
applies — it stacks under whatever theme or legendary clause is active.

## 2. Drop-variant themes — "storm & anvil" (mace-unique, shared across all 7 bases)

Names read `[root] [base] [rarity]` — e.g. `rhaistes war mace [rare]`. One effect package per root
per rarity; only the damage number differs by base. Roots are mace-only (framework §3, 2026-07-07
re-theme); magnitudes drawn from the framework §4 menu. Epic adds the lane's signature clause.
Hues: placeholder runs until the in-client pass.

Rhaistes' "durability dmg" half of its framework mechanical identity is already the family-wide
crush baseline above (§1, every mace, every rarity) — its ladder below climbs armor pen only, not
a second durability track.

| Root | Lane | Uncommon | Rare | Epic (+ signature) |
|---|---|---|---|---|
| **Ennosigaios** | quake | 8% splash on hit (≤3 targets, P26) | 10% splash on hit, +8% damage | 12% splash on hit, +10% damage — **every 5th hit fires a guaranteed, full-power splash burst (NthHitSplash 5/12/3; the stagger half was trimmed at wiring — stagger is Kataigis' identity)** |
| **Rhaistes** | sunder | +10% armor pen (P27) | +15% armor pen, +8% crit chance | +20% armor pen, +10% crit chance — **every 4th hit ignores armor entirely** |
| **Kataigis** | concussion | 4% stagger proc, 1s (P13, §9.2) | 6% stagger proc, 1s, +8% damage | 8% stagger proc, 1s, +10% damage — **crits stagger the target (1s stun, §9 immunity)** |
| **Eryma** | anvil | +6% block | +6% block, 8% DR on block | +8% block, 12% DR on block, brief thorns after a block — **a successful block grants a brief window of bonus damage reduction (BlockGrantsDrBurst 8%/5s)** |
| **Kamatos** | exhaust | 6% stam regen | 8% stam regen, drain 2 stamina from the target | 10% stam regen, drain 3 stamina from the target — **no event signature** |

*Phase-3 wiring notes (04-maces §2):* Eryma's Epic cell originally read "next swing deals bonus
damage," but the engine's approved on-block clause is `BlockGrantsDrBurst` (a DR window), shared
with Phalanx; the cell was re-worded to match, and the framework §3 "thorns" half of Eryma's
identity was folded into the Epic row. Kamatos: the engine has no "%-based leech-the-target"
field, so "stamina leech" is rendered as `StamRegenPct` (attacker stam on hit) plus
`DefenderStamDrainFlat` (target exhaust); the "bonus damage vs low-stamina targets" signature has
no engine clause, so Kamatos carries no event signature (its Epic capstone is the target
stam-drain, on top of the family-wide crush stamina drain in §1).

Splash/stagger effects reset or cap per framework §9 (splash ≤3 targets, stun procs ≤1s with 10s
immunity, framework §9.2). Signature clauses layer onto the bumped Epic package, per framework §4.

## 3. Legendaries (unique per base × theme)

Every legendary = **all Epic effects of its root** + the unique clause below, plus the standing
crush identity (§1) on every hit. Damage from §1 Legendary column. Single-click shows the proper
noun (`Keraunos [legendary]`); base shape appears in the tooltip.

Namespace for this family: **Cyclopes, Hecatoncheires, storm/thunder implements, and smith-forge
myth** — distinct from the wind/war/hunt/shield/underworld namespace already claimed by
`01-axes.md`.

**Names and ids are frozen** — the 2026-07-07 re-theme re-points each line to the mace-unique
roots via the lane bijection below and re-audits clauses against the new lane fantasy. Clause text
below restates the **registry (code truth)**; several pre-retheme doc lines had drifted from the
registry — invented riders with no encoded parameters (e.g. Briareos' stamina refund,
Chalkoteuchos' lifesteal-×2 rider) — the registry wording wins. A legendary's unique `ClauseType`
must also never equal its own lane's Epic-tier signature type, even at a different cadence — the
archery family's Pede line hit exactly this collision (same type, different N, still double-fires
the same proc on every qualifying hit), so Kataigis below is audited against that rule too. Audit
column: FIT = clause kept verbatim (registry-true wording); SWAP = clause replaced (old → new).

**Lane bijection (old → new):** Zephyr→Ennosigaios · Phobos→Rhaistes · Agrotera→Kataigis ·
Pallas→Eryma · Stygian→Kamatos.

### Ennosigaios line (was Zephyr — the earth-shaker's aftershocks)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Club | **Briareos** | guaranteed extra swing every 5th hit | FIT (registry: `ExtraSwingEveryN(5)` carries no stamina-refund parameter; the old doc's appended "each extra swing also refunds its stamina cost" was drift — dropped) |
| Mace | **Kottos** | guaranteed extra swing on the first hit of any fight | FIT (registry: `ExtraSwingFirstHit` carries no on-kill parameter; the old doc's appended "on-kill: full stamina restore" was drift — dropped) |
| Maul | **Gyges** | guaranteed extra swing every 5th hit; that swing ignores 10% armor (P27) | FIT |
| War axe | **Polyphemos** | guaranteed extra swing every 6th hit; extra swings deal 10% splash (≤3 targets) | FIT (the archetype — `ExtraSwingSplash(6,10,3)` restates verbatim). **Open risk, flag for Phase 2:** if the Ennosigaios Epic-tier signature is wired as `ExtraSwingSplash` too (its "every 5th hit splash+stagger" phrasing is the natural match), this collides with the signature the same way Thyella/Kataigis did below — verify at engine wiring and re-cadence or re-type if so |
| Hammer pick | **Pyrphoros** | guaranteed extra swing every 5th hit; extra swings also leech 5% stamina | FIT |
| War mace | **Elektor** | guaranteed extra swing after a successful parry | FIT (registry: `ExtraSwingOnParry` carries no rider parameter; the old doc's appended "killing blow restores 25 stamina" was drift — dropped) |
| War hammer | **Selaios** | guaranteed extra swing every 7th hit; that swing staggers (1s stun, §9 immunity) | SWAP: `ExtraSwingEveryN(5,1)` → `ExtraSwingEveryN(7,1)` — cadence-5 + stagger duplicated the Ennosigaios Epic signature's own "every 5th hit splash + stagger" phrasing; shifted cadence to the 7th hit to stay distinct |

### Rhaistes line (was Phobos — the smasher's sunder)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Club | **Brontes** | guaranteed crit on the first hit of any fight | SWAP: `CritFirstHit(0,10,3)` → `CritFirstHit(0,0,0)` — the splash rider is Ennosigaios' territory (P26), foreign to Rhaistes' sunder/armor-pen fantasy |
| Mace | **Steropes** | guaranteed crit every 5th hit; crits ignore 10% armor (P27) | FIT (the archetype — `CritArmorPen(5,10)` restates verbatim) |
| Maul | **Arges** | guaranteed crit every 6th hit; crits ignore 15% armor (P27) | SWAP: `CritSplash(6,10,3)` → `CritArmorPen(6,15)` — splash is Ennosigaios' territory, foreign to Rhaistes; params kept distinct from Steropes to vary the ladder |
| War axe | **Pyrakmon** | guaranteed crit every 5th hit; crits deal double damage vs targets below 15% HP | FIT |
| Hammer pick | **Thyella** | guaranteed crit every 7th hit; crits ignore 20% armor (P27) | SWAP: `CritStagger(6)` → `CritArmorPen(7,20)` — `CritStagger` is Kataigis' Epic-tier signature type ("crits stagger"); a Rhaistes legendary carrying it collides with the sibling lane's signature and is foreign to Rhaistes' own fantasy besides, so it's swapped to Rhaistes' own archetype with cadence/params distinct from Steropes and Arges |
| War mace | **Sthenaros** | guaranteed crit every 5th hit | FIT (registry: `CritEveryN(5)` carries no stamina-restore parameter; the old doc's appended "crit kills restore full stamina" was drift — dropped) |
| War hammer | **Keraunos** | guaranteed crit every 8th hit; crits ignore 25% armor (P27) | SWAP: `CritElemental(5,0)` → `CritArmorPen(8,25)` — elemental/lightning proc has no owning lane among the five re-themed maces roots and reads as foreign to Rhaistes' sunder identity; swapped to the line's archetype, escalated for the war-hammer flagship |

Rhaistes never uses `NthHitFullArmorPen` — that type is the natural shape of Rhaistes' own Epic
signature ("every 4th hit ignores armor entirely"); every legendary above uses `CritArmorPen`
instead (partial pen gated behind a crit, not a full-ignore gated behind raw hit count), which is
mechanically distinct from the signature and safe to stack with it.

### Kataigis line (was Agrotera — the tempest's concussion)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Club | **Kelmis** | guaranteed crit every 3rd hit | SWAP: `MarkNearbyAllies(3)` → `CritEveryN(3)` — mark is foreign to a concussion/stagger lane. `CritStagger` itself is reserved for the Epic-tier signature ("crits stagger") — reusing it on a legendary, even at a different cadence, double-fires the stagger proc on every crit (the same collision class the archery family's Pede line hit) — so this and the rest of the line draw on other crit/extra-swing vehicles instead |
| Mace | **Damnameneus** | guaranteed crit every 5th hit; crits deal double damage vs targets below 15% HP | SWAP: `MarkAllSources25(25)` → `CritExecuteUnder15(5)` — mark is foreign to Kataigis' concussion fantasy |
| Maul | **Chalybos** | guaranteed crit on the first hit of any fight | SWAP: `MarkSpreadOnDeath(3)` → `CritFirstHit(0,0,0)` — mark is foreign to Kataigis' concussion fantasy |
| War axe | **Chalkeus** | guaranteed extra swing every 6th hit; that swing staggers (1s stun, §9 immunity) | SWAP: `MarkHealBlock(3)` → `ExtraSwingEveryN(6,1)` — mark/heal-block is foreign to Kataigis; this legendary still carries a literal stagger, routed through the extra-swing trigger (a different event than a landed crit) so it never competes with the Epic-tier signature |
| Hammer pick | **Kabeiros** | every 7th hit strikes twice; the second strike always crits | SWAP: `MarkAllSources25(25)` → `DoubleStrikeEveryN(7)` — mark is foreign to Kataigis' concussion fantasy |
| War mace | **Pyrigenes** | guaranteed crit every 8th hit; crits deal double damage vs full-HP targets | SWAP: `MarkFirstHit` → `CritFullHpDouble(8)` — mark is foreign to Kataigis; the old doc's appended "splash hits from this weapon also mark" was drift, never in the registry parameters — dropped |
| War hammer | **Aitnaios** | guaranteed extra swing every 9th hit; that swing staggers (1s stun, §9 immunity) | SWAP: `PoisonTickDoubled` → `ExtraSwingEveryN(9,1)` — poison/mark is foreign to Kataigis; like Chalkeus, this one gets a genuine stagger via the extra-swing trigger rather than `CritStagger` |

This is the heaviest-swap line in the family (7 of 7) — old-Agrotera's mark clauses have no home
in a concussion/stagger lane, and `CritStagger` itself is off-limits at legendary tier because
Kataigis' own Epic signature already owns it.

### Eryma line (was Pallas — the bulwark's answer)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Club | **Khalkaspis** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) | FIT |
| Mace | **Chalkodamas** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) | FIT |
| Maul | **Akmon** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (1s stun, §9) | FIT |
| War axe | **Akmonides** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) | FIT |
| Hammer pick | **Skeptron** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (1s stun, §9) | FIT |
| War mace | **Adamastos** | guaranteed block vs the first hit of any fight; a successful block also drains 2 stamina from the attacker | FIT |
| War hammer | **Ombrios** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (1s stun, §9) | FIT |

Every entry in this line is FIT — `BlockFirstHit`, `ReflectFirstHit`, and `BlockDrainStam` are all
tagged as the Pallas/block-family clause group in the registry's own `ClauseType` organization, and
carry over to Eryma without alteration. None resembles the Eryma signature's "next swing after a
block" shape, so there's no type-collision risk to flag here.

### Kamatos line (was Stygian — toil unto collapse)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Club | **Chalkoteuchos** | crits drain the target's stamina fully | FIT (registry: `StamDrainOnCrit` carries no rider parameter; the old doc's appended "lifesteal ×2 vs targets below 30% HP" restates the root's Epic-tier base package, not a unique addition — dropped for clarity) |
| Mace | **Sphyreus** | on-kill: full stamina and mana restore | FIT (registry: `OnKillRestore(2)` only; the old doc's appended "lifesteal ×2 vs targets below 30% HP" restates the root's Epic-tier base package — dropped for clarity) |
| Maul | **Empyros** | guaranteed lifesteal proc on every crit | FIT (registry: `LifestealOnCrit` carries no on-kill parameter; the old doc's appended "on-kill: full stamina restore" was drift — dropped) |
| War axe | **Astrapios** | guaranteed lifesteal proc on every crit | FIT (registry: `LifestealOnCrit` carries no on-kill parameter; the old doc's appended "on-kill: full stamina restore" was drift — dropped) |
| Hammer pick | **Brontaios** | on-kill: full stamina and mana restore | FIT (registry: `OnKillRestore(2)` only; the old doc's appended "lifesteal ×2 vs targets below 30% HP" restates the root's Epic-tier base package — dropped for clarity) |
| War mace | **Aitherios** | guaranteed lifesteal proc on every crit | FIT (registry: `LifestealOnCrit` carries no on-kill parameter; the old doc's appended "on-kill: full stamina restore" was drift — dropped) |
| War hammer | **Pyriphaes** | on-kill: full stamina and mana restore | FIT (registry: `OnKillRestore(2)` only; the old doc's appended "lifesteal ×2 vs targets below 30% HP" restates the root's Epic-tier base package — dropped for clarity) |

`StamDrainOnCrit` is Kamatos' archetype fit per its stated identity (stam leech + attacker stam
drain, P7). `LifestealOnCrit` is HP-based (P6), not stam-based (P7) — kept FIT because the
registry's own `ClauseType` comments group it under "Drain family (Stygian)" alongside
`StamDrainOnCrit`/`OnKillRestore`, and the names are frozen; flagged below as a judgment call. None
of the three types used resembles the Kamatos signature's "bonus dmg vs low stamina" shape, so
there's no type-collision risk here either.

## 4. Legendary registry (claimed names — review pass merges these)

*Re-theme note (2026-07-07):* all 35 names and ids are unchanged by the per-family root re-theme;
only each line's root and the audited clauses above moved.

Briareos, Kottos, Gyges, Polyphemos, Pyrphoros, Elektor, Selaios, Brontes, Steropes, Arges,
Pyrakmon, Thyella, Sthenaros, Keraunos, Kelmis, Damnameneus, Chalybos, Chalkeus, Kabeiros,
Pyrigenes, Aitnaios, Khalkaspis, Chalkodamas, Akmon, Akmonides, Skeptron, Adamastos, Ombrios,
Chalkoteuchos, Sphyreus, Empyros, Astrapios, Brontaios, Aitherios, Pyriphaes. (35)

*Merge-pass note (2026-07-07):* Typhon→Thyella (the squall; fencing owns the monster) and
Enkelados→Kabeiros (forge-daimon; polearms owns the giant).

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-base unique names, Uncommon→Epic, same damage anchors)
will be authored in the crafting pass, themed to mythic smithing materials (adamant, orichalcum,
Cretan bronze per framework §11). Nothing here blocks them.
