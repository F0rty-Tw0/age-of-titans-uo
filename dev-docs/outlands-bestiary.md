# UO Outlands — Full Bestiary (Wild Creature Stats)

> Compiled 2026-07-10. Companion to `outlands-research.md` (skills/systems) and `outlands-creatures.md` (creature-system design patterns — read that first for what these numbers MEAN).
> Source: https://wiki.uooutlands.com/Creatures_List — authoritative backing data in `Module:WildCreatureData` (raw Lua, 1,288 entries, 8 slayer groups).
> Extraction: 8 parallel agents, one per slayer group. Best batches pulled the raw Lua module / raw page markdown verbatim (zero AI summarization); two earlier batches used a summarizing fetch and carry column-drift flags (noted per section).

## Coverage

| Slayer group | Rows | Quality |
|---|---|---|
| Beastial | 228 | verbatim raw markdown, verified contiguous |
| Construct | 106 | summarizer fetch — column positions partly unverified |
| Daemonic | 116 | raw table data, first-10 + last-3 fields verified stable |
| Elemental | 114 | summarizer fetch — rows 61-106 column drift, flagged inline |
| Humanoid | 210 | raw Lua module, grep-verified 210/210 |
| Monstrous | 184 | full-page single-pass fetch, verified contiguous |
| Nature | 167 | raw Lua module, field-verified |
| Undead | 141 | raw Lua module, field-verified |
| **Total** | **1,266** | of 1,288 module entries — 22 unaccounted (likely blank/other slayer values); wiki category lists 1,277 creature pages (some have no stat row: incomplete/event pages) |
| Tameable (appendix) | 238 | raw Lua module `Module:TameableCreatureData`, verified end-to-end — tamed stats, slots, classes, abilities |

## Column legend

Full source column order: `Name | Location | Slayer | Difficulty | Gold | Hits | MeleeDmg | Wrestling | Armor | MagicResist | Parry | AtkSpd | Magery | SpellDmg | Poison | Poisoning | PoisonResist | Stealth | AI | Speed | UniqueScaler`

- Sections differ slightly in projection (some drop Slayer since it's constant, some drop Parry/Stealth) — each section header states its own format.
- **The source omits blank optional cells entirely** (no placeholder), so middle optional fields (Parry → Stealth) vary per row; the raw-Lua sections normalize this with `-`, the verbatim-markdown sections preserve rows as printed. First fields (through MagicResist) and last three (AI | Speed | UniqueScaler) are positionally reliable everywhere.
- `?` = wiki's own placeholder (likely "formula-based, not flat"). `X` = boolean flag (e.g. Stealth-capable). `-` = blank.
- **UniqueScaler** = the hand-picked constant fed into the hidden Difficulty formula (covers special abilities stats don't capture). Higher = creature punches above its raw stats.
- **Gold ≈ Difficulty × 10** for normal mobs; bosses/named are manually boosted above formula (e.g. 50,000/100,000/200,000 flat values).

## Known source-level flaws (wiki's own data, preserved verbatim)

- Spectral Bishop (Undead): melee damage "1205-30" — min/max transposed typo in the wiki's Lua data.
- Eerie Spirit / Reaper Of Souls: melee "-1", zeroed stats — special-mechanic placeholder entries.
- Several named creatures have `?` for Gold (excluded from the Gold=Diff×10 audit).

## How to use this file

For our shard this is a **tuning reference**, not content to copy: percentile curves per tier (see `outlands-creatures.md` §2), dungeon roster compositions (filter by Location), role templates (AI × MagicResist × Speed combos), and the UniqueScaler values showing which archetypes Outlands considers "stats undersell it" (stealthers, CC-users, high-speed mobs cluster at 1.4-2.0+).

---
# Raw: Beastial section (agent: list-beastial) — 228 rows, COMPLETE (verbatim raw markdown, zero AI reformatting)

Header order per row (bracketed only when present; blanks omitted): Name | Location | Slayer | Difficulty | Gold | Hits | MeleeDmg | Wrestling | Armor | MagicResist | [Parry] | [AtkSpd] | [Magery] | [SpellDmg] | [Poison] | [Poisoning] | [PoisonResist] | [Stealth] | AI | Speed | UniqueScaler
First=Hellhound, last=Jaguar, one unbroken block. Fixed-slot mapping NOT safe for middle optional fields (verified counter-example: Spitting Viper).

| Hellhound | Inferno | Beastial | 93.8 | 939 | 6,000 | 40 - 50 | 105 | 25 | 25 | ? | Melee | Fast | 1.25 |
| Air Drake | Cavernam | Beastial | 92.7 | 928 | 6,000 | 40 - 50 | 110 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Bird Of Paradise | Wilderness | Beastial | 194.7 | 1,947 | 10,000 | 20 - 30 | 110 | 25 | 175 | ? | 175 | 35 - 45 | Mage | Fast | 1.25 |
| Giant Chameleon | Kraul Hive | Beastial | 105.9 | 1,060 | 6,000 | 40 - 50 | 100 | 25 | 25 | ? | X | Melee | Medium | 1.5 |
| Stinger | Mount Petram | Beastial | 18.1 | 181 | 1,500 | 15 - 25 | 85 | 25 | 25 | ? | Greater | 50 | 40% | Melee | VeryFast | 1 |
| Diseased Primordial | ? | Beastial | 12.4 | 125 | 1,000 | 15 - 25 | 80 | 25 | 25 | ? | Melee | VeryFast | 1.1 |
| Skulker | Aegis Keep | Beastial | 49.3 | 494 | 3,000 | 20 - 30 | 90 | 50 | 25 | ? | X | Melee | Fast | 1.5 |
| Gargantua Spider | Kraul Hive | Beastial | 276.6 | 2,767 | 20,000 | 60 - 80 | 130 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1.3 |
| Tidal Sea Serpent | Tidal Tomb | Beastial | 259.2 | 2,593 | 20,000 | 70 - 80 | 130 | 25 | 25 | ? | Melee | Medium | 1.25 |
| Eagle | Wilderness | Beastial | 0.6 | 7 | 75 | 4 - 8 | 50 | 25 | 25 | ? | Melee | VeryFast | 1 |
| Stone Asp | ? | Beastial | 29.7 | 297 | 2,500 | 15 - 25 | 100 | 75 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1 |
| Antlion | Darkmire Temple | Beastial | 80.4 | 804 | 6,000 | 40 - 50 | 90 | 100 | 25 | ? | Melee | Slow | 1.2 |
| Acarid | Cavernam | Beastial | 31.3 | 314 | 2,500 | 15 - 25 | 90 | 25 | 25 | ? | Melee | Medium | 1.25 |
| Chicken | Wilderness | Beastial | 0.4 | 4 | 50 | 2 - 4 | 30 | 25 | 25 | ? | Melee | Medium | 1 |
| Silver Serpent | Time Dungeon | Beastial | 119.3 | ? | 1,300 | 120 - 170 | 120 | 50 | 200 | 70 | Lethal | 100 | 100% | Melee | SuperFast | 1 |
| Flame Purger | Inferno | Beastial | 65.2 | 653 | 5,000 | 30 - 40 | 80 | 50 | 25 | ? | Ranged | Slow | 1.3 |
| Boreal Faerie Wyrm | Cavernam | Beastial | 81 | 811 | 6,000 | 30 - 40 | 110 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Acid Slug | Time Dungeon | Beastial | 387.8 | 3,879 | 22,000 | 50 - 60 | 110 | 100 | 50 | 50 | Greater | 33 | 99% | Melee | VeryFast | 1.6 |
| Drake Whelp | Nusero | Beastial | 18.2 | 183 | 1,500 | 15 - 25 | 70 | 25 | 25 | ? | Melee | Medium | 1.2 |
| Warpig | Wilderness | Beastial | 106.4 | 1,064 | 8,000 | 30 - 40 | 110 | 25 | 25 | ? | Melee | Fast | 1.35 |
| Greater Dragon | Time Dungeon | Beastial | 1330 | 13,302 | 60,000 | 75 - 100 | 130 | 150 | 300 | 50 | 250 | 50 - 60 | 75% | MeleeMage | VeryFast | 1.7 |
| Bloodskipper | Aegis Keep | Beastial | 183.6 | 1,836 | 12,000 | 50 - 60 | 110 | 25 | 25 | ? | Melee | VeryFast | 1.4 |
| Cat | Wilderness | Beastial | 0.4 | 4 | 50 | 3 - 6 | 30 | 25 | 25 | ? | Melee | Medium | 1 |
| Blood Dragon | Aegis Keep | Beastial | 143.8 | 1,438 | 10,000 | 50 - 60 | 110 | 50 | 25 | ? | Melee | Medium | 1.3 |
| Ember Drake | Nusero | Beastial | 89.3 | 893 | 6,000 | 40 - 50 | 100 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Cryptwing | Ossuary | Beastial | 60.1 | 602 | 4,000 | 20 - 30 | 110 | 25 | 25 | ? | Melee | VeryFast | 1.35 |
| Fire Beetle | Inferno | Beastial | 51.6 | 517 | 4,000 | 30 - 40 | 80 | 75 | 25 | ? | Melee | Medium | 1.14 |
| Embear | Inferno | Beastial | 47.2 | 472 | 3,500 | 25 - 35 | 75 | 25 | 25 | ? | Melee | Fast | 1.25 |
| Blood Courser | Aegis Keep | Beastial | 65 | 650 | 4,000 | 30 - 40 | 90 | 25 | 100 | ? | 75 | 15 - 25 | Mage | VeryFast | 1.14 |
| Colossal Trapdoor Spider | Mount Petram | Beastial | 175.1 | 1,752 | 12,000 | 50 - 60 | 120 | 25 | 25 | ? | Greater | 50 | 40% | X | Melee | Slow | 1.4 |
| Bullvore | Inferno | Beastial | 111.6 | 1,117 | 8,000 | 50 - 60 | 95 | 50 | 25 | ? | Melee | Fast | 1.14 |
| Trapdoor Spider | Mount Petram | Beastial | 21.9 | 220 | 1,500 | 15 - 25 | 65 | 25 | 25 | ? | Greater | 50 | 40% | X | Melee | Slow | 1.4 |
| Jungle Stalker | Wilderness | Beastial | 1047 | 100,000 | 250,000 | 25 - 35 | 110 | 50 | 100 | ? | Melee | Fast | 1.6 |
| Anaconda | Darkmire Temple | Beastial | 63.6 | 636 | 5,000 | 30 - 40 | 100 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Fast | 1 |
| Air Dragon | Cavernam | Beastial | 138.3 | 1,383 | 10,000 | 50 - 60 | 110 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Spitting Viper | Kraul Hive | Beastial | 40.8 | 409 | 2,500 | 20 - 30 | 80 | 25 | 25 | ? | 50 | 60% | Ranged | Fast | 1.5 |
| Rhinocerus Beetle | Cavernam | Beastial | 201.1 | 2,011 | 14,000 | 70 - 80 | 90 | 100 | 25 | ? | Melee | Medium | 1.35 |
| Tidal Krait | Tidal Tomb | Beastial | 203.1 | 2,032 | 14,000 | 50 - 60 | 130 | 50 | 25 | ? | Deadly | 50 | 60% | Melee | Fast | 1.25 |
| Norse Cattle | Wilderness | Beastial | 32.5 | ? | 3,000 | 20 - 30 | 90 | 25 | 25 | ? | Melee | Fast | 1 |
| Colossal Frog | New Player Dungeon | Beastial | 3.7 | 37 | 400 | 10 - 20 | 35 | 25 | 25 | ? | Melee | Slow | 1 |
| Azure Wyrm | Nusero | Beastial | 126 | 1,260 | 8,000 | 50 - 60 | 115 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Phoenix | Ossuary | Beastial | 194.4 | 1,944 | 8,000 | 30 - 40 | 90 | 25 | 200 | ? | 175 | 35 - 45 | Mage | Fast | 1.4 |
| Navreys Broodling | Time Dungeon | Beastial | 175.5 | 1,756 | 12,000 | 30 - 40 | 100 | 50 | 50 | 40 | Deadly | 25 | 70% | X | Melee | SuperFast | 1.4 |
| Ochu | Time Dungeon | Beastial | 443.2 | 4,432 | 24,000 | 60 - 70 | 110 | 50 | 150 | 50 | Deadly | 50 | 60% | Melee | VeryFast | 1.5 |
| Watery Leaper | Ocean | Beastial | 60.9 | 610 | 4,000 | 30 - 40 | 100 | 25 | 25 | ? | Melee | VeryFast | 1.2 |
| Savage Primordial | Kraul Hive | Beastial | 111.8 | 1,119 | 6,000 | 40 - 50 | 120 | 25 | 25 | ? | Melee | VeryFast | 1.35 |
| Harvestman | Nusero | Beastial | 102.7 | 1,027 | 6,000 | 40 - 50 | 120 | 25 | 25 | ? | Melee | Medium | 1.35 |
| Colossal Poison Dart Frog | Darkmire Temple | Beastial | 57.7 | 577 | 4,000 | 30 - 40 | 100 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Slow | 1.1 |
| Giant Blood Purger | Aegis Keep | Beastial | 267.3 | 2,673 | 18,000 | 80 - 90 | 130 | 50 | 25 | ? | Ranged | Slow | 1.3 |
| Fire Ant | Ossuary | Beastial | 18.7 | 187 | 1,500 | 15 - 25 | 70 | 25 | 25 | ? | Melee | Fast | 1.2 |
| Primordial | Nusero | Beastial | 24.3 | 244 | 2,000 | 15 - 25 | 80 | 25 | 25 | ? | Melee | VeryFast | 1.14 |
| Sunscale | Wilderness | Beastial | 207 | 2,070 | 12,000 | 60 - 70 | 130 | 50 | 100 | ? | Melee | Medium | 1.4 |
| Colossal Huntsman | Wilderness | Beastial | 210.9 | 2,109 | 12,000 | 60 - 70 | 130 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1.35 |
| Chameleon | Nusero | Beastial | 32.1 | 322 | 2,500 | 15 - 25 | 70 | 25 | 25 | ? | X | Melee | Medium | 1.35 |
| Shade Wolf | The Mausoleum | Beastial | 86.7 | 868 | 5,000 | 40 - 50 | 95 | 25 | 25 | ? | X | Melee | Fast | 1.35 |
| Arctic Bullvore | Cavernam | Beastial | 138.8 | 1,389 | 10,000 | 50 - 60 | 100 | 50 | 25 | ? | Melee | Fast | 1.25 |
| Colossal Dung Beetle | Kraul Hive | Beastial | 249.1 | 2,492 | 18,000 | 70 - 80 | 110 | 75 | 25 | ? | Melee | Medium | 1.35 |
| Shadow Beast | Time Dungeon | Beastial | 371.6 | 3,716 | 14,000 | 50 - 60 | 120 | 50 | 75 | 60 | X | Melee | SuperFast | 1.65 |
| Asp | Nusero | Beastial | 29.1 | 292 | 2,500 | 15 - 25 | 100 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1 |
| Reef Serpent | Pulma | Beastial | 64 | 641 | 5,000 | 30 - 40 | 90 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Fast | 1 |
| Great Sunken Serpent | Pulma | Beastial | 1374 | 100,000 | 200,000 | 50 - 60 | 100 | 50 | 25 | ? | X | Melee | Medium | 2 |
| Tidal Mantis | Pulma | Beastial | 89 | 890 | 6,000 | 40 - 50 | 110 | 50 | 25 | ? | Melee | Fast | 1.14 |
| Giant Spitting Viper | Kraul Hive | Beastial | 73.7 | 738 | 4,000 | 30 - 40 | 100 | 25 | 25 | ? | 50 | 60% | Ranged | Fast | 1.5 |
| Dog | Wilderness | Beastial | 0.4 | 4 | 50 | 3 - 6 | 30 | 25 | 25 | ? | Melee | Medium | 1 |
| Death Adder | ? | Beastial | 69.4 | ? | 5,000 | 30 - 40 | 110 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Fast | 1 |
| Pit Dragon | Nusero | Beastial | 595.7 | 50,000 | 100,000 | 30 - 40 | 100 | 50 | 25 | ? | Melee | VeryFast | 1.8 |
| Cave Bear | Mount Petram | Beastial | 29.4 | 294 | 2,500 | 20 - 30 | 75 | 25 | 25 | ? | Melee | Fast | 1.1 |
| Swamp Spider | Darkmire Temple | Beastial | 17.1 | 172 | 1,500 | 10 - 20 | 80 | 25 | 25 | ? | Greater | 50 | 40% | Melee | Medium | 1.1 |
| Sabeartooth | Cavernam | Beastial | 38.8 | 389 | 3,000 | 20 - 30 | 75 | 25 | 25 | ? | Melee | Fast | 1.25 |
| Stone Adder | Nusero | Beastial | 62.3 | 624 | 5,000 | 30 - 40 | 100 | 75 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1 |
| Smoke Faerie Dragon | Nusero | Beastial | 84.5 | 846 | 5,000 | 35 - 45 | 100 | 50 | 25 | 25 | ? | X | Melee | Medium | 1.4 |
| Diseased Ankheg | ? | Beastial | 22.4 | 224 | 1,500 | 25 - 35 | 80 | 75 | 25 | ? | 50 | 40% | Melee | Slow | 1.25 |
| Rime Guar | Cavernam | Beastial | 19.1 | 192 | 1,500 | 20 - 30 | 70 | 75 | 25 | ? | Melee | Medium | 1.14 |
| Sun Wyrm | Nusero | Beastial | 151.2 | 1,512 | 8,000 | 50 - 60 | 115 | 50 | 25 | ? | Melee | Medium | 1.5 |
| Black Widow | Darkmire Temple | Beastial | 54.6 | 546 | 3,000 | 30 - 40 | 110 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Medium | 1.2 |
| Silverback | Darkmire Temple | Beastial | 23.8 | 238 | 2,000 | 15 - 25 | 80 | 25 | 25 | ? | Melee | Fast | 1.14 |
| Pig | Wilderness | Beastial | 0.6 | 6 | 75 | 3 - 6 | 30 | 25 | 25 | ? | Melee | Medium | 1 |
| Darkscale | The Mausoleum | Beastial | 317.7 | 3,178 | 24,000 | 70 - 80 | 130 | 50 | 100 | 25 | ? | Melee | Medium | 1.35 |
| Rustled Cattle | Wilderness | Beastial | 33.5 | ? | 3,000 | 20 - 30 | 100 | 25 | 25 | ? | Melee | Fast | 1 |
| Fire Crawler | Inferno | Beastial | 80 | 801 | 6,000 | 40 - 50 | 100 | 100 | 25 | ? | Melee | Slow | 1.14 |
| Wyvern Hatchling | Nusero | Beastial | 37.2 | 372 | 3,000 | 20 - 30 | 90 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Medium | 1 |
| Green Bloodworm | Time Dungeon | Beastial | 438 | 4,380 | 21,000 | 60 - 70 | 110 | 50 | 100 | 60 | Lethal | 25 | 60% | Melee | VeryFast | 1.5 |
| Silver Stag | Time Dungeon | Beastial | 33.2 | ? | 600 | 90 - 130 | 110 | 25 | 125 | 60 | 200 | 40 - 50 | 100% | MeleeMage | SuperFast | 1 |
| Dirge Spider | Undermountain | Beastial | 22.6 | ? | 2,000 | 15 - 25 | 80 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1 |
| Eldritch Drake | Nusero | Beastial | 126.3 | 1,263 | 6,000 | 40 - 50 | 100 | 50 | 150 | ? | 125 | 25 - 35 | Mage | Medium | 1.3 |
| Colossal Strider | Wilderness | Beastial | 258.1 | 2,581 | 16,000 | 80 - 100 | 110 | 50 | 25 | ? | Melee | Fast | 1.25 |
| Colossal Frost Scorpion | Cavernam | Beastial | 74 | 741 | 5,000 | 40 - 50 | 90 | 75 | 25 | ? | Deadly | 50 | 60% | Melee | Slow | 1.14 |
| Aegis Rat | Aegis Keep | Beastial | 16.9 | 169 | 1,500 | 10 - 20 | 70 | 25 | 25 | ? | Melee | Medium | 1.2 |
| Jungle Mantis | Kraul Hive | Beastial | 71.4 | 714 | 4,000 | 30 - 40 | 110 | 50 | 25 | ? | Deadly | 50 | 40% | Melee | Fast | 1.25 |
| Searing Mantis | Inferno | Beastial | 37.3 | 373 | 3,000 | 15 - 25 | 85 | 50 | 25 | ? | Melee | Fast | 1.25 |
| Colossal Spitting Viper | Kraul Hive | Beastial | 123.9 | 1,240 | 6,000 | 40 - 50 | 130 | 25 | 25 | ? | 50 | 60% | Ranged | Fast | 1.5 |
| Barbed Prowler | Cavernam | Beastial | 138.4 | 1,384 | 10,000 | 40 - 50 | 110 | 50 | 25 | ? | Melee | Fast | 1.35 |
| Manticore | The Mausoleum | Beastial | 253.3 | 2,533 | 18,000 | 60 - 70 | 130 | 50 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1.3 |
| Solbeast | Wilderness | Beastial | 277.3 | 2,774 | 20,000 | 70 - 80 | 130 | 50 | 150 | ? | Melee | Medium | 1.3 |
| Colossal Black Widow | Darkmire Temple | Beastial | 198.2 | 1,983 | 14,000 | 50 - 60 | 140 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Medium | 1.2 |
| Monitor Hatchling | Nusero | Beastial | 21.5 | 215 | 2,000 | 15 - 25 | 65 | 50 | 25 | ? | Melee | Medium | 1.1 |
| Colossal Searing Scorpion | Ossuary | Beastial | 74 | 741 | 5,000 | 40 - 50 | 90 | 75 | 25 | ? | Deadly | 50 | 60% | Melee | Slow | 1.14 |
| Adder | Nusero | Beastial | 54.4 | 545 | 4,000 | 30 - 40 | 100 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Fast | 1 |
| Corpse Eater | The Mausoleum | Beastial | 17.8 | 179 | 1,500 | 10 - 20 | 65 | 50 | 25 | ? | Melee | Fast | 1.25 |
| Giant Frog | New Player Dungeon | Beastial | 1.6 | 17 | 200 | 5 - 10 | 30 | 25 | 25 | ? | Melee | Slow | 1 |
| Rock Guar | Mount Petram | Beastial | 15.7 | 157 | 1,500 | 15 - 25 | 75 | 75 | 25 | ? | Melee | Medium | 1 |
| Firebat | Inferno | Beastial | 17.4 | 175 | 1,500 | 10 - 20 | 85 | 25 | 25 | ? | Melee | Medium | 1.2 |
| Sea Serpent | Ocean | Beastial | 75.3 | 753 | 4,000 | 40 - 50 | 90 | 50 | 25 | ? | Melee | Slow | 1.5 |
| Blood Ape | Aegis Keep | Beastial | 33.9 | 340 | 2,500 | 20 - 30 | 80 | 25 | 25 | ? | Melee | Fast | 1.25 |
| Arctic Arachnid | Winterlands | Beastial | 117.2 | 1,173 | 10,000 | 50 - 60 | 110 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Medium | 0.95 |
| Silver Wolf | Time Dungeon | Beastial | 36 | ? | 600 | 90 - 100 | 130 | 50 | 50 | 90 | Melee | VeryFast | 1 |
| Searing Lizard | Inferno | Beastial | 30.5 | 306 | 2,500 | 20 - 30 | 65 | 50 | 25 | ? | Melee | Medium | 1.2 |
| Blood Purger | Aegis Keep | Beastial | 201 | 2,010 | 16,000 | 60 - 70 | 120 | 50 | 25 | ? | Ranged | Slow | 1.3 |
| Radiant Burrowbug | Ossuary | Beastial | 114.8 | 1,149 | 8,000 | 50 - 60 | 90 | 100 | 25 | ? | Melee | Slow | 1.3 |
| Terathan Larva | Mount Petram | Beastial | 14.1 | 142 | 1,500 | 10 - 20 | 65 | 25 | 25 | ? | Melee | Fast | 1 |
| Sinewseeker | Aegis Keep | Beastial | 283.9 | 2,839 | 14,000 | 55 - 65 | 130 | 50 | 25 | ? | X | Melee | Fast | 1.8 |
| Wolfhound | Aegis Keep | Beastial | 32.6 | 326 | 2,500 | 20 - 30 | 80 | 25 | 25 | ? | Melee | Fast | 1.2 |
| Dread Spider | Time Dungeon | Beastial | 318 | 3,180 | 13,000 | 50 - 60 | 100 | 75 | 175 | 40 | 175 | 35 - 45 | Deadly | 50 | 95% | MeleeMage | SuperFast | 1.5 |
| Lurking Turkey | ? | Beastial | 106 | 1,060 | 4,000 | 30 - 40 | 120 | 25 | 100 | ? | X | Melee | Fast | 2 |
| Deep Crawler | Pulma | Beastial | 57.1 | 571 | 5,000 | 30 - 40 | 85 | 100 | 25 | ? | Melee | Slow | 1.1 |
| Bonehorn | Wilderness | Beastial | 114.4 | 1,145 | 8,000 | 40 - 50 | 110 | 50 | 25 | ? | Melee | Fast | 1.25 |
| Giant Termite | Ocean | Beastial | 23.1 | 231 | 2,000 | 25 - 35 | 80 | 75 | 25 | ? | Melee | Slow | 1 |
| Bloodwolf | Aegis Keep | Beastial | 81.8 | 819 | 5,000 | 40 - 50 | 90 | 25 | 25 | ? | Melee | Fast | 1.3 |
| Eldritch Dragon | Nusero | Beastial | 205.7 | 2,058 | 10,000 | 50 - 60 | 110 | 50 | 200 | ? | 175 | 35 - 45 | Mage | Medium | 1.3 |
| Swamp Dragon | Darkmire Temple | Beastial | 150.2 | 1,502 | 10,000 | 50 - 60 | 110 | 50 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1.25 |
| Devilbat | Mount Petram | Beastial | 20.8 | 208 | 1,500 | 15 - 25 | 85 | 25 | 25 | ? | Melee | VeryFast | 1.25 |
| Void Serpent | Time Dungeon | Beastial | 554.2 | 5,542 | 25,000 | 50 - 60 | 100 | 75 | 150 | ? | 275 | 55 - 65 | Deadly | 50 | 75% | Mage | SuperFast | 1.6 |
| Aegis Asp | Aegis Keep | Beastial | 24.5 | 245 | 2,000 | 15 - 25 | 90 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1.05 |
| Plague Rat | ? | Beastial | 2.4 | 25 | 250 | 5 - 10 | 30 | 25 | 25 | ? | 40% | Melee | Medium | 1.14 |
| Hunting Cat | ? | Beastial | 34.8 | ? | 2,500 | 15 - 25 | 100 | 25 | 25 | 45 | Melee | VeryFast | 1.14 |
| Hind | ? | Beastial | 0.8 | 9 | 100 | 5 - 10 | 40 | 25 | 25 | ? | Melee | Fast | 1 |
| Giant Rat | ? | Beastial | 2 | 20 | 200 | 5 - 10 | 20 | 25 | 25 | ? | Melee | Medium | 1.2 |
| Giant Plague Rat | ? | Beastial | 5.5 | 55 | 500 | 10 - 20 | 40 | 25 | 25 | ? | 40% | Melee | Medium | 1.14 |
| Death Asp | ? | Beastial | 27.7 | ? | 2,500 | 10 - 20 | 90 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Medium | 1 |
| Drake | Nusero | Beastial | 82.1 | 822 | 6,000 | 40 - 50 | 100 | 50 | 25 | ? | Melee | Medium | 1.14 |
| Hoary Wyrm | Winterlands | Beastial | 189.9 | 1,900 | 16,000 | 70 - 80 | 130 | 75 | 25 | ? | 60% | Melee | Medium | 1 |
| Wolf | Wilderness | Beastial | 4.4 | 45 | 400 | 10 - 20 | 40 | 25 | 25 | ? | Melee | Fast | 1.14 |
| Walrus | Wilderness | Beastial | 1.8 | 19 | 200 | 7 - 14 | 60 | 25 | 25 | ? | Melee | Medium | 1 |
| Dusk Drake | Mount Petram | Beastial | 92.8 | 929 | 6,000 | 40 - 50 | 100 | 50 | 25 | ? | Melee | Medium | 1.3 |
| Sneaking Spiderling | Time Dungeon | Beastial | 363.5 | 3,635 | 14,000 | 50 - 60 | 110 | 25 | 125 | 70 | Greater | 50 | 50% | X | Melee | SuperFast | 1.5 |
| Shadow Prowler | Kraul Hive | Beastial | 219.6 | 2,196 | 14,000 | 60 - 70 | 140 | 50 | 25 | 25 | ? | Melee | Fast | 1.25 |
| Phase Spider | Wilderness | Beastial | 1022 | 100,000 | 250,000 | 25 - 35 | 105 | 25 | 25 | ? | Deadly | 20 | 60% | X | Melee | Medium | 1.6 |
| Meat Cur | Time Dungeon | Beastial | 412.3 | 4,124 | 18,000 | 50 - 70 | 110 | 100 | 50 | 60 | 25% | Melee | SuperFast | 1.55 |
| Water Dragon | Pulma | Beastial | 132.7 | 1,328 | 10,000 | 50 - 60 | 110 | 50 | 25 | ? | Melee | Medium | 1.2 |
| Spitting Spider | Wilderness | Beastial | 40.9 | 409 | 2,500 | 20 - 30 | 90 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Fast | 1.3 |
| Emperor Dragon | Nusero | Beastial | 1276 | 100,000 | 200,000 | 40 - 50 | 100 | 50 | 25 | ? | Melee | Fast | 2 |
| Sheep | Wilderness | Beastial | 0.4 | 4 | 50 | 3 - 6 | 30 | 25 | 25 | ? | Melee | Medium | 1 |
| Sandstalker | Wilderness | Beastial | 103.3 | 1,034 | 6,000 | 30 - 40 | 120 | 25 | 25 | ? | X | Melee | Fast | 1.5 |
| Sand Crab | Wilderness | Beastial | 2.1 | 22 | 200 | 7 - 14 | 55 | 75 | 25 | ? | Melee | Medium | 1.14 |
| Boreal Wyrm | Cavernam | Beastial | 212 | 2,121 | 16,000 | 60 - 70 | 130 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Aegis Scorpion | Aegis Keep | Beastial | 31.4 | 314 | 2,500 | 20 - 30 | 90 | 75 | 25 | ? | Deadly | 50 | 60% | Melee | Slow | 1.05 |
| Phase Spider Mirror | Wilderness | Beastial | 171.4 | 1,714 | 30,000 | 30 - 40 | 120 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1 |
| Navreys Spiderling | Time Dungeon | Beastial | 133.1 | 1,331 | 12,000 | 30 - 40 | 100 | 25 | 25 | ? | Deadly | 25 | 60% | Melee | VeryFast | 1.3 |
| Flamehound | Inferno | Beastial | 24.1 | 242 | 2,000 | 15 - 25 | 70 | 25 | 25 | ? | Melee | Fast | 1.2 |
| Fortress Beetle | Pulma | Beastial | 128.5 | 1,286 | 8,000 | 60 - 70 | 100 | 100 | 25 | ? | Melee | Medium | 1.2 |
| Greater Sea Serpent | Time Dungeon | Beastial | 501.1 | 5,012 | 28,000 | 40 - 50 | 100 | 100 | 200 | ? | 225 | 45 - 55 | Greater | 33 | 40% | Mage | Fast | 1.55 |
| Slime Purger | Ocean | Beastial | 74 | 741 | 5,000 | 35 - 45 | 90 | 25 | 25 | ? | 60% | Ranged | Slow | 1.35 |
| Colossal Termite | Ocean | Beastial | 59.1 | 592 | 5,000 | 40 - 50 | 90 | 75 | 25 | ? | Melee | Slow | 1 |
| Husk Crab | Pulma | Beastial | 53.5 | 536 | 4,000 | 30 - 40 | 90 | 75 | 25 | ? | Melee | Medium | 1.14 |
| Termite | Ocean | Beastial | 10.5 | 105 | 1,000 | 15 - 25 | 75 | 50 | 25 | ? | Melee | Slow | 1 |
| Magma Serpent | Wilderness | Beastial | 161.7 | 1,618 | 12,000 | 60 - 70 | 110 | 50 | 25 | ? | X | Melee | Medium | 1.2 |
| Giant Bat | New Player Dungeon | Beastial | 1.8 | 18 | 200 | 5 - 10 | 45 | 25 | 25 | ? | Melee | Fast | 1.05 |
| Corpse Purger | The Mausoleum | Beastial | 101.2 | 1,012 | 7,000 | 40 - 50 | 90 | 50 | 25 | ? | Ranged | Slow | 1.4 |
| Ruby Wyrmling | Nusero | Beastial | 90.7 | 908 | 5,000 | 40 - 50 | 105 | 50 | 25 | ? | Melee | Medium | 1.4 |
| Goat | Wilderness | Beastial | 0.6 | 6 | 75 | 4 - 8 | 35 | 25 | 25 | ? | Melee | Medium | 1 |
| Fiery Leaper | Ossuary | Beastial | 93.3 | 933 | 6,000 | 30 - 40 | 110 | 25 | 25 | ? | Melee | VeryFast | 1.35 |
| Giant Strider | Wilderness | Beastial | 169.2 | 1,692 | 12,000 | 60 - 70 | 100 | 50 | 25 | ? | Melee | Fast | 1.25 |
| Giant Spider | Wilderness | Beastial | 29 | 290 | 2,500 | 15 - 25 | 80 | 25 | 25 | ? | Greater | 50 | 60% | Melee | Medium | 1.1 |
| Great Hart | Wilderness | Beastial | 1.1 | 11 | 125 | 6 - 12 | 40 | 25 | 25 | ? | Melee | Fast | 1 |
| Kraul Hydra | Kraul Hive | Beastial | 1561 | 100,000 | 200,000 | 50 - 60 | 120 | 50 | 25 | ? | Melee | Fast | 2 |
| Cuckoo | Wilderness | Beastial | 0.2 | 2 | 25 | 2 - 4 | 25 | 25 | 25 | ? | Melee | Fast | 1 |
| Cow | Wilderness | Beastial | 1.3 | 13 | 150 | 6 - 12 | 50 | 25 | 25 | ? | Melee | Medium | 1 |
| Searing Bullvore | Inferno | Beastial | 138.8 | 1,389 | 10,000 | 50 - 60 | 100 | 50 | 25 | ? | Melee | Fast | 1.25 |
| Brood Spider | Wilderness | Beastial | 43.6 | ? | 4,000 | 15 - 25 | 110 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1 |
| Goretusk | Aegis Keep | Beastial | 329.9 | 3,299 | 24,000 | 75 - 85 | 130 | 50 | 25 | ? | Melee | Fast | 1.3 |
| Ankheg | Mount Petram | Beastial | 107.2 | 1,073 | 8,000 | 40 - 50 | 100 | 100 | 25 | ? | Greater | 40% | Melee | Slow | 1.3 |
| Bull | Wilderness | Beastial | 2.3 | 23 | 250 | 8 - 16 | 40 | 25 | 25 | ? | Melee | Fast | 1 |
| Navrey Night-Eyes | Time Dungeon | Beastial | 1809 | 18,094 | 125,000 | 50 - 60 | 110 | 75 | 250 | 50 | 250 | 50 - 60 | Lethal | 75 | 90% | MeleeMage | SuperFast | 1.5 |
| Ruby Wyrm | Nusero | Beastial | 141.1 | 1,411 | 8,000 | 50 - 60 | 115 | 50 | 25 | ? | Melee | Medium | 1.4 |
| White Wyrm | Cavernam | Beastial | 136 | 1,361 | 8,000 | 50 - 60 | 115 | 50 | 25 | ? | Melee | Medium | 1.35 |
| Silver Bear | Time Dungeon | Beastial | 67.4 | ? | 2,000 | 70 - 100 | 120 | 125 | 75 | 75 | 40 | 50% | Melee | Fast | 1 |
| Rotworm | Time Dungeon | Beastial | 344.8 | 3,449 | 17,000 | 50 - 60 | 120 | 75 | 100 | 60 | 25% | Melee | VeryFast | 1.5 |
| Dragon Whelp | Nusero | Beastial | 23.9 | 240 | 2,000 | 15 - 25 | 75 | 25 | 50 | ? | Melee | Medium | 1.2 |
| Colossal Boa | Wilderness | Beastial | 174.7 | 1,748 | 12,000 | 50 - 60 | 130 | 50 | 25 | ? | Greater | 50 | 40% | Melee | Fast | 1.2 |
| Sand Crawler | Ossuary | Beastial | 57.1 | 571 | 5,000 | 30 - 40 | 85 | 100 | 25 | ? | Melee | Slow | 1.1 |
| Navreys Spawn | Time Dungeon | Beastial | 226.2 | 2,262 | 15,000 | 40 - 50 | 120 | 50 | 50 | 40 | Lethal | 33 | 80% | Melee | SuperFast | 1.2 |
| Kith | Time Dungeon | Beastial | 287.5 | 2,876 | 12,000 | 70 - 80 | 100 | 50 | 100 | ? | Deadly | 25 | 66% | Melee | SuperFast | 1.6 |
| Scorpion | Ossuary | Beastial | 15.1 | 151 | 1,500 | 10 - 20 | 70 | 75 | 25 | ? | Greater | 50 | 40% | Melee | Slow | 1 |
| Guar | Wilderness | Beastial | 3.5 | 35 | 400 | 5 - 10 | 40 | 75 | 25 | ? | Melee | Medium | 1 |
| Ancient Wyrm | Time Dungeon | Beastial | 10646 | 106,463 | 250,000 | 120 - 150 | 120 | 200 | 600 | 60 | 800 | 160 - 170 | 80% | MeleeMage | SuperFast | 1.6 |
| Alpha Shadowbeast | Time Dungeon | Beastial | 2478 | 24,784 | 180,000 | 70 - 80 | 150 | 50 | 50 | 60 | X | Melee | SuperFast | 1.5 |
| Winter Wolf | Cavernam | Beastial | 25.5 | 255 | 2,000 | 15 - 25 | 75 | 25 | 25 | ? | Melee | Fast | 1.25 |
| Fellbeast | The Mausoleum | Beastial | 423.1 | 4,232 | 35,000 | 80 - 90 | 140 | 50 | 150 | ? | Melee | Medium | 1.3 |
| Giant Scorpion | Ossuary | Beastial | 25.7 | 257 | 2,500 | 15 - 25 | 75 | 75 | 25 | ? | Greater | 50 | 40% | Melee | Slow | 1 |
| Vampire Bat | The Mausoleum | Beastial | 67.2 | 672 | 4,000 | 30 - 40 | 140 | 25 | 25 | ? | Melee | Fast | 1.2 |
| Diseased Stalker | ? | Beastial | 25.8 | 259 | 1,500 | 30 - 40 | 90 | 25 | 25 | ? | X | Melee | VeryFast | 1.2 |
| Devourer Beetle | The Mausoleum | Beastial | 113.3 | 1,133 | 8,000 | 40 - 50 | 95 | 75 | 25 | ? | Melee | Fast | 1.3 |
| Greater Smoke Dragon | Time Dungeon | Beastial | 1380 | 13,804 | 60,000 | 70 - 80 | 130 | 100 | 175 | 25 | 60 | 175 | 35 - 45 | 50% | X | MeleeMage | SuperFast | 1.9 |
| Carrion Beetle | The Mausoleum | Beastial | 65.2 | 653 | 5,000 | 30 - 40 | 90 | 75 | 25 | ? | Melee | Medium | 1.2 |
| Water Drake | Pulma | Beastial | 89.3 | 893 | 6,000 | 40 - 50 | 100 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Blood Scorpion | Aegis Keep | Beastial | 83.9 | 839 | 6,000 | 30 - 40 | 100 | 75 | 25 | ? | Deadly | 50 | 60% | Melee | Slow | 1.25 |
| Swamp Drake | Darkmire Temple | Beastial | 97.7 | 977 | 6,000 | 40 - 50 | 100 | 50 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1.25 |
| Stone Viper | ? | Beastial | 37.2 | 373 | 3,000 | 20 - 30 | 100 | 75 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1 |
| Great Abyssal Hornbeast | Cavernam | Beastial | 595.7 | 50,000 | 100,000 | 30 - 40 | 100 | 50 | 25 | ? | Melee | VeryFast | 1.8 |
| Fire Salamander | Inferno | Beastial | 29.3 | 294 | 2,500 | 15 - 25 | 80 | 25 | 25 | ? | Melee | Medium | 1.2 |
| Blood Hunter | Aegis Keep | Beastial | 76.4 | 764 | 5,000 | 30 - 40 | 120 | 25 | 25 | ? | Melee | VeryFast | 1.2 |
| Sword Spider | Kraul Hive | Beastial | 112.8 | 1,129 | 6,000 | 40 - 50 | 120 | 25 | 25 | ? | Deadly | 50 | 60% | Melee | Medium | 1.35 |
| Colossal Blazing Beetle | Ossuary | Beastial | 208.5 | 2,086 | 14,000 | 70 - 80 | 90 | 100 | 25 | ? | Melee | Medium | 1.4 |
| Cave Bat | Ossuary | Beastial | 37.2 | 373 | 3,000 | 15 - 25 | 110 | 25 | 25 | ? | Melee | VeryFast | 1.14 |
| Black Cat | Ossuary | Beastial | 56.4 | 564 | 3,000 | 25 - 35 | 110 | 25 | 25 | ? | Melee | Fast | 1.5 |
| Wyvern | Nusero | Beastial | 96.4 | 964 | 8,000 | 40 - 50 | 100 | 50 | 25 | ? | Lethal | 50 | 80% | Melee | Medium | 1 |
| Sun Wyrmling | Nusero | Beastial | 97.2 | 972 | 5,000 | 40 - 50 | 105 | 50 | 25 | ? | Melee | Medium | 1.5 |
| Smoke Drake | Nusero | Beastial | 97.6 | 976 | 6,000 | 40 - 50 | 100 | 50 | 25 | 25 | ? | X | Melee | Medium | 1.35 |
| Smoke Dragon | Nusero | Beastial | 151.2 | 1,512 | 10,000 | 50 - 60 | 110 | 50 | 25 | 25 | ? | X | Melee | Medium | 1.35 |
| Primordial Whelp | Nusero | Beastial | 17 | 170 | 1,500 | 10 - 20 | 75 | 25 | 25 | ? | Melee | VeryFast | 1.14 |
| Monitor | Nusero | Beastial | 28.4 | 285 | 2,500 | 20 - 30 | 70 | 50 | 25 | ? | Melee | Medium | 1.1 |
| Komodo | Nusero | Beastial | 36 | 360 | 3,000 | 25 - 35 | 75 | 50 | 25 | ? | Melee | Medium | 1.1 |
| Blood Serpent | Aegis Keep | Beastial | 82.6 | 827 | 6,000 | 25 - 35 | 95 | 25 | 25 | ? | Lethal | 50 | 60% | Melee | Fast | 1.2 |
| Dragon | Nusero | Beastial | 127.2 | 1,272 | 10,000 | 50 - 60 | 110 | 50 | 25 | ? | Melee | Medium | 1.14 |
| Giant Black Widow | Darkmire Temple | Beastial | 93.2 | 933 | 5,000 | 40 - 50 | 120 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Medium | 1.2 |
| Azure Wyrmling | Nusero | Beastial | 81 | 810 | 5,000 | 40 - 50 | 105 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Earth Drake | Mount Petram | Beastial | 92.8 | 929 | 6,000 | 40 - 50 | 100 | 50 | 25 | ? | Melee | Medium | 1.3 |
| Earth Dragon | Mount Petram | Beastial | 143.8 | 1,438 | 10,000 | 50 - 60 | 110 | 50 | 25 | ? | Melee | Medium | 1.3 |
| Offal Eater | Time Dungeon | Beastial | 368.1 | 3,681 | 18,000 | 60 - 70 | 120 | 50 | 125 | ? | Deadly | 50 | 80% | Melee | SuperFast | 1.6 |
| Dusk Dragon | Mount Petram | Beastial | 143.8 | 1,438 | 10,000 | 50 - 60 | 110 | 50 | 25 | ? | Melee | Medium | 1.3 |
| Nightstalker | The Mausoleum | Beastial | 122 | 1,221 | 8,000 | 50 - 60 | 90 | 25 | 25 | ? | X | Melee | Fast | 1.3 |
| Giant Trapdoor Spider | Mount Petram | Beastial | 46.7 | 468 | 3,000 | 25 - 35 | 75 | 25 | 25 | ? | Greater | 50 | 40% | X | Melee | Slow | 1.4 |
| Aegis Whelp | Aegis Keep | Beastial | 34.6 | 346 | 2,500 | 20 - 30 | 90 | 25 | 25 | ? | Melee | VeryFast | 1.2 |
| Ember Dragon | Nusero | Beastial | 138.3 | 1,383 | 10,000 | 50 - 60 | 110 | 50 | 25 | ? | Melee | Medium | 1.25 |
| Blood Drake | Aegis Keep | Beastial | 89.3 | 893 | 6,000 | 40 - 50 | 90 | 50 | 25 | ? | Melee | Medium | 1.3 |
| Giant Poison Dart Frog | Darkmire Temple | Beastial | 33.9 | 339 | 2,500 | 20 - 30 | 90 | 25 | 25 | ? | Lethal | 50 | 80% | Melee | Slow | 1.1 |
| White Wyrmling | Cavernam | Beastial | 87.5 | 875 | 5,000 | 40 - 50 | 105 | 50 | 25 | ? | Melee | Medium | 1.35 |
| Jaguar | Darkmire Temple | Beastial | 22.3 | 223 | 1,500 | 15 - 25 | 70 | 25 | 25 | ? | X | Melee | VeryFast | 1.4 |
# Raw: Construct section (agent: list-construct) — 106 rows

Format: Name|Location|Difficulty|(sometimes Gold)|Hits|MeleeDmg|Wrestling|Armor|MagicResist|...|AI|Speed|UniqueScaler
CAVEAT: two fetch batches, inconsistent column layouts — batch 2 (Reanimated Knight onward) has Gold inserted after Difficulty; batch 1 mostly lacks it. Several rows missing AtkSpd/Magery pairs. Field POSITIONS unverified vs live table; values themselves reliable. Confirmed complete: next row after "Icy Sedimental" is Daemonic section.

## Batch 1 (no Gold column in most rows)
Spare Parts|The Mausoleum|28.9|2,500|15 - 25|60|25|25|?|Medium|1.25|-|-|Melee|Medium|1.25
Lemura|Cavernam|100.6|6,000|40 - 50|110|50|25|?|Fast|1.3|-|-|Melee|Fast|1.3
Coagulator|Aegis Keep|54.2|4,000|25 - 35|80|25|25|25|?|Medium|1.3|-|-|Melee|Medium|1.3
Shambler|The Mausoleum|71.4|6,000|50 - 60|75|25|25|?|Slow|1.05|-|-|Melee|Slow|1.05
Weirdling|?|34.1|2,000|15 - 25|90|25|125|?|100|20 - 30|Slow|1.1|MeleeMage|Slow|1.1
Ocean Bedrock|Tidal Tomb|134.3|8,000|60 - 70|110|100|25|?|Medium|1.2|-|-|Melee|Medium|1.2
Effigy|Aegis Keep|20.1|2,000|15 - 25|75|50|25|?|Medium|1|-|-|Melee|Medium|1
Newly Reanimated|The Mausoleum|20.9|2,000|15 - 25|75|25|25|?|Medium|1.05|-|-|Melee|Medium|1.05
Corpus|Aegis Keep|393.8|16,000|70 - 80|130|25|25|?|Slow|2.25|-|-|Melee|Slow|2.25
Forgotten Golem|Time Dungeon|2425|150,000|100 - 130|120|500|100|100|50|125%|Fast|1.4|-|-|Melee|Fast|1.4
Gristle|The Mausoleum|259.2|14,000|45 - 55|170|25|100|?|Medium|1.7|-|-|Melee|Medium|1.7
Living Weapon|Time Dungeon|385.3|20,000|50 - 65|110|125|50|60|-|Medium|1.55|-|-|Melee|VeryFast|1.55
Exodus Construct Mk3|Time Dungeon|232.6|20,000|40 - 50|100|100|100|?|100%|Fast|1.6|-|-|Melee|Fast|1.6
Defiler Sentinel|Wilderness|988.9|250,000|25 - 35|100|75|100|?|Medium|1.6|-|-|Melee|Medium|1.6
Cave Gorger|Cavernam|124|8,000|40 - 50|100|25|25|?|Greater|50|40%|Medium|1.4|Melee|Medium|1.4
Greater Headless|Time Dungeon|167.3|12,000|50 - 60|110|50|75|?|25%|VeryFast|1.25|-|-|Melee|VeryFast|1.25
Greater Gazer|Time Dungeon|428.7|20,000|50 - 60|100|75|225|50|175|35 - 45|25%|Fast|1.5|MeleeMage|Fast|1.5
Prison Sentinel|Wilderness|78.9|5,000|30 - 40|90|75|25|?|Medium|1.4|-|-|Melee|Fast|1.4
Snowpiercer|Cavernam|28.4|2,000|15 - 25|65|25|25|?|Slow|1.5|-|-|Ranged|Slow|1.5
Blood Mephit|Shadowspire Cathedral|77.6|4,000|25 - 35|90|25|125|?|100|20 - 30|Fast|1.2|-|Mage|Fast|1.2
Bronze Automaton|Time Dungeon|242.7|16,000|60 - 70|110|175|50|?|100%|Fast|1.4|-|-|Melee|Fast|1.4
Hoarfrost|Cavernam|91.4|6,000|50 - 60|80|50|25|?|Slow|1.3|-|-|Melee|Slow|1.3
Observer|Pulma|24|2,000|15 - 25|70|25|25|?|Slow|1.25|-|-|Ranged|Slow|1.25
Entrails|Aegis Keep|218.4|14,000|60 - 70|150|25|25|25|?|Slow|1.35|-|-|Melee|Slow|1.35
Flesh Hound|The Mausoleum|67|5,000|25 - 35|80|25|25|?|Fast|1.35|-|-|Melee|Fast|1.35
Marble Golem|Time Dungeon|420.7|22,000|50 - 60|100|150|50|50|150|30 - 40|100%|Fast|1.5|MeleeMage|Fast|1.5
Slimy Mass|Ocean|56|4,000|25 - 35|100|25|25|25|?|60%|Slow|1.3|-|Ranged|Slow|1.3
Living Arsenal|Time Dungeon|2503|130,000|90 - 100|130|125|75|60|100%|SuperFast|1.7|-|-|Melee|SuperFast|1.7
Shadowglass|The Mausoleum|215|12,000|70 - 80|120|50|25|50|?|Medium|1.35|-|-|Melee|Medium|1.35
Winter Tendrils|Cavernam|181.7|14,000|50 - 60|140|25|25|25|?|Slow|1.3|-|-|Melee|Slow|1.3
The Cataract|Time Dungeon|3858|180,000|60 - 70|120|125|350|70|250|50 - 60|VeryFast|1.5|-|Mage|VeryFast|1.5
Sanguine Effigy|Aegis Keep|275|16,000|60 - 70|120|25|25|?|Slow|1.8|-|-|Melee|Slow|1.8
Reanimated Healer|The Mausoleum|79.2|5,000|10 - 20|90|25|100|?|75|15 - 25|Medium|1.2|-|Mage|Medium|1.2
Redcell|Aegis Keep|276|20,000|70 - 80|120|25|25|50|?|Medium|1.35|-|-|Melee|Medium|1.35
Precursor Warden|Pulma|209.8|12,000|50 - 60|110|75|25|?|Slow|1.8|-|-|Melee|Slow|1.8
Earth Mephit|Shadowspire Cathedral|77.6|4,000|25 - 35|90|25|125|?|100|20 - 30|Fast|1.2|-|Mage|Fast|1.2
Broken Idol|Mount Petram|37.2|2,500|10 - 20|70|75|75|?|50|10 - 20|VerySlow|1.1|-|Mage|VerySlow|1.1
Abomination|The Mausoleum|112.5|8,000|40 - 50|90|25|25|?|Medium|1.4|-|-|Melee|Medium|1.4
Stygian Servitor|?|32.2|3,000|10 - 20|70|75|100|?|60%|Fast|1.2|-|-|Melee|Fast|1.2
Reanimated Dragon|The Mausoleum|143.3|10,000|50 - 60|100|50|25|?|Medium|1.35|-|-|Melee|Medium|1.35
Insatiable Viscera|?|51.9|4,000|20 - 30|90|25|25|25|?|Medium|1.3|-|-|Melee|Medium|1.3
Marble Minotaur|Shadowspire Cathedral|134|6,000|40 - 50|110|100|25|?|Fast|1.7|-|-|Melee|Fast|1.7
Calamity|The Mausoleum|69.9|5,000|30 - 40|100|25|25|25|?|Slow|1.3|-|-|Ranged|Slow|1.3
Bone Blade|Ossuary|48.1|4,000|20 - 30|130|100|0|?|VeryFast|1|-|-|Melee|VeryFast|1
Watcher Idol|Mount Petram|45.2|3,000|15 - 25|70|75|75|?|50|10 - 20|VerySlow|1.14|-|Mage|VerySlow|1.14
Tidecaller Sentry|Ocean|80.4|4,000|20 - 30|100|25|150|?|125|25 - 35|Slow|1.1|-|Mage|Slow|1.1
Temple Guardian|Ossuary|93.5|6,000|40 - 50|90|75|25|?|Medium|1.35|-|-|Melee|Medium|1.35
Eldritch Mephit|Shadowspire Cathedral|77.6|4,000|25 - 35|90|25|125|?|100|20 - 30|Fast|1.2|-|Mage|Fast|1.2
Precursor Guardian|Pulma|131.2|8,000|40 - 50|90|75|25|?|Medium|1.6|-|-|Melee|Medium|1.6
Stygian Gaoler|Wilderness|1504|200,000|50 - 60|120|75|25|?|Medium|2|-|-|Melee|Medium|2
Astral Dread|The Mausoleum|317.3|20,000|35 - 45|140|25|225|?|225|45 - 55|Medium|1.2|-|Mage|Medium|1.2
Horrific Calamity|?|65.2|5,000|25 - 35|100|25|25|25|?|Slow|1.3|-|-|Ranged|Slow|1.3
Strangeling|?|1310|250,000|25 - 35|110|50|200|?|100|20 - 30|Fast|1.6|-|MeleeMage|Fast|1.6
Gazer|Wilderness|38.8|2,500|10 - 20|80|25|100|?|75|15 - 25|Slow|1|-|Mage|Slow|1
Reanimated Archer|The Mausoleum|42.8|5,000|30 - 40|95|25|25|?|Medium|1.14|-|-|Ranged|Medium|1.14
Reanimated Mage|The Mausoleum|33.6|2,000|10 - 20|80|25|100|?|75|15 - 25|Fast|1.05|-|Mage|Fast|1.05
Jade Androsphinx|Ossuary|359.7|14,000|60 - 70|140|75|250|?|-|Fast|2|-|-|Melee|Fast|2

## Batch 2 (Gold present after Difficulty)
Reanimated Knight|The Mausoleum|61.5|615|6,000|50 - 60|100|25|25|50|?|Melee|VeryFast|1.2
Molten Idol|Wilderness|72.1|721|4,000|30 - 40|90|50|125|?|100|20 - 30|Mage|Slow|1.1
Wintergrip|?|100.6|1,007|6,000|15 - 25|110|25|125|?|125|25 - 35|Mage|Slow|1.05
Riftling|?|10.6|106|1,000|10 - 20|70|25|125|?|-|Ranged|Slow|1.1
Outsider|?|51|511|2,500|25 - 35|110|50|150|?|125|25 - 35|Mage|Slow|1
Gazer Larva|Wilderness|27.3|274|2,000|10 - 20|70|25|75|?|50|10 - 20|Mage|Slow|1
Scarab|Ossuary|57.5|576|4,000|30 - 40|80|50|25|?|-|Melee|Fast|1.25
Disastral|?|187.2|1,873|10,000|20 - 30|120|25|175|?|175|35 - 45|Mage|Medium|1.2
Flesh Mob|The Mausoleum|203.4|2,034|14,000|60 - 70|130|25|25|?|-|Melee|Medium|1.3
Angelic Stonework|Shadowspire Cathedral|141.5|1,415|12,000|40 - 50|110|100|25|?|-|Melee|Slow|1.35
Prison Watchstone|Wilderness|122.6|1,227|6,000|30 - 40|95|75|150|?|125|25 - 35|Mage|VerySlow|1.25
Prison Warden|Wilderness|145.2|1,452|10,000|50 - 60|110|75|25|?|-|Melee|Medium|1.3
Fire Mephit|Shadowspire Cathedral|77.6|777|4,000|25 - 35|90|25|125|?|100|20 - 30|Mage|Fast|1.2
Poison Mephit|Shadowspire Cathedral|77.8|779|4,000|25 - 35|90|25|125|?|100|20 - 30|40%|Mage|Fast|1.2
Reanimated Daemon|The Mausoleum|205.2|2,052|16,000|60 - 70|120|25|200|?|175|35 - 45|MeleeMage|Medium|1.1
Marble Gargoyle|Shadowspire Cathedral|161.1|1,612|12,000|50 - 60|120|100|125|?|100|20 - 30|MeleeMage|Medium|1.25
Silver Weapon|Time Dungeon|387.2|?|4,000|200 - 250|150|75|25|60|-|Melee|VeryFast|1
Iron Automaton|Time Dungeon|517.9|5,179|30,000|85 - 95|130|300|125|?|100%|Melee|VeryFast|1.4
Reanimated Gladiator|The Mausoleum|41.4|414|5,000|35 - 45|100|25|25|50|?|-|Melee|Medium|1.05
Exodus Construct Mk5|Time Dungeon|401.6|4,017|30,000|50 - 60|110|125|125|?|100%|Melee|VeryFast|1.8
Eldritch Experiment|The Mausoleum|48|480|4,000|20 - 30|80|25|25|?|-|Ranged|Medium|1.25
Reanimated Hunter|The Mausoleum|46.7|468|5,000|40 - 50|85|25|25|?|-|Ranged|Fast|1.14
Insatiable Mass|?|65.2|652|5,000|25 - 35|100|25|25|25|?|-|Ranged|Slow|1.3
Flesh Golem|Time Dungeon|436.1|4,362|24,000|60 - 70|120|125|75|50|-|Melee|VeryFast|1.5
Exodus Construct Mk4|Time Dungeon|291.3|2,913|25,000|45 - 55|110|125|125|?|100%|Melee|Fast|1.6
Exodus Construct Mk 1|Time Dungeon|107|1,071|10,000|30 - 40|90|100|50|?|100%|Melee|Medium|1.3
Errant Core Of Exodus|Time Dungeon|3963|39,634|200,000|40 - 50|120|200|100|100|60|250|50 - 60|100%|X|Mage|SuperFast|1.5
Brass Automaton|Time Dungeon|323.6|3,236|20,000|70 - 80|120|250|75|?|100%|Melee|Fast|1.4
Malady|Kraul Hive|56.2|562|3,500|30 - 40|100|25|25|25|?|-|Melee|Medium|1.3
Sinew And Bone|The Mausoleum|253.1|2,532|16,000|60 - 70|150|25|25|?|-|Melee|Medium|1.4
Blood Gorger|Aegis Keep|253.7|2,538|20,000|60 - 70|140|25|25|?|-|Melee|Medium|1.3
Reanimated Soldier|The Mausoleum|39.9|400|5,000|30 - 40|110|25|25|?|-|Melee|Fast|1.05
Reanimated Pirate|The Mausoleum|46.7|468|5,000|30 - 40|90|50|25|50|60|-|Melee|Fast|1.05
Reanimated Cyclops|The Mausoleum|103.1|1,032|8,000|50 - 60|80|25|25|?|-|Melee|Medium|1.2
Raw Flesh|The Mausoleum|27.3|274|3,000|10 - 20|65|25|25|?|-|Melee|Slow|1.1
Meat Puppet|The Mausoleum|183.7|1,837|12,000|70 - 80|120|25|25|?|-|Melee|Medium|1.2
Hunchback|The Mausoleum|61.7|618|5,000|30 - 40|80|25|25|?|-|Melee|Medium|1.2
Eldritch Dreamer|The Mausoleum|248|2,481|14,000|50 - 60|140|25|200|?|200|40 - 50|MeleeMage|Medium|1.35
Stone Familiar|Shadowspire Cathedral|131.9|1,320|10,000|60 - 70|100|100|25|?|-|Melee|Medium|1.1
Reanimated Warrior|The Mausoleum|46.6|466|5,000|50 - 60|90|25|25|?|-|Melee|Medium|1.1
Frost Mephit|Shadowspire Cathedral|77.6|777|4,000|25 - 35|90|25|125|?|100|20 - 30|Mage|Fast|1.2
Daemonic Stonework|Shadowspire Cathedral|225.6|2,256|16,000|60 - 70|130|100|200|?|175|35 - 45|MeleeMage|Medium|1.14
Precursor Servitor|Pulma|29.3|293|2,500|10 - 20|65|75|25|?|-|Melee|Fast|1.3
Sphinx|Ossuary|59.7|598|4,000|30 - 40|90|50|25|?|-|Melee|Fast|1.25
Overseer|Pulma|80.2|803|6,000|40 - 50|110|25|125|?|100|20 - 30|MeleeMage|Slow|1
Exodus Construct Mk 2|Time Dungeon|157.4|1,575|15,000|40 - 50|100|125|75|?|100%|Melee|Medium|1.3
Stalagfright|Cavernam|40.6|406|3,000|20 - 30|80|25|75|?|50|10 - 20|MeleeMage|VerySlow|1.14
Winterglass|Cavernam|236|2,360|16,000|70 - 80|120|75|25|?|-|Melee|Medium|1.3
Icy Sedimental|Cavernam|230.8|2,308|14,000|70 - 80|120|100|25|?|-|Melee|Medium|1.35
# Raw: Daemonic section (agent: list-daemonic) — 116 rows, COMPLETE

Format: Name|Location|Difficulty|Gold|Hits|MeleeDmg|Wrestling|Armor|MagicResist|Parry|[sparse 0-7: AtkSpd/Magery/SpellDmg/Poison/Poisoning/PoisonResist/Stealth]|AI|Speed|UniqueScaler
QUALITY: raw HTML-derived (not summarizer-reformatted). First 10 fields + last 3 (AI|Speed|UniqueScaler) positionally RELIABLE. Middle sparse segment can't be safely split into labeled columns (source omits blank cells). Verified contiguous Bloody Hornbeast → Infernal Giant, next section = Elemental.

Bloody Hornbeast|Ocean|81.9|819|5,000|40 - 50|95|50|100|?|Melee|VeryFast|1.2
Frozen Fury|Cavernam|76.4|765|5,000|40 - 50|90|50|25|?|Melee|Medium|1.25
Abyssal Assassin|Wilderness|1491|100,000|250,000|25 - 35|120|50|100|60|Greater|20|40%|X|Melee|VeryFast|1.6
Daemonic Infantry|Aegis Keep|250.5|2,506|16,000|70 - 80|140|25|25|?|Melee|Medium|1.3
The Weaver Of Fate|Time Dungeon|2566|25,660|175,000|50 - 60|110|50|300|25|50|275|55 - 65|90%|Mage|SuperFast|1.3
Rift Warrior|Omni Realm|125.8|1,258|8,000|50 - 60|110|100|25|?|Melee|Medium|1.25
Screaming Horror|Wilderness|1580|100,000|250,000|30 - 40|120|50|100|60|Melee|VeryFast|1.6
Balron|Time Dungeon|888|8,881|45,000|65 - 75|120|75|300|60|200|40 - 50|MeleeMage|VeryFast|1.5
Mud Gargoyle|Shadowspire Cathedral|109.6|1,097|7,000|30 - 40|110|75|125|?|100|20 - 30|MeleeMage|Medium|1.25
The Damsel|Time Dungeon|3009|30,090|250,000|60 - 70|110|150|125|?|300|60 - 70|80%|Mage|SuperFast|1.6
Searing Imp|Inferno|24.3|244|1,500|10 - 20|60|25|75|?|50|10 - 20|Mage|Medium|1.14
Entropy Feaster|Time Dungeon|575.9|5,760|24,000|80 - 90|110|125|100|50|50|Deadly|33|75%|Melee|VeryFast|1.55
Lesser Blood Daemon|Aegis Keep|133.9|1,339|8,000|45 - 55|120|25|175|?|150|30 - 40|MeleeMage|Medium|1.14
Chaos Dweller|Time Dungeon|704.8|7,048|28,000|70 - 80|110|75|150|70|65%|Melee|SuperFast|1.65
Flamekeeper|Wilderness|580|25,000|75,000|40 - 50|120|50|150|?|125|25 - 35|MeleeMage|Medium|1.6
Minion|Pulma|27.3|274|2,000|10 - 20|65|25|75|?|50|10 - 20|Mage|Slow|1
Dreadmere The Damned|Time Dungeon|3684|36,841|170,000|100 - 120|150|150|500|60|350|70 - 80|100%|MeleeMage|SuperFast|1.35
Chaos Knight|Aegis Keep|108.3|1,084|8,000|30 - 40|110|75|25|?|Melee|Medium|1.4
Gargoyle Primogen|Shadowspire Cathedral|1611|100,000|200,000|50 - 60|110|75|150|?|125|25 - 35|MeleeMage|Medium|2
Shadow Minion|The Mausoleum|71.2|712|4,000|10 - 20|90|25|125|?|100|20 - 30|Mage|Slow|1.1
Blood Fiend|Aegis Keep|116.8|1,169|8,000|45 - 55|110|75|25|?|Melee|Slow|1.3
Blazing Archfiend|Wilderness|232.2|2,322|14,000|60 - 70|130|50|175|?|175|35 - 45|MeleeMage|Medium|1.3
Mirror Of Katrina|Time Dungeon|1491|14,913|60,000|80 - 100|150|100|200|60|275|55 - 65|100%|MeleeMage|SuperFast|2
Storm Daemon|Omni Realm|7878|200,000|1,500,000|50 - 60|130|50|100|?|Melee|VeryFast|1.4
Iron Gargoyle|Shadowspire Cathedral|97|970|7,000|35 - 45|90|100|125|?|100|20 - 30|MeleeMage|Medium|1.1
Frost Daevil|Cavernam|400.6|4,007|30,000|65 - 75|140|25|225|?|200|40 - 50|MeleeMage|Medium|1.4
Rage Daemon|Aegis Keep|421.3|4,214|30,000|75 - 85|140|25|225|?|200|40 - 50|MeleeMage|Fast|1.4
Frigid Hornbeast|Cavernam|61.6|617|4,000|30 - 40|100|50|25|?|Melee|VeryFast|1.2
Abyss Whisperer|Time Dungeon|995.8|9,959|35,000|40 - 50|120|75|200|60|200|40 - 50|66%|Mage|VeryFast|1.7
Infernus|Inferno|1536|100,000|200,000|40 - 50|110|50|150|?|125|25 - 35|MeleeMage|Medium|2
Infernal Gargoyle|Inferno|94.5|945|6,000|40 - 50|100|75|125|?|100|20 - 30|MeleeMage|Medium|1.14
Greater Infernal Daemon|Wilderness|1305|100,000|250,000|30 - 40|100|50|150|?|100|20 - 30|MeleeMage|Medium|1.6
Crag Daemon|Cavernam|212.5|2,125|16,000|65 - 75|130|75|175|?|150|30 - 40|MeleeMage|Medium|1.14
Horned Devil|Shadowspire Cathedral|167.5|1,675|10,000|50 - 60|130|50|100|45|Melee|Fast|1.1
Winged Hulk|Shadowspire Cathedral|118.6|1,187|8,000|55 - 65|100|25|25|?|Melee|Medium|1.2
Blood Hellion|Aegis Keep|49.5|496|3,500|25 - 35|80|25|25|?|Melee|VeryFast|1.25
Aegis Mongbat|Aegis Keep|25.6|257|2,500|15 - 25|80|25|25|?|Melee|Medium|1.05
Jungle Devourer|?|38.2|382|2,500|25 - 35|100|50|25|?|Melee|Medium|1.25
Infernal Mage|Inferno|145.5|1,455|8,000|40 - 50|110|25|175|?|150|30 - 40|Mage|Medium|1.14
Abyssal Executioner|Time Dungeon|4828|48,277|275,000|70 - 90|120|125|100|60|Lethal|75|90%|X|Melee|SuperFast|1.8
Entropic Weaver|Time Dungeon|657.7|6,577|22,000|40 - 50|100|50|250|50|50|225|45 - 55|33%|Mage|VeryFast|1.5
Blood Feaster|Aegis Keep|109.8|1,099|8,000|40 - 50|100|50|25|?|Melee|Medium|1.3
Blood Daemon|Aegis Keep|217.5|2,175|16,000|60 - 70|130|25|200|?|175|35 - 45|MeleeMage|Medium|1.14
Infernal Beastmaster|Inferno|69.9|700|8,000|40 - 50|110|50|25|45|Melee|Medium|1.2
Lesser Molten Daemon|Inferno|129.8|1,298|8,000|45 - 55|120|50|175|?|150|30 - 40|MeleeMage|Medium|1.1
Mirror Of Mariah|Time Dungeon|1453|14,531|42,000|40 - 50|100|50|400|50|50|400|80 - 90|33%|Mage|SuperFast|1.45
Fire Minion|Inferno|27.3|273|2,000|15 - 25|65|25|50|?|25|5 - 15|Mage|Medium|1.2
Mirror Of Jaana|Time Dungeon|743.1|7,432|50,000|40 - 50|110|50|300|40|300|60 - 70|75%|MeleeMage|SuperFast|1.5
Fen Daemon|Mount Petram|208.7|2,088|16,000|60 - 70|130|25|200|?|175|35 - 45|60%|MeleeMage|Medium|1.1
Aegis Imp|Aegis Keep|22.3|223|1,500|10 - 20|80|25|75|?|50|10 - 20|Mage|Medium|1.05
Roaming Destruction|Time Dungeon|2948|29,476|150,000|70 - 80|120|200|100|75|60|300|60 - 70|99%|MeleeMage|SuperFast|1.5
Molten Daemon|Inferno|220.6|2,206|16,000|60 - 70|130|50|200|?|175|35 - 45|MeleeMage|Medium|1.14
Daemon|Wilderness|191.5|1,915|14,000|60 - 70|120|25|200|?|175|35 - 45|MeleeMage|Medium|1.1
Infernal Daemon|Inferno|217.5|2,175|16,000|60 - 70|130|25|200|?|175|35 - 45|MeleeMage|Medium|1.14
Mirror Of Geoffrey|Time Dungeon|433.7|4,338|60,000|70 - 90|130|300|50|60|65%|Melee|SuperFast|1.45
Infernal Knight|Inferno|102.7|1,028|12,000|50 - 60|130|75|25|50|?|Melee|VeryFast|1.35
Non-Euclidean Space|Time Dungeon|1044|10,438|42,000|60 - 70|110|150|75|50|60|225|45 - 55|75%|MeleeMage|SuperFast|1.66
Chaotic Mass|Time Dungeon|720.7|7,208|31,000|80 - 90|100|75|300|60|100%|Melee|SuperFast|1.6
The One Left Behind|Time Dungeon|5095|50,954|250,000|70 - 80|150|75|200|70|275|55 - 65|50%|MeleeMage|SuperFast|1.65
Drowned Daemon|Pulma|208|2,081|16,000|60 - 70|130|25|200|?|175|35 - 45|MeleeMage|Medium|1.1
Familiar|The Mausoleum|21.1|212|1,500|10 - 20|60|25|75|?|50|10 - 20|Mage|Fast|1
Maleficarum The Voidwalker|Time Dungeon|1833|18,332|70,000|90 - 100|150|100|400|60|300|60 - 70|100%|MeleeMage|SuperFast|1.55
Incubus|The Mausoleum|179.4|1,794|10,000|10 - 20|130|25|200|?|200|40 - 50|Mage|Medium|1.05
Rift Footman|Omni Realm|90.1|901|6,000|40 - 50|100|75|25|?|Melee|Medium|1.25
Tidecaller Minion|Ocean|58.5|585|3,500|15 - 25|90|25|125|?|100|20 - 30|Mage|Slow|1
Abyssal Daemon|Omni Realm|8215|200,000|1,500,000|55 - 65|130|50|150|?|150|30 - 40|MeleeMage|Medium|1.4
Blood Ravager|Aegis Keep|113.6|1,136|8,000|50 - 60|110|50|25|?|Melee|Medium|1.14
Frost Daemon|Wilderness|217.5|2,175|16,000|60 - 70|130|25|200|?|175|35 - 45|MeleeMage|Medium|1.14
Horrific Fiend|?|27.7|278|2,000|20 - 30|90|75|25|?|Melee|Slow|1.25
Infernal Archer|Inferno|39.8|399|4,000|30 - 40|85|25|25|?|Ranged|Medium|1.25
Mirror Of Iolo|Time Dungeon|1145|11,447|50,000|70 - 85|120|100|150|50|90|175|35 - 45|Mage|SuperFast|1.2
Molten Mongbat|Inferno|34.3|344|2,500|20 - 30|90|50|25|?|Melee|Medium|1.25
Smouldering Gargoyle|Wilderness|138.7|1,388|8,000|40 - 50|110|75|150|?|150|30 - 40|MeleeMage|Medium|1.2
Infernal Chosen|Inferno|73.6|736|8,000|50 - 60|120|75|25|50|?|Melee|Medium|1.25
Blood Curdler|Aegis Keep|314.9|3,150|20,000|60 - 70|130|50|25|?|Melee|VeryFast|1.5
Infernal Sorcerer|Inferno|196.1|1,962|10,000|30 - 40|110|25|225|?|200|40 - 50|Mage|Medium|1.14
Lloth's Balor|Undermountain|458.1|50,000|1,000,000|30 - 40|120|50|100|?|100|20 - 30|MeleeMage|Fast|0.15
Deep Devourer|Pulma|120.7|1,207|8,000|50 - 60|95|50|25|?|Melee|Medium|1.3
Ice Knight|Cavernam|239.9|2,399|14,000|60 - 70|130|75|25|?|Melee|Medium|1.5
Frigid Archfiend|Cavernam|194.9|1,950|12,000|55 - 65|120|50|175|?|150|30 - 40|MeleeMage|Medium|1.3
Rift Minion|Omni Realm|44.8|449|3,000|20 - 30|100|25|125|?|100|20 - 30|MeleeMage|Slow|1
Astral Daemon|Omni Realm|8554|200,000|1,500,000|50 - 60|120|50|200|?|175|35 - 45|MeleeMage|Medium|1.4
Feral Mongbat|Time Dungeon|65.4|655|5,000|30 - 40|80|50|50|80|Melee|VeryFast|0.85
Gargoyle|Wilderness|29.9|299|2,000|25 - 35|85|75|100|?|75|15 - 25|MeleeMage|Medium|1
Void Spawn|Time Dungeon|824.3|8,244|38,000|50 - 60|110|50|100|25|40|275|55 - 65|75%|Mage|VeryFast|1.5
Void Lurker|Time Dungeon|697|6,970|32,000|50 - 60|110|75|75|50|250|50 - 60|75%|MeleeMage|VeryFast|1.55
All Seer|Time Dungeon|959.1|9,592|42,000|40 - 50|120|75|275|50|275|55 - 65|75%|MeleeMage|SuperFast|1.7
Corrupted Hornbeast|Shadowspire Cathedral|121.6|1,216|8,000|40 - 50|105|50|25|?|Melee|VeryFast|1.3
The One Who Wanders|Time Dungeon|4963|49,627|300,000|90 - 100|130|150|200|50|300|60 - 70|50%|MeleeMage|SuperFast|1.55
The One From Beyond The Stars|Time Dungeon|29244|292,445|750,000|25 - 35|140|150|300|100|80|450|90 - 100|75%|Mage|VeryFast|1.5
The Forgotten One|Time Dungeon|4138|41,377|250,000|80 - 90|140|100|275|70|Greater|50|50%|Melee|SuperFast|1.5
Molten Hellion|Inferno|65.5|656|4,000|30 - 40|95|50|25|?|Melee|VeryFast|1.3
Reflection|Time Dungeon|462.4|4,624|20,000|50 - 60|110|50|125|50|200|40 - 50|MeleeMage|VeryFast|1.5
Mooing Chaos|Time Dungeon|10536|105,358|400,000|80 - 90|140|350|350|80|75%|X|Melee|SuperFast|2.2
Mirror Of The Avatar|Time Dungeon|3126|31,262|100,000|50 - 60|150|175|200|50|60|325|65 - 75|99%|Mage|SuperFast|1.7
Mirror Of Julia|Time Dungeon|409|4,090|50,000|70 - 90|140|125|100|50|50%|Ranged|SuperFast|1.7
Mongbat|Wilderness|2.1|21|250|5 - 10|25|25|25|?|Melee|Medium|1
Maw Of The Abyss|Time Dungeon|6909|69,091|275,000|50 - 60|110|75|400|60|350|70 - 80|90%|MeleeMage|SuperFast|2.2
Lesser Balron|Time Dungeon|559.8|5,599|28,000|80 - 90|130|50|200|50|175|35 - 45|MeleeMage|VeryFast|1.4
Khil The Arch-Daemon|Time Dungeon|332.7|3,327|24,000|50 - 60|120|100|250|?|150|30 - 40|MeleeMage|VeryFast|1.5
Twisted Prevalian|Time Dungeon|576.6|5,767|25,000|60 - 70|110|125|75|50|60|75%|Melee|SuperFast|1.65
Chaos Spawn|Time Dungeon|741.7|7,418|32,000|75 - 85|120|75|100|25|70|50%|Ranged|VeryFast|1.6
Bedlam Gazer|Time Dungeon|724.9|7,250|26,000|30 - 40|120|75|225|50|225|45 - 55|25%|Mage|VeryFast|1.55
Timeless Cursed|Time Dungeon|587|5,871|21,000|50 - 60|100|75|150|40|275|55 - 65|75%|MeleeMage|VeryFast|1.8
Chaos Footman|Aegis Keep|38.8|388|3,000|25 - 35|95|75|25|?|Melee|Medium|1.1
Golden Gargoyle|Shadowspire Cathedral|125.4|1,254|7,000|35 - 45|110|75|125|?|100|20 - 30|MeleeMage|Medium|1.4
Imp|The Mausoleum|27.4|274|2,000|10 - 20|80|25|75|?|50|10 - 20|Mage|Medium|1
Crimson Gargoyle|Shadowspire Cathedral|114|1,141|7,000|30 - 40|110|75|125|?|100|20 - 30|MeleeMage|Medium|1.3
Rift Daemon|Omni Realm|226.9|2,270|16,000|60 - 70|130|25|200|?|175|35 - 45|MeleeMage|Medium|1.2
Lesser Rift Daemon|Omni Realm|139.7|1,397|8,000|45 - 55|120|25|175|?|150|30 - 40|MeleeMage|Medium|1.2
Mirror Of Dupre|Time Dungeon|598.9|5,990|90,000|70 - 80|120|200|100|50|70|50%|Melee|SuperFast|1.5
Aegis Minion|Aegis Keep|22.2|223|1,500|10 - 20|65|25|75|?|50|10 - 20|Mage|Slow|1.05
Infernal Soldier|Inferno|48.6|487|5,000|40 - 50|95|75|25|?|Melee|Medium|1.2
Gargoyle Archon|Shadowspire Cathedral|712.5|50,000|100,000|35 - 45|100|75|125|?|100|20 - 30|MeleeMage|Medium|1.8
Chaos Warrior|Aegis Keep|97.3|974|6,000|40 - 50|110|75|25|?|Melee|Medium|1.3
Infernal Giant|Inferno|130.8|1,308|10,000|50 - 60|90|25|25|?|Ranged|Medium|1.3
# Raw: Elemental section (agent: list-elemental) — 114 rows

Format (rows 1-60, 107-114): Name|Location|Slayer(-)|Difficulty|Gold|Hits|MeleeDmg|Wrestling|Armor|MagicResist|...|Magery|SpellDmg|AI|Speed|UniqueScaler
CAVEAT: rows 61-106 (Time-Dungeon-heavy middle batch) have COLUMN DRIFT — Difficulty/MagicResist/AtkSpd columns dropped/merged by summarizer; treat per-column values in that range as UNVERIFIED. Order preserved from page; "Muck" confirmed last Elemental row.

Water Spirit|Ocean|-|81.2|813|4,000|20 - 30|100|0|125|50|-|100|20 - 30|Mage|Medium|1.25
Dark Water|Pulma|-|140.4|1,404|8,000|50 - 60|110|25|175|25|-|150|30 - 40|MeleeMage|Slow|1.2
Putrid Water|Darkmire Temple|-|130.6|1,306|8,000|40 - 50|100|25|125|25|-|100|20 - 30|MeleeMage|Medium|1.35
Nox Elemental (Lesser)|Time Dungeon|-|430.1|4,302|15,000|60 - 70|100|50|125|50|50|200|40 - 50|MeleeMage|VeryFast|1.5
Standing Water|Wilderness|-|138.4|1,384|10,000|50 - 60|110|25|25|25|-|Melee|Medium|1.25|-
Permafrost|Cavernam|-|320.3|3,204|20,000|60 - 70|150|0|25|75|-|Melee|VeryFast|1.4
Blood Elemental|Aegis Keep|-|118.2|1,182|8,000|40 - 50|120|25|150|25|-|125|25 - 35|MeleeMage|Medium|1.1
Ancient Earth|Mount Petram|-|59.1|592|5,000|40 - 50|90|75|25|-|Melee|Slow|1|-
Black Ice|Winterlands|-|107.1|1,071|8,000|60 - 70|100|100|25|-|Melee|Medium|1|-
Cracked Earth Elemental|New Player Dungeon|-|8|80|800|15 - 25|50|75|25|-|Melee|Slow|1|-
Etinorox Wisp|Time Dungeon|-|502.4|5,025|20,000|30 - 40|100|75|275|-|275|55 - 65|Mage|VeryFast|1.6
Khamsin|Ossuary|-|133.4|1,335|7,000|40 - 50|120|0|25|50|60|Melee|VeryFast|1.1
The Leper|Time Dungeon|-|12737|127,373|500,000|70 - 80|140|125|300|100|50|450|90 - 100|Mage|SuperFast|1.5
Vgntrikphongue The Entropic Touch|Time Dungeon|-|11990|119,904|500,000|30 - 40|100|100|300|100|50|400|80 - 90|Mage|SuperFast|1.6
Volcanic Elemental|Wilderness|-|1063|100,000|250,000|35 - 45|90|100|100|-|Melee|Slow|1.6
Emerald Elemental|Mount Petram|-|141.4|1,415|10,000|55 - 65|110|100|25|-|Melee|Slow|1.25
Snowdrift|Cavernam|-|21|210|1,500|15 - 25|80|25|100|25|-|Melee|Slow|1.35
Dust Storm|Ossuary|-|542.9|5,429|40,000|60 - 70|140|0|25|75|-|Melee|Fast|1.8
Brackish Water|Darkmire Temple|-|49.1|492|3,000|20 - 30|85|25|100|25|-|75|15 - 25|MeleeMage|Medium|1.2
Amethyst Elemental|Mount Petram|-|101.1|1,012|7,000|45 - 55|100|100|25|-|Melee|Slow|1.25
Heart Of The Mountain|Inferno|-|846|50,000|100,000|30 - 40|90|25|150|-|125|25 - 35|Mage|VeryFast|1.8
Unbound Energy Vortex|Time Dungeon|-|322.8|3,229|17,000|60 - 70|120|50|150|-|Deadly|25|60%|Melee|SuperFast|1.5
Star Sapphire Elemental|Mount Petram|-|166.4|1,664|12,000|60 - 70|115|100|25|-|Melee|Slow|1.25
Bloodshard|Aegis Keep|-|242.3|2,424|14,000|60 - 70|110|75|150|-|Melee|VerySlow|1.8
Living Earth|Mount Petram|-|102.3|1,023|7,000|40 - 50|100|75|25|-|Melee|Slow|1.35
Lodestone|Mount Petram|-|544.2|50,000|100,000|30 - 40|90|100|25|-|-|Melee|Medium|1.8
Burning Ash Tree|Inferno|-|151.4|1,515|10,000|40 - 50|90|75|150|-|125|25 - 35|Mage|SuperSlow|1.4
Raging Tide|Ocean|-|67.9|679|3,000|30 - 40|100|25|150|25|-|175|35 - 45|MeleeMage|VeryFast|1.1
Insatiable Slime|-|-|17.9|180|1,500|15 - 25|70|25|25|25|-|50|40%|Melee|VerySlow|1.14
Ixplantoc The Builder|Time Dungeon|-|8801|88,008|500,000|90 - 110|130|175|150|70|-|Melee|SuperFast|1.6
Volcanic Familiar|Shadowspire Cathedral|-|134|1,341|10,000|50 - 60|110|75|25|-|Melee|Medium|1.2
King Muck|Time Dungeon|-|2879|28,786|200,000|80 - 90|120|500|25|25|50|Lethal|25|100%|Melee|SuperFast|1.45
Pure Water|Pulma|-|58.2|583|4,000|25 - 35|90|25|100|25|-|75|15 - 25|MeleeMage|Fast|1.1
Firestorm|Inferno|-|92|920|6,000|30 - 40|130|0|125|50|-|100|20 - 30|MeleeMage|Medium|1.14
Snow Flurry|Cavernam|-|26.6|267|2,000|15 - 25|90|0|75|50|-|50|10 - 20|MeleeMage|Fast|1.05
Shardling|Winterlands|-|61.3|614|6,000|30 - 40|100|100|25|-|Melee|Slow|1|-
Silicite|Cavernam|-|80.1|802|6,000|40 - 50|90|100|25|-|Melee|VerySlow|1.25
Searing Earth|Inferno|-|71.1|711|5,000|40 - 50|80|75|25|-|Melee|Slow|1.25
Bloodstone Elemental|Aegis Keep|-|225.5|2,255|16,000|75 - 85|110|100|25|-|Melee|Slow|1.3
Earth Elemental|Wilderness|-|54.7|548|5,000|35 - 45|85|75|25|-|Melee|Slow|1|-
Greater Water Elemental|Time Dungeon|-|209.9|2,099|13,000|40 - 50|110|50|125|-|150|30 - 40|MeleeMage|VeryFast|1.4
Rimestone|Cavernam|-|74.5|746|5,000|40 - 50|90|100|25|-|Melee|Slow|1.25
Snow Elemental|Cavernam|-|30.8|309|2,500|25 - 35|70|50|25|-|Melee|Slow|1.14
Bonfire Wisp|Inferno|-|138.1|1,381|6,000|30 - 40|130|25|200|25|-|175|35 - 45|Mage|Medium|1.14
Packed Snow|Winterlands|-|95|950|8,000|50 - 60|100|50|25|-|Melee|Slow|1.05
Volt Wisp|Mount Petram|-|138.1|1,381|6,000|30 - 40|130|25|200|25|-|175|35 - 45|Mage|Medium|1.14
Blood Golem|Aegis Keep|-|251.1|2,511|20,000|70 - 80|110|100|25|-|Melee|Slow|1.35
Twilight Guardian|Aegis Keep|-|85.2|853|5,000|30 - 40|100|0|125|50|-|100|20 - 30|MeleeMage|Slow|1.25
Sapphire Elemental|Mount Petram|-|117.4|1,175|8,000|50 - 60|105|100|25|-|Melee|Slow|1.25
Restless Sea|Ocean|-|24.8|248|1,500|20 - 30|90|0|100|50|-|75|15 - 25|MeleeMage|Fast|1.1
Raw Fire|Inferno|-|105.5|1,056|6,000|30 - 40|90|0|150|50|-|125|25 - 35|Mage|Medium|1.1
Amber Elemental|Mount Petram|-|70.3|703|5,000|35 - 45|90|100|25|-|Melee|Slow|1.25
Ocean Spray|Ocean|-|14.2|142|1,000|15 - 25|70|0|75|-|50|10 - 20|MeleeMage|Medium|1.1
Citrine Elemental|Mount Petram|-|42.3|423|3,000|25 - 35|90|100|25|-|Melee|Slow|1.25
Aegis Slime|Aegis Keep|-|17.8|178|1,500|15 - 25|75|25|25|25|-|Melee|VerySlow|1.2
Discordant Note|Time Dungeon|-|7539|75,386|500,000|60 - 70|130|100|200|90|Lethal|50|75%|Melee|SuperFast|1.6
Gelatinous Ooze|Time Dungeon|-|303.4|3,035|15,000|70 - 80|100|300|25|-|Deadly|40|100%|Melee|VeryFast|1.5
Cistern Gorgon|The Mausoleum|-|623.3|50,000|100,000|30 - 40|110|75|25|-|Ranged|VeryFast|1.8
Charoite|Cavernam|-|105.9|1,059|8,000|50 - 60|100|100|25|-|Melee|Slow|1.14
Greater Air Elemental|Time Dungeon|-|238.8|2,388|12,000|30 - 40|90|50|75|40|150|30 - 40|MeleeMage|VeryFast|1.5

## BELOW: rows 61-106 — COLUMN DRIFT, per-column values UNVERIFIED (format ~ Name|Location|Difficulty|Gold|Hits|MeleeDmg|Wrestling|Armor|MR-or-AtkSpd|...|AI|Speed|UniqueScaler)
Poison Finger|Time Dungeon|2706|27055|200000|60-70|110|100|275|VeryFast|1.5|-|-|MeleeMage|VeryFast|1.5
Protector Of Time|Time Dungeon|177.4|1775|10000|40-50|110|75|150|Fast|1.14|-|-|MeleeMage|Fast|1.14
Desert Wind|Ossuary|418.7|4187|24000|60-70|140|0|225|Fast|1.2|-|-|MeleeMage|Fast|1.2
Vortex|?|29.1|291|2000|20-30|60|0|150|Fast|1.14|-|-|Melee|Fast|1.14
Red Rubble|?|40.2|?|4000|20-30|90|75|25|Medium|1|-|-|Melee|Medium|1
Wisp|Wilderness|120.1|1201|6000|30-40|130|25|200|Medium|1|-|-|Mage|Medium|1
Water Elemental|Wilderness|63.3|633|5000|30-40|100|25|100|Medium|1|-|-|MeleeMage|Medium|1
Living Flame|Inferno|39.9|399|3000|15-25|80|0|25|Medium|1.4|-|-|Melee|Medium|1.4
Scoria|Wilderness|135.6|1357|10000|60-70|100|75|25|Slow|1.2|-|-|Melee|Slow|1.2
Red Rocks|Wilderness|111.6|1116|8000|30-40|110|75|25|Slow|1.5|-|-|Melee|Slow|1.5
Raging Tempest|Wilderness|164.7|1647|8000|30-40|130|25|150|Fast|1.3|-|-|Mage|Fast|1.3
Guardian Of Time|Time Dungeon|126.8|1268|7000|30-40|110|50|150|Fast|1.14|-|-|MeleeMage|Fast|1.14
Tourmaline Elemental|Mount Petram|55.5|556|4000|30-40|85|100|25|Slow|1.25|-|-|Melee|Slow|1.25
Void Slime|The Mausoleum|118.2|1183|8000|40-50|130|25|100|Slow|1.3|-|-|Ranged|Slow|1.3
Magma Elemental|Wilderness|142.9|1430|8000|40-50|110|0|25|Medium|1.3|-|-|MeleeMage|Medium|1.3
Ice Elemental|Wilderness|51.5|516|4000|30-40|80|50|25|Slow|1.2|-|-|Melee|Slow|1.2
Grinding Stone|Wilderness|80|801|6000|40-50|100|100|25|Slow|1.14|-|-|Melee|Slow|1.14
Floodwater|Wilderness|96.9|969|8000|30-40|120|0|25|Medium|1.25|-|-|Melee|Medium|1.25
Fire Elemental|Wilderness|64.7|647|4000|10-20|75|0|125|Medium|1|-|-|Mage|Medium|1
Pure Obsidian|Wilderness|171.8|1718|12000|70-80|110|100|25|Slow|1.2|-|-|Melee|Slow|1.2
Blackrock Elemental|Time Dungeon|645.9|6460|40000|80-90|110|75|600|VeryFast|1.55|-|-|Melee|VeryFast|1.55
Air Elemental|Wilderness|54.1|541|4000|20-30|110|0|100|Fast|1|-|-|MeleeMage|Fast|1
The Gray Exile|Undermountain|355|50000|1000000|20-30|100|0|200|Medium|0.1|-|-|Mage|Medium|0.1
Tox|Time Dungeon|1775|17753|110000|70-80|120|100|250|SuperFast|1.5|-|-|Melee|SuperFast|1.5
The Pilgrim|Time Dungeon|7269|72687|400000|70-80|150|150|200|SuperFast|1.5|-|-|Melee|SuperFast|1.5
The One Who Seeks|Time Dungeon|1773|17729|90000|60-70|120|100|275|SuperFast|1.5|-|-|Mage|SuperFast|1.5
Greater Blackrock Elemental|Time Dungeon|4326|43261|200000|110-120|130|100|800|SuperFast|1.65|-|-|Melee|SuperFast|1.65
Plantrilph The Screamer|Time Dungeon|8405|84045|500000|70-90|150|150|200|SuperFast|1.6|-|-|Melee|SuperFast|1.6
Nox Elemental (Greater)|Time Dungeon|504.3|5043|22000|60-70|110|50|175|VeryFast|1.5|-|-|MeleeMage|VeryFast|1.5
Nexus Warden|Time Dungeon|1056|10557|60000|50-60|130|175|300|VeryFast|1.6|-|-|MeleeMage|VeryFast|1.6
Void Elemental|Time Dungeon|877.9|8780|32000|60-70|110|100|225|VeryFast|1.65|-|-|MeleeMage|VeryFast|1.65
Greater Fire Elemental|Time Dungeon|227.5|2275|12000|20-30|100|50|150|Fast|1.45|-|-|Mage|Fast|1.45
Greater Earth Elemental|Time Dungeon|225.3|2254|14000|60-70|110|150|75|Fast|1.4|-|-|Melee|Fast|1.4
Gpouldvrng Of The Abyss|Time Dungeon|13051|130512|500000|40-50|75|100|400|SuperFast|1.6|-|-|Mage|SuperFast|1.6
Silver Elemental|Time Dungeon|231.6|?|8000|80-100|150|150|150|Fast|1|-|-|Melee|Fast|1
Conservator Of Time|Time Dungeon|245.8|2458|15000|45-55|110|100|200|Fast|1.14|-|-|MeleeMage|Fast|1.14
Bob|Time Dungeon|9667|96666|500000|70-80|120|125|350|SuperFast|1.6|-|-|MeleeMage|SuperFast|1.6
Ozghulrathagr The Many|Time Dungeon|8960|89598|500000|60-70|110|75|300|SuperFast|1.6|-|-|MeleeMage|SuperFast|1.6
Rgrundrthag The Swarm|Time Dungeon|7534|75338|500000|80-100|120|150|100|SuperFast|1.6|-|-|Melee|SuperFast|1.6
Rime Elemental|Cavernam|48.6|487|3000|20-30|85|25|100|Slow|1.2|-|-|MeleeMage|Slow|1.2
Shallow Water|Pulma|17.4|174|1500|15-25|65|25|25|VerySlow|1.2|-|-|Melee|VerySlow|1.2
Murky Water|Pulma|85.3|853|5000|35-45|100|25|125|Medium|1.2|-|-|MeleeMage|Medium|1.2
Maelstrom|Pulma|22.7|227|2000|15-25|60|0|25|Fast|1|-|-|Melee|Fast|1
Aged Earth|Mount Petram|27.5|275|2500|25-35|75|75|25|Slow|1|-|-|Melee|Slow|1
Greater Blood Elemental|Time Dungeon|497|4970|18000|60-70|100|100|75|VeryFast|1.45|-|-|MeleeMage|VeryFast|1.45
Foul Water|Darkmire Temple|97.1|971|6000|30-40|90|25|125|Medium|1.25|-|-|MeleeMage|Medium|1.25

## Rows 107-114 (clean format again)
Lava Elemental|Inferno|100.4|1,004|6,000|40 - 50|90|25|125|25|?|100|20 - 30|MeleeMage|Medium|1.25
Elder Watcher|Time Dungeon|1,084|10,844|75,000|40 - 50|110|75|300|100|50|350|70 - 80|MeleeMage|Fast|1.1
Ruby Elemental|Mount Petram|85.4|854|6,000|40 - 50|95|100|25|?|-|Melee|Slow|1.25
Mountain Air|Mount Petram|52.2|523|4,000|20 - 30|110|0|100|50|?|75|15 - 25|MeleeMage|Fast|1
Diamond Elemental|Mount Petram|193|1,930|14,000|65 - 75|120|100|25|?|-|Melee|Slow|1.25
Volcanite|Inferno|87.2|872|6,000|40 - 50|90|75|150|?|-|Melee|VerySlow|1.35
Living Water|Pulma|35.7|358|3,000|20 - 30|80|0|25|50|?|-|Melee|VeryFast|1.1
Muck|Darkmire Temple|17.7|177|1,500|15 - 25|65|25|25|25|?|Greater|50|40%|Melee|VerySlow|1.14
# Raw: Humanoid section (agent: list-humanoid) — 210 rows, AUTHORITATIVE (raw Lua module, grep-verified 210/210)

Format: Name|Location|Difficulty|Gold|Hits|MeleeDmg|Wrestling|Armor|MagicResist|AtkSpd|Magery|SpellDmg|Poison|AI|Speed|UniqueScaler
Parry/Stealth flags omitted by design in this 16-column projection.

Drow Captain|Ocean|60|601|8,000|60-70|130|25|25|-|-|-|-|Melee|Medium|1
Drow Crewman|Ocean|31.9|320|4,000|30-40|110|25|25|-|-|-|-|Melee|Medium|1
Druidic Voyager|Ocean|58.6|587|8,000|50-60|120|25|150|-|-|-|-|Melee|Fast|1
Explorer Captain|Ocean|63.6|636|8,000|60-70|120|50|25|-|-|-|-|Melee|Medium|1.05
Fisherman|Ocean|31.9|320|4,000|30-40|100|25|25|-|-|-|-|Melee|Medium|1
Fisherman Captain|Ocean|62.9|630|8,000|60-70|120|25|25|-|-|-|-|Melee|Medium|1.05
Hive Control Captain|Ocean|57.2|572|8,000|50-60|120|25|150|-|-|-|-|Melee|Medium|1
Marine|Ocean|34.1|341|4,000|30-40|100|75|25|-|-|-|-|Melee|Medium|1.05
Merchant Captain|Ocean|65.1|651|8,000|60-70|120|50|25|-|-|-|-|Melee|Medium|1.05
Norse Captain|Ocean|66.8|669|9,000|65-75|115|50|25|-|-|-|-|Melee|Medium|1
Norse Crewman|Ocean|36.3|363|4,500|35-45|95|25|25|-|-|-|-|Melee|Medium|1
Pirate|Ocean|31.9|320|4,000|30-40|100|25|25|-|-|-|-|Melee|Medium|1
Pirate Captain|Ocean|65.1|651|8,000|60-70|120|50|25|-|-|-|-|Melee|Medium|1.05
Raider|Ocean|37.7|378|4,500|35-45|100|25|25|-|-|-|-|Melee|Medium|1.05
Raider Captain|Ocean|72.7|728|9,000|65-75|120|50|25|-|-|-|-|Melee|Medium|1.1
Sailor|Ocean|31.9|320|4,000|30-40|100|25|25|-|-|-|-|Melee|Medium|1
Spy|Ocean|31.3|314|4,000|25-35|95|25|25|-|-|-|Greater|Melee|Medium|1
Tidecaller Cultist|Ocean|166.9|1,670|8,000|30-40|100|25|200|-|175|35-45|-|Mage|Slow|1.2
Tradesman|Ocean|31.9|320|4,000|30-40|100|25|25|-|-|-|-|Melee|Medium|1
Tradesman Foreman|Ocean|65.4|654|8,000|65-75|120|50|25|-|-|-|-|Melee|Medium|1.05
Vile Pedagogue|Ocean|139.6|1,396|8,000|20-30|100|25|200|-|175|35-45|-|Mage|Medium|1
Aegis High Priestess|?|1438|100,000|200,000|25-35|110|25|175|-|150|30-40|-|MeleeMage|Medium|2
Stranger|?|10.8|109|1,200|15-25|85|50|50|45|-|-|-|Ranged|Medium|1
Aegis High Priest|Aegis Keep|1438|100,000|200,000|25-35|110|25|175|-|150|30-40|-|MeleeMage|Medium|2
Sanguineous|Aegis Keep|390.9|50,000|100,000|30-40|100|75|25|-|-|-|-|Melee|VeryFast|1.8
Speaker For The Dead|Ossuary|857.1|50,000|100,000|30-40|90|50|150|-|125|25-35|-|Mage|Fast|1.8
Oblivion Deathmage|Wilderness|945.3|50,000|100,000|35-45|100|25|200|-|150|30-40|-|Mage|Medium|1.8
Aegis Bloodrider|Aegis Keep|148.7|1,487|18,000|80-90|140|75|25|-|-|-|-|Melee|VeryFast|1.35
Aegis Cultist|Aegis Keep|39.4|395|3,500|20-30|85|25|75|-|-|-|-|Mage|Slow|1.35
Aegis Deep Miner|Aegis Keep|141.1|1,411|14,000|40-50|120|25|25|-|-|-|-|Melee|Medium|2
Aegis Knight|Aegis Keep|60.9|610|6,000|40-50|110|75|25|-|-|-|-|Melee|VeryFast|1.25
Aegis Noble|Aegis Keep|35|350|4,000|35-45|100|50|25|-|-|-|-|Melee|Medium|1.05
Cave Explorer|Cavernam|174|1,741|12,000|60-70|140|50|100|-|-|-|-|Melee|Fast|2.25
Cave Hunter|Cavernam|180.4|1,804|12,000|50-60|150|50|25|-|-|-|-|Ranged|Fast|2.5
Ursal Forager|Cavernam|31|310|3,000|20-30|85|50|25|-|-|-|-|Melee|Fast|1.25
Ursal Huntsman|Cavernam|32.8|329|3,000|25-35|90|50|25|-|-|-|-|Melee|Fast|1.3
Ursal Throatsinger|Cavernam|46.9|469|3,000|15-25|80|50|75|-|50|10-20|-|Mage|Fast|1.2
Darkmire Bear Druid|Darkmire Temple|60.3|603|6,000|30-40|100|50|25|-|-|-|-|Melee|Fast|1.4
Darkmire Elder|Darkmire Temple|113.4|1,134|5,000|15-25|90|25|125|-|100|20-30|-|Mage|Medium|1.5
Darkmire Hunter|Darkmire Temple|48.8|489|5,000|30-40|95|50|25|-|-|-|-|Ranged|VeryFast|1.25
Darkmire Pathfinder|Darkmire Temple|56.8|569|5,000|40-50|100|50|25|-|-|-|-|Ranged|VeryFast|1.35
Darkmire Tribal|Darkmire Temple|69.6|697|5,000|20-30|100|25|25|-|-|-|-|Ranged|Medium|2
Darkmire Wolf Druid|Darkmire Temple|49|491|5,000|20-30|110|50|25|45|-|-|-|Melee|Fast|1.3
Reaper Of Souls|Field of Souls|0.2|3|50|-1|0|0|0|-|-|-|-|Melee|Medium|0.8
Petram Cultist|Mount Petram|106.7|1,068|6,000|15-25|100|25|175|-|150|30-40|-|Mage|Medium|1
Easter Rabbit|-|27.8|279|2,000|10-20|150|25|300|60|-|-|-|Melee|VeryFast|1
Rift Cultist|Omni Realm|76.1|761|5,000|30-40|110|50|175|-|150|30-40|-|MeleeMage|Medium|1
Hierarch|Ossuary|127.1|1,272|6,000|20-30|80|25|125|-|100|20-30|-|Mage|Slow|1.5
Ossuarian Deathbringer|Ossuary|81.3|814|8,000|40-50|110|25|25|-|-|-|-|Melee|Fast|1.5
Ossuarian Dervish|Ossuary|41.6|416|5,000|25-35|100|25|25|75|-|-|-|Melee|Medium|1
Ossuarian Embalmer|Ossuary|70.1|702|5,000|30-40|95|25|100|-|75|15-25|-|MeleeMage|Medium|1.25
Ossuarian Executioner|Ossuary|55.9|560|5,000|30-40|100|25|25|-|-|-|-|Melee|Medium|1.5
Ossuarian Fire Priestess|Ossuary|94.4|944|5,000|20-30|85|25|125|-|100|20-30|-|Mage|Medium|1.25
Ossuarian Firebrand|Ossuary|117.9|1,180|8,000|30-40|120|25|25|45|-|-|-|Melee|Medium|1.3
Ossuarian Hunter|Ossuary|51.7|517|6,000|30-40|100|25|25|45|-|-|-|Melee|VeryFast|1.1
Ossuarian Lion Warrior|Ossuary|51|510|6,000|35-45|110|25|25|-|-|-|-|Melee|Medium|1.14
Ossuarian Pyromancer|Ossuary|98.3|983|5,000|20-30|90|25|125|-|100|20-30|-|Mage|Medium|1.3
Ossuarian Sightless Seer|Ossuary|170.1|1,702|8,000|30-40|90|25|175|-|150|30-40|-|Mage|Medium|1.35
Ossuarian Skirmisher|Ossuary|41.4|415|5,000|35-45|110|25|25|-|-|-|-|Melee|Medium|1.05
Ossuarian Slinger|Ossuary|61.3|614|5,000|30-40|90|25|25|-|-|-|-|Ranged|Medium|1.14
Precursor Conduit|Pulma|81.4|814|6,000|40-50|100|50|25|-|-|-|-|Ranged|Medium|1.8
Precursor Engineer|Pulma|52.5|526|5,000|35-45|95|50|25|-|-|-|-|Melee|Medium|1.35
Precursor Lancer|Pulma|48.2|482|5,000|40-50|90|50|25|-|-|-|-|Melee|Medium|1.2
Precursor Operator|Pulma|72.8|728|6,000|40-50|110|50|25|60|-|-|-|Melee|Fast|1.35
Precursor Sentry|Pulma|29.9|300|3,000|20-30|85|50|25|-|-|-|-|Melee|Medium|1.25
Precursor Technologist|Pulma|59.2|593|5,000|25-35|85|50|25|-|-|-|-|Ranged|Medium|1.6
Fleshweaver|The Mausoleum|187.6|1,876|10,000|20-30|120|25|200|-|200|40-50|-|Mage|Medium|1.1
Insane Reanimator|The Mausoleum|105.3|1,053|6,000|10-20|90|25|150|-|125|25-35|-|Mage|Medium|1.1
Master Reanimator|The Mausoleum|153|1,531|8,000|10-20|100|25|200|-|175|35-45|-|Mage|Medium|1.1
Muddled Vampire Hunter|The Mausoleum|96.3|964|10,000|40-50|140|50|25|-|-|-|-|Ranged|Medium|1.6
Reanimator|The Mausoleum|56.6|567|4,000|20-30|90|25|100|-|75|15-25|-|MeleeMage|Medium|1.2
Reanimator Apprentice|The Mausoleum|34.2|342|2,500|15-25|80|25|75|-|50|10-20|-|MeleeMage|Medium|1.2
Reanimator Rector|The Mausoleum|84.1|842|6,000|30-40|100|25|125|-|100|20-30|-|MeleeMage|Medium|1.2
Reanimator Zealot|The Mausoleum|49.6|496|3,000|10-20|75|25|100|-|75|15-25|-|Mage|Medium|1.1
Lowtide Scavenger|Tidal Tomb|44.6|446|4,000|35-45|100|25|25|-|-|-|-|Melee|Medium|1.35
Apostate Apothecary|Time Dungeon|163.4|1,634|14,000|60-70|130|50|125|80|-|-|-|Melee|Fast|1.45
Apostate Captain|Time Dungeon|156.1|1,561|16,000|45-55|110|125|50|60|-|-|-|Melee|VeryFast|1.55
Apostate Knight|Time Dungeon|264.9|2,650|24,000|75-85|150|100|25|80|-|-|-|Melee|VeryFast|1.5
Apostate Mage|Time Dungeon|182|1,821|9,000|30-40|110|25|200|-|125|25-35|-|Mage|Fast|1.5
Apostate Major|Time Dungeon|264.7|2,648|30,000|80-90|140|150|100|50|-|-|-|Melee|VeryFast|1.5
Apostate Medic|Time Dungeon|145.3|1,453|14,000|50-60|120|25|125|60|-|-|-|Melee|Fast|1.55
Apostate Ranger|Time Dungeon|249.1|2,491|19,000|50-60|130|75|50|50|-|-|-|Ranged|VeryFast|2.29
Apostate Scout|Time Dungeon|140.9|1,409|11,000|50-60|110|50|50|80|-|-|Deadly|Ranged|VeryFast|1.5
Apostate Soldier|Time Dungeon|149.3|1,494|15,000|50-60|120|75|50|60|-|-|-|Melee|VeryFast|1.45
Apostate Wizard|Time Dungeon|283.5|2,835|14,000|30-40|110|25|200|-|200|40-50|-|Mage|VeryFast|1.4
Arannis The Researcher Of Time|Time Dungeon|399.5|3,995|16,000|20-30|100|50|125|40|225|45-55|-|Mage|VeryFast|1.35
Arnold The Bodybuilder|Time Dungeon|1002|10,019|90,000|45-55|100|75|200|-|225|45-55|-|MeleeMage|VeryFast|1.8
Assassin Of The Void|Time Dungeon|571.8|5,719|30,000|80-90|120|50|150|90|-|-|Deadly|Melee|SuperFast|2.35
Blackrock Cultist|Time Dungeon|546|5,461|21,000|35-45|110|25|150|50|225|45-55|-|Mage|VeryFast|1.35
Captain Of The Guard|Time Dungeon|650|6,500|75,000|80-90|150|250|75|80|-|-|-|Melee|VeryFast|1.7
Captain Reginald|Time Dungeon|838.3|8,383|90,000|100-120|130|200|100|90|-|-|-|Melee|SuperFast|1.55
Exodus Project Overseer|Time Dungeon|376.6|3,766|21,000|20-30|100|50|175|-|200|40-50|-|Mage|Fast|1.5
Exodus Researcher|Time Dungeon|407.6|4,076|16,000|20-30|100|50|125|50|200|40-50|-|Mage|Fast|1.25
Exoguard|Time Dungeon|329.2|3,293|25,000|70-80|150|200|50|70|-|-|-|Melee|VeryFast|1.9
High Priestess Of The Void|Time Dungeon|1565|15,647|80,000|20-30|120|50|200|50|325|65-75|-|Mage|SuperFast|1.3
Indecorous Mage|Time Dungeon|277.8|2,779|13,000|20-30|95|50|125|40|175|35-45|-|Mage|Fast|1.25
Mirror Of Shamino|Time Dungeon|613.2|6,132|60,000|70-80|130|50|75|70|-|-|-|Ranged|SuperFast|2.2
Priest Of Mondain|Time Dungeon|317.3|3,174|15,000|10-20|120|25|150|-|225|45-55|-|Mage|Fast|1.4
Priestess Of The Void|Time Dungeon|613.4|6,134|23,000|10-20|100|25|150|40|275|55-65|-|Mage|SuperFast|1.5
Sigil Of The Void|Time Dungeon|481.8|4,818|30,000|80-90|130|75|225|80|-|-|-|Ranged|VeryFast|2.4
Soldier Of The Void|Time Dungeon|607.4|6,074|40,000|80-90|120|125|50|80|-|-|-|Melee|SuperFast|2.29
Vesperon The Time Cultist|Time Dungeon|415.1|4,152|16,000|30-40|110|50|125|50|175|35-45|-|Mage|Fast|1.4
Watchman Of The Void|Time Dungeon|575.6|5,757|35,000|70-80|130|75|100|90|-|-|-|Ranged|VeryFast|2.8
Arboreal Defender|Wilderness|38.5|386|4,000|40-50|130|25|25|-|-|-|-|Melee|Fast|1.1
Arboreal Huntsman|Wilderness|51.6|517|4,000|20-30|120|25|25|45|-|-|Deadly|Melee|Fast|1.5
Arboreal Stalker|Wilderness|43.5|435|4,000|20-30|130|25|25|60|-|-|-|Melee|Fast|1.3
Arboreal Tree-Tongue|Wilderness|84.4|845|4,000|15-25|110|25|125|-|100|20-30|-|Mage|Fast|1.3
Arboreal Warden|Wilderness|47.3|473|4,000|35-45|120|25|25|-|-|-|-|Ranged|Fast|1.4
Arboreal Watcher|Wilderness|42.4|424|4,000|30-40|120|25|25|-|-|-|-|Ranged|Fast|1.3
Astral Summoner|Wilderness|137.1|1,371|8,000|30-40|120|50|175|-|175|35-45|-|MeleeMage|Medium|1.25
Barbaric Bruiser|Wilderness|105.1|1,052|12,000|70-80|110|50|25|-|-|-|-|Melee|Fast|1.3
Barbaric Chieftain|Wilderness|126.4|1,264|14,000|70-90|130|50|25|-|-|-|-|Melee|Fast|1.4
Barbaric Hunter|Wilderness|74.5|745|10,000|60-80|120|50|25|20|-|-|-|Ranged|Fast|1.2
Barbaric Shaman|Wilderness|162.5|1,626|10,000|30-40|110|25|150|-|150|30-40|-|Mage|Medium|1.14
Barbaric Warrior|Wilderness|82.4|824|10,000|40-50|120|50|25|45|-|-|-|Melee|Fast|1.2
Blood Cult Enforcer|Wilderness|66.8|668|6,000|50-60|110|25|25|-|-|-|-|Melee|Medium|1.4
Blood Cult Fanatic|Wilderness|48.8|489|5,000|50-60|110|25|25|-|-|-|-|Melee|Medium|1.14
Blood Cult Prelate|Wilderness|128.1|1,281|6,000|10-20|100|25|175|-|150|30-40|-|Mage|Medium|1.2
Blood Cult Priest|Wilderness|68.6|687|5,000|30-40|100|25|125|-|100|20-30|-|MeleeMage|Medium|1.1
Blood Cult Torturer|Wilderness|58.7|588|6,000|30-40|110|25|25|-|-|-|-|Melee|Medium|1.4
Blood Cult Zealot|Wilderness|46|461|5,000|30-40|100|25|25|45|-|-|-|Melee|Medium|1.14
Brigand Ambusher|Wilderness|37.5|376|3,500|30-40|100|25|25|-|-|-|-|Ranged|Medium|1.3
Brigand Beastmaster|Wilderness|34.6|347|3,500|35-45|95|50|25|-|-|-|-|Melee|Medium|1.14
Brigand Footman|Wilderness|29.7|298|3,500|30-40|100|50|25|-|-|-|-|Melee|Medium|1
Brigand Hedge Mage|Wilderness|51.7|518|3,000|10-20|90|25|125|-|100|20-30|-|Mage|Medium|1
Brigand Knifeman|Wilderness|37.2|373|3,500|25-35|90|25|25|60|-|-|-|Melee|Medium|1.2
Brigand Leader|Wilderness|39.9|400|4,500|50-60|110|50|25|-|-|-|-|Melee|Medium|1
Brigand Ransacker|Wilderness|39.2|392|3,500|35-45|100|50|25|-|-|-|-|Melee|Medium|1.3
Brigand Thug|Wilderness|42.5|426|4,000|35-45|95|50|25|-|-|-|-|Melee|Medium|1.25
Bushwhacker|Wilderness|71.4|715|6,000|30-40|120|25|25|-|-|-|-|Melee|Medium|1.7
Cattle Rustler|Wilderness|89.3|894|8,000|30-40|110|25|25|-|-|-|-|Melee|Medium|1.8
Deranged Monster Hunter|Wilderness|50.2|503|5,000|40-50|90|50|25|-|-|-|-|Melee|Medium|1.25
Deserter Captain|Wilderness|48|481|4,500|50-60|120|50|25|-|-|-|-|Melee|Medium|1.2
Deserter Mage|Wilderness|51.7|518|3,000|15-25|90|25|125|-|100|20-30|-|Mage|Medium|1
Deserter Scout|Wilderness|33.5|335|3,500|35-45|100|25|25|-|-|-|-|Ranged|Fast|1.1
Deserter Soldier|Wilderness|36.4|365|3,500|40-50|110|50|25|45|-|-|-|Melee|Medium|1.05
Disgraced Knight|Wilderness|59.7|598|6,000|50-60|120|75|25|-|-|-|-|Melee|Medium|1.2
Disgraced Man-At-Arms|Wilderness|47.4|474|5,000|40-50|110|50|25|-|-|-|-|Melee|Medium|1.14
Disgraced Squire|Wilderness|41.4|414|5,000|30-40|100|50|25|-|-|-|-|Ranged|Medium|1.1
Dragon Worshipper|Wilderness|166.7|1,667|10,000|50-60|120|25|150|45|150|30-40|-|MeleeMage|Medium|1.14
Drow Blademaster|Wilderness|71.6|716|8,000|40-50|120|50|50|60|-|-|Deadly|Melee|VeryFast|1
Drow Ranger|Wilderness|76.6|767|7,000|35-45|110|50|50|45|-|-|-|Ranged|VeryFast|1.4
Drow Spellweaver|Wilderness|140.6|1,406|7,000|20-30|100|25|175|-|150|30-40|-|Mage|VeryFast|1.2
Druid Bear Warrior|Wilderness|52.2|523|5,000|30-40|100|25|25|-|-|-|-|Melee|Medium|1.4
Druid Elk Warrior|Wilderness|53.2|533|5,000|30-40|110|25|25|-|-|-|-|Melee|Fast|1.4
Duergar Battlerager|Wilderness|96.7|968|9,000|30-40|100|75|50|-|-|-|-|Melee|Medium|1.8
Duergar Defender|Wilderness|78.4|784|9,000|50-60|110|75|50|-|-|-|-|Melee|Medium|1.25
Duergar Runemaster|Wilderness|91.2|913|9,000|40-50|105|50|50|-|-|-|-|Ranged|Medium|1.6
Evil Mage|Wilderness|56.4|565|4,000|10-20|80|25|100|-|75|15-25|-|Mage|Medium|1
Frenzied Gladiator|Wilderness|46.5|466|5,000|30-40|90|25|25|-|-|-|-|Melee|Medium|1.25
Frostbane Acolyte|Wilderness|78.7|788|4,000|10-20|95|50|125|-|100|20-30|-|Mage|Medium|1.2
Frostbane Priest|Wilderness|103.8|1,038|5,000|10-20|100|50|150|-|125|25-35|-|Mage|Medium|1.2
Frostbane Vicar|Wilderness|129.9|1,300|6,000|10-20|105|50|175|-|150|30-40|-|Mage|Medium|1.2
Gloomwood Brute|Wilderness|96.6|967|10,000|60-80|120|25|25|-|-|-|-|Melee|Medium|1.4
Gloomwood Firebreather|Wilderness|122.8|1,229|8,000|40-50|120|50|25|-|-|-|-|Ranged|Medium|1.35
Gloomwood Fury|Wilderness|110.8|1,108|8,000|20-30|120|25|25|90|-|-|-|Melee|Fast|1.14
Gloomwood Hunter|Wilderness|93.6|936|10,000|30-40|120|25|25|45|-|-|-|Ranged|VeryFast|1.5
Gloomwood Medicine Woman|Wilderness|139.7|1,397|8,000|30-40|120|25|150|-|150|30-40|-|MeleeMage|Medium|1.4
Gloomwood Pathfinder|Wilderness|93.2|932|10,000|40-50|120|25|25|-|-|-|-|Ranged|VeryFast|1.5
Gloomwood Shaman|Wilderness|147.1|1,472|8,000|30-40|110|50|150|-|150|30-40|-|Mage|Medium|1.14
Gloomwood Snakehandler|Wilderness|107.2|1,073|8,000|20-30|120|25|25|-|-|-|-|Ranged|Medium|1.6
Grave Robber|Wilderness|25.5|255|3,000|30-40|90|25|25|-|-|-|-|Melee|Medium|1
Highwayman|Wilderness|63.5|636|6,000|30-40|110|50|25|-|-|-|-|Ranged|Medium|1.5
Horsethief|Wilderness|66.2|663|8,000|70-80|130|50|25|25|-|-|-|Ranged|VeryFast|1.05
Living Necromancer|Wilderness|106.7|1,068|6,000|10-20|100|25|175|-|150|30-40|-|Mage|Medium|1
Marooned Pirate|Wilderness|26.1|261|3,000|30-40|100|25|25|-|-|-|-|Melee|Medium|1
Marooned Pirate Captain|Wilderness|42.7|428|4,000|40-50|120|75|25|-|-|-|-|Melee|Medium|1.2
Minax Prisoner|Wilderness|67.4|674|4,000|40-50|110|25|150|-|125|25-35|-|MeleeMage|Medium|1
Nomadic Guard|Wilderness|37|371|4,000|40-50|110|50|25|-|-|-|-|Melee|Medium|1.05
Nomadic Horse Archer|Wilderness|41.2|412|4,500|25-35|90|50|25|45|-|-|-|Ranged|VeryFast|1.1
Nomadic Janissary|Wilderness|45|450|4,000|30-40|100|50|25|45|-|-|-|Melee|Medium|1.3
Nomadic Outrider|Wilderness|40.8|409|4,000|35-45|110|50|25|-|-|-|-|Melee|VeryFast|1.14
Nomadic Rifleman|Wilderness|45.6|456|4,500|50-60|90|25|25|-|-|-|-|Ranged|VeryFast|1.1
Norse Axeman|Wilderness|50.8|508|5,000|40-50|95|50|25|-|-|-|-|Melee|Medium|1.25
Norse Bear Rider|Wilderness|63.2|632|6,000|50-60|95|50|25|-|-|-|-|Melee|VeryFast|1.25
Norse Firetender|Wilderness|93.7|937|5,000|25-35|90|25|150|-|125|25-35|-|Mage|Medium|1.1
Norse Hammerman|Wilderness|50.7|508|5,000|40-50|90|50|25|-|-|-|-|Melee|Medium|1.25
Norse Herdsman|Wilderness|60.7|607|5,000|30-40|90|25|25|-|-|-|-|Melee|Fast|1.6
Norse Mage|Wilderness|83.1|832|5,000|10-20|90|25|125|-|100|20-30|-|Mage|Medium|1.1
Norse Settler|Wilderness|63.2|633|5,000|20-30|80|25|25|-|-|-|-|Melee|Fast|1.8
Norse Swordsman|Wilderness|51.7|518|5,000|35-45|100|50|25|-|-|-|-|Melee|Medium|1.3
Norse Trapper|Wilderness|55.2|552|5,000|30-40|95|25|25|45|-|-|-|Melee|Fast|1.35
Outlaw|Wilderness|57.3|574|6,000|50-60|120|25|25|-|-|-|-|Ranged|Medium|1.2
Rebel Captain|Wilderness|50.3|504|4,500|50-60|110|75|25|-|-|-|-|Melee|Medium|1.25
Rebel Field Medic|Wilderness|34|340|3,500|20-30|90|25|25|45|-|-|-|Melee|Medium|1.2
Rebel Mage|Wilderness|45.2|453|3,000|10-20|90|25|100|-|75|15-25|-|Mage|Medium|1
Rebel Scout|Wilderness|31.1|311|3,500|25-35|90|25|25|-|-|-|-|Ranged|Fast|1.1
Rebel Soldier|Wilderness|29.7|298|3,500|30-40|100|50|25|-|-|-|-|Melee|Medium|1
Sunstone Air Mage|Wilderness|131.2|1,312|8,000|30-40|110|50|150|-|150|30-40|-|MeleeMage|Medium|1.3
Sunstone Alchemer|Wilderness|72.8|729|8,000|20-30|120|50|25|45|-|-|Lethal|Melee|Medium|1.4
Sunstone Guardsman|Wilderness|60.3|603|8,000|40-50|120|50|25|-|-|-|-|Melee|Medium|1.1
Sunstone Noble|Wilderness|141.6|1,417|10,000|30-40|120|25|25|45|100|20-30|-|MeleeMage|Medium|1.25
Sunstone Royal Guard|Wilderness|104.7|1,048|10,000|60-80|130|50|25|-|-|-|-|Melee|Medium|1.5
Sunstone Sentry|Wilderness|61.8|619|8,000|30-40|110|50|25|45|-|-|-|Ranged|Medium|1.14
Traveling Sellbow|Wilderness|94.7|947|10,000|30-40|130|50|25|45|-|-|-|Ranged|VeryFast|1.5
Traveling Sellsword|Wilderness|108.3|1,083|10,000|50-60|130|50|25|45|-|-|-|Melee|Medium|1.5
Tribal Chieftain|Wilderness|67|670|6,000|60-70|120|0|25|-|-|-|-|Melee|Medium|1.35
Tribal Fanatic|Wilderness|43|431|5,000|40-50|110|0|25|-|-|-|-|Melee|Medium|1.1
Tribal Hunter|Wilderness|54.1|541|5,000|40-50|100|25|25|-|-|-|-|Melee|VeryFast|1.3
Tribal Poisoner|Wilderness|60.2|603|5,000|30-40|100|25|25|45|-|-|-|Melee|Medium|1.5
Tribal Shaman|Wilderness|98.3|983|5,000|20-30|90|25|125|-|100|20-30|-|Mage|Medium|1.3
Umbermare Defender|Wilderness|34.1|341|4,000|35-45|110|50|25|-|-|-|-|Melee|Medium|1
Umbermare Harrier|Wilderness|42.4|425|4,500|35-45|100|50|25|-|-|-|-|Ranged|VeryFast|1.1
Umbermare Lancer|Wilderness|54.6|546|4,500|50-60|110|50|25|-|-|-|-|Melee|VeryFast|1.3
Umbermare Marksman|Wilderness|36.2|363|4,000|50-60|100|50|25|20|-|-|-|Ranged|Medium|1.1
Umbermare Outrider|Wilderness|46.4|464|4,500|35-45|110|50|25|-|-|-|-|Melee|VeryFast|1.2
Umbermare Ranger|Wilderness|38.4|384|4,000|35-45|110|50|25|-|-|-|-|Ranged|Medium|1.14
Umbermare Seer|Wilderness|85.6|856|4,500|10-20|90|50|125|-|100|20-30|-|Mage|VeryFast|1.2
Umbermare Spearman|Wilderness|36.3|363|4,000|40-50|120|50|25|-|-|-|-|Melee|Medium|1.05
Ursal Exile|Wilderness|54.3|544|6,000|50-60|110|50|25|-|-|-|-|Ranged|Fast|1.1
# Raw: Monstrous section (agent: list-monstrous) — 184 rows, COMPLETE (verified contiguous, single-pass fetch)

Format: Name|Location|Difficulty|Gold|Hits|MeleeDmg|Wrestling|Armor|MagicResist|[sparse optional: Parry/AtkSpd/Magery/SpellDmg/Poison/Poisoning/PoisonResist/Stealth]|AI|Speed|UniqueScaler
Source table omits blank optional cells — middle segment varies per row. First 9 + last 3 fields reliable.

Minotaur Skirmisher|Wilderness|132.4|1,325|8,000|40 - 50|110|50|25|60|X|Melee|Fast|1.1
Weald Rat Chieftain|Darkmire Temple|32.1|322|3,000|15 - 25|90|25|25|45|Melee|Medium|1
Minotaur Berserker|Mount Petram|101|1,011|6,000|40 - 50|90|75|25|?|Melee|Fast|1.4
Ophidian Warrior|Nusero|70.1|702|5,000|40 - 50|110|50|25|?|Greater|50|40%|Melee|Medium|1
Opilion Worker|Cavernam|34.1|341|3,000|20 - 30|80|50|25|?|Melee|Medium|1.1
Minotaur Runesmith|Mount Petram|127.4|1,274|8,000|40 - 50|110|75|200|?|Melee|Fast|1.35
Cold One|Time Dungeon|170.9|1,710|12,000|45 - 55|110|75|50|?|Melee|VeryFast|1.35
Bottomfeeder|Tidal Tomb|63.9|640|3,500|35 - 45|110|25|25|?|Melee|Medium|1.35
Organgrinder|Aegis Keep|280|2,800|20,000|70 - 80|130|25|25|?|Melee|Medium|1.35
Blood Ogre Mage|Aegis Keep|64.1|641|4,500|40 - 50|80|25|100|?|75|15 - 25|MeleeMage|Slow|1.1
Deformed Troll|New Player Dungeon|8.9|90|800|15 - 25|45|25|25|?|Melee|Slow|1.14
Giant Toxic Zoa|Tidal Tomb|289.2|2,892|18,000|50 - 60|120|25|250|?|225|45 - 55|80%|Mage|Medium|1.14
Ogre Bonecrusher King|Time Dungeon|1199|11,992|90,000|80 - 90|120|150|75|50|Melee|VeryFast|1.5
Ratman Spirit Hunter|?|22.3|223|1,500|15 - 25|110|25|25|100|?|Deadly|50|40%|X|Melee|Medium|1.1
Cyclops|Wilderness|89.8|899|8,000|50 - 60|90|25|25|?|Melee|Medium|1
Icy Lurker|Cavernam|118.3|1,183|8,000|40 - 50|100|50|25|?|Melee|Medium|1.4
Lurker|Mount Petram|122.7|1,227|8,000|50 - 60|90|50|25|?|Melee|Medium|1.35
Ophidian Dragonguard|Nusero|111.8|1,119|8,000|50 - 60|120|50|25|?|Deadly|50|60%|Melee|Medium|1
Ratman|Wilderness|20.2|202|2,000|15 - 25|80|25|25|?|Melee|Medium|1
Plague Ratman|?|10.1|102|1,000|10 - 20|90|25|25|?|40%|Melee|Medium|1
Troll King|Time Dungeon|418|4,181|26,000|30 - 40|90|125|125|50|150|30 - 40|50%|MeleeMage|Fast|1.5
Marshscale Hunter|Wilderness|127.7|1,278|10,000|40 - 50|110|50|25|?|Melee|Medium|1.3
Stonescale Tribal|Nusero|26.4|265|2,500|20 - 30|75|75|25|?|Melee|Medium|1
Gargan Flameshaper|Shadowspire Cathedral|179.9|1,800|12,000|55 - 65|120|50|25|?|Melee|Medium|1.35
Orc Mage|Wilderness|24.1|241|2,000|10 - 20|65|25|75|?|50|10 - 20|MeleeMage|Medium|1
Opilion Attendant|Cavernam|48.7|487|4,000|25 - 35|85|50|25|?|Melee|Medium|1.14
Ogre Bonecrusher|Time Dungeon|319.8|3,199|17,000|70 - 80|120|125|75|?|Melee|VeryFast|1.5
Gargan Stonekin|Shadowspire Cathedral|71.8|718|5,000|35 - 45|110|75|25|?|Ranged|Medium|1.14
Outdrider Broodwitch|Kraul Hive|181.5|1,816|8,000|40 - 50|120|25|200|?|175|35 - 45|Mage|Fast|1.3
Crippled Ettin|New Player Dungeon|12.6|127|1,250|20 - 30|50|25|25|?|Melee|Slow|1
Lord Bile|Darkmire Temple|1263|100,000|200,000|50 - 60|90|50|25|?|80%|Melee|Slow|2
Mountain Tribe Hunter|Time Dungeon|178.4|1,784|18,000|50 - 60|110|100|50|80|Ranged|VeryFast|1.5
Forktongue Spinebreaker|Nusero|118.9|1,189|8,000|40 - 50|100|50|25|?|Melee|Fast|1.35
Combustive Zoa|Tidal Tomb|179.5|1,795|10,000|40 - 50|110|25|200|?|175|35 - 45|Mage|Medium|1.14
Exothrrug|Time Dungeon|3702|37,020|150,000|60 - 70|120|200|200|75|60|Lethal|100|95%|X|Melee|SuperFast|2.29
Headless Fred's Head|Time Dungeon|2215|22,151|150,000|40 - 50|120|0|300|70|275|55 - 65|MeleeMage|VeryFast|1.4
Terathan Exile Drone|Kraul Hive|52.1|522|3,500|30 - 40|90|50|25|?|Melee|Medium|1.25
River Tribe Shaman|Time Dungeon|210.1|2,102|12,000|50 - 60|120|50|200|?|175|35 - 45|Greater|20|75%|MeleeMage|VeryFast|1.5
Anubite|Ossuary|177.7|1,778|10,000|50 - 60|100|50|25|?|Melee|Fast|1.6
Stone Harpy|Mount Petram|21.8|219|2,000|20 - 30|75|75|25|?|Melee|Medium|1
Mesmer|Time Dungeon|513.8|5,138|19,000|50 - 60|120|125|75|50|250|50 - 60|33%|MeleeMage|VeryFast|1.45
Lizardman|Wilderness|26.2|263|2,500|20 - 30|75|50|25|?|Melee|Medium|1
Ogre Lord|Wilderness|72.5|726|6,000|50 - 60|90|25|25|?|Melee|Slow|1
Hungry Prevalian|Time Dungeon|620.9|6,210|20,000|70 - 80|120|50|200|70|50%|Melee|SuperFast|1.65
Frail Orc|New Player Dungeon|3.8|38|400|10 - 20|40|25|25|?|Melee|Medium|1
Blood Sorcerer|Aegis Keep|110|1,101|6,000|20 - 30|90|25|150|?|125|25 - 35|Mage|Slow|1.14
Ogre Mage|Wilderness|68.8|688|5,000|40 - 50|80|25|125|?|100|20 - 30|MeleeMage|Slow|1
Kobold|Aegis Keep|26.2|263|2,000|15 - 25|80|25|25|?|Melee|Medium|1.3
Headless Fred|Time Dungeon|1995|19,948|140,000|70 - 80|120|300|0|70|Melee|SuperFast|1.5
Blood Harpy|Aegis Keep|48.6|486|3,500|20 - 30|80|25|25|?|Melee|Medium|1.4
Goblin|Aegis Keep|16.9|169|1,500|10 - 20|70|25|25|?|Melee|Medium|1.2
Orc Reaver Captain|Ocean|66.1|661|9,000|65 - 75|115|50|25|?|Melee|Medium|1
The Mountain Tribe Bozz|Time Dungeon|1155|11,547|80,000|70 - 80|110|200|75|70|Melee|SuperFast|1.4
Cyclopean Fire Warrior|Wilderness|112.3|1,124|8,000|50 - 60|90|25|25|?|Melee|Medium|1.25
Opilion Conservator|Cavernam|82.6|826|5,000|30 - 40|90|50|125|?|100|20 - 30|MeleeMage|Medium|1.2
Ettin Stoneguard|Wilderness|68.8|689|6,000|45 - 55|85|75|25|?|Melee|Slow|1
Minotaur Battleborne|Mount Petram|141.4|1,415|8,000|50 - 60|120|75|25|?|Melee|Fast|1.3
Gargan Scorcher|Shadowspire Cathedral|177.2|1,772|8,000|40 - 50|110|75|25|?|X|Melee|Medium|2
Blood Cyclops|Aegis Keep|111.7|1,118|8,000|50 - 60|80|25|25|?|Melee|Medium|1.3
Gargan Flamekin|Shadowspire Cathedral|79.2|793|5,000|40 - 50|110|50|25|?|Ranged|Medium|1.2
Minotaur|Mount Petram|84.6|847|6,000|40 - 50|100|25|25|?|Melee|Fast|1.14
Tidal Warrior|?|81.9|820|6,000|35 - 45|105|50|25|?|Melee|Medium|1.2
Forktongue Shocktrooper|Nusero|129|1,290|8,000|50 - 60|100|50|25|?|Melee|Fast|1.3
Gremlin|Aegis Keep|15.4|155|1,500|10 - 20|70|25|25|?|Melee|Medium|1.1
The Sovereign|Time Dungeon|8392|83,918|300,000|70 - 80|150|100|400|50|60|375|75 - 85|95%|MeleeMage|SuperFast|2
Sunken Tentacle|?|46.9|469|4,000|20 - 30|130|25|25|?|Melee|Slow|1.1
Ettin Lord|Time Dungeon|434.4|4,345|24,000|60 - 70|110|125|75|60|Melee|VeryFast|1.45
Mind Flayer|Undermountain|109.7|1,097|6,000|30 - 40|110|25|175|?|150|30 - 40|MeleeMage|Slow|1.2
Terathan Broodguard|Mount Petram|139.6|1,397|8,000|50 - 60|130|50|25|?|Deadly|50|60%|Melee|Medium|1.2
Feeble Ratman|New Player Dungeon|3.8|38|400|10 - 20|40|25|25|?|Melee|Medium|1
Bloody Dracolisk|Aegis Keep|298.8|2,988|20,000|80 - 90|130|50|25|?|Melee|Medium|1.3
Seducesa|Wilderness|1132|100,000|250,000|30 - 40|110|50|100|?|Melee|Fast|1.6
Arachnai Hivematron|Darkmire Temple|184.4|1,845|12,000|60 - 70|140|50|200|?|175|35 - 45|MeleeMage|Medium|1.1
Lizardman Elite Warrior|Wilderness|81.9|820|6,000|35 - 45|105|50|25|?|Melee|Medium|1.2
Terathan Drone|Mount Petram|25.8|258|2,000|15 - 25|70|50|25|?|Melee|Medium|1.3
River Tribe Oracle|Time Dungeon|185.5|1,856|11,000|40 - 50|100|25|150|40|Deadly|25|75%|Melee|VeryFast|1.45
Mountain Harpy|Mount Petram|25.5|256|2,000|15 - 25|70|25|25|?|Melee|Medium|1.3
Earth Sorcerer|Darkmire Temple|126.2|1,263|8,000|20 - 30|100|25|175|?|150|30 - 40|Mage|Slow|1
River Tribe Priest|Time Dungeon|214.9|2,149|12,000|20 - 30|90|25|175|40|150|30 - 40|Greater|25|75%|MeleeMage|Fast|1.5
Terathan Exile Warrior|Kraul Hive|102.5|1,025|6,000|45 - 55|110|50|25|?|Melee|Medium|1.3
Broodbearer|Wilderness|572.4|25,000|75,000|30 - 40|120|50|150|?|125|25 - 35|Lethal|25|80%|MeleeMage|Medium|1.6
Injured Ogre|New Player Dungeon|10.2|102|1,000|20 - 30|45|25|25|?|Melee|Slow|1
Lizardman Elite Hunter|Wilderness|72.5|726|5,000|30 - 40|100|50|25|?|Melee|Medium|1.3
Harpy|Wilderness|19.9|199|2,000|15 - 25|75|25|25|?|Melee|Medium|1
Kobold Trapper|Time Dungeon|165|1,651|14,000|50 - 60|90|50|50|?|25|Melee|VeryFast|1.25
Mountain Tribe Scout|Time Dungeon|180.2|1,802|15,000|55 - 65|120|100|25|70|Deadly|20|25%|Melee|SuperFast|1.5
Hunter Tentacle|?|6.9|70|500|10 - 20|110|25|25|?|20%|Melee|Medium|1.25
Ocean's Fury|?|1432|100,000|200,000|50 - 60|110|50|25|?|Melee|Medium|2
The Ravenous|Time Dungeon|1575|15,746|100,000|80 - 90|130|75|250|70|50%|Melee|SuperFast|1.35
Bloodrat|Aegis Keep|39.2|392|3,000|25 - 35|90|25|25|?|Melee|Medium|1.14
Forktongue Battlekhan|Nusero|212.3|2,123|12,000|60 - 80|120|50|25|?|Melee|VeryFast|1.3
Yeti|?|72.7|727|6,000|40 - 50|110|25|25|?|Melee|Fast|0.95
Tundra Shaman|?|69.6|697|5,000|30 - 40|100|25|150|?|100|20 - 30|MeleeMage|Fast|1
Slithercreep|Wilderness|1086|100,000|250,000|35 - 45|90|50|100|?|Melee|Medium|1.6
Tidal Dracolisk|?|110|1,100|7,000|50 - 60|110|50|25|?|Melee|Medium|1.2
Terror Tentacle|?|70.8|?|6,000|15 - 25|150|0|25|50|60|60%|Melee|Fast|1
Siren|?|13.2|132|1,000|15 - 25|130|25|200|?|60%|Melee|VeryFast|1
Ratman Spirit Warrior|?|18.9|189|1,500|20 - 30|100|25|25|100|?|40%|Melee|Medium|1
Terathan Goliath|Mount Petram|1390|100,000|200,000|50 - 60|100|75|25|?|40%|Melee|Medium|2
Cyclopean Tyrant|Time Dungeon|316.1|3,161|20,000|70 - 80|100|125|75|?|Melee|VeryFast|1.5
Ratman Spirit Mage|?|30.4|305|1,500|10 - 20|90|25|125|100|?|100|20 - 30|40%|Mage|Medium|1
Murkvine|?|41.7|?|5,000|20 - 30|150|25|25|?|Melee|Slow|1
Maw Tentacle|?|52.3|523|4,000|20 - 30|110|25|25|?|Melee|Medium|1.25
Ratman Warrior|Time Dungeon|412|4,121|24,000|75 - 85|120|100|50|75|?|40%|Melee|VeryFast|1.5
Headless|?|3.4|34|400|5 - 10|30|25|25|?|Melee|Medium|1
Gripping Tentacle|?|31.3|?|5,000|15 - 25|150|25|25|60|Melee|Slow|0.7
Cave Elderogre|Cavernam|213|2,131|16,000|70 - 80|120|25|25|?|Melee|Medium|1.2
Kobold Engineer|Winterlands|73.5|736|4,000|30 - 40|100|25|25|?|Melee|Fast|1.5
Mermadon|Tidal Tomb|61.5|615|4,000|40 - 50|110|25|25|?|Melee|Medium|1.1
Weald Rat Shaman|Wilderness|41.4|415|2,500|10 - 20|80|25|75|45|50|10 - 20|Mage|Medium|1
Orc Reaver|Ocean|37|371|4,500|40 - 50|90|25|25|?|Melee|Medium|1
Troll Shaman|Wilderness|67.4|675|4,000|35 - 45|90|25|125|?|100|20 - 30|MeleeMage|Slow|1.14
Troll|Wilderness|44.2|442|3,500|30 - 40|80|25|25|?|Melee|Slow|1.14
Ratman Witchdoctor|?|1617|100,000|250,000|25 - 35|80|50|200|?|125|25 - 35|Mage|Slow|1.6
The Sleeper|Time Dungeon|2653|26,527|225,000|70 - 80|100|75|275|60|Deadly|33|75%|Melee|SuperFast|1.55
Orc|Wilderness|15.2|152|1,500|15 - 25|70|25|25|?|Melee|Medium|1
Ogre|Wilderness|46.8|468|4,000|40 - 50|75|25|25|?|Melee|Slow|1
Marshscale Warrior|Wilderness|151.3|1,513|12,000|50 - 60|120|50|25|?|Melee|Medium|1.2
Meat Thing|Time Dungeon|3637|36,367|225,000|80 - 100|130|100|350|70|95%|Melee|SuperFast|1.5
Minotaur War Shaman|Wilderness|115.2|1,152|8,000|50 - 60|120|50|125|?|100|20 - 30|MeleeMage|Fast|1.1
Minotaur Vanquisher|Wilderness|173|1,731|12,000|50 - 60|110|75|25|?|Melee|Fast|1.35
Minotaur Conqueror|Wilderness|195.5|1,956|14,000|60 - 80|120|75|25|?|Melee|Fast|1.14
Terathan Warrior|Mount Petram|70|701|5,000|30 - 40|90|50|25|?|Melee|Medium|1.3
Marshscale Warchief|Wilderness|234.2|2,343|16,000|70 - 80|130|50|25|?|Melee|Medium|1.25
Lizardman Warchief|Wilderness|128.4|1,285|8,000|50 - 60|120|50|25|?|Melee|Medium|1.25
Lizardman Shaman|Wilderness|45.6|457|3,000|10 - 20|75|50|100|?|75|15 - 25|Mage|Medium|1
Ratman Wizard|Time Dungeon|478.3|4,784|15,000|25 - 35|90|25|150|50|50|250|50 - 60|60%|Mage|Fast|1.25
Toxic Zoa|Tidal Tomb|180.1|1,801|10,000|40 - 50|110|25|200|?|175|35 - 45|60%|Mage|Medium|1.14
Strongarm Bill|Time Dungeon|3001|30,012|120,000|100 - 150|200|75|100|100|50|50%|Melee|SuperFast|1.3
Ettin|Wilderness|54.6|547|5,000|40 - 50|75|25|25|?|Melee|Slow|1
Terathan Exile Matron|Kraul Hive|132.1|1,322|8,000|50 - 60|120|50|175|?|150|30 - 40|MeleeMage|Medium|1.1
Minotaur Reaver|Wilderness|137.3|1,374|8,000|40 - 50|110|50|25|?|Melee|Fast|1.5
Horrific Lurker|?|36.7|367|2,500|25 - 35|100|50|25|?|Melee|Medium|1.2
Icy Dracolisk|Cavernam|128.4|1,285|8,000|50 - 60|110|50|25|?|Melee|Medium|1.3
Weald Rat|Darkmire Temple|22.1|222|2,000|15 - 25|80|25|25|45|Melee|Medium|1
Desertwing|Wilderness|88.3|883|6,000|40 - 50|100|25|25|?|Melee|Medium|1.25
Arachnai Soldier|Darkmire Temple|84.6|847|6,000|40 - 50|120|50|25|?|Melee|Medium|1.1
Bridge Troll|Wilderness|90.4|905|6,000|50 - 60|110|25|25|?|Melee|Slow|1.14
Behemoth Basilisk|Cavernam|1288|100,000|200,000|40 - 50|110|75|25|?|60%|Melee|Medium|2
Drider Scourge|Undermountain|184.4|1,844|12,000|50 - 60|120|25|175|?|150|30 - 40|MeleeMage|Fast|1.25
Tidal Warchief|?|128.4|1,285|8,000|50 - 60|120|50|25|?|Melee|Medium|1.25
Outdrider Broodguard|Kraul Hive|170.1|1,702|10,000|60 - 70|130|25|175|?|150|30 - 40|MeleeMage|Fast|1.2
Sunken Sorcerer|Pulma|143.8|1,439|6,000|10 - 20|100|25|150|?|125|25 - 35|Mage|Slow|1.5
Warped Prevalian|Time Dungeon|593.1|5,931|20,000|80 - 90|120|100|50|50|50|25%|Melee|SuperFast|1.6
Cave Dracolisk|Nusero|114.2|1,143|8,000|50 - 60|100|50|50|?|Melee|Medium|1.2
Time Lurker|Time Dungeon|580.1|5,802|26,000|50 - 60|110|75|150|25|40|225|45 - 55|75%|X|MeleeMage|SuperFast|1.7
Ratman Rogue|Time Dungeon|467.2|4,673|15,000|60 - 70|120|25|50|75|60|Deadly|50|75%|X|Melee|SuperFast|1.55
Ratman Chief|Time Dungeon|4965|49,648|250,000|60 - 80|80|75|225|60|300|60 - 70|33%|Mage|VeryFast|1.5
Pack Goblin|Time Dungeon|17.1|171|80,000|5 - 10|100|50|100|?|Ranged|SuperFast|0.1
Tidal Hunter|?|72.5|726|5,000|30 - 40|100|50|25|?|Melee|Medium|1.3
Mountain Tribe Warrior|Time Dungeon|190.9|1,909|20,000|60 - 70|110|125|25|60|Melee|VeryFast|1.5
Arachnai Ravager|Darkmire Temple|117.4|1,175|8,000|50 - 60|130|50|25|?|Melee|Medium|1.1
Kobold Thief|Time Dungeon|185.4|1,855|16,000|30 - 40|100|50|75|?|Lesser|25|50%|X|Melee|SuperFast|1.5
Invisible Stalker|Time Dungeon|307.7|3,078|12,000|60 - 70|140|50|225|?|Deadly|10|33%|X|Melee|SuperFast|1.6
Witch Harpy|Mount Petram|110.3|1,103|6,000|30 - 40|100|25|150|?|125|25 - 35|Mage|Medium|1.14
Strangehell Lashweed|?|27.4|275|1,500|15 - 25|110|25|100|?|25|20%|Ranged|Fast|1.5
Ophidian Mage|Nusero|71.3|713|4,500|20 - 30|90|50|125|?|100|20 - 30|Mage|Medium|1
Wendigo|Cavernam|117.7|1,177|8,000|50 - 60|100|25|25|25|?|X|Melee|Slow|1.3
Orc Lord|Wilderness|39.9|400|3,000|25 - 35|80|50|25|?|Melee|Medium|1.2
Ophidian Matron|Nusero|145|1,450|9,000|60 - 70|120|50|200|?|175|35 - 45|Deadly|50|60%|MeleeMage|Medium|1
The Insatiable Maw|?|529.6|50,000|100,000|30 - 40|90|25|25|?|Melee|Medium|1.8
Giant Combustive Zoa|Tidal Tomb|287.9|2,879|18,000|50 - 60|120|25|250|?|225|45 - 55|Mage|Medium|1.14
Giant Astral Zoa|Tidal Tomb|287.9|2,879|18,000|50 - 60|120|25|250|?|225|45 - 55|Mage|Medium|1.14
Astral Zoa|Tidal Tomb|179.5|1,795|10,000|40 - 50|110|25|200|?|175|35 - 45|Mage|Medium|1.14
Writhing Tentacle|Pulma|15.8|158|1,500|10 - 20|90|25|25|?|Melee|Slow|1.1
Orc Captain|Wilderness|21.6|217|2,000|20 - 30|75|50|25|?|Melee|Medium|1
Gargan Stoneshaper|Shadowspire Cathedral|165.5|1,656|12,000|50 - 60|120|75|25|?|Melee|Medium|1.3
Flesh Wolf|Time Dungeon|489|4,891|20,000|70 - 80|110|25|100|60|25%|Melee|SuperFast|1.5
Troll Thrall|Time Dungeon|361.1|3,611|20,000|60 - 70|100|100|75|60|Melee|VeryFast|1.4
Drider Websinger|Undermountain|195.1|1,951|10,000|10 - 20|110|25|200|?|175|35 - 45|Mage|Fast|1.25
Drowned Lurker|Pulma|107.1|1,072|6,000|40 - 50|100|50|25|?|Melee|Medium|1.5
Blood Orc|Aegis Keep|42.3|423|3,000|30 - 40|80|25|25|?|Melee|Medium|1.2
Befuddler|Pulma|147|1,470|10,000|50 - 60|100|25|25|?|Melee|Medium|1.4
The River Tribe Bozz|Time Dungeon|806.5|8,066|65,000|50 - 60|100|75|250|50|250|50 - 60|Lethal|50|75%|MeleeMage|SuperFast|1.3
Ophidian Shaman|Nusero|118.7|1,188|7,000|30 - 40|100|50|175|?|150|30 - 40|Mage|Medium|1
Mechanical Minion|Winterlands|146.9|1,469|14,000|70 - 80|100|75|25|?|Melee|Slow|1
Wounded Harpy|New Player Dungeon|3.8|38|400|10 - 20|40|25|25|?|Melee|Medium|1
Sickly Lizardman|New Player Dungeon|6.1|62|600|15 - 25|45|50|25|?|Melee|Medium|1
Blood Troll|Aegis Keep|79.6|797|5,000|40 - 50|85|25|25|?|Melee|Slow|1.4
Blind Orc Mage|New Player Dungeon|4.6|47|400|10 - 20|40|25|50|?|25|5 - 15|MeleeMage|Medium|1
Terathan Matron|Mount Petram|82.4|824|6,000|40 - 50|110|50|125|?|100|20 - 30|MeleeMage|Medium|1
Terathan Broodwitch|Mount Petram|119.4|1,195|8,000|40 - 50|120|50|175|?|150|30 - 40|Deadly|50|60%|MeleeMage|Medium|1
Winterwing|Cavernam|136|1,360|10,000|40 - 50|120|25|25|?|Melee|Medium|1.35
Nagalid|Cavernam|56.3|564|3,500|25 - 35|120|50|25|45|Melee|Medium|1.14
# Raw: Nature section (agent: list-nature) — 167 rows, AUTHORITATIVE (raw Lua module extraction)

SOURCE DISCOVERY: table is generated by Module:Creatures from **Module:WildCreatureData** (raw Lua, 1,288 total creature entries, 8 slayer groups: Beastial, Construct, Daemonic, Elemental, Humanoid, Monstrous, Nature, Undead). Fetch via action=raw gives verbatim values — no summarizer drift. Field-by-field verified vs sample row.

Column order: Name|Location|Slayer|Difficulty|Gold|Hits|MeleeDmg|Wrestling|Armor|MagicResist|Parry|AtkSpd|Magery|SpellDmg|Poison|Poisoning|PoisonResist|Stealth|AI|Speed|UniqueScaler
('-' = blank, '?' verbatim, 'X' verbatim boolean)

Hivefly Crewman|Ocean|Nature|95.3|954|6000|40-50|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Hivemind Crewman|Ocean|Nature|102.3|1024|8000|40-50|100|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.2
Insatiable Slug|?|Nature|27.2|272|2500|15-25|60|25|25|-|-|-|-|-|-|-|-|Ranged|Slow|1.2
Foulglow Kraul Hivemother|?|Nature|684.3|50000|100000|40-50|110|50|25|-|-|-|-|-|-|-|-|Melee|Fast|1.8
Strangehell Deathstalk|?|Nature|47.1|472|2500|25-35|130|25|200|-|-|-|-|-|-|0.2|-|Melee|VeryFast|1.3
Strangehell Spikebush|?|Nature|40.7|408|2500|15-25|100|50|200|-|-|-|-|-|-|0.2|-|Melee|Fast|1.5
Strangehell Vines|?|Nature|25.6|256|1500|15-25|120|25|100|-|-|-|-|Deadly|50|0.4|-|Melee|VeryFast|1.2
Giant Slug|?|Nature|6.5|65|600|10-20|50|25|25|-|-|-|-|-|-|0.4|-|Melee|Slow|1.14
The Terrorwood|Darkmire Temple|Nature|536.2|50000|100000|30-40|90|50|25|-|-|-|-|-|-|0.4|-|Melee|Medium|1.8
Kraul Hivemother|Kraul Hive|Nature|684.3|50000|100000|40-50|110|50|25|-|-|-|-|-|-|-|-|Melee|Fast|1.8
Aegis Leech|Aegis Keep|Nature|39.7|397|3000|20-30|75|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.35
Bloodworm|Aegis Keep|Nature|104|1041|8000|40-50|90|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.35
Clay Man|Aegis Keep|Nature|45.8|458|4000|25-35|80|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.1
Doppelganger|Aegis Keep|Nature|96.5|965|6000|40-50|130|25|150|-|-|125|25-35|-|-|-|-|MeleeMage|Fast|1.05
Frostbark|Cavernam|Nature|236.1|2361|16000|50-60|110|75|225|-|-|200|40-50|-|-|0.8|-|Mage|SuperSlow|1.3
Glacial Creep|Cavernam|Nature|32.9|330|2500|20-30|70|75|25|-|-|-|-|-|-|-|-|Melee|Slow|1.3
Walking Avalanche|Cavernam|Nature|161|1611|12000|60-70|100|75|25|-|-|-|-|-|-|-|-|Melee|Slow|1.3
Winterweed|Cavernam|Nature|83|831|5000|30-40|100|25|25|-|-|-|-|-|-|0.6|-|Melee|Medium|1.5
Arboreal Wisp|Darkmire Temple|Nature|138.1|1381|6000|30-40|130|25|200|25|-|175|35-45|-|-|-|-|Mage|Medium|1.14
Colossal Swamp Slug|Darkmire Temple|Nature|65.8|658|6000|30-40|75|25|25|-|-|-|-|-|-|-|-|Ranged|Slow|1.2
Creeping Earth|Darkmire Temple|Nature|67.9|679|5000|30-40|100|50|25|25|-|-|-|-|-|-|X|Melee|Slow|1.25
Creeping Soil|Darkmire Temple|Nature|26.6|266|2000|20-30|75|50|25|25|-|-|-|-|-|-|X|Melee|Slow|1.25
Dryad|Darkmire Temple|Nature|31.6|316|2000|15-25|90|25|75|-|-|50|10-20|-|-|-|-|Mage|Medium|1.14
Faery|Darkmire Temple|Nature|34.4|344|2500|15-25|130|25|75|-|-|-|-|-|-|-|-|Ranged|Fast|1.2
Fey Spirit|Darkmire Temple|Nature|52|520|3000|15-25|90|25|100|-|-|75|15-25|-|-|-|-|Mage|Medium|1.14
Fey Spirit Matron|Darkmire Temple|Nature|71.3|714|4000|20-30|100|25|125|-|-|100|20-30|-|-|-|-|Mage|Medium|1.1
Giant Swamp Slug|Darkmire Temple|Nature|27.2|272|2500|15-25|60|25|25|-|-|-|-|-|-|-|-|Ranged|Slow|1.2
Myconid|Darkmire Temple|Nature|99|990|8000|30-40|80|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.5
Otyugh|Darkmire Temple|Nature|139.3|1393|8000|30-40|90|75|25|-|-|-|-|-|-|-|-|Melee|Slow|2
Sentient Vines|Darkmire Temple|Nature|46|461|4000|20-30|110|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.1
Spirit Bear|Darkmire Temple|Nature|41.3|-|4000|25-35|80|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1
Spirit Sabertusk|Darkmire Temple|Nature|54.3|-|5000|30-40|85|0|25|50|-|-|-|-|-|-|-|Melee|Fast|1
Spirit Wolf|Darkmire Temple|Nature|34|-|3000|15-25|90|0|100|25|45|-|-|-|-|-|-|Melee|VeryFast|1
Stranglevines|Darkmire Temple|Nature|21.4|214|2000|15-25|75|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.1
Wildwood Guardian|Darkmire Temple|Nature|170.3|1703|12000|70-80|110|75|25|-|-|-|-|-|-|-|-|Melee|Slow|1.2
Wildwood Reaper|Darkmire Temple|Nature|197|1970|12000|50-60|90|75|175|-|-|150|30-40|-|-|0.8|-|Mage|SuperSlow|1.5
Wildwood Scourge|Darkmire Temple|Nature|159|1591|10000|60-70|90|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Blightspot|Kraul Hive|Nature|61.5|615|3500|30-40|80|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.6
Bogyugh|Kraul Hive|Nature|233.2|2332|12000|50-60|110|75|25|-|-|-|-|-|-|-|-|Melee|Slow|2
Bramblefolk|Kraul Hive|Nature|63.1|632|3000|20-30|90|25|75|-|-|75|15-25|-|-|-|-|Mage|Medium|1.4
Brasshopper|Kraul Hive|Nature|71.7|718|4000|35-45|100|50|25|-|-|-|-|-|-|-|-|Melee|Fast|1.35
Camomeal|Kraul Hive|Nature|112.5|1125|6000|40-50|110|25|25|-|-|-|-|-|-|0.4|X|Melee|Slow|1.6
Colossal Crystal Beetle|Kraul Hive|Nature|157.2|1572|10000|70-80|120|75|100|-|-|-|-|-|-|-|-|Melee|Slow|1.14
Colossal Swamp Beetle|Kraul Hive|Nature|148.6|1487|10000|70-80|120|75|25|-|-|-|-|-|50|-|-|Melee|Slow|1.1
Corrosive Hivedrone|Kraul Hive|Nature|59.7|597|4000|30-40|100|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.25
Corrosive Hivefly|Kraul Hive|Nature|77.3|773|5000|30-40|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Corrosive Hiveguard|Kraul Hive|Nature|257.7|2578|18000|70-80|120|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Corrosive Hivelarva|Kraul Hive|Nature|31.2|313|2500|20-30|90|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.14
Corrosive Hivemind|Kraul Hive|Nature|136.9|1370|8000|55-65|110|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.3
Corrosive Hivereaver|Kraul Hive|Nature|234.6|2347|16000|60-70|130|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Corrosive Hivestinger|Kraul Hive|Nature|109.1|1092|6000|45-55|110|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Corrosive Hivewarden|Kraul Hive|Nature|245.7|2458|18000|60-70|130|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Corrosive Hivewarrior|Kraul Hive|Nature|146.7|1467|8000|55-65|120|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Foulglow Hivedrone|Kraul Hive|Nature|59.7|597|4000|30-40|100|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.25
Foulglow Hivefly|Kraul Hive|Nature|77.3|773|5000|30-40|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Foulglow Hiveguard|Kraul Hive|Nature|257.7|2578|18000|70-80|120|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Foulglow Hivelarva|Kraul Hive|Nature|31.2|313|2500|20-30|90|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.14
Foulglow Hivemind|Kraul Hive|Nature|136.9|1370|8000|55-65|110|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.3
Foulglow Hivereaver|Kraul Hive|Nature|234.6|2347|16000|60-70|130|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Foulglow Hivestinger|Kraul Hive|Nature|109.1|1092|6000|45-55|110|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Foulglow Hivewarden|Kraul Hive|Nature|245.7|2458|18000|60-70|130|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Foulglow Hivewarrior|Kraul Hive|Nature|146.7|1467|8000|55-65|120|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Giant Crystal Beetle|Kraul Hive|Nature|89.7|898|6000|50-60|100|75|100|-|-|-|-|-|-|-|-|Melee|Slow|1.14
Giant Locust|Kraul Hive|Nature|57.3|574|3500|30-40|110|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.25
Giant Swamp Beetle|Kraul Hive|Nature|89.3|894|6000|50-60|100|75|25|-|-|-|-|Greater|50|0.4|-|Melee|Slow|1.1
Glowworm|Kraul Hive|Nature|59.1|591|4000|30-40|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.25
Goop|Kraul Hive|Nature|73.4|735|5000|40-50|90|25|25|25|-|-|-|-|-|-|-|Melee|Medium|1.2
Green Capper|Kraul Hive|Nature|96.1|962|6000|30-40|90|25|25|-|-|-|-|-|-|0.6|-|Melee|Medium|1.6
Siltsifter Hivedrone|Kraul Hive|Nature|59.7|597|4000|30-40|100|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.25
Siltsifter Hivefly|Kraul Hive|Nature|77.3|773|5000|30-40|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Siltsifter Hiveguard|Kraul Hive|Nature|257.7|2578|18000|70-80|120|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Siltsifter Hivelarva|Kraul Hive|Nature|31.2|313|2500|20-30|90|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.14
Siltsifter Hivemind|Kraul Hive|Nature|136.9|1370|8000|55-65|110|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.3
Siltsifter Hivereaver|Kraul Hive|Nature|234.6|2347|16000|60-70|130|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Siltsifter Hivestinger|Kraul Hive|Nature|109.1|1092|6000|45-55|110|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Siltsifter Hivewarden|Kraul Hive|Nature|245.7|2458|18000|60-70|130|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Siltsifter Hivewarrior|Kraul Hive|Nature|146.7|1467|8000|55-65|120|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Spelltouched Hivedrone|Kraul Hive|Nature|71.3|714|4000|30-40|100|50|100|-|-|100|20-30|-|-|-|-|MeleeMage|Medium|1.2
Spelltouched Hivefly|Kraul Hive|Nature|92.6|927|5000|30-40|100|25|100|-|-|100|20-30|-|-|-|-|MeleeMage|Medium|1.35
Spelltouched Hiveguard|Kraul Hive|Nature|269.5|2696|18000|70-80|120|50|200|-|-|175|35-45|-|-|-|-|MeleeMage|Medium|1.3
Spelltouched Hivelarva|Kraul Hive|Nature|38.2|383|2500|20-30|90|25|75|-|-|75|15-25|-|-|-|-|MeleeMage|Medium|1.1
Spelltouched Hivemind|Kraul Hive|Nature|152.3|1524|8000|55-65|110|75|175|-|-|150|30-40|-|-|-|-|MeleeMage|Medium|1.25
Spelltouched Hivereaver|Kraul Hive|Nature|255.3|2554|16000|60-70|130|25|200|-|-|175|35-45|-|-|-|-|MeleeMage|Medium|1.35
Spelltouched Hivestinger|Kraul Hive|Nature|121.4|1214|6000|45-55|110|25|125|-|-|125|25-35|-|-|-|-|MeleeMage|Medium|1.35
Spelltouched Hivewarden|Kraul Hive|Nature|268|2681|18000|60-70|130|75|200|-|-|175|35-45|-|-|-|-|MeleeMage|Medium|1.3
Spelltouched Hivewarrior|Kraul Hive|Nature|147.7|1477|8000|55-65|120|50|150|-|-|125|25-35|-|-|-|-|MeleeMage|Medium|1.3
Vinekin|Kraul Hive|Nature|69|690|3500|40-50|110|50|25|-|-|-|-|-|-|-|X|Melee|Medium|1.35
Deathstalk|Mount Petram|Nature|112.2|1123|8000|40-50|110|25|25|-|-|-|-|-|-|0.4|-|Melee|Slow|1.35
Fungaloid|Mount Petram|Nature|84|840|6000|25-35|90|25|25|-|-|-|-|-|-|0.4|-|Melee|Medium|1.5
Lichenid|Mount Petram|Nature|118.6|1186|8000|50-60|130|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.1
Moss Giant|Mount Petram|Nature|167.2|1673|12000|60-70|100|75|25|-|-|-|-|-|-|-|-|Melee|Slow|1.35
Mountain Vines|Mount Petram|Nature|20.3|204|2000|10-20|75|25|25|-|-|-|-|-|-|-|-|Melee|SuperSlow|1.2
Noxweed|Mount Petram|Nature|93.3|933|6000|30-40|100|25|25|-|-|-|-|-|-|0.6|-|Melee|Medium|1.5
Wicked Willow|Mount Petram|Nature|43.1|432|4000|20-30|60|75|75|-|-|50|10-20|-|-|0.8|-|Mage|SuperSlow|1
Army Ant|Nusero|Nature|16.7|167|1500|10-20|75|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.14
Colossal Sandroach|Ossuary|Nature|132.5|1326|8000|50-60|90|100|25|-|-|-|-|-|-|-|-|Melee|Slow|1.5
Creeping Pestilence|Ossuary|Nature|77.9|780|6000|30-40|90|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.35
Plague Of Locusts|Ossuary|Nature|105.4|1054|8000|30-40|130|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1.3
Sand Muck|Ossuary|Nature|56.8|569|4000|20-30|100|25|25|25|-|-|-|-|-|-|-|Melee|VeryFast|1.3
Bugman|Ossuary|Nature|300.1|3001|24000|70-80|120|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Sandshark|Ossuary|Nature|434.7|4348|20000|60-70|130|25|25|-|-|-|-|-|-|-|X|Melee|Fast|2.2
Corpse Flower|Shadowspire Cathedral|Nature|143.3|1434|9000|40-50|120|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.5
Deathcap|Shadowspire Cathedral|Nature|149.7|1497|10000|40-50|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.6
Deathvines|Shadowspire Cathedral|Nature|108.5|1085|12000|20-30|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Gravebug|Shadowspire Cathedral|Nature|151|1511|9000|40-50|100|75|25|-|-|-|-|-|-|-|-|Melee|Fast|1.6
Entozoon|The Mausoleum|Nature|195.4|1954|14000|60-70|120|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.3
Aquamarine Smallshell|Tidal Tomb|Nature|41.6|417|3000|25-35|90|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.2
Biolumen Shellpod|Tidal Tomb|Nature|112.1|1121|8000|35-45|100|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Clearwater Grouper|Tidal Tomb|Nature|152.6|1527|10000|60-70|100|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.25
Colossal Pincer Crab|Tidal Tomb|Nature|271.5|2716|18000|70-80|130|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Colossal Scourgeon|Tidal Tomb|Nature|261.9|2620|20000|70-80|120|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.25
Dungeon-Nest Crab|Tidal Tomb|Nature|53.5|536|4000|30-40|90|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.14
Flying Manta|Tidal Tomb|Nature|55.9|560|3500|35-45|95|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.2
Frenzyfin|Tidal Tomb|Nature|149.1|1491|8000|50-60|120|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.4
Giant Kelpdweller|Tidal Tomb|Nature|287.8|2878|16000|60-70|120|25|25|-|-|-|-|-|-|-|X|Melee|Fast|1.7
Giant Pincer Crab|Tidal Tomb|Nature|151|1511|10000|50-60|120|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.3
Greater Frenzyfin|Tidal Tomb|Nature|306.2|3062|20000|70-80|130|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.4
Greater Razorfin|Tidal Tomb|Nature|295.3|2953|20000|70-80|130|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.35
Greater Voidfin|Tidal Tomb|Nature|328.1|3281|20000|70-80|130|25|25|-|-|-|-|-|-|-|X|Melee|Fast|1.5
Ironscale Grouper|Tidal Tomb|Nature|143.4|1435|10000|60-70|100|75|25|-|-|-|-|-|-|-|-|Melee|Fast|1.14
Kelpdweller|Tidal Tomb|Nature|130.1|1302|6000|40-50|110|25|25|-|-|-|-|-|-|-|X|Melee|Fast|1.7
Lighthouse Shellpod|Tidal Tomb|Nature|112.1|1121|8000|35-45|100|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Oozing Noughtilus|Tidal Tomb|Nature|45.5|455|3000|25-35|90|50|25|50|-|-|-|Greater|50|0.4|-|Melee|Slow|1.25
Pincer Crab|Tidal Tomb|Nature|6.3|64|500|10-20|100|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.14
Razorfin|Tidal Tomb|Nature|143.7|1438|8000|50-60|120|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.35
Ruby Smallshell|Tidal Tomb|Nature|41.6|417|3000|25-35|90|75|25|-|-|-|-|-|-|-|-|Melee|Medium|1.2
Scourgeon|Tidal Tomb|Nature|127.8|1279|8000|50-60|110|25|25|-|-|-|-|-|-|-|-|Melee|Fast|1.25
Singing Noughtilus|Tidal Tomb|Nature|40.9|409|3000|25-35|90|50|25|50|-|-|-|-|-|-|-|Melee|Slow|1.2
Voidfin|Tidal Tomb|Nature|159.7|1598|8000|50-60|120|25|25|-|-|-|-|-|-|-|X|Melee|Fast|1.5
Greater Corpser|Time Dungeon|Nature|176|1761|12000|50-60|100|75|100|-|-|-|-|Greater|33|0.75|-|Melee|SuperFast|1.2
Greater Reaper|Time Dungeon|Nature|251|2511|14000|10-20|80|25|175|-|-|200|40-50|-|-|0.5|-|Mage|Fast|1.25
Gutworm|Time Dungeon|Nature|467.8|4678|22000|60-70|110|175|50|-|40|-|-|-|-|0.5|-|Melee|SuperFast|1.75
Gutworm Larva|Time Dungeon|Nature|51.5|-|6000|30-40|100|100|25|-|-|-|-|-|-|0.5|-|Melee|VeryFast|0.75
Living Waste|Time Dungeon|Nature|346.5|3466|15000|60-70|110|50|125|-|60|-|-|Greater|25|0.95|-|Melee|VeryFast|1.45
Noxweed (Time)|Time Dungeon|Nature|19.6|-|1200|15-25|90|25|25|-|-|-|-|-|-|0.6|-|Melee|Medium|1.5
Witherbark|Time Dungeon|Nature|969.4|9695|80000|20-40|100|50|200|-|50|250|50-60|-|-|0.5|-|Mage|SuperFast|1
Zorn|Time Dungeon|Nature|454.7|4548|24444|50-60|110|50|400|-|60|-|-|-|-|1|-|Melee|VeryFast|1.7
Mind Flayer Thrall|Undermountain|Nature|36.6|-|2500|30-40|120|25|150|-|-|-|-|-|-|-|-|Melee|Fast|1
Ancient Oak|Wilderness|Nature|131.2|1313|8000|60-70|100|75|25|-|-|-|-|-|-|-|-|Melee|Slow|1.3
Brambler|Wilderness|Nature|79.2|793|5000|40-50|110|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.2
Brushfyre|Wilderness|Nature|143.8|1439|10000|50-60|120|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.25
Cinderwood|Wilderness|Nature|102.7|1028|6000|40-50|100|75|150|-|-|125|25-35|-|-|0.8|-|Mage|SuperSlow|1.25
Corpser|Wilderness|Nature|12.2|122|1500|10-20|60|25|25|-|-|-|-|-|-|-|-|Melee|SuperSlow|1
Crude Oil|Wilderness|Nature|143.9|1440|10000|50-60|110|25|25|25|-|-|-|-|-|-|-|Melee|Medium|1.3
Daylights|Wilderness|Nature|132.1|1321|6000|30-40|130|25|200|25|-|175|35-45|-|-|-|-|Mage|Medium|1.1
Dust Devil|Wilderness|Nature|181.9|1819|12000|20-30|140|0|25|75|-|-|-|-|-|-|-|Melee|Fast|2
Fungle|Wilderness|Nature|81.3|813|5000|40-50|100|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.35
Fyrewood|Wilderness|Nature|169.4|1695|12000|60-80|110|75|25|-|-|-|-|-|-|-|-|Melee|Slow|1.25
Grasping Vines|Wilderness|Nature|38.5|385|3000|20-30|110|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.14
Hardwood|Wilderness|Nature|114.1|1142|8000|50-60|100|75|25|-|-|-|-|-|-|-|-|Melee|Slow|1.25
Murkshroom|Wilderness|Nature|99|991|8000|30-40|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Murkvines|Wilderness|Nature|119.5|1195|8000|20-40|150|25|25|-|-|-|-|-|-|-|-|Melee|SuperSlow|1.8
Naiad|Wilderness|Nature|57.1|572|3000|25-35|110|25|125|-|-|100|20-30|-|-|-|-|Mage|Medium|1.1
Naiad Matron|Wilderness|Nature|98.5|986|5000|25-35|120|25|150|-|-|125|25-35|-|-|-|-|Mage|Medium|1.14
Oasis Spirit Guard|Wilderness|Nature|110.3|1103|8000|30-40|100|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1.5
Oasis Spirit Keeper|Wilderness|Nature|195.5|1956|12000|50-60|110|50|25|50|-|-|-|-|-|-|-|Melee|Fast|1.5
Reaper|Wilderness|Nature|64.6|646|5000|20-30|70|50|125|-|-|100|20-30|-|-|0.8|-|Mage|SuperSlow|1
Spitroleum|Wilderness|Nature|116.2|1163|8000|30-40|120|0|25|25|-|-|-|-|-|-|-|Ranged|Medium|1.5
Sporier|Wilderness|Nature|70.9|709|4000|30-40|100|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.5
Swampgas|Wilderness|Nature|151|1510|8000|20-30|130|25|150|50|-|150|30-40|-|-|0.8|-|Mage|Medium|1.14
Tarbody|Wilderness|Nature|103.3|1033|8000|30-40|110|25|25|25|-|-|-|-|-|-|-|Ranged|Slow|1.4
Mirror Ice|Winterlands|Nature|85.8|859|6000|30-40|120|25|25|-|-|-|-|-|-|-|-|Melee|VeryFast|1.2
Changeling|-|Nature|28.8|288|1500|20-30|100|25|125|-|-|100|20-30|-|-|-|-|MeleeMage|Fast|1.14
Giant Sandroach|-|Nature|23.7|-|2000|20-30|60|100|25|-|-|-|-|-|-|-|-|Melee|Fast|1.1
Kelpie|-|Nature|63.2|632|4000|30-40|110|25|125|-|-|100|20-30|-|-|-|-|MeleeMage|Fast|1.05
Sylph|-|Nature|33.3|334|1500|15-25|130|25|250|-|-|100|20-30|-|-|0.4|-|Mage|Medium|1.14
Wildwood Avenger|-|Nature|1053|100000|250000|35-45|90|75|100|-|-|-|-|-|-|-|-|Melee|Slow|1.6
# Raw: Undead section (agent: list-nature) — 141 rows, AUTHORITATIVE (raw Lua module)

Column order: Name|Location|Slayer|Difficulty|Gold|Hits|MeleeDmg|Wrestling|Armor|MagicResist|Parry|AtkSpd|Magery|SpellDmg|Poison|Poisoning|PoisonResist|Stealth|AI|Speed|UniqueScaler
DATA FLAW (source-level, verified): Spectral Bishop minmeleedmg=1205/maxmeleedmg=30 in the wiki's own Lua data — wiki typo, preserved verbatim.

Drowned Dead|Ocean|Undead|18.5|186|1500|10-20|70|0|50|50|-|50|10-20|-|-|0.4|-|MeleeMage|VerySlow|1
Ghostly Captain|Ocean|Undead|64.3|644|8000|60-70|115|25|25|50|-|-|-|-|-|-|-|Melee|Medium|1.05
Ghostly Crewman|Ocean|Undead|32.6|326|4000|30-40|95|25|25|50|-|-|-|-|-|-|-|Melee|Medium|1
Lost Soul|Ocean|Undead|20.6|207|1500|15-25|80|0|75|-|-|50|10-20|-|-|0.4|-|MeleeMage|Slow|1.1
Echo Of A Lost Age|Wilderness|Undead|719|25000|75000|30-40|120|0|200|100|-|150|30-40|-|-|0.6|-|Mage|Slow|1.6
The Forgotten King|Ossuary|Undead|1387|100000|200000|50-60|100|75|25|-|-|-|-|-|-|-|-|Melee|Medium|2
Ancient Drowned Dragon|Pulma|Undead|535.2|50000|100000|30-40|90|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.8
Gatekeeper|The Mausoleum|Undead|1851|100000|200000|30-40|90|50|175|-|-|150|30-40|-|-|-|-|Mage|Medium|2
Marinerbane|Tidal Tomb|Undead|655.7|50000|100000|40-50|110|50|25|-|-|-|-|-|-|-|-|Melee|?|1.8
Balewight|Wilderness|Undead|1536|100000|250000|25-35|70|50|200|100|-|100|20-30|-|-|0.2|-|Mage|VerySlow|1.6
Lich Primarch|Wilderness|Undead|1619|100000|250000|25-35|75|50|200|-|-|125|25-35|-|-|0.4|X|Mage|Slow|1.6
Aegis Lich|Aegis Keep|Undead|67.9|679|4000|20-30|85|25|125|-|-|100|20-30|-|-|-|-|Mage|Medium|1.05
Decaying Dragon|Aegis Keep|Undead|376.9|3769|24000|80-90|130|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.5
Entombed|Aegis Keep|Undead|55.5|555|5000|25-35|75|25|25|-|-|-|-|-|-|-|-|Melee|VerySlow|1.25
Killer Dress|Aegis Keep|Undead|253.2|2533|14000|50-60|150|0|200|100|-|175|35-45|-|-|-|-|Mage|Fast|1.3
Malform|Aegis Keep|Undead|95.6|957|8000|40-50|100|25|25|-|-|-|-|-|-|-|-|Melee|VerySlow|1.25
Bonechiller|Cavernam|Undead|367.5|3676|22000|50-60|130|0|300|50|-|275|55-65|-|-|-|-|MeleeMage|Medium|1.35
Dead Of Winter|Cavernam|Undead|225.6|2256|12000|50-60|130|25|200|50|-|150|30-40|-|-|-|-|Mage|Fast|1.4
Frozen Dead|Cavernam|Undead|57.8|579|3000|10-20|60|0|75|50|-|50|10-20|-|-|-|-|Mage|VerySlow|1.5
Haunter|Cavernam|Undead|196.3|1964|10000|30-40|130|0|200|50|-|175|35-45|-|-|-|-|Mage|Medium|1.25
Ice Lich|Cavernam|Undead|254|2540|12000|40-50|120|25|250|-|-|225|45-55|-|-|-|-|Mage|Medium|1.25
Rime Court Wizard|Cavernam|Undead|264.8|2648|14000|30-40|120|0|225|50|-|200|40-50|-|-|0.4|-|Mage|Medium|1.3
Rime Jarl|Cavernam|Undead|228.8|2288|18000|70-80|150|0|200|100|-|-|-|-|-|0.4|-|Melee|Medium|2.25
Rime Royal Lancer|Cavernam|Undead|186.1|1861|16000|60-70|130|0|125|75|-|-|-|-|-|0.4|-|Melee|VeryFast|2
Rime Spirit|Cavernam|Undead|83.2|832|5000|20-30|80|0|125|50|-|100|20-30|-|-|-|-|Mage|Medium|1.1
Rime Spirit Champion|Cavernam|Undead|73|730|8000|50-60|130|0|25|100|-|-|-|-|-|-|-|Melee|Medium|1.25
Rime Spirit Knight|Cavernam|Undead|71.9|720|8000|60-70|120|0|25|100|-|-|-|-|-|-|-|Melee|VeryFast|1.1
Rime Spirit Skirmisher|Cavernam|Undead|55.1|551|6000|55-65|130|0|25|75|-|-|-|-|-|-|-|Melee|Medium|1.1
Rime Spirit Soldier|Cavernam|Undead|54.2|542|6000|50-60|120|0|25|100|-|-|-|-|-|-|-|Melee|Medium|1.1
Eerie Spirit|Field of Souls|Undead|18.1|182|6000|-1|0|0|0|-|-|-|-|-|-|0.6|-|Melee|Medium|0.6
Roaming Nightmare|Field of Souls|Undead|233.1|2331|18000|50-60|110|0|25|50|-|-|-|-|-|0.6|X|Melee|Fast|1.5
Roaming Terror|Field of Souls|Undead|171.8|1718|16000|30-40|130|0|25|50|60|-|-|-|-|0.6|X|Melee|Fast|1.1
Ghostly Dragonknight|Nusero|Undead|83.7|837|9000|50-60|130|0|25|100|-|-|-|-|-|-|-|Melee|Medium|1.35
Ghostly Dragontamer|Nusero|Undead|148.5|1486|8000|30-40|110|0|175|75|-|150|30-40|-|-|-|-|Mage|Medium|1.14
Apophite Spirit|Ossuary|Undead|137.9|1379|7000|50-60|130|0|25|50|-|-|-|-|-|0.8|-|Melee|Medium|1.4
Blightwalker|Ossuary|Undead|122.1|1222|10000|40-50|90|50|25|-|-|-|-|-|-|-|-|Melee|Slow|1.4
Desiccated Husk|Ossuary|Undead|74.9|749|5000|40-50|85|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.25
Gaunt Form|Ossuary|Undead|61.7|618|5000|30-40|90|25|100|-|-|75|15-25|-|-|-|-|MeleeMage|Medium|1
Ghoul|Ossuary|Undead|11.1|112|1000|10-20|50|25|50|-|-|25|5-15|-|-|-|-|MeleeMage|Medium|1
Jackal Spirit|Ossuary|Undead|117.2|1173|8000|40-50|100|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1.4
Lich|Ossuary|Undead|45.2|453|3000|10-20|90|25|100|-|-|75|15-25|-|-|-|-|Mage|Medium|1
Lich Magus|Ossuary|Undead|84.8|849|6000|10-20|85|25|125|-|-|100|20-30|-|-|-|-|Mage|Medium|1
Mummy|Ossuary|Undead|70|700|6000|30-40|70|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.3
Necromancer|Ossuary|Undead|33.1|331|2500|10-20|60|25|75|-|-|50|10-20|-|-|-|-|Mage|Medium|1
Nightmare|Ossuary|Undead|67.8|679|4000|15-25|90|25|100|-|-|75|15-25|-|-|-|-|Mage|Fast|1.2
Phantasm|Ossuary|Undead|124.7|1248|6000|20-30|90|0|150|50|-|125|25-35|-|-|-|-|Mage|Medium|1.3
Rag Witch|Ossuary|Undead|77.5|775|4000|10-20|80|25|125|-|-|100|20-30|-|-|-|X|Mage|Fast|1.2
Revenant|Ossuary|Undead|57.6|576|4000|40-50|110|0|100|50|-|75|15-25|-|-|-|-|MeleeMage|Fast|1
Shriveled Corpse|Ossuary|Undead|72.6|727|6000|35-45|75|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.25
Skeletal Archer|Ossuary|Undead|15.8|158|1500|10-20|60|25|25|-|-|-|-|-|-|-|-|Ranged|Medium|1.14
Skeletal Dragon|Ossuary|Undead|127.4|1274|10000|50-60|100|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.2
Skeletal Fiend|Ossuary|Undead|84.5|846|6000|30-40|80|50|25|-|-|-|-|-|-|-|-|Melee|Slow|1.5
Skeletal Guardian|Ossuary|Undead|32.7|328|3000|25-35|75|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1
Skeletal Knight|Ossuary|Undead|31.9|319|3000|20-30|85|25|25|50|-|-|-|-|-|-|-|Melee|Medium|1
Skeletal Mage|Ossuary|Undead|27.4|274|2000|10-20|75|25|75|-|-|50|10-20|-|-|-|-|Mage|Medium|1
Skeletal Marksman|Ossuary|Undead|29.4|295|2500|20-30|70|25|25|-|-|-|-|-|-|-|-|Ranged|Medium|1.14
Skeleton|Ossuary|Undead|9|91|1000|10-20|40|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1
Smouldering Lich|Ossuary|Undead|90.7|908|5000|20-30|90|25|125|-|-|100|20-30|-|-|-|-|Mage|Medium|1.2
Spectral Terror|Ossuary|Undead|205.8|2058|12000|40-50|110|0|200|50|-|175|35-45|-|-|-|-|Mage|Medium|1.2
Withering Bowman|Ossuary|Undead|64|640|5000|30-40|90|25|25|-|-|-|-|-|-|-|-|Ranged|Medium|1.2
Zombie Dragon|Ossuary|Undead|92|920|8000|50-60|90|75|25|-|-|-|-|Deadly|-|0.6|-|Melee|Medium|1
Drowned Dragon|Pulma|Undead|148.6|1487|10000|50-60|100|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1.4
Sea Hag|Pulma|Undead|141.4|1414|6000|10-20|110|25|200|-|-|175|35-45|-|-|-|X|Mage|VeryFast|1.2
Soldier's Widow|Shadowspire Cathedral|Undead|147.2|1472|8000|25-35|110|25|175|-|-|150|30-40|-|-|-|-|MeleeMage|VeryFast|1.35
Spectral Bard|Shadowspire Cathedral|Undead|164.3|1643|10000|30-40|110|0|200|50|-|175|35-45|-|-|-|-|Mage|Medium|1.05
Spectral Bishop|Shadowspire Cathedral|Undead|164.5|1646|8000|1205-30|100|0|175|50|-|150|30-40|-|-|-|-|Mage|Slow|1.3
Spectral Footman|Shadowspire Cathedral|Undead|133.1|1332|10000|50-60|120|0|25|75|-|-|-|-|-|-|-|Melee|Medium|1.14
Spectral Knight|Shadowspire Cathedral|Undead|174.4|1744|12000|50-60|120|0|25|100|-|-|-|-|-|-|-|Melee|Medium|1.35
Spectral Lancer|Shadowspire Cathedral|Undead|94.8|948|12000|60-70|120|0|25|75|-|-|-|-|-|-|-|Melee|VeryFast|1.2
Spectral Marksman|Shadowspire Cathedral|Undead|97.2|973|10000|40-50|120|0|25|50|45|-|-|-|-|-|-|Ranged|Medium|1.5
Spectral Pontiff|Shadowspire Cathedral|Undead|187.8|1878|10000|30-40|110|0|200|50|-|175|35-45|-|-|-|-|Mage|Slow|1.2
Spectral Priest|Shadowspire Cathedral|Undead|147.8|1479|8000|15-25|100|0|150|50|-|125|25-35|-|-|-|-|Mage|Slow|1.3
Spectral Warrior|Shadowspire Cathedral|Undead|144.7|1448|10000|50-60|120|0|25|75|-|-|-|-|-|-|-|Melee|Medium|1.25
Apparition|The Mausoleum|Undead|117.2|1173|8000|40-50|100|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1.4
Banshee|The Mausoleum|Undead|115.6|1156|6000|10-20|110|0|150|50|-|125|25-35|-|-|-|-|Mage|VerySlow|1.2
Blightmare|The Mausoleum|Undead|141.8|1418|8000|40-50|100|25|150|-|-|125|25-35|-|-|-|-|Mage|Fast|1.25
Bound Soul|The Mausoleum|Undead|121.5|1215|8000|40-50|120|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Corpsebride|The Mausoleum|Undead|87.7|877|6000|20-30|110|25|25|-|-|-|-|-|-|-|-|Melee|VeryFast|1.5
Elder Vampire|The Mausoleum|Undead|132.8|1329|10000|50-60|130|50|125|-|-|100|20-30|-|-|-|-|MeleeMage|VeryFast|1.35
Elder Vampire Countess|The Mausoleum|Undead|169.3|1693|10000|40-50|120|50|200|-|-|175|35-45|-|-|-|-|MeleeMage|VeryFast|1.35
Forgotten Soul|The Mausoleum|Undead|132.7|1328|8000|40-50|120|0|150|50|-|125|25-35|-|-|-|-|MeleeMage|Medium|1.25
Herald Of Night|The Mausoleum|Undead|223.7|2238|12000|40-50|150|0|150|50|-|-|-|-|-|-|-|Melee|Medium|1.8
Mirror Image|The Mausoleum|Undead|31.8|-|1500|15-25|60|25|150|-|-|125|25-35|-|-|-|-|Mage|Medium|1
Shadowguard|The Mausoleum|Undead|131.8|1319|8000|50-60|120|0|150|100|-|125|25-35|-|-|-|-|MeleeMage|Medium|1.14
Tormented Soul|The Mausoleum|Undead|121.5|1215|8000|40-50|120|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.35
Vampire|The Mausoleum|Undead|119.2|1192|8000|40-50|120|50|100|-|-|75|15-25|-|-|-|-|MeleeMage|Fast|1.3
Vampire Countess|The Mausoleum|Undead|145.2|1452|8000|30-40|110|50|175|-|-|150|30-40|-|-|-|-|MeleeMage|Fast|1.3
Vampire Patrician|The Mausoleum|Undead|248.2|2482|16000|70-80|150|50|200|-|-|200|40-50|-|-|-|-|MeleeMage|Fast|1.4
Vampire Primogen|The Mausoleum|Undead|329.5|3296|18000|60-70|140|50|250|-|-|250|50-60|-|-|-|-|Mage|Fast|1.2
Vampiric Consort|The Mausoleum|Undead|168.2|1682|14000|60-70|140|50|150|-|-|-|-|-|-|-|X|Melee|Fast|2
Vampiric Huntsman|The Mausoleum|Undead|147.1|1472|18000|70-80|140|75|100|-|-|-|-|-|-|-|-|Melee|VeryFast|1.4
Drowned Buccaneer|Tidal Tomb|Undead|159.6|1597|14000|70-80|130|0|25|75|25|-|-|-|-|-|-|Ranged|Medium|2
Drowned Mariner|Tidal Tomb|Undead|66.7|667|6000|60-70|120|0|25|50|25|-|-|-|-|-|-|Ranged|Medium|1.4
Drowned Swashbuckler|Tidal Tomb|Undead|170.9|1710|14000|50-60|130|0|25|100|45|-|-|-|-|-|-|Melee|Medium|2
Soulcatcher|Tidal Tomb|Undead|84.9|850|6000|40-50|100|0|25|50|-|-|-|-|-|-|-|Ranged|Medium|1.2
Burning Lich Lord|Time Dungeon|Undead|2304|23036|135000|45-55|100|100|200|-|60|225|45-55|-|-|0.75|-|Mage|VeryFast|1.45
Ceaseless Hunger|Time Dungeon|Undead|763.5|7636|60000|60-70|140|75|200|33|60|-|-|-|-|0.25|-|Melee|VeryFast|1.3
Corpse Drinker|Time Dungeon|Undead|545.6|5457|30000|60-70|110|150|50|-|60|-|-|-|-|0.75|-|Melee|VeryFast|1.6
Crypt Ghast|Time Dungeon|Undead|330.8|3308|15000|50-60|140|50|175|25|50|-|-|-|-|0.25|-|Melee|VeryFast|1.5
Crypt Wraith|Time Dungeon|Undead|179.7|1797|11000|50-60|90|25|125|50|-|-|-|-|-|0.33|-|Melee|VeryFast|1.5
Cursed Prevalian|Time Dungeon|Undead|572.8|5728|27000|50-60|100|50|225|50|50|200|40-50|-|-|0.33|-|MeleeMage|VeryFast|1.55
Doomed Deckhand|Time Dungeon|Undead|278|2781|25000|70-80|120|100|50|100|70|-|-|-|-|0.33|-|Melee|Fast|1.7
Dread Rotlord|Time Dungeon|Undead|713.6|7136|100000|50-60|120|75|175|-|-|-|-|Deadly|75|0.75|-|Melee|VeryFast|1.3
Epochal Lich Lord|Time Dungeon|Undead|3047|30471|300000|30-40|100|75|300|-|-|250|50-60|-|-|0.8|-|Mage|VeryFast|1.6
Fenpire|Time Dungeon|Undead|398.4|3984|21000|60-70|110|50|200|-|-|225|45-55|Greater|50|0.95|-|MeleeMage|Fast|1.6
Greater Lich|Time Dungeon|Undead|454.3|4543|16000|30-40|100|25|125|-|40|225|45-55|-|-|0.75|-|Mage|Fast|1.55
Greater Rotting Corpse|Time Dungeon|Undead|370.5|3705|21000|70-80|130|50|150|25|-|-|-|-|-|0.75|-|Melee|VeryFast|1.5
Old Friend Of Captain Johne|Time Dungeon|Undead|431.5|4316|40000|40-50|100|75|175|50|40|150|30-40|-|-|0.5|-|MeleeMage|VeryFast|1.3
Phantom|Time Dungeon|Undead|246|2460|16000|65-75|130|100|75|75|70|-|-|-|-|0.33|-|Melee|VeryFast|1.9
Skeletal Lord|Time Dungeon|Undead|189.7|1897|12000|50-60|110|75|100|-|-|-|-|-|-|-|-|Melee|VeryFast|1.4
Skeletal Wizard|Time Dungeon|Undead|216.5|2165|11000|10-20|90|25|125|25|-|150|30-40|-|-|-|-|Mage|Fast|1.45
The Cursed Prevalian Constable|Time Dungeon|Undead|1353|13525|100000|60-70|120|75|300|75|50|250|50-60|-|-|0.33|-|MeleeMage|VeryFast|1.3
Tyball The Cursed|Time Dungeon|Undead|4324|43239|190000|50-60|110|75|275|25|60|325|65-75|-|-|0.8|-|Mage|SuperFast|1.5
Zombie Lord|Time Dungeon|Undead|191.5|1915|13000|40-50|100|75|125|-|-|-|-|Deadly|50|0.75|-|Melee|VeryFast|1.45
Angry Ghost|Wilderness|Undead|68.2|682|4000|25-35|100|0|100|50|-|75|15-25|-|-|-|-|MeleeMage|Medium|1.3
Cadaver|Wilderness|Undead|51|511|4000|25-35|75|25|25|-|-|-|-|-|-|-|-|Melee|Slow|1.3
Ghostly Archer|Wilderness|Undead|33.9|340|4000|25-35|90|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1.1
Ghostly Footman|Wilderness|Undead|37.2|373|4000|30-40|95|0|25|75|-|-|-|-|-|-|-|Melee|Medium|1.14
Ghostly Foreman|Wilderness|Undead|43.1|432|4000|30-40|90|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1.35
Ghostly Knight|Wilderness|Undead|45.2|453|5000|50-60|105|0|25|75|-|-|-|-|-|-|-|Melee|Medium|1.05
Ghostly Lumberjack|Wilderness|Undead|34.1|341|4000|40-50|80|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1
Lingering Handmaid|Wilderness|Undead|68.4|685|4000|20-30|90|0|125|50|-|100|20-30|-|-|-|-|MeleeMage|VeryFast|1.2
Lingering Maiden|Wilderness|Undead|95.2|953|5000|20-30|100|0|175|50|-|150|30-40|-|-|-|-|Mage|VerySlow|1
Skeletal Retinue|Wilderness|Undead|36|361|3000|25-35|100|25|25|50|-|-|-|-|-|-|-|Melee|Medium|1
Skeletal Scout|Wilderness|Undead|37.5|375|3000|25-35|90|25|25|-|-|-|-|-|-|-|-|Ranged|Medium|1.1
Skeletal Woodsman|Wilderness|Undead|26.4|264|2500|20-30|80|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1
Spectral Cavalry|Wilderness|Undead|74.4|745|8000|40-50|120|0|25|75|-|-|-|-|-|-|-|Melee|Fast|1.35
Spectral Citizen|Wilderness|Undead|49.5|496|6000|20-30|100|0|25|50|45|-|-|-|-|-|-|Melee|Medium|1.2
Spectral Craftsman|Wilderness|Undead|57.3|573|6000|50-60|100|0|25|50|-|-|-|-|-|-|-|Melee|Medium|1.2
Spectral Guard|Wilderness|Undead|73.1|731|8000|60-70|120|0|25|75|-|-|-|-|-|-|-|Melee|Medium|1.2
Spectral Militia|Wilderness|Undead|52.8|529|6000|35-45|110|0|25|75|-|-|-|-|-|-|-|Melee|Medium|1.2
Spectral Scout|Wilderness|Undead|65.8|658|8000|20-30|110|0|25|50|45|-|-|-|-|-|-|Ranged|VeryFast|1.3
Spectral Scribe|Wilderness|Undead|117.4|1175|6000|20-30|100|0|150|50|-|150|30-40|-|-|-|-|Mage|Medium|1.1
Spectral Seer|Wilderness|Undead|105.3|1054|6000|30-40|100|0|150|50|-|150|30-40|-|-|-|-|MeleeMage|Medium|1.25
Vengeful Ghost|Wilderness|Undead|64.7|647|4000|25-35|95|0|100|50|45|75|15-25|-|-|-|-|MeleeMage|Fast|1
Wight|Wilderness|Undead|41.3|413|3000|25-35|85|0|100|50|-|75|15-25|-|-|-|-|MeleeMage|Medium|1
Frozen Remains|-|Undead|13.5|-|1500|10-20|50|50|25|-|-|-|-|-|-|-|-|Melee|Medium|1
Polterghast|-|Undead|88.6|-|6000|30-40|120|25|150|50|-|125|25-35|-|-|-|-|MeleeMage|Medium|1
Zombie|-|Undead|5.3|54|600|10-20|25|25|25|-|-|-|-|-|-|0.2|-|Melee|Slow|1
Dune Revenant|Ossuary|Undead|283|2830|20000|70-80|120|0|25|50|-|-|-|-|-|-|-|Melee|Slow|1.5
Remnant|Ossuary|Undead|259.2|2593|20000|70-80|130|25|25|-|-|-|-|-|-|-|-|Melee|Medium|1.25


---

# Appendix: Tameable Creatures (agent: tameable-v2) — 238 rows, AUTHORITATIVE

Source: `Module:TameableCreatureData` (raw Lua, verified end-to-end; two hallucinated rows from an earlier summarizer pass — "Basilisk", "Brood Beetle" — confirmed absent from source and excluded).

Format: `Name|MinTaming|Slots|Class|TamedHits|TamedDmg|TamedArmor|TamedWrestling|TamedMR|UnderdogScalar|Abilities`
Abilities = cooldown,passive,innate (in that order); `-` = none; `(mount=true)` noted inline.

## Derived design facts (from this data)

- **Underdog Scalar is a formula, not hand-set**: fits `1.4 − 0.4 × (MinTaming − 50) / 70` exactly across the table (MinTaming 50 → 1.4, 120 → 1.0). One linear comeback curve for the whole taming meta.
- **Tamed HP budget is slot-tiered**: 1-slot ≈ 25–225 HP, 2-slot ≈ 150–350, 3-slot ≈ 200–550, 4-slot ≈ 350–800 (Fortress Beetle). Confirms pets live on a fixed power budget independent of wild stats.
- **Ability naming is a component system**: shared verbs re-skinned per element (Massive X Breath, X Barrage, Spellburn/Spellchill/Spellvenom..., X Shield) — same "shared engine, themed payload" pattern as weapon-skill procs.
- Fields in source not projected here: combat type (Melee/Spell), poisonresist/specialresist fractions, poisontype+poisoning, stealth flag.
- Sibling modules discovered (not extracted, future targets): `Module:ShipCreatureData`, `Module:StrangelandsCreatureData`, `Module:SummonableCreatureData`, `Module:FollowerAbilityData`.

## 1 slot (107 creatures)

Aegis Rat|55|1|Utility|100|16-20|25|65|100|1.371|Mirror,Disease
Aegis Mongbat|65|1|Attack|150|18-22|25|70|100|1.314|Mirror
Aegis Slime|75|1|Tank|175|20-24|25|70|150|1.257|Mirror,Regeneration,Elusive Form
Wolfhound|80|1|Attack|175|16-20|25|70|50|1.228|Charge,Frenzy
Blood Ape|85|1|Attack|175|18-22|25|65|50|1.2|Blood Frenzy,Bleed
Skulker|90|1|Utility|175|16-20|50|70|50|1.171|Vanish,Disease,Backstab
Aegis Imp|100|1|Attack|150|11-15|25|55|125|1.114|Mirror
Aegis Minion|110|1|Attack|175|12-15|25|65|100|1.057|Mirror
Aegis Scorpion|100|1|Tank|175|16-20|75|70|100|1.114|Mirror
Aegis Whelp|105|1|Attack|175|22-26|50|75|100|1.085|Mirror,Frenzy
Aegis Asp|105|1|Utility|175|16-20|50|80|100|1.085|Mirror
Aegis Leech|110|1|Attack|175|22-26|25|75|100|1.057|Mirror,Burrow
Blood Hunter|115|1|Attack|175|20-24|25|90|50|1.028|Blood Frenzy,Bleed
Sinewseeker|120|1|Attack|200|18-22|50|75|50|1.0|Vanish+Devour,Bleed,Backstab
Bloodskipper|120|1|Attack|200|18-24|25|80|50|1.0|Blood Breath,Bleed
Rime Guar|100|1|Tank|200|16-20|75|75|50|1.114|-,Chill Touch,Mule
Snowdrift|100|1|Utility|150|18-22|25|75|100|1.114|Flurry,Regeneration,Elusive Form
Lemura|120|1|Tank|200|18-22|75|85|125|1.0|Reactive Armor+Magic Reflect,Spellshield
Winter Wolf|65|1|Utility|150|16-20|25|60|50|1.314|-,Chill Touch+Frenzy
Boreal Faerie Wyrm|110|1|Utility|200|18-22|50|75|50|1.057|Boreal Breath
Muck|50|1|Utility|100|12-16|50|55|100|1.4|-,Regeneration,Elusive Form
Swamp Spider|55|1|Utility|125|12-16|25|60|50|1.371|Web
Silverback|60|1|Attack|150|18-22|25|60|50|1.342|-,Frenzy
Jaguar|65|1|Attack|125|12-16|25|65|50|1.314|Vanish,Frenzy,Backstab
Giant Swamp Slug|65|1|Attack|200|18-22|25|70|50|1.314|-,-,Slime Barrage
Firebat|50|1|Attack|125|12-16|25|65|50|1.4|-,Flamestrike+Bleed
Flamehound|55|1|Attack|125|14-18|25|60|50|1.371|-,Flamestrike+Frenzy
Searing Imp|80|1|Attack|150|9-12|25|50|100|1.228|-,Spellburn
Fire Minion|90|1|Attack|150|9-13|25|60|75|1.171|-,Spellburn
Searing Lizard|65|1|Utility|150|18-22|50|55|50|1.314|Crush,Flamestrike
Fire Salamander|60|1|Attack|150|14-18|25|60|50|1.342|-,Flamestrike+Frenzy
Molten Mongbat|95|1|Attack|175|20-24|25|75|50|1.142|-,Flamestrike
Spitting Viper|100|1|Utility|175|22-28|50|75|50|1.114|-,-,Venom Barrage
Rock Guar|50|1|Tank|125|14-18|75|55|50|1.4|-,-,Mule
Cave Bear|60|1|Attack|175|18-22|25|55|50|1.342|-,Enrage
Trapdoor Spider|75|1|Utility|125|14-18|25|60|50|1.257|Vanish+Web,-,Backstab
Devilbat|80|1|Utility|150|18-22|25|80|50|1.228|-,Bleed+Weaken
Stinger|90|1|Utility|150|16-20|25|80|50|1.171|-
Primordial Whelp|50|1|Attack|125|14-18|25|65|50|1.4|-,Frenzy
Monitor Hatchling|50|1|Utility|125|16-20|50|55|50|1.4|Crush
Primordial|60|1|Attack|150|16-20|25|70|50|1.342|-,Frenzy
Monitor|60|1|Utility|175|18-22|50|65|50|1.342|Crush
Drake Whelp|75|1|Attack|125|16-20|50|60|50|1.257|Fire Breath
Chameleon|75|1|Attack|150|16-20|25|65|50|1.257|Vanish,Frenzy,Backstab
Asp|70|1|Utility|125|12-16|50|75|50|1.285|-
Dragon Whelp|85|1|Attack|150|18-22|50|60|50|1.2|Fire Breath
Army Ant|100|1|Utility|175|20-24|50|75|50|1.114|Swarmstrike
Komodo|100|1|Utility|200|24-30|50|70|50|1.114|Crush
Smoke Faerie Dragon|120|1|Attack|200|18-22|50|75|50|1.0|Steam Cloud,-,Backstab+Elusive Form
Scorpion|55|1|Tank|125|14-18|75|55|50|1.371|-
Fire Ant|60|1|Attack|125|16-20|50|65|55|1.342|-,Flamestrike
Cave Bat|70|1|Attack|125|18-22|25|80|50|1.285|-,Bleed
Sphinx|110|1|Tank|200|18-22|75|80|100|1.057|Reactive Armor+Magic Reflect
Black Cat|120|1|Attack|175|22-28|25|90|50|1.0|-,Frenzy+Bad Luck
Cryptwing|110|1|Attack|175|20-26|25|95|50|1.057|Burrow,Frenzy
Giant Sandroach|105|1|Tank|225|20-24|75|75|50|1.085|-,-,Mule
Fiery Leaper|120|1|Attack|200|18-24|25|80|50|1.0|Fire Breath,Flamestrike
Sand Muck|115|1|Utility|175|22-26|25|80|100|1.028|Sandblast,Regeneration,Elusive Form
Shallow Water|50|1|Utility|100|16-20|25|60|100|1.4|-,Soak+Regeneration,Elusive Form
Minion|70|1|Attack|150|9-13|25|50|75|1.285|-
Husk Crab|90|1|Tank|175|18-22|75|75|50|1.171|Crush
Blood Mephit|115|1|Attack|175|12-16|25|70|100|1.028|-,Spellflaying
Earth Mephit|115|1|Utility|175|12-16|25|70|100|1.028|-,Spellcrush
Eldritch Mephit|115|1|Utility|175|11-15|25|70|100|1.028|-,Spellbreak
Fire Mephit|115|1|Attack|175|12-15|25|70|100|1.028|-,Spellburn
Frost Mephit|115|1|Utility|175|12-16|25|70|100|1.028|-,Spellchill
Poison Mephit|115|1|Utility|175|12-16|25|70|100|1.028|Spellvenom
Gravebug|115|1|Tank|225|18-22|75|75|50|1.028|Devour,Disease
Familiar|50|1|Attack|100|8-11|25|40|75|1.4|-
Imp|60|1|Attack|125|9-12|25|45|100|1.342|-
Corpse Eater|55|1|Utility|125|18-22|50|60|50|1.371|-,Disease
Vampire Bat|90|1|Attack|150|20-24|25|85|50|1.171|Devour,Bleed
Shadow Minion|120|1|Utility|175|13-17|25|70|100|1.0|-,Weaken
Shade Wolf|100|1|Attack|175|18-22|25|70|50|1.114|Vanish,Frenzy,Backstab
Void Slime|110|1|Attack|200|24-28|25|80|50|1.057|-,Regeneration,Elusive Form+Slime Barrage
Incubus|115|1|Utility|175|12-15|25|70|100|1.028|-,Dreamlull
Aquamarine Smallshell|100|1|Utility|175|20-24|75|75|50|1.114|Water Claw
Dungeon-Nest Crab|105|1|Utility|175|20-24|75|80|50|1.085|Dig
Oozing Noughtilus|110|1|Attack|200|18-22|50|75|50|1.057|Ooze Aura,-,Mule+Shell
Ruby Smallshell|100|1|Utility|175|20-24|75|75|50|1.114|Pierce Claw
Singing Noughtilus|110|1|Attack|200|20-24|50|80|50|1.057|Sing,-,Mule+Shell
Sheep|25|1|Attack|50|6-10|25|20|50|1.4|-
Goat|25|1|Attack|50|6-10|25|20|50|1.4|-
Pig|25|1|Attack|50|6-10|25|20|50|1.4|-
Chicken|25|1|Attack|50|4-6|25|20|50|1.4|-
Bird|25|1|Attack|25|4-6|25|25|50|1.4|-
Hind|30|1|Attack|75|8-12|25|35|50|1.4|-
Great Hart|35|1|Attack|100|14-18|25|40|50|1.4|-
Eagle|35|1|Attack|75|8-12|25|45|50|1.4|-
Cat|25|1|Attack|50|6-10|25|20|50|1.4|-
Dog|25|1|Attack|50|6-10|25|20|50|1.4|-
Llama|50|1|Attack|150|12-16|25|40|50|1.4|- (mount=true)
Horse|50|1|Attack|150|12-16|25|40|50|1.4|- (mount=true)
Desert Ostard|65|1|Attack|150|12-16|25|70|50|1.314|-,Frenzy (mount=true)
Forest Ostard|75|1|Attack|150|14-18|25|70|50|1.257|- (mount=true)
Tundra Ostard|85|1|Attack|175|14-18|25|75|50|1.2|-,Frenzy (mount=true)
Frenzied Ostard|95|1|Attack|175|16-22|25|80|50|1.142|-,Frenzy (mount=true)
Elk|35|1|Attack|100|14-18|25|40|50|1.4|-
Sand Crab|55|1|Tank|125|16-20|75|60|50|1.371|Crush
Cougar|45|1|Attack|125|14-18|25|55|50|1.4|-,Frenzy
Wolf|45|1|Attack|125|14-18|25|55|50|1.4|-,Frenzy
Black Bear|50|1|Attack|150|18-22|25|50|50|1.4|-,Enrage
Brown Bear|50|1|Attack|150|18-22|25|50|50|1.4|-,Enrage
Grizzly Bear|55|1|Attack|175|18-22|25|55|50|1.371|-,Enrage
Polar Bear|55|1|Attack|175|18-22|25|55|50|1.371|-,Enrage
Sandstalker|120|1|Attack|225|18-22|25|80|50|1.0|Vanish,-,Backstab+Dust Up
Mongbat|30|1|Attack|75|10-14|25|50|50|1.4|-
Giant Bat|40|1|Attack|100|10-14|25|60|50|1.4|-,Bleed
Guar|40|1|Tank|125|12-16|50|45|50|1.4|-,-,Mule
Giant Frog|40|1|Attack|100|16-20|25|50|50|1.4|-,Regeneration
Giant Rat|30|1|Utility|75|10-14|25|55|50|1.4|-,Disease

## 2 slots (68 creatures)

Bloodwolf|100|2|Attack|325|34-38|25|80|50|1.114|Charge+Blood Rage,Bleed
Blood Scorpion|110|2|Tank|250|34-38|75|75|50|1.057|Blood Shield,Bleed
Blood Drake|110|2|Attack|300|38-48|50|80|50|1.057|Massive Blood Breath,Bleed
Blood Courser|110|2|Attack|250|26-30|25|75|75|1.057|-,Spellflaying (mount=true)
Blood Serpent|115|2|Utility|275|30-36|50|80|50|1.028|Blood Expertise,Bleed
Bloodworm|120|2|Attack|350|44-54|25|80|50|1.0|Burrow+Blood Healing,Bleed
Blood Purger|110|2|Attack|250|34-40|50|75|50|1.057|Giant Blood Barrage,Bleed,Blood Barrage
Air Drake|100|2|Utility|300|32-38|50|85|50|1.114|Air Breath+Air Shield
Sabeartooth|85|2|Utility|275|30-36|25|70|50|1.2|Chilled Charge,Chill Touch
Acarid|105|2|Attack|275|36-48|25|80|50|1.085|Burrow
White Wyrmling|110|2|Utility|300|30-36|50|85|50|1.057|Massive Ice Breath,Chill Touch
Frigid Hornbeast|120|2|Utility|350|36-48|25|80|50|1.0|Frigid Blast,Enrage
Antlion|80|2|Attack|250|28-34|75|75|50|1.228|Dig,-,Mule
Colossal Swamp Slug|75|2|Attack|350|38-46|25|75|50|1.257|-,-,Slime Barrage
Anaconda|85|2|Utility|250|34-40|50|80|50|1.2|-
Giant Poison Dart Frog|90|2|Utility|250|32-36|25|85|50|1.171|-,Regeneration
Swamp Drake|105|2|Utility|300|36-42|50|80|50|1.085|Swamp Breath
Black Widow|100|2|Utility|200|32-36|25|90|50|1.114|Web,Disease
Searing Mantis|70|2|Attack|250|28-34|50|75|50|1.285|-,Flamestrike+Bleed
Embear|65|2|Attack|225|26-32|25|70|50|1.314|Charge,Flamestrike
Fire Beetle|75|2|Tank|250|30-36|75|75|50|1.257|-,Flamestrike,Mule
Hellhound|90|2|Attack|300|30-36|25|80|50|1.171|Charge+Fire Breath,Flamestrike,Mule
Brasshopper|120|2|Attack|300|38-44|50|85|50|1.0|Brass Shield+Brass Tacks
Giant Chameleon|100|2|Attack|250|30-38|25|85|50|1.114|Vanish,Frenzy,Backstab
Giant Crystal Beetle|110|2|Tank|300|34-40|75|80|100|1.057|Spelleater Shield,-,Mule
Giant Locust|110|2|Utility|275|36-42|50|80|50|1.057|Swarmstrike
Giant Spitting Viper|110|2|Utility|250|30-38|50|80|50|1.057|-,-,Venom Barrage
Giant Swamp Beetle|110|2|Tank|300|32-38|75|80|100|1.057|Poisoneater Shield,-,Mule
Glowworm|110|2|Utility|275|40-50|25|80|50|1.057|Glowmark
Jungle Mantis|105|2|Utility|300|38-44|50|80|50|1.085|-,Bleed
Savage Primordial|115|2|Attack|300|40-46|25|80|50|1.028|Primal Rage
Giant Trapdoor Spider|95|2|Utility|200|30-36|25|80|50|1.142|Vanish+Web,-,Backstab
Ankheg|105|2|Utility|300|34-42|75|80|50|1.085|Poison Dig,-,Mule
Dusk Drake|100|2|Utility|300|34-42|50|85|50|1.114|Massive Dusk Breath
Earth Drake|105|2|Tank|300|28-38|50|75|50|1.085|Earth Breath+Earth Shield
Drake|85|2|Attack|300|30-36|50|80|50|1.2|Fire Breath
Wyvern Hatchling|95|2|Utility|200|30-36|50|90|50|1.142|-
Adder|80|2|Utility|225|28-36|50|75|50|1.228|-
Flame Purger|90|2|Attack|225|36-44|50|75|50|1.171|Giant Fire Barrage,-,Fire Barrage
Eldritch Drake|105|2|Utility|300|22-28|50|70|100|1.085|Eldritch Breath,Spellshield
Ember Drake|105|2|Attack|300|34-40|50|80|50|1.085|Massive Fire Breath,Flamestrike
Smoke Drake|110|2|Attack|300|32-38|50|85|50|1.057|Steam Cloud,-,Backstab+Elusive Form
Harvestman|120|2|Utility|275|40-46|25|90|50|1.0|Burrow,Disease
Ruby Wyrmling|110|2|Utility|300|40-46|50|80|50|1.057|Massive Ruby Breath
Azure Wyrmling|110|2|Attack|300|32-38|50|85|50|1.057|Concussion Breath
Sun Wyrmling|110|2|Attack|300|38-44|50|80|50|1.057|Scorching Breath
Giant Scorpion|65|2|Tank|225|26-30|75|70|50|1.314|-
Scarab|70|2|Tank|250|34-40|75|70|100|1.285|Reactive Armor+Magic Reflect,-,Mule
Nightmare|100|2|Attack|225|22-28|25|75|75|1.114|-,Spellburn
Radiant Burrowbug|115|2|Attack|325|30-38|75|80|50|1.028|Fiery Dig,Flamestrike,Mule
Tidal Mantis|100|2|Utility|300|40-46|50|80|50|1.114|-,Bleed+Soak
Water Drake|100|2|Utility|300|38-44|50|80|50|1.114|Massive Water Breath
Reef Serpent|100|2|Utility|225|32-38|50|80|50|1.114|-
Corrupted Hornbeast|120|2|Utility|350|40-50|25|80|50|1.0|Corruption Blast,Enrage
Carrion Beetle|65|2|Tank|250|26-32|75|70|50|1.314|-,Disease,Mule
Corpse Purger|110|2|Utility|250|34-40|50|75|50|1.057|Giant Corpse Barrage,Disease,Corpse Barrage
Blightmare|120|2|Utility|275|26-32|25|75|75|1.0|-,Spellblight
Nightstalker|110|2|Attack|300|32-40|25|85|50|1.057|Vanish+Charge,Frenzy,Backstab
Devourer Beetle|100|2|Tank|300|32-38|75|80|50|1.114|Devour,Disease,Mule
Entozoon|120|2|Utility|300|42-54|25|80|50|1.0|Devour,Eversion
Biolumen Shellpod|115|2|Attack|325|34-40|75|80|50|1.028|Biolumen Barrage
Giant Pincer Crab|110|2|Tank|300|32-38|75|80|50|1.057|-,Pinch
Lighthouse Shellpod|115|2|Attack|325|34-40|75|80|50|1.028|Light Barrage
Cow|30|2|Attack|150|24-30|25|40|50|1.4|-
Bull|35|2|Attack|200|26-32|25|40|50|1.4|-
Walrus|35|2|Attack|200|10-14|25|30|50|1.4|-
Bison|35|2|Attack|200|26-32|25|40|50|1.4|-
Bonehorn|115|2|Attack|275|40-46|25|80|50|1.028|Devour,Gore
Warpig|110|2|Attack|325|40-50|25|85|50|1.057|-,Warpigment
Colossal Frog|50|2|Attack|200|32-38|25|55|50|1.4|-,Regeneration
Giant Spider|60|2|Utility|200|26-30|25|70|50|1.342|Web

## 3 slots (45 creatures)

Blood Dragon|120|3|Attack|450|52-66|50|85|50|1.0|Massive Blood Breath,Bleed
Goretusk|115|3|Attack|550|46-62|25|85|50|1.028|-,Enrage+Gore,Mule
Blood Gorger|120|3|Attack|425|44-58|25|85|50|1.0|Gorge,Grasp+Bleed
Decaying Dragon|120|3|Attack|450|42-54|50|75|50|1.0|Flesheater,Bleed
Giant Blood Purger|120|3|Attack|350|44-54|50|85|50|1.0|Giant Blood Barrage,Bleed,Blood Barrage
Arctic Bullvore|100|3|Utility|500|42-56|25|85|50|1.114|Chilled Charge,Chill Touch,Mule
Air Dragon|110|3|Utility|450|44-60|50|90|50|1.057|Air Breath+Air Shield
Colossal Frost Scorpion|115|3|Tank|400|38-54|75|85|50|1.028|-,Chill Touch
White Wyrm|120|3|Utility|450|42-58|50|90|50|1.0|Massive Ice Breath,Chill Touch
Cave Gorger|110|3|Utility|400|46-62|25|80|50|1.057|Gorge,Grasp
Boreal Wyrm|120|3|Utility|450|52-72|50|85|50|1.0|Boreal Breath
Colossal Poison Dart Frog|100|3|Utility|350|48-60|25|90|50|1.114|-,Regeneration
Arboreal Wisp|105|3|Attack|225|36-42|25|85|150|1.085|-,Earth Seeds,Elusive Form
Swamp Dragon|115|3|Utility|450|50-60|50|85|50|1.028|Swamp Breath
Giant Black Widow|110|3|Utility|300|48-60|25|95|50|1.057|Web,Disease
Bullvore|80|3|Attack|450|44-60|25|80|50|1.228|Charge,-,Mule
Bonfire Wisp|110|3|Attack|250|36-42|25|85|150|1.057|-,Spellburn,Elusive Form
Searing Bullvore|120|3|Attack|550|50-68|25|90|50|1.0|Charge,Flamestrike,Mule
Fire Crawler|100|3|Tank|450|42-62|75|80|50|1.114|Crush,Flamestrike
Colossal Crystal Beetle|120|3|Tank|450|50-70|75|85|100|1.0|Spelleater Shield,-,Mule
Colossal Spitting Viper|120|3|Utility|350|40-46|50|85|50|1.0|-,-,Venom Barrage
Colossal Swamp Beetle|120|3|Tank|450|46-62|75|85|100|1.0|Poisoneater Shield,-,Mule
Sword Spider|120|3|Utility|300|52-64|25|95|50|1.0|Web,Swordspin
Volt Wisp|115|3|Utility|275|34-42|25|90|150|1.028|-,Shock,Elusive Form
Dusk Dragon|110|3|Utility|450|50-66|50|90|50|1.057|Massive Dusk Breath
Earth Dragon|115|3|Tank|450|38-58|50|80|50|1.028|Earth Breath+Earth Shield
Dragon|95|3|Attack|450|40-60|50|85|50|1.142|Fire Breath
Wyvern|105|3|Utility|300|48-58|50|100|50|1.085|-
Eldritch Dragon|115|3|Utility|450|36-44|50|80|150|1.028|Eldritch Breath,Spellshield
Ember Dragon|115|3|Attack|450|48-64|50|85|50|1.028|Massive Fire Breath,Flamestrike
Smoke Dragon|120|3|Attack|450|46-62|50|90|50|1.0|Steam Cloud,-,Backstab+Elusive Form
Ruby Wyrm|120|3|Utility|450|52-72|50|85|50|1.0|Massive Ruby Breath
Azure Wyrm|120|3|Attack|450|46-62|50|90|50|1.0|Concussion Breath
Sun Wyrm|120|3|Attack|450|52-66|50|85|50|1.0|Scorching Breath
Sand Crawler|90|3|Tank|400|48-62|75|80|50|1.171|Crush,-,Mule
Temple Guardian|115|3|Attack|350|44-66|75|85|100|1.028|Vanish+Reactive Armor+Magic Reflect,-,Backstab
Skeletal Dragon|100|3|Utility|450|50-68|50|70|50|1.114|Massive Bone Breath
Colossal Sandroach|115|3|Tank|500|50-70|75|85|50|1.028|-
Colossal Searing Scorpion|110|3|Tank|375|38-56|75|85|50|1.057|-,Flamestrike
Deep Crawler|80|3|Tank|350|48-62|75|75|50|1.228|Crush,-,Mule
Drowned Dragon|115|3|Utility|450|42-54|50|75|50|1.028|Drowned Barrage
Water Dragon|110|3|Utility|450|52-66|50|85|50|1.057|Massive Water Breath
Manticore|105|3|Utility|400|50-70|50|90|50|1.085|Manticore Venom
Colossal Pincer Crab|120|3|Tank|450|48-64|75|85|50|1.0|-,Pinch
Wisp|100|3|Attack|200|32-38|25|80|125|1.114|-,Spellsurge,Elusive Form
Giant Strider|110|3|Attack|500|50-70|25|85|50|1.057|War Stomp,-,Mule

## 4 slots (18 creatures)

Barbed Prowler|120|4|Attack|400|58-76|50|95|50|1.0|Barb Swarm,Frenzy
Colossal Black Widow|120|4|Utility|400|60-80|25|100|50|1.0|Web,Disease
Otyugh|120|4|Tank|500|38-56|75|80|100|1.0|Spine Barrage
Bogyugh|120|4|Tank|500|32-52|75|80|100|1.0|Bog Barrage
Colossal Dung Beetle|120|4|Tank|550|52-74|75|80|50|1.0|Dung Roller,-,Mule
Gargantua Spider|120|4|Utility|450|62-84|25|105|50|1.0|Weakening Web
Shadow Prowler|120|4|Attack|400|58-76|50|90|50|1.0|-,Frenzy,Elusive Form+Shadowstrikes
Colossal Trapdoor Spider|120|4|Utility|450|58-76|25|90|50|1.0|Vanish+Web,-,Backstab
Phoenix|120|4|Attack|350|42-64|25|70|125|1.0|Epic Barrage,Spellburn+Spellsurge
Colossal Blazing Beetle|120|4|Tank|600|52-74|75|80|50|1.0|Fiery Charge,Flamestrike,Mule
Fortress Beetle|120|4|Tank|800|60-80|75|90|100|1.0|Mirror,Grit,Mule
Darkscale|120|4|Utility|500|60-80|50|95|50|1.0|Darkstrike,-,Elusive Form
Tidal Krait|120|4|Utility|400|50-70|50|100|50|1.0|Choke
Colossal Huntsman|120|4|Utility|450|62-84|25|105|50|1.0|Hunting Web
Colossal Strider|120|4|Attack|700|60-80|25|95|50|1.0|War Stomp,-,Mule
Sunscale|120|4|Attack|500|60-80|50|95|50|1.0|Scorching Breath
Colossal Boa|120|4|Utility|400|56-72|50|100|50|1.0|Constrict
Bird of Paradise|120|4|Utility|350|42-64|25|70|125|1.0|Tranquility
