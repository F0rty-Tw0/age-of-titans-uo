using ModernUO.Serialization;

namespace Server.Mobiles;

// Newbie-dungeon elites: guaranteed bag 2 on death via the shared DungeonElite drop path.
[SerializationGenerator(0, false)]
public abstract partial class NewbieElite : DungeonElite
{
    protected NewbieElite(AIType aiType) : base(aiType)
    {
    }

    public override int EliteBagLevel => 2;
}
