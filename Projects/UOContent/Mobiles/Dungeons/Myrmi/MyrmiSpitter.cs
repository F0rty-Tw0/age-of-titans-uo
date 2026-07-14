using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L3 trash. Donor: Terathan Drone.
[SerializationGenerator(0, false)]
public partial class MyrmiSpitter : BaseCreature
{
    [Constructible]
    public MyrmiSpitter() : base(AIType.AI_Melee)
    {
        Body = 71;
        Hue = 0x0021;
        BaseSoundID = 594;

        SetStr(100, 125);
        SetDex(85, 105);
        SetInt(30, 45);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 18);
        SetResistance(ResistanceType.Cold, 10, 18);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 18);

        SetSkill(SkillName.Poisoning, 50.0, 70.0);
        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a myrmex spitter's corpse";
    public override string DefaultName => "a myrmex spitter";

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
