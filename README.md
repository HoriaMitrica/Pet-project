# Adventure&Co

A story-driven 2D action-RPG side-scroller built in Unity. The moment-to-moment play takes after
roguelite brawlers like *Risk of Rain*: fast movement, enemies closing in, gear that changes how you
play. Everything sits inside an authored story with towns, NPCs and quests. The look is classic
medieval-fantasy MMO, in the spirit of *4Story*.

> Status: early prototype. Movement, melee combat, one enemy type, inventory, crafting, chests and a
> shop all work in `SampleScene`. Nothing has been built for release yet.
>
> Demo of an earlier build: https://youtu.be/dV5KV0XlNGc

## Quick start

| What | Value |
| --- | --- |
| Unity version | **6000.6.7f1** (see `Adventure&Co/ProjectSettings/ProjectVersion.txt`) |
| Project folder | `Adventure&Co/` (open this folder in Unity Hub, not the repo root) |
| Main scene | `Assets/Scenes/SampleScene.unity` (the only scene in Build Settings) |
| Other scenes | `TownLevel.unity`, `Level_1.unity` (level blockouts) |
| Render pipeline | Built-in (2D) |
| Input | Legacy Input Manager |

### Controls (current prototype)

| Key | Action |
| --- | --- |
| A / D or ← / → | Walk |
| Left Shift (held) | Run |
| Space | Jump |
| Z / X | Basic / strong attack |
| E | Interact (pick up, talk to shopkeeper, open chest) |
| I | Toggle inventory |
| V | Toggle crafting menu |
| Right-click slot | Item action menu (use / split / throw or sell) |
| Double-click slot | Use item (inventory) or take item (chest) |
| Drag slot | Move, stack or swap. Drop outside the window to throw away. |

## Documentation

| Doc | What it covers |
| --- | --- |
| [docs/GAME_VISION.md](docs/GAME_VISION.md) | Design pillars, target feel, the systems we're building toward |
| [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) | How the code is organised and how the systems talk to each other |
| [docs/ROADMAP.md](docs/ROADMAP.md) | Feature branches, known bugs and tech debt, in priority order |
| [docs/WORKFLOW.md](docs/WORKFLOW.md) | Git branching, Unity/YAML gotchas, the Unity MCP setup |

## Repository layout

```
.
├── Adventure&Co/          Unity project (Assets/, Packages/, ProjectSettings/)
│   └── Assets/
│       ├── Scripts/       All gameplay C# (see docs/ARCHITECTURE.md)
│       ├── Prefabs/       UI widgets, enemies, NPCs, pickups, item prefabs
│       ├── Scenes/        SampleScene (playground), TownLevel, Level_1
│       ├── Tiles/         HouseBuilder + Swamp tilesets
│       ├── Player Sprites/, NPC_Friendly/, NPC_enemies/, ItemIcons/  Art
│       └── Animations/    Animator controllers & clips
├── docs/                  Project documentation
└── .mcp.json              Unity MCP server config (for Claude Code)
```

## Credits

- Code & design: Horia Mitrica
- Level work: AlleRodie
