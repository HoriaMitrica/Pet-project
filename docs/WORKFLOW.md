# Workflow

## Branches

- `main` is always the integrated, working game. Don't commit to it directly. Merge feature
  branches into it with a PR.
- `feature/<name>` holds one system or piece of work. Branch from `main`, and merge `main` back in
  often (especially after another feature lands).
- Keep feature branches short-lived. Scene and prefab files merge badly, so long-lived branches
  that both edit `SampleScene.unity` or `MainUi.prefab` *will* conflict.

## Unity + Git rules of thumb

- **Always commit `.meta` files together with their asset.** Moving or renaming in Unity keeps
  the GUID. Moving files in Explorer without the `.meta` breaks every reference to them.
- **Don't edit the same scene or prefab on two branches at once.** Unity YAML conflicts are
  painful. If you have to, split UI into nested prefabs (one per widget) so changes land in
  different files.
- For unavoidable scene conflicts, set up Unity's Smart Merge (UnityYAMLMerge):
  ```
  git config merge.unityyamlmerge.name "Unity SmartMerge"
  git config merge.unityyamlmerge.driver "'<UnityEditorPath>/Data/Tools/UnityYAMLMerge.exe' merge -p %O %B %A %A"
  ```
  plus `*.unity merge=unityyamlmerge` and `*.prefab merge=unityyamlmerge` in a `.gitattributes`.
- **Close Unity (or at least save) before switching branches.** Unity watches the files and will
  reimport mid-checkout.
- Ignored and regenerated per machine: `Library/`, `Temp/`, `Logs/`, `UserSettings/`, `*.sln(x)`,
  `*.csproj`, `.vscode/`, `.idea/` (see the root `.gitignore`).

## Large assets (do this before the texture drop)

The repo is about 38 MB today, which is fine. Big texture, audio or font packs will grow history
fast and can never be removed from it. Before importing them, install Git LFS and track binaries:

```
git lfs install
git lfs track "*.png" "*.psd" "*.wav" "*.ogg" "*.mp3" "*.ttf" "*.otf" "*.fbx"
git add .gitattributes
```

Everyone who clones then needs Git LFS installed. Existing PNGs can stay in normal git. LFS
applies to files added after tracking starts.

## Unity MCP (AI-assisted editing)

The project includes **MCP for Unity** (`com.coplaydev.unity-mcp`, see `Packages/manifest.json`).
The repo-root `.mcp.json` points Claude Code at it:

1. Open the project in Unity → *Window → MCP for Unity*.
2. Transport **HTTP Local** (`http://127.0.0.1:8080`) → **Start Server**.
3. Run Claude Code from the repo root. It picks up `.mcp.json` and can read scenes and the
   console, run play mode and edit assets.

## Versions

- Unity **6000.6.7f1** (upgraded from 2022.3.20f1). Open with exactly this version. Opening with an
  older editor will fail or corrupt assets.
