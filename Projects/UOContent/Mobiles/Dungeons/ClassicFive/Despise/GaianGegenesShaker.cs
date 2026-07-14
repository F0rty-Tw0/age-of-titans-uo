using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gegenes sub-faction. L5. Donor: EarthElemental.
[SerializationGenerator(0, false)]
public partial class GaianGegenesShaker : BaseCreature
{
    [Constructible]
    public GaianGegenesShaker() : base(AIType.AI_Melee)
    {
        Body = 14;
        Hue = 0x0455;
        BaseSoundID = 268;

        SetStr(240, 270);
        SetDex(40, 58);
        SetInt(35, 55);

        SetHits(330, 360);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 3900;
        Karma = -3900;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a gegenes earth-shaker's corpse";
    public override string DefaultName => "a gegenes earth-shaker";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    // Tremor: 20% chance to stagger the defender and sap stamina. Knockback is a flavor
    // message only - no position change (no knockback primitive in this codebase).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The tremor knocks you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
