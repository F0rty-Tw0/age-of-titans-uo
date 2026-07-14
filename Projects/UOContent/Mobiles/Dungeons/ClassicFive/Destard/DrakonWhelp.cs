using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon family. L6 trash. Donor: Drake.
[SerializationGenerator(0, false)]
public partial class DrakonWhelp : BaseCreature
{
    [Constructible]
    public DrakonWhelp() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(60, 61);
        Hue = 0x0501;
        BaseSoundID = 362;

        SetStr(420, 450);
        SetDex(100, 120);
        SetInt(80, 100);

        SetHits(475, 485);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 80);
        SetDamageType(ResistanceType.Fire, 20);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 4000;
        Karma = -4000;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a drakon whelp's corpse";
    public override string DefaultName => "a drakon whelp";

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
