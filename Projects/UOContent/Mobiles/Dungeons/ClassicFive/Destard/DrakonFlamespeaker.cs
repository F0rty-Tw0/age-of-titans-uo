using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon family. L7 trash. Donor: OphidianMage.
[SerializationGenerator(0, false)]
public partial class DrakonFlamespeaker : BaseCreature
{
    [Constructible]
    public DrakonFlamespeaker() : base(AIType.AI_Mage)
    {
        Body = 85;
        Hue = 0x0489;
        BaseSoundID = 639;

        SetStr(200, 230);
        SetDex(200, 220);
        SetInt(550, 600);

        SetHits(645, 655);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.EvalInt, 100.0, 110.0);
        SetSkill(SkillName.Magery, 100.0, 110.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 70.0, 90.0);
        SetSkill(SkillName.Wrestling, 30.0, 60.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a drakon flamespeaker's corpse";
    public override string DefaultName => "a drakon flamespeaker";

    public override int LootBagLevel => 6;

    // Ember-word: a passive fire pop on a successful hit, à la Alastor's fire-curse burst.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            DoHarmful(defender);
            defender.Damage(Utility.RandomMinMax(10, 16), this);
            defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            defender.PublicOverheadMessage(MessageType.Regular, 0x0489, true, "The flamespeaker's ember-word sears your flesh!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
