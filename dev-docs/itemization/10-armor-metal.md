# 10 — Armor: Metal (Ringmail, Chainmail, Platemail)

**Derives from:** `00-framework.md` (budgets §2, ARMOR themes §3, primitives §5, armor ladder §7,
balance rules §9). Format follows `02-swords.md` / `01-axes.md` (canonical).
**Scope:** the three metal armor materials — ringmail, chainmail, platemail — plus the five metal
helm shape variants. Crafting deferred (see §5).

---

## 1. Base ladder & AR matrix

Piece AR = `material ratio × slot weight × A[rarity]`, rounded to the nearest integer
(`A` = 12, 14, 17, 21, 26 for Common…Legendary, per framework §2). Material ratios: ring **.86**,
chain **.93**, plate **1.00** (framework §7). Slot weights: chest/tunic **1.00**, legs **.70**,
helm **.50**, arms **.40**, gorget **.25**, gloves **.25**.

Commons are plain base items: stock name, no hue, no effects.

Real T2A shapes only — ringmail has no helm/gorget piece, chainmail has no arms/gloves/gorget
piece, in stock UO. Platemail alone fields the full six-slot set.

### Ringmail (ratio .86)

| Piece | Slot weight | Common | Uncommon | Rare | Epic | Legendary |
|---|---:|---:|---:|---:|---:|---:|
| Tunic | 1.00 | 10 | 12 | 15 | 18 | 22 |
| Legs | 0.70 | 7 | 8 | 10 | 13 | 16 |
| Arms | 0.40 | 4 | 5 | 6 | 7 | 9 |
| Gloves | 0.25 | 3 | 3 | 4 | 5 | 6 |
| **Suit total** | | **24** | **28** | **35** | **43** | **53** |

### Chainmail (ratio .93)

| Piece | Slot weight | Common | Uncommon | Rare | Epic | Legendary |
|---|---:|---:|---:|---:|---:|---:|
| Tunic | 1.00 | 11 | 13 | 16 | 20 | 24 |
| Legs | 0.70 | 8 | 9 | 11 | 14 | 17 |
| Coif | 0.50 | 6 | 7 | 8 | 10 | 12 |
| **Suit total** | | **25** | **29** | **35** | **44** | **53** |

### Platemail (ratio 1.00)

| Piece | Slot weight | Common | Uncommon | Rare | Epic | Legendary |
|---|---:|---:|---:|---:|---:|---:|
| Tunic | 1.00 | 12 | 14 | 17 | 21 | 26 |
| Legs | 0.70 | 8 | 10 | 12 | 15 | 18 |
| Arms | 0.40 | 5 | 6 | 7 | 8 | 10 |
| Gorget | 0.25 | 3 | 4 | 4 | 5 | 7 |
| Gloves | 0.25 | 3 | 4 | 4 | 5 | 7 |
| Helm (see below) | 0.50 | 6 | 7 | 9 | 11 | 13 |
| **Suit total** | | **37** | **45** | **53** | **65** | **81** |

Common (37) and Legendary (81) plate-suit totals match the framework §7 sanity check
(≈37 / ≈81) exactly.

**Metal helm shape variants** — helmet, bascinet, norse helm, close helm, plate helm — are all
the same row above (plate ratio × helm-slot weight), kept plate for simplicity per spec; they
differ only in model, never in AR. Chainmail's helm-slot piece is the **coif**, computed on the
chain ratio (0.93), not plate — it is mechanically distinct from the plate helm variants, not a
sixth shape of them.

## 2. Drop-variant lanes — five roots per material

**2026-07-07 re-theme.** Metal roots are now **per material** (framework §3): ringmail, chainmail
and platemail each field their own five roots — a root is a statement of what you are wearing.
Names read `[root] [piece] [rarity]` — e.g. `phylax chain tunic [rare]`, `adamas plate helm [epic]`.
One effect package per root per rarity; only the AR number (§1) differs by piece. Hues: material
run from framework §3, shaded by rarity.

**Magnitudes are carried over, not re-derived.** Every per-rarity number below re-uses a retired
shared-table scale per primitive (armor magnitudes did not change in the re-theme — they re-mix):

| Primitive | Uncommon | Rare | Epic | Legendary | Scale source |
|---|---|---|---|---|---|
| Bonus AR | +1 | +2 | +3 | +4 | retired Polias |
| Damage reduction | +1% | +2% | +2–3% | +3% | retired Polias |
| Shrug (halve one blow) | 3%* | 4%* | 5% | 8% | retired Polias (E/L 5/8; U/R extended for shrug-primary lanes*) |
| Thorns / reflect | 2% | 3% | 5% | 6% | retired Cyclopean |
| Flame-burst proc | — | 4%* | 4% | 8% | retired Cyclopean |
| Self-repair | slow | yes | yes | yes | retired Cyclopean |
| HP regen | +8% | +12% | +18% | +25% | retired Paean |
| Spell DR | 2% | 3% | 5% | 6% | retired Tritonian |
| Para/stun resist | 10% | 15% | 20% | 30% | retired Tritonian |
| Resisting Spells skill | — | — | +5 | +5 | retired Tritonian |
| Weight reduction | −10% | −20% | −30% | −40% | retired Talarian |
| Stam regen | +4% | +6% | +8% | +10% | retired Talarian |

New primitives with **no** legacy armor scale are placeholders (tune live): on-kill stam
5/8/10/15% max, on-kill HP 3/5/8/10% max. Riders drawn from the P3b jewelry/clothing pool
(marked †) are nearest-fit until an armor-native equivalent is wired.

**Epic rider (bolded)** is the lane's signature clause (an existing armor rider ClauseType, framework
§5), carried at Epic **and** Legendary — same shape swords use for their Epic signature. The
Legendary column is a step **above** Epic (framework §4 armor rule) and adds the §3 unique clause
on top of the Epic package.

> **Stacking & caps.** Per-material roots stack by mechanic **StackGroup** (framework §9.4:
> strongest instance full, the rest at 50%), enforcing the §9.8 shared defensive pools across
> everything worn — armor + shield + clothing together: bonus AR ≤ 15 · DR ≤ 12% · shrug ≤ 20% ·
> thorns/reflect ≤ 25% · HP regen ≤ 60% · spell DR ≤ 18% · dodge ≤ 12%. "Block" on the Hoplites
> lane counts against the **shrug** pool (it is the halve-a-blow mechanic, not a shield block).
> Tune live.

### Ringmail — "the hoplite's kit" (ratio .86)

| Root | Lane | Uncommon | Rare | Epic (+ signature) | Legendary |
|---|---|---|---|---|---|
| **Hoplites** | line-shield (AR + block) | +1 bonus AR | +2 bonus AR, 3% block | +3 bonus AR, 5% block — **the first blow of each fight is always blocked (ShrugFirstHitGuaranteed)** | +4 bonus AR, 8% block + unique (§3) |
| **Taxis** | formation (DR + para resist) | +1% DR, 10% para resist | +2% DR, 15% para resist | +2% DR, 20% para resist — **a resisted para/stun stuns the attacker (ParaResistStunsAttacker, §9)** | +3% DR, 30% para resist + unique (§3) |
| **Dromos** | march (stam regen + weight) | −10% weight, +4% stam regen | −20% weight, +6% stam regen | −30% weight, +8% stam regen — **stamina-regen ticks also refresh mana (StamRegenMirrorsManaHalf)** | −40% weight, +10% stam regen + unique (§3) |
| **Zoster** | war-belt (durability + self-repair) | self-repair | self-repair, +1 bonus AR | self-repair, +2 bonus AR — **each self-repair tick restores a little max HP (SelfRepairRestoresHp)** | self-repair, +3 bonus AR + unique (§3) |
| **Alkimos** | valiant (shrug) | 3% shrug | 4% shrug | 5% shrug — **a fully shrugged blow briefly stuns the attacker (ShrugStunAttacker, 1s, §9)** | 8% shrug + unique (§3) |

### Chainmail — "the watchful wall" (ratio .93)

| Root | Lane | Uncommon | Rare | Epic (+ signature) | Legendary |
|---|---|---|---|---|---|
| **Phylax** | guard (AR + shrug) | +1 bonus AR | +2 bonus AR, 3% shrug | +3 bonus AR, 5% shrug — **a fully shrugged blow briefly stuns the attacker (ShrugStunAttacker, 1s, §9)** | +4 bonus AR, 8% shrug + unique (§3) |
| **Egregoros** | unsleeping (para resist + first-hit shrug) | 10% para resist | 15% para resist, 3% shrug | 20% para resist, 5% shrug — **the first blow of each fight is always shrugged (ShrugFirstHitGuaranteed)** | 30% para resist, 8% shrug + unique (§3) |
| **Teichos** | wall (DR) | +1% DR | +2% DR | +3% DR — **spell DR doubles for 3s after taking a crit — the wall hardens when struck (SpellDrBurstOnCritTaken)** | +3% DR (pool cap), +1 bonus AR + unique (§3) |
| **Halysis** | the chain (durability + self-repair) | self-repair | self-repair, +1 bonus AR | self-repair, +2 bonus AR — **self-repair rate surges for a few seconds after taking a crit (SelfRepairBurstOnCritBlock)** | self-repair, +3 bonus AR + unique (§3) |
| **Phrourion** | fortress (spell DR) | 2% spell DR | 3% spell DR | 5% spell DR — **spell DR rises vs the first spell of each fight (SpellDrBoostFirstHit)** | 6% spell DR + unique (§3) |

### Platemail — "the forged colossus" (ratio 1.00)

| Root | Lane | Uncommon | Rare | Epic (+ signature) | Legendary |
|---|---|---|---|---|---|
| **Adamas** | adamant (top AR + DR) | +1 bonus AR, +1% DR | +2 bonus AR, +2% DR | +3 bonus AR, +2% DR — **once per fight, a hit that would drop the wearer under 10% HP instead fires a full regen tick — the adamant holds at the brink (EmergencyRegenTick)** | +4 bonus AR, +3% DR + unique (§3) |
| **Kaminos** | kiln (flame proc + reflect) | 2% reflect | 3% reflect, 4% flame-burst proc | 5% reflect, 8% flame-burst proc — **the flame-burst proc chance doubles vs the first attacker of any fight (FlameProcDoubleFirstHit, P14)** | 6% reflect, flame-burst proc + unique (§3) |
| **Kolossos** | colossus (shrug + stun/para resist) | 3% shrug, 10% para resist | 4% shrug, 15% para resist | 5% shrug, 20% para resist — **a resisted para/stun stuns the attacker (ParaResistStunsAttacker, §9)** | 8% shrug, 30% para resist + unique (§3) |
| **Panoplia** | panoply (bonus AR + durability) | +1 bonus AR, slow self-repair | +2 bonus AR, self-repair | +3 bonus AR, self-repair — **each self-repair tick also restores 1% max HP — the panoply sustains its bearer (SelfRepairRestoresHp)** | +4 bonus AR, self-repair + unique (§3) |
| **Akamatos** | unwearying (self-repair + HP regen) | +8% HP regen, slow self-repair | +12% HP regen, self-repair | +18% HP regen, self-repair — **HP regen triples for a few seconds after taking a crit (HpRegenBurstOnCritTaken)** | +25% HP regen, self-repair + unique (§3) |

> **Rider-pool note.** The armor rider pool is shallow by mechanic: three shrug clauses
> (ShrugStunAttacker / ShrugReflect / ShrugFirstHitGuaranteed) serve six shrug-touching lanes, and
> only two body-armor self-repair riders exist. Signatures therefore recur across lanes; within any
> one lane the Epic rider never equals that lane's §3 unique clause (framework §4 invariant). Final
> assignment is a live-tune target.

## 3. Legendaries (unique per material × theme)

A legendary is one proper noun per **material × theme** (3 materials × 5 roots = 15), not per piece
— it drops as a random slot piece of its material (AR from §1's Legendary column for whichever slot
it lands on). The name embeds the slot piece (`Talos Platemail Gorget [legendary]`, framework §6
revised 2026-07-13). Each legendary = all Epic effects of its (new) root (§2) + the unique clause below.

**2026-07-07 re-theme.** All 15 names and ids are **frozen** (registry is code truth,
`LegendaryRegistry.cs` ids 186–200). Only each line's **root** moved, via the fixed per-material
bijection, and each clause is re-audited against the new lane fantasy. **Audit column:** FIT = the
frozen registry clause fits the new lane, kept verbatim; SWAP = the clause is foreign to the new
lane (or would duplicate the lane's Epic rider) and is re-pointed (old → new) for the engine pass.

**Metal bijection (old root → new root, per material):**
- **Ring:** Polias→Hoplites · Cyclopean→Zoster · Paean→Alkimos · Tritonian→Taxis · Talarian→Dromos
- **Chain:** Polias→Phylax · Cyclopean→Halysis · Paean→Phrourion · Tritonian→Egregoros · Talarian→Teichos
- **Plate:** Polias→Adamas · Cyclopean→Kaminos · Paean→Akamatos · Tritonian→Kolossos · Talarian→Panoplia

### Ringmail (ids 186, 189, 192, 195, 198)

| New root | Name | Unique clause | Audit |
|---|---|---|---|
| **Hoplites** | **Kekrops** (186) | a fully shrugged/blocked blow briefly stuns the attacker (1s, §9) | FIT — shrug-stun sits cleanly on the block/shield-wall lane |
| **Zoster** | **Perdix** (189) | self-repair rate surges for a few seconds after the war-belt turns a crit (SelfRepairBurstOnCritBlock) | SWAP: FlameProcDoubleFirstHit → SelfRepairBurstOnCritBlock — a forge flame-proc is foreign to the durability/self-repair war-belt |
| **Alkimos** | **Machaon** (192) | a fully shrugged blow reflects part of its damage back at the attacker (ShrugReflect, P12) | SWAP: AutoCureRestoresStamMana → ShrugReflect — auto-cure mending is foreign to the pure-shrug valiant lane (ShrugReflect differs from the lane's ShrugStunAttacker Epic rider) |
| **Taxis** | **Nereus** (195) | spell DR rises to 10% for 5s after resisting a paralyze/stun (ParaResistBoostsSpellDr) | FIT — para-resist→DR is exactly the DR + para-resist formation identity |
| **Dromos** | **Automedon** (198) | on-kill: full stamina restore + extended stun immunity (OnKillStamRestoreExtendImmunity) | SWAP: DodgeRefundStam → OnKillStamRestoreExtendImmunity — the march lane carries no dodge trigger; a stamina/momentum on-kill fits the relentless march |

### Chainmail (ids 187, 190, 193, 196, 199)

| New root | Name | Unique clause | Audit |
|---|---|---|---|
| **Phylax** | **Erechtheus** (187) | a fully shrugged blow reflects 15% of its damage back at the attacker (ShrugReflect, P12) | FIT — shrug-reflect on the AR + shrug guard lane |
| **Halysis** | **Erichthonios** (190) | each self-repair tick also restores a little max HP (SelfRepairRestoresHp) | SWAP: FlameProcHealBlock → SelfRepairRestoresHp — flame/heal-block is foreign to the durability/self-repair chain |
| **Phrourion** | **Podaleirios** (193) | spell DR doubles for a few seconds after taking a crit — the fortress hardens when struck (SpellDrBurstOnCritTaken) | SWAP: OnKillRestoreMissingHpPct → SpellDrBurstOnCritTaken — an on-kill heal is foreign to the spell-DR fortress |
| **Egregoros** | **Proteus** (196) | a resisted paralyze/stun attempt briefly stuns the attacker instead (1s, §9) — (ParaResistStunsAttacker) | FIT — para-resist counter on the para-resist unsleeping lane |
| **Teichos** | **Patroklos** (199) | the first blow taken each fight applies no secondary effect (FirstHitNoSecondaryEffect) | SWAP: OnKillDodgeDoubleDuration → FirstHitNoSecondaryEffect — dodge/on-kill is foreign to the pure-DR wall; an effect-nullifying opener is the "wall" fantasy |

### Platemail (ids 188, 191, 194, 197, 200)

| New root | Name | Unique clause | Audit |
|---|---|---|---|
| **Adamas** | **Kadmos** (188) | the first blow of any fight is always shrugged, regardless of the roll (ShrugFirstHitGuaranteed, P14) | FIT — the unbreakable turns the opener; sits well on the top-AR + DR adamant lane |
| **Kaminos** | **Talos** (191) | the flame-burst proc chance rises to 12% while the wearer is below 30% HP (FlameProcBoostLowHp) | FIT — a flame rider on the flame-proc + reflect kiln (exemplar-perfect) |
| **Akamatos** | **Iapyx** (194) | stamina regen gains the same bonus as HP regen — the tireless body refreshes every pool (StamRegenMirrorsHp) | SWAP: AutoCureClearsDebuffsOnce → StamRegenMirrorsHp — auto-cure mending is foreign to the self-repair + HP-regen unwearying lane |
| **Kolossos** | **Glaukos** (197) | the Resisting Spells bonus doubles to +10 while the wearer is below 50% HP (ResistSkillDoubleLowHp) | FIT — a resist-skill boost on the shrug + stun/para-resist colossus |
| **Panoplia** | **Damastor** (200) | the first blow taken each fight applies no secondary effect (FirstHitNoSecondaryEffect) | SWAP: DodgeRestoreMana → FirstHitNoSecondaryEffect — dodge is foreign to the heavy AR + durability panoply; a completeness/nullify reading fits full armor |

## 4. Legendary registry (claimed names — review pass merges these)

Kekrops, Erechtheus, Kadmos, Perdix, Erichthonios, Talos, Machaon, Podaleirios, Iapyx, Nereus,
Proteus, Glaukos, Automedon, Patroklos, Damastor. (15)

*Re-theme note (2026-07-07):* all 15 names and ids (registry 186–200) are **unchanged** by the
per-material root re-theme — only each line's root re-pointed to the new material-specific lane (per
the §3 bijection) and the audited clauses moved. 8 clauses SWAP (Perdix, Machaon, Automedon;
Erichthonios, Podaleirios, Patroklos; Iapyx, Damastor), 7 FIT.

*Merge-pass note (2026-07-07):* Polyidos renamed to Iapyx (staves owns the seer as Polyeidos);
Podaleirios' on-kill heal was retired in the re-theme (its line now reads spell-DR under Phrourion).

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-material unique names, Uncommon→Epic, same AR anchors)
will be re-themed to mythic materials (adamant, orichalcum, Cretan bronze) in the crafting pass.
Nothing here blocks them.
