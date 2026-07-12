using System.Collections.Generic;

namespace Server.Engines.Rarity;

// The pantheon domain a variant root belongs to (framework §3 myth column). Public because
// Patron God devotion (WornEffectState) keys off it: 3+ worn legendaries of one domain pledge
// the wearer to that god.
public enum PantheonDomain : byte
{
    Sky,        // Zeus / storm — tinted bolt + thunder
    War,        // Ares / wrath / the hoplite line — explosion burst
    Hunt,       // Artemis / venom — poison-green pierce
    Aegis,      // Athena / wards and walls — blessing shimmer
    Underworld, // Hades / grave lines — curse mist
    Sea,        // Poseidon — blue surge
    Forge,      // Hephaestus / hearth — flame column
    Sun,        // Apollo / Nike / Tyche — radiant sparkle
    Wind,       // Hermes / the winds — grey rush
    Night,      // Nyx / Hecate / shadow lines — violet gloom
    Nature      // nymphs / Demeter / Aphrodite — green bloom
}

// Per-pantheon proc flourish: when a legendary's unique clause fires, its god answers with a
// signature particle burst + sound. The SHAPE and SOUND come from the root's pantheon domain
// (Zeus answers with a bolt, Hades with grave-mist, Hephaestus with forge-fire); the TINT is the
// root's own Legendary body shade, so every root reads distinct while its domain stays
// recognizable at a glance. Weapon procs flash on the struck target, armor/shield/dodge procs on
// the wearer. A wearer DEVOTED to the proccing root's domain (Patron God, WornEffectState) gets
// a sparkle crown on top of the domain effect. Particle/sound ids are the classic-client pairs
// already used by the magery spells in this repo (verified against Spells/*, 2026-07-11); exact
// in-client look flagged for the hue pass, same as the body-hue placeholders.
public static class PantheonFx
{
    // One flourish per mobile per window — a crit-splash-extra-swing chain must read as ONE
    // divine answer, not a strobe. Lazy-expiry tick map, evicted via RarityEffects.OnMobileGone.
    private const long ThrottleMs = 600;

    private static readonly Dictionary<Mobile, long> _lastFx = new();

    // Weapon legendary proc: the god strikes THROUGH the wielder — flourish on the struck target,
    // boosted when the WIELDER is devoted to the proccing domain.
    public static void PlayWeaponProc(Mobile source, Mobile target, VariantRoot root) =>
        Play(target, root, IsDevotedTo(source, root));

    // Armor/shield/jewelry legendary proc: the god shields the wearer — flourish on the wearer.
    public static void PlayWornProc(Mobile wearer, VariantRoot root) =>
        Play(wearer, root, IsDevotedTo(wearer, root));

    private static bool IsDevotedTo(Mobile m, VariantRoot root) =>
        WornEffectState.GetAggregate(m).IsDevotedTo(GetDomain(root));

    private static void Play(Mobile on, VariantRoot root, bool devoted)
    {
        if (on?.Map == null || on.Map == Map.Internal || root == VariantRoot.None)
        {
            return;
        }

        var now = Core.TickCount;

        if (_lastFx.TryGetValue(on, out var last) && now - last < ThrottleMs)
        {
            return;
        }

        _lastFx[on] = now;

        PlayForDomain(on, GetDomain(root), VariantRootInfo.GetBodyHue(root, ItemRarity.Legendary), devoted);
    }

    // Unthrottled domain playback — the production path above throttles before calling this;
    // the [PantheonFxTest GM preview drives it directly.
    internal static void PlayForDomain(Mobile on, PantheonDomain domain, int hue, bool devoted)
    {
        if (on?.Map == null || on.Map == Map.Internal)
        {
            return;
        }

        // Devotion crown: the patron answers louder for the pledged — an extra hued sparkle
        // above the domain effect (no extra sound; the domain's own sound carries it).
        if (devoted)
        {
            on.FixedParticles(0x375A, 10, 30, 5010, hue, 0, EffectLayer.Head);
        }

        switch (domain)
        {
            case PantheonDomain.Sky:
                {
                    on.BoltEffect(hue);
                    on.PlaySound(0x29);
                    break;
                }
            case PantheonDomain.War:
                {
                    on.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    on.PlaySound(0x307);
                    break;
                }
            case PantheonDomain.Hunt:
                {
                    on.FixedParticles(0x374A, 10, 15, 5021, hue, 0, EffectLayer.Waist);
                    on.PlaySound(0x205);
                    break;
                }
            case PantheonDomain.Aegis:
                {
                    on.FixedParticles(0x373A, 10, 15, 5018, hue, 0, EffectLayer.Waist);
                    on.PlaySound(0x1EA);
                    break;
                }
            case PantheonDomain.Underworld:
                {
                    on.FixedParticles(0x374A, 10, 15, 5028, hue, 0, EffectLayer.Waist);
                    on.PlaySound(0x1E1);
                    break;
                }
            case PantheonDomain.Sea:
                {
                    on.FixedParticles(0x375A, 10, 15, 5037, hue, 0, EffectLayer.Waist);
                    on.PlaySound(0x213);
                    break;
                }
            case PantheonDomain.Forge:
                {
                    on.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    on.PlaySound(0x208);
                    break;
                }
            case PantheonDomain.Sun:
                {
                    on.FixedParticles(0x375A, 9, 20, 5016, hue, 0, EffectLayer.Waist);
                    on.PlaySound(0x1F2);
                    break;
                }
            case PantheonDomain.Wind:
                {
                    on.FixedParticles(0x3728, 10, 13, 2023, hue, 0, EffectLayer.Waist);
                    on.PlaySound(0x5C);
                    break;
                }
            case PantheonDomain.Night:
                {
                    on.FixedParticles(0x374A, 10, 15, 5038, hue, 0, EffectLayer.Head);
                    on.PlaySound(0x1FB);
                    break;
                }
            case PantheonDomain.Nature:
                {
                    on.FixedParticles(0x376A, 9, 32, 5030, hue, 0, EffectLayer.Waist);
                    on.PlaySound(0x202);
                    break;
                }
        }
    }

    // Root -> pantheon domain (framework §3 myth column). Every claimed root maps explicitly;
    // the default only catches None/future roots (Athena's shimmer is the least-wrong fallback).
    // Internal: WornEffectState counts worn legendaries per domain for Patron God devotion.
    internal static PantheonDomain GetDomain(VariantRoot root) => root switch
    {
        // Zeus and the storm-sky
        VariantRoot.Olympian or VariantRoot.Kataigis or VariantRoot.Prester => PantheonDomain.Sky,

        // Ares, wrath, blood, and the hoplite line
        VariantRoot.Phobos or VariantRoot.Menis or VariantRoot.Aristeia or VariantRoot.Haima
            or VariantRoot.Ephodos or VariantRoot.Belos or VariantRoot.Sarisa or VariantRoot.Hoplites
            or VariantRoot.Taxis or VariantRoot.Zoster or VariantRoot.Alkimos or VariantRoot.Maenad
            or VariantRoot.Amyntor => PantheonDomain.War,

        // Artemis, venom, and the hunt
        VariantRoot.Agrotera or VariantRoot.Ios or VariantRoot.Kentron or VariantRoot.Toxikon
            or VariantRoot.Pede or VariantRoot.Skopos or VariantRoot.Kynegis or VariantRoot.Batos
            or VariantRoot.Arkas or VariantRoot.Elaphis => PantheonDomain.Hunt,

        // Athena — wards, walls, and the watchful line
        VariantRoot.Pallas or VariantRoot.Areia or VariantRoot.Phalanx or VariantRoot.Eryma
            or VariantRoot.Aegis or VariantRoot.Herkos or VariantRoot.Phylax or VariantRoot.Egregoros
            or VariantRoot.Teichos or VariantRoot.Phrourion or VariantRoot.Polias
            or VariantRoot.Alexikakos => PantheonDomain.Aegis,

        // Hades and the grave lines
        VariantRoot.Stygian or VariantRoot.Zophos or VariantRoot.Theristes or VariantRoot.Kamatos
            or VariantRoot.Melinoe or VariantRoot.Makaria or VariantRoot.Tymbos or VariantRoot.Nekyia
            or VariantRoot.Katachthon => PantheonDomain.Underworld,

        // Poseidon and the breakwater
        VariantRoot.Ennosigaios or VariantRoot.Tritonian or VariantRoot.Probolos => PantheonDomain.Sea,

        // Hephaestus, the forge, and the hearth
        VariantRoot.Rhaistes or VariantRoot.Cyclopean or VariantRoot.Adamas or VariantRoot.Kaminos
            or VariantRoot.Kolossos or VariantRoot.Panoplia or VariantRoot.Akamatos
            or VariantRoot.Halysis or VariantRoot.Hestian => PantheonDomain.Forge,

        // Apollo the radiant, Nike, and Tyche
        VariantRoot.Phoibos or VariantRoot.Hekatos or VariantRoot.Manteia or VariantRoot.Paean
            or VariantRoot.Laurel or VariantRoot.Tychean => PantheonDomain.Sun,

        // Hermes and the winds
        VariantRoot.Zephyr or VariantRoot.Aiolos or VariantRoot.Horme or VariantRoot.Talarian
            or VariantRoot.Pnoe or VariantRoot.Dromos => PantheonDomain.Wind,

        // Nyx, Hecate, and the shadow lines
        VariantRoot.Nyxian or VariantRoot.Hecatean or VariantRoot.Skia or VariantRoot.Ophis
            or VariantRoot.Empousa or VariantRoot.Baskania or VariantRoot.Arachne => PantheonDomain.Night,

        // The nymphs, Demeter, and Aphrodite
        VariantRoot.Naias or VariantRoot.Dryas or VariantRoot.Oreias or VariantRoot.Melissa
            or VariantRoot.Panika or VariantRoot.Demetrian or VariantRoot.Charis => PantheonDomain.Nature,

        _ => PantheonDomain.Aegis
    };

    // ---- Patron God devotion metadata (consumed by WornEffectState.Rebuild) ------------------

    // The god who answers a devoted suit — buff-bar label text.
    internal static string GetPatronName(PantheonDomain domain) => domain switch
    {
        PantheonDomain.Sky        => "Zeus",
        PantheonDomain.War        => "Ares",
        PantheonDomain.Hunt       => "Artemis",
        PantheonDomain.Aegis      => "Athena",
        PantheonDomain.Underworld => "Hades",
        PantheonDomain.Sea        => "Poseidon",
        PantheonDomain.Forge      => "Hephaestus",
        PantheonDomain.Sun        => "Apollo",
        PantheonDomain.Wind       => "Hermes",
        PantheonDomain.Night      => "Nyx",
        _                         => "Demeter"
    };

    // Devotion perk category: 0 = offense (+damage), 1 = defense (+DR into the capped pool),
    // 2 = utility (+regen into the pools). Same magnitudes as one Divine Resonance echo.
    internal static int GetDomainCategory(PantheonDomain domain) => domain switch
    {
        PantheonDomain.War or PantheonDomain.Hunt or PantheonDomain.Sky or PantheonDomain.Sun => 0,
        PantheonDomain.Aegis or PantheonDomain.Forge or PantheonDomain.Sea                    => 1,
        _                                                                                     => 2
    };

    internal static string GetPerkText(PantheonDomain domain) => GetDomainCategory(domain) switch
    {
        0 => "+4% damage",
        1 => "+2% damage reduction",
        _ => "+10% regen"
    };

    // A representative tint for a domain (first claimed root that maps to it) — used by the
    // [PantheonFxTest preview so each domain shows with a realistic hue. Cold path only.
    internal static int SampleHue(PantheonDomain domain)
    {
        for (var i = 1; i < VariantRootInfo.RootCount; i++)
        {
            var root = (VariantRoot)i;

            if (GetDomain(root) == domain)
            {
                return VariantRootInfo.GetBodyHue(root, ItemRarity.Legendary);
            }
        }

        return 0;
    }

    public static void Evict(Mobile m)
    {
        if (m != null)
        {
            _lastFx.Remove(m);
        }
    }
}
