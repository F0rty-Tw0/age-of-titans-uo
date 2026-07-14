using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard mini-boss. Donor: SerpentineDragon.
[SerializationGenerator(0, false)]
public partial class Pythios : DungeonElite
{
    // Rally the brood's summons; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public Pythios() : base(AIType.AI_Mage)
    {
        Name = "Pythios";
        Title = "the Cult-Hierarch";

        Body = 103;
        Hue = 0x0489;
        BaseSoundID = 362;

        SetStr(700, 750);
        SetDex(150, 170);
        SetInt(500, 550);

        SetHits(705, 715);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Poison, 25);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 100.0, 115.0);
        SetSkill(SkillName.Magery, 105.0, 120.0);
        SetSkill(SkillName.Meditation, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 60.0, 85.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "the cult-hierarch's corpse";

    public override bool ReacquireOnMovement => true;
    public override bool AutoDispel => true;
    public override int Meat => 19;
    public override int Hides => 20;
    public override HideType HideType => HideType.Barbed;
    public override int Scales => 6;
    public override ScaleType ScaleType => Utility.RandomBool() ? ScaleType.Black : ScaleType.White;

    public override int EliteBagLevel => 7;

    public override int GetIdleSound() => 0x2C4;

    public override int GetAttackSound() => 0x2C0;

    public override int GetDeathSound() => 0x2C1;

    public override int GetAngerSound() => 0x2C4;

    public override int GetHurtSound() => 0x2C3;

    // Rally the brood: separate roll, calls up to two Drake adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Drake());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
