# UO Outlands — Creature Systems Research

> Compiled 2026-07-10 from https://wiki.uooutlands.com. Companion to `outlands-research.md` (skills/systems).
> Focus: the *systems* behind their bestiary — data model, scaling curves, families, variants, bosses, reward economy. Individual stat blocks sampled only to reverse-engineer the curves.
> Confidence: formulas via summarizing fetch — reliable for design, spot-check before copying numbers into balance code. Open flags at the bottom.

---

## 1. The Core Insight: One Number Runs Everything

Every creature has a **Difficulty Value** — computed by a hidden formula from stats plus a hand-picked **Unique Scalar** (covers special mechanics stats can't capture). Anyone can Animal Lore any creature to see it. Observed range: 0.2 (Cuckoo) → 29,244 (The One From Beyond The Stars).

**The two-layer indirection:**

```
stats + Unique Scalar ──(hidden formula)──▶ Difficulty ──(×10)──▶ Gold Value ──▶ most rewards
```

- **Gold Value = Difficulty × 10** — confirmed *exactly* across two families (Aegis Knight 60.9→610, Lich 67.9→679, Blood Cult Zealot 46→461, Prelate 128.1→1,281). Bosses are hand-boosted above formula (Aegis High Priest: predicted 14,380, actual 100,000; Ancient Wyrm ≈ ×24 curve).
- **Gold Value drives**: gold dropped, rare/special loot chance, **XP** (`Damage% × GoldValue` — one formula for Aspect XP, Mastery Chain XP, Codex XP), passive taming gain chance, Black Goods steal-rarity, Pit Trial scoring.
- **Difficulty consumed directly** (not via gold) by: barding duration `(60 − Difficulty) × (EffBarding/100)`, steal-retaliation `−2%×√Difficulty`, Spirit Speak summon-duration harvesting `×(50×√Difficulty)`.

**Design lesson**: one tuning knob per creature. Buff a monster's stats → Difficulty recalculates → gold, XP, loot, steal-rarity, barding all auto-adjust. Paragon upscaling and Challenger Dungeon upscaling both reuse this exact recalc path — no separate reward tables anywhere.

---

## 2. The Creature Data Model

Each creature has up to 3 **independent** stat blocks:

1. **Wild** — Location, Slayer Group, Difficulty, Gold, Hits, Melee Damage range, Wrestling, Parry, Armor, Magic Resist, Attack Speed, Magery, Spell Damage, Poison, AI type (`Melee` / `MeleeMage`).
2. **Tameable** (if tameable) — Ability Class (Attack/Utility/Tank), Min Taming, Control Slots, its own Hits/Damage/Armor/Wrestling/Resists, **Underdog Scalar**, Follower Abilities.
3. **Summonable** (if spell-conjurable) — stats scale by caster Spirit Speak breakpoints (base/80/100/120/150); or **Leveling Progression** (pet levels 1-10 / 10-15).

Deliberately absent: Str/Dex/Int, Fame/Karma, full skill lists, loot tables. Difficulty-first model, not an RPG stat sheet.

### Scaling curves (from 13 sampled creatures)

| Tier | Example | Hits | Melee Dmg | Magic Resist | Difficulty / Gold |
|---|---|---|---|---|---|
| Fodder | Aegis Rat | 1,500 | 10-20 | 25 | 1.7 / 16.9 |
| Trivial | Adder | 4,000 | 30-40 | 25 | 54.4 / 545 |
| Mid | Bloodwolf | 5,000 | 40-50 | 25 | 81.8 / 819 |
| Themed | Blood Elemental | 8,000 | 40-50 | 150 (caster) | 118.2 / 1,182 |
| High tame | Dragon / Earth Dragon | 10,000 | 50-60 | 25 | 127-144 / ~1,300-1,400 |
| Classic elite | Balron | 45,000 | 65-75 | 300 | 888 / 8,881 |
| Endgame | Ancient Wyrm | 106,463 | 120-150 | 600 | 10,646 / 250,000 |
| Raid | Abyssal Daemon | 1,500,000 | 55-65 | 150 | 8,215 / 200,000 |

- **Hits scale exponentially; melee damage scales gently.** Difficulty is an attrition/DPS-race lever, not a one-shot lever. A boss doesn't hit much harder than a knight — it has 33× the HP.
- **Magic Resist is the cleanest tier signal**: world mobs flat 25 → caster/endgame 150 → bosses 300-600. Assigned by ROLE (casters high, melee low), not rank.
- **Armor tracks tank identity**, not tier (glass-cannon casters keep 25 at high Difficulty).
- Dragon vs Earth Dragon: identical wild stats, different Difficulty/Gold/MinTame — the formula weights tamed potential, not just visible wild stats.

---

## 3. Family & Dungeon Ecology

### Themed families (how one theme = 20+ creatures cheaply)
- Aegis Keep roster: 76 creatures — "Aegis X" family (16) + "Blood X" block (~30) + fillers.
- **No shared family ability.** Cohesion = naming + location + the Difficulty/Gold curve. Abilities are per-creature and theme-appropriate (Aegis Rat: Mirror + Disease).
- Rank escalation = scale the HP curve + pick a role-appropriate defensive spike. Higher rank can mean LOWER melee damage but big Magic Resist (Blood Cult Prelate vs Zealot) — "harder" = different role, not bigger numbers everywhere.

### Element recolor variants (Dragon line, 3/3 sampled)
- **Wild stat block 100% identical** across Dragon / Air Dragon / Blood Dragon. Only Gold/Difficulty drift slightly for dungeon context.
- Formula: **shared skeleton + reflavored signature attack + ONE bonus ability.** Fire Breath (damage) → Air Breath (damage+weaken) + Air Shield → Massive Blood Breath (3-target cone+bleed) + Bleed passive.
- **One variant per dungeon** (Nusero/Cavernam/Aegis Keep) — each dungeon gets a "signature" flavor of a shared archetype without new combat math.

### Dungeon structure
- 12 core dungeons + Time Dungeon (endgame) + 2 subterranean + 4 sanctuary (low-risk/low-reward, incl. New Player Dungeon).
- **Weekly Friday rotation**: one random core dungeon becomes **Sanctuary** (PvP-restricted, gold halved, mining off, skinning halved) and one becomes **Challenger** (spawns upscaled "as if Lesser Paragon" → Difficulty recalc → rewards auto-scale; bosses exempt; stale spawns force-despawned at reset).
- Boss rooms have a **Custodian** creature patrolling while the boss is despawned — reveals hidden players with fire fields, blocks re-hiding 10s.

---

## 4. Variant Systems

### Paragon (the cheap elite overlay)
- ~1 in 400 loot-dropping spawns; tag on the existing creature, not a new type. Tiers among paragons: Lesser 60% / Regular 30% / Greater 10%, shown above head.
- Same abilities, scaled stats/skills. Difficulty recalculates → rewards auto-scale (same path as Challenger Dungeon).
- Always drops a **sealed Paragon Chest** — 1 stone, empty until lockpicked, then fills with loot. Kills "peek and skip"; makes Lockpicking+Detect Hidden mandatory in the loot chain.
- Stealing from a Paragon always yields Black Goods.

### Boss-Type Creatures (the participation-scaled encounter)
Categories: Shrine / Mini / Boss / Omni / Lore / Event. Opposite philosophy to Paragon: not a rarity roll — a raid that grows with attendance.

| Type | Base HP | +HP per player | Player cap | Hinder resist |
|---|---|---|---|---|
| Lore-Boss | 75,000 | +40,000 | 10 | 70% |
| Mini-Boss | 100,000 | +40,000 | 20 | 75% |
| Boss | 200,000 | +80,000 | 30 | 80% |
| Omni Boss | 1,500,000 | +500,000 | 40 | 90% |

- Each unique player dealing 1,000+ damage: +0.5% boss damage, +0.5% loot, +1% minion-spawn chance. **Once per IP** — anti-multibox clamp on the scaling itself.
- Resisted crowd control converts into bonus damage taken by the caster's target... (hinder resist 70-95%); barding gives a fixed "Boss Barding" debuff (-25% attack/cast speed, +33% ability delays) instead of real CC; Discordance scaled down 40-50% and non-stacking across players.
- Loot drops as **Skull Tokens** that become items when moved off the corpse.
- Natural respawn 24-48h; natural spawns can trigger **contested events** — everyone flags grey, field spells banned, orange-flag escalation after 2 min.

### Looting rights (the anti-leech layer)
- Normal creatures: top damager / first-attacker (+15% simulated damage bonus on sub-100k-HP creatures) / their guild+party.
- Bosses: **threshold-gated** — individual 2-10% of total damage (by tier) OR guild/party combined 6-30% grants rights. Corpse shows red (rights) / blue (no rights).
- Party damage-share splits XP evenly but the Boss Results screen still shows true individual damage.
- **Results visibility delayed 5 minutes** post-kill and participant-only — stops third parties from scouting the results window to raid the winners. Post-kill griefing treated as its own abuse category.

### Omni Bosses (the endgame summon chain)
- Summoned via **Tome of Heroism**: boss kills award Tokens of Heroism (one random contributor); feed tokens into the tome; completing ALL mini-boss + boss tokens unlocks the Omni summon (+4 Mastery Chain Links).
- Consumed tokens add loot bonus (+2.5% per mini-boss, +5% per boss, cap 50%) — rewards *prerequisite completion*, not just fight damage.
- Fight happens in a **Lawless Omni Realm**: all grey, factions ignored, randomized lair with rotating escape moongates.
- Drops 1 Mastery Chain Link per 500k damage, min 3, at least 1 Gold-tier.

### Long-tail collectible layers (parallel, independent)
- **Black Goods** — thief currency from Pilfering/Mugging any creature; rarity scales with Difficulty; spent at Black Market. Gives thieves a Difficulty-scaled reason to visit hard content.
- **Antiquities** — ultra-rare collectibles across all systems; each player registers a type once ever; each instance registered by ONE player globally with server-wide announcement — "first to find" prestige loop.
- **Dungeon Rares** — 4 catalog tiers per dungeon (boss uniques, mini-boss equipment sets, decor, collectible cards).
- **Dungeon Slayers** — 1% of slayer items affect ALL creatures in one dungeon instead of one type; takes the dungeon's hue.
- "Corrupted" as a variant system: **does not exist** (ruled out — it's one unique creature's name).

---

## 5. Taming-Side Creature Design

### Tamed stats are a separate hand-tuned dataset — the key trick
Wild Dragon: 10,000 HP. Tamed Dragon: **450 HP** (~4.5%). Tamed HP always lands in the low hundreds regardless of wild scale. Pet power lives on its own fixed budget — **buffing a dungeon monster never accidentally buffs the tamer meta.**

- **Underdog Scalar**: per-creature float, inversely tracks tier (1.4 on Brown Bear, 1.03 on Earth Dragon) — the comeback lever keeping weak pets viable.
- **Ability Class**: Attack / Utility / Tank — coarse role tag driving the Bestiary upgrade pools.
- **Follower Abilities**: typed **Passive** (proc, e.g. Bleed 15% on hit) or **Cooldown** (activated, e.g. Charge: teleport 12 tiles hit 3 targets; Fire Breath: DamageMax×1.5 projectile). Documented plain-English per creature. ("Innate" type mentioned on taming pages but never observed on creature pages — unconfirmed.)
- Two scaling systems by acquisition: tamed pets level (1-10 strong growth, 10-15 halved growth); summons scale by caster's Spirit Speak breakpoints.

### Legacy Trait Groups — REMOVED system, kept for reference
Outlands *used to* have 22 archetype groups (Breath, Feral, Serpent, Mage, ...) where pets picked 1-of-2 traits at levels 2/4/6/8/10 from a shared ~35-trait vocabulary (Predator +10% dmg, Survival +10% DR, Venom +20% vs poisoned, Breach ignores 50 MR, Thorns reflect, Mule pack capacity...). Level 2 was always Predator-vs-Survival; level 10 was Vicious + a group signature.
**They abandoned it** for flat per-level stat growth + Follower Abilities. No rationale documented. Full legacy tables preserved at `wiki/Legacy_Trait_Groups` if we ever want the trait vocabulary — it's a ready-made, internally-consistent buff lexicon.

---

## 6. Reward Economy Layers (stack order)

1. **Base**: Difficulty → Gold Value → gold/XP/loot rolls.
2. **Progressive Loot**: creature gains +5% loot per hour alive, cap +250% (50h). Anti-camping — rewards hunting aged spawns over farming respawn points. (Ocean/NPD excluded.)
3. **Weekly Region Bonuses** (bank board): +15% gold, +25% respawn, +20% XP, +15% crafting exceptional, +10% vendor rebate — rotating pools; Ocean and Wilderness roll separately.
4. **Weekly dungeon rotation**: Challenger (up) / Sanctuary (down).
5. **Player buffs**: Fortune Aspect, Guild Favors, Mastery Chain Links — multiplicative, apply to all attackers.
6. **Orthogonal taxonomies**: Slayer groups (7+: Beastial, Daemonic, Humanoid, Monstrous, Construct, Elemental, Nature — pure targeting axis, +15/30/45% tiers); skinning yield (keys off player skill, not creature Difficulty).

---

## 7. What This Means For Our Shard

1. **Build the Difficulty hub first.** One derived number per creature (stats + hand-tuned scalar) driving gold/XP/loot beats hand-placed loot tables. Retunes ripple automatically. This slots naturally next to our rarity system's D-values (`dev-docs/itemization/00-framework.md` — D=(10,15.6,27.2,46.2,111)) — same philosophy, one scalar driving reward tiers.
2. **Families are a content multiplier**: one theme + HP curve + role-appropriate resist + per-rank names = a 15-creature dungeon roster from one afternoon of tuning. Recolors: shared skeleton + reflavored signature ability + one bonus — with stock client art (hues) this costs nothing.
3. **Separate wild and tamed stat budgets** if/when we do taming content — Outlands' cleanest balance decision.
4. **Two elite philosophies, use both**: Paragon overlay (rarity roll + stat multiplier + guaranteed chest) for ambient spice; participation-scaled bosses (threshold looting rights, IP-capped scaling) for social endgame.
5. **Anti-abuse is designed in, not patched in**: sealed chests, damage thresholds, per-IP scaling caps, 5-min results delay, contested-fight flagging. Every reward mechanic ships with its abuse counter.
6. 🟢 All server-side. Recolored families, difficulty economy, paragons, boss scaling — zero client work on stock T2A + modern client art.

---

## 8. Open Flags (unverified)

- "Innate" follower-ability type documented on taming pages but never observed on creature pages.
- Paragon tameability: unconfirmed either way. Boss taming/stealing: unconfirmed (likely N/A).
- Blood Cult creature abilities didn't extract from wiki text (unconfirmed absence, likely extraction limitation).
- Exact Difficulty formula is deliberately hidden by Outlands — only inputs (stats, Unique Scalar) and outputs (Difficulty, Gold ×10) are public.
- Aglaisis The Arch-Daemon / Arannis The Researcher Of Time: wiki pages genuinely incomplete (tagged as such) — named-boss mechanics are a wiki gap, not a fetch failure. "Researcher of Time" may exist under multiple renamed variants.
- No wiki mapping of creatures to dungeon floors; no canonical spawn-system doc; dungeon rare drop rates undocumented.
- Hue/color values not text-extractable; recolor assumption is name-implied.
- Wild-block absence on Brown Bear (tame/summon-only?) unresolved.
