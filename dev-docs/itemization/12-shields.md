# 12 — Shields

**Derives from:** `00-framework.md` (budgets §2, themes §3 — shields carry five shield-only roots,
Athena keeps **Aegis** — primitives §5, shield ladder §7, balance rules §9). Format follows
`01-axes.md`.
**Source spec:** team-lead itemization brief (2026-07-07), shield family pass. **Re-themed**
2026-07-07 per the per-family root directive (00-framework.md §3): the four non-Aegis roots are
re-pointed from shared-god names (Cyclopean/Paean/Tritonian/Talarian) to shield-only roots
(Amyntor/Pnoe/Herkos/Probolos); Aegis is restated unchanged. Names and ids in §3/§4 are frozen —
only each line's root and (where audited) its unique clause moved.

---

## 1. Base ladder & AR matrix

AR = `ratio × S[rarity]`, S = (8, 10, 12, 15, 20), rounded to integer. Single worn slot — no
speed/swing axis, one shape column only.

| Shape | Ratio | Common | Uncommon | Rare | Epic | Legendary |
|---|---:|---:|---:|---:|---:|---:|
| Buckler | 0.60 | 5 | 6 | 7 | 9 | 12 |
| Wooden shield | 0.68 | 5 | 7 | 8 | 10 | 14 |
| Wooden kite | 0.76 | 6 | 8 | 9 | 11 | 15 |
| Metal shield | 0.84 | 7 | 8 | 10 | 13 | 17 |
| Metal kite | 0.92 | 7 | 9 | 11 | 14 | 18 |
| Heater | 1.00 | 8 | 10 | 12 | 15 | 20 |

Sanity: Common/Legendary heater AR = 8 / 20 — exact match to the S[rarity] anchors themselves
(ratio 1.00), same identity check axes.md ran on the ornate axe.

Commons are the plain base items: stock name, no hue, no effects.

## 2. Drop-variant themes (shared across all 6 shapes)

Names read `[root] [shape] [rarity]` — e.g. `amyntor metal kite [rare]`. One effect package per
root per rarity; only the AR number differs by shape. Roots are shield-only (framework §3,
2026-07-07 re-theme); magnitudes keep the **retired shared tables' own shield scale** per
primitive — shields run ~2× an armor piece and carry their own Legendary-above-Epic step (per
framework §4's armor/shield exception — this is not a deviation to flag, it's the documented rule).
Each new lane's Epic column adds one bolded signature clause, drawn from the existing shield-rider
`ClauseType` catalog, that all six shapes of that root share before individual legendaries add
their own unique clause in §3. Hues: god/epithet run from framework §3, shade by rarity.

**Lane bijection (old → new; BaseIndex/shape unchanged per line):** Aegis→Aegis (unchanged) ·
Cyclopean→Amyntor (reflect, old Cyclopean-shield scale) · Paean→Pnoe (regen-on-parry, old
Paean-shield scale) · Tritonian→Herkos (spell DR, old Tritonian-shield scale) · Talarian→Probolos
(DR on block, old Aegis ParryDR scale + Talarian's stamina-regen support; weight/dodge dropped —
foreign to a block-DR fantasy).

| Root | Theme — myth | Uncommon | Rare | Epic | Legendary (base) |
|---|---|---|---|---|---|
| **Aegis** | Parry — the aegis itself | +3% parry chance | +5% parry chance, 5% DR on parry | +7% parry chance, 10% DR on parry — **brief thorns after every parry** | +8% parry chance, 12% DR on parry — **brief thorns after every parry** |
| **Amyntor** | Counter — amyntor, the defender | reflect 4% of melee damage taken | reflect 6%, slow self-repair | reflect 8%, self-repair — **every parry reflects an extra 6% on top of thorns (ParryExtraReflect)** | reflect 10%, self-repair — **every parry reflects an extra 6% on top of thorns (ParryExtraReflect)** |
| **Pnoe** | Second Wind — pnoe, breath | +10% HP regen rate | +15% HP regen rate, +5% heals received | +20% HP regen rate, +8% heals received — **a successful parry restores 10% max stamina (ParryRestoresStam)** | +25% HP regen rate, +10% heals received — **a successful parry restores 10% max stamina (ParryRestoresStam)** |
| **Herkos** | Barrier — herkos, the fence of war | 3% spell DR | 5% spell DR, 15% paralyze/stun resist | 7% spell DR, 25% paralyze/stun resist, +5 Resisting Spells — **resisting a paralyze/stun attempt raises spell DR to 15% for 5s** | 8% spell DR, 30% paralyze/stun resist, +5 Resisting Spells — **resisting a paralyze/stun attempt raises spell DR to 15% for 5s** |
| **Probolos** | Breakwater — probolos, the breakwater | +5% DR on block | +8% DR on block, +8% stamina regen | +10% DR on block, +10% stamina regen — **a successful block fires an elemental proc back at the attacker** | +12% DR on block, +12% stamina regen — **a successful block fires an elemental proc back at the attacker** |

**Shared-pool note:** shield effects count toward the same armor-suit caps as body/helm pieces —
parry, DR, dodge, regen, and spell-DR pools are shared across the whole worn suit, not additive
per item (framework §9 rule 4/5 apply to shields exactly as to armor). Probolos's DR-on-block
numbers are pinned so its Legendary-base (12%) lands exactly on the §9 rule 8 damage-reduction
ceiling, the same way Aegis's own DR-on-parry column already does — there is no headroom to bump
a shield's own DR further without breaching the shared cap.

**Legendary-base rule:** per framework §4, armor/shield Legendary is intentionally a step *above*
Epic (not "= Epic" as on weapons) — the numbers above already reflect that; the unique clause is
layered on top per shape in §3.

## 3. Legendaries (unique per shape × theme)

Every legendary = **all Legendary-base effects of its root** (§2, rightmost column) + the unique
clause below, drawn from primitives P1–P27 and respecting §9 (stuns ≤2s + 10s immunity, splash
≤3, first-hit triggers reset only after 30s out of combat). Single-click shows the proper noun
(`Aias [legendary]`); base shape appears in the tooltip (`a heater`).

Clause text below restates the **registry (code truth)** — `LegendaryRegistry.cs` ids 216–245.
Audit column: FIT = clause kept verbatim (already fits its new root's fantasy); SWAP = clause
replaced (old → new) because it duplicated the lane's chosen Epic signature or was clearly
foreign to the new root. Names and ids are unchanged by the re-theme — only roots and (for SWAPs)
clauses moved.

### Aegis line (Athena — shield-bearers of legend; unchanged)

| Shape | Name | Unique clause | Audit |
|---|---|---|---|
| Buckler | **Ankyle** | guaranteed parry vs the first hit of any fight (P11) | FIT |
| Wooden shield | **Oiliades** | parrying a crit briefly stuns the attacker (P13, §9 cap) | FIT |
| Wooden kite | **Salamis** | every parry reflects an extra 6% of the blocked damage on top of thorns (P12) | FIT |
| Metal shield | **Telamon** | every 10th parry repairs 1 point of the shield's durability (P22) | FIT |
| Metal kite | **Sakos** | while below 30% HP, the first hit taken is guaranteed-parried (P11) | FIT |
| Heater | **Aias** | guaranteed parry vs the first hit of any fight, and that parry always triggers a brief stun regardless of crit (P11 + P13, §9 cap) | FIT |

### Amyntor line (was Cyclopean — the counter-blow)

| Shape | Name | Unique clause | Audit |
|---|---|---|---|
| Buckler | **Amphion** | the first hit taken each fight is reflected for 15% of its damage and staggers the attacker (P12, §9 cap) | SWAP: FlameProcDoubleLowDurability → ReflectFirstHit(15) — flame-burst proc is Hephaistos-forge flavor, foreign to Amyntor's pure reflect/counter fantasy |
| Wooden shield | **Zethos** | self-repair rate doubles for 5s after blocking a crit (P22) | FIT |
| Wooden kite | **Proitos** | reflect % rises to 15% vs the first hit of any fight (P12) | FIT |
| Metal shield | **Tiryns** | the first hit taken each fight is reflected for 15% of its damage and heal-blocks the attacker for 3s (§9.3 first-hit gate) | SWAP: FlameProcEveryN → ReflectHealBlock(15, 3) — flame-burst is foreign to the counter fantasy; ShrugReflect was rejected at wiring (shields grant no shrug, trigger unreachable) |
| Metal kite | **Danaos** | each self-repair tick also restores 1% of the wearer's max HP (P22 + P17) | FIT |
| Heater | **Akrisios** | reflect damage from a blocked crit briefly stuns the attacker (P12 + P13, §9 cap) | FIT |

### Pnoe line (was Paean — second wind)

| Shape | Name | Unique clause | Audit |
|---|---|---|---|
| Buckler | **Phylakos** | HP regen rate triples for 5s immediately after taking a crit (P17) | FIT |
| Wooden shield | **Autonoos** | on the wearer's first kill each fight, restore an additional 15% max HP (P23) | FIT |
| Wooden kite | **Aiakos** | guaranteed block vs the first hit of any fight; that block restores 10% stamina (P7) | SWAP: HealBlockOnFirstHitLanded → BlockRestoreStam(10) — heal-blocking is foreign to Pnoe's regen fantasy; NOT ParryRestoresStam, which became Pnoe's own Epic signature (invariant: unique ≠ signature) |
| Metal shield | **Aristaios** | mana regen rate gains the same bonus as HP regen (P8) | FIT |
| Metal kite | **Boutes** | stamina regen rate gains the same bonus as HP regen (P7) | FIT |
| Heater | **Asklepios** | on-kill: restore 25% of max HP (P23) | FIT |

### Herkos line (was Tritonian — the fence of war)

| Shape | Name | Unique clause | Audit |
|---|---|---|---|
| Buckler | **Laomedon** | spell DR rises to 12% vs the first spell hit of any fight (P20) | FIT |
| Wooden shield | **Ilion** | +5 additional Resisting Spells while below 50% HP (P19) | FIT |
| Wooden kite | **Palaimon** | spell DR doubles for 3s after being struck by a crit (P20) | FIT |
| Metal shield | **Alkathous** | +10 Resisting Spells for 5s after resisting a stun/paralyze attempt (P19) | FIT |
| Metal kite | **Megareus** | spell DR also applies to poison damage taken (P20) | FIT |
| Heater | **Hyperbios** | the first hit taken each fight applies no secondary effect — stun, mark, or poison — only raw damage (P20) | FIT |

### Probolos line (was Talarian — the breakwater)

| Shape | Name | Unique clause | Audit |
|---|---|---|---|
| Buckler | **Panoptes** | guaranteed block vs the first hit of any fight; a successful block restores 10% stamina (P7) | SWAP: DodgeRegenBurst → BlockRestoreStam(10) — dodge-triggered regen belongs to the retired stride/dodge fantasy, foreign to Probolos's block-DR identity |
| Wooden shield | **Abderos** | guaranteed block vs the first hit of any fight (P11) | FIT |
| Wooden kite | **Myrtilos** | guaranteed block vs the first hit of any fight; that block drains 2 stamina from the attacker (P7) | SWAP: WeightReductionSuiteBurstOnDodge → BlockDrainStam(2) — weight/dodge rider is explicitly foreign to a block-DR fantasy |
| Metal shield | **Kerberos** | guaranteed block vs the first hit of any fight (P11) | FIT |
| Metal kite | **Lasthenes** | guaranteed block vs the first hit of any fight; that block leeches 10% mana from the attacker (P8) | SWAP: StamRegenMirrorsManaHalf → BlockManaLeech(10) — passive regen-mirroring belongs to Pnoe's second-wind fantasy, not block-DR |
| Heater | **Melanippos** | guaranteed block vs the first hit of any fight; a block guarantees the blocker's next hit crits (P4) | SWAP: OnKillStamRestoreExtendImmunity → BlockNextShotCrit — on-kill stamina restore + stun-immunity extension belongs to Pnoe/Aegis-adjacent fantasies, not block-DR |

**Audit tally:** 23 FIT, 7 SWAP. Aegis (6/6 FIT, unchanged) and Herkos (6/6 FIT — the retired
Tritonian riders were already spell-DR/para-resist themed end to end) needed no changes. Amyntor
needed 2 swaps (both flame-burst-proc riders — Hephaistos-forge flavor with no home in a pure
reflect/counter fantasy). Pnoe needed 1 swap (a heal-block curse rider had no place in a
regen-on-parry fantasy; replaced with the one new approved clause, `ParryRestoresStam`). Probolos
needed 4 of 6 swapped — beyond the two dodge/weight riders flagged directly in the brief, the
remaining two retained riders (`StamRegenMirrorsManaHalf`, `OnKillStamRestoreExtendImmunity`) were
also judged foreign (regen-mirroring and on-kill-plus-immunity read as Pnoe/Aegis material, not
block-DR) and swapped toward the `Block*` rider family already in the catalog (reused from the
Pallas weapon lines — the shield-only rider catalog had no spare block-DR-flavored entries left
once Abderos/Kerberos claimed the two `ParryFirstHitGuaranteed` slots). This leaves Probolos with
a flatter clause profile than the other four lines (every legendary is now some variant of
"guaranteed block vs first hit + reward") — flagging for review since the shield-only catalog
doesn't yet have an "every-Nth-block" or passive-DR clause to break up that repetition.

## 4. Legendary registry (claimed names — review pass merges these)

*Re-theme note (2026-07-07):* all 30 names and ids are unchanged by the per-family root re-theme;
only each line's root (§2 bijection) and the 7 audited clauses above (§3) moved.

Ankyle, Oiliades, Salamis, Telamon, Sakos, Aias, Amphion, Zethos, Proitos, Tiryns, Danaos,
Akrisios, Phylakos, Autonoos, Aiakos, Aristaios, Boutes, Asklepios, Laomedon, Ilion, Palaimon,
Alkathous, Megareus, Hyperbios, Panoptes, Abderos, Myrtilos, Kerberos, Lasthenes, Melanippos. (30)

*Merge-pass note (2026-07-07):* 5 names re-assigned — metal armor keeps Erechtheus/Kekrops
(city-king Polias line), swords keeps Sthenelos, fencing keeps Kaineus/Ladon (tightest fit).
Shields keep Akrisios/Proitos (Proitos raised Tiryns' walls with the Cyclopes — sits beside the
Tiryns shield here); swords renamed its copies. Asklepios' on-kill heal reduced from full to 25%
max HP at the balance pass (paired with the same trim on metal armor's Podaleirios).

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-shape unique names, Uncommon→Epic, same AR anchors)
belong to the crafting pass, re-themed to mythic materials. Nothing here blocks them.
