using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Ismenian brood. L7. Donor: Wyvern.
[SerializationGenerator(0, false)]
public partial class DrakonIsmenianWyrm : BaseCreature
{
    [Constructible]
    public DrakonIsmenianWyrm() : base(AIType.AI_Melee)
    {
        Body = 62;
        Hue = 0x0501;
        BaseSoundID = 362;

        SetStr(565, 605);
        SetDex(165, 185);
        SetInt(65, 90);

        SetHits(645, 670);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Poison, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 95, 100);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.Poisoning, 70.0, 90.0);
        SetSkill(SkillName.MagicResist, 75.0, 90.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 9200;
        Karma = -9200;

        VirtualArmor = 58;
    }

    public override string CorpseName => "an Ismenian wyrm's corpse";
    public override string DefaultName => "an Ismenian wyrm";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 6;

    public override int GetAttackSound() => 713;

    public override int GetAngerSound() => 718;

    public override int GetDeathSound() => 716;

    public override int GetHurtSound() => 721;

    public override int GetIdleSound() => 725;

    // Tail-lash: 15% chance to knock the attacker back and sap stamina. Knockback is a
    // flavor message only - no position change (no knockback primitive in this codebase),
    // same idiom as DrownedTholos/TideHerald.OnGaveMeleeAttack.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            attacker.Stam -= 14;
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The wyrm's tail-lash knocks you back!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
