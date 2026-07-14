using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash. Donor: Ice Fiend.
[SerializationGenerator(0, false)]
public partial class TartarusTormentFiend : BaseCreature
{
    [Constructible]
    public TartarusTormentFiend() : base(AIType.AI_Mage)
    {
        Body = 43;
        Hue = 0x0022;
        BaseSoundID = 357;

        SetStr(560, 610);
        SetDex(220, 250);
        SetInt(260, 290);

        SetHits(895, 905);

        SetDamage(21, 26);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 65, 75);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 95.0, 110.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 105.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "a torment-fiend's corpse";
    public override string DefaultName => "a torment-fiend";

    public override bool CanFly => true;

    public override int LootBagLevel => 7;

    // Sear: a weaker echo of Alastor's fire curse burst.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            DoHarmful(defender);
            defender.Damage(Utility.RandomMinMax(15, 22), this);
            defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            defender.PublicOverheadMessage(MessageType.Regular, 0x22, true, "The torment-fiend's touch sears your flesh!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
