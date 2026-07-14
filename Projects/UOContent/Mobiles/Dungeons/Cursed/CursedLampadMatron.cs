using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L8 trash. Donor: Ancient Lich.
[SerializationGenerator(0, false)]
public partial class CursedLampadMatron : BaseCreature
{
    // Called shades; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public CursedLampadMatron() : base(AIType.AI_Mage)
    {
        Body = 78;
        Hue = 0x0486;
        BaseSoundID = 412;

        SetStr(300, 330);
        SetDex(180, 210);
        SetInt(340, 380);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 40, 48);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 35, 42);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 12000;
        Karma = -12000;

        VirtualArmor = 58;
    }

    public override string CorpseName => "the Lampad matron's ashen remains";
    public override string DefaultName => "the Lampad lantern-matron";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 7;

    // Light the Dead: 15% chance on being hit to call up to two dispellable barrow shades.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(
                this,
                _adds,
                2,
                () =>
                {
                    var shade = new CursedShade { Summoned = true };
                    return shade;
                }
            );
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
