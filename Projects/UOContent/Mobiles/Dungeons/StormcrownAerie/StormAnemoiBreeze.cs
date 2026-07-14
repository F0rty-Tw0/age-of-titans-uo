using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L8. Donor: Wisp.
[SerializationGenerator(0, false)]
public partial class StormAnemoiBreeze : BaseCreature
{
    [Constructible]
    public StormAnemoiBreeze() : base(AIType.AI_Mage)
    {
        Body = 58;
        Hue = 0x0481;
        BaseSoundID = 466;

        SetStr(260, 300);
        SetDex(220, 250);
        SetInt(220, 250);

        SetHits(780, 850);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a wandering breeze's corpse";
    public override string DefaultName => "a wandering breeze";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
