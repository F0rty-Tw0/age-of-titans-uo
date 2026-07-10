using Server.Engines.Rarity;
using Server.Targeting;

namespace Server.Commands
{
    public static class RarityCommands
    {
        public static void Configure()
        {
            CommandSystem.Register("Legendary", AccessLevel.GameMaster, Legendary_OnCommand);
        }

        [Usage("Legendary <id>")]
        [Description(
            "Targets an equippable item and mints it as the legendary with the given registry id " +
            "(see LegendaryRegistry). Sets rarity, root, name, hue and effects. Does NOT enforce " +
            "global uniqueness — an admin mint can duplicate an existing legendary."
        )]
        private static void Legendary_OnCommand(CommandEventArgs e)
        {
            if (e.Length != 1)
            {
                e.Mobile.SendMessage("Usage: [Legendary <id>");
                return;
            }

            var id = e.GetInt32(0);

            if (id <= 0 || id > ushort.MaxValue || !LegendaryRegistry.TryGet((ushort)id, out var entry))
            {
                e.Mobile.SendMessage($"No legendary with id {id}.");
                return;
            }

            e.Mobile.SendMessage($"Target the item to mint as {entry.Name} (#{id}).");
            e.Mobile.Target = new LegendaryTarget((ushort)id);
        }

        private class LegendaryTarget : Target
        {
            private readonly ushort _id;

            public LegendaryTarget(ushort id) : base(-1, false, TargetFlags.None) => _id = id;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is not Item item || item is not IVariantItem)
                {
                    from.SendMessage("That item cannot hold a rarity variant (need a weapon, armor, jewel, or clothing).");
                    return;
                }

                // ApplyLegendary validates the root/family fit the item type and throws otherwise.
                try
                {
                    RarityEffects.ApplyLegendary(item, _id);
                    LegendaryRegistry.TryGet(_id, out var entry);
                    from.SendMessage($"Minted {entry.Name} (#{_id}).");
                }
                catch (System.ArgumentException ex)
                {
                    from.SendMessage(ex.Message);
                }
            }
        }
    }
}
