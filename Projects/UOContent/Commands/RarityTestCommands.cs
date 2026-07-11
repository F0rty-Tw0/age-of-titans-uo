using System;
using Server.Engines.Rarity;
using Server.Items;
using Server.Targeting;

namespace Server.Commands;

// GM testing kit for the rarity system. The loot roller (a later phase) is the production path
// for generating variants/legendaries — these commands exist so every effect can be exercised
// in-game now (armor slot-sets, capstones, legendaries, stackable-poison riders).
public static class RarityTestCommands
{
    public static void Initialize()
    {
        // Legendary minting lives in RarityCommands ([Legendary <id|name>]) — not duplicated here.
        CommandSystem.Register("GenVariant", AccessLevel.GameMaster, GenVariant_OnCommand);
        CommandSystem.Register("ClearVariant", AccessLevel.GameMaster, ClearVariant_OnCommand);
        CommandSystem.Register("GenArmorSet", AccessLevel.GameMaster, GenArmorSet_OnCommand);
    }

    [Usage("GenVariant <root> <rarity>")]
    [Description("Applies a root drop-variant at the given rarity to a targeted item, e.g. GenVariant Polias Epic.")]
    private static void GenVariant_OnCommand(CommandEventArgs e)
    {
        if (e.Length != 2 || !Enum.TryParse<VariantRoot>(e.GetString(0), true, out var root) ||
            !Enum.TryParse<ItemRarity>(e.GetString(1), true, out var rarity))
        {
            e.Mobile.SendMessage("Usage: GenVariant <root> <rarity>  (e.g. GenVariant Polias Epic)");
            return;
        }

        e.Mobile.SendMessage($"Target the item to become {root} {rarity}.");
        e.Mobile.BeginTarget(-1, false, TargetFlags.None, (from, targeted) =>
            {
                if (targeted is not Item item)
                {
                    from.SendMessage("That is not an item.");
                    return;
                }

                try
                {
                    RarityEffects.ApplyVariant(item, root, rarity);
                    from.SendMessage($"{item.GetType().Name} is now {root} {rarity}.");
                }
                catch (ArgumentException ex)
                {
                    from.SendMessage(ex.Message);
                }
            }
        );
    }

    [Usage("ClearVariant")]
    [Description("Reverts a targeted item to its plain, un-themed state.")]
    private static void ClearVariant_OnCommand(CommandEventArgs e)
    {
        e.Mobile.SendMessage("Target the item to clear.");
        e.Mobile.BeginTarget(-1, false, TargetFlags.None, (from, targeted) =>
            {
                if (targeted is not Item item)
                {
                    from.SendMessage("That is not an item.");
                    return;
                }

                RarityEffects.ClearVariant(item);
                from.SendMessage($"{item.GetType().Name} cleared.");
            }
        );
    }

    // The five thematic roots per material (framework §3's 30 material roots). With no explicit
    // root, GenArmorSet deals these round-robin across the set — closer to real mixed drops, and
    // it exercises §9.4 stacking plus the P5 dedupe in one equip.
    private static VariantRoot[] MaterialRoots(ArmorMaterialType material) => material switch
    {
        ArmorMaterialType.Leather =>
            [VariantRoot.Naias, VariantRoot.Dryas, VariantRoot.Oreias, VariantRoot.Melissa, VariantRoot.Panika],
        ArmorMaterialType.Studded =>
            [VariantRoot.Kynegis, VariantRoot.Batos, VariantRoot.Arkas, VariantRoot.Elaphis, VariantRoot.Skia],
        ArmorMaterialType.Bone =>
            [VariantRoot.Melinoe, VariantRoot.Makaria, VariantRoot.Tymbos, VariantRoot.Nekyia, VariantRoot.Katachthon],
        ArmorMaterialType.Ringmail =>
            [VariantRoot.Hoplites, VariantRoot.Taxis, VariantRoot.Dromos, VariantRoot.Zoster, VariantRoot.Alkimos],
        ArmorMaterialType.Chainmail =>
            [VariantRoot.Phylax, VariantRoot.Egregoros, VariantRoot.Teichos, VariantRoot.Halysis, VariantRoot.Phrourion],
        ArmorMaterialType.Plate =>
            [VariantRoot.Adamas, VariantRoot.Kaminos, VariantRoot.Kolossos, VariantRoot.Panoplia, VariantRoot.Akamatos],
        _ => null
    };

    [Usage("GenArmorSet <material> [rarity=Epic] [root]")]
    [Description("Fills your backpack with a full armor set of the given material (Leather/Studded/Bone/Ringmail/Chainmail/Plate). Without a root, the material's five thematic roots are dealt round-robin; pass a root for a uniform set.")]
    private static void GenArmorSet_OnCommand(CommandEventArgs e)
    {
        if (e.Length < 1 || !Enum.TryParse<ArmorMaterialType>(e.GetString(0), true, out var material))
        {
            e.Mobile.SendMessage("Usage: GenArmorSet <material> [rarity=Epic] [root]");
            return;
        }

        var rarity = ItemRarity.Epic;

        if (e.Length >= 2 && !Enum.TryParse(e.GetString(1), true, out rarity))
        {
            e.Mobile.SendMessage($"Unknown rarity '{e.GetString(1)}'.");
            return;
        }

        var uniformRoot = VariantRoot.None;

        if (e.Length >= 3 && !Enum.TryParse(e.GetString(2), true, out uniformRoot))
        {
            e.Mobile.SendMessage($"Unknown root '{e.GetString(2)}'.");
            return;
        }

        var materialRoots = MaterialRoots(material);

        // One piece per body slot the material actually has (mempo shares the gorget layer, so
        // the studded set uses StuddedGorget — see BaseArmor.BodyPosition: Layer.Neck => Gorget).
        BaseArmor[] pieces = material switch
        {
            ArmorMaterialType.Leather =>
            [
                new LeatherCap(), new LeatherGorget(), new LeatherChest(),
                new LeatherArms(), new LeatherGloves(), new LeatherLegs()
            ],
            ArmorMaterialType.Studded =>
            [
                new StuddedGorget(), new StuddedChest(), new StuddedArms(),
                new StuddedGloves(), new StuddedLegs()
            ],
            ArmorMaterialType.Bone =>
            [
                new BoneHelm(), new BoneChest(), new BoneArms(),
                new BoneGloves(), new BoneLegs()
            ],
            ArmorMaterialType.Ringmail =>
            [
                new RingmailChest(), new RingmailArms(), new RingmailGloves(), new RingmailLegs()
            ],
            ArmorMaterialType.Chainmail => [new ChainCoif(), new ChainChest(), new ChainLegs()],
            ArmorMaterialType.Plate =>
            [
                new PlateHelm(), new PlateGorget(), new PlateChest(),
                new PlateArms(), new PlateGloves(), new PlateLegs()
            ],
            _ => null
        };

        if (pieces == null)
        {
            e.Mobile.SendMessage($"{material} is not a slot-set material (use Leather/Studded/Bone/Ringmail/Chainmail/Plate).");
            return;
        }

        for (var i = 0; i < pieces.Length; i++)
        {
            var piece = pieces[i];
            var root = uniformRoot != VariantRoot.None ? uniformRoot : materialRoots[i % materialRoots.Length];

            try
            {
                RarityEffects.ApplyVariant(piece, root, rarity);
            }
            catch (ArgumentException ex)
            {
                piece.Delete();
                e.Mobile.SendMessage($"{piece.GetType().Name}: {ex.Message}");
                continue;
            }

            e.Mobile.AddToBackpack(piece);
        }

        var rootLabel = uniformRoot != VariantRoot.None ? uniformRoot.ToString() : "mixed-root";
        e.Mobile.SendMessage($"{pieces.Length} {rootLabel} {rarity} {material} pieces added to your pack.");
    }
}
