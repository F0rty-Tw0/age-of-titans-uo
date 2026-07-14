using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L6 trash. Donor: Arctic Ogre Lord.
[SerializationGenerator(0, false)]
public partial class RimeBoreadLancer : BaseCreature
{
    [Constructible]
    public RimeBoreadLancer() : base(AIType.AI_Melee)
    {
        Body = 135;
        Hue = 0x0485;
        BaseSoundID = 427;

        SetStr(310, 350);
        SetDex(110, 135);
        SetInt(75, 100);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 48, 56);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a Boread frost-lancer's corpse";
    public override string DefaultName => "a Boread frost-lancer";

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
