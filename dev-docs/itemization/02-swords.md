# 02 — Swords (Swordsmanship)

**Derives from:** `00-framework.md` (budgets §2, themes §3, effect pattern §4, primitives §5,
ladder §7). Format mirrors `01-axes.md` (canonical format doc) exactly.
**Source:** base ratios/speeds and Legendary DPS targets are fixed by framework §7 — carried over
unchanged. Legendary names and unique clauses are newly authored for this pass, drawn from a
dedicated namespace lane: Trojan War heroes, the Perseid cycle, and named mythic swords.

---

## 1. Base ladder & damage matrix

Damage = `ratio × D[rarity]`, D = (10, 15.6, 27.2, 46.2, 111). Speed = swing seconds (bigger =
slower). DPS columns are base values before speed effects.

| Base | Ratio | Speed | Common | Uncommon | Rare | Epic | Legendary | Leg. DPS |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Butcher knife | 0.60 | 2.55s | 6.0 | 9.4 | 16.3 | 27.7 | 66.6 | 26.12 |
| Cleaver | 0.65 | 2.65s | 6.5 | 10.1 | 17.7 | 30.0 | 72.2 | 27.23 |
| Cutlass | 0.70 | 2.75s | 7.0 | 10.9 | 19.0 | 32.3 | 77.7 | 28.25 |
| Scimitar | 0.75 | 2.85s | 7.5 | 11.7 | 20.4 | 34.7 | 83.2 | 29.21 |
| Katana | 0.80 | 2.95s | 8.0 | 12.5 | 21.8 | 37.0 | 88.8 | 30.10 |
| Broadsword | 0.85 | 3.05s | 8.5 | 13.3 | 23.1 | 39.3 | 94.3 | 30.93 |
| Longsword | 0.90 | 3.15s | 9.0 | 14.0 | 24.5 | 41.6 | 99.9 | 31.71 |
| Viking sword | 0.95 | 3.25s | 9.5 | 14.8 | 25.8 | 43.9 | 105.4 | 32.45 |

Commons are the plain base items: stock name, no hue, no effects.

## 2. Drop-variant themes — "the hero's duel" (sword-unique, shared across all 8 bases)

Names read `[root] [base] [rarity]` — e.g. `phoibos scimitar [rare]`. One effect package per
root per rarity; only the damage number differs by base. Roots are sword-only (framework §3,
2026-07-07 re-theme); magnitudes drawn from the framework §4 menu. Epic adds the lane's
signature clause. Hues: placeholder runs until the in-client pass.

| Root | Lane | Uncommon | Rare | Epic (+ signature) |
|---|---|---|---|---|
| **Phoibos** | precision | +6% hit chance | +6% hit chance, +8% crit chance | +8% hit chance, +10% crit chance — **the first hit of each fight always crits** |
| **Areia** | riposte | +6% block | +6% block, 8% DR on block | +8% block, 12% DR on block — **a block guarantees your next hit crits** |
| **Menis** | wrath-ramp | +2% dmg per consecutive hit on one target, max 5 stacks (P28) | +3%/stack, max 5, +8% damage | +3%/stack, max 6, +10% damage — **reaching max stacks bursts 12% splash (≤3 targets)** |
| **Aristeia** | glory | on-kill: restore 10% max stam | on-kill: 15% stam, +8% damage | on-kill: 20% stam, +10% damage — **on-kill: full stam + your next swing within 5s crits** |
| **Haima** | bleed | 8% gash chance (poison tick, P10) | 10% gash chance, +6% lifesteal | 12% gash chance, +8% lifesteal — **targets bleeding from your gash take +10% damage from you** |

Ramp resets on target swap or when combat lapses (framework P28). "Gash" runs on the poison
primitive; the +damage-taken signature runs on the mark machinery — tooltips show the engine's
mark/poison wording.

## 3. Legendaries (unique per base × theme)

Every legendary = **all Epic effects of its root** + the unique clause below. Damage from §1
Legendary column. Single-click shows the proper noun (`Harpe [legendary]`); base shape appears in
the tooltip.

Namespace lane for this family: Trojan War figures (both sides) and the Perseid cycle. **Names
and ids are frozen** — the 2026-07-07 re-theme re-points each line to the sword-unique roots via
the lane bijection below and re-audits clauses against the new lane fantasy. Clause text below
restates the **registry (code truth)**; a few pre-retheme doc lines had drifted from the
registry (e.g. Xanthos) — the registry wording wins. Audit column: FIT = clause kept verbatim;
SWAP = clause replaced (old → new).

**Lane bijection (old → new):** Zephyr→Menis · Phobos→Phoibos · Agrotera→Haima · Pallas→Areia ·
Stygian→Aristeia.

### Menis line (was Zephyr — the relentless onslaught)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Butcher knife | **Podarkes** | every 5th hit strikes twice; the second strike always crits | SWAP: ExtraSwingOnParry → DoubleStrikeEveryN(5) — parry rider belongs to Areia's fantasy |
| Cleaver | **Antilochos** | guaranteed extra swing every 5th hit; extra swings always land | FIT |
| Cutlass | **Xanthos** | guaranteed extra swing every 5th hit | FIT |
| Scimitar | **Eumelos** | guaranteed extra swing every 5th hit | FIT |
| Katana | **Idaios** | guaranteed extra swing on the first hit of any fight | FIT |
| Broadsword | **Thoas** | guaranteed extra swing every 5th hit; that swing briefly staggers (1s stun, §9) | FIT |
| Longsword | **Rhesos** | guaranteed extra swing every 5th hit; extra swings ignore 10% armor (P27) | FIT |
| Viking sword | **Meriones** | guaranteed extra swing every 5th hit; extra swings also leech 5% stamina | FIT |

### Phoibos line (was Phobos — the unerring strike)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Butcher knife | **Diomedes** | crits deal double damage vs full-HP targets | SWAP: CritFirstHit → CritFullHpDouble — duplicates the Phoibos Epic signature (invariant: unique ≠ signature) |
| Cleaver | **Hektor** | guaranteed crit every 5th hit; crits deal 10% splash (≤3 targets) | FIT |
| Cutlass | **Sarpedon** | guaranteed crit every 6th hit; crits deal double damage vs targets below 15% HP | FIT |
| Scimitar | **Aineias** | guaranteed crit every 5th hit; crits ignore 15% armor (P27) | SWAP: CritPoisonTick → CritArmorPen(5, 15) — poison rider belongs to Haima |
| Katana | **Idomeneus** | guaranteed crit every 6th hit; crits deal 15% splash (≤3 targets) | FIT |
| Broadsword | **Neoptolemos** | your crits deal 10% splash (≤3 targets) | SWAP: CritFirstHit(splash) → CritSplash(0, 10, 3) — first-hit-crit duplicates the signature; the splash rider survives |
| Longsword | **Agenor** | guaranteed crit every 5th hit | FIT |
| Viking sword | **Chrysaor** | guaranteed crit every 6th hit; crits ignore 15% armor (P27) | FIT |

### Haima line (was Agrotera — the opened vein; "mark" reads as a bleeding gash)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Butcher knife | **Kephalos** | guaranteed gash-mark on the first hit of any fight; marked targets take +25% damage from all sources | FIT |
| Cleaver | **Peirithoos** | guaranteed gash-mark on first hit; your poison ticks on marked targets are doubled | FIT |
| Cutlass | **Paris** | guaranteed gash-mark on every crit; marked targets cannot be healed for 3s (§9.3) | FIT |
| Scimitar | **Melanion** | guaranteed gash-mark on the first hit of any fight | FIT |
| Katana | **Dolon** | guaranteed gash-mark on first hit; up to 3 nearby allies of the target are marked at half bonus | FIT |
| Broadsword | **Odysseus** | guaranteed gash-mark on first hit; on the marked target's death the mark jumps to ≤3 nearby enemies | FIT |
| Longsword | **Perseus** | guaranteed gash-mark on first hit; marked targets take +25% damage from all sources | FIT |
| Viking sword | **Harpe** | guaranteed gash-mark on every crit | FIT |

### Areia line (was Pallas — the perfect riposte)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Butcher knife | **Nestor** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) | FIT |
| Cleaver | **Polydamas** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) | FIT |
| Cutlass | **Antenor** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (§9) | FIT |
| Scimitar | **Menestheus** | guaranteed block vs first hit; a successful block restores 10% stamina | FIT |
| Katana | **Eurypylos** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) | FIT |
| Broadsword | **Sthenelos** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (§9) | FIT |
| Longsword | **Amphitryon** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (§9) | FIT |
| Viking sword | **Deiphobos** | guaranteed block vs first hit; a successful block restores 10% stamina | FIT |

### Aristeia line (was Stygian — glory drinks deep)

| Base | Name | Unique clause | Audit |
|---|---|---|---|
| Butcher knife | **Iphitos** | on-kill: full stamina and mana restore | FIT |
| Cleaver | **Memnon** | on-kill: full stamina and mana restore | SWAP: LifestealOnCrit → OnKillRestore(2). The originally-drafted OnKillFullStamNextHitCrit **is** the Aristeia Epic signature itself (audit invariant: a legendary's unique clause may not equal its lane signature — and that clause is engine-designated signature-only), so it cannot be a unique clause; the on-kill restore stands instead |
| Cutlass | **Euphorbos** | on-kill: full stamina and mana restore | FIT |
| Scimitar | **Palamedes** | guaranteed lifesteal proc on every crit | FIT (borderline — victory-feast flavor; watch at review) |
| Katana | **Agamemnon** | on-kill: full stamina and mana restore | FIT |
| Broadsword | **Aigisthos** | guaranteed lifesteal proc on every crit | FIT (borderline, as Palamedes) |
| Longsword | **Elektryon** | guaranteed lifesteal proc on every crit | FIT (borderline, as Palamedes) |
| Viking sword | **Achilles** | crits drain the target's stamina fully | FIT |

## 4. Legendary registry (claimed names — review pass merges these)

*Re-theme note (2026-07-07):* all 40 names and ids are unchanged by the per-family root re-theme;
only each line's root and the audited clauses above moved.

Podarkes, Antilochos, Xanthos, Eumelos, Idaios, Thoas, Rhesos, Meriones, Diomedes, Hektor,
Sarpedon, Aineias, Idomeneus, Neoptolemos, Agenor, Chrysaor, Kephalos, Peirithoos, Paris,
Melanion, Dolon, Odysseus, Perseus, Harpe, Nestor, Polydamas, Antenor, Menestheus, Eurypylos,
Sthenelos, Amphitryon, Deiphobos, Iphitos, Memnon, Euphorbos, Palamedes, Agamemnon, Aigisthos,
Elektryon, Achilles. (40)

*Merge-pass note (2026-07-07):* 11 names re-assigned to resolve cross-family collisions — archery
keeps Teukros/Pandaros/Philoktetes/Penthesileia, metal armor keeps
Automedon/Machaon/Podaleirios/Glaukos/Patroklos, shields keep Akrisios/Proitos. Swords retains the
disputed core it claimed first (Antilochos, Meriones, Diomedes, Sarpedon, Idomeneus).

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-base unique names, Uncommon→Epic, same damage anchors)
will be re-themed to mythic materials in the crafting pass. Nothing here blocks them.
