using Server.Items;

namespace Server.Engines.Rarity;

// Option A milestone (armor-slot-set-design.md "FINAL MATRIX"): Epic/Legendary armor Signature
// clauses re-keyed from per-root to per-(material x slot). Shields are excluded — they stay on
// the existing root-keyed ArmorEffectTable.Signature lookup (single slot, no per-slot theme).
// Every live cell below is a globally-unique clause; cells with no entry (out-of-scope
// materials/slots) resolve to ClauseType.None via Get's default-tuple fallback.
public static class ArmorSlotSignatureTable
{
    internal const int MaterialCount = 13; // ArmorMaterialType.Cloth..Stone (also sizes the P4 set counter)
    private const int SlotCount = 7;      // ArmorBodyType.Gorget..Shield

    private static readonly (ClauseType Signature, short S1, short S2, short S3)[,] _cells =
        new (ClauseType, short, short, short)[MaterialCount, SlotCount];

    static ArmorSlotSignatureTable()
    {
        // ---- Leather — "the nymph's hide" ----
        Set(ArmorMaterialType.Leather, ArmorBodyType.Helmet, ClauseType.SpellDrBoostFirstHit, 10);
        Set(ArmorMaterialType.Leather, ArmorBodyType.Gorget, ClauseType.RerollFirstResist);
        Set(ArmorMaterialType.Leather, ArmorBodyType.Chest, ClauseType.FirstHitNoSecondaryEffect);
        Set(ArmorMaterialType.Leather, ArmorBodyType.Arms, ClauseType.ReflectBoostFirstHit, 10);
        Set(ArmorMaterialType.Leather, ArmorBodyType.Gloves, ClauseType.DodgeGrantsCounterWindow);
        Set(ArmorMaterialType.Leather, ArmorBodyType.Legs, ClauseType.DodgeRegenBurst, 20, 5);

        // ---- Studded — "the hunter's brand" ----
        Set(ArmorMaterialType.Studded, ArmorBodyType.Helmet, ClauseType.SpellDrVsPoisonDot); // StuddedMempo only
        Set(ArmorMaterialType.Studded, ArmorBodyType.Gorget, ClauseType.ParaResistStunsAttacker);
        Set(ArmorMaterialType.Studded, ArmorBodyType.Chest, ClauseType.ShrugFirstHitPoisonAttacker);
        Set(ArmorMaterialType.Studded, ArmorBodyType.Arms, ClauseType.ShrugReflect, 10);
        Set(ArmorMaterialType.Studded, ArmorBodyType.Gloves, ClauseType.OnKillDodgeDoubleDuration, 5);
        Set(ArmorMaterialType.Studded, ArmorBodyType.Legs, ClauseType.StamRegenMirrorsHp);

        // ---- Bone — "the grave-warden" ----
        Set(ArmorMaterialType.Bone, ArmorBodyType.Helmet, ClauseType.SpellDrBurstOnCritTaken, 3);
        Set(ArmorMaterialType.Bone, ArmorBodyType.Chest, ClauseType.ShrugFirstHitDrainStam, 5);
        Set(ArmorMaterialType.Bone, ArmorBodyType.Arms, ClauseType.ReflectCritStun);
        Set(ArmorMaterialType.Bone, ArmorBodyType.Gloves, ClauseType.OnKillRestoreMissingHpPct, 50);
        Set(ArmorMaterialType.Bone, ArmorBodyType.Legs, ClauseType.OnKillRestoreHpPct, 10);

        // ---- Ringmail ----
        Set(ArmorMaterialType.Ringmail, ArmorBodyType.Chest, ClauseType.ShrugFirstHitDrBurst, 8, 3);
        Set(ArmorMaterialType.Ringmail, ArmorBodyType.Arms, ClauseType.ShrugStunAttacker);
        Set(ArmorMaterialType.Ringmail, ArmorBodyType.Gloves, ClauseType.OnKillStamRestoreExtendImmunity, 5);
        Set(ArmorMaterialType.Ringmail, ArmorBodyType.Legs, ClauseType.StamRegenMirrorsManaHalf);

        // ---- Chainmail — only 3 live slots (Helmet/Chest/Legs) ----
        Set(ArmorMaterialType.Chainmail, ArmorBodyType.Helmet, ClauseType.ParaResistBoostsSpellDr, 15, 5);
        Set(ArmorMaterialType.Chainmail, ArmorBodyType.Chest, ClauseType.EmergencyRegenTick, 10);
        Set(ArmorMaterialType.Chainmail, ArmorBodyType.Legs, ClauseType.HpRegenBurstOnCritTaken, 5);

        // ---- Plate — "the forged colossus" ----
        Set(ArmorMaterialType.Plate, ArmorBodyType.Helmet, ClauseType.ParaResistBoostsResistSkill, 5, 5);
        Set(ArmorMaterialType.Plate, ArmorBodyType.Gorget, ClauseType.FirstParaAutoFails);
        Set(ArmorMaterialType.Plate, ArmorBodyType.Chest, ClauseType.ShrugFirstHitGuaranteed);
        Set(ArmorMaterialType.Plate, ArmorBodyType.Arms, ClauseType.ShrugReflectStun, 10);
        Set(ArmorMaterialType.Plate, ArmorBodyType.Gloves, ClauseType.HealBlockOnFirstHitLanded, 3);
        Set(ArmorMaterialType.Plate, ArmorBodyType.Legs, ClauseType.OnKillRestoreExtraHp, 15);
    }

    private static void Set(
        ArmorMaterialType material, ArmorBodyType slot, ClauseType signature,
        short s1 = 0, short s2 = 0, short s3 = 0
    ) =>
        _cells[(int)material, (int)slot] = (signature, s1, s2, s3);

    // Zero-allocation lookup. Any material/slot pair with no entry above (out-of-scope material,
    // shield slot, or an un-populated cell) returns the default None-tuple.
    public static (ClauseType Signature, short S1, short S2, short S3) Get(ArmorMaterialType material, ArmorBodyType slot)
    {
        if ((int)material < 0 || (int)material >= MaterialCount || (int)slot < 0 || (int)slot >= SlotCount)
        {
            return default;
        }

        return _cells[(int)material, (int)slot];
    }
}
