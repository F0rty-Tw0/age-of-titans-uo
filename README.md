<p align="center">
  <a href="https://muo.gg"><img alt="ModernUO - Ultima Online Server Emulator for the modern era!" src="https://cdn.muo.gg/gh/muo-logo.svg" width=128px /></a>
</p>

ModernUO [![Discord](https://img.shields.io/discord/751317910504603701?logo=discord&style=social)](https://muo.gg/discord) [![Subreddit subscribers](https://img.shields.io/reddit/subreddit-subscribers/modernuo?style=social&label=/r/modernuo)](https://muo.gg/reddit/) [![Twitter Follow](https://img.shields.io/twitter/follow/modernuo?label=@modernuo&style=social)](https://muo.gg/twitter)
=====

##### Ultima Online Server Emulator for the modern era!
[![GitHub license](https://img.shields.io/github/license/modernuo/ModernUO?color=blue)](https://github.com/modernuo/ModernUO/blob/master/LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/modernuo/ModernUO?logo=github&style=flat)](https://github.com/modernuo/ModernUO/stargazers)
[![GitHub issues](https://img.shields.io/github/issues/modernuo/ModernUO?logo=github)](https://github.com/modernuo/ModernUO/issues)
<br/>
[![GitHub build](https://img.shields.io/github/actions/workflow/status/modernuo/ModernUO/build-test.yml?branch=main&logo=github)](https://github.com/modernuo/ModernUO/actions)
[![Azure Pipelines build](https://dev.azure.com/modernuo/modernuo/_apis/build/status/Build?branchName=main)](https://dev.azure.com/modernuo/modernuo/_build/latest?definitionId=1&branchName=main)

## Age of Titans — Shard Customizations

This repository is a fork of [ModernUO](https://github.com/modernuo/ModernUO) customized for the **Age of Titans** shard: T2A era, Felucca-only. The sections below document the shard systems added on top of upstream (last updated 2026-07-14).

### Player & Mob Leveling — `Projects/UOContent/Engines/Leveling/`
- 10-level XP progression. Kills award XP to every player with looting rights, scaled by the mob-vs-player level gap (0.25x for easy kills up to 1.5x for mobs 2+ levels above you; mobs 3+ levels below award nothing).
- Reaching levels 1–5 tops up total stats (100 → 300) one point at a time, round-robin Str/Dex/Int, honoring stat locks and the 200 per-stat cap.
- Per-skill caps replace the total skill cap as the shard's limiter: 50.0 at level 0 rising to 100.0 at level 5+.
- Mob level derives from max HP (9 hand-tuned brackets), with an override table pinning ambient/farm animals to level 0 and bumping casters (liches, mages) above their HP bracket.
- Single-clicking a creature shows a `[lvl N]` tag hued by difficulty: gray (no XP), green (easy), white (even), yellow (tough), red (dangerous).
- `[level` shows progress; `[levelguide` explains the system; a one-time primer gump appears on first level-up.

### Loot Bags — `Projects/UOContent/Engines/LootBags/`
- Non-controlled creatures can drop a level-tagged `LootBag` (default bag level = mob level, `LootBagLevel` is virtual per class). Drop chance ramps convex from 10% at level 1 to 50% at level 10 (`LootBagConfig.ChanceForMobLevel`); level-0 ambient creatures never drop one.
- Each bag rolls exactly **one** item — no Commons ever drop. Rarity odds scale with bag level (`LootRoller._rarityWeights`); bag 9 is a 90/10 Epic/Legendary split, bag 10 is **100% guaranteed Legendary**.
- Themed dungeon trash (the newbie barrow, the Five Domains ladder) overrides the default down one tier — bag level = mob level − 1 — while open-world biome creatures keep the plain mob-level default. Dungeon elites and bosses opt out of the roll entirely and force-drop a guaranteed bag (`DungeonElite.EliteBagLevel`/`EliteBagCount`), telegraphed with a corpse sparkle. **The Stygian Deep's Hades is the only bag-10 source on the shard.**
- Bag hue and displayed `[level N]` tag reflect the level. Full reference: `dev-docs/loot-and-drops.md`.

### World Content — Dungeons & Bestiary
942 custom creatures across 63 named elites/bosses, all spawnable, level-pinned, and covered by tests (`dev-docs/beast-reference.md` is the generated full roster; `dev-docs/dungeon-ladder.md` covers the ladder below).
- **Barrow of the Unremembered** (L0–3, 15 mobs) — the newbie safe on-ramp.
- **Five Domains dungeon ladder** (L4–10) — Drowned Tholos, Cinderworks, Nemean Wildwood, Stormcrown Aerie, Stygian Deep, one per god, climbing to Hades's throne.
- **Enhanced Classic Five** — Despise, Deceit, Shame, Destard, Hythloth each get a themed family and a named elite alongside the untouched stock roster.
- **9 themed gap-family dungeons** — Fire (Pyre), Ice (Rime), Khaldun (Cursed), Solen Hive (Myrmi), Terathan Keep (Ophian), Orc Caves (Lykai), Covetous (Argus), Wrong (Wayman), Painted Caves (Pelasg).
- **5 open-world biome families** (Groves, Peaks, Mire, Restless, Shore) plus **The Labors** — 6 named roaming world-hunt elites.

### Equipment Rarity — `Projects/UOContent/Engines/Rarity/`
- Five tiers (common, uncommon, rare, epic, legendary) on weapons, armor, clothing, and jewelry. Serialized with versioned migrations, GM-settable via `[props`.
- Non-common items show `rarity: <tier>` in tooltips and a `[tier]` suffix on T2A single-click labels, including unidentified magic items.
- Pantheon altar hub (`[Add PantheonAltar`): salvage Uncommon–Epic variants into ichor (2/4/8), spend ichor on deterministic tier upgrades (20 → Rare, 80 → Epic; item keeps its theme), or make the two-for-one legendary domain offering. A world-broadcast announcement fires on epic+ finds when the loot bag is opened.
- Pantheon-targeted farming (`dev-docs/itemization/30-pantheon-bags.md`, approved design, **implementation pending**): themed mobs will drop god-locked bags 70% of the time so players can farm a specific god's gear.

### Floating Combat Text — `Projects/UOContent/Misc/FloatingCombatText.cs`
- Overhead numbers replace the client's raw damage packet: red melee, red-orange spells, green heals, dark-green poison, tagged with the source (e.g. `-19 (Flame Strike)`).
- Shown to both attacker and target with correct per-perspective hues; zero-allocation stackalloc formatting.
- Hooked into melee, spells, heal spells, potions, bandages, and poison ticks.

### Double-Click to Equip — `Projects/UOContent/Items/DoubleClickEquip.cs`
- Double-clicking a wearable in your backpack equips it, swapping whatever occupies the layer (and the conflicting hand for weapons) back into the pack.
- The same swap logic backs paperdoll drag-equip. Tools (axes, pole arms, fishing pole, crook, throwing dagger, fireworks wand) equip first, then run their use action.
- Casting no longer clears your hands (`ClearHandsOnCast` off).

### Banded Skill Gain — `Projects/UOContent/Skills/SkillCheck.cs`
- The compound RunUO gain formula is replaced with a flat chance per band: 90% below 60.0 skill, 75% to 95.0, 50% to 100.0.
- Band edges and chances are `ServerConfiguration` settings (`skills.gainBand*` / `skills.gainChance*`) — tunable without a rebuild.

### Attack-on-Sight Notoriety — `Projects/UOContent/Misc/Notoriety.cs`
- Aggressive creatures (FightMode Closest/Strongest/Weakest) always show red. FightMode Evil creatures show red to negative-karma characters. Pets and summons are exempt.

### Shard Setup & Ops
- T2A expansion, Felucca-only maps, starting city fixed to Felucca; character creation uses a fixed 30/25/25 stat spread with all skills vendor-trained to 30.0.
- Single-click item detail labels (damage, protection, etc.) controlled by `ItemInfoConfiguration`.
- Command and network-disconnect audit logs; shard configuration is tracked in the repository (`Distribution/Saves` — world and account data — and generated pathfinding data are gitignored).

### Tests
- Each system ships with xUnit coverage under `Projects/UOContent.Tests/`: leveling math, loot bag drops, rarity serialization/labels, combat text, equip swaps, notoriety, skill gain curve, and single-click packets.

## Known Improvements / Tech Debt

Ranked review of the systems above — what to fix and how.

### 1. Skill gain bands dropped per-skill difficulty
The new `SkillCheck.GainChance` ignores `skill.Info.GainFactor`, so hard skills (Taming, Magery) gain exactly as fast as trivial ones (Camping). If uniform speed is the design, document it; otherwise the difficulty signal is lost.

**Fix:** multiply the band chance by `skill.Info.GainFactor` (or a clamped version of it) inside `GainChance`, keeping the bands as the base curve.

### 2. ~~`IRarity.MaxRarity` is not enforced~~ (fixed 2026-07-06)
Every base returned `Legendary` and nothing clamped `Rarity` against it, so the property was decorative — a GM (or future code) could set any tier on any item.

**Fixed:** each base now hand-writes the `Rarity` property (`[SerializableProperty]`, same slot — no version bump) and clamps through a shared `RaritySystem.Clamp(value, MaxRarity)`, covering both out-of-range negatives (→ common) and tiers above the item's cap. Covered by clamp tests in `RarityItemTests`.

## Requirements
#### Supported Operating Systems
[![Windows 10/11/2012 R2/2016/2019/2022/2025](https://img.shields.io/badge/-server%202025-3c78d5?labelColor=222222&logo=data:image/svg%2bxml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHJvbGU9ImltZyIgdmlld0JveD0iMCAwIDI0IDI0Ij48dGl0bGU+V2luZG93czwvdGl0bGU+PHBhdGggZD0iTTAsMEgxMS4zNzdWMTEuMzcySDBaTTEyLjYyMywwSDI0VjExLjM3MkgxMi42MjNaTTAsMTIuNjIzSDExLjM3N1YyNEgwWm0xMi42MjMsMEgyNFYyNEgxMi42MjMiIGZpbGw9IiMzYzc4ZDUiLz48L3N2Zz4=)](https://www.microsoft.com/en-US/evalcenter/evaluate-windows-server-2022)
![MacOS 14+](https://img.shields.io/badge/-sonoma-222222?logo=apple&logoColor=white&labelColor=222222)
[![Debian 12+](https://img.shields.io/badge/-trixie-A81D33?logo=debian&logoColor=A81D33&labelColor=222222)](https://www.debian.org/distrib/)
[![Ubuntu 22+ LTS](https://img.shields.io/badge/-26LTS-E95420?logo=ubuntu&logoColor=E95420&labelColor=222222)](https://ubuntu.com/download/server)
<br/>
[![Alpine 3.22+](https://img.shields.io/badge/-3.22-0D597F?logo=alpinelinux&logoColor=0D597F&labelColor=222222)](https://alpinelinux.org/downloads/)
[![Fedora 42+](https://img.shields.io/badge/-44-51a2da?logo=fedora&logoColor=51a2da&labelColor=222222)](https://getfedora.org/en/server/download/)
[![RedHat 9+](https://img.shields.io/badge/-10-BE0000?logo=redhat&logoColor=BE0000&labelColor=222222)](https://access.redhat.com/downloads)
[![CentOS Stream 9+](https://img.shields.io/badge/-stream_10-262577?logo=centos&logoColor=white&labelColor=222222)](https://www.centos.org/download/)
[![AlmaLinux 10](https://img.shields.io/badge/-10-0F4266?logo=almalinux&logoColor=white&labelColor=222222)](https://almalinux.org/get-almalinux/)
[![openSUSE 15.6+](https://img.shields.io/badge/-15.6-73BA25?logo=openSUSE&logoColor=73BA25&labelColor=222222)](https://get.opensuse.org/)
[![SUSE Enterprise 15 SP6](https://img.shields.io/badge/-15%20SP6-0C322C?logo=suse&logoColor=30BA78&labelColor=222222)](https://www.suse.com/download/sles/)
[![Linux Mint 21+](https://img.shields.io/badge/-21-87CF3E?logo=linux%20mint&logoColor=87CF3E&labelColor=222222)](https://linuxmint.com/download.php)
[![Arch](https://img.shields.io/badge/-Arch-1793D1?logo=archlinux&logoColor=1793D1&labelColor=222222)](https://archlinux.org/download/)

#### Required Frameworks
##### All Operating Systems
[![.NET](https://img.shields.io/badge/-10.0.0-5C2D91?logo=.NET&logoColor=white&labelColor=222222)](https://dotnet.microsoft.com/download/dotnet/10.0)

##### Windows
[![VC++ Redistributable v14](https://img.shields.io/badge/-Redist%20v14-00599C?logo=cplusplus&logoColor=white&labelColor=222222)](https://aka.ms/vc14/vc_redist.x64.exe)

#### Hardware

| Use | vCPU | RAM | Storage |
|---|---|---|---|
| Development / test | 2 **dedicated** | 2 GB | SSD |
| Small live shard (< 50 concurrent) | 4 dedicated | 4 GB | NVMe |
| Medium (50–200) | 4–8 | 8 GB | NVMe |
| Large (200+) | 8+, high clock | 16 GB+ | NVMe |

See [dev-docs/server-requirements.md](dev-docs/server-requirements.md) for more information.

#### Development
[![git](https://img.shields.io/badge/-git-F05032?logo=git&logoColor=F05032&labelColor=222222)](https://git-scm.com/downloads)
[![.NET](https://img.shields.io/badge/-%2010.0.100%20SDK-5C2D91?logo=.NET&logoColor=white&labelColor=222222)](https://dotnet.microsoft.com/download/dotnet/10.0)

#### Supported IDEs
<p align="left">
  <a href="https://www.jetbrains.com/rider/download"><img height="64" title="Jetbrains Rider 2025.3+" alt="Jetbrains Rider 2025.3+" src="https://cdn.muo.gg/gh/jetbrains-rider.svg"></a>
  <a href="#"><img alt="space" width="32" src="https://cdn.muo.gg/gh/space.png"></a>
  <a href="https://code.visualstudio.com/download"><img height="64" title="VSCode" alt="VSCode" src="https://cdn.muo.gg/gh/vscode.svg"></a>
  <a href="#"><img alt="space" width="32" src="https://cdn.muo.gg/gh/space.png"></a>
  <a href="https://visualstudio.microsoft.com/vs/community/"><img height="64" title="Visual Studio 2026" alt="Visual Studio 2026" src="https://cdn.muo.gg/gh/vs2026.svg"></a>
</p>

## Getting Started
- Install prerequisite [requirements](https://github.com/modernuo/ModernUO#requirements)
- Clone this repository (or download the [latest](https://github.com/modernuo/ModernUO/archive/refs/heads/main.zip)):
  - `git clone https://github.com/modernuo/ModernUO.git`
- Open `ModernUO.sln` to start developing

## Building/Publishing

#### Interactive Mode (Recommended for new users)
Run `./publish.cmd` (Windows) or `./publish.sh` (Linux/macOS) with no arguments to launch the guided build tool. It will:
- Check prerequisites (.NET SDK, native libraries)
- Walk you through configuration and platform selection
- Build and publish the server to the `Distribution` directory
- Show deployment instructions for cross-compiled builds

#### Command Line
```shell
./publish.cmd [release|debug] [os] [arch]
```
- `os` - [Supported operating systems](https://github.com/dotnet/core/blob/main/release-notes/10.0/supported-os.md)
  - `win` - [Windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows)
  - `osx` - [macOS](https://learn.microsoft.com/en-us/dotnet/core/install/macos)
  - `linux` - [Linux](https://learn.microsoft.com/en-us/dotnet/core/install/linux)
- `arch`
  - `x64` - Intel/AMD 64-bit
  - `arm64` - ARM 64-bit

## Linux Prerequisites
### Fedora, CentOS, RHEL, AlmaLinux, Rocky, etc
```shell
dnf upgrade --refresh -y
# RHEL-family distributions (CentOS Stream, AlmaLinux, Rocky, RHEL) need CRB and EPEL enabled.
# Fedora skips this block.
dnf install -y dnf-plugins-core
dnf config-manager --set-enabled crb
dnf install -y epel-release
# Prerequisites
dnf install -y findutils libicu libdeflate libargon2 tzdata
```

### Ubuntu, Debian, etc
```shell
apt-get update -y
# The ICU runtime package carries the ABI version in its name (libicu74, libicu76, …) and has no
# stable alias, so match it by pattern rather than pinning a release-specific name.
apt-get install -y '^libicu[0-9]+$' libdeflate0 libargon2-1 tzdata
```

Only the runtime libraries are needed — the `-dev`/`-devel` packages are not. Run
`./build-tool --check-prereqs` to check the current machine and print the exact packages your
release needs.

`zstd` is not listed because ZstdNet bundles `libzstd` for every platform, and `liburing` is not
listed because IORingGroup issues `io_uring` syscalls directly.

If the shard's configured time zone is a legacy alias such as `US/Eastern`, Debian 12 and Ubuntu
24.04 also need `tzdata-legacy`. See [Platform Prerequisites](dev-docs/platform-prerequisites.md)
for what each dependency is for and what breaks without it.

## OSX Requirements
```shell
brew install icu4c libdeflate argon2
```

## Running the Server
- Follow the [publish](https://github.com/modernuo/ModernUO#buildingpublishing) instructions
- The `Distribution` directory is portable — copy it to your production server for deployment
- Run `ModernUO.exe` or `dotnet ModernUO.dll` from the `Distribution` directory
- On first run, the server will prompt you to configure game data file locations

## Troubleshooting / FAQ
- See [FAQ](./FAQ.md)

## Want to sponsor?
Thank you for supporting us! You can find out how by visiting the [sponsors](./SPONSORS.md) page.

## Collaborators
[![Kamron Batman](https://images.weserv.nl/?url=avatars.githubusercontent.com/u/3953314&h=64&w=64&fit=cover&mask=circle&maxage=1d)](https://github.com/kamronbatman)
[![Mark1145](https://images.weserv.nl/?url=avatars.githubusercontent.com/u/15312181&h=64&w=64&fit=cover&mask=circle&maxage=1d)](https://github.com/mark1145)

## Thanks
- RunUO Team & Community
- [Voxpire](https://github.com/Voxpire), the ServUO Team & Community
- [Karasho](https://github.com/andreakarasho), [Jaedan](https://github.com/jaedan) and the ClassicUO Community

<br/><br/>
<p align=center>Development Tools & Plugins provided with &hearts; by<br/><br/><a href="https://www.jetbrains.com/?from=ModernUO"><img src="https://cdn.muo.gg/gh/jetbrains.svg" height="64px" alt="JetBrains" title="JetBrains" /></a><br/>
<a href="https://material-theme.com/"><img src="https://github.com/AtomMaterialUI/material-theme-issues/raw/master/logo.svg" width="64px" alt="Material Theme" title="Material Theme"></a></p>

## Code Signing Policy

Free code signing provided by [SignPath.io](https://about.signpath.io), certificate by [SignPath Foundation](https://signpath.org).

This program will not transfer any information to other networked systems unless specifically requested by the user or the person installing or operating it

### Teams & Roles
Approvers & Committers: [Development Team](https://github.com/orgs/modernuo/teams/development-team)

