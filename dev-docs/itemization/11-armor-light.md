# 11 — Light Armor (Leather / Studded / Bone)

**Derives from:** `00-framework.md` (budgets §2, ARMOR themes §3, primitives §5, armor ladder §7,
balance rules §9). Format follows `02-swords.md` / `01-axes.md` (canonical family-doc structure).
**Lane:** Light armor is the mage/dex-suit lane — no meditation penalty on leather, low weight
across the family.

---

## 1. AR matrix (one table per material)

AR = `material ratio × slot weight × A[rarity]`, rounded to nearest integer. A = (12, 14, 17, 21,
26) for (Common, Uncommon, Rare, Epic, Legendary), per framework §2. Slot weights: chest 1.00 ·
legs .70 · helm .50 · arms .40 · gorget .25 · gloves .25 (framework §7).

### Leather — ratio .65

Real T2A shapes: cap, gorget, gloves, arms, legs, tunic (chest slot).

| Piece | Slot weight | Common | Uncommon | Rare | Epic | Legendary |
|---|---:|---:|---:|---:|---:|---:|
| Tunic (chest) | 1.00 | 8 | 9 | 11 | 14 | 17 |
| Legs | .70 | 5 | 6 | 8 | 10 | 12 |
| Cap (helm) | .50 | 4 | 5 | 6 | 7 | 8 |
| Arms | .40 | 3 | 4 | 4 | 5 | 7 |
| Gorget | .25 | 2 | 2 | 3 | 3 | 4 |
| Gloves | .25 | 2 | 2 | 3 | 3 | 4 |

Female body-style variants (e.g. leather bustier) are the same slot weight and share every AR
value above — cosmetic shape only.

### Studded — ratio .72

Real T2A shapes: tunic (chest slot), legs, arms, gloves, gorget. No studded helm/cap exists in
T2A — this material has no helm row.

| Piece | Slot weight | Common | Uncommon | Rare | Epic | Legendary |
|---|---:|---:|---:|---:|---:|---:|
| Tunic (chest) | 1.00 | 9 | 10 | 12 | 15 | 19 |
| Legs | .70 | 6 | 7 | 9 | 11 | 13 |
| Arms | .40 | 3 | 4 | 5 | 6 | 7 |
| Gorget | .25 | 2 | 3 | 3 | 4 | 5 |
| Gloves | .25 | 2 | 3 | 3 | 4 | 5 |

### Bone — ratio .79

Real T2A shapes: helmet, arms, legs, gloves, tunic (chest slot). No bone gorget exists in T2A —
this material has no gorget row.

| Piece | Slot weight | Common | Uncommon | Rare | Epic | Legendary |
|---|---:|---:|---:|---:|---:|---:|
| Tunic (chest) | 1.00 | 9 | 11 | 13 | 17 | 21 |
| Legs | .70 | 7 | 8 | 9 | 12 | 14 |
| Helmet (helm) | .50 | 5 | 6 | 7 | 8 | 10 |
| Arms | .40 | 4 | 4 | 5 | 7 | 8 |
| Gloves | .25 | 2 | 3 | 3 | 4 | 5 |

**Orc helm** is a bone-helm shape variant (same base item, alternate look) — it uses the Bone
Helmet row above unchanged (5 / 6 / 7 / 8 / 10).

Commons are the plain base items: stock name, no hue, no effects.

## 2. Drop-variant lanes — five roots per material

**2026-07-07 re-theme.** Light-armor roots are now **per material** (framework §3): leather, studded
and bone each field their own five roots. Names read `[root] [material] [piece] [rarity]` — e.g.
`melissa leather tunic [rare]`, `arkas studded gloves [epic]`. One effect package per root per rarity;
only the AR number (§1) differs by material/piece. Hues: material run from framework §3, shaded by
rarity.

**Magnitudes are carried over, not re-derived.** Every per-rarity number below re-uses a retired
shared-table scale per primitive (armor magnitudes did not change in the re-theme — they re-mix):

| Primitive | Uncommon | Rare | Epic | Legendary | Scale source |
|---|---|---|---|---|---|
| Bonus AR | +1 | +2 | +3 | +4 | retired Polias |
| Damage reduction | +1% | +2% | +2% | +3% | retired Polias |
| Shrug (halve one blow) | 3%* | 4%* | 5% | 8% | retired Polias (E/L 5/8; U/R extended*) |
| Thorns / reflect | 2% | 3% | 5% | 6% | retired Cyclopean |
| HP regen | +8% | +12% | +18% | +25% | retired Paean |
| Heals received | — | +5% | +8% | +10% | retired Paean |
| Auto-cure tick | — | — | 5% periodic | yes | retired Paean |
| Spell DR | 2% | 3% | 5% | 6% | retired Tritonian |
| Para/stun resist | 10% | 15% | 20% | 30% | retired Tritonian |
| Weight reduction | −10% | −20% | −30% | −40% | retired Talarian |
| Stam regen | +4% | +6% | +8% | +10% | retired Talarian |
| Dodge | 3%* | 4%* | 5% | 8% | retired Talarian (E/L 3/5; U/R extended for dodge-primary lanes*, top bumped) |

New primitives with **no** legacy armor scale are placeholders (tune live): poison resist
5/8/10/12%, Hiding skill bonus +2/+3/+5/+5, on-kill stam 5/8/10/15% max, on-kill HP 3/5/8/10% max.
Riders drawn from the P3b jewelry/clothing pool (marked †) are nearest-fit until an armor-native
equivalent is wired.

**Epic rider (bolded)** is the lane's signature clause (an existing armor rider ClauseType, framework
§5), carried at Epic **and** Legendary — same shape swords use for their Epic signature. The
Legendary column is a step **above** Epic (framework §4 armor rule) and adds the §3 unique clause
on top of the Epic package.

> **Stacking & caps.** Per-material roots stack by mechanic **StackGroup** (framework §9.4:
> strongest instance full, the rest at 50%), enforcing the §9.8 shared defensive pools across
> everything worn — armor + shield + clothing together: bonus AR ≤ 15 · DR ≤ 12% · shrug ≤ 20% ·
> thorns/reflect ≤ 25% · HP regen ≤ 60% · spell DR ≤ 18% · dodge ≤ 12%. Tune live.

### Leather — "the nymph's hide" (ratio .65)

| Root | Lane | Uncommon | Rare | Epic (+ signature) | Legendary |
|---|---|---|---|---|---|
| **Naias** | mending (HP regen + heals received) | +8% HP regen | +12% HP regen, +5% heals received | +18% HP regen, +8% heals received — **once per fight, a near-lethal hit fires a full regen tick instead (EmergencyRegenTick)** | +25% HP regen, +10% heals received + unique (§3) |
| **Dryas** | briar (thorns + poison resist) | 2% reflect, 5% poison resist | 3% reflect, 8% poison resist | 5% reflect, 10% poison resist — **spell DR also applies to poison damage-over-time ticks (SpellDrVsPoisonDot)** | 6% reflect, 12% poison resist + unique (§3) |
| **Oreias** | stride (weight + stam regen) | −10% weight, +4% stam regen | −20% weight, +6% stam regen | −30% weight, +8% stam regen — **on-kill: full stamina restore + extended stun immunity (OnKillStamRestoreExtendImmunity)** | −40% weight, +10% stam regen + unique (§3) |
| **Melissa** | balm (auto-cure + heals received) | +5% heals received | 5% periodic auto-cure, +5% heals received | auto-cure tick, +8% heals received — **the auto-cure tick also clears mark/heal-block, once per fight (AutoCureClearsDebuffsOnce)** | auto-cure tick, +10% heals received + unique (§3) |
| **Panika** | startle (dodge) | 3% dodge | 4% dodge | 5% dodge — **dodge chance doubles vs the first attack of any fight (DodgeDoubleFirstAttack, P14)** | 8% dodge + unique (§3) |

### Studded — "the hunter's brand" (ratio .72)

| Root | Lane | Uncommon | Rare | Epic (+ signature) | Legendary |
|---|---|---|---|---|---|
| **Kynegis** | chase (dodge + stam regen) | 3% dodge, +4% stam regen | 4% dodge, +6% stam regen | 5% dodge, +8% stam regen — **a successful dodge grants +stam regen for a few seconds (DodgeRegenBurst)** | 8% dodge, +10% stam regen + unique (§3) |
| **Batos** | bramble (thorns) | 2% reflect | 3% reflect | 5% reflect — **reflect % rises vs the first attacker of any fight (ReflectBoostFirstHit, P14)** | 6% reflect + unique (§3) |
| **Arkas** | bear (bonus AR + shrug) | +1 bonus AR | +2 bonus AR, 3% shrug | +3 bonus AR, 5% shrug — **the first blow of each fight is always shrugged (ShrugFirstHitGuaranteed)** | +4 bonus AR, 8% shrug + unique (§3) |
| **Elaphis** | deer (weight + dodge) | −10% weight, 3% dodge | −20% weight, 4% dodge | −30% weight, 5% dodge — **a successful dodge also restores a little mana (DodgeRestoreMana)** | −40% weight, 8% dodge + unique (§3) |
| **Skia** | shadow (poison resist + Hiding skill) | 5% poison resist, +2 Hiding | 8% poison resist, +3 Hiding | 10% poison resist, +5 Hiding — **poison resist doubles while hidden (PoisonResistDoubleWhileHidden†, placeholder)** | 12% poison resist, +5 Hiding + unique (§3) |

### Bone — "the grave-warden" (ratio .79)

| Root | Lane | Uncommon | Rare | Epic (+ signature) | Legendary |
|---|---|---|---|---|---|
| **Melinoe** | phantom (dodge + para resist) | 3% dodge, 10% para resist | 4% dodge, 15% para resist | 5% dodge, 20% para resist — **a successful dodge opens a brief counter window (DodgeGrantsCounterWindow, new — placeholder)** | 8% dodge, 30% para resist + unique (§3) |
| **Makaria** | blessed death (on-kill restores) | on-kill: 5% max stam | on-kill: 8% max stam | on-kill: 10% max stam — **on-kill: restore max HP % (OnKillRestoreHpPct, placeholder scale)** | on-kill: 15% max stam + unique (§3) |
| **Tymbos** | tomb (bonus AR + DR) | +1 bonus AR, +1% DR | +2 bonus AR, +2% DR | +3 bonus AR, +2% DR — **spell DR doubles for 3s after taking a crit — the barrow hardens when struck (SpellDrBurstOnCritTaken)** | +4 bonus AR, +3% DR + unique (§3) |
| **Nekyia** | death-ward (spell DR) | 2% spell DR | 3% spell DR | 5% spell DR — **spell DR rises vs the first spell of each fight (SpellDrBoostFirstHit)** | 6% spell DR + unique (§3) |
| **Katachthon** | grave-thorns (thorns/reflect) | 2% reflect | 3% reflect | 5% reflect — **reflect % rises vs the first attacker of any fight (ReflectBoostFirstHit, P14)** | 6% reflect + unique (§3) |

> **Rider-pool note.** The armor rider pool is shallow by mechanic (three shrug clauses, a handful
> of dodge and thorns riders), so signatures recur across lanes; within any one lane the Epic rider
> never equals that lane's §3 unique clause (framework §4 invariant). Skia's Hiding-skill primitive
> and the on-kill HP/stam scales are new — placeholder magnitudes, tune live. Final assignment is a
> live-tune target.

**Design note:** light armor is the mage/dex-suit lane. Leather leans mending/dodge (Naias, Panika,
Melissa), studded leans the hunt (Kynegis, Skia dodge/poison/hide), bone trades some of that lane
back for raw AR and grave-defensive utility (Tymbos, Nekyia). Low base weight on leather/studded
stacks naturally with casters and dexers alike.

## 3. Legendaries (unique per material × theme)

Every legendary = **all Epic effects of its (new) root** (§2) + the unique clause below. AR from §1
Legendary column for the material. A legendary drops as a random slot piece of its material (e.g.
*Nemea* may drop as a cap, gorget, gloves, arms, legs, or tunic) — the proper noun is fixed to
material × theme, not to a specific slot. Single-click shows the proper noun (`Nemea [legendary]`);
the base piece shape appears in the tooltip.

**2026-07-07 re-theme.** All 15 names and ids are **frozen** (registry is code truth,
`LegendaryRegistry.cs` ids 201–215). Only each line's **root** moved, via the fixed per-material
bijection, and each clause is re-audited against the new lane fantasy. **Audit column:** FIT = the
frozen registry clause fits the new lane, kept verbatim; SWAP = the clause is foreign to the new
lane (or would duplicate the lane's Epic rider) and is re-pointed (old → new) for the engine pass.

**Light bijection (old root → new root, per material):**
- **Leather:** Polias→Naias · Cyclopean→Dryas · Paean→Melissa · Tritonian→Panika · Talarian→Oreias
- **Studded:** Polias→Arkas · Cyclopean→Batos · Paean→Elaphis · Tritonian→Skia · Talarian→Kynegis
- **Bone:** Polias→Tymbos · Cyclopean→Katachthon · Paean→Makaria · Tritonian→Nekyia · Talarian→Melinoe

### Leather (ids 201, 204, 207, 210, 213)

| New root | Name | Unique clause | Audit |
|---|---|---|---|
| **Naias** | **Nemea** (201) | HP regen triples for a few seconds after taking a crit — the spring wells up when you are hurt (HpRegenBurstOnCritTaken) | SWAP: ShrugFirstHitGuaranteed → HpRegenBurstOnCritTaken — a shrug clause is foreign to the HP-regen mending lane |
| **Dryas** | **Teumessos** (204) | reflect % rises vs the first attacker of any fight (ReflectBoostFirstHit, P14) | SWAP: FlameProcDoubleFirstHit → ReflectBoostFirstHit — a forge flame-proc is foreign to the thorns + poison-resist briar |
| **Melissa** | **Kyrene** (207) | the auto-cure tick also restores 5% of missing HP when it fires (AutoCureRestoresHpPct) | FIT — an auto-cure rider on the auto-cure + heals-received balm (exemplar-perfect) |
| **Panika** | **Arethousa** (210) | a successful dodge opens a brief counter window (DodgeGrantsCounterWindow) | SWAP: FirstParaAutoFails → DodgeGrantsCounterWindow — a para-resist clause is foreign to the pure-dodge startle lane; dodge→counter is the startle fantasy |
| **Oreias** | **Kyllene** (213) | stamina-regen ticks also refresh mana — the tireless stride (StamRegenMirrorsManaHalf) | SWAP: DodgeDoubleFirstAttack → StamRegenMirrorsManaHalf — the stride lane carries no dodge trigger; a stamina rider fits weight + stam regen |

### Studded (ids 202, 205, 208, 211, 214)

| New root | Name | Unique clause | Audit |
|---|---|---|---|
| **Arkas** | **Kithairon** (202) | every shrugged blow briefly stuns the attacker (1s, §9) — (ShrugStunAttacker) | FIT — shrug-stun on the AR + shrug bear lane |
| **Batos** | **Khimaira** (205) | reflecting a crit briefly stuns the attacker (ReflectCritStun) | SWAP: FlameProcPoison → ReflectCritStun — a flame-proc is foreign to the pure-thorns bramble; a thorns rider fits |
| **Elaphis** | **Daphne** (208) | weight reduction applies suit-wide for a few seconds after a dodge (WeightReductionSuiteBurstOnDodge) | SWAP: LowHpEmergencyCure → WeightReductionSuiteBurstOnDodge — an emergency-cure is foreign to the weight + dodge deer; the hind's lightness on a leap fits |
| **Skia** | **Skylla** (211) | spell DR also applies to poison damage-over-time ticks (SpellDrVsPoisonDot) | FIT — poison-DoT resistance on the poison-resist + Hiding shadow lane |
| **Kynegis** | **Melanippe** (214) | a successful dodge also refunds 15 stamina (DodgeRefundStam, P7) | FIT — dodge-refunds-stam on the dodge + stam-regen chase lane |

### Bone (ids 203, 206, 209, 212, 215)

| New root | Name | Unique clause | Audit |
|---|---|---|---|
| **Tymbos** | **Erymanthos** (203) | the first blow taken each fight applies no secondary effect (FirstHitNoSecondaryEffect) | SWAP: ShrugReflect → FirstHitNoSecondaryEffect — a shrug/reflect clause belongs to the grave-thorns lane; a deadening nullify fits the AR + DR tomb |
| **Katachthon** | **Echidna** (206) | reflecting a crit briefly stuns the attacker (ReflectCritStun) | SWAP: FlameProcSplash → ReflectCritStun — a flame-proc is foreign to the thorns/reflect grave-thorns; a thorns rider fits |
| **Makaria** | **Keryneia** (209) | once per fight, if a hit would drop the wearer near death, the regen tick fires immediately for its full amount (EmergencyRegenTick) | FIT — cheating death reads cleanly on Makaria (Hades' daughter, blessed death); the death-defiance carries the off-package regen |
| **Nekyia** | **Krommyon** (212) | the first failed para/stun resist roll each fight is rerolled once (RerollFirstResist) | FIT (borderline) — the death-ward's anti-magic identity carries a resist-reroll; both are anti-magic defence |
| **Melinoe** | **Kalydon** (215) | a successful dodge grants +stam regen for 3 seconds (DodgeRegenBurst, P7) | FIT — a dodge rider on the dodge + para-resist phantom lane |

## 4. Legendary registry (claimed names — review pass merges these)

Nemea, Kithairon, Erymanthos, Teumessos, Khimaira, Echidna, Kyrene, Daphne, Keryneia, Arethousa,
Skylla, Krommyon, Kyllene, Melanippe, Kalydon. (15)

*Re-theme note (2026-07-07):* all 15 names and ids (registry 201–215) are **unchanged** by the
per-material root re-theme — only each line's root re-pointed to the new material-specific lane (per
the §3 bijection) and the audited clauses moved. 8 clauses SWAP (Nemea, Teumessos, Arethousa,
Kyllene; Khimaira, Daphne; Erymanthos, Echidna), 7 FIT.

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-base unique names, Uncommon→Epic, same AR anchors) exist
in the approved source spec and will be re-themed to mythic materials in the crafting pass.
Nothing here blocks them.
