using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Panther.
[SerializationGenerator(0, false)]
public partial class WyldPanther : BaseCreature
{
    [Constructible]
    public WyldPanther() : base(AIType.AI_Melee)
    {
        Body = 0xD6;
        Hue = 0x0798;
        BaseSoundID = 0x462;

        SetStr(220, 260);
        SetDex(150, 180);
        SetInt(40, 60);

        SetHits(620, 700);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 4500;
        Karma = -4500;

        VirtualArmor = 62;
    }

    public override string CorpseName => "a bronze-maned corpse";
    public override string DefaultName => "a bronze-maned panther";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override bool BleedImmune => true;

    public override int LootBagLevel => 6;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Pounce: 15% chance to blink to the far side of the attacker.
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

        var x = attacker.X + dx;
        var y = attacker.Y + dy;
        var loc = new Point3D(x, y, Map.GetAverageZ(x, y));

        if (!Map.CanSpawnMobile(loc))
        {
            return;
        }

        Location = loc;
        FixedParticles(0x3728, 1, 10, 0x26B8, EffectLayer.Waist);
        PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*the panther vanishes in a blur, landing behind you*");
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
