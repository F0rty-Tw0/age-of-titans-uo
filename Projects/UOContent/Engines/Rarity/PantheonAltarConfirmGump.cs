using System;
using Server.Gumps;

namespace Server.Engines.Rarity;

// Shared OK/Cancel confirmation for the altar's salvage and upgrade flows. Content is built by
// the caller; the callback fires with true on OK. Stale confirms are safe: SalvageSystem.Try*
// re-validates at execution, so a moved item or spent ichor degrades to a message, not a dupe.
public class PantheonAltarConfirmGump : StaticWarningGump<PantheonAltarConfirmGump>
{
    public override int Width => 360;
    public override int Height => 220;
    public override bool Singleton => true;

    private readonly string _content;

    public override string Content => _content;

    public PantheonAltarConfirmGump(string content, Action<bool> callback) : base(callback) =>
        _content = content;
}
