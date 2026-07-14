using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L6. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class BrineSquallHarrier : BaseCreature
{
    [Constructible]
    public BrineSquallHarrier() : base(AIType.AI_Melee)
    {
        Name = "a squall-harrier";

        Body = 30;
        Hue = 0x0481;
        BaseSoundID = 402;

        SetStr(200, 230);
        SetDex(190, 210);
        SetInt(80, 100);

        SetHits(515, 525);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 30);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 65.0, 80.0);
        SetSkill(SkillName.Tactics, 85.0, 100.0);
        SetSkill(SkillName.Wrestling, 80.0, 95.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a squall-harrier's corpse";

    public override bool BleedImmune => true;
    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 5;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Gale-slap: 15% chance to blast the attacker 2 tiles away.
        if (Map == null || !Alive || Utility.RandomDouble() >= 0.15)
        {
            return;
        }

        var dx = Math.Sign(attacker.X - X);
        var dy = Math.Sign(attacker.Y - Y);

        if (dx == 0 && dy == 0)
        {
            dx = Utility.RandomBool() ? 1 : -1;
        }

        var x = attacker.X + dx * 2;
        var y = attacker.Y + dy * 2;
        var loc = new Point3D(x, y, Map.GetAverageZ(x, y));

        if (!Map.CanSpawnMobile(loc))
        {
            return;
        }

        attacker.Location = loc;
        FixedParticles(0x3728, 1, 10, 0x26B8, EffectLayer.Waist);
        PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*a gust of wind slaps you back*");
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
