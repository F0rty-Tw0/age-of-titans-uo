# Wiki Showcase — Plan

Static wiki site showcasing shard content: items, monsters, skills, mechanics, gathering, professions.
Everything data-driven is **auto-generated** from the C# codebase — we change code a lot, the wiki must not rot.

## Architecture (no API server)

```
Projects/WikiExport/  (console app, .NET 10)
  ├─ bootstraps headless server (mirrors UOContent.Tests/Fixtures/TestServerInitializer.cs)
  ├─ reflects over Item / BaseCreature subclasses → DTOs
  ├─ renders sprite PNGs (art.mul + hues.mul from client files, baked hue)
  ├─ emits TypeScript models from DTO types (single source of truth = C# DTOs)
  └─ writes everything into wiki/public/data/ + wiki/src/app/models/generated.ts

wiki/                 (Angular SPA, standalone components + signals)
  ├─ reads JSON from public/data/ — no HTTP API, client-side search/filter
  ├─ generated types → fully typed frontend
  └─ hand-written markdown pages for prose sections (mechanics, professions)
```

Pipeline: `dotnet run --project Projects/WikiExport` → JSON + PNGs + generated.ts land in `wiki/`.
Deploy: static hosting (GitHub Pages / Cloudflare Pages). Re-export on release, commit, auto-deploy.

## Feasibility anchors (confirmed)

- **Headless instantiation**: `Projects/UOContent.Tests/Fixtures/TestServerInitializer.cs` already boots
  Core + World + TileData without a shard. Exporter reuses this recipe. Needs `MODERNUO_TEST_DATA_DIR`
  (client files at `F:\UO`).
- **Hue math**: hue entry = 32 shades; pixel red channel (0–31) indexes the table; partial-hue flag
  tints only grayscale pixels. Well-documented format, small code.

## Known hard edges (named up front, not hidden)

1. **Rarity/variant items — export the tables, not instances.** (Resolved: checked
   `Engines/Rarity/Effects/`.) The engine is already table-driven: `ArmorEffectTable`
   [root × rarity], `ArmorSlotSignatureTable` [material × slot], `WeaponEffectTable`,
   `AccessoryEffectTable`, `LegendaryRegistry`. The variant space is combinatorial, so
   per-item export would explode into near-duplicates. Instead:
   - `items.json` — base pieces only (name, itemID, hue, slot, material, AR, sprite).
   - `effect-tables.json` — roots + per-rarity rows + slot-signature matrix, each cell
     with **rendered text via `ClauseText.Describe` / `Build*Summary`** — the same code
     that builds in-game tooltips, so wiki wording never drifts from the game.
   - `legendaries.json` — from `LegendaryRegistry`, one entry per unique.
   Frontend composes: material × slot signature matrix page, per-item "what each
   root/rarity does here" panel.
2. **Monster loot tables are imperative code** (`GenerateLoot()` bodies). Full auto-extraction is not
   realistic. Options: (a) run `GenerateLoot()` N times and aggregate observed drops, (b) annotate drops
   declaratively over time, (c) hand-maintained loot JSON per boss. Start with (a) for common loot +
   hand-written notes for uniques.
3. **Art reader** — server engine does not read art pixel data. Need a reader for `art.mul`/`artLegacyMUL.uop`
   + `hues.mul`. Formats are simple and documented; hand-roll a minimal reader (~200 lines) inside
   WikiExport, PNG out via ImageSharp. Fallback: lift reader code from UltimaSDK/ClassicUO (both C#).

## Phases

### Phase 1 — Exporter skeleton + items
- `Projects/WikiExport/` console project referencing Server + UOContent.
- Headless bootstrap (copy TestServerInitializer recipe, adjust Expansion to T2A).
- DTOs: `ItemDto`, `MonsterDto`, `SkillDto` — plain records.
- Reflect + instantiate item classes → `items.json`.
- TS emitter: reflection over DTO records → `generated.ts` (interfaces, ~50 lines of emitter).

### Phase 2 — Sprites
- Minimal `art.mul`/`.uop` + `hues.mul` reader.
- Render PNG per (itemId, hue) actually referenced by exported data → `wiki/public/img/items/`.
- Monster body sprites: use animation frame 0 or gump art — decide when we see what looks good.

### Phase 3 — Angular SPA
- `wiki/` app: routes = Home, Items, Monsters, Skills, Mechanics, Gathering, Professions.
- Item browser: grid + filters (slot, material, rarity tier, hue swatch), detail panel.
- Monster browser: stats block, sprite, observed loot.
- Skills: table of shard-specific changes (auto where possible, prose where not).
- Professions: auto-exported from craft systems (`CraftSystem`/`CraftItem` are declarative —
  recipes, skill reqs, materials → `crafts.json`).
- Gathering: auto-exported from harvest systems (`HarvestDefinition` — veins, resources,
  skill ranges → `gathering.json`).
- Mechanics: markdown pages rendered in-app (marked/ngx-markdown), content in
  `wiki/content/*.md` — hand-written prose, versioned with code. Rule: table-shaped → JSON
  export; paragraph-shaped → markdown.

### Phase 4 — Pipeline polish
- One command regenerates everything; CI job builds + deploys on tag/release.
- Diff-friendly JSON (stable ordering) so PRs show content changes readably.

## Data model sketch (v1, will grow)

```csharp
record ItemDto(string Id, string Name, int ItemId, int Hue, string? Slot,
               string? Material, string? RarityTier, double Weight,
               Dictionary<string, string> Props, string Sprite);

record MonsterDto(string Id, string Name, int Body, int Hue, int Hits, int Str, int Dex, int Int,
                  Dictionary<string, double> Skills, string[] ObservedLoot, string Sprite);
```

## Decisions taken (defaults, veto any)

- In-repo (`wiki/` at root) — wiki versions together with content changes.
- GitHub Pages hosting.
- Pre-baked PNGs only; no canvas hueing (rejected as overkill).
- Prose sections are hand-written markdown, not extracted.

## Open (needs brainstorm, next session)

- Section content map: what exactly goes under Mechanics vs Professions vs Gathering.
- Which item properties are showcase-worthy (tie into itemization framework docs).
- Rarity/legendary presentation — globally-unique items deserve special page treatment.
