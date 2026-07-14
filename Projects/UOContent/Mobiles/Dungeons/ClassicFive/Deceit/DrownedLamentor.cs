using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family. L5 trash. Donor: Bogle.
[SerializationGenerator(0, false)]
public partial class DrownedLamentor : BaseCreature
{
    [Constructible]
    public DrownedLamentor() : base(AIType.AI_Mage)
    {
        Body = 153;
        Hue = 0x0847;
        BaseSoundID = 0x482;

        SetStr(125, 150);
        SetDex(92, 112);
        SetInt(180, 205);

        SetHits(355, 365);

        SetDamage(14, 19);

        SetResistance(ResistanceType.Physical, 38, 48);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 80.0, 92.0);
        SetSkill(SkillName.Magery, 80.0, 92.0);
        SetSkill(SkillName.MagicResist, 72.0, 84.0);
        SetSkill(SkillName.Tactics, 64.0, 76.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 2000;
        Karma = -2000;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a bone-green ghostly corpse";
    public override string DefaultName => "a drowned lamentor";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 4;

    // Ebb away: 15% chance to blink 4-6 tiles clear of the attacker.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Map == null || !Alive || Utility.RandomDouble() >= 0.15)
        {
            return;
        }

        var dx = Math.Sign(X - attacker.X);
        var dy = Math.Sign(Y - attacker.Y);

        if (dx == 0 && dy == 0)
        {
            dx = Utility.RandomBool() ? 1 : -1;
        }

        var dist = Utility.RandomMinMax(4, 6);
        var x = X + dx * dist;
        var y = Y + dy * dist;
        var loc = new Point3D(x, y, Map.GetAverageZ(x, y));

        if (!Map.CanSpawnMobile(loc))
        {
            return;
        }

        Location = loc;
        FixedParticles(0x3728, 1, 10, 0x26B8, EffectLayer.Waist);
        PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*the lamentor ebbs away into the brine*");
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
