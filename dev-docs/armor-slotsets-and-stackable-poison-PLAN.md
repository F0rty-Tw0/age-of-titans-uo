# Execution Plan — Armor Slot-Sets + Stackable Poison

Living plan for two features designed 2026-07-10. Self-contained (does not depend on any scratchpad). Reprompt me by pointing at this file + a phase, e.g. *"Resume ARMOR Phase 3 from dev-docs/armor-slotsets-and-stackable-poison-PLAN.md."*

> **Not committed.** Everything below is uncommitted working-tree state unless noted. Never `git commit`/`push` without explicit ask.

---

## CURRENT STATE (2026-07-11)

- **ARMOR Milestone A: DONE, build green (exit 0), NOT runtime-verified, NOT committed.**
  - New `Projects/UOContent/Engines/Rarity/Effects/ArmorSlotSignatureTable.cs` — `(material × slot) → (ClauseType, S1,S2,S3)`, 26 cells populated + 4 `None`/TODO cells.
  - `WornEffectState.cs` — armor branch threads `IsShield/Material/Slot` through `armorItems`; signature read redirected at the `AppendSignature` site (`~:287`) with guard `!isShield && rarity >= Epic`.
  - `RarityEffects.Tooltips.cs` — both ARMOR tooltip sites redirected. Weapons/shields/accessories untouched.
- **ARMOR Phase 3: DONE, build green (0 errors, 0 warnings), NOT runtime-verified, NOT committed.**
  - 4 enum members added mid-enum after `ShrugFirstHitGuaranteed` (`LegendaryRegistry.cs` ~:82) — verified ClauseType is never serialized (items persist LegendaryId only), so mid-enum insert is save-safe.
  - 4 `ClauseText.Describe` arms; 4 table cells filled (Studded/Bone/Ringmail Chest + Plate Arms).
  - `RarityEffects.Defense.cs`: the 3 `ShrugFirstHit*` clauses joined the guaranteed-first-shrug loop; riders dispatch in the shrug switch (`when firstHit` gated); `ShrugFirstHitDrBurst` consume loop added in the DR section, gated `!firstHit` so the arming hit doesn't double-dip.
  - ⚠ Carried to P6: DrBurst stacks on agg.DrPct with no 12%-cap clamp (mirrors existing BlockGrantsDrBurst pattern).
- **ARMOR Phase 4: DONE, build green (0 errors, 0 warnings), NOT runtime-verified, NOT committed.**
  - Set detection: `epicPieces` counter in `WornEffectState.Rebuild` — counts **Epic+ non-shield** pieces per material (gate mirrors the slot-signature Epic+ gate; decision, not in plan text). `WornAggregate` gains `HasCapstone` + `CapstoneMaterial` (slot math proves at most one set can be complete).
  - Capstone buff icons: indefinite icon while set complete, synced add/remove in Rebuild vs previous aggregate + Evict. Icons: Evasion/InjectedStrike/DeathStrike/Block/Toughness/Knockout.
  - Effects: Leather +6 dodge folded pre-cap in Rebuild; Studded/Bone/Plate debuff trio in `ApplyArmorHitRiders` (WeaponHit.cs) — Venom=Lesser poison (interim, → stack later), Grave-Chill=2s heal-block (re-applies only after expiry), Siege-Shock=1s TryStun (10s immunity is rate limiter); Ward-Surge = 5s window on crit-taken (`CombatFxState.ArmWardSurge`), consume in Defense.cs DR section as **DR→cap(12)** (cap-fix #2 pre-applied); Phalanx-Thorns = inline +8 reflect clamped to 25-cap (no burst state — behaviorally identical since it'd re-arm every hit).
  - Burst duration 5s (Frenzy template default) — plan left it unspecified, tunable.
  - AbsorbForDefenderArmor early-out now also checks `HasCapstone: false`.
- **ARMOR Phase 5: DONE, build green (0 errors, 0 warnings), NOT runtime-verified, NOT committed.**
  - `WornEffectState.DedupeClauses` (called in Rebuild after all signature appends, before syncs): worn list collapses same-ClauseType entries, strongest wins. Covers all worn-side dispatch sites at once (they all iterate this list).
  - `Beats`: P1 desc, P2 desc; "every Nth" clauses (FlameProcEveryN/ParryRepairsEveryN) invert — smaller nonzero N wins; ties prefer shield-sourced (parry gates key off source), then real legendary (Id!=0) over synthetic signature. Known simplification: raw P compare — an entry relying on its dispatch-site default (P=0) loses to an explicit weaker value.
  - Per-item dual-clause seams (WeaponHit:~216, Defense:~108, Procs:~26) intentionally untouched — guarded by the registry's "unique != signature within a lane" invariant, not runtime dedupe.
  - `RarityEffects.IsShieldSourced` made internal for the tie-break.
- **ARMOR Phase 6: DONE, build green (0 errors, 0 warnings), NOT runtime-verified, NOT committed.**
  - Cap fix #1: `AdjustHitsRegenRate` (Worn.cs) clamps final effective regen `pct` to `HpRegenCap` (60) — fixes chain-legs ×3 breach AND all stacked multipliers (hidden ×2 + potion ×2 + crit ×3 could previously reach ×12 of base). Chose final-clamp over the plan's "base ≤20 or burst ×2" alternatives — subsumes both and every combination.
  - P3 flag resolved: `ShrugFirstHitDrBurst` consume folded into the DR calc, `Math.Min(drPct + P1, DrCap)` — no longer a second unclamped subtraction.
  - `HpRegenCap` made public alongside DrCap/ReflectCap.
  - Weapon-path `BlockGrantsDrBurst` (Defense.cs ~:40) — fixed too (user call, 2026-07-11): burst clamped to remaining DR headroom `min(S1, DrCap - agg.DrPct)`; a suit at the 12% cap gains nothing from the block window.
- **ARMOR Phase 7 (tests): DONE — full suite 876/876 green (`MODERNUO_TEST_DATA_DIR='F:\UO' dotnet test Projects/UOContent.Tests/...`). NOT committed. In-game verify still pending (list below).**
  - New `ArmorSlotSignatureTableTests.cs` (pure): populated cells incl. all 4 P3 cells, unpopulated/out-of-range → None, 30-cell global uniqueness.
  - New `ArmorSlotSetTests.cs` (sequential collection): Epic redirect to slot table / sub-Epic no-redirect; plate 4-piece set + break-on-removal; Epic-gate (Rare piece doesn't count); chainmail threshold 3; leather capstone +6 dodge; dedupe (strongest P1, EveryN inversion, shield-source/real-Id ties, different clauses untouched); ShrugFirstHitDrBurst end-to-end (shrug 50, then 92 via clamped +8 DR — advances `Core._tickCount` between hits because RegisterHitTaken is idempotent per tick); regen burst clamps to 60 (8s → 6.25s not 5.71s).
  - Drive-by fix (pre-existing flake surfaced by the new parallel test class): `HostileMobileNotorietyTests` hand-rolled a world boot in its cctor — raced the shared fixture in parallel, clobbered ServerConfiguration when serialized. Now calls the once-guarded `TestServerInitializer.Initialize()` + joined the sequential collection.
  - **In-game verify checklist (user):**
    1. Epic+ armor tooltips show the (material×slot) signature line; shields unchanged.
    2. Complete a 4-piece Epic plate set → indefinite "Siege-Shock" buff icon appears; remove a piece → icon gone. (Icons: Evasion/InjectedStrike/DeathStrike/Block/Toughness/Knockout — check they render sensibly in ClassicUO.)
    3. Studded Venom poisons on hit; Bone heal-blocks 2s; Plate staggers (once per 10s); Chain shows timed "Ward-Surge" (Protection icon) after taking a crit.
    4. Ringmail chest signature: first hit halved + "Fortified" float, following hits visibly reduced ~3s.
    5. Poison FCT/labels on Venom read correctly (interim Lesser poison until stackable-poison lands).
- **POISON PP1–PP5: DONE (2026-07-11), build green, suite 882/882 (6 consecutive clean runs), NOT committed, NOT in-game verified.**
  - `Server/Poison.cs`: new `PoisonStack` (Poison + per-stack From + TicksElapsed + LastDamage + Refresh).
  - `Mobile.cs`: `_poisonStacks` (cap `MaxPoisonStacks = 5`); `m_Poison` kept as strongest-stack MIRROR (all legacy reads — `Poisoned`, healthbar, cure levels — unchanged); `Poison = x` setter keeps legacy replace-all semantics; `AppendPoisonStack` (first stack constructs the timer = cadence anchor); `OnPoisonStackExpired` (prune + mirror recompute); `ApplyPoison(from, poison, bool refreshOnly = false)` — refresh path runs before the cap check; `CheckHigherPoison` re-semanticized to "stack cap reached".
  - `UOContent/Misc/Poison.cs` `PoisonTimer`: merged tick — sums per-stack damage (per-stack T2A math + LastDamage quirk + Darkglow/Parasitic riders per stack source), prunes expired, ONE AOS.Damage attributed to the oldest stack's source, buff label `"-{total} poison x{n}"` re-sent only on count change. Ctor no longer sets `From = victim` (old oddity); `From` is fallback attribution only.
  - PP3 wired: PoisonField/GasTrap/FactionGasTrap/PuzzleChest/Food pass `refreshOnly: true`.
  - PP5 reconciled by reading: EvilOmen IncreaseLevel applies to the appended stack; `TryResistPoisonApplication` consulted per application via CheckPoisonImmunity; `timer.From = from` in PlayerMobile/BaseCreature kept as harmless fallback.
  - **Not serialized** — confirmed `m_Poison` was never in Mobile serialization.
- **POISON PP6: stack-model tests DONE** (`PoisonStackTests.cs`: append+mirror, cap-ignore at 6th, cure-all, area-refresh re-arm, expiry-shrink+mirror-recompute, legacy setter replace-all). **Merged-tick damage math NOT unit-tested** (timers never fire without the game loop) — in-game checklist: poison with 2-3 stacks shows ONE summed tick number; "-X poison xN" buff label; number shrinks as stacks expire; cure clears all; standing in a poison field refreshes instead of stacking. **Balance pass (§Balance watch) pending playtest.**
- **Loose-ends pass (2026-07-11, suite 884/884 ×3): DONE, NOT committed.**
  - `SpellDrVsPoisonDot` was a declared **no-op** (registry/text/tables, zero dispatch sites) — the plan's verify-at-code suspicion was right. Now wired: `RarityEffects.ReducePoisonTickDamage` (enabler clause routes `GetSpellDrPct` onto the merged poison tick), called from `PoisonTimer.OnTick`. Tests: enabler reduces (Skylla + Tritonian coif), non-enabler suit passes through.
  - 🔴 **Design-table bug found**: `BaseArmor.BodyPosition` maps `Layer.Neck → Gorget`, and mempos are Neck-layer — `StuddedMempo` is a GORGET, not a helm. The Studded-Helm cell is unreachable via armor (no studded helm item exists); clause still reachable via Skylla/Megareus legendaries. Cell left in place (harmless, future-proof) — matrix line 78's "*Studded Helm = StuddedMempo*" assumption is wrong.
  - Venom capstone "interim" comment updated — with global stacking live, every Venom hit now appends a Lesser stack (cap 5) automatically, exactly the design's end-state.
- **GM testing commands (2026-07-11): DONE, build green, NOT committed.** Documented in `dev-docs/gm-testing-commands.md` (+ CLAUDE.md table row). Existing `[Legendary <id>]` extended to accept exact names (RarityCommands.cs); new `RarityTestCommands.cs` adds `[GenVariant <root> <rarity>]`, `[ClearVariant]`, `[GenArmorSet <material> [rarity=Epic] [root]]` (full set to backpack; default deals the material's 5 thematic roots round-robin, explicit root = uniform). `[LootTest <level 0-10> [count]]` already existed for loot bags. Loot roller remains the production path (later phase).
- **Test-infra root-cause fix (drive-by, required for a trustworthy gate):** the intermittent 12-test pathfinding cluster + the LabelVariantDetails flake were mid-suite `ServerConfiguration.Load(true)` calls dropping the boot-added MODERNUO_TEST_DATA_DIR data directory → lazily-loaded map sectors/clilocs read empty. Fixed: `TestServerInitializer.ReloadConfiguration()` preserves the dirs; ExpansionConfigurationTests uses it; FloatingCombatTextTests/BaseCreatureSingleClickTests/RarityItemTests hand-rolled cctor boots replaced with the once-guarded `TestServerInitializer.Initialize()` (+ joined the sequential collection where they touch world state). Before: ~50% of runs failed; after: 6/6 green.

Build (shard may lock DLLs): `dotnet build Projects/UOContent/UOContent.csproj -p:OutDir=scratch-verify -p:SolutionDir=E:/age-of-titans-uo/`
Do NOT boot the shard with uncommitted foreign changes. Runtime verify = unit test or user in-game check.

---

## FEATURE 1 — ARMOR SLOT-SETS

### Locked decisions
- **Option A**: roots keep numeric lanes + names + hues. The Epic/Legendary **Signature** is re-keyed root → **(material × slot)**. So a suit's slots never carry the same effect.
- Full **global uniqueness** across all (material×slot) cells.
- Set threshold = **min(4, available slots)** → chainmail=3, rest=4.
- Scope: 6 rooted materials (leather/studded/bone/ringmail/chainmail/plate). **Shields excluded** (single-slot). Wood/Stone/Cloth/Dragon deferred (unrooted today).
- Slot roles: Helm=Ward · Gorget=Composure · Chest=Bulwark · Arms=Riposte · Gloves=Aggression · Legs=Endurance.

### The 30-cell matrix
Params are descriptive — cross-check actual engine param usage before setting S1/S2/S3 (some "1s" values are hardcoded in-engine, leave S=0). Milestone A already verified the 26 non-NEW cells.

| Material | Slot | ClauseType | NEW | Params |
|---|---|---|---|---|
| Leather | Helm | SpellDrBoostFirstHit | | P1=10 |
| Leather | Gorget | RerollFirstResist | | |
| Leather | Chest | FirstHitNoSecondaryEffect | | |
| Leather | Arms | ReflectBoostFirstHit | | P1=10 |
| Leather | Gloves | DodgeGrantsCounterWindow | | P1=3 |
| Leather | Legs | DodgeRegenBurst | | P1=20,P2=5 |
| Studded | Helm* | SpellDrVsPoisonDot | | |
| Studded | Gorget | ParaResistStunsAttacker | | |
| Studded | Chest | ShrugFirstHitPoisonAttacker | **NEW** | shrug+Lesser |
| Studded | Arms | ShrugReflect | | P1=10 |
| Studded | Gloves | OnKillDodgeDoubleDuration | | P1=5 |
| Studded | Legs | StamRegenMirrorsHp | | |
| Bone | Helm | SpellDrBurstOnCritTaken | | P1=3 |
| Bone | Chest | ShrugFirstHitDrainStam | **NEW** | shrug+drain5 |
| Bone | Arms | ReflectCritStun | | |
| Bone | Gloves | OnKillRestoreMissingHpPct | | P1=50 |
| Bone | Legs | OnKillRestoreHpPct | | P1=10 |
| Ringmail | Chest | ShrugFirstHitDrBurst | **NEW** | P1=8,P2=3 |
| Ringmail | Arms | ShrugStunAttacker | | |
| Ringmail | Gloves | OnKillStamRestoreExtendImmunity | | P1=5 |
| Ringmail | Legs | StamRegenMirrorsManaHalf | | |
| Chainmail | Helm | ParaResistBoostsSpellDr | | P1=15,P2=5 |
| Chainmail | Chest | EmergencyRegenTick | | P1=10 |
| Chainmail | Legs | HpRegenBurstOnCritTaken | | P1=5 ⚠cap |
| Plate | Helm | ParaResistBoostsResistSkill | | P1=5,P2=5 |
| Plate | Gorget | FirstParaAutoFails | | |
| Plate | Chest | ShrugFirstHitGuaranteed | | |
| Plate | Arms | ShrugReflectStun | **NEW** | P1=10 |
| Plate | Gloves | HealBlockOnFirstHitLanded | | P1=3 |
| Plate | Legs | OnKillRestoreExtraHp | | P1=15 |

*Studded Helm = optional (only StuddedMempo exists).

### 4 new clauses (all cheap — reuse shrug-rider dispatch in `AbsorbForDefenderArmor`, Defense.cs ~:190/:209)
1. `ShrugFirstHitPoisonAttacker` — shrug 1st hit + poison attacker (reuse ApplyPoison :361).
2. `ShrugFirstHitDrainStam` — shrug 1st hit + drain attacker stam (reuse :127).
3. `ShrugFirstHitDrBurst` — shrug 1st hit + DR window (reuse ArmClauseBurst :151/:40).
4. `ShrugReflectStun` — shrugged blow reflect + stun (reuse ShrugReflect :220 + TryStun :213).
Each = enum member + `ClauseText.Describe` arm + one switch case. 0 need new engine logic.

### 6 capstones (threshold min(4,slots); fire at existing hooks; no serialization; reuse BuffIcon + label)
| Material | Name | Type | Effect | Hook |
|---|---|---|---|---|
| Leather | Evasion | self | +6% dodge | WeaponHit.cs:73 |
| Studded | Venom | debuff | on-hit poison tick (→ "add a stack" once poison feature lands) | Defense.cs:361 |
| Bone | Grave-Chill | debuff | on-hit heal-block 2s | WeaponHit.cs:612 |
| Plate | Siege-Shock | debuff | on-hit 1s stagger | Defense.cs:213 |
| Chainmail | Ward-Surge | self | +12% DR burst on crit-taken (⚠ clamp to cap) | WeaponHit.cs:593 |
| Ringmail | Phalanx-Thorns | self | +8% reflect burst on hit-taken | Defense.cs:278 |

Set detection: add a per-material piece counter in `WornEffectState.Rebuild` item-walk (`~:137-161`), flag in `WornAggregate` when count >= min(4,slots). Template = "Frenzy" self-buff in `CombatFxState.cs` (dict + Arm/Get/expiry-on-read + AddCustomBuff + Evict).

### 2 cap fixes (pre-existing breaches to fix)
1. Chain Legs `HpRegenBurstOnCritTaken`: engine TRIPLES regen (Worn.cs:69). 25%→75% > 60% cap → base ≤20% or burst ×2.
2. Chain capstone +12% DR burst is AT the 12% cap and stacks with base DR → clamp total DR ≤12 or set burst ~8%.

### Verify-at-code checks
- Shrug-riders (ShrugReflect/ShrugStunAttacker/ReflectCritStun) need the suit to supply ShrugPct/ReflectPct vehicle. Leather uses self-sufficient ReflectBoostFirstHit — OK.
- `SpellDrVsPoisonDot` (Studded Helm): verify it actually reduces poison DoT, else no-op.
- Legendary de-collision: per-material agents proposed swaps (see git history / re-derive); finalize against this matrix so no legendary unique clause equals a slot signature.

### Phase status & how to reprompt
- **P1 plumbing — DONE** (Milestone A).
- **P2 data (26 cells) — DONE**; 4 NEW-clause cells pending P3.
- **P3 new clauses — DONE** (2026-07-11, see CURRENT STATE).
- **P4 capstones — DONE** (2026-07-11, see CURRENT STATE).
- **P5 dedupe safety-net — DONE** (2026-07-11, see CURRENT STATE).
- **P6 cap fixes — DONE** (2026-07-11, see CURRENT STATE; + weapon-path BlockGrantsDrBurst per user call).
- **P7 tests — DONE** (2026-07-11, 876/876). In-game verify checklist pending (see CURRENT STATE). ARMOR feature code-complete; **next: POISON PP1** (Server engine stack model).

---

## FEATURE 2 — STACKABLE POISON (global)

### Locked model
- Up to **5 stacks** per mobile, each with its **own expiry**.
- **One merged tick** = SUM of active stacks' per-tick damage, shown as one number; shrinks as stacks expire.
- ApplyPoison **appends** a stack; at 5 → **ignore new**.
- **Cure = clear ALL** stacks.
- **Area sources refresh, don't stack** (PoisonField/gas/traps) — needs a non-stacking apply flag. **All direct hits stack.**
- Per-tick damage = keep T2A values initially; **tune after playtest**.
- Display: buff-bar label `"-{tick} poison x{n}"` (BuffHelper label is a free string). Floating text already tags poison ticks.
- **Not serialized** — no save-migration work.

### Engine touch points (Projects/Server — explicit user request to edit engine)
`Mobile.cs`: `m_Poison` (:334) → stack collection (cap 5); `Poison` setter (:2158); `ApplyPoison` (:8634) append/ignore/area-refresh; `CheckHigherPoison` (:8594) → "canAppend (count<cap)"; `CurePoison` (:8694) clear-all; `PoisonTimer` prop (:2155). `Poison.cs` (:9) base. Overrides: `PlayerMobile.ApplyPoison` (:4036), `BaseCreature.ApplyPoison` (:1542) use `PoisonImpl.IncreaseLevel`; resist hook `RarityEffects.TryResistPoisonApplication`.
UOContent: `Misc/Poison.cs` `PoisonImpl`/`PoisonTimer` (:12/:47) → aggregate tick; area sources `PoisonField`, `GasTrap`, `FactionGasTrap`, `PuzzleChest`, food.

### Balance watch
- 🔴 5× stacked ticks = burst ceiling (5 Lethal = 5× dmg). Flat-sum by decision; add diminishing only if playtest demands.
- 🔴 Poisoning skill + poisoned weapons now stack every swing — big melee-poison PvP shift.
- 🔴 Area-refresh rule is the runaway guard — must be correct.

### Phase status & how to reprompt
- **PP1–PP5 — DONE** (2026-07-11, see CURRENT STATE).
- **PP6 — stack-model tests DONE; remaining: in-game verify (merged tick number, "-X poison xN" label, area-refresh behavior) + balance pass after playtest.**

---

## EXECUTION GUIDANCE
- **Order:** finish ARMOR (P3→P7) before POISON, unless you redirect. They intersect only at studded "Venom" (ships as interim on-hit poison, becomes "add a stack" after PP done).
- **Per phase:** delegate to `deep-executor`/`executor` with the phase brief + this file; build-verify with the command above; **do not commit**; I review the diff before the next phase.
- **Conventions:** follow `modernuo-code-audit` (braces everywhere, `_camelCase`/`PascalCase`, no LINQ on hot paths, switch expressions where clean, no `Console.WriteLine`, single-threaded — no locks).
