# Newbie Dungeon — Mob Bestiary

Reference for every creature in the Barrow of the Unremembered. All values
pulled from the source files in `Projects/UOContent/Mobiles/NewbieDungeon/`
(2026-07-14). Design intent per mob: `dev-docs/newbie-dungeon.md` §3.
Setup/spawn placement: `dev-docs/newbie-dungeon-setup.md`.

Shared facts:

- All mob levels are **pinned** in `LevelConfig.MobLevelOverrides` — HP tuning
  can change freely without shifting the `[lvl N]` tag, XP, or bag level.
- Trash rolls the global loot-bag chance (`LootBagConfig.ChanceForMobLevel`)
  but overrides `LootBagLevel` DOWN (L1 mobs → bag 0, L2 → bag 1) so trash
  never reaches bag 2's Rare table. Elites skip the roll and force-drop.
- All undead are in `OppositionGroup.FeyAndUndead` and `BleedImmune`.

## Level 1 — entry trash (bag 0, Uncommon-only)

### a frail skeleton — `NewbieBoneShade`
| | |
|---|---|
| Level tag | 1 (yellow to a fresh level-0 player) |
| Body / hue | Skeleton 50/56, pale bone 0x03B2 |
| HP / dmg | 25–35 / 2–4, slow (speed 0.3/0.6), Wrestling 25–35 |
| Immunities | Bleed, Lesser poison |
| Teaches | targeting + the kill loop — first safe kill |

### a barrow rat — `NewbieGraveRat`
| | |
|---|---|
| Level tag | 1 |
| Body / hue | GiantRat 0xD7, sickly grey 0x0481 |
| HP / dmg | 20–30 / 1–3, FAST mover (speed 0.1/0.2), weakest hitter |
| Teaches | movement/chasing — it runs, you follow |

### a shambling corpse — `NewbieCorpseCrawler`
| | |
|---|---|
| Level tag | 1 |
| Body / hue | Zombie 3, unhued |
| HP / dmg | 30–45 / 2–5, slow (0.3/0.6) |
| Immunities | Bleed, Lesser poison |
| Teaches | nothing new — pacing filler, slightly tankier |

## Level 2 — deeper trash (bag 1, Uncommon-only)

### a grave-touched ghoul — `NewbieGraveMiasma`
| | |
|---|---|
| Level tag | 2 (red to a level-0 player, white at 2) |
| Body / hue | Ghoul 153, underworld green 0x0851 |
| HP / dmg | 70–90 / 3–6, Poisoning 40–50 |
| Special | **HitPoison = Lesser** — melee applies poison |
| Immunities | Bleed, Lesser poison |
| Teaches | the buff-bar read + cure/bandage reaction — first status effect |

### a restless dead — `NewbieRestlessArcher`
| | |
|---|---|
| Level tag | 2 |
| Body / hue | Skeleton 50/56, ash grey 0x0385 |
| HP / dmg | 66–85 / 3–6, Dex 80–95 — fastest attacker in the dungeon |
| Immunities | Bleed, Lesser poison |
| Teaches | positioning/kiting — standing still and trading hits hurts |
| Note | melee despite the name (no bow); rename candidate if it confuses |

## Level 3 — named elites (guaranteed bag 2, the only Rare source)

Shared via abstract `NewbieElite` base: `DropsLootBag => false` (opts out of
the global roll), `OnDeath` force-drops `LootBag(2)` + one `LootRoller.Roll(2)`
item into the corpse, telegraphed with sparkle 0x3728 + shimmer sound 0x1F2.
Bag 2 rarity: 80/20 Uncommon/Rare. Tuning target: a solo top-of-gate (level 3)
character should NOT reliably win — **unvalidated, needs live tuning pass**.
All three: Fame 1500 / Karma −1500, LootPack.Meager side loot, ~15 min respawn.

### the Boatless Ferryman — `NewbieCharon` (foreshadows HERMES)
| | |
|---|---|
| Body / hue | Headless One 31, deep shadow 0x0455 |
| HP / dmg | 180–220 / 8–14, Wrestling 70–80, VirtualArmor 45 |
| Role | the balanced brawler; story hook — demands a coin you don't have |

### Anax, the Unyielded — `NewbieFallenChampion` (foreshadows ARES)
| | |
|---|---|
| Body / hue | Bone Knight 57, dull blood 0x0021 |
| HP / dmg | 200–240 / 10–16, Tactics+Wrestling 90–100, VirtualArmor 50 |
| Special | 60% COLD damage split — hits through low-level physical resists |
| Role | the hardest hitter; the "bring a friend" wall |

### the Warden of the Gate — `NewbieHollowWarden` (foreshadows HADES)
| | |
|---|---|
| Body / hue | Bone Magi 148, gold-ochre 0x08A5 |
| HP / dmg | 170–200 / 7–12, **AI_Mage**, Magery/EvalInt 70–80 |
| Immunities | Bleed, Regular poison (stronger than trash) |
| Role | the caster; stands before the sealed deeper gate (decor) |
| Note | T2A-clean spell kit — no Necromancy (AOS-era, deliberately dropped) |

## Non-combat

### the Ferryman's Shade — `NewbieFerryman` (greeter)
| | |
|---|---|
| Body / hue | Headless One 31, 0x0455 — visually rhymes with the Charon elite |
| Behavior | Blessed (invulnerable) + CantWalk; a prop, not a combatant |
| Speech | "Fresh blood. The dead ahead are weak — go blood them." — fires once per range-approach (4 tiles), leaving+returning re-arms it |

## Quick tuning map

| Want to change | Touch |
|---|---|
| A mob's XP/tag/bag level | `LevelConfig.MobLevelOverrides` pin (NOT its HP) |
| Trash bag drop % | `LootBagConfig.ChanceForMobLevel` (shard-wide!) |
| Elite difficulty | HP/damage/skills in the elite's own file |
| Elite reward | `NewbieElite.OnDeath` (bag level, FX) |
| Poison strength | `HitPoison` on NewbieGraveMiasma |
| Greeter line | `NewbieFerryman.OnMovement` |
