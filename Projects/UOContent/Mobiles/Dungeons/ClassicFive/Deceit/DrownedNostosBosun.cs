using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Themed sub-faction - the Nostoi. L5 trash. Donor: SkeletalKnight.
[SerializationGenerator(0, false)]
public partial class DrownedNostosBosun : BaseCreature
{
    [Constructible]
    public DrownedNostosBosun() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x0841;
        BaseSoundID = 451;

        SetStr(225, 255);
        SetDex(78, 98);
        SetInt(40, 55);

        SetHits(330, 355);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 24, 34);
        SetResistance(ResistanceType.Cold, 52, 62);
        SetResistance(ResistanceType.Poison, 27, 37);
        SetResistance(ResistanceType.Energy, 34, 44);

        SetSkill(SkillName.MagicResist, 66.0, 76.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 2150;
        Karma = -2150;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a verdigris skeletal corpse";
    public override string DefaultName => "the Nostoi bosun";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    // Lash of the deep: 20% chance to sap stamina on a successful hit.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*the bosun's lash saps your strength*");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
