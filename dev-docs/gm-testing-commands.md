# GM Testing Commands

In-game commands for exercising the rarity, loot-bag, and slot-set systems. All are `AccessLevel.GameMaster` unless noted. Square brackets = the shard's command prefix.

## Rarity / Legendary

| Command | What it does | Source |
|---|---|---|
| `[Legendary <id\|name>` | Target an item → mint it as that legendary (rarity, root, name, hue, effects). Accepts registry id or exact name, case-insensitive. Does NOT enforce global uniqueness — admin mints can duplicate. | `Commands/RarityCommands.cs` |
| `[GenVariant <root> <rarity>` | Target an item → apply a root drop-variant at a rarity. Example: `[GenVariant Tritonian Epic`. | `Commands/RarityTestCommands.cs` |
| `[ClearVariant` | Target an item → revert to plain (no root, Common, no hue/name). Re-roll friendly. | `Commands/RarityTestCommands.cs` |
| `[GenArmorSet <material> [rarity=Epic] [root]` | Fill your backpack with a full armor set of that material. No root → the material's 5 thematic roots dealt round-robin (like real mixed drops; exercises §9.4 stacking + P5 dedupe). Explicit root → uniform set. Epic+ completes the slot-set capstone. | `Commands/RarityTestCommands.cs` |
| `[GrantIchor [amount=100]` | Adds a stack of ichor (altar salvage/upgrade currency). Salvage/upgrade run through the altar — see the **Pantheon** section below. (Rates: salvage yields 2/4/8 ichor for Uncommon/Rare/Epic — **halved for a weapon/armor below half durability**; upgrade costs 20/80 to raise a themed item one tier, Epic cap.) | `Commands/RarityTestCommands.cs` |

Wrong base shape on `[Legendary`? The validation message tells you what it needs ("is a shield legendary", "bound to a different clothing piece", material mismatch).

**Examples**

```
[Legendary Skylla            ← by name
[Legendary 211               ← same legendary by id
[GenArmorSet plate           ← Epic mixed-root plate set → capstone "Siege-Shock" on equip
[GenArmorSet chainmail legendary phylax   ← uniform Legendary Phylax chain set
[GenVariant Naias Epic       ← target a LEATHER piece (material-locked root)
```

## Pantheon (altar, devotion, FX)

| Command | What it does | Source |
|---|---|---|
| `[Add PantheonAltar` | Spawns the altar hub ("altar of the twelve", immovable shrine). Double-click within 3 tiles → gump with three flows: **Legendary Offering** (offer two legendaries of one pantheon domain → the god grants one new random legendary of that same domain, consuming both — the two-for-one Patron chase), **Salvage** (destroy an Uncommon–Epic variant for ichor), **Upgrade** (spend ichor to raise a themed item one tier, Epic cap). | `Engines/Rarity/PantheonAltar.cs` |
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
