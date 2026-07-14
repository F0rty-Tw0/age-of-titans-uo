using ModernUO.Serialization;

namespace Server.Mobiles;

// The Labors (dev-docs/open-world-bestiary.md §6) - roaming world-hunt, L6. Donor: Cougar.
[SerializationGenerator(0, false)]
public partial class LaborNemeanLion : DungeonElite
{
    [Constructible]
    public LaborNemeanLion() : base(AIType.AI_Melee)
    {
        Body = 63;
        Hue = 0x0798;
        BaseSoundID = 0x73;

        SetStr(300, 340);
        SetDex(160, 190);
        SetInt(40, 55);

        SetHits(530, 550);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 63);
        SetResistance(ResistanceType.Fire, 25, 33);
        SetResistance(ResistanceType.Cold, 25, 33);
        SetResistance(ResistanceType.Poison, 25, 33);
        SetResistance(ResistanceType.Energy, 25, 33);

        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 85.0, 97.0);
        SetSkill(SkillName.Wrestling, 88.0, 100.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 58;
    }

    public override string CorpseName => "the Nemean Lion's corpse";
    public override string DefaultName => "the Nemean Lion";

    public override bool ClickTitle => false;

    public override bool BleedImmune => true;

    public override int EliteBagLevel => 6;

    // Impervious Hide: 20% chance to reflect a quarter of the incoming blow back, no debuff.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.20 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            attacker.Damage(damage * 25 / 100, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The lion's hide throws your blow back at you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
