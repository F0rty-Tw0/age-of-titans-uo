using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family. L5 trash. Donor: BoneKnight.
[SerializationGenerator(0, false)]
public partial class DrownedOathbreaker : BaseCreature
{
    [Constructible]
    public DrownedOathbreaker() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x0847;
        BaseSoundID = 451;

        SetStr(230, 260);
        SetDex(75, 95);
        SetInt(38, 55);

        SetHits(370, 380);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 42, 52);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 52, 62);
        SetResistance(ResistanceType.Poison, 28, 38);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 82.0, 92.0);
        SetSkill(SkillName.Wrestling, 82.0, 92.0);

        Fame = 2100;
        Karma = -2100;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a bone-green skeletal corpse";
    public override string DefaultName => "a drowned oathbreaker";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    // Grave hunger: 20% chance to siphon a third of the dealt damage back as self-heal.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            Hits += damage / 3;
            PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*the oathbreaker feeds on your wound*");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
