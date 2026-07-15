using System.Collections.Generic;
using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Stygian Deep elite (dev-docs/dungeon-ladder.md §5). Callback: the grown "Warden of the
// Gate" foreshadowed by the barrow's NewbieHollowWarden; donor Hellhound (body 98).
[SerializationGenerator(0, false)]
public partial class StygianCerberus : DungeonElite
{
    // Non-serialized: add tracking rebuilds naturally after a restart (DungeonAbilities idiom).
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public StygianCerberus() : base(AIType.AI_Melee)
    {
        Name = "Cerberus";
        Title = "Warden of the Gate";

        Body = 98;
        Hue = 0x0453;
        BaseSoundID = 229;

        SetStr(560, 610);
        SetDex(160, 180);
        SetInt(80, 110);

        SetHits(2900, 3100);

        SetDamage(25, 31);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 65, 75);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 50, 60);

        SetSkill(SkillName.MagicResist, 100.0, 110.0);
        SetSkill(SkillName.Tactics, 105.0, 115.0);
        SetSkill(SkillName.Wrestling, 105.0, 115.0);

        Fame = 35000;
        Karma = -35000;

        VirtualArmor = 75;
    }

    public override string CorpseName => "a hound's charred corpse";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 9;

    private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }

    // Threefold Maw: 15% on being hit, another head snaps into being (capped).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new StygianHound());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    protected override Item CreateDecoDrop() => new StatueOfCerberus();
}
