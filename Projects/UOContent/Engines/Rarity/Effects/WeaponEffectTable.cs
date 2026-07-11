namespace Server.Engines.Rarity;

// One row of weapon theme magnitudes for a given [root, rarity].
// Percent fields are whole percents (8 = +8%). Zero-valued = effect absent.
public readonly struct WeaponEffectRow
{
    // Zephyr (speed)
    public int SwingSpeedPct { get; init; }
    public int HitChancePct { get; init; }
    public int ExtraSwingPct { get; init; }

    // Phobos (damage)
    public int DamagePct { get; init; }
    public int CritChancePct { get; init; }
    public int CritDamagePct { get; init; }

    // Agrotera (mark)
    public int MarkChancePct { get; init; }
    public int MarkBonusPct { get; init; }
    public bool MarkPoisonTick { get; init; }

    // Pallas (defense)
    public int BlockPct { get; init; }
    public int BlockDrPct { get; init; }
    public bool BlockThorns { get; init; }

    // Stygian (drain)
    public int LifestealPct { get; init; }
    public int StamRegenPct { get; init; }
    public bool LifestealExecute { get; init; } // x2 lifesteal vs targets under 30% HP

    // ---- P2 re-theme: always-on numeric lane fields (populated by later data phases) -----
    public int SplashPct { get; init; }              // EndWeaponHit -> Splash()
    public int ArmorPenPct { get; init; }            // P27: scales absorbed AR by (100-pen)/100
    public int StaggerProcPct { get; init; }         // % chance to stun the target 1s (§9.2 caps)
    public int PoisonApplyPct { get; init; }         // % chance to poison on hit
    public int PoisonTier { get; init; }             // 0 = Lesser, 1 = Regular
    public int ManaLeechPct { get; init; }           // % of the target's mana leeched on hit
    public int ElementalProcPct { get; init; }       // % chance for an elemental proc
    public int ElementalKind { get; init; }          // 0 = lightning, 1 = fire
    public int HealBlockProcPct { get; init; }       // % chance to heal-block the target (3s cap)
    public int NthHitBonusPct { get; init; }         // +dmg% on every NthHitN hit
    public int NthHitN { get; init; }
    public int RampPerStackPct { get; init; }        // +dmg% per consecutive same-target hit (P28)
    public int RampMaxStacks { get; init; }
    public int FirstHitBonusPct { get; init; }       // +dmg% on the first hit of a fight
    public int OnKillStamPct { get; init; }          // on-kill: restore this % of max stamina
    public int DefenderStamDrainFlat { get; init; }  // flat stamina drained from the target on hit

    // ---- P2 re-theme: worn-side utility (folded into WornEffectState when the weapon is held) --
    public int SpellDrPct { get; init; }
    public int ManaRegenPct { get; init; }
    public int DodgePct { get; init; }
    public int ResistSkillBonus { get; init; }
    public int HealsReceivedPct { get; init; } // Manteia staff lane: heals-received while held
    public bool AutoCure { get; init; }         // Manteia staff lane: periodic auto-cure tick while held

    // ---- P2 re-theme: per-root Epic signature (event-gated proc, engine-dispatched) -----
    public ClauseType Signature { get; init; }
    public short S1 { get; init; }
    public short S2 { get; init; }
    public short S3 { get; init; }

    public bool IsEmpty => this is
    {
        SwingSpeedPct: 0, HitChancePct: 0, ExtraSwingPct: 0,
        DamagePct: 0, CritChancePct: 0, CritDamagePct: 0,
        MarkChancePct: 0, MarkBonusPct: 0, MarkPoisonTick: false,
        BlockPct: 0, BlockDrPct: 0, BlockThorns: false,
        LifestealPct: 0, StamRegenPct: 0, LifestealExecute: false,
        SplashPct: 0, ArmorPenPct: 0, StaggerProcPct: 0, PoisonApplyPct: 0, PoisonTier: 0,
        ManaLeechPct: 0, ElementalProcPct: 0, ElementalKind: 0, HealBlockProcPct: 0,
        NthHitBonusPct: 0, NthHitN: 0, RampPerStackPct: 0, RampMaxStacks: 0, FirstHitBonusPct: 0,
        OnKillStamPct: 0, DefenderStamDrainFlat: 0,
        SpellDrPct: 0, ManaRegenPct: 0, DodgePct: 0, ResistSkillBonus: 0,
        HealsReceivedPct: 0, AutoCure: false,
        Signature: ClauseType.None, S1: 0, S2: 0, S3: 0
    };
}

// Array-backed facade over the family registry (the single source of truth — the per-family
// magnitudes now live in Families/*.cs, e.g. AxesFamily/SwordsFamily). Populated once at type init
// from FamilyRegistry.WeaponRows; zero allocation per lookup.
public static class WeaponEffectTable
{
    private static readonly WeaponEffectRow[,] _rows = FamilyRegistry.WeaponRows;

    // Zero-allocation lookup. Returns an all-zero row for roots/rarities with no package.
    public static WeaponEffectRow Get(VariantRoot root, ItemRarity rarity)
    {
        if (root == VariantRoot.None)
        {
            return default;
        }

        return _rows[(int)root, (int)rarity];
    }
}
