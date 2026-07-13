# 21 — Clothing (curated, bonus-bearing cloth)

**Derives from:** `00-framework.md` (budgets §2, CLOTHING theme table §3, effect pattern §4,
primitives §5, ladder note §7). Format follows `01-axes.md` (canonical), adapted: clothing has no
damage/AR ladder — §1 is a curated piece list instead of a base/damage matrix.

---

## 1. Curated pieces

No ratio, no speed, no AR. Every dropped-clothing piece below shares the *same* effect tables —
only the cosmetic shape (and hue) differs. Full effect budget goes entirely into theme effects
(framework §7): **clothing carries no Armor Rating at any rarity, ever.**

| Piece | Garment slot |
|---|---|
| Robe | outer torso (robe layer) |
| Cloak | cloak layer |
| Doublet | torso (worn over a shirt) |
| Fancy shirt | shirt layer |
| Tunic | torso |
| Body sash | waist/torso wrap |
| Kilt | legs |
| Skirt | legs |
| Straw hat | head |
| Wide-brim hat | head |
| Feathered hat | head |
| Cap | head |
| Boots | feet |
| Thigh boots | feet |
| Fur boots | feet |
| Shoes | feet |
| Sandals | feet |
| Long pants | legs (pants layer) |
| Short pants | legs (pants layer) |

**Piece count: 19** (17 original shapes + long pants + short pants).

Commons are plain stock items: stock name, no root, no hue, no effects (naming grammar §6).

### Two classes by opportunity cost

UO layer facts split the pool into two magnitude classes:

- **Over-armor cloth** (robe/cloak/body sash/doublet/tunic/fancy shirt/kilt/skirt/feet) sits on
  layers that DON'T conflict with armor — you wear it *on top of* a full suit, so it pays no armor
  cost and runs at the standard clothing magnitudes below (§2, ~50% of jewelry, framework §4).
- **Armor-displacing cloth** — the four **hats** (head/helm layer, which REPLACE metal/leather
  helms) and the two new **cloth pants** (pants layer, which REPLACE leg armor). Equipping one
  sacrifices an armor slot: its AR, its material root, its (material × slot) signature, and a step
  of capstone-set progress. In exchange these run at **2× the clothing magnitudes = jewelry level**.
  This is DATA — the doubled rows are declared in `ClothingFamily.cs` (`ClothingDisplacing` lanes),
  not a runtime multiplier — and a piece is classified as displacing purely by its layer
  (`Layer.Helm` or `Layer.Pants`). Kilt and skirt are `Layer.OuterLegs`, NOT pants, so they stay
  over-armor cloth. No doubled clothing field trips a framework §9.8 shared-pool ceiling (max dodge
  8 < 12; stationary regen 40% folds under the 60% HP-regen consume cap), so no cap-clamps apply.

**Rarity cap (clothing-specific):** dropped named clothing runs **Uncommon → Epic only**. No
curated piece above ever drops at Legendary. Legendary clothing exists *only* as the 10 bound
relics in §3 (5 body pieces + 5 hats) — not as a rarity tier any non-relic shape can roll into.

Hue: god run from framework §3 clothing table, shade per the ramp rule (§3) — Common unhued,
climbing through Epic's shade. Legendary shades belong to the relics alone (§3 below, tune in-client).

## 2. Drop-variant themes (shared across all 17 pieces)

Names read `[root] [piece] [rarity]` — e.g. `charis fancy shirt [rare]`. Magnitudes run at ~50% of
jewelry's (framework §4). No Legendary row: capped at Epic per §1.

| Root | God | Uncommon | Rare | Epic |
|---|---|---|---|---|
| **Laurel** | Nike | on-kill +5 stamina | on-kill +10 stamina | on-kill +10 stamina, +5 HP |
| **Charis** | Aphrodite | +5% karma gained | +10% karma gained, vendor prices 3% better | +15% karma gained, vendor prices 5% better |
| **Maenad** | Dionysos | 2% chance on being struck: frenzy (+10% damage, 5s) | 3% chance, frenzy (+10% damage, 5s) | 4% chance, frenzy (+10% damage, 5s) also grants +10% swing speed |
| **Hestian** | Hestia | +10% HP regen while stationary ≥10s | +15% HP regen while stationary ≥10s | +20% HP regen while stationary ≥10s, also applies to mana |
| **Arachne** | Arachne | 2% dodge chance | 3% dodge chance, +5% poison resist | 4% dodge chance, +10% poison resist |

Note: clothing dodge (Arachne) counts toward the shared dodge suit cap alongside armor's Stride/
Talarian dodge (framework pools). *Flagged: framework §9 as currently written does not yet state
the numeric cap — carrying the note through per brief; reconcile the exact percentage when the
armor family doc (Talarian/Stride) is written and at README merge.*

Same-god stacking (worn on multiple clothing pieces, or clothing + another family) follows
framework §9 rule 4: strongest instance applies fully, the rest at 50%.

### Displacing magnitudes (hats + cloth pants)

The armor-displacing shapes (hats, long/short pants) run the same effect fields at **2×** the rows
above — exactly doubled, booleans unchanged. No value trips a §9.8 ceiling even doubled, so no
cap-clamps apply.

| Root | God | Uncommon | Rare | Epic |
|---|---|---|---|---|
| **Laurel** | Nike | on-kill +10 stamina | on-kill +20 stamina | on-kill +20 stamina, +10 HP |
| **Charis** | Aphrodite | +10% karma gained | +20% karma gained, vendor prices 6% better | +30% karma gained, vendor prices 10% better |
| **Maenad** | Dionysos | 4% on being struck: frenzy (+20% damage, 5s) | 6% chance, frenzy (+20% damage, 5s) | 8% chance, frenzy (+20% damage, 5s) also +20% swing speed |
| **Hestian** | Hestia | +20% HP regen while stationary ≥10s | +30% HP regen while stationary ≥10s | +40% HP regen while stationary ≥10s, also mana |
| **Arachne** | Arachne | 4% dodge chance | 6% dodge, +10% poison resist | 8% dodge, +20% poison resist |

## 3. Legendaries — the five relics

Clothing has no per-base × theme legendary matrix (§1 cap). Instead, exactly **5** legendary
relics exist — one per theme god, each permanently bound to one specific piece shape. All five are
the Fates and their weavers; every unique clause echoes thread, fate, or the loom. Each relic
carries its theme's full Epic effects plus the one clause below.

| Relic | Theme (God) | Bound piece | Unique clause |
|---|---|---|---|
| **Klotho** | Laurel (Nike) | Body sash | Klotho spins anew: on-kill, beyond the Epic restore, gain a 5s burst of +20% stamina regen (P17) — a second kill within that window stacks the burst once more (max 2). |
| **Lachesis** | Charis (Aphrodite) | Fancy shirt | Lachesis measures the same thread for beast and man alike: +15 skill points to Animal Taming while worn (P19). |
| **Atropos** | Maenad (Dionysos) | Kilt | The cut Atropos deals is always final: while frenzied, blows carry a 4% chance to briefly sever the target's footing (1s stagger, §9 immunity applies). |
| **Ariadne** | Hestian (Hestia) | Robe | Ariadne's thread never frays: the robe's stitching self-mends, immune to durability loss (P22). |
| **Penelope** | Arachne (Arachne) | Cloak | The unraveled shroud lashes back: a successful dodge reflects 10% of the avoided damage onto the attacker (P12). |

Each clause draws from exactly one framework primitive (P12, P17, P19, P22, P13-family stagger)
and respects §9 (Atropos's stagger is ≤2s with the standard 10s post-stun immunity; none of the
five introduce banned mechanics per §5).

### The hat cycle (armor-displacing relics)

A second cycle of **5** legendary relics, one per theme god, each bound to a **hat** shape (two may
share a shape — uniqueness is the (family, root, base) triple). These carry the displacing Epic
package (2× magnitudes, §2 above) plus the one clause below. Crowns and veils, echoing each god's
myth.

| Relic | Theme (God) | Bound hat | Unique clause | Myth |
|---|---|---|---|---|
| **Kotinos** | Laurel (Nike) | Straw hat | on-kill: full stamina, the next hit crits within 5s | the olive victory-wreath of the games |
| **Kisseus** | Maenad (Dionysos) | Wide-brim hat | while frenzied, a 6% chance to stagger the struck target 1s | the ivy crown of the god's revelers |
| **Diadema** | Charis (Aphrodite) | Feathered hat | +15% to all healing received while worn | the diadem of the goddess of beauty |
| **Kalyptra** | Hestian (Hestia) | Cap | regen while standing still kicks in after 5s instead of 10s | the veil of the hearth-tender |
| **Kalathos** | Arachne (Arachne) | Straw hat | a successful dodge grants +20% stamina regen for 5s | the weaver's basket-crown |

The item name embeds the base shape (`Klotho Body Sash`, per BuildLegendaryName), so both the
single-click label and the OPL name line carry it — there is no separate OPL subtitle line.

## 4. Legendary registry (claimed names — review pass merges these)

Klotho, Lachesis, Atropos, Ariadne, Penelope, Kotinos, Kisseus, Diadema, Kalyptra, Kalathos. (10)

## 5. Crafting (deferred)

Per framework §11: crafted clothing variants (per-piece unique names, Uncommon→Epic, no
Legendary — consistent with the drop cap in §1) will be re-themed to mythic materials in the
crafting pass. Nothing here blocks them.
