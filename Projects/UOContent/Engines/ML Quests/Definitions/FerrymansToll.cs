using System;
using System.Collections.Generic;
using Server.Engines.Leveling;
using Server.Engines.LootBags;
using Server.Engines.MLQuests.Objectives;
using Server.Engines.MLQuests.Rewards;
using Server.Engines.Rarity;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.MLQuests.Definitions;

// "The Ferryman's Toll" — the 5-quest newbie chain given and turned in by NewbieFerryman
// (the Ferryman's Shade) at the mouth of the Barrow of the Unremembered. Design:
// dev-docs/newbie-dungeon.md §Quests; mob roster: dev-docs/newbie-dungeon-mobs.md.
//
// Chain shape mirrors AGhostOfCovetous: the first link (FirstBlood) is a normal starter with
// NextQuest set; every later link overrides IsChainTriggered so it is only offered as the
// previous link's follow-up (never randomly); the terminal link (TheRoadDown) is OneTimeOnly so
// the whole chain records completion and can never be offered again. CanOffer walks the NextQuest
// list from the offered quest and refuses once it reaches a done OneTimeOnly link (MLQuest.cs:135).
//
// Three of the objectives (salvage, upgrade, reach-level) are custom two-class objectives whose
// progress is a single session-only bool (ExtraDataType stays None — nothing is serialized), set
// by FerrymansTollHooks when the matching altar/level event fires. A mid-quest server restart
// therefore drops that flag and the player redoes the action.

public class FirstBlood : MLQuest
{
    public FirstBlood()
    {
        Activated = true;
        Title = "First Blood";
        Description =
            "Fresh blood. The dead ahead are weak — go blood them. Five frail skeletons, no more. Come back when it is done.";
        RefusalMessage = "Then you are not ready to cross. Go.";
        InProgressMessage = "Five frail skeletons. The dead do not count themselves.";
        CompletionMessage = "Blooded already. Good. The deeper dead wait — and they do not stay down easy.";
        CompletionNotice = CompletionNoticeShort;

        Objectives.Add(new KillObjective(5, new[] { typeof(NewbieBoneShade) }, "frail skeletons"));

        Rewards.Add(new FilledLootBagReward(0, "a pauper's grave-gift (loot bag)"));
    }

    public override Type NextQuest => typeof(GraveGoods);
}

public class GraveGoods : MLQuest
{
    public GraveGoods()
    {
        Activated = true;
        Title = "Grave Goods";
        Description =
            "The deeper dead are restless — grave-touched and archers both. Put eight of them down and I will see you rewarded from the grave's own hoard.";
        RefusalMessage = "The deeper dead keep their goods, then.";
        InProgressMessage = "Eight of the restless deeper dead. Go.";
        CompletionMessage = "The grave gives up its gift. Take it — you have earned the digging.";
        CompletionNotice = CompletionNoticeShort;

        Objectives.Add(
            new KillObjective(
                8,
                new[] { typeof(NewbieGraveMiasma), typeof(NewbieRestlessArcher) },
                "the restless deeper dead"
            )
        );

        Rewards.Add(new FilledLootBagReward(1, "a grave-gift (loot bag)"));
    }

    public override Type NextQuest => typeof(PriceOfPassage);
    public override bool IsChainTriggered => true;
}

public class PriceOfPassage : MLQuest
{
    public PriceOfPassage()
    {
        Activated = true;
        Title = "The Price of Passage";
        Description =
            "Passage is never free. Take some ruined thing to the Pantheon Altar and salvage it — feed the gods their due. Do it once.";
        RefusalMessage = "No offering, no passage.";
        InProgressMessage = "Salvage one item at the Pantheon Altar. The gods are patient. I am not.";
        CompletionMessage = "The altar has drunk. Here — ichor, the gods' own leavings.";
        CompletionNotice = CompletionNoticeShort;

        Objectives.Add(new SalvageObjective());

        Rewards.Add(new ItemReward("Pantheon Ichor", typeof(PantheonIchor), 10));
    }

    public override Type NextQuest => typeof(TemperedInShadow);
    public override bool IsChainTriggered => true;
}

public class TemperedInShadow : MLQuest
{
    public TemperedInShadow()
    {
        Activated = true;
        Title = "Tempered in Shadow";
        Description =
            "Salvage is only half the craft. Take an item to the Pantheon Altar and upgrade it — temper it in the dark. Once will do.";
        RefusalMessage = "Then your gear stays as weak as you.";
        InProgressMessage = "Upgrade one item at the Pantheon Altar.";
        CompletionMessage = "Tempered. You begin to understand the road down. Take this from the hoard.";
        CompletionNotice = CompletionNoticeShort;

        Objectives.Add(new UpgradeObjective());

        Rewards.Add(new FilledLootBagReward(1, "a grave-gift (loot bag)"));
    }

    public override Type NextQuest => typeof(TheRoadDown);
    public override bool IsChainTriggered => true;
}

public class TheRoadDown : MLQuest
{
    public TheRoadDown()
    {
        Activated = true;
        OneTimeOnly = true;
        Title = "The Road Down";
        Description =
            "One thing more. Grow — reach the second rung of your strength. The barrow only lets the living climb so far.";
        RefusalMessage = "Then you linger among the dead. Suit yourself.";
        InProgressMessage = "Reach level 2. Kill, and you will.";
        CompletionMessage = "At four, the way up opens — come collect your coin.";
        CompletionNotice = CompletionNoticeShort;

        Objectives.Add(new ReachLevelObjective(2));

        Rewards.Add(new DummyReward("The Ferryman's regard — and the road to your coin."));
        // Chain capstone, once per character (OneTimeOnly): a bag-2 like the elites drop.
        // Elites stay the only REPEATABLE Rare source.
        Rewards.Add(new FilledLootBagReward(2, "a king's grave-gift (loot bag)"));
    }

    public override bool IsChainTriggered => true;
}

// Reward: a loot bag of the given level pre-filled with one rolled item. The bag itself is the
// only container item, matching how ItemReward hands the backpack a single item.
public class FilledLootBagReward : BaseReward
{
    private readonly int _bagLevel;

    public FilledLootBagReward(int bagLevel, TextDefinition name) : base(name)
    {
        _bagLevel = bagLevel;
    }

    public override void AddRewardItems(PlayerMobile pm, List<Item> rewards)
    {
        // Barrow quest rewards carry Hades' mark (30-pantheon-bags.md §2) — the Ferryman
        // pays in the coin of the house he serves.
        var bag = new LootBag(_bagLevel, PantheonDomain.Underworld);
        bag.DropItem(LootRoller.Roll(_bagLevel, PantheonDomain.Underworld));
        rewards.Add(bag);
    }
}

// Shared base for the two Pantheon-altar action objectives. Progress is a single session-only bool
// flipped by FerrymansTollHooks; ExtraDataType stays None so nothing is written to the save.
public abstract class AltarActionObjective : BaseObjective
{
    protected AltarActionObjective(TextDefinition name) => Name = name;

    public TextDefinition Name { get; }

    public override void WriteToGump(ref DynamicGumpBuilder builder, ref int y)
    {
        if (Name.Number > 0)
        {
            builder.AddHtmlLocalized(98, y, 312, 16, Name.Number, 0x5F90);
        }
        else if (Name.String != null)
        {
            builder.AddLabel(98, y, 0x481, Name.String);
        }

        y += 16;
    }
}

public abstract class AltarActionObjectiveInstance : BaseObjectiveInstance
{
    protected AltarActionObjectiveInstance(AltarActionObjective objective, MLQuestInstance instance)
        : base(instance, objective) => Objective = objective;

    public AltarActionObjective Objective { get; }

    public bool Completed { get; private set; }

    // Called by FerrymansTollHooks when the matching altar action fires for this player.
    public void MarkComplete()
    {
        Completed = true;
        CheckComplete();
    }

    public override bool IsCompleted() => Completed;

    public override void WriteToGump(ref DynamicGumpBuilder builder, ref int y)
    {
        Objective.WriteToGump(ref builder, ref y);

        base.WriteToGump(ref builder, ref y);

        if (IsCompleted())
        {
            builder.AddHtmlLocalized(113, y, 312, 20, 1055121, 0x7FFF); // Complete
            y += 16;
        }
    }
}

public class SalvageObjective : AltarActionObjective
{
    public SalvageObjective() : base("Salvage an item at the Pantheon Altar")
    {
    }

    public override BaseObjectiveInstance CreateInstance(MLQuestInstance instance) =>
        new SalvageObjectiveInstance(this, instance);
}

public class SalvageObjectiveInstance : AltarActionObjectiveInstance
{
    public SalvageObjectiveInstance(SalvageObjective objective, MLQuestInstance instance)
        : base(objective, instance)
    {
    }
}

public class UpgradeObjective : AltarActionObjective
{
    public UpgradeObjective() : base("Upgrade an item at the Pantheon Altar")
    {
    }

    public override BaseObjectiveInstance CreateInstance(MLQuestInstance instance) =>
        new UpgradeObjectiveInstance(this, instance);
}

public class UpgradeObjectiveInstance : AltarActionObjectiveInstance
{
    public UpgradeObjectiveInstance(UpgradeObjective objective, MLQuestInstance instance)
        : base(objective, instance)
    {
    }
}

// Completes when the player reaches a target character level (Server.Engines.Leveling).
public class ReachLevelObjective : BaseObjective
{
    public ReachLevelObjective(int level = 2) => RequiredLevel = level;

    public int RequiredLevel { get; }

    public override void WriteToGump(ref DynamicGumpBuilder builder, ref int y)
    {
        builder.AddLabel(98, y, 0x481, $"Reach level {RequiredLevel}");
        y += 16;
    }

    public override BaseObjectiveInstance CreateInstance(MLQuestInstance instance) =>
        new ReachLevelObjectiveInstance(this, instance);
}

public class ReachLevelObjectiveInstance : BaseObjectiveInstance
{
    private bool _reached;

    public ReachLevelObjectiveInstance(ReachLevelObjective objective, MLQuestInstance instance)
        : base(instance, objective) => Objective = objective;

    public ReachLevelObjective Objective { get; }

    // Called from the level-up hook with the level just gained. Uses the passed level, not
    // LevelSystem.GetLevel: during ApplyLevelUp the stored context.Level is still the OLD value
    // (it is written only after the level-up loop returns), so GetLevel would read stale here.
    public void NotifyLevel(int level)
    {
        if (level >= Objective.RequiredLevel)
        {
            _reached = true;
            CheckComplete();
        }
    }

    // Fallback to the live level covers the case where the quest is accepted at or above the target
    // (no further level-up event will fire); the NPC turn-in double-click re-checks this.
    public override bool IsCompleted() =>
        _reached || LevelSystem.GetLevel(Instance.Player) >= Objective.RequiredLevel;

    public override void WriteToGump(ref DynamicGumpBuilder builder, ref int y)
    {
        Objective.WriteToGump(ref builder, ref y);

        base.WriteToGump(ref builder, ref y);

        if (IsCompleted())
        {
            builder.AddHtmlLocalized(113, y, 312, 20, 1055121, 0x7FFF); // Complete
            y += 16;
        }
    }
}

// Dispatches the three custom objective triggers from their source systems into any matching
// active quest objective instances the player holds. Mirrors MLQuestSystem.HandleKill's
// context -> QuestInstances -> objective-instance walk (a kill/action counts once per quest).
public static class FerrymansTollHooks
{
    public static void OnSalvaged(Mobile from) => NotifyAltarAction<SalvageObjectiveInstance>(from);

    public static void OnUpgraded(Mobile from) => NotifyAltarAction<UpgradeObjectiveInstance>(from);

    private static void NotifyAltarAction<T>(Mobile from) where T : AltarActionObjectiveInstance
    {
        if (!MLQuestSystem.Enabled || from is not PlayerMobile pm)
        {
            return;
        }

        var context = MLQuestSystem.GetContext(pm);

        if (context == null)
        {
            return;
        }

        var instances = context.QuestInstances;

        for (var i = instances.Count - 1; i >= 0; --i)
        {
            var instance = instances[i];

            if (instance.ClaimReward)
            {
                continue;
            }

            foreach (var objective in instance.Objectives)
            {
                if (!objective.Expired && objective is T altar)
                {
                    altar.MarkComplete();
                    break;
                }
            }
        }
    }

    public static void OnLevelUp(PlayerMobile pm, int level)
    {
        if (!MLQuestSystem.Enabled || pm == null)
        {
            return;
        }

        var context = MLQuestSystem.GetContext(pm);

        if (context == null)
        {
            return;
        }

        var instances = context.QuestInstances;

        for (var i = instances.Count - 1; i >= 0; --i)
        {
            var instance = instances[i];

            if (instance.ClaimReward)
            {
                continue;
            }

            foreach (var objective in instance.Objectives)
            {
                if (!objective.Expired && objective is ReachLevelObjectiveInstance reach)
                {
                    reach.NotifyLevel(level);
                    break;
                }
            }
        }
    }
}
