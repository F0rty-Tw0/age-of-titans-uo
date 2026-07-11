# UO Outlands — Design Research

> Compiled 2026-07-09 from https://wiki.uooutlands.com (every skill page + cross-cutting systems).
> Purpose: inspiration reference for our T2A shard. Outlands proves "T2A era" is a *ruleset choice*, not a content ceiling — everything below is server-side code on a T2A combat base.
> Confidence: formulas quoted from wiki via summarizing fetch — numbers reliable, spot-check before copying exactly. Open flags listed at the bottom.

---

## 1. The Big Answer: How Outlands Stays "T2A"

- **Era = rules, not content.** T2A gives them: no AOS item properties, old combat math, Fel-only rules. Nothing about T2A blocks new skills, systems, procs, or effects — those are all server code.
- **Chivalry/Necromancy exist** — but share only *names* with AOS versions. Fully custom ability systems (see §4).
- **Skill slots recycled, not added**: Remove Trap merged into Detect Hidden; Bowcraft/Fletching merged into Carpentry; Forensic Evaluation reinvented as the skinning skill. Dead classic skills become new mechanics without touching the client skill list.
- **Client art** covers new monsters/maps (custom patch + ClassicUO fork) — the only part that needs client-side work. Everything in this doc except art is doable on stock ModernUO + T2A config.

---

## 2. Cross-Cutting Design Patterns (the reusable DNA)

These patterns repeat across nearly every skill — this is the real design system:

### 2.1 The universal training ladder
Every skill: **0-50 buy from NPC → 50-80 New Player Dungeon prop → 80-120 real activity.** Standardized rungs; the New Player Dungeon is a deliberate mid-tier training hub for *everything* (combat dummies, locksmith boxes, campfire spots, pickpocket dips).

### 2.2 Every skill feeds combat
Almost every skill grants supplemental damage, usually the same formula:
**PvM: `Base × 25% × (Skill/100)` — PvP: `Base × 10% × (Skill/100)`.**
Seen on: Blacksmithing, Carpentry, Mining (macing), Lumberjacking (axes), Tracking, Forensic Eval, Cartography, Camping, weapon skills as secondaries. Result: no 120-point investment is combat-dead; "crafter" and "fighter" aren't separate power tracks.

### 2.3 PvM/PvP dual formulas everywhere
Every bonus has two coefficients — PvP always weaker (often exactly 40% of PvM, or hard-capped, or **disabled entirely with a flat compensation bonus** as in Chivalry/Necromancy/taming abilities/cooking buffs). PvE power growth never inflates PvP. This is their single most consistent rule.

### 2.4 The Symbol ability framework (Chivalry/Necromancy)
One chassis, two skins:
| | Chivalry | Necromancy |
|---|---|---|
| Resource | Holy Symbols | Unholy Symbols |
| Regen / cap | 1 per 5s / skill÷10 | same |
| Cost / cooldown | 1-5 symbols / 30s per ability | same |
| Capped by paired skill | Tactics | Magery |
| Trains by | melee hits w/ book carried | spell casts w/ book carried |
| Book cost | 5k gold or 25k fame | same |
| PvP | abilities OFF, +10% melee dmg | abilities OFF, +10% spell dmg |
Abilities are instant, uninterruptible, no empty hands, auto-renew toggle. 10 abilities each, unlocked at 50/55/60...95 skill.

### 2.5 The Codex template (weapon-skill meta-progression)
Every weapon skill (+ Parrying, Healing, Alchemy, Fishing, casters via Arcane Codex, thieves via Thieves Codex) gets a **Codex**: requires 80 skill + 80 Tactics (or thematically-paired gate), earns XP from kills, unlocks **5 stances** (toggleable combat modes), **finishers**, and **weapon abilities** via rank points. Same scaffold reskinned per skill audience.

### 2.6 The blessed-item point-buy template
Bard Codex, Taming Bestiary ("Affinity"), Arcane Codex share one chassis:
**blessed Inscription-crafted item → 80+ dual-skill gate → damage/gold-value-scaled XP → ~20 capped upgrade points → tiered trees → free reset every 5 minutes → XP persists even if item lost.**
A reusable template for adding meta-progression to any skill family.

### 2.7 Themed procs per weapon skill
One shared engine (10% base proc, "Special Attack", duration 8-16s scaled by weapon speed + creature difficulty), different payload per skill:
- Swords → **Lacerate** (stacking, +2% ally proc chance/stack — cooperative!) + **Bleed** DoT
- Macing → **Pierce** (armor shred) + PvP stamina drain
- Fencing → **Cripple** (stacking -6% defense, +6% poison/disease taken)
- Wrestling → **Stagger** + unique **Combo Meter** (3 consecutive hits = +50% damage)
- Archery → **Hinder** (full CC lockdown 4-8s)
Flavor identity through payload, not through separate systems.

### 2.8 Anti-abuse as a design layer
Wherever a mechanic could be spammed or griefed, there's a purpose-built counter:
- **Poison Tolerance**: repeated poisons give victim stacking resistance (cap 50%, 5 min) — DoT diminishing returns.
- **Stealing Suspicion**: each notice = cumulative +10% notice chance *in that town* for 12 hours — persistent heat meter.
- **Stealth engagement penalty**: backstabbing someone else's aggroed target = 50% damage — anti-kill-steal tax.
- **Provocation tether**: beyond EffSkill/2 tiles, provoked creatures deal 1 damage — no long-range CC exploits.
- **Gathering teleport lock**: 60s harvest delay after recall/gate + must move 5 steps — anti-macro without killing convenience.
- **PvM steal caps**: 1,000 gold/creature, creature "barren" after 3 steals.
- **Taming PvP caps**: per-slot damage caps, reduced ability rates, 80% move speed vs players — a whole separate balance layer.

### 2.9 QoL as a reward, not a given
Grind stays, friction dies: Smart Harvest (auto-harvest all nearby nodes/corpses), `[AutoStealth`, auto-renew abilities, continuous lockpicking, Hunt Mode arrows, auto-scroll-usage toggle, satchels reducing reagent/ammo consumption, `[VetSupplies` quick-use, bulk potion crafting (25 at once), bulk container ID at 120. Notably: **AFK land fishing is officially allowed** (reduced yield), while endgame ship fishing is Captcha-gated — casual and dedicated paths split deliberately.

### 2.10 Continuous curves instead of binary gates
- Meditation: armor penalty is per-piece percentage (location% × material%) instead of classic's "any metal = no regen".
- Lockpicking: progress bar (each attempt adds %) instead of binary RNG fail-forever.
- Tactics: sub-100 skill is a damage *penalty* — smooth curve that punishes dabbling.

### 2.11 Account-wide vs character-bound split
Broad meta-progression is **account-shared** (Achievements points, Societies reward points); deep combat power is **character-bound** (Aspects, Codex XP). Alt-friendly breadth, main-defining depth.

### 2.12 Skill co-dependence by design
Paired gates everywhere: Taming↔Animal Lore (equal skill required), Chivalry↔Tactics cap, Necro↔Magery cap, Musicianship as hard cap over all 3 bard skills, Snooping+Stealing joint guild gate, Codexes requiring 2×80. Prevents single-skill cherry-picking; templates cost real points.

---

## 3. Skills — Full Findings

### 3.1 Weapon skills

**Swordsmanship** — proc: Lacerate (max 5 stacks, +2% ally special chance each) + Bleed (ticks/3s for 15s). Codex stances: Aggressive/Defensive/Cleave/Warrior/Flaying; finishers Bleed Out, Execute. One-handed swords poisonable; axes not.

**Mace Fighting** — proc: Pierce (+100% damage, 25 armor-shred stacks). PvP: hits have (Damage/50) chance to drain 5 stamina. Codex: stances Aggressive/Defensive/Cleave/Wild Swing/Sunder; finishers Pulverize, Shatter; abilities Pummel/Stun/Smash. Unpoisonable, slow, high damage.

**Fencing** — proc: Cripple (max 5 stacks, each -6% defense +6% poison/disease taken). Codex finisher **Assassinate = 400% damage**, Flurry +15% speed. All fencing weapons poisonable; fastest weapon class (speeds 58→30).

**Wrestling** — **Combo Meter**: 3 consecutive hits on same target = +50% damage (resets on miss/5s/target change). Proc: Stagger (+150% damage, stacks to 45%). Grants (15% × Skill/100) mana refund on spellcasting + defends while casting — the caster-hybrid weapon skill. PvP damage scales with raw Dex. Codex stances: Dragon/Crab/Spider/Monkey/Crane.

**Archery** — must stand still 0.5s before firing (PvP 1.0s at 25 Dex → 0.5s at 100 Dex). Proc: Hinder — 4-8s full lockdown (no move/cast/melee), +100% damage. Codex: Skirmish (fire while moving), Full Draw (+50%). Quivers cut ammo use. Range 10 (bows) / 8 (xbows).

**Tactics** — universal damage multiplier: 0-100 adds `Base × (-50% + Tactics/200)` (sub-100 = penalty!); 100+ adds `Base × 1% × (Tactics-100)`. Gates every weapon Codex at 80.

**Parrying** — parry chance 50%×(Skill/100), -75% damage on parry. Spell-parry vs creatures (25%×Skill/100) — PvM only. **Taunt** (directed + area): aggro control + AR bonus, 15s CD — parry as active tanking kit. Shield AR scales with skill. 80 Magery + 80 Parry = cast/meditate while holding shield. Codex: Shield Bash/Warding/Testudo/Mirror/Bulwark; finishers Last Stand, Barrier.

### 3.2 Combat support

**Anatomy** — +20%×(Skill/100) weapon damage (exempt from PvP SDi cap), +proc-chance boost, +5% swing speed PvM, -25% bleed/disease taken, bandage-slip immunity chance. Gates bandage poison-cure (60) and resurrection (80).

**Healing** — heal = (Healing/100)×(40-60)×(1+0.2×Anatomy/100). **Progressive poison curing** (each fail banks +100% base chance). Bandage slips: -2% per hit taken. **Stationary bonus**: stand still 5s → 2-tile bandage range. 100 Heal + 80 Anat = guaranteed rez. Healer's Codex: ally damage/resist buffs, faster self-bandage.

**Arms Lore** — 75%×(Skill/100) chance to ignore durability loss. PvM: +10% special-attack chance, +10% ability-meter fill, +5% swing speed (all ×Skill/100). **Disarm** at 80 Arms Lore + 80 weapon skill.

**Magic Resist** — 12.5-37.5% spell damage reduction (PvM+PvP). PvM-only: **Spell Absorption** (25%×Skill/100 chance, -75% damage) and **Spell Siphon** (60-min buff on first spell hit per 5 min: +swing speed, +mana refund, +damage resist). Interrupt avoidance from damage.

**Tracking** — locate + automated **Hunt Mode** (directional arrows, 5 targeting modes). Real combat skill: PvM +25%×(Skill/100) weapon AND spell damage, +10 barding; PvP +10% both. Hidden-target range synergy with Detect Hidden.

### 3.3 Magic skills

**Magery** — classic 8 circles/64 spells. **Charged Spells**: 10% chance +50% damage (PvM both directions). Interrupt rules: circles 4-8 always interrupt; circle-1 spam gets 5s immunity window. AoE without LOS = -50%. Walls/fields meleeable (100/200 HP). Scroll-casting = +20 effective Magery. Wizard's Satchel cuts reagent use.

**Eval Int** — PvM ×(0.75 + 0.75×Skill/100); PvP ×(0.75 + 0.375×Skill/100) — half coefficient. Substitutes for Animal Lore stat-viewing.

**Meditation** — per-piece armor penalty = Location% (7-35%) × Material% (leather 0 → plate 100%). Regen reduction = 2× total penalty. Passive 1 mana/2s → 1/0.5s meditating at cap.

**Inscription** — scroll refund chance (40%×Skill/100 circles 1-6; 80% circles 7-8; 100% at 125 eff). Buff durations +400%×(Skill/100). Auto-scroll-usage toggle. Magic Reflect persistence PvM / one-shot PvP. Martial Manual: melee bonus for book-carriers.

**Spirit Speak** — repurposed as universal summon-scaling skill (decoupled from necro): summons get +150% HP, +25% speed, +50% damage/wrestling, +armor/resist, dispel resistance. **Corpse Harvesting** auto-extends summon duration (base 2 min + 8×Skill/100, max 30 via corpses). Summoner's Tome at 80 SS + 80 Magery. Summons drop to printed-skill stats when controller PvP-flagged.

**Alchemy** — potion power scales with skill (GHeal 50%×Skill/100 PvM / 25% PvP). Kegs (100 potions, locked 2 min after PvP flag). Bulk-craft 25 at once. **Potion Codex** (80+, PvM-only enhancement tree: armor pen, cooldowns, no-consume...). Sticky explosion potions at 80+. Bridges into Aspect economy (Distillations/Crystals at 120).

### 3.4 The custom ability skills

**Chivalry** — see §2.4. 10 abilities 50→95 skill: Remove Curse (debuff ignore), Dispel Evil (reflect/evasion), Cleanse by Fire, Consecrate Weapon (+40% special damage), Close Wounds, Enemy of One (+20% melee), Noble Sacrifice (heal/rez), Divine Fury (+15% swing), Sacred Journey (travel gate + aura), Holy Light (AoE heal+damage). Trains passively via melee combat with book carried. Effective skill capped by Tactics.

**Necromancy** — mirror of Chivalry. Vengeful Spirit (decaying undead summon), Poison Strike, Evil Omen (+20% spell damage, 25% self-damage risk), Corpse Skin (disease DoT), Vampiric Embrace (corpse-fueled follower heal), Mind Rot, Blood Oath (HP-cost follower buff), Strangle (delay spell damage +30%), Wither (special mana pool), Pain Spike (corpse-targeted AoE). Capped by Magery; trains via spellcasting.

### 3.5 Rogue skills

**Hiding** — +100 effective inside friendly house, +50 within 1 tile of exterior. Reveal lockouts: 5s (player-caused) / 10s (creature).
**Stealth** — steps resource: 5 + 10×(Skill/100) per activation. **Backstab** (80 Stealth + 80 weapon): +800-1600% scaled by weapon speed × (Skill/100), +25% accuracy/poison chance. Anti-KS: 50% penalty vs others' aggroed targets. `[AutoStealth`.
**Snooping** — hidden snooping shows no notice messages. Thieves Guild gate: 80 Snoop + 80 Steal. Book of Grifts logs attempts.
**Stealing** — weight caps scale with skill; town-scoped **Suspicion** heat (+10% notice per incident, 12h); PvM stealing via Grey Hand guild (1k gold cap/creature, barren after 3); +25% vs Disarmed/Hamstrung targets.
**Detect Hidden** — **absorbs Remove Trap entirely**: trap-removal progress system (like lockpicking), chest tiers 1-8, tool material bonuses. Also anti-stealth + Tracking range synergy.
**Poisoning** — 5 tiers (Lesser 5dmg/10s → Lethal 25dmg/5s). Stacking on creatures (max 8 per player); tier-upgrade chances split PvM/PvP; **Poison Tolerance** counter-play (victim stacks +5/10/15% resist, cap 50%); >18 tiles = auto-downgrade; Lethal consumes carried potion.
**Forensic Evaluation** — REINVENTED: now the skinning skill. Yield 50% + 150%×(Skill/100); 9 colored leather tiers; Smart Harvest (auto-carve grey corpses within 2 tiles); +25% PvM damage passive.
**Lockpicking** — progress-based (attempts accumulate to 100%); chance = (EffSkill - ChestMin)×2.5%; 8 difficulty tiers (0-125 skill); tool material tiers (Valorite +8 skill/+20% progress); continuous auto-attempts.

### 3.6 Bard skills

**Musicianship** — hard CAP for all 3 bard skills (gear bonuses included). Three **Barding Songs** (self-target AoE ally buffs, 15 min, 50-tile): +5%×(EffBarding/100) each — damage resist (Disco) / healing received (Peace) / damage bonus (Provo). Defensive Barding (PvP): effective Wrestling/Resist = (D+P+P)/2, cap 100.
**Provocation** — provoked creatures +500% damage vs creatures, +50% more vs provoked. **Tether** = Skill/2 tiles (beyond: 1 damage). Allies +10% accuracy vs provoked targets.
**Peacemaking** — duration (60 - difficulty)×(EffBarding/100); Area version (8 tiles, banned in guard zones); allies +15% accuracy vs pacified.
**Discordance** — -25% damage dealt / +25% taken; **+15% more if stacked with Provoke/Pacify** (bard trinity bonus); Area version; +25 effective skill vs already-barded targets.
**Begging** — land: gold (capped 5/beg, 50/vendor/day). Sea: **Crew Motivation** — captain buffs all crew +25%×(Skill/100) damage. Legacy skill attached to the new naval system.
**Herding** — activated Shepherd's Crook buffs ALL followers passively: PvM +22%×(EffSkill/100) damage, +11% resist. Crook material tiers scale it. Trains passively while pets fight.

### 3.7 Taming skills

**Animal Taming** — success (Skill-Min)×8%. **Passive Taming Gains**: killing with an appropriately-difficult pet triggers skill checks (gold-value-scaled) — Taming ×2, Lore ×4, Herding ×10 rates. **Pet leveling 1-15** (levels 1-10 give double per-level bonuses of 10-15). Ability classes Attack/Utility/Tank with Innate/Passive/Cooldown abilities. **Underdog bonus** (using pets below your tier: up to +40% damage/+20% resist). **Zoology Guild** auto-buffs server-wide underused creature types. Massive PvP cap layer (per-slot damage caps, reduced ability rates, 80% speed vs players).
**Animal Lore** — mandatory equal-skill pair with Taming. Gates Vet thresholds (60 = pet poison cure, 80 = pet rez). Shows classes/abilities.
**Veterinary** — **Veterinary Supplies** (craftable consumable: AoE-heals all followers within 2 tiles, 5s CD, 50% effectiveness tradeoff). Followers get permanent +10%×(EffVet/100) damage. Stationary 2-tile bandage bonus. Wrong-skill-for-target = 50% penalty.

### 3.8 Gathering/crafting skills

**Mining / Lumberjacking** — same template: Smart Harvest auto-loop; 10 colored tiers (81.75% → 0.25% discovery, skill-gated 0-110); colored tools (+bonus, +durability); 60s post-teleport lock + 5-step stationary penalty; resource-map radius +100%×(Skill/100); **feed weapon damage** (Mining→maces, Lumberjacking→axes, 25% PvM / 10% PvP).
**Blacksmithing** — colored ingot gear tiers (to +55 durability/+45% AR/+9 tactics damage at Avarite); Repair Kits (95+, batch-restore in containers/other players' packs); +25%×(Skill/100) armor bonus wearing crafted armor; 25/10 damage split.
**Tailoring** — **damage reflection**: adjacent creature attacks reflect 150%×(Skill/100) back; armor bonus 50%×(Skill/100); crafts Wrestling weapons; Aspect Cloth at 120; recolor kits at 100.
**Carpentry** — **absorbs Bowcraft/Fletching** (bows/ammo crafted here). Ship stats randomized ±10%, skill reduces bad rolls; instruments scale barding +50%×(Skill/100); 25/10 damage split.
**Tinkering** — the one pure-utility crafting skill (zero combat bonus, deliberate). Cross-skill gates: needs Item ID 100 / Carpentry 100 for some recipes. Mastercrafting Diagrams gate advanced items.
**Cartography** — **Explorer Packs**: 5s channel → 60-min Exploration Effect (persists through death): +25% damage vs treasure-map creatures + ship bonuses; hike without campfire. Deciphering minigame gump.
**Item Identification** — amplifies bonuses of already-IDed gear (weapon damage ×100%×Skill/100); 105+ shows vendor prices, 110 essence values, 120 bulk-ID containers.
**Taste Identification** — poison amplifier (+100%×Skill/100 poison damage, -50% target resist/cure); shares Food Satisfaction system with Cooking; Herbal Poultice: 50%×(Skill/100) chance to ignore any DoT tick.
**Cooking** — **Food Satisfaction** 6 tiers (Measly→Delectable), eat every 60s to climb, 60-min effect: PvM mana-regen proc (2.5-25% base + skill scaling) + swing speed (+0.5-5% + scaling). Killed by PvP flag (2-min cutoff). Snacking = instant HP/mana/stam restore scaled by skill.
**Fishing** — AFK land fishing allowed (reduced yield); ship fishing Captcha-gated (100-120 training); SOS/Fishing Spots spawn creature encounters; aquarium collection (3 rarities × 2 water types); World Records leaderboard per species at docks; Fishing Codex with 600% finisher.
**Camping** — permanent +200 stones carry / +50 item slots per 100 skill (always on); **Hiking** fast-travel via World Atlas at 60+; bedroll instant logout; +25% base weapon damage vs creatures.

---

## 4. Cross-Cutting Systems

### Aspect Mastery (endgame gear progression)
23+ Aspects (Air, Fire, Death, Water, Lightning, Frost, Earth, Arcane, Holy, Shadow, Poison, Blood, Void, Temporal, Eldritch, War, Command, Discipline, Madness, Lyric, Harvest, Fortune, Gadget, Artisan, Chromatic, Agorawave) applied to Weapons/Spellbooks/Armor. Character-bound. Loop: unlock (Distillations + Cores + Kit) → activate (5 Arcane Essence per use) → earn Aspect XP from kills/pilfering/lockpicking while geared → **15 tiers** (500 → 250,000 XP), tier-ups consume escalating materials + Aspect Triumph at T15. Weapons get accuracy/tactics/special-chance + unique per-aspect effects; armor bonuses partially deactivate on PvP flag. Can run below permanent tier to save Essence.
**Takeaway**: gear-power ladder fed by broad activity, decoupled from skill caps — endgame progression after skills are done.

### Societies (weekly account-wide jobs)
6 factions (Adventurer Lodge, Artificer Enclave, Monster Hunter Society, Order of Armorers, Seafarer League, Tradesman Union). 10 jobs/week per **account**, any mix; jobs = craft-consumption, kill quotas, taming tasks (auto-adjusted weekly). Points account-wide → mount tokens, Mastery Chain Links, reforging tools.
**Takeaway**: weekly loop cross-pollinating crafting/combat/taming into one shared pool; alt-friendly.

### Codex items (the shared meta-progression template)
Arcane Codex (casters: 80 Wrestling + 80 Magery; stances Leech/Shield/Scatter/Fracture/Surge; finishers Clarity 300%/Catalyst), Bard Codex (2× barding skills 80+; XP from barded-creature kills; 8 upgrades/3 tiers, e.g. Revolution Song +40-200% provoked damage), Taming Bestiary (80 Taming + 80 Lore; XP scaled by control-slot usage; Attack/Utility/Tank pools, 5 cross-class points).
Shared chassis: **blessed Inscription item → dual 80-skill gate → activity-scaled XP → ~20 point cap → tiered trees → 5-minute free respec → XP survives item loss.**

### Achievements (account meta-currency)
14 categories spanning every system. Account-wide points; rewards 50–1,200 pts: skill Mastery Scrolls, skill-cap Orbs, mounts (Horse 350 → Direwolf 1,200), storage Tomes, Mastery Chain Links, cosmetics.
**Takeaway**: the umbrella layer — aggregates all systems into one spendable pool; Mastery Chain Links recur as glue currency across Achievements AND Societies.

---

## 5. What This Means For Our Shard

1. 🟢 **Everything above is server-side.** T2A config blocks none of it. New art (monsters/maps) is the only client-side lift.
2. **Steal the templates, not the content**: the training ladder, the 25/10 PvM/PvP split, the Symbol framework, the Codex chassis, the blessed-item point-buy — each is a parameterized system we can implement once and skin many times.
3. **Their balance philosophy in one line**: PvE power can grow forever (procs, tiers, aspects) because every formula has a PvP twin that's capped, halved, or replaced with a flat bonus.
4. **Fits our direction**: rarity system + Greek itemization are already "era-neutral systems on T2A base" — same move Outlands makes everywhere.
5. Skill slots 49 (Necromancy) and 51 (Chivalry) already exist unconditionally in our `Projects/Server/Skills.cs`; Chivalry/Necro spell registration is gated by one `if (Core.AOS)` in `Projects/UOContent/Spells/Initializer.cs` — but per §2.4, an Outlands-style build means a new ability system, not unlocking the AOS spells.

---

## 6. Open Flags (unverified details)

- Eval Int: "10% cap on supplemental skills combined (Tracking/Camping/Inscription)" — possible content bleed from another wiki section.
- Spirit Speak: two dispel-resistance figures (flat 20% vs 50%×Printed/100) — may be two mechanics or a fetch artifact.
- Herding: PvP resist formula printed without `/100` divisor on wiki — likely typo.
- Remove Trap page claims Harvest Aspect armor boosts trap removal; Detect Hidden page doesn't corroborate.
- All numbers extracted via summarizing fetch — spot-check the live page before copying any formula verbatim into balance code.
- Canonical wiki page names: `Aspect_Mastery` (not `Aspect`), `Taming_Bestiary` (not `Affinity_System`).
