# 20 — Jewelry (Ring / Bracelet / Necklace / Earrings)

**Derives from:** `00-framework.md` (budgets §2, jewelry themes §3, effect pattern §4, primitives
§5, ladder note §7). Format follows `01-axes.md` (canonical), adapted for a no-AR/no-damage,
effects-only family.

---

## 1. Slots

Four slots: **ring, bracelet, necklace, earrings**. No AR, no damage — the entire item budget is
spent on effects.

All four slots share **identical effect tables** per theme and rarity. A rare olympian ring and a
rare olympian necklace are mechanically the same item in a different slot — only the icon/base
shape differs. This also means the same theme × rarity combination exists on 4 physical items, and
a Legendary gets one unique name **per slot**, not per "base" (there is no damage/speed ladder to
drive a base list, per framework §7).

**Stacking rule (framework §9.4 — restated prominently, this is the rule that matters most for a
family where a player can wear all 4 slots at once):** if a character wears the same god theme on
more than one accessory, the **strongest instance applies in full; every other instance of that
same theme applies at 50% magnitude.** Wearing four Legendary Olympian pieces does not grant 4×
the stat bonus or a 20% proc rate — it grants one full Legendary Olympian package plus three
half-strength copies. Mixing different themes across the four slots is not subject to this rule
(see framework §9.5 for the separate cross-item-power audit).

## 2. Drop-variant themes (shared across all 4 slots)

Names read `[root] [slot] [rarity]` — e.g. `hecatean necklace [rare]`. One effect package per root
per rarity; the package is identical on every slot. Hues: god run from framework §3, shade by
rarity.

| Root | God | Uncommon | Rare | Epic | Legendary |
|---|---|---|---|---|---|
| **Olympian** | Zeus | +2 to one stat, rolled at creation (Str/Dex/Int) | +4 to that stat | +6 to that stat, 3% lightning proc on your melee hits (P24) | +9 to that stat, 5% lightning proc + unique clause (§3) |
| **Hecatean** | Hecate | +8% mana regen | +12% mana regen, +4% spell damage | +18% mana regen, +7% spell damage, 3% mana leech on spell damage | +25% mana regen, +10% spell damage, 5% mana leech + unique clause (§3) |
| **Tychean** | Tyche | 2% chance an incoming hit is halved | 3% chance an incoming hit is halved | 4% chance an incoming hit is halved, plus 4% chance your missed swing re-rolls once | 5% chance an incoming hit is halved, 6% re-roll chance + unique clause (§3) |
| **Nyxian** | Nyx | +5 Hiding (P19) | +5 Hiding, +5 Stealth | +10 Hiding, +10 Stealth, night sight | +15 Hiding, +15 Stealth, night sight, +10% poison resist + unique clause (§3) |
| **Demetrian** | Demeter | +4% HP/stam/mana regen | +6% HP/stam/mana regen | +8% HP/stam/mana regen, +10% healing-potion effect | +12% HP/stam/mana regen, +15% healing-potion effect + unique clause (§3) |

## 3. Legendaries (unique per slot × theme)

Every legendary = **all Epic effects of its root** (bumped to the Legendary numbers in §2) + the
unique clause below. Single-click shows the proper noun (`Hyperion [legendary]`); the slot shape
appears in the OPL subtitle (`a ring`). Per framework instruction for this family, clauses stay in
the **utility / resource-economy** lane (stamina, mana, regen, durability, resist windows) — raw
extra damage belongs to the weapon families, not jewelry.

### Olympian line (Zeus — storm and sky)

| Slot | Name | Unique clause |
|---|---|---|
| Ring | **Hyperion** | lightning proc also refunds 10 stamina |
| Bracelet | **Ouranos** | lightning proc has a 50% chance to also restore 10 mana |
| Necklace | **Aither** | the rolled stat bonus also applies at half value to a second stat of your choice (P18) |
| Earrings | **Astraios** | lightning proc grants +10% magic resist for 3s (P20) |

### Hecatean line (Hecate — moon and witchcraft)

| Slot | Name | Unique clause |
|---|---|---|
| Ring | **Selene** | mana leech proc also restores 5 stamina |
| Bracelet | **Asteria** | on-kill: full mana restore (P23) |
| Necklace | **Phoibe** | mana regen bonus doubles while below 25% mana |
| Earrings | **Theia** | mana leech proc grants +10% magic resist for 3s (P20) |

### Tychean line (Tyche — fortune)

| Slot | Name | Unique clause |
|---|---|---|
| Ring | **Ananke** | a halved hit also triggers a 3s HP/stam/mana regen pulse (P17) |
| Bracelet | **Metis** | a re-rolled miss becomes a guaranteed graze that restores 10 stamina |
| Necklace | **Nemesis** | items you wear take no durability loss for 3s after a halved hit (P22) |
| Earrings | **Themis** | halving a hit grants +10% magic resist for 3s (P20) |

### Nyxian line (Nyx — night)

| Slot | Name | Unique clause |
|---|---|---|
| Ring | **Hypnos** | breaking stealth by attacking refunds the stamina cost of that swing |
| Bracelet | **Khaos** | HP/stam/mana regen doubles while hidden (P17) |
| Necklace | **Moros** | successfully hiding restores 10 mana |
| Earrings | **Achlys** | poison resist bonus doubles while hidden |

### Demetrian line (Demeter — harvest)

| Slot | Name | Unique clause |
|---|---|---|
| Ring | **Gaia** | potions also restore 10 stamina |
| Bracelet | **Rhea** | regen bonus doubles for 5s after drinking any potion |
| Necklace | **Tethys** | potions also restore 10 mana |
| Earrings | **Okeanos** | on-kill: instantly gain the effect of your strongest held regen potion (P23) |

## 4. Legendary registry (claimed names — review pass merges these)

Hyperion, Ouranos, Aither, Astraios, Selene, Asteria, Phoibe, Theia, Ananke, Metis, Nemesis,
Themis, Hypnos, Khaos, Moros, Achlys, Gaia, Rhea, Tethys, Okeanos. (20)

*Merge-pass note (2026-07-07):* Chaos respelled Khaos (avoids UO Order/Chaos shield collision);
Hemera (goddess of day) swapped to Moros (doom-child of Nyx) — day didn't belong in the night line.

## 5. Crafting (deferred)

Per framework §11: crafted jewelry (per-slot unique names, Uncommon→Epic, same effect anchors)
exists in the future crafting pass, using mythic material naming. Nothing here blocks them.
