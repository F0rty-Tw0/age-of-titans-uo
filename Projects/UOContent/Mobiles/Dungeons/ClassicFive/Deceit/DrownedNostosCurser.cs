using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Themed sub-faction - the Nostoi. L5 trash. Donor: BoneMagi.
[SerializationGenerator(0, false)]
public partial class DrownedNostosCurser : BaseCreature
{
    [Constructible]
    public DrownedNostosCurser() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x0847;
        BaseSoundID = 451;

        SetStr(118, 142);
        SetDex(86, 106);
        SetInt(180, 205);

        SetHits(320, 345);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 48, 58);
        SetResistance(ResistanceType.Poison, 26, 36);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.EvalInt, 78.0, 90.0);
        SetSkill(SkillName.Magery, 78.0, 90.0);
        SetSkill(SkillName.MagicResist, 70.0, 82.0);
        SetSkill(SkillName.Tactics, 62.0, 74.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 2000;
        Karma = -2000;

        VirtualArmor = 43;
    }

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "a Nostoi curser";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    // Salt rot: 20% chance to sap mana on a successful hit.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*salt rot creeps into your veins*");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
