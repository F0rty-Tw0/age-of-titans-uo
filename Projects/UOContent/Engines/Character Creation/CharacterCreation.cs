using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ModernUO.CodeGeneratedEvents;
using Server.Accounting;
using Server.Items;
using Server.Logging;
using Server.Maps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.CharacterCreation;

public static partial class CharacterCreation
{
    private static readonly ILogger logger = LogFactory.GetLogger(typeof(CharacterCreation));

    // ponytail: one fixed spawn — client still renders the city page, server sends/uses only this
    public static readonly CityInfo StartingCity =
        new("Britain", "Sweet Dreams Inn", 1075074, 1496, 1628, 10, Map.Felucca);

    private static readonly CityInfo[] _startingCities = [StartingCity];

    public static CityInfo[] GetStartingCities() => _startingCities;

    [GeneratedEvent(nameof(CharacterCreatedEvent))]
    public static partial void CharacterCreatedEvent(CharacterCreatedEventArgs e);

    private static void AddBackpack(this Mobile m)
    {
        var pack = m.Backpack;

        if (pack == null)
        {
            pack = new Backpack();
            pack.Movable = false;

            m.AddItem(pack);
        }

        m.PackItem(new RedBook("a book", m.Name, 20, true));
        m.PackItem(new Gold(1000)); // Starting gold can be customized here
        m.PackItem(new Dagger());
        m.PackItem(new Candle());
    }

    private static Mobile CreateMobile(Account a)
    {
        if (a.Count >= a.Limit)
        {
            return null;
        }

        for (var i = 0; i < a.Length; ++i)
        {
            if (a[i] == null)
            {
                return a[i] = new PlayerMobile();
            }
        }

        return null;
    }

    [OnEvent(nameof(CharacterCreatedEvent))]
    private static void OnCharacterCreated(CharacterCreatedEventArgs args)
    {
        var state = args.State;

        if (state == null)
        {
            return;
        }

        var newChar = CreateMobile(args.Account as Account);

        if (newChar == null)
        {
            logger.Information("Login: {NetState}: Character creation failed, account full", state);
            return;
        }

        args.Mobile = newChar;

        newChar.Player = true;
        newChar.AccessLevel = args.Account.AccessLevel;
        newChar.Female = args.Female;
        newChar.Hue = newChar.Race.ClipSkinHue(args.Hue & 0x3FFF) | 0x8000;
        newChar.Hunger = 20;

        SetName(newChar, args.Name);
        newChar.AddBackpack();

        if (newChar.AccessLevel == AccessLevel.Player)
        {
            // Race, profession, stats, skills, and city picks from the client are ignored by design.
            var race = Race.DefaultRace;
            newChar.Race = race;

            if (newChar is PlayerMobile pm)
            {
                if (((Account)pm.Account).Young)
                {
                    pm.Young = true;

                    newChar.BankBox.DropItem(new NewPlayerTicket
                    {
                        Owner = newChar
                    });
                }
            }

            newChar.InitStats(30, 25, 25); // 80 total; L1 top-up brings it to 100

            // Start "vendor-trained": every skill at 30.0 so players skip NPC training.
            // The default 700.0 total cap would then block all gains (SkillCheck requires
            // Total < Cap), so raise it out of the way — per-skill caps from
            // LevelSystem.ApplyCaps are this shard's real limiter.
            for (var i = 0; i < newChar.Skills.Length; i++)
            {
                newChar.Skills[i].BaseFixedPoint = 300;
            }

            newChar.SkillsCap = newChar.Skills.Length * 1000;

            GiveProfessionItems(newChar, null, args.ShirtHue, args.PantsHue);

            if (race.ValidateHair(newChar, args.HairID))
            {
                newChar.HairItemID = args.HairID;
                newChar.HairHue = race.ClipHairHue(args.HairHue & 0x3FFF);
            }

            if (race.ValidateFacialHair(newChar, args.BeardID))
            {
                newChar.FacialHairItemID = args.BeardID;
                newChar.FacialHairHue = race.ClipHairHue(args.BeardHue & 0x3FFF);
            }

            if (TestCenter.Enabled)
            {
                TestCenter.FillBankbox(newChar);
            }
        }
        else
        {
            newChar.Str = 100;
            newChar.Int = 100;
            newChar.Dex = 100;

            for (var i = 0; i < newChar.Skills.Length; i++)
            {
                newChar.Skills[i].BaseFixedPoint = 1000;
            }

            newChar.Race = Race.Human;
            newChar.Blessed = true;
            newChar.AddItem(new StaffRobe(newChar.AccessLevel));
        }

        var city = GetStartLocation(args);
        newChar.MoveToWorld(city.Location, city.Map);

        logger.Information(
            "Login: {0}: New character being created (account={1}, character={2}, serial={3}, started.city={4}, started.location={5}, started.map={6})",
            state,
            args.Account.Username,
            newChar.Name,
            newChar.Serial,
            city.City,
            city.Location,
            city.Map);

        new WelcomeTimer(newChar).Start();
    }

    private static CityInfo GetStartLocation(CharacterCreatedEventArgs args)
    {
        if (args.Mobile.AccessLevel > AccessLevel.Player &&
            ExpansionInfo.CoreExpansion.MapSelectionFlags.Includes(MapSelectionFlags.Felucca))
        {
            return new CityInfo("Green Acres", "Green Acres", 5445, 1153, 0, Map.Felucca);
        }

        return StartingCity;
    }

    private static void SetName(Mobile m, string name)
    {
        name = name.Trim();

        if (!NameVerification.ValidatePlayerName(name))
        {
            name = "Generic Player";
        }

        m.Name = name;
    }

    private static void GiveProfessionItems(Mobile m, ProfessionInfo profession, int shirtHue, int pantsHue)
    {
        var elf = m.Race == Race.Elf;
        var gargoyle = m.Race == Race.Gargoyle;

        switch (profession?.Name.ToLowerInvariant())
        {
            case "necromancer":
                {
                    Container regs = new BagOfNecroReagents { LootType = LootType.Regular };

                    if (!Core.AOS)
                    {
                        foreach (var item in regs.Items)
                        {
                            item.LootType = LootType.Newbied;
                        }
                    }

                    m.PackItem(regs);

                    EquipItem(m, new BoneHelm());

                    if (elf)
                    {
                        EquipItem(m, new ElvenMachete());
                        EquipItem(m, NecroHue(new LeafChest()));
                        EquipItem(m, NecroHue(new LeafArms()));
                        EquipItem(m, NecroHue(new LeafGloves()));
                        EquipItem(m, NecroHue(new LeafGorget()));
                        EquipItem(m, NecroHue(new LeafLegs()));
                        EquipItem(m, new ElvenBoots());
                    }
                    else if (gargoyle)
                    {
                        EquipItem(m, new GlassSword());
                        EquipItem(m, NecroHue(m.Female ? new GargishLeatherChestType2() : new GargishLeatherChestType1()));
                        EquipItem(m, NecroHue(m.Female ? new GargishLeatherArmsType2() : new GargishLeatherArmsType1()));
                        EquipItem(m, NecroHue(m.Female ? new GargishLeatherKiltType2() : new GargishLeatherKiltType1()));
                        EquipItem(m, NecroHue(m.Female ? new GargishLeatherLegsType2() : new GargishLeatherLegsType1()));
                    }
                    else
                    {
                        EquipItem(m, new BoneHarvester());
                        EquipItem(m, NecroHue(new LeatherChest()));
                        EquipItem(m, NecroHue(new LeatherArms()));
                        EquipItem(m, NecroHue(new LeatherGloves()));
                        EquipItem(m, NecroHue(new LeatherGorget()));
                        EquipItem(m, NecroHue(new LeatherLegs()));
                        EquipItem(m, NecroHue(new Skirt()));
                        EquipItem(m, new Sandals(0x8FD));
                    }

                    // animate dead, evil omen, pain spike, summon familiar, wraith form
                    m.PackItem(new NecromancerSpellbook(0x8981ul));
                    return;
                }
            case "paladin":
                {
                    if (elf)
                    {
                        EquipItem(m, new ElvenMachete());
                        EquipItem(m, new WingedHelm());
                        EquipItem(m, new LeafGorget());
                        EquipItem(m, new LeafArms());
                        EquipItem(m, new LeafChest());
                        EquipItem(m, new LeafLegs());
                        EquipItem(m, new LeafGloves());
                        EquipItem(m, new ElvenBoots()); // Verify hue
                    }
                    else if (gargoyle)
                    {
                        EquipItem(m, new GlassSword());
                        EquipItem(m, m.Female ? new GargishStoneChestType2() : new GargishStoneChestType1());
                        EquipItem(m, m.Female ? new GargishStoneArmsType2() : new GargishStoneArmsType1());
                        EquipItem(m, m.Female ? new GargishStoneKiltType2() : new GargishStoneKiltType1());
                        EquipItem(m, m.Female ? new GargishStoneLegsType2() : new GargishStoneLegsType1());
                    }
                    else
                    {
                        EquipItem(m, new Broadsword());
                        EquipItem(m, new Helmet());
                        EquipItem(m, new PlateGorget());
                        EquipItem(m, new RingmailArms());
                        EquipItem(m, new RingmailChest());
                        EquipItem(m, new RingmailLegs());
                        EquipItem(m, new RingmailGloves());
                        EquipItem(m, new ThighBoots(0x748));
                        EquipItem(m, new Cloak(0xCF));
                        EquipItem(m, new BodySash(0xCF));
                    }

                    m.PackItem(new BookOfChivalry());
                    return;
                }
            case "samurai":
                {
                    if (elf)
                    {
                        EquipItem(m, new RavenHelm());
                        EquipItem(m, new HakamaShita(0x2C3));
                        EquipItem(m, new Hakama(0x2C3));
                        EquipItem(m, new SamuraiTabi(0x2C3));
                        EquipItem(m, new TattsukeHakama(0x22D));
                        EquipItem(m, new Bokuto());
                    }
                    else if (gargoyle)
                    {
                        EquipItem(m, new GargishTalwar());
                        EquipItem(m, m.Female ? new GargishLeatherChestType2() : new GargishLeatherChestType1());
                        EquipItem(m, m.Female ? new GargishLeatherArmsType2() : new GargishLeatherArmsType1());
                        EquipItem(m, m.Female ? new GargishLeatherKiltType2() : new GargishLeatherKiltType1());
                        EquipItem(m, m.Female ? new GargishLeatherLegsType2() : new GargishLeatherLegsType1());
                    }
                    else
                    {
                        EquipItem(m, new LeatherJingasa());
                        EquipItem(m, new HakamaShita(0x2C3));
                        EquipItem(m, new Hakama(0x2C3));
                        EquipItem(m, new SamuraiTabi(0x2C3));
                        EquipItem(m, new TattsukeHakama(0x22D));
                        EquipItem(m, new Bokuto());
                    }

                    m.PackItem(new Scissors());
                    m.PackItem(new Bandage(50));
                    m.PackItem(new BookOfBushido());

                    return;
                }
            case "ninja":
                {
                    ReadOnlySpan<int> hues = [0x1A8, 0xEC, 0x99, 0x90, 0xB5, 0x336, 0x89];
                    // TODO: Verify that's ALL the hues for that above.

                    if (elf)
                    {
                        EquipItem(m, new AssassinSpike());
                        EquipItem(m, new TattsukeHakama(hues.RandomElement()));
                        EquipItem(m, new HakamaShita(0x2C3));
                        EquipItem(m, new NinjaTabi(0x2C3));
                        EquipItem(m, new Kasa());
                    }
                    else if (gargoyle)
                    {
                        EquipItem(m, new DualPointedSpear());
                        EquipItem(m, m.Female ? new GargishLeatherChestType2() : new GargishLeatherChestType1());
                        EquipItem(m, m.Female ? new GargishLeatherArmsType2() : new GargishLeatherArmsType1());
                        EquipItem(m, m.Female ? new GargishLeatherKiltType2() : new GargishLeatherKiltType1());
                        EquipItem(m, m.Female ? new GargishLeatherLegsType2() : new GargishLeatherLegsType1());
                    }
                    else
                    {
                        EquipItem(m, new Tekagi());
                        EquipItem(m, new TattsukeHakama(hues.RandomElement()));
                        EquipItem(m, new HakamaShita(0x2C3));
                        EquipItem(m, new NinjaTabi(0x2C3));
                        EquipItem(m, new Kasa());
                    }

                    m.PackItem(new SmokeBomb());
                    m.PackItem(new SmokeBomb());
                    m.PackItem(new SmokeBomb());
                    m.PackItem(new SmokeBomb());
                    m.PackItem(new SmokeBomb());
                    m.PackItem(new BookOfNinjitsu());

                    return;
                }
            case "swordsman":
            case "fencer":
            case "warrior":
            case "mace fighter":
                {
                    if (elf)
                    {
                        EquipItem(m, new Circlet());
                        EquipItem(m, new HideGorget());
                        EquipItem(m, new HideChest());
                        EquipItem(m, new HidePauldrons());
                        EquipItem(m, new HideGloves());
                        EquipItem(m, new HidePants());
                        EquipItem(m, new ElvenBoots());
                    }
                    else if (gargoyle)
                    {
                        EquipItem(m, m.Female ? new GargishLeatherChestType2() : new GargishLeatherChestType1());
                        EquipItem(m, m.Female ? new GargishLeatherArmsType2() : new GargishLeatherArmsType1());
                        EquipItem(m, m.Female ? new GargishLeatherKiltType2() : new GargishLeatherKiltType1());
                        EquipItem(m, m.Female ? new GargishLeatherLegsType2() : new GargishLeatherLegsType1());
                    }
                    else
                    {
                        EquipItem(m, new Bascinet());
                        EquipItem(m, new StuddedGorget());
                        EquipItem(m, new StuddedChest());
                        EquipItem(m, new StuddedArms());
                        EquipItem(m, new StuddedGloves());
                        EquipItem(m, new StuddedLegs());
                        EquipItem(m, new ThighBoots());
                    }
                    break;
                }
        }

        m.AddShirt(shirtHue);
        m.AddPants(pantsHue);
        m.AddShoes();

        // All elves get a wild staff
        if (elf)
        {
            EquipItem(m, new WildStaff());
        }
    }

    private static void EquipItem(Mobile m, Item item, bool mustEquip = false)
    {
        if (item == null)
        {
            return;
        }

        if (!Core.AOS && item.LootType == LootType.Regular)
        {
            item.LootType = LootType.Newbied;
        }

        if (m?.EquipItem(item) == true)
        {
            return;
        }

        var pack = m?.Backpack;

        if (!mustEquip && pack != null)
        {
            pack.DropItem(item);
        }
        else
        {
            item.Delete();
        }
    }

    private static void PackItem(this Mobile m, Item item)
    {
        if (!Core.AOS && item.LootType == LootType.Regular)
        {
            item.LootType = LootType.Newbied;
        }

        var pack = m.Backpack;

        if (pack != null)
        {
            pack.DropItem(item);
        }
        else
        {
            item.Delete();
        }
    }

    private static void AddShirt(this Mobile m, int shirtHue)
    {
        var hue = Utility.ClipDyedHue(shirtHue & 0x3FFF);
        var raceFlag = m.Race.RaceFlag;

        var shirt = raceFlag switch
        {
            Race.AllowElvesOnly                   => new ElvenShirt(hue),
            Race.AllowGargoylesOnly when m.Female => new GargishClothChestType2 { Hue = hue },
            Race.AllowGargoylesOnly               => new GargishClothChestType1 { Hue = hue },
            // Humans
            _ => (Item)(Utility.Random(3) switch
            {
                0 => new Shirt(hue),
                1 => new FancyShirt(hue),
                _ => new Doublet(hue)
            })
        };

        EquipItem(m, shirt);
    }

    private static void AddPants(this Mobile m, int pantsHue)
    {
        var hue = Utility.ClipDyedHue(pantsHue & 0x3FFF);
        var raceFlag = m.Race.RaceFlag;
        var female = m.Female;

        var pants = raceFlag switch
        {
            Race.AllowElvesOnly                 => new ElvenPants(hue),
            Race.AllowGargoylesOnly when female => new GargishClothLegsType2 { Hue = hue },
            Race.AllowGargoylesOnly             => new GargishClothLegsType1 { Hue = hue },
            // Humans
            _ => (Item)(Utility.RandomBool() switch
            {
                true when female  => new Skirt(hue),
                true              => new LongPants(hue),
                false when female => new Kilt(hue),
                false             => new ShortPants(hue)
            })
        };

        EquipItem(m, pants);
    }

    private static void AddShoes(this Mobile m)
    {
        if (m.Race == Race.Elf)
        {
            EquipItem(m, new ElvenBoots());
        }
        else if (m.Race == Race.Human)
        {
            EquipItem(m, new Shoes(Utility.RandomYellowHue()));
        }
    }

    private static Item NecroHue(Item item)
    {
        item.Hue = 0x2C3;

        return item;
    }
}
