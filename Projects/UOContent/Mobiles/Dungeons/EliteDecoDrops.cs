using System;
using Server.Items;

namespace Server.Mobiles;

// House-deco drop pool for named dungeon elites (level 7+, 1% roll — see DungeonElite.OnDeath).
// Tier A = level 7-8 (statuettes + common Doom artifacts). Tier B = level 9-10, adds the
// rarer Doom artifacts and the 7 heritage rug deeds on top of everything in Tier A.
public static class EliteDecoDrops
{
    private static readonly Func<Item>[] _tierA =
    [
        () => new MonsterStatuette(MonsterStatuetteType.Minotaur),
        () => new MonsterStatuette(MonsterStatuetteType.Daemon),
        () => new MonsterStatuette(MonsterStatuetteType.Dragon),
        () => new MonsterStatuette(MonsterStatuetteType.Lich),
        () => new MonsterStatuette(MonsterStatuetteType.Gargoyle),
        () => new MonsterStatuette(MonsterStatuetteType.EarthElemental),
        () => new MonsterStatuette(MonsterStatuetteType.FireElemental),
        () => new MonsterStatuette(MonsterStatuetteType.Efreet),
        () => new MonsterStatuette(MonsterStatuetteType.Reaper),
        () => new MonsterStatuette(MonsterStatuetteType.Gazer),
        // Doom artifacts, ArtifactRarity <= 3
        () => new BooksWestArtifact(),
        () => new BooksNorthArtifact(),
        () => new BooksFaceDownArtifact(),
        () => new BottleArtifact(),
        () => new BrazierArtifact(),
        () => new DamagedBooksArtifact(),
        () => new LampPostArtifact(),
        () => new RockArtifact(),
        () => new SkullCandleArtifact(),
        () => new StretchedHideArtifact()
    ];

    private static readonly Func<Item>[] _tierBOnly =
    [
        // Doom artifacts, ArtifactRarity >= 4
        () => new BackpackArtifact(),
        () => new BloodyWaterArtifact(),
        () => new CocoonArtifact(),
        () => new EggCaseArtifact(),
        () => new GruesomeStandardArtifact(),
        () => new LeatherTunicArtifact(),
        () => new RuinedPaintingArtifact(),
        () => new SaddleArtifact(),
        () => new SkinnedDeerArtifact(),
        () => new SkinnedGoatArtifact(),
        () => new StuddedLeggingsArtifact(),
        () => new StuddedTunicArtifact(),
        () => new TarotCardsArtifact(),
        // Heritage rug deeds
        () => new BlueDecorativeRugDeed(),
        () => new GoldenDecorativeRugDeed(),
        () => new PinkFancyRugDeed(),
        () => new RedPlainRugDeed(),
        () => new BlueFancyRugDeed(),
        () => new BluePlainRugDeed(),
        () => new CinnamonFancyRugDeed()
    ];

    private static readonly Func<Item>[] _tierB = [.. _tierA, .. _tierBOnly];

    public static Item Roll(int mobLevel) => (mobLevel >= 9 ? _tierB : _tierA).RandomElement()();
}
