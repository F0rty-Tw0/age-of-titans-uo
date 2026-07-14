using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves, Leleges cave-clan. L4 trash. Donor: Orcish Mage.
[SerializationGenerator(0, false)]
public partial class PelasgLelexAugur : BaseCreature
{
    [Constructible]
    public PelasgLelexAugur() : base(AIType.AI_Mage)
    {
        Body = 140;
        Hue = 0x0798;
        BaseSoundID = 0x45A;

        SetStr(150, 175);
        SetDex(80, 100);
        SetInt(110, 135);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.EvalInt, 55.0, 68.0);
        SetSkill(SkillName.Magery, 55.0, 68.0);
        SetSkill(SkillName.MagicResist, 52.0, 65.0);
        SetSkill(SkillName.Tactics, 50.0, 62.0);
        SetSkill(SkillName.Wrestling, 45.0, 58.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a Leleges bone-augur's corpse";
    public override string DefaultName => "a Leleges bone-augur";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override bool CanHeal => true;

    public override int LootBagLevel => 3;

    // Bone-Rattle: 20% chance to sap stamina and rattle the defender's nerve.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 10;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The bone-augur's rattle saps your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
