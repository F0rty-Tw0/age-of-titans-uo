using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - the Lykaonid wolf-sons, L5 trash. Donor: Orcish Mage.
[SerializationGenerator(0, false)]
public partial class LykaiLykaonidShaman : BaseCreature
{
    [Constructible]
    public LykaiLykaonidShaman() : base(AIType.AI_Mage)
    {
        Body = 140;
        Hue = 0x0021;
        BaseSoundID = 0x45A;

        SetStr(220, 250);
        SetDex(130, 150);
        SetInt(120, 145);

        SetHits(320, 360);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 65.0, 78.0);
        SetSkill(SkillName.Magery, 65.0, 78.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 55.0, 68.0);
        SetSkill(SkillName.Wrestling, 45.0, 60.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a Lykaonid blood-shaman's corpse";
    public override string DefaultName => "a Lykaonid blood-shaman";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
