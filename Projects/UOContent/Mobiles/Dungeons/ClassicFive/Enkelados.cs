using System;
using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-enhancements.md) - Despise elite. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class Enkelados : DungeonElite
{
    // Buried Giant's roar adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public Enkelados() : base(AIType.AI_Melee)
    {
        Name = "Enkelados";
        Title = "the Buried Giant";

        Body = 75;
        Hue = 0x0972;
        BaseSoundID = 604;

        SetStr(400, 460);
        SetDex(90, 110);
        SetInt(40, 60);

        SetHits(300, 360);

        SetDamage(14, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 70.0, 90.0);
        SetSkill(SkillName.Tactics, 85.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 100.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a buried giant's corpse";

    public override int EliteBagLevel => 5;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // The earth heaves!: 15% chance for a small physical AoE plus a 2s freeze on the attacker.
        if (Utility.RandomDouble() < 0.15 && Map != null)
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "The earth heaves!");

            foreach (var m in Map.GetMobilesInRange(Location, 2))
            {
                var root = (m as BaseCreature)?.GetMaster() ?? m;

                if (m != this && m.Alive && root is PlayerMobile && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.Damage(Utility.RandomMinMax(8, 12), this);
                }
            }

            attacker.Freeze(TimeSpan.FromSeconds(2));
        }

        // Separate roll: calls up to two Lizardman adds.
        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Lizardman());
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
