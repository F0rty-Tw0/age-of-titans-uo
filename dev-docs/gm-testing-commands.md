# GM Testing Commands

In-game commands for exercising the rarity, loot-bag, and slot-set systems. All are `AccessLevel.GameMaster` unless noted. Square brackets = the shard's command prefix.

## Rarity / Legendary

| Command | What it does | Source |
|---|---|---|
| `[Legendary <id\|name>` | Target an item → mint it as that legendary (rarity, root, name, hue, effects). Accepts registry id or exact name, case-insensitive. Does NOT enforce global uniqueness — admin mints can duplicate. | `Commands/RarityCommands.cs` |
| `[GenVariant <root> <rarity>` | Target an item → apply a root drop-variant at a rarity. Example: `[GenVariant Tritonian Epic`. | `Commands/RarityTestCommands.cs` |
| `[ClearVariant` | Target an item → revert to plain (no root, Common, no hue/name). Re-roll friendly. | `Commands/RarityTestCommands.cs` |
| `[GenArmorSet <material> [rarity=Epic] [root]` | Fill your backpack with a full armor set of that material. No root → the material's 5 thematic roots dealt round-robin (like real mixed drops; exercises §9.4 stacking + P5 dedupe). Explicit root → uniform set. Epic+ completes the slot-set capstone. | `Commands/RarityTestCommands.cs` |

Wrong base shape on `[Legendary`? The validation message tells you what it needs ("is a shield legendary", "bound to a different clothing piece", material mismatch).

**Examples**

```
[Legendary Skylla            ← by name
[Legendary 211               ← same legendary by id
[GenArmorSet plate           ← Epic mixed-root plate set → capstone "Siege-Shock" on equip
[GenArmorSet chainmail legendary phylax   ← uniform Legendary Phylax chain set
[GenVariant Naias Epic       ← target a LEATHER piece (material-locked root)
```

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
