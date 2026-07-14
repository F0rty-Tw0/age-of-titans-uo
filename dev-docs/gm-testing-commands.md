# GM Testing Commands

In-game commands for exercising the rarity, loot-bag, and slot-set systems. All are `AccessLevel.GameMaster` unless noted. Square brackets = the shard's command prefix.

## Rarity / Legendary

| Command | What it does | Source |
|---|---|---|
| `[Legendary <id\|name>` | Target an item → mint it as that legendary (rarity, root, name, hue, effects). Accepts registry id or exact name, case-insensitive. Does NOT enforce global uniqueness — admin mints can duplicate. | `Commands/RarityTestCommands.cs` |
| `[GenLegendaryFamily <family\|weaponType> [piece]` | Fills your backpack with EVERY legendary in the given family. Weapons: a family name (swords/axes/fencing) → all legendaries for that family; a weapon type (dagger/katana) → only that base type. Armor (plate/bone/leather/etc.) → one sub-bag per piece type with every legendary on that piece (e.g. 5 plate legendaries × 6 pieces = 30 items). Armor + piece (gorget/helm/chest/arms/gloves/legs) → all legendaries on that one piece. | `Commands/RarityTestCommands.cs` |
| `[GenVariant <root> <rarity>` | Target an item → apply a root drop-variant at a rarity. Example: `[GenVariant Tritonian Epic`. | `Commands/RarityTestCommands.cs` |
| `[ClearVariant` | Target an item → revert to plain (no root, Common, no hue/name). Re-roll friendly. | `Commands/RarityTestCommands.cs` |
| `[GenVariantFamily <family\|weaponType> <rarity> [piece]` | Like GenLegendaryFamily but applies a root variant at the given rarity instead of a named legendary. Armor material (plate/bone) → sub-bags per piece type with every root variant at that rarity. Armor + piece → all roots on that one piece. Shields/jewelry/clothing → one bag, each root on a random shape. Family-wide armor (`metalarmor`/`lightarmor`) is NOT supported — roots are material-locked, name a material. Examples: `[GenVariantFamily plate epic`, `[GenVariantFamily bone gloves rare`, `[GenVariantFamily fencing uncommon`, `[GenVariantFamily shields epic`. | `Commands/RarityTestCommands.cs` |
| `[GenArmorSet <material> [rarity=Epic] [root]` | Fill your backpack with a bag holding a full UNIFORM armor set of that material. No root → the material's 5 thematic roots rotate round-robin per call (call it 5× for all five uniform sets). Explicit root → that uniform set. Epic+ completes the slot-set capstone. | `Commands/RarityTestCommands.cs` |
| `[GrantIchor [amount=100]` | Adds a stack of ichor (altar salvage/upgrade currency). Salvage/upgrade run through the altar — see the **Pantheon** section below. (Rates: salvage yields 2/4/8 ichor for Uncommon/Rare/Epic — **halved for a weapon/armor below half durability**; upgrade costs 20/80 to raise a themed item one tier, Epic cap.) | `Commands/RarityTestCommands.cs` |

Wrong base shape on `[Legendary`? The validation message tells you what it needs ("is a shield legendary", "bound to a different clothing piece", material mismatch).

**Examples**

```
[Legendary Skylla            ← by name
[Legendary 211               ← same legendary by id
[GenArmorSet plate           ← Epic uniform plate set (root rotates per call) → capstone "Siege-Shock" on equip
[GenArmorSet chainmail legendary phylax   ← uniform Legendary Phylax chain set
[GenVariant Naias Epic       ← target a LEATHER piece (material-locked root)
[GenLegendaryFamily fencing    ← all 30 fencing legendaries (family-wide)
[GenLegendaryFamily dagger     ← only the 5 Dagger legendaries (by weapon type)
[GenLegendaryFamily swords     ← all 48 swords legendaries (family-wide)
[GenLegendaryFamily katana     ← only the 6 Katana legendaries (by weapon type)
[GenLegendaryFamily plate      ← 5 plate legendaries × 6 piece types = 30 items in sub-bags
[GenLegendaryFamily plate gorget ← same 5 legendaries, all on PlateGorget
```

## Pantheon (altar, devotion, FX)

| Command | What it does | Source |
|---|---|---|
| `[Add PantheonAltar` | Spawns the altar hub ("altar of the twelve", immovable shrine). Double-click within 3 tiles → gump with three flows: **Legendary Offering** (offer two legendaries from ANY domain → the gods grant one new random legendary from the offered relics' FAMILIES — dagger + kris → fencing; dagger + axe → fencing or axes — consuming both, never returning one of the two offered while other choices exist), **Salvage** (destroy an Uncommon–Epic variant for ichor), **Upgrade** (spend ichor to raise a themed item one tier, Epic cap). | `Engines/Rarity/PantheonAltar.cs` |
| `[GrantIchor [amount=100]` | Adds a stack of ichor (the altar's salvage/upgrade currency) to your backpack. | `Commands/RarityTestCommands.cs` |
| `[PantheonFxTest` | Plays every pantheon domain's legendary proc flourish on you, 1.5s apart (11 domains), ending with the devotion "crown" flourish — the in-client FX/sound verification pass. Chat echoes each domain, its patron god, and its perk text. | `Commands/RarityTestCommands.cs` |

**Devotion / Patron / Exarch** — no command; wear the pieces. 3+ worn legendaries whose roots share one pantheon domain pledge you to that god: buff bar shows `Patron: <god>`; 5+ → `Exarch of <god>` (perk doubled). Mint the pieces with `[Legendary` and equip them; the altar's Legendary Offering + ichor are the intended chase toward those thresholds.

## Loot bags

| Command | What it does | Source |
|---|---|---|
| `[LootTest <level 0-10> [count≤100]` | Creates loot bags of the given level, each filled by `LootRoller.Roll` (the production drop path). | `Commands/LootTest.cs` |

Bag level drives the rarity weight table (level 0 ≈ Common-heavy … level 10 ≈ Epic/Legendary-capable) — see `Engines/LootBags/LootRoller.cs` and `RarityConfig.MaxRarityForBagLevel`.

## Reference

- **Legendary ids/names**: `Engines/Rarity/Effects/LegendaryRegistry.cs` (`_entries` — id, name, root, family, clause, params).
- **Rarities**: Common, Uncommon, Rare, Epic, Legendary.
- **Set materials + thresholds**: leather/studded/bone/plate = 4 Epic+ pieces, ringmail = 4, chainmail = 3.
- **Material roots** (what `[GenArmorSet]` deals):
  - Leather: Naias, Dryas, Oreias, Melissa, Panika
  - Studded: Kynegis, Batos, Arkas, Elaphis, Skia
  - Bone: Melinoe, Makaria, Tymbos, Nekyia, Katachthon
  - Ringmail: Hoplites, Taxis, Dromos, Zoster, Alkimos
  - Chainmail: Phylax, Egregoros, Teichos, Halysis, Phrourion
  - Plate: Adamas, Kaminos, Kolossos, Panoplia, Akamatos
- **Generic armor roots** (any armor): Polias (not shields), Cyclopean, Paean, Tritonian, Talarian. Shields: Aegis, Amyntor, Probolos, Herkos, Pnoe. Jewelry: Olympian, Hecatean, Tychean, Nyxian, Demetrian. Clothing: Laurel, Charis, Maenad, Hestian, Arachne.
- Base items for `[Legendary` targeting: spawn with the stock `[Add <ItemType>` command.

## Leveling / newbie dungeon

| Command | What it does | Source |
|---|---|---|
| `[SetLevel <0-10>` | Target a player → set their level directly, XP snapped to that level's threshold. Raising replays every level-up crossed (stat top-up, caps, the level-4 coin/bolt grant); lowering just resets level + caps. | `Commands/LevelTestCommands.cs` |
| `[GiveXP <amount>` | Target a player → routes through the production `LevelSystem.AwardXP` path. Must target a Player-access character — staff targets no-op by design (same exemption real XP gain uses). | `Commands/LevelTestCommands.cs` |
| `[NewbieBarrow` | Teleports you to the newbie dungeon's entrance `GoLocation` (`dev-docs/newbie-dungeon.md`). | `Commands/LevelTestCommands.cs` |
| `[Level` | Player-access. Shows your current level, total XP, and XP remaining to the next level. | `Commands/LevelCommand.cs` |
| `[LevelGuide` | Player-access. Reopens the leveling primer gump (normally shown once, on first ding). | `Commands/LevelCommand.cs` |

## Build & test quickref

| Purpose | Command |
|---|---|
| Compile-verify while the shard is running (avoids the Distribution DLL lock) | `dotnet build Projects/UOContent/UOContent.csproj -p:OutDir=scratch-verify -p:SolutionDir=E:/age-of-titans-uo/` |
| Full test suite (shard must be STOPPED first — Distribution DLLs lock) | `MODERNUO_TEST_DATA_DIR='F:\UO' dotnet test Projects/UOContent.Tests/` |
| Serialization migrations after a `[SerializationGenerator]` change | `dotnet run --project Projects/BuildTool -- --action migrate` |
| Production build + run | `dotnet build` from repo root, then run ModernUO from `Distribution/` |

**GM smoke script — newbie dungeon**

1. `[NewbieBarrow` — teleport to the entrance.
2. `[SetLevel 0` — confirm the entry gate lets you through at level 0.
3. Single-click each mob type — verify `[lvl N]` tag hues match the level gap (yellow for L1/L2 mobs vs. a level-0 character, red inside the elite depth).
4. Kill a few trash mobs — confirm XP gain messages and watch for the loot-bag sparkle telegraph (not guaranteed at trash level).
5. `[SetLevel 3` — enter the elite chamber and kill one elite — confirm the guaranteed bag-2 drop + corpse sparkle/sound telegraph fire every time.
6. `[GiveXP <amount>` repeatedly to cross level 4 — confirm the bolt effect, the Ferryman's Coin grant (once only — re-run and confirm no duplicate), and that re-entering the dungeon at level 4+ ejects you back to the entrance with the "outgrown it" message.
7. PvP-block spot checks with two characters inside the dungeon: melee swing, a field spell, an explosion potion, and a pet attack should all be blocked player-vs-player while mob-vs-player and player-vs-mob stay unaffected.
