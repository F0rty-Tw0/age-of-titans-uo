using System.Collections.Generic;
using ModernUO.Serialization;
using Server.Engines.Rarity;
using Server.Items;

namespace Server.Mobiles;

// Stygian Deep boss (dev-docs/dungeon-ladder.md §5) — the only bag-10 source on the shard.
// Donor: Daemon (body 9). Payoff of the barrow's sealed gate: full katabasis, Hades's throne.
[SerializationGenerator(0, false)]
public partial class StygianLord : DungeonElite
{
    // Non-serialized: add tracking rebuilds naturally after a restart (DungeonAbilities idiom).
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public StygianLord() : base(AIType.AI_Mage)
    {
        Name = "Hades";
        Title = "Lord of the Unseen";

        Body = 9;
        Hue = 0x0489;
        BaseSoundID = 357;

        SetStr(700, 750);
        SetDex(150, 170);
        SetInt(550, 600);

        SetHits(3200, 3600);

        SetDamage(26, 34);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 65, 75);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 60, 70);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.EvalInt, 115.0, 125.0);
        SetSkill(SkillName.Magery, 115.0, 125.0);
        SetSkill(SkillName.MagicResist, 115.0, 130.0);
        SetSkill(SkillName.Tactics, 105.0, 115.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 50000;
        Karma = -50000;

        VirtualArmor = 85;
    }

    public override string CorpseName => "the corpse of the Unseen Lord";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    // One bag 10 (guaranteed Legendary since 2026-07-14) + one bag 9 (90/10 Epic/Legendary)
    // via the OnDeath override below — two bag-10s would mean two guaranteed Legendaries
    // per respawn, too hot even for Hades.
    public override int EliteBagLevel => 10;

    private static MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }

    // Sovereign of the Dead: 12% on being hit, one roll drives both the self-heal
    // and the shade levy from the ranks (capped).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.12)
        {
            Hits += HitsMax / 10;
            DungeonAbilities.TrySpawnAdd(this, _adds, 3, () => new StygianShade());
        }
    }

    public override void OnDeath(Container c)
    {
        base.OnDeath(c); // guaranteed bag 10 + telegraph via DungeonElite

        var bag = new LootBag(9, PantheonDomain.Underworld);
        bag.DropItem(Engines.LootBags.LootRoller.Roll(9, PantheonDomain.Underworld));
        c.DropItem(bag);
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    protected override Item CreateDecoDrop() => new BidentOfHades();
}
