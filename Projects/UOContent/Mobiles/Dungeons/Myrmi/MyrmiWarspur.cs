using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L5 trash. Donor: Terathan Warrior.
[SerializationGenerator(0, false)]
public partial class MyrmiWarspur : BaseCreature
{
    [Constructible]
    public MyrmiWarspur() : base(AIType.AI_Melee)
    {
        Body = 70;
        Hue = 0x0966;
        BaseSoundID = 589;

        SetStr(220, 250);
        SetDex(105, 125);
        SetInt(40, 55);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.Poisoning, 60.0, 80.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a myrmex warspur's corpse";
    public override string DefaultName => "a myrmex warspur";

    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
