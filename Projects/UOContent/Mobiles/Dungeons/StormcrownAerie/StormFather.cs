using System.Collections.Generic;
using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder.md §4) - boss. Donor: Dragon. Boss is held at
// bag8 (x2) so bag9 stays reserved for the elite and bag10 stays exclusive to the Hades fight.
[SerializationGenerator(0, false)]
public partial class StormFather : DungeonElite
{
    // Hundred Hands adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Cataclysm"

    [Constructible]
    public StormFather() : base(AIType.AI_Mage)
    {
        Name = "Typhon";
        Title = "the Hundred-Storm";

        Body = 12;
        Hue = 0x0492;
        BaseSoundID = 362;

        SetStr(820, 860);
        SetDex(160, 190);
        SetInt(500, 550);

        SetHits(2380, 2400);

        SetDamage(24, 30);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 65, 75);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 70, 80);

        SetSkill(SkillName.EvalInt, 100.0, 110.0);
        SetSkill(SkillName.Magery, 100.0, 110.0);
        SetSkill(SkillName.MagicResist, 105.0, 115.0);
        SetSkill(SkillName.Tactics, 100.0, 110.0);
        SetSkill(SkillName.Wrestling, 95.0, 105.0);

        Fame = 28000;
        Karma = -28000;

        VirtualArmor = 78;
    }

    public override string CorpseName => "the Hundred-Storm's corpse";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 8;
    public override int EliteBagCount => 2;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Hundred Hands: 12% chance on being hit to raise another storm wisp to his side (capped).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.12)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new StormWisp());
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

    protected override Item CreateDecoDrop() => new TyphonsStormStandard();
}
