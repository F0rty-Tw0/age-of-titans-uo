# Pantheon Loot Bags — Targeted Farming (Spec)

Status: **bag system IMPLEMENTED 2026-07-15** (§2 drop rules, §3 map → `PantheonLootMap`, §4 bag
identity → `LootBag` v1, §5 roll → `LootRoller.Roll(bagLevel, domain)`, §7 wiring, §9 `[LootBag`
GM command; tests green). **§6 god sanctums NOT yet built** — content expansion pending.
Consumer of: `LootBag`, `LootRoller`, `PantheonFx.GetDomain`, `LegendaryRegistry`, the bestiary
family prefixes (`dev-docs/beast-reference.md`). Owns one new map (creature family → domain) and
the **god-sanctum content expansion** (§6). Framework (`00-framework.md`) stays law; this doc adds
a *source* dimension to bag drops.

## 1. Concept

Every themed creature family serves a god. 70% of its bags carry that god's mark, and a marked
bag holds ONLY that god's gear — at every rarity tier. Players target-farm a pantheon by farming
its dungeons; every god's flagship dungeon ladders all the way to a bag-10 avatar boss, so every
god's legendaries have a guaranteed farm route.

## 2. Drop rules

| Rule | Value |
|---|---|
| Themed mob drop split | **70%** god's bag / **30%** generic bag (rolled at drop time) |
| Themed bag contents | **100% god-locked**, every rarity tier — no leaks |
| Elites & bosses (`DungeonElite`) | **always** the god's bag (no 70/30) |
| Generic bags | today's roll, unchanged — all themes, all rarities, legendaries included |
| Rarity weights | `LootRoller._rarityWeights` untouched (bag 9 = 90/10 Epic/Legendary, bag 10 = 100% Legendary) |
| Bag 10 sources | every god's **avatar boss** (§6) — a themed bag 10 = guaranteed god-locked legendary |
| Hades distinction | hardest avatar fight + his unique bonus bag 9 (1×10 + 1×9 stays his signature) |
| Quest bags (Ferryman's Toll) | Underworld-themed (barrow flavor) |
| Stock/neutral mobs, Labors | generic bags (per-Labor domains later — out of scope) |

No exclusivity redirect: stock classics (Balron, Dragon, Daemon…) keep their existing legendary
odds via generic bags. Themed farming is *faster and targeted*, not the only path.

## 3. Family → god map

One prefix table (the bestiary guard-test idiom), plus a per-type override hook for specials.

| Domain (god) | Creature prefixes | Sources (flagship **bold**) |
|---|---|---|
| Sea (Poseidon) | Tide, Brine, Shore | **Drowned Tholos**, Shame, coasts |
| Forge (Hephaestus) | Cinder | **Cinderworks** |
| Hunt (Artemis) | Wyld, Lykai | **Nemean Wildwood**, Orc Caves |
| Sky (Zeus) | Storm, Peak | **Stormcrown Aerie**, mountains |
| Underworld (Hades) | Stygian, Drowned, Restless, Newbie | **Stygian Deep**, Deceit, graveyards, Barrow |
| Nature (Demeter) | Gaian, Grove, Pelasg | **Despise**, forests, Painted Caves |
| War (Ares) | Tartarus, Drakon, Myrmi, Wayman | **Hythloth**, Destard, Solen Hive, Wrong |
| Night (Nyx) | Cursed, Mire, Ophian | **Khaldun**, swamps, Terathan Keep |
| Wind (Hermes) | Rime | **Ice** (Boreas) |
| Aegis (Athena) | Argus | **Covetous** (Argus Panoptes — the Aegis chain lanes ARE the watchman line: Egregoros/Phylax/Phrourion) |
| Sun (Apollo) | Pyre | **Fire** (Prometheus's stolen flame → Apollo's radiance) |

All 11 domains covered. Named singles (Enkelados, Minos, …) resolve via their family's map row
or the override hook.

Map audit 2026-07-14 (vs `PantheonFx.GetDomain` root themes): Argus moved Sun→Aegis, Pyre moved
Forge→Sun, Wayman moved Aegis→War (Theseus's road villains are killers, not wardens; War's
13-lane pool feeds four families).

**Coverage note (accepted)**: gods own unequal drop-lane pools — War 13, Aegis 11, Hunt 10,
Underworld 9, Forge 8, Night/Nature 7, Sun/Wind 5, Sky 3, **Sea 2** (maces + shields only,
≈82/18 after renormalization). Nature drops **no weapons** (nymph lines — by design). Thin
pools = strong identity; accepted as-is. Lane expansion for Sea/Sky, if ever, is a
`00-framework.md` change — separate scope. LegacyRoots are decode-only, never rolled.

## 4. Bag identity

`LootBag` serialization v1 (MigrateFrom V0): optional `Domain` (nullable byte).

- Name: `a loot bag of Poseidon` (via `PantheonFx.GetPatronName`)
- Hue: domain hue (`PantheonFx.SampleHue(domain)`), overrides the level hue
- Single-click: `[level N]` unchanged; OPL adds the god line
- Generic bags unchanged (null domain = today's behavior exactly)

## 5. Roll mechanics

New overload `LootRoller.Roll(bagLevel, PantheonDomain domain)` (+ `RollDecision` counterpart,
kept pure/testable):

1. **Static init**: per-domain candidate tables — for each domain, which weapon families,
   armor materials, shield/jewelry/clothing lanes contain roots with `GetDomain(root) == domain`.
2. **Category roll renormalizes** over the categories that exist for that domain (a god with no
   jewelry lane never drops jewelry — correct, not a bug).
3. Theme roll picks uniformly among the domain's roots within the chosen family/material.
4. Rarity roll unchanged (same `_rarityWeights`); Legendary resolves via
   `TryGetByRootAndBase` as today — domain-locked automatically because the theme is.
5. Generic path (`Roll(bagLevel)`, no domain): byte-for-byte today's behavior.

## 6. God sanctums — ladder extension to L10 (content expansion)

Each god's **flagship dungeon** gains a sanctum wing: the family's ladder extends to L10 with
~10 new ranks (fill from the family's current top up through 10), crowned by an **avatar boss**.

- **Avatar boss**: L10 `DungeonElite`, `EliteBagLevel = 10` — guaranteed god-locked legendary.
  Named champion/aspect of the god (Hades precedent: the god may appear personally). One spawn
  point, count 1, long respawn (Hades convention).
- **Sanctum ranks**: L8-9 champions + L6-7 bridge ranks as each family needs; trash-bag rule
  unchanged (`LootBagLevel = pin − 1`), so sanctum trash drops bags 7-9 — themed 70/30, giving
  each god a farmable legendary tail (bag 9 = 10% legendary) below the boss.
- Existing tops: Stygian already L9-10 (Hades = the Underworld avatar, unchanged); Aerie L8-9
  (needs avatar + thin fill); the other 9 flagships climb from L5-8.
- ~100+ new creatures total; same family prefixes → guard tests, `[Beast`, beast-reference,
  and this map cover them automatically. Pins in `LevelConfig.MobLevelOverrides` as usual.
- **Placement**: placeholder coords in each flagship's spawn file (sanctum section), user
  GM-places like the Five Domains. Region polish (light/music) optional per dungeon.
- Donor-copy sweep applies (CantWalk / FightMode.Aggressor / GetWeaponAbility / PoisonImmune).

## 7. Wiring

- `PantheonLootMap` (new, `Engines/LootBags/`): prefix table + `Type` override dict +
  `TryGetDomain(BaseCreature, out PantheonDomain)`.
- `BaseCreature` drop site (~`:3035`): resolve domain; if themed, 70/30 → themed or generic bag.
- `DungeonElite.OnDeath`: always themed when the map resolves.
- Ferryman quest rewards: construct with Underworld.

## 8. Tests

- Every domain rolls ≥1 valid combo at every rarity (guard).
- Themed decision never returns a theme outside its domain (sampled).
- Generic decision identical to today (existing tests keep passing untouched).
- 70/30 mob-drop split within tolerance (drop-site logic factored to a pure helper).
- Every bestiary family prefix resolves in `PantheonLootMap` (reflection guard, same idiom as
  `BestiaryGuardTests`).
- Sanctum guard: every domain has ≥1 `DungeonElite` with `EliteBagLevel == 10` (avatar).

## 9. GM testing

`[LootBag <level> [god]` — spawn a bag (themed if god given) in your pack. Documented in
`dev-docs/gm-testing-commands.md`.

## Out of scope

- Per-Labor domains, seasonal/event bags.
- Altar changes (domain-reroll already exists and remains the cross-domain valve).
- Rarity-weight or drop-chance retuning.
