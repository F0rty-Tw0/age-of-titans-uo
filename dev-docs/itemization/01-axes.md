# 01 — Axes (Swordsmanship)

**Derives from:** `00-framework.md` (budgets §2, themes §3, effect pattern §4, primitives §5,
ladder §7). This doc is the **canonical format** for every family doc.
**Source spec:** user-approved axe spec (2026-07-07), re-themed to the Greek pantheon. All damage,
speed, and budget numbers carried over unchanged; effect text adapted only where the framework
bans a mechanic (movement speed, cooldowns, disarm-immunity — substitutions per §5).

---

## 1. Base ladder & damage matrix

Damage = `ratio × D[rarity]`, D = (10, 13, 17, 22, 30). Speed = swing seconds (bigger =
slower). DPS columns are base values before speed effects.

| Base | Ratio | Speed | Common | Uncommon | Rare | Epic | Legendary | Leg. DPS |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Hatchet | 0.65 | 2.75s | 6.5 | 8.5 | 11.1 | 14.3 | 19.5 | 7.09 |
| Axe | 0.70 | 2.85s | 7.0 | 9.1 | 11.9 | 15.4 | 21.0 | 7.37 |
| Battle axe | 0.75 | 2.95s | 7.5 | 9.8 | 12.8 | 16.5 | 22.5 | 7.63 |
| Double axe | 0.80 | 3.05s | 8.0 | 10.4 | 13.6 | 17.6 | 24.0 | 7.87 |
| Executioner's axe | 0.85 | 3.15s | 8.5 | 11.0 | 14.4 | 18.7 | 25.5 | 8.10 |
| Two-handed axe | 0.90 | 3.25s | 9.0 | 11.7 | 15.3 | 19.8 | 27.0 | 8.31 |
| Large battle axe | 0.95 | 3.35s | 9.5 | 12.3 | 16.1 | 20.9 | 28.5 | 8.51 |
| Ornate axe | 1.00 | 3.45s | 10.0 | 13.0 | 17.0 | 22.0 | 30.0 | 8.70 |

Commons are the plain base items: stock name, no hue, no effects.

## 2. Drop-variant themes (shared across all 8 bases)

*Roots and mechanics unchanged by the 2026-07-07 per-family re-theme — Zephyr, Phobos, Agrotera,
Pallas, and Stygian are now axe-exclusive roots (framework §3).*

Names read `[root] [base] [rarity]` — e.g. `phobos battle axe [rare]`. One effect package per
root per rarity; only the damage number differs by base. Hues: god run from framework §3,
shade by rarity.

| Root | God | Uncommon | Rare | Epic |
|---|---|---|---|---|
| **Zephyr** | Hermes | +8% swing speed | +8% swing speed, +6% hit chance | +10% swing speed, +8% hit chance, 10% extra-swing proc |
| **Phobos** | Ares | +8% damage | +8% damage, +8% crit chance | +10% damage, +10% crit chance, +20% crit damage |
| **Agrotera** | Artemis | 6% Huntress' Mark chance (mark: +8% dmg from you, 5s) | 8% mark chance, mark +10% | 10% mark chance, mark +14%, marked target also takes a poison tick |
| **Pallas** | Athena | +6% block | +6% block, 8% DR on block | +8% block, 12% DR on block, brief thorns after a block |
| **Stygian** | Hades | +6% lifesteal | +6% lifesteal, +6% stam regen | +8% lifesteal, +8% stam regen, lifesteal ×2 vs targets <30% HP |

## 3. Legendaries (unique per base × theme)

Every legendary = **all Epic effects of its root** + the unique clause below. Damage from §1
Legendary column. Single-click shows the proper noun (`Labrys [legendary]`); base shape appears
in the tooltip.

### Zephyr line (Hermes — winds & harpies)

| Base | Name | Unique clause |
|---|---|---|
| Hatchet | **Aello** | guaranteed extra swing after a successful parry; killing blow restores 25 stamina |
| Axe | **Boreas** | guaranteed extra swing every 5th hit; each extra swing refunds its stamina cost |
| Battle axe | **Podarge** | guaranteed extra swing every 5th hit; killing blow restores 25 stamina |
| Double axe | **Ocypete** | guaranteed double strike every 5th hit; the second strike always crits |
| Executioner's axe | **Euros** | guaranteed extra swing on the first hit of any fight; on-kill: full stamina |
| Two-handed axe | **Notos** | guaranteed extra swing every 5th hit; that swing staggers (1s stun, §9 immunity) |
| Large battle axe | **Celaeno** | guaranteed extra swing every 5th hit; extra swings ignore 10% armor (P27) |
| Ornate axe | **Aellopos** | guaranteed extra swing every 5th hit; extra swings also leech 5% stamina |

### Phobos line (Ares — war's circle)

| Base | Name | Unique clause |
|---|---|---|
| Hatchet | **Ker** | guaranteed crit on the first hit of any fight; crits deal 10% splash (3 targets) |
| Axe | **Enyo** | guaranteed crit every 5th hit; crits ignore 10% armor (P27) |
| Battle axe | **Alala** | guaranteed crit every 6th hit; crits deal 10% splash (3 targets) |
| Double axe | **Labrys** | guaranteed crit every 5th hit; crits deal 15% splash (3 targets) |
| Executioner's axe | **Polemos** | guaranteed crit every 6th hit; crits deal double damage vs targets below 15% HP |
| Two-handed axe | **Deimos** | guaranteed crit every 6th hit; crits deal 10% splash (3 targets) |
| Large battle axe | **Eris** | guaranteed crit every 5th hit; crits deal 10% splash (3 targets) |
| Ornate axe | **Enyalios** | guaranteed crit every 6th hit; crit kills restore 25% of max HP's stamina equivalent (full stam if lower) |

### Agrotera line (Artemis — the hunt)

| Base | Name | Unique clause |
|---|---|---|
| Hatchet | **Taygete** | guaranteed mark on the first hit of any fight; up to 3 nearby allies of the target are marked at half bonus |
| Axe | **Kallisto** | guaranteed mark on the first hit of any fight; marked targets take +25% damage from all sources |
| Battle axe | **Britomartis** | guaranteed mark on the first hit of any fight; on the marked target's death the mark jumps to up to 3 nearby enemies |
| Double axe | **Oupis** | guaranteed mark on the first hit of any fight; marked targets take +25% damage from all sources |
| Executioner's axe | **Aktaion** | guaranteed mark on every crit; marked targets cannot be healed for 3s (P25 cap) |
| Two-handed axe | **Orion** | guaranteed mark on the first hit of any fight; marked targets take +25% damage from all sources |
| Large battle axe | **Sagaris** | guaranteed mark on the first hit of any fight; splash hits from this weapon also mark |
| Ornate axe | **Atalanta** | guaranteed mark on the first hit of any fight; your poison ticks on marked targets are doubled |

### Pallas line (Athena — the shield-arm)

| Base | Name | Unique clause |
|---|---|---|
| Hatchet | **Itonia** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) |
| Axe | **Alalkomeneis** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) |
| Battle axe | **Promachos** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) |
| Double axe | **Gorgoneion** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (1s stun, §9) |
| Executioner's axe | **Glaukopis** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) |
| Two-handed axe | **Tritogeneia** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (1s stun, §9) |
| Large battle axe | **Hippia** | the first hit taken each fight is reflected for 20% of its damage and staggers the attacker (1s stun, §9) |
| Ornate axe | **Parthenos** | guaranteed block vs the first hit of any fight; blocking a crit briefly stuns the attacker (§9) |

### Stygian line (Hades — the underworld)

| Base | Name | Unique clause |
|---|---|---|
| Hatchet | **Lethe** | crits drain the target's stamina fully; lifesteal ×2 vs targets below 30% HP |
| Axe | **Acheron** | on-kill: full stamina and mana restore; lifesteal ×2 vs targets below 30% HP |
| Battle axe | **Kokytos** | guaranteed lifesteal proc on every crit; on-kill: full stamina restore |
| Double axe | **Phlegethon** | guaranteed lifesteal proc on every crit; on-kill: full stamina restore |
| Executioner's axe | **Charon** | on-kill: full stamina and mana restore; lifesteal ×2 vs targets below 30% HP |
| Two-handed axe | **Erebos** | on-kill: full stamina and mana restore; lifesteal ×2 vs targets below 30% HP |
| Large battle axe | **Tartaros** | guaranteed lifesteal proc on every crit; on-kill: full stamina restore |
| Ornate axe | **Thanatos** | on-kill: full stamina and mana restore; lifesteal ×2 vs targets below 30% HP |

## 4. Legendary registry (claimed names — review pass merges these)

Aello, Boreas, Podarge, Ocypete, Euros, Notos, Celaeno, Aellopos, Ker, Enyo, Alala, Labrys,
Polemos, Deimos, Eris, Enyalios, Taygete, Kallisto, Britomartis, Oupis, Aktaion, Orion, Sagaris,
Atalanta, Itonia, Alalkomeneis, Promachos, Gorgoneion, Glaukopis, Tritogeneia, Hippia, Parthenos,
Lethe, Acheron, Kokytos, Phlegethon, Charon, Erebos, Tartaros, Thanatos. (40)

## 5. Crafting (deferred)

Per framework §11: crafted variants (per-base unique names, Uncommon→Epic, same damage anchors)
exist in the approved source spec and will be re-themed to mythic materials in the crafting pass.
Nothing here blocks them.
