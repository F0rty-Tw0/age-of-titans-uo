using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: Ghoul (body 153).
[SerializationGenerator(0, false)]
public partial class StygianGhoul : BaseCreature
{
    [Constructible]
    public StygianGhoul() : base(AIType.AI_Melee)
    {
        Body = 153;
        Hue = 0x0454;
        BaseSoundID = 0x482;

        SetStr(420, 460);
        SetDex(120, 150);
        SetInt(50, 80);

        SetHits(2000, 2250);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 95.0, 105.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 20000;
        Karma = -20000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "a starving ghoul's corpse";
    public override string DefaultName => "a starving ghoul";

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
