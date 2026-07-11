using ModernUO.Serialization;
using Server.Gumps;
using Server.Items;
using Server.Targeting;

namespace Server.Engines.Rarity;

// Altar hub for the rarity economy, three flows behind one gump (PantheonAltarGump):
// 1. Legendary Offering — the Patron God chase: offer TWO legendaries of one pantheon domain
//    and the god answers with ONE new legendary of that same domain (random entry, never one of
//    the two offered while other choices exist). Both offerings are consumed — a two-for-one
//    gamble toward the 3-piece Patron / 5-piece Exarch thresholds without touching drop rates.
// 2. Salvage — destroy an Uncommon..Epic variant item for ichor (SalvageSystem).
// 3. Upgrade — spend ichor to raise a themed item one tier, Epic cap (SalvageSystem).
// GM-placed shrine decoration ([Add PantheonAltar), no state.
[SerializationGenerator(0)]
public partial class PantheonAltar : Item
{
    [Constructible]
    public PantheonAltar() : base(0x139A) // marble statue — reads as a shrine
    {
        Movable = false;
        Name = "altar of the twelve";
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.Alive || !from.InRange(GetWorldLocation(), 3))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
            return;
        }

        PantheonAltarGump.DisplayTo(from, this);
    }

    public void BeginLegendaryOffering(Mobile from)
    {
        from.SendMessage("Offer the first legendary from your backpack.");
        from.BeginTarget(-1, false, TargetFlags.None, (m, targeted) => OnFirstOffering(m, targeted));
    }

    public void BeginSalvage(Mobile from)
    {
        from.SendMessage("Choose the item from your backpack to salvage.");
        from.BeginTarget(-1, false, TargetFlags.None, static (m, targeted) => OnSalvageTarget(m, targeted));
    }

    public void BeginUpgrade(Mobile from)
    {
        from.SendMessage("Choose the item from your backpack to upgrade.");
        from.BeginTarget(-1, false, TargetFlags.None, static (m, targeted) => OnUpgradeTarget(m, targeted));
    }

    private static void OnSalvageTarget(Mobile from, object targeted)
    {
        if (targeted is not Item item)
        {
            from.SendMessage("That cannot be salvaged.");
            return;
        }

        if (!SalvageSystem.CanSalvage(from, item, out var yield, out var reason))
        {
            from.SendMessage(reason);
            return;
        }

        from.SendGump(new PantheonAltarConfirmGump(
            $"{item.Name}<br>will be DESTROYED, yielding {yield} ichor.<br><br>Proceed?",
            ok =>
            {
                if (ok)
                {
                    SalvageSystem.TrySalvage(from, item); // re-validates; stale confirm = no-op
                }
            }
        ));
    }

    private static void OnUpgradeTarget(Mobile from, object targeted)
    {
        if (targeted is not Item item)
        {
            from.SendMessage("That cannot be upgraded.");
            return;
        }

        if (!SalvageSystem.CanUpgrade(from, item, out var next, out var cost, out var reason))
        {
            from.SendMessage(reason);
            return;
        }

        var current = ((IRarity)item).Rarity;
        var carried = from.Backpack?.GetAmount(typeof(PantheonIchor)) ?? 0;

        from.SendGump(new PantheonAltarConfirmGump(
            $"{item.Name}<br>{RarityConfig.GetName(current)} to {RarityConfig.GetName(next)}<br>" +
            $"Cost: {cost} ichor (carrying {carried}).<br><br>Proceed?",
            ok =>
            {
                if (ok)
                {
                    SalvageSystem.TryUpgrade(from, item); // re-validates; stale confirm = no-op
                }
            }
        ));
    }

    private void OnFirstOffering(Mobile from, object targeted)
    {
        if (!TryResolveOffering(from, targeted, out var first, out var firstEntry))
        {
            return;
        }

        var domain = PantheonFx.GetDomain(firstEntry.Root);

        from.SendMessage($"{PantheonFx.GetPatronName(domain)} listens. Offer the second legendary of the same god.");
        from.BeginTarget(-1, false, TargetFlags.None, (m, secondTargeted) =>
            OnSecondOffering(m, secondTargeted, first, firstEntry)
        );
    }

    private void OnSecondOffering(Mobile from, object targeted, Item first, LegendaryEntry firstEntry)
    {
        if (!TryResolveOffering(from, targeted, out var second, out var secondEntry))
        {
            return;
        }

        if (second == first)
        {
            from.SendMessage("You must offer two different relics.");
            return;
        }

        // Re-validate the first offering — it may have moved/vanished while targeting.
        if (first.Deleted || !first.IsChildOf(from.Backpack))
        {
            from.SendMessage("The first offering is no longer in your backpack.");
            return;
        }

        var domain = PantheonFx.GetDomain(firstEntry.Root);

        if (PantheonFx.GetDomain(secondEntry.Root) != domain)
        {
            from.SendMessage($"Both relics must belong to {PantheonFx.GetPatronName(domain)}'s domain.");
            return;
        }

        var rolled = RollDomainLegendary(domain, firstEntry.Id, secondEntry.Id);

        if (rolled.Id == 0)
        {
            from.SendMessage("The god has nothing else to offer for this pairing.");
            return;
        }

        first.Delete();
        second.Delete();

        var reward = Server.Engines.LootBags.LootRoller.ConstructForLegendary(rolled);
        RarityEffects.ApplyLegendary(reward, rolled.Id);

        if (!from.AddToBackpack(reward))
        {
            reward.MoveToWorld(from.Location, from.Map);
        }

        from.SendMessage($"{PantheonFx.GetPatronName(domain)} accepts the offering and grants: {rolled.Name}.");
        PantheonFx.PlayWornProc(from, rolled.Root);
    }

    private static bool TryResolveOffering(Mobile from, object targeted, out Item item, out LegendaryEntry entry)
    {
        item = targeted as Item;
        entry = default;

        if (item is not IVariantItem variant || variant.LegendaryId == 0 ||
            !LegendaryRegistry.TryGet(variant.LegendaryId, out entry))
        {
            from.SendMessage("Only a legendary relic may be offered.");
            return false;
        }

        if (!item.IsChildOf(from.Backpack))
        {
            from.SendMessage("The offering must be in your backpack.");
            return false;
        }

        return true;
    }

    // Uniform pick over the domain's registry entries, excluding the two offered ids so the god
    // never hands back what was just given up (unless the domain is too small to avoid it, in
    // which case the exclusion relaxes). Cold path — a linear registry scan is fine.
    private static LegendaryEntry RollDomainLegendary(PantheonDomain domain, ushort excludeA, ushort excludeB)
    {
        var entries = LegendaryRegistry.Entries;
        var count = 0;

        for (var pass = 0; pass < 2; pass++)
        {
            var exclude = pass == 0; // second pass (tiny domain): allow the offered ids back in

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];

                if (PantheonFx.GetDomain(entry.Root) != domain ||
                    exclude && (entry.Id == excludeA || entry.Id == excludeB))
                {
                    continue;
                }

                count++;

                if (Utility.Random(count) == 0)
                {
                    // Reservoir pick of a uniform random match without a temp list.
                    _rolled = entry;
                }
            }

            if (count > 0)
            {
                return _rolled;
            }
        }

        return default;
    }

    // Scratch slot for the reservoir pick above — single-threaded game loop, never concurrent.
    private static LegendaryEntry _rolled;
}
