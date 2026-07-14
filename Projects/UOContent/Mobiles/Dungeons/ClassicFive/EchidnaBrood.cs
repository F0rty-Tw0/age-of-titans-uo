using ModernUO.Serialization;

namespace Server.Mobiles;

// The Brood of Echidna (dev-docs/classic-five-enhancements.md) - shared skeleton for five
// ambient dungeon-flavored recolors. Donor: Giant Serpent (body/sound). Not elites: normal
// level-scaled loot-bag chance, no LootBagLevel override. Each skin passes its own
// name/hue/HP/damage/venom tier and adds exactly one dungeon-flavored bonus.
[SerializationGenerator(0, false)]
public abstract partial class EchidnaBrood : BaseCreature
{
    protected EchidnaBrood(
        string name,
        int hue,
        int minHits,
        int maxHits,
        int minDamage,
        int maxDamage,
        int fame
    ) : base(AIType.AI_Melee)
    {
        Name = name;
        Body = 0x15;
        Hue = hue;
        BaseSoundID = 219;

        SetStr(minHits, maxHits);
        SetDex(56, 80);
        SetInt(66, 85);

        SetHits(minHits, maxHits);
        SetMana(0);

        SetDamage(minDamage, maxDamage);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Poison, 60);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 5, 10);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Poison, 70, 90);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.Poisoning, 70.1, 100.0);
        SetSkill(SkillName.MagicResist, 25.1, 40.0);
        SetSkill(SkillName.Tactics, 65.1, 80.0);
        SetSkill(SkillName.Wrestling, 60.1, 85.0);

        Fame = fame;
        Karma = -fame;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a serpent's corpse";

    // Venom tier per skin as a property override — a ctor-set field would come back
    // null after deserialization (the parameterized ctor never runs on world load).
    public abstract override Poison HitPoison { get; }
    public override Poison PoisonImmune => Poison.Deadly;

    public override int Meat => 4;
    public override int Hides => 15;
    public override HideType HideType => HideType.Spined;
}
