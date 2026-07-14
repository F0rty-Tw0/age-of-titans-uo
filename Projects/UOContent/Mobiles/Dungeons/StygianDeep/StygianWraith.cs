using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: Wraith (body 26).
[SerializationGenerator(0, false)]
public partial class StygianWraith : BaseCreature
{
    [Constructible]
    public StygianWraith() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0455;
        BaseSoundID = 0x482;

        SetStr(260, 300);
        SetDex(110, 140);
        SetInt(460, 520);

        SetHits(2000, 2250);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 20000;
        Karma = -20000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "a river wraith's corpse";
    public override string DefaultName => "a river wraith";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }

    // Lethe Chill: 20% on a landed hit, drain the defender's mana.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 15;
            defender.PublicOverheadMessage(MessageType.Regular, 0x480, true, "Lethe Chill saps your mana!");
        }
    }
}
