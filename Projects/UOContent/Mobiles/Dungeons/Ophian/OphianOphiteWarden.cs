using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - Ophite hierophants, L7 trash. Donor: Ophidian Knight.
[SerializationGenerator(0, false)]
public partial class OphianOphiteWarden : BaseCreature
{
    [Constructible]
    public OphianOphiteWarden() : base(AIType.AI_Melee)
    {
        Body = 86;
        Hue = 0x0798;
        BaseSoundID = 634;

        SetStr(420, 460);
        SetDex(130, 155);
        SetInt(55, 70);

        SetHits(610, 670);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 54, 62);
        SetResistance(ResistanceType.Fire, 32, 40);
        SetResistance(ResistanceType.Cold, 32, 40);
        SetResistance(ResistanceType.Poison, 48, 56);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.Poisoning, 70.0, 88.0);
        SetSkill(SkillName.MagicResist, 70.0, 82.0);
        SetSkill(SkillName.Tactics, 82.0, 94.0);
        SetSkill(SkillName.Wrestling, 82.0, 94.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 60;
    }

    public override string CorpseName => "an Ophite high-warden's corpse";
    public override string DefaultName => "an Ophite high-warden";

    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 6;

    // Coil Riposte: 20% chance to reflect ~25% of the incoming blow back as poison damage.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.20 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            AOS.Damage(attacker, this, damage * 25 / 100, 0, 0, 0, 100, 0);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The coils riposte with venom!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
