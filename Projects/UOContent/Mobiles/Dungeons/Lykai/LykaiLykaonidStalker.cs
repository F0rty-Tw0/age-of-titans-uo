using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - the Lykaonid wolf-sons, L5 trash. Donor: Grey Wolf.
[SerializationGenerator(0, false)]
public partial class LykaiLykaonidStalker : BaseCreature
{
    [Constructible]
    public LykaiLykaonidStalker() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(25, 27);
        Hue = 0x0964;
        BaseSoundID = 0xE5;

        SetStr(220, 250);
        SetDex(100, 120);
        SetInt(45, 60);

        SetHits(320, 360);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a Lykaonid stalker's corpse";
    public override string DefaultName => "a Lykaonid stalker";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 4;

    // Throat-Rip: 20% chance to tear a defender's stamina on a successful hit.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The stalker tears at your throat!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
