using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Evil Mage (human
// body; T2A has no distinct huntress body).
[SerializationGenerator(0, false)]
public partial class WyldThiasosStalker : BaseCreature
{
    [Constructible]
    public WyldThiasosStalker() : base(AIType.AI_Melee)
    {
        Body = 0x190;
        Hue = 0x0483;

        SetStr(210, 240);
        SetDex(140, 170);
        SetInt(60, 80);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 4500;
        Karma = -4500;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a Thiasos stalker's corpse";
    public override string DefaultName => "a Thiasos stalker";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Vanish: 15% chance to blink to the far side of the attacker.
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
        PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*the stalker melts into shadow, reappearing behind you*");
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
