# Newbie Dungeon — Design (v3, approved 2026-07-14)

Safe on-ramp dungeon for fresh characters. Consumer of existing systems
(Leveling, Rarity bags, PantheonIchor salvage) — owns no new systems.

v3 adds theme, mob roster, aesthetics, and first-session hooks on top of the
v2 structure. Draft v1's "prerequisite systems" section is obsolete.

## 1. Decisions vs. draft v1

| Draft v1 item | Decision | Why |
|---|---|---|
| Build EXP/level system first | Already exists | `Projects/UOContent/Engines/Leveling/` — numbers design-final |
| Item level-req framework | Cut | YAGNI: dungeon drops Uncommon only; revisit if twinking hurts |
| Power Score | = character level | Level already gates skills (50→100) and stats |
| Account-level access gating | Cut → char-level gate | Alt prize pool (safe Uncommon/Ichor trickle) not worth account plumbing |
| Elite instancing | Cut → contested shared spawn | Classic UO; zero instancing machinery |
| Bags capped at Common (§6.2) | Replaced by framework table | Framework law: bags never drop Common (directive 2026-07-08) |
| New salvage currency | Cut → PantheonIchor | Salvage/upgrade hub already committed |
| Talent system | Deferred | Not required for launch (unchanged from v1) |
| XP pacing | Real curve everywhere | User decision 2026-07-14: delete FirstLevelXP knob, L1 = 3750. Known risk: first ding lands ~18–25 min in; first session leans on entrance mood + bag lottery |

## 2. Theme — The Barrow of the Unremembered

A sunken burial mound at the edge of the living world: the shallow antechamber
of the underworld, where the forgotten dead stir because no coin was placed on
their eyes. The new player's first hour is *katabasis* — go down, survive,
return changed. Undead = richest native T2A art; the place is the HADES domain
made walkable and quietly advertises the Stygian legendary lane.

- Greeter NPC at the mouth: **the Boatless Ferryman** (also elite #1's story
  hook). One spoken line, no tutorial gump: *"Fresh blood. The dead ahead are
  weak — go blood them."*
- A visibly **sealed deeper gate** in the elite depth — the promise of real
  Hades content later. Decor only, no mechanics.

## 3. Structure & mob roster

Mob levels 1–2 (level-0 mobs award zero XP by design — `LevelConfig.GapHue`).
Entry player is level 0: L1 mobs read yellow, L2 red — the hue system is the
tutorial. Classes prefixed `Newbie*`, folder
`Projects/UOContent/Mobiles/NewbieDungeon/`, natural display names, hue-only
re-skins of T2A bodies.

**Level 1 (entry):**

| Class | Display | Body | Hue | Teaches |
|---|---|---|---|---|
| `NewbieBoneShade` | a frail skeleton | Skeleton | pale bone ~0x03B2 | targeting + kill loop |
| `NewbieGraveRat` | a barrow rat | GiantRat | sickly grey ~0x0481 | movement |
| `NewbieCorpseCrawler` | a shambling corpse | Zombie | unhued | pacing filler |

**Level 2 (deeper):**

| Class | Display | Body | Hue | Teaches |
|---|---|---|---|---|
| `NewbieGraveMiasma` | a grave-touched ghoul | Ghoul/Zombie | underworld green ~0x0851 | poison → buff-bar read + cure |
| `NewbieRestlessArcher` | a restless dead | Skeleton | ash grey ~0x0385 | burst speed → kiting/positioning |

**Level 3 elites (3 named, contested shared spawns, ~15 min respawn placeholder):**

| Class | Display | Body | Hue | Foreshadows |
|---|---|---|---|---|
| `NewbieCharon` | the Boatless Ferryman | Headless One | deep shadow ~0x0455 | HERMES (guide of the dead) |
| `NewbieFallenChampion` | Anax, the Unyielded | Skeletal Knight | dull blood ~0x0021 | ARES (hits hardest) |
| `NewbieHollowWarden` | the Warden of the Gate | Bone Magi | gold-ochre ~0x08A5 | HADES (guards the sealed gate) |

Elite tuning check: a solo top-of-gate (level 3) character should NOT reliably
solo an elite — grouping pressure is the graduation lesson. Verify against a
level-3 char's DPS, not a vet's.

## 4. Loot

- Bag drops: L1 mob → **bag 0** (~1%), L2 mob → **bag 1** (~5%),
  elite → **bag 2** guaranteed. Percentages are placeholders — tune live.
- Rarity free via `RarityConfig.MaxRarityForBagLevel` + framework weights:
  bags 0–1 = 100% Uncommon; bag 2 = 80/20 Uncommon/Rare. Elites are the only
  Rare source; main game stays strictly better.
- **Bag-drop telegraph**: when a bag rolls, fire a rarity-hued sparkle +
  shimmer sound on the corpse (`Effects.SendLocationEffect` + `PlaySound`) and
  a private overhead on the killer ("Something glimmers in the remains").
  No region/server announce — dungeon max is Rare, never announces.
- Salvage: standard PantheonIchor flow, no dungeon-specific rules.

## 5. Graduation & keepsake

- Entry gate: region `OnEnter`, **character level ≤ 3** or ejected with a message.
- Graduation is automatic: at level 4 the gap curve grays out L1 mobs
  (`LevelConfig.GapMultiplier`, gap ≤ −3 → 0 XP). No internal doors.
- **Level-4 moment**: `Effects.SendBoltEffect` on the player + regional
  thunder — nearby players witness the graduation. Ferryman send-off line:
  *"Not yet your time. Go up — and remember the road down."*
- **The Ferryman's Coin** — one blessed (`LootType.Blessed` + Newbied) trinket
  handed once per character at level 4. Pure keepsake: no stats, no trade
  value, named item only. Endowment hook, one class, one flag.
  (User chose "simpler" over the fragment-collection/shrine/trophy systems —
  those live in the brainstorm archive if wanted later.)

## 6. First-session polish (Tier 1, all small)

1. **Loud level-ups** (currently silent — #1 churn killer): chime
   (`PlaySound`) + golden sparkle (`FixedParticles` ~0x373A) + overhead
   "LEVEL N" + buff-bar icon via `BuffHelper.AddBuff` showing new skill caps.
   Lives in the level-up path, benefits the whole shard.
2. **Zone mood via config**: per-region `AlterLightLevel` ramp (entry ~6 →
   L1 ~12 → L2 ~20 → elite ~26 near-black), per-zone `Region.Music`
   (Approach → Cave01 → Dungeon2 → Death/StygianAbyss), engine auto-swaps.
3. **Threshold flashes**: `SendScreenEffect(DarkFlash)` descending into L2,
   `LightFlash` into elite chambers — region OnEnter or `EffectController`.
4. **Elite chamber branding**: `PantheonFx.PlayForDomain(..., Underworld, ...)`
   grave-mist burst on chamber entry — existing wrapper, one call.
5. **Ambient triggers**: GM-placed `EffectController` nodes (InRange) for
   drips, distant growls, corridor smoke. Config, not code.

## 7. Safety region

One custom `Region` subclass over the dungeon area:

1. Player↔player harmful acts blocked — must cover indirect paths
   (fields, AoE, summons/pets). Verify every path routes through
   `CanBeHarmful`/region checks at implementation time.
2. No criminal/murder flagging inside.
3. Corpse looting owner-only.

## 8. Hard-refuse list (do not add, ever)

- Guaranteed Rare (or any power item) on graduation — twinking pipeline.
- Tradeable cosmetics priced in Ichor — gold-laundering/alt-farm engine.
- Daily-login reward escalators — rewards staying in the tutorial.
- Text-wall onboarding gumps — teach via mob design + hue tags instead.

## 9. Launch checklist

- [ ] Delete `LevelConfig.FirstLevelXP = 1` testing knob + its `level == 1`
      branch (`LevelConfig.cs:49`; L1 must cost 3750 — user decision, real
      curve everywhere).
- [ ] Verify indirect-harm coverage (fields/AoE/pets) in the region rules.
- [ ] Serialization review on new mob classes + Ferryman's Coin (dupe footgun).
- [ ] Verify `Effects.ParticleSupportType` behavior on ClassicUO
      (`Effects.cs:58-62` defaults to Detect → particles may be withheld
      shard-wide, PantheonFx procs included). If confirmed, set
      `ParticleSupportType.Full` at boot — shard-wide fix, not dungeon-only.
- [ ] Elite HP/damage tuned so a solo level-3 cannot reliably win.
- [ ] Tuning pass: bag drop %s, elite respawn timer.

## 10. Open (decide at implementation)

1. Physical location — which map area hosts the barrow.
2. Elite respawn timer + drop % values (placeholders above).
3. First-kill-of-type XP bonus — leveling-system feature, out of scope here.
4. Monitor first-session churn after launch: with the real 3750 curve the
   first ding lands ~18–25 min in. If retention data says players quit before
   it, revisit the dungeon-local XP option (rejected for launch 2026-07-14).

## Implementation notes (2026-07-14)

Corrections against the v3 draft above, discovered during implementation:

- **Level-ups were not silent.** `LevelSystem.ApplyLevelUp` already had a
  particle burst + sound + chat message before this pass (§6.1 mischaracterized
  this as a from-scratch feature). What was actually added: the overhead
  "LEVEL N" text and the buff-bar icon — those two were the real gap.
- **Coin flag correction.** The Ferryman's Coin is `LootType.Blessed`, not
  "Blessed and Newbied" as §5 says — `Item.cs` makes the two flags mutually
  exclusive, so it can only carry one.
- **Buff API name correction.** The buff-bar hook is `BuffHelper.AddCustomBuff`,
  not `BuffHelper.AddBuff` as §6.1 says.
- **Greeter naming clash.** The greeter NPC's in-game name is "the Ferryman's
  Shade" (`NewbieFerryman`), distinct from the elite `NewbieCharon`'s display
  name "the Boatless Ferryman" — both were drawn from the same story hook (§2),
  so the names were split to avoid confusion between the two.
- **NewbieGraveMiasma body.** Uses the Ghoul body (not a re-skinned Zombie) —
  closer to the "grave-touched" read the display name implies.
- **NewbieHollowWarden loot.** Does not drop Necromancy reagents/items —
  Necromancy is AOS-era content and this shard targets T2A (see
  `dev-docs/itemization/00-framework.md` era notes).
- **Placeholder coordinates locked in for implementation**: Felucca,
  x 6700–6850 / y 700–750, split into four sibling regions (entrance, L1
  trash, L2 trash, elite depth) — still GM-repositionable, per §10 item 1.
- **Launch checklist item done**: `LevelConfig.FirstLevelXP` knob and its
  `level == 1` branch are deleted; level 1 now costs the table's 3750 XP
  everywhere. `[SetLevel`/`[GiveXP` (see `dev-docs/gm-testing-commands.md`)
  replace the knob for testing.
- **Setup runbook**: `dev-docs/newbie-dungeon-setup.md` — coordinates,
  regions/spawner JSON, boot smoke, dressing, tuning knobs, troubleshooting.
- **Quest chain added**: "The Ferryman's Toll", a 5-quest once-per-character
  chain on `NewbieFerryman` (MLQuests engine — flag flipped
  `questSystem.enableMLQuests: True`). Definitions + custom
  salvage/upgrade/reach-level objectives:
  `Engines/ML Quests/Definitions/FerrymansToll.cs`; setup in the runbook §9.
