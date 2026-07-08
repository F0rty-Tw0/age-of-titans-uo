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

Commons are plain stock items: stock name, no root, no hue, no effects (naming grammar §6).

**Rarity cap (clothing-specific):** dropped named clothing runs **Uncommon → Epic only**. No
curated piece above ever drops at Legendary. Legendary clothing exists *only* as the 5 bound
relics in §3 — not as a rarity tier any of the 12 shapes above can roll into.

Hue: god run from framework §3 clothing table, shade per the ramp rule (§3) — Common unhued,
climbing through Epic's shade. Legendary shades belong to the relics alone (§3 below, tune in-client).

## 2. Drop-variant themes (shared across all 12 pieces)

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

Single-click shows the proper noun (`Klotho [legendary]`); base shape (`a body sash`) appears in
the OPL subtitle line, per naming grammar §6.

## 4. Legendary registry (claimed names — review pass merges these)

Klotho, Lachesis, Atropos, Ariadne, Penelope. (5)

## 5. Crafting (deferred)

Per framework §11: crafted clothing variants (per-piece unique names, Uncommon→Epic, no
Legendary — consistent with the drop cap in §1) will be re-themed to mythic materials in the
crafting pass. Nothing here blocks them.
