using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Engines.Leveling;

// Pure configuration + math for the player leveling system. No mobile state is
// mutated here; everything is a static table lookup or a pure function so it can
// be unit tested in isolation. All numbers are design-final.
public static class LevelConfig
{
    public const int MaxLevel = 10;

    // Never push a single stat past this during a level-up top-up.
    public const int PerStatCap = 200;

    // Total (RawStr + RawDex + RawInt) top-up target when reaching level 1..5.
    // Index i corresponds to level i + 1, so threshold(1) = 100 .. threshold(5) = 300.
    private static readonly int[] _statThresholds = { 100, 150, 200, 250, 300 };

    // Per-skill Skill.Cap by level 0..5 (100-scale). Level > 5 stays at 100.0.
    private static readonly double[] _skillCaps = { 50.0, 60.0, 70.0, 80.0, 90.0, 100.0 };

    // Hand-tuning hook. When a type is present it wins over the HP heuristic in GetMobLevel.
    public static readonly Dictionary<Type, int> MobLevelOverrides = new()
    {
        // Ambient / farm / pack animals: no XP, gray tag. Predators (wolves, bears,
        // panthers, snakes, scorpions) intentionally stay on the HP curve.
        [typeof(Bird)] = 0, [typeof(Chicken)] = 0, [typeof(Rabbit)] = 0, [typeof(JackRabbit)] = 0,
        [typeof(Cat)] = 0, [typeof(Dog)] = 0, [typeof(Rat)] = 0, [typeof(SewerRat)] = 0,
        [typeof(Goat)] = 0, [typeof(MountainGoat)] = 0, [typeof(Pig)] = 0, [typeof(Sheep)] = 0,
        [typeof(Cow)] = 0, [typeof(Bull)] = 0, [typeof(Boar)] = 0,
        [typeof(Horse)] = 0, [typeof(PackHorse)] = 0, [typeof(PackLlama)] = 0,
        [typeof(Llama)] = 0, [typeof(RidableLlama)] = 0,
        [typeof(Hind)] = 0, [typeof(GreatHart)] = 0,
        [typeof(Dolphin)] = 0, [typeof(Walrus)] = 0, [typeof(Squirrel)] = 0, [typeof(Ferret)] = 0,
        [typeof(Eagle)] = 0, [typeof(DesertOstard)] = 0, [typeof(ForestOstard)] = 0,

        // Pantheon tamables (1 pet + 1 mount per dungeon): ambient taming targets on a
        // pet stat budget — no XP, gray tag, same as stock farm/mount animals.
        [typeof(TideBull)] = 0, [typeof(TideSteed)] = 0,
        [typeof(CinderHound)] = 0, [typeof(CinderSteed)] = 0,
        [typeof(WyldCub)] = 0, [typeof(WyldCourser)] = 0,
        [typeof(StormDrakeling)] = 0, [typeof(StormZostrich)] = 0,
        [typeof(StygianWhelp)] = 0, [typeof(StygianNightmare)] = 0,
        [typeof(GaianEarthbear)] = 0, [typeof(GaianOrn)] = 0,
        [typeof(DrownedHound)] = 0, [typeof(DrownedCharger)] = 0,
        [typeof(BrineLynx)] = 0, [typeof(BrineOclock)] = 0,
        [typeof(DrakonBroodling)] = 0, [typeof(DrakonZostrich)] = 0,
        [typeof(TartarusHellcat)] = 0, [typeof(TartarusZostrich)] = 0,

        // Casters punch above their HP: +1..2 over the HP curve.
        [typeof(EvilMage)] = 3, [typeof(EvilMageLord)] = 4,
        [typeof(SkeletalMage)] = 3, [typeof(BoneMagi)] = 3,
        [typeof(OrcishMage)] = 4, [typeof(RatmanMage)] = 4,
        [typeof(Gazer)] = 3, [typeof(ElderGazer)] = 6,
        [typeof(OphidianMage)] = 5, [typeof(OphidianArchmage)] = 6,
        [typeof(Lich)] = 5, [typeof(LichLord)] = 7, [typeof(AncientLich)] = 9,

        // Newbie dungeon (Barrow of the Unremembered): pinned regardless of HP tuning so
        // the [lvl N] tag, XP gap, and bag level stay stable as stats get balanced.
        [typeof(NewbieBoneShade)] = 1, [typeof(NewbieGraveRat)] = 1, [typeof(NewbieCorpseCrawler)] = 1,
        [typeof(NewbieBoneBowman)] = 1, [typeof(NewbieBarrowBat)] = 1,
        [typeof(NewbieGraveMiasma)] = 2, [typeof(NewbieRestlessArcher)] = 2, [typeof(NewbieMourner)] = 2,
        [typeof(NewbieChanter)] = 2, [typeof(NewbieGraveArcher)] = 2, [typeof(NewbieWight)] = 2,
        [typeof(NewbieCharon)] = 3, [typeof(NewbieFallenChampion)] = 3, [typeof(NewbieHollowWarden)] = 3,

        // Dungeon ladder — Five Domains (dev-docs/dungeon-ladder.md). Same rule as the
        // barrow: pins keep tag/XP/bag stable while HP gets tuned.
        // The Drowned Tholos (Sea, L4-5)
        [typeof(TideDrudge)] = 4, [typeof(TideBrinescale)] = 4,
        [typeof(TideHoplite)] = 5, [typeof(TideMaw)] = 5, [typeof(TideSentinel)] = 5,
        [typeof(TideWarden)] = 5, [typeof(TideHerald)] = 5,
        // The Cinderworks (Forge, L5-6)
        [typeof(CinderThrall)] = 5, [typeof(CinderImp)] = 5,
        [typeof(CinderGargoyle)] = 6, [typeof(CinderAutomaton)] = 6, [typeof(CinderSentinel)] = 6,
        [typeof(CinderCyclops)] = 6, [typeof(CinderHeart)] = 6,
        // The Nemean Wildwood (Hunt, L6-7)
        [typeof(WyldHound)] = 6, [typeof(WyldStalker)] = 6,
        [typeof(WyldPanther)] = 7, [typeof(WyldPython)] = 7, [typeof(WyldSentinel)] = 7,
        [typeof(WyldMatriarch)] = 7, [typeof(WyldStag)] = 7,
        // The Stormcrown Aerie (Sky, L8-9)
        [typeof(StormHarpy)] = 8, [typeof(StormWisp)] = 8,
        [typeof(StormTitan)] = 9, [typeof(StormDrake)] = 9, [typeof(StormSentinel)] = 9,
        [typeof(StormChained)] = 9, [typeof(StormFather)] = 9,
        // The Stygian Deep (Underworld, L9-10)
        [typeof(StygianShade)] = 9, [typeof(StygianHound)] = 9,
        [typeof(StygianReaper)] = 10, [typeof(StygianBoneLord)] = 10, [typeof(StygianSentinel)] = 10,
        [typeof(StygianCharon)] = 10, [typeof(StygianCerberus)] = 10, [typeof(StygianLord)] = 10,

        // Classic Five enhancement (dev-docs/classic-five-enhancements.md): casters and
        // breath types that punch above their HP, anchoring each dungeon's band.
        [typeof(BloodElemental)] = 6, [typeof(FireGargoyle)] = 5,
        [typeof(Daemon)] = 6, [typeof(Drake)] = 6, [typeof(Dragon)] = 7, [typeof(Balron)] = 8,
        // Classic Five named elites
        [typeof(Enkelados)] = 5, [typeof(Aiakos)] = 6, [typeof(Thaumas)] = 7,
        [typeof(Ladon)] = 8, [typeof(Alastor)] = 9,

        // Classic Five families (dev-docs/classic-five-bestiary.md)
        // Despise — the Gaian earthborn
        [typeof(GaianSpartos)] = 3, [typeof(GaianClayborn)] = 3, [typeof(GaianSownSeed)] = 3,
        [typeof(GaianClayServitor)] = 3, [typeof(GaianSpearborn)] = 4, [typeof(GaianEarthbloodChampion)] = 4,
        [typeof(GaianPhalangite)] = 4, [typeof(GaianTitanWard)] = 4, [typeof(Chthonios)] = 4,
        // Deceit — the Drowned dead
        [typeof(DrownedDead)] = 4, [typeof(DrownedShade)] = 4, [typeof(DrownedMournling)] = 4,
        [typeof(DrownedBrackishHulk)] = 4, [typeof(DrownedLegionary)] = 5, [typeof(DrownedOathbreaker)] = 5,
        [typeof(DrownedWailer)] = 5, [typeof(DrownedLamentor)] = 5, [typeof(Minos)] = 5,
        // Shame — the Brine storm-brood
        [typeof(BrineSpume)] = 5, [typeof(BrineGale)] = 5, [typeof(BrineSeep)] = 5,
        [typeof(BrineLeechEel)] = 5, [typeof(BrineSurge)] = 6, [typeof(BrineMaelstrom)] = 6,
        [typeof(BrineTempest)] = 6, [typeof(BrineSquallHarrier)] = 6, [typeof(Glaukos)] = 6,
        // Destard — the Drakon cult
        [typeof(DrakonWhelp)] = 6, [typeof(DrakonAcolyte)] = 6, [typeof(DrakonSerpentling)] = 6,
        [typeof(DrakonScaleHound)] = 6, [typeof(DrakonWyrmling)] = 7, [typeof(DrakonFlamewing)] = 7,
        [typeof(DrakonZealot)] = 7, [typeof(DrakonFlamespeaker)] = 7, [typeof(Pythios)] = 7,
        // Hythloth — the Tartarus brood
        [typeof(TartarusImp)] = 7, [typeof(TartarusGargoyle)] = 7, [typeof(TartarusSoulgorger)] = 7,
        [typeof(TartarusHellbrand)] = 7, [typeof(TartarusFiend)] = 8, [typeof(TartarusTormentFiend)] = 8,
        [typeof(TartarusEnforcer)] = 8, [typeof(TartarusStonewrath)] = 8, [typeof(Eurynomos)] = 8,

        // Five Domains expanded bestiary (dev-docs/dungeon-ladder-bestiary.md)
        // Drowned Tholos +28
        [typeof(TideConscript)] = 4, [typeof(TideDiver)] = 4, [typeof(TideRower)] = 4,
        [typeof(TideLeech)] = 4, [typeof(TideEel)] = 4, [typeof(TideJelly)] = 2,
        [typeof(TideCrabling)] = 2, [typeof(TideGull)] = 2,
        [typeof(TideMarine)] = 5, [typeof(TidePikeman)] = 5, [typeof(TideVotary)] = 5,
        [typeof(TideHexer)] = 5, [typeof(TideCoralguard)] = 5, [typeof(TideAbyssEel)] = 5,
        [typeof(TideReefserpent)] = 5, [typeof(TideSpinecrab)] = 5, [typeof(TideBloated)] = 5,
        [typeof(TideWraith)] = 5, [typeof(TidePelagosDeckhand)] = 5, [typeof(TidePelagosLookout)] = 5,
        [typeof(TidePelagosHarpooner)] = 5, [typeof(TidePelagosDrummer)] = 5, [typeof(TidePelagosNavigator)] = 5,
        [typeof(TidePelagosOarmaster)] = 5, [typeof(TidePelagosBosun)] = 5, [typeof(TidePelagosBrineshade)] = 5,
        [typeof(TidePelagosCaptain)] = 5, [typeof(TideNavarch)] = 5,
        // Cinderworks +28
        [typeof(CinderStoker)] = 5, [typeof(CinderHauler)] = 5, [typeof(CinderEmberling)] = 5,
        [typeof(CinderSalamander)] = 5, [typeof(CinderSlaghound)] = 5, [typeof(CinderScorial)] = 5,
        [typeof(CinderMoth)] = 5, [typeof(CinderKeryxCourier)] = 5, [typeof(CinderMothling)] = 5,
        [typeof(CinderSlagling)] = 5, [typeof(CinderEmberrat)] = 5,
        [typeof(CinderGrelt)] = 6, [typeof(CinderForgewright)] = 6, [typeof(CinderPyreling)] = 6,
        [typeof(CinderMoltling)] = 6, [typeof(CinderBronzeOgre)] = 6, [typeof(CinderSlagDaemon)] = 6,
        [typeof(CinderKindler)] = 6, [typeof(CinderCrucible)] = 6, [typeof(CinderKeryx)] = 6,
        [typeof(CinderKeryxHerald)] = 6, [typeof(CinderKeryxSentry)] = 6, [typeof(CinderKeryxBellows)] = 6,
        [typeof(CinderKeryxSapper)] = 6, [typeof(CinderKeryxWarden)] = 6, [typeof(CinderKeryxSmith)] = 6,
        [typeof(CinderKeryxForeman)] = 6, [typeof(CinderKedalion)] = 6,
        // Nemean Wildwood +28
        [typeof(WyldWolf)] = 6, [typeof(WyldLynx)] = 6, [typeof(WyldBoar)] = 6,
        [typeof(WyldViper)] = 6, [typeof(WyldStinger)] = 6, [typeof(WyldHarrier)] = 6,
        [typeof(WyldHart)] = 6, [typeof(WyldThiasosHound)] = 6, [typeof(WyldFawn)] = 6,
        [typeof(WyldSpiderling)] = 6, [typeof(WyldSprite)] = 6,
        [typeof(WyldGrizzly)] = 7, [typeof(WyldDirewolf)] = 7, [typeof(WyldPuma)] = 7,
        [typeof(WyldConstrictor)] = 7, [typeof(WyldTreant)] = 7, [typeof(WyldCorpser)] = 7,
        [typeof(WyldSpider)] = 7, [typeof(WyldNemean)] = 7, [typeof(WyldThiasosHuntress)] = 7,
        [typeof(WyldThiasosArcher)] = 7, [typeof(WyldThiasosWitch)] = 7, [typeof(WyldThiasosPriestess)] = 7,
        [typeof(WyldThiasosPanther)] = 7, [typeof(WyldThiasosStalker)] = 7, [typeof(WyldThiasosHoundmaster)] = 7,
        [typeof(WyldThiasosMatron)] = 7, [typeof(WyldProkris)] = 7,
        // Stormcrown Aerie +28
        [typeof(StormRoc)] = 8, [typeof(StormGale)] = 8, [typeof(StormSprite)] = 8,
        [typeof(StormGazer)] = 8, [typeof(StormThunderharpy)] = 8, [typeof(StormChainling)] = 8,
        [typeof(StormEfreet)] = 8, [typeof(StormAnemoiDrift)] = 8, [typeof(StormAnemoiBreeze)] = 8,
        [typeof(StormAnemoiOutrider)] = 8, [typeof(StormMoth)] = 8, [typeof(StormSparrow)] = 8,
        [typeof(StormPuff)] = 8,
        [typeof(StormColossus)] = 9, [typeof(StormTitanling)] = 9, [typeof(StormWyrm)] = 9,
        [typeof(StormElemental)] = 9, [typeof(StormGargoyle)] = 9, [typeof(StormEye)] = 9,
        [typeof(StormThunderdrake)] = 9, [typeof(StormOgreKing)] = 9, [typeof(StormAnemoiNorthwind)] = 9,
        [typeof(StormAnemoiSquall)] = 9, [typeof(StormAnemoiHerald)] = 9, [typeof(StormAnemoiTempest)] = 9,
        [typeof(StormAnemoiVortex)] = 9, [typeof(StormAnemoiGale)] = 9, [typeof(StormEphialtes)] = 9,
        // Stygian Deep +28
        [typeof(StygianWight)] = 9, [typeof(StygianGhoul)] = 9, [typeof(StygianMummy)] = 9,
        [typeof(StygianBogle)] = 9, [typeof(StygianCorpse)] = 9, [typeof(StygianAsphodelKnight)] = 9,
        [typeof(StygianStyxMage)] = 9, [typeof(StygianWraith)] = 9, [typeof(StygianDamned)] = 9,
        [typeof(StygianWitness)] = 9, [typeof(StygianWisp)] = 9, [typeof(StygianGraverat)] = 9,
        [typeof(StygianShademoth)] = 9,
        [typeof(StygianBoneKnight)] = 10, [typeof(StygianLich)] = 10, [typeof(StygianRevenant)] = 10,
        [typeof(StygianBoneColossus)] = 10, [typeof(StygianDishound)] = 10, [typeof(StygianDaemon)] = 10,
        [typeof(StygianLichLord)] = 10, [typeof(StygianTormentor)] = 10, [typeof(StygianCondemned)] = 10,
        [typeof(StygianJudge)] = 10, [typeof(StygianHarrower)] = 10, [typeof(StygianErinys)] = 10,
        [typeof(StygianScourge)] = 10, [typeof(StygianArbiter)] = 10, [typeof(StygianRhadamanthys)] = 10,

        // Open-world biome families + the Labors (dev-docs/open-world-bestiary.md)
        // Groves (forest)
        [typeof(GroveStag)] = 2, [typeof(GroveThornwolf)] = 2, [typeof(GroveBriarboar)] = 3,
        [typeof(GroveFaun)] = 3, [typeof(GroveNettleback)] = 3, [typeof(GrovePiper)] = 3,
        // Peaks (mountain)
        [typeof(PeakRoc)] = 4, [typeof(PeakTur)] = 4, [typeof(PeakBronzeEttin)] = 4,
        [typeof(PeakCragOgre)] = 5, [typeof(PeakCyclops)] = 5, [typeof(PeakThunderroc)] = 5,
        // Mire (swamp)
        [typeof(MireAlligator)] = 5, [typeof(MireSkulker)] = 5, [typeof(MireToad)] = 5,
        [typeof(MireSerpentspawn)] = 5, [typeof(MireBogthing)] = 6, [typeof(MireHydraspawn)] = 6,
        // Restless (graveyard)
        [typeof(RestlessSkeleton)] = 2, [typeof(RestlessZombie)] = 2, [typeof(RestlessGhoul)] = 3,
        [typeof(RestlessShade)] = 3, [typeof(RestlessWight)] = 3, [typeof(RestlessWraith)] = 3,
        // Shore (coast)
        [typeof(ShoreCrab)] = 3, [typeof(ShoreTideeel)] = 3, [typeof(ShoreReefserpent)] = 4,
        [typeof(ShoreSiren)] = 4, [typeof(ShoreBrineling)] = 4, [typeof(ShoreLuresiren)] = 4,
        // The Labors (named world-hunts)
        [typeof(LaborCeryneianHind)] = 4, [typeof(LaborCalydonianBoar)] = 5,
        [typeof(LaborNemeanLion)] = 6, [typeof(LaborStymphalianHarpy)] = 6,
        [typeof(LaborErymanthianBoar)] = 7, [typeof(LaborCretanBull)] = 8,

        // Gap families (dev-docs/gap-families-bestiary.md)
        // Pyre
        [typeof(PyreCinderrat)] = 5, [typeof(PyreCrawler)] = 5, [typeof(PyreAsp)] = 5,
        [typeof(PyreHound)] = 5, [typeof(PyreEmberling)] = 5, [typeof(PyreServitor)] = 6,
        [typeof(PyreSerpent)] = 6, [typeof(PyreUnburnt)] = 6, [typeof(PyreEfreet)] = 6,
        [typeof(PyrePyromancer)] = 7, [typeof(PyreDaemon)] = 7, [typeof(PyrePhlegyas)] = 8,
        // Rime
        [typeof(RimeThrall)] = 4, [typeof(RimeArcher)] = 4, [typeof(RimeShaman)] = 4,
        [typeof(RimeStalker)] = 4, [typeof(RimeSerpent)] = 4, [typeof(RimeSpider)] = 4,
        [typeof(RimeOoze)] = 4, [typeof(RimeElemental)] = 5, [typeof(RimeGiant)] = 6,
        [typeof(RimeFiend)] = 6, [typeof(RimeWarlord)] = 7, [typeof(RimeAbaris)] = 8,
        // Cursed
        [typeof(CursedShade)] = 5, [typeof(CursedDigger)] = 5, [typeof(CursedDelver)] = 5,
        [typeof(CursedSentinel)] = 6, [typeof(CursedArmour)] = 6, [typeof(CursedBonemagi)] = 6,
        [typeof(CursedNecromancer)] = 6, [typeof(CursedAccursed)] = 6, [typeof(CursedZealot)] = 7,
        [typeof(CursedRevenant)] = 7, [typeof(CursedSummoner)] = 7, [typeof(CursedAeetes)] = 9,
        // Myrmi
        [typeof(MyrmiForager)] = 3, [typeof(MyrmiDrone)] = 3, [typeof(MyrmiSoldier)] = 4,
        [typeof(MyrmiPikebug)] = 4, [typeof(MyrmiAntlion)] = 4, [typeof(MyrmiVenomspur)] = 5,
        [typeof(MyrmiBroodguard)] = 6, [typeof(MyrmiAvenger)] = 6, [typeof(MyrmiWarden)] = 5,
        [typeof(MyrmiMyrmex)] = 7,
        // Ophian
        [typeof(OphianScale)] = 4, [typeof(OphianAcolyte)] = 4, [typeof(OphianReaver)] = 5,
        [typeof(OphianKnight)] = 5, [typeof(OphianPriest)] = 5, [typeof(OphianConstrictor)] = 5,
        [typeof(OphianScaledbrute)] = 6, [typeof(OphianMount)] = 6, [typeof(OphianArchpriest)] = 6,
        [typeof(OphianKeto)] = 7,
        // Lykai
        [typeof(LykaiScout)] = 2, [typeof(LykaiWretch)] = 2, [typeof(LykaiRaider)] = 3,
        [typeof(LykaiHound)] = 3, [typeof(LykaiSkirmisher)] = 3, [typeof(LykaiSnare)] = 3,
        [typeof(LykaiShaman)] = 4, [typeof(LykaiBrute)] = 5, [typeof(LykaiWarleader)] = 5,
        [typeof(LykaiNyktimos)] = 6,
        // Argus
        [typeof(ArgusMote)] = 3, [typeof(ArgusThrall)] = 3, [typeof(ArgusOoze)] = 3,
        [typeof(ArgusSnare)] = 3, [typeof(ArgusEye)] = 3, [typeof(ArgusHarpy)] = 4,
        [typeof(ArgusStoneEye)] = 4, [typeof(ArgusDeadhunter)] = 4, [typeof(ArgusVaultspider)] = 5,
        [typeof(ArgusHoardmage)] = 6, [typeof(ArgusOverseer)] = 6, [typeof(ArgusErysichthon)] = 8,
        // Wayman
        [typeof(WaymanCutpurse)] = 3, [typeof(WaymanBrigand)] = 3, [typeof(WaymanThug)] = 4,
        [typeof(WaymanWaylayer)] = 4, [typeof(WaymanHedgewizard)] = 5, [typeof(WaymanAutomaton)] = 6,
        [typeof(WaymanCaptain)] = 6, [typeof(WaymanArtificer)] = 6, [typeof(WaymanPeriphetes)] = 8,
        // Pelasg
        [typeof(PelasgVermin)] = 2, [typeof(PelasgForager)] = 3, [typeof(PelasgHunter)] = 3,
        [typeof(PelasgShaman)] = 4, [typeof(PelasgPhoroneus)] = 4,

        // Roster-parity expansions (gap/classic/biome bestiary docs, 2026-07-14)
        // Pyre expansion
        [typeof(PyreAshgull)] = 5, [typeof(PyreAshwraith)] = 6, [typeof(PyreBrandbeast)] = 7,
        [typeof(PyreCharhound)] = 5, [typeof(PyreCindermoth)] = 5, [typeof(PyreCoalgeist)] = 6,
        [typeof(PyreCoalwalker)] = 5, [typeof(PyreEmberwing)] = 5, [typeof(PyreKaminosAcolyte)] = 5,
        [typeof(PyreKaminosAnvilguard)] = 7, [typeof(PyreKaminosBrand)] = 6, [typeof(PyreKaminosCindercaller)] = 7,
        [typeof(PyreKaminosEmberbrand)] = 7, [typeof(PyreKaminosEmbermonk)] = 6, [typeof(PyreKaminosForgemaster)] = 7,
        [typeof(PyreKaminosScald)] = 6, [typeof(PyreKaminosStoker)] = 5, [typeof(PyreMagmaton)] = 6,
        [typeof(PyrePhaethon)] = 7, [typeof(PyreScorchling)] = 5, [typeof(PyreScoria)] = 6,
        [typeof(PyreSlaggrub)] = 5, [typeof(PyreSlagling)] = 5,
        // Rime expansion
        [typeof(RimeBoar)] = 4, [typeof(RimeBoreadArcher)] = 5, [typeof(RimeBoreadGale)] = 7,
        [typeof(RimeBoreadHerald)] = 7, [typeof(RimeBoreadHoundmaster)] = 5, [typeof(RimeBoreadLancer)] = 6,
        [typeof(RimeBoreadOutrider)] = 6, [typeof(RimeBoreadRider)] = 5, [typeof(RimeBoreadScout)] = 4,
        [typeof(RimeBoreadShaman)] = 6, [typeof(RimeCheimon)] = 7, [typeof(RimeColossus)] = 7,
        [typeof(RimeCrone)] = 4, [typeof(RimeGolem)] = 6, [typeof(RimeGrub)] = 4,
        [typeof(RimeHowler)] = 4, [typeof(RimeMauler)] = 6, [typeof(RimeMoth)] = 4,
        [typeof(RimeReaver)] = 5, [typeof(RimeSeer)] = 6, [typeof(RimeSparrow)] = 4,
        [typeof(RimeVole)] = 4, [typeof(RimeWight)] = 5,
        // Cursed expansion
        [typeof(CursedBoneguard)] = 6, [typeof(CursedDreadknight)] = 7, [typeof(CursedGheist)] = 6,
        [typeof(CursedGravecaller)] = 5, [typeof(CursedGravehound)] = 5, [typeof(CursedGravemage)] = 6,
        [typeof(CursedGraverat)] = 5, [typeof(CursedGraveslime)] = 5, [typeof(CursedGravewurm)] = 6,
        [typeof(CursedHierophant)] = 8, [typeof(CursedLampadCrossroads)] = 7, [typeof(CursedLampadHex)] = 7,
        [typeof(CursedLampadMatron)] = 8, [typeof(CursedLampadNovice)] = 5, [typeof(CursedLampadPyre)] = 7,
        [typeof(CursedLampadShade)] = 6, [typeof(CursedLampadTorch)] = 6, [typeof(CursedLampadWailer)] = 7,
        [typeof(CursedLampadWarden)] = 6, [typeof(CursedPallbearer)] = 5, [typeof(CursedPerses)] = 8,
        [typeof(CursedTombmoth)] = 5, [typeof(CursedWight)] = 5,
        // Myrmi expansion
        [typeof(MyrmiAiakidGoad)] = 6, [typeof(MyrmiAiakidLancer)] = 5, [typeof(MyrmiAiakidMarshal)] = 7,
        [typeof(MyrmiAiakidPhalanx)] = 5, [typeof(MyrmiAiakidRunner)] = 3, [typeof(MyrmiAiakidShield)] = 4,
        [typeof(MyrmiAiakidSpear)] = 4, [typeof(MyrmiAiakidSpearcaller)] = 6, [typeof(MyrmiAiakidVenomcaster)] = 6,
        [typeof(MyrmiBroodpriest)] = 6, [typeof(MyrmiBurrower)] = 4, [typeof(MyrmiChitinguard)] = 4,
        [typeof(MyrmiGatherer)] = 3, [typeof(MyrmiGnat)] = 3, [typeof(MyrmiGrub)] = 3,
        [typeof(MyrmiHivelord)] = 6, [typeof(MyrmiMenoitios)] = 7, [typeof(MyrmiMite)] = 3,
        [typeof(MyrmiRaidbug)] = 4, [typeof(MyrmiRavager)] = 6, [typeof(MyrmiSoldierRed)] = 5,
        [typeof(MyrmiSpitter)] = 3, [typeof(MyrmiStinger)] = 4, [typeof(MyrmiTunneler)] = 3,
        [typeof(MyrmiWarspur)] = 5,
        // Ophian expansion
        [typeof(OphianAsp)] = 4, [typeof(OphianBasilisk)] = 5, [typeof(OphianBroodmother)] = 7,
        [typeof(OphianCoilrat)] = 4, [typeof(OphianColossus)] = 6, [typeof(OphianConstrictorGreat)] = 6,
        [typeof(OphianCrawler)] = 4, [typeof(OphianDrakon)] = 7, [typeof(OphianHatchling)] = 4,
        [typeof(OphianLancer)] = 5, [typeof(OphianOphiteAcolyte)] = 4, [typeof(OphianOphiteConstrictor)] = 6,
        [typeof(OphianOphiteEnvenomer)] = 7, [typeof(OphianOphiteHierophant)] = 8, [typeof(OphianOphiteMystic)] = 6,
        [typeof(OphianOphiteOracle)] = 6, [typeof(OphianOphiteScaleward)] = 5, [typeof(OphianOphiteSerpentguard)] = 5,
        [typeof(OphianOphiteWarden)] = 7, [typeof(OphianPoine)] = 8, [typeof(OphianScuttler)] = 4,
        [typeof(OphianSerpentmage)] = 6, [typeof(OphianTitanspawn)] = 7, [typeof(OphianVenomancer)] = 5,
        [typeof(OphianWarden)] = 4,
        // Lykai expansion
        [typeof(LykaiBat)] = 2, [typeof(LykaiBloodrunner)] = 4, [typeof(LykaiChieftain)] = 6,
        [typeof(LykaiCur)] = 2, [typeof(LykaiForager)] = 3, [typeof(LykaiGrub)] = 2,
        [typeof(LykaiHowler)] = 4, [typeof(LykaiHunter)] = 3, [typeof(LykaiLykaonidBrute)] = 5,
        [typeof(LykaiLykaonidHoundmaster)] = 6, [typeof(LykaiLykaonidHunter)] = 4, [typeof(LykaiLykaonidPriest)] = 6,
        [typeof(LykaiLykaonidPrince)] = 6, [typeof(LykaiLykaonidShaman)] = 5, [typeof(LykaiLykaonidSkin)] = 4,
        [typeof(LykaiLykaonidStalker)] = 5, [typeof(LykaiLykaonidWhelp)] = 3, [typeof(LykaiMainalos)] = 6,
        [typeof(LykaiMauler)] = 4, [typeof(LykaiRatling)] = 2, [typeof(LykaiRavener)] = 5,
        [typeof(LykaiReaver)] = 5, [typeof(LykaiTracker)] = 3, [typeof(LykaiWhelp)] = 2,
        [typeof(LykaiWitchdoctor)] = 4,
        // Argus expansion
        [typeof(ArgusCoinwraith)] = 4, [typeof(ArgusEyetyrant)] = 7, [typeof(ArgusGilded)] = 3,
        [typeof(ArgusGildedhound)] = 4, [typeof(ArgusGildspider)] = 5, [typeof(ArgusGoldrat)] = 3,
        [typeof(ArgusGoldwyrm)] = 6, [typeof(ArgusHoardknight)] = 5, [typeof(ArgusMidas)] = 7,
        [typeof(ArgusMoteling)] = 3, [typeof(ArgusScryeye)] = 3, [typeof(ArgusTelchineApprentice)] = 4,
        [typeof(ArgusTelchineBlight)] = 7, [typeof(ArgusTelchineGildmage)] = 7, [typeof(ArgusTelchineGuard)] = 6,
        [typeof(ArgusTelchineHexer)] = 6, [typeof(ArgusTelchineOverwarden)] = 8, [typeof(ArgusTelchineSeer)] = 6,
        [typeof(ArgusTelchineSmith)] = 5, [typeof(ArgusTelchineWarden)] = 5, [typeof(ArgusVaultguard)] = 4,
        [typeof(ArgusVaultmoth)] = 3, [typeof(ArgusWatcher)] = 5,
        // Wayman expansion
        [typeof(WaymanBowman)] = 4, [typeof(WaymanBrigadier)] = 6, [typeof(WaymanBronzeguard)] = 6,
        [typeof(WaymanCogwright)] = 6, [typeof(WaymanCrow)] = 3, [typeof(WaymanCudgeler)] = 4,
        [typeof(WaymanCurhound)] = 3, [typeof(WaymanEnforcer)] = 5, [typeof(WaymanFootpad)] = 3,
        [typeof(WaymanGearhound)] = 5, [typeof(WaymanIronclad)] = 7, [typeof(WaymanIsthmianArcher)] = 4,
        [typeof(WaymanIsthmianAutomaton)] = 6, [typeof(WaymanIsthmianBedwright)] = 7, [typeof(WaymanIsthmianBravo)] = 6,
        [typeof(WaymanIsthmianCaptain)] = 7, [typeof(WaymanIsthmianCutthroat)] = 4, [typeof(WaymanIsthmianHexer)] = 5,
        [typeof(WaymanIsthmianPinebender)] = 6, [typeof(WaymanIsthmianReaver)] = 5, [typeof(WaymanProcrustes)] = 7,
        [typeof(WaymanRat)] = 3, [typeof(WaymanRoadwitch)] = 5, [typeof(WaymanRuffian)] = 4,
        [typeof(WaymanSlinger)] = 3, [typeof(WaymanWarlord)] = 7,
        // Pelasg expansion
        [typeof(PelasgApeman)] = 3, [typeof(PelasgBrute)] = 4, [typeof(PelasgCavebat)] = 2,
        [typeof(PelasgCavebear)] = 4, [typeof(PelasgCaveviper)] = 3, [typeof(PelasgCreeper)] = 2,
        [typeof(PelasgDaubed)] = 3, [typeof(PelasgGrub)] = 2, [typeof(PelasgGrubber)] = 2,
        [typeof(PelasgKnapper)] = 2, [typeof(PelasgLelexAper)] = 4, [typeof(PelasgLelexAugur)] = 4,
        [typeof(PelasgLelexBrute)] = 4, [typeof(PelasgLelexForager)] = 2, [typeof(PelasgLelexHunter)] = 3,
        [typeof(PelasgLelexKnapper)] = 3, [typeof(PelasgLelexMother)] = 4, [typeof(PelasgLelexToadherd)] = 3,
        [typeof(PelasgLelexTrapmaster)] = 4, [typeof(PelasgOchremage)] = 4, [typeof(PelasgPelasgos)] = 4,
        [typeof(PelasgSlinger)] = 3, [typeof(PelasgSpearman)] = 3, [typeof(PelasgToadherd)] = 3,
        [typeof(PelasgToadling)] = 2, [typeof(PelasgTorchbearer)] = 3, [typeof(PelasgTrapper)] = 3,
        [typeof(PelasgWarchief)] = 4, [typeof(PelasgWarhunter)] = 4, [typeof(PelasgWhelp)] = 2,
        // Gaian expansion
        [typeof(GaianBoulderback)] = 4, [typeof(GaianClayColossus)] = 5, [typeof(GaianClaymason)] = 4,
        [typeof(GaianClodling)] = 3, [typeof(GaianDelver)] = 4, [typeof(GaianDustadder)] = 3,
        [typeof(GaianEarthwyrm)] = 4, [typeof(GaianFurrowborn)] = 3, [typeof(GaianGegenesDigger)] = 4,
        [typeof(GaianGegenesElder)] = 5, [typeof(GaianGegenesHurler)] = 5, [typeof(GaianGegenesShaker)] = 5,
        [typeof(GaianGegenesThrall)] = 4, [typeof(GaianGegenesWarden)] = 5, [typeof(GaianGraniteAdder)] = 4,
        [typeof(GaianMountainborn)] = 5, [typeof(GaianPeloreus)] = 5, [typeof(GaianQuarrybrute)] = 4,
        [typeof(GaianRootcrawler)] = 3, [typeof(GaianRubblecrawler)] = 3, [typeof(GaianSlinger)] = 4,
        [typeof(GaianStoneshaper)] = 4, [typeof(GaianStonewarden)] = 5, [typeof(GaianTerraGorger)] = 5,
        // Drowned expansion
        [typeof(DrownedBanshee)] = 5, [typeof(DrownedBrinerot)] = 4, [typeof(DrownedDeepGaunt)] = 6,
        [typeof(DrownedFloater)] = 4, [typeof(DrownedGraveEel)] = 4, [typeof(DrownedHarbinger)] = 6,
        [typeof(DrownedHaunt)] = 4, [typeof(DrownedKeen)] = 5, [typeof(DrownedMarine)] = 5,
        [typeof(DrownedNecromage)] = 5, [typeof(DrownedNostosArcher)] = 5, [typeof(DrownedNostosBosun)] = 5,
        [typeof(DrownedNostosCurser)] = 5, [typeof(DrownedNostosDeckhand)] = 5, [typeof(DrownedNostosNavigator)] = 5,
        [typeof(DrownedNostosOarsman)] = 5, [typeof(DrownedPhrontis)] = 6, [typeof(DrownedReaver)] = 5,
        [typeof(DrownedRower)] = 4, [typeof(DrownedSailor)] = 4, [typeof(DrownedSaltMummy)] = 5,
        [typeof(DrownedSilt)] = 4, [typeof(DrownedTidewraith)] = 5, [typeof(DrownedWrecklich)] = 6,
        // Brine expansion
        [typeof(BrineAbyssal)] = 6, [typeof(BrineCrusher)] = 6, [typeof(BrineCyclone)] = 6,
        [typeof(BrineDeepcoil)] = 5, [typeof(BrineFrost)] = 5, [typeof(BrineGlacier)] = 6,
        [typeof(BrineHailspite)] = 7, [typeof(BrineJelly)] = 5, [typeof(BrineOrmenos)] = 7,
        [typeof(BrinePetrel)] = 5, [typeof(BrineReefcrab)] = 5, [typeof(BrineRiptide)] = 6,
        [typeof(BrineSpray)] = 5, [typeof(BrineSquallHawk)] = 6, [typeof(BrineSquallwind)] = 5,
        [typeof(BrineStormHarpy)] = 5, [typeof(BrineTelchinAdept)] = 6, [typeof(BrineTelchinBrinesmith)] = 6,
        [typeof(BrineTelchinDrowner)] = 6, [typeof(BrineTelchinGaler)] = 6, [typeof(BrineTelchinStormcaller)] = 7,
        [typeof(BrineTelchinTideward)] = 6, [typeof(BrineTideElemental)] = 6, [typeof(BrineWaterlord)] = 7,
        // Drakon expansion
        [typeof(DrakonAshviper)] = 6, [typeof(DrakonBroodviper)] = 6, [typeof(DrakonDrakeling)] = 7,
        [typeof(DrakonEmberdrake)] = 7, [typeof(DrakonFirewyrm)] = 7, [typeof(DrakonGreatDrake)] = 8,
        [typeof(DrakonHatchling)] = 6, [typeof(DrakonHoardWyrm)] = 8, [typeof(DrakonIsmenianAugur)] = 7,
        [typeof(DrakonIsmenianCoil)] = 7, [typeof(DrakonIsmenianDrake)] = 7, [typeof(DrakonIsmenianFang)] = 7,
        [typeof(DrakonIsmenianWard)] = 7, [typeof(DrakonIsmenianWyrm)] = 7, [typeof(DrakonIsmenos)] = 7,
        [typeof(DrakonLavaAdder)] = 6, [typeof(DrakonMatron)] = 7, [typeof(DrakonOphidianAvenger)] = 7,
        [typeof(DrakonOphidianSeer)] = 7, [typeof(DrakonOphidianSpear)] = 6, [typeof(DrakonScaleraptor)] = 6,
        [typeof(DrakonScalerat)] = 6, [typeof(DrakonSerpentGuard)] = 7, [typeof(DrakonWyvern)] = 7,
        // Tartarus expansion
        [typeof(TartarusAshhound)] = 7, [typeof(TartarusBrimstone)] = 8, [typeof(TartarusChaosbrand)] = 7,
        [typeof(TartarusCinderImp)] = 7, [typeof(TartarusDaemonspawn)] = 8, [typeof(TartarusEfreet)] = 7,
        [typeof(TartarusEmberwing)] = 7, [typeof(TartarusGargoyleLord)] = 7, [typeof(TartarusGoreling)] = 7,
        [typeof(TartarusHexfiend)] = 8, [typeof(TartarusImpling)] = 7, [typeof(TartarusMenoetius)] = 8,
        [typeof(TartarusPyrehound)] = 7, [typeof(TartarusRimefiend)] = 8, [typeof(TartarusScourgewing)] = 8,
        [typeof(TartarusSlatewarden)] = 8, [typeof(TartarusSoulflayer)] = 8, [typeof(TartarusSuccubus)] = 8,
        [typeof(TartarusTitanBreaker)] = 8, [typeof(TartarusTitanColossus)] = 8, [typeof(TartarusTitanJailer)] = 8,
        [typeof(TartarusTitanShackled)] = 8, [typeof(TartarusTitanThrall)] = 8, [typeof(TartarusTitanWarden)] = 7,
        // Grove expansion
        [typeof(GroveAdder)] = 3, [typeof(GroveApe)] = 3, [typeof(GroveBrambleHound)] = 3,
        [typeof(GroveBrambleNymph)] = 3, [typeof(GroveBrambleQueen)] = 3, [typeof(GroveBrambleThornling)] = 2,
        [typeof(GroveBrambleTreant)] = 3, [typeof(GroveBrambleWarden)] = 3, [typeof(GroveDoe)] = 2,
        [typeof(GroveDryad)] = 3, [typeof(GroveElk)] = 3, [typeof(GroveFangwolf)] = 2,
        [typeof(GroveFawn)] = 2, [typeof(GroveFinch)] = 2, [typeof(GroveForestrat)] = 2,
        [typeof(GroveGreywood)] = 2, [typeof(GroveHare)] = 2, [typeof(GroveHuntwolf)] = 3,
        [typeof(GroveKomosBacchant)] = 3, [typeof(GroveKomosDancer)] = 3, [typeof(GroveKomosFaun)] = 2,
        [typeof(GroveKomosGoatling)] = 2, [typeof(GroveKomosHornmaster)] = 3, [typeof(GroveKomosReveler)] = 3,
        [typeof(GroveLunamoth)] = 2, [typeof(GroveLynx)] = 2, [typeof(GroveMossbear)] = 3,
        [typeof(GrovePanther)] = 3, [typeof(GroveRazorback)] = 3, [typeof(GroveSaplingreaper)] = 3,
        [typeof(GroveSatyr)] = 3, [typeof(GroveShaggybear)] = 3, [typeof(GroveSilenos)] = 3,
        [typeof(GroveSowthing)] = 2, [typeof(GroveSprite)] = 2, [typeof(GroveStinger)] = 3,
        [typeof(GroveThornspider)] = 3, [typeof(GroveThornvine)] = 3, [typeof(GroveThrush)] = 2,
        [typeof(GroveTusker)] = 2, [typeof(GroveViper)] = 2, [typeof(GroveWebspinner)] = 2,
        [typeof(GroveWhitehart)] = 3, [typeof(GroveWillowisp)] = 2, [typeof(GroveWisp)] = 3,
        // Peak expansion
        [typeof(PeakBear)] = 4, [typeof(PeakBoulderbear)] = 5, [typeof(PeakBronzeArcher)] = 5,
        [typeof(PeakBronzeCaptain)] = 5, [typeof(PeakBronzeMastiff)] = 4, [typeof(PeakBronzeSentry)] = 5,
        [typeof(PeakBronzeWarden)] = 5, [typeof(PeakBronzeWatchman)] = 4, [typeof(PeakCliffgargoyle)] = 5,
        [typeof(PeakCondor)] = 4, [typeof(PeakCragtroll)] = 5, [typeof(PeakCragwolf)] = 4,
        [typeof(PeakCyclopsYoung)] = 4, [typeof(PeakEaglet)] = 4, [typeof(PeakElderGazer)] = 5,
        [typeof(PeakEttin)] = 4, [typeof(PeakFrostbear)] = 5, [typeof(PeakGazerling)] = 4,
        [typeof(PeakGetBoulderthrow)] = 5, [typeof(PeakGetChieftain)] = 5, [typeof(PeakGetElder)] = 5,
        [typeof(PeakGetHerdsman)] = 5, [typeof(PeakGetRam)] = 5, [typeof(PeakGetShepherd)] = 4,
        [typeof(PeakGraniteGolem)] = 5, [typeof(PeakGriffonharpy)] = 5, [typeof(PeakLion)] = 4,
        [typeof(PeakOgre)] = 4, [typeof(PeakOgreBrute)] = 5, [typeof(PeakOgrelord)] = 5,
        [typeof(PeakOldCyclops)] = 5, [typeof(PeakPika)] = 4, [typeof(PeakRam)] = 4,
        [typeof(PeakRocFledgling)] = 4, [typeof(PeakRockhound)] = 4, [typeof(PeakSnowcat)] = 4,
        [typeof(PeakSnowwolf)] = 5, [typeof(PeakStoneEttin)] = 5, [typeof(PeakStonegazer)] = 5,
        [typeof(PeakStoneling)] = 4, [typeof(PeakStonetroll)] = 4, [typeof(PeakStormroc)] = 5,
        [typeof(PeakTalos)] = 5, [typeof(PeakTwoheadEttin)] = 5, [typeof(PeakWildgoat)] = 4,
        // Mire expansion
        [typeof(MireAcidThing)] = 6, [typeof(MireAdder)] = 5, [typeof(MireBlackgator)] = 6,
        [typeof(MireBlacksnake)] = 5, [typeof(MireBloattoad)] = 6, [typeof(MireBloodfly)] = 5,
        [typeof(MireBoghulk)] = 6, [typeof(MireBogreaper)] = 6, [typeof(MireBogserpent)] = 5,
        [typeof(MireBullfrog)] = 5, [typeof(MireCoilserpent)] = 6, [typeof(MireCroc)] = 5,
        [typeof(MireDragonfly)] = 5, [typeof(MireDrowned)] = 6, [typeof(MireFenhorror)] = 6,
        [typeof(MireFenmother)] = 6, [typeof(MireGreatgator)] = 6, [typeof(MireHeron)] = 5,
        [typeof(MireHornbeast)] = 5, [typeof(MireLeech)] = 5, [typeof(MireLernaBrute)] = 6,
        [typeof(MireLernaCultist)] = 5, [typeof(MireLernaHierophant)] = 6, [typeof(MireLernaServant)] = 6,
        [typeof(MireLernaThrall)] = 5, [typeof(MireLernaZealot)] = 6, [typeof(MireLizardBrute)] = 6,
        [typeof(MireLizardman)] = 5, [typeof(MireMuckElemental)] = 5, [typeof(MireMudrat)] = 5,
        [typeof(MireOozeElemental)] = 6, [typeof(MireRatmage)] = 5, [typeof(MireRatman)] = 5,
        [typeof(MireScaledArcher)] = 5, [typeof(MireScaledBrute)] = 6, [typeof(MireScaledHunter)] = 5,
        [typeof(MireScaledKing)] = 6, [typeof(MireScaledShaman)] = 6, [typeof(MireScaledSpear)] = 5,
        [typeof(MireSlimer)] = 5, [typeof(MireStinger)] = 5, [typeof(MireStranglevine)] = 6,
        [typeof(MireSwampspider)] = 5, [typeof(MireToadspawn)] = 5, [typeof(MireWidow)] = 6,
        // Restless expansion
        [typeof(RestlessBanshee)] = 3, [typeof(RestlessBarrowking)] = 3, [typeof(RestlessBellringer)] = 3,
        [typeof(RestlessBogle)] = 3, [typeof(RestlessBonearcher)] = 3, [typeof(RestlessBonefinch)] = 2,
        [typeof(RestlessBoneguard)] = 3, [typeof(RestlessBonemage)] = 3, [typeof(RestlessBonerat)] = 2,
        [typeof(RestlessBones)] = 2, [typeof(RestlessBonewalker)] = 3, [typeof(RestlessBoundone)] = 3,
        [typeof(RestlessCadaver)] = 3, [typeof(RestlessCarrionbat)] = 2, [typeof(RestlessCrow)] = 2,
        [typeof(RestlessCryptbat)] = 2, [typeof(RestlessFeaster)] = 3, [typeof(RestlessGhast)] = 3,
        [typeof(RestlessGloom)] = 2, [typeof(RestlessGnawer)] = 2, [typeof(RestlessGravebound)] = 3,
        [typeof(RestlessGravedigger)] = 3, [typeof(RestlessGravemoth)] = 2, [typeof(RestlessGraverat)] = 2,
        [typeof(RestlessHeadless)] = 2, [typeof(RestlessHusk)] = 2, [typeof(RestlessKeener)] = 3,
        [typeof(RestlessLegionArcher)] = 3, [typeof(RestlessLegionKnight)] = 3, [typeof(RestlessLegionMarshal)] = 3,
        [typeof(RestlessLegionPikeman)] = 3, [typeof(RestlessLegionSoldier)] = 3, [typeof(RestlessLegionStandard)] = 3,
        [typeof(RestlessLegionnaire)] = 3, [typeof(RestlessMourner)] = 3, [typeof(RestlessMummy)] = 3,
        [typeof(RestlessPallbearer)] = 3, [typeof(RestlessPhantom)] = 3, [typeof(RestlessPsychopomp)] = 3,
        [typeof(RestlessRattler)] = 2, [typeof(RestlessRotling)] = 2, [typeof(RestlessSexton)] = 3,
        [typeof(RestlessSpecter)] = 3, [typeof(RestlessVampirebat)] = 3, [typeof(RestlessWailer)] = 3,
        // Shore expansion
        [typeof(ShoreBarnacleback)] = 4, [typeof(ShoreBrineserpent)] = 4, [typeof(ShoreBrinespawn)] = 3,
        [typeof(ShoreCoralthing)] = 4, [typeof(ShoreCormorant)] = 3, [typeof(ShoreDeepGuard)] = 4,
        [typeof(ShoreDeepHound)] = 4, [typeof(ShoreDeepNereid)] = 4, [typeof(ShoreDeepOracle)] = 4,
        [typeof(ShoreDeepPrince)] = 4, [typeof(ShoreDeepTideguard)] = 4, [typeof(ShoreDeepone)] = 4,
        [typeof(ShoreDrownedSailor)] = 3, [typeof(ShoreFiddler)] = 3, [typeof(ShoreGull)] = 3,
        [typeof(ShoreHarpy)] = 3, [typeof(ShoreHermit)] = 3, [typeof(ShoreKarkinos)] = 4,
        [typeof(ShoreKingcrab)] = 4, [typeof(ShoreLampbearer)] = 3, [typeof(ShoreMoray)] = 4,
        [typeof(ShoreReaver)] = 4, [typeof(ShoreReefspear)] = 3, [typeof(ShoreReefstalker)] = 3,
        [typeof(ShoreReefviper)] = 4, [typeof(ShoreRockcrab)] = 3, [typeof(ShoreSandcrawler)] = 3,
        [typeof(ShoreSandflea)] = 3, [typeof(ShoreSandpiper)] = 3, [typeof(ShoreSeaSerpent)] = 4,
        [typeof(ShoreSeacow)] = 4, [typeof(ShoreSeahag)] = 4, [typeof(ShoreSeaharpy)] = 4,
        [typeof(ShoreSeal)] = 3, [typeof(ShoreSeaslime)] = 3, [typeof(ShoreShorerat)] = 3,
        [typeof(ShoreShrieker)] = 4, [typeof(ShoreSpinecrab)] = 4, [typeof(ShoreSpineeel)] = 3,
        [typeof(ShoreTidal)] = 4, [typeof(ShoreTidewitch)] = 4, [typeof(ShoreUrchin)] = 3,
        [typeof(ShoreWreckLord)] = 4, [typeof(ShoreWreckWitch)] = 4, [typeof(ShoreWrecker)] = 4
    };

    // Cumulative XP needed to reach each level (index 0 = level 1 .. index 9 = level 10).
    // Hand-tuned, not a formula: per-level cost = kill target x mob XP one level above the
    // player, where the +1 level gap applies the 1.25x GapMultiplier bonus and mob XP is
    // BaseMobXP(mobLevel). Kill targets by level: L1=30, L2=40, L3=60, L4=80, L5=100, L6=120,
    // L7=140, L8=160, L9=180, L10=200. E.g. L1 = 30 * 100 * 1.25 = 3750; the L10 leg alone is
    // 200 * 1000 * 1.25 = 250000, on top of the L1..L9 total for a 963750 cumulative.
    private static readonly long[] _cumulativeXP =
    {
        3_750, 13_750, 36_250, 76_250, 138_750, 228_750, 351_250, 511_250, 713_750, 963_750
    };

    // Cumulative XP needed to reach the given level.
    public static long XPToReach(int level)
    {
        if (level <= 0)
        {
            return 0;
        }

        if (level > MaxLevel)
        {
            level = MaxLevel;
        }

        return _cumulativeXP[level - 1];
    }

    // Level for a cumulative XP total, clamped to 0..MaxLevel.
    public static int LevelForXP(long xp)
    {
        for (var level = 1; level <= MaxLevel; level++)
        {
            if (xp < XPToReach(level))
            {
                return level - 1;
            }
        }

        return MaxLevel;
    }

    // StatCap at a given level: [100,150,200,250,300][Min(level, 4)].
    // L0 -> 100 (fresh char), L1 -> 150 ... L4+ -> 300. This lets passive stat
    // training run ahead of the level-up top-up.
    public static int StatCapFor(int level)
    {
        if (level < 0)
        {
            level = 0;
        }

        return _statThresholds[Math.Min(level, 4)];
    }

    // Top-up target total when reaching level 1..5. threshold(1) = 100 .. threshold(5) = 300.
    public static int StatThreshold(int level) => _statThresholds[Math.Clamp(level, 1, 5) - 1];

    // Per-skill cap for a level. Levels 0..5 use the table; level > 5 stays at 100.0.
    public static double SkillCapFor(int level)
    {
        if (level < 0)
        {
            level = 0;
        }

        return level <= 5 ? _skillCaps[level] : 100.0;
    }

    // Multiplier applied to a receiving player's XP share, keyed on gap = mobLevel - playerLevel.
    public static double GapMultiplier(int mobLevel, int playerLevel)
    {
        var gap = mobLevel - playerLevel;

        return gap switch
        {
            <= -3 => 0.0,
            -2    => 0.25,
            -1    => 0.5,
            0     => 1.0,
            1     => 1.25,
            _     => 1.5 // gap >= +2
        };
    }

    // Overhead-label hue for a mob's [lvl N] tag as seen by a player, keyed on the same
    // gap brackets as GapMultiplier. Level 0 mobs award no XP, so they always read gray.
    // Hues are placeholders to tune in-game (same spirit as the loot bag hue table).
    public static int GapHue(int mobLevel, int playerLevel)
    {
        if (mobLevel <= 0)
        {
            return 0x3B2; // gray — always 0 XP
        }

        return (mobLevel - playerLevel) switch
        {
            <= -3 => 0x3B2, // gray: trivial, 0x XP
            -2 or -1 => 0x3F,  // green: easy, reduced XP
            0 => 0x481, // white: even
            1 => 0x35,  // yellow: tough, 1.25x
            _ => 0x22   // red: danger, 1.5x (gap >= +2)
        };
    }

    // v2 mob level from real (post-T2A-scaling) max hit points. Minimum HP level is 1;
    // level 0 exists only via MobLevelOverrides pins (ambient/farm creatures).
    public static int MobLevelFromHits(int hitsMax) =>
        hitsMax switch
        {
            <= 65   => 1,  // mongbat, giant rat, slime, headless
            <= 100  => 2,  // zombie, skeleton, wolves
            <= 160  => 3,  // orc, ratman, low elementals
            <= 240  => 4,  // ogre, troll, lich
            <= 380  => 5,  // ore elementals, elder gazer, efreet
            <= 550  => 6,  // drake, daemon
            <= 720  => 7,  // titan, blood elemental, phoenix
            <= 950  => 8,  // dragon, white wyrm, ogre lord
            <= 2400 => 9,  // balron, ancient wyrm, hydra
            _       => 10  // future custom bosses, champion-tier
        };

    // Override table wins first; otherwise fall back to the HP heuristic.
    public static int GetMobLevel(BaseCreature bc)
    {
        if (bc == null)
        {
            return 1;
        }

        if (MobLevelOverrides.TryGetValue(bc.GetType(), out var level))
        {
            return level;
        }

        return MobLevelFromHits(bc.HitsMax);
    }

    // Base XP a mob is worth on death before per-player gap scaling.
    public static int BaseMobXP(int mobLevel) => mobLevel * 100;

    // Pure top-up distribution. Given the three current stat values, their "is locked Up"
    // flags, and the number of points to hand out, returns how many points each stat gains.
    //
    // Rules: distribute one point at a time, round-robin Str -> Dex -> Int, into stats whose
    // lock is Up and are below PerStatCap. If no stat is Up, all three are candidates. If every
    // candidate reaches PerStatCap with points left, spill into the remaining stats still below
    // the cap. delta <= 0 hands out nothing.
    public static (int strInc, int dexInc, int intInc) DistributeTopUp(
        int str, int dex, int intel,
        bool strUp, bool dexUp, bool intUp,
        int delta
    )
    {
        if (delta <= 0)
        {
            return (0, 0, 0);
        }

        // Explicit initializers matter: this assembly builds with SkipLocalsInit, so a bare
        // stackalloc is not zeroed. increments accumulates, so it must start at zero.
        Span<int> values = stackalloc int[3] { str, dex, intel };
        Span<int> increments = stackalloc int[3] { 0, 0, 0 };
        Span<bool> candidates = stackalloc bool[3] { strUp, dexUp, intUp };

        // No stat marked Up -> all three are eligible.
        if (!strUp && !dexUp && !intUp)
        {
            candidates[0] = true;
            candidates[1] = true;
            candidates[2] = true;
        }

        // Fill the candidate stats first, then spill the remainder into the rest.
        delta = FillRoundRobin(values, increments, candidates, delta);

        if (delta > 0)
        {
            Span<bool> spill = stackalloc bool[3];
            spill[0] = !candidates[0];
            spill[1] = !candidates[1];
            spill[2] = !candidates[2];
            FillRoundRobin(values, increments, spill, delta);
        }

        return (increments[0], increments[1], increments[2]);
    }

    // Round-robin one point at a time over eligible stats until delta runs out or every
    // eligible stat sits at PerStatCap. Returns the leftover delta.
    private static int FillRoundRobin(Span<int> values, Span<int> increments, ReadOnlySpan<bool> eligible, int delta)
    {
        while (delta > 0)
        {
            var placed = false;

            for (var i = 0; i < 3; i++)
            {
                if (delta <= 0)
                {
                    break;
                }

                if (eligible[i] && values[i] < PerStatCap)
                {
                    values[i]++;
                    increments[i]++;
                    delta--;
                    placed = true;
                }
            }

            if (!placed)
            {
                break;
            }
        }

        return delta;
    }
}
