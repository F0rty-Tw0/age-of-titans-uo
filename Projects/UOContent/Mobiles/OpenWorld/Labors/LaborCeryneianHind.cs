using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Labors (dev-docs/open-world-bestiary.md §6) - roaming world-hunt, L4. Donor: Great Hart.
[SerializationGenerator(0, false)]
public partial class LaborCeryneianHind : DungeonElite
{
    [Constructible]
    public LaborCeryneianHind() : base(AIType.AI_Melee)
    {
        Body = 0xEA;
        Hue = 0x0486;

        SetStr(150, 180);
        SetDex(140, 170);
        SetInt(30, 50);

        SetHits(230, 240);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 6000;
        Karma = -6000;

        VirtualArmor = 46;
    }

    public override string CorpseName => "the Ceryneian Hind's corpse";
    public override string DefaultName => "the Ceryneian Hind";

    public override bool ClickTitle => false;

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int EliteBagLevel => 4;

    // Bounding Flight: 25% chance to bound 6-8 tiles away from the attacker - a chase fight.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Map == null || !Alive || Utility.RandomDouble() >= 0.25)
        {
            return;
        }

        var dx = Math.Sign(X - attacker.X);
        var dy = Math.Sign(Y - attacker.Y);

        if (dx == 0 && dy == 0)
        {
            dx = Utility.RandomBool() ? 1 : -1;
        }

        var dist = Utility.RandomMinMax(6, 8);
        var x = X + dx * dist;
        var y = Y + dy * dist;
        var loc = new Point3D(x, y, Map.GetAverageZ(x, y));

        if (!Map.CanSpawnMobile(loc))
        {
            return;
        }

        Location = loc;
        FixedParticles(0x3728, 1, 10, 0x26B8, EffectLayer.Waist);
        PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*the hind bounds away in a golden blur*");
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
