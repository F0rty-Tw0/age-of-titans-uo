// Pantheon tamables — Stygian Deep (dev-docs/dungeon-ladder.md). Donor: HellHound.
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class StygianWhelp : BaseCreature
{
    [Constructible]
    public StygianWhelp() : base(AIType.AI_Melee, FightMode.Aggressor)
    {
        Body = 98;
        Hue = 0x0453;
        BaseSoundID = 229;

        SetStr(260, 300);
        SetDex(160, 180);
        SetInt(80, 110);

        SetHits(460, 500);

        SetDamage(17, 20);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 58, 65);
        SetResistance(ResistanceType.Fire, 65, 75);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 62;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 98.7;
    }

    public override string CorpseName => "a cerberus whelp corpse";
    public override string DefaultName => "a cerberus whelp";

    public override int Meat => 1;
    public override FoodType FavoriteFood => FoodType.Meat;
}
