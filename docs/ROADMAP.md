# Roadmap, Known Issues & Tech Debt

## Feature branches

All four branch off `main`. They overlap heavily (inventory, crafting and shops all touch
`Inventory.cs`, the item model and `MainUi.prefab`), so **merge in this order** and update each
remaining branch from `main` after every merge:

| Order | Branch | Goal |
| --- | --- | --- |
| 1 | `feature/inventory-core` | New item and container foundation that everything else builds on |
| 2 | `feature/crafting` | Recipe assets, stations, discovery, variable outcomes |
| 2 | `feature/shops` | Data-driven stock, buy/sell/buyback, pricing rules |
| 3 | `feature/art-direction` | 4Story-style UI skin, palette, lighting (URP 2D), new texture import |

`feature/art-direction` can start any time on lighting, tiles and the palette. Leave the UI skin
until the inventory, crafting and shop UIs stop moving.

### 1. feature/inventory-core (do this first)
- [ ] Turn items into pure data: an `ItemDefinition` ScriptableObject (replacing the
      `MasterItem`-prefab + `ItemInfo` pair) with a stable string ID, so saves and recipes don't
      need prefabs.
- [ ] Add a single `ItemContainer` class (slots, add/remove/split/merge/move-to-other-container)
      used by the inventory, chests and later shop stock. This removes the ~300 duplicated lines
      between `Inventory` and `Storage`, and between `InventoryUISlot` and `StorageUISlot`.
- [ ] Fire events (`OnSlotChanged`) instead of containers calling UI code directly.
- [ ] Do drag/drop with `IBeginDragHandler` / `IDropHandler` (EventSystem) instead of
      Physics2D colliders on UI.
- [ ] One `Interactable` + `PlayerInteractor` (nearest target, single **E** press) replacing the
      per-object `Update` polling in `Pickup` / `Chest` / `Shopkeeper`.
- [ ] Derive the rarity colour from `ItemRarity` (one table) instead of per-item `RarityColor`.
- [ ] Equipment slots → `Stats`.

### 2a. feature/crafting
- [ ] `Recipe` ScriptableObject: inputs, output, station, unlock condition.
- [ ] Crafting stations as interactables. The menu filters by station.
- [ ] Recipe discovery (NPC, quest, scroll) plus "known recipes" in the save.
- [ ] Ingredient quality → output rarity / stat rolls.
- [ ] Enhancement (+N upgrades) on equipment.

### 2b. feature/shops
- [ ] `ShopInventory` data: stock, limits, restock, buy/sell multipliers.
- [ ] Buyback list.
- [ ] Event-driven `ShopEntry` (today it polls in `Update`).
- [ ] A dedicated sell flow, separate from "throw away" in `ActionMenu` / `RemoveFromInventory`.

### 3. feature/art-direction
- [ ] Decide on URP + 2D Renderer (and migrate materials) before importing the big texture drop.
- [ ] UI kit: panel and frame sprites, fonts, slot states, rarity palette.
- [ ] Set up Git LFS before adding large texture or audio packs (see WORKFLOW.md).

## Known bugs (on `main` today)

| Area | Bug |
| --- | --- |
| Storage | `Storage.RemoveItemAtIndex` (partial remove) passes the *remaining* amount to `Chest.RemoveItem` instead of the removed amount, so the chest's saved contents drift from what the UI shows. Reopening a chest can duplicate or lose items. |
| Storage | `Storage.IncreaseAmountAtIndex` never writes back to the chest. |
| Inventory ↔ Storage | When stacking between containers, `MoveFromInventoryToStorageIndex` / `MoveFromStorageToInventoryIndex` read the amount with the *other* container's index, which gives wrong amounts and can throw `IndexOutOfRange`. |
| UI | `MainWidget.ShowStorage` / `HideStorage` use `_storageUiCanvas`, which is never assigned. They throw a NullReferenceException if anything calls them. |
| Combat | Player death only plays an animation. Enemies never despawn and drop no loot. |
| IDs | `UniqueIDGenerator` IDs depend on spawn order, so they can't be used for saving. |
| Editor | Unity 6 warns that TextMesh Pro Essential Resources are missing (Window → TextMesh Pro → Import TMP Essential Resources). |

## Tech debt

- No assembly definitions. Everything is in `Assembly-CSharp`.
- Legacy Input Manager (`activeInputHandler: 0`). Consider the Input System package once
  controls settle (gamepad support, rebinding).
- Hard-coded tuning values: starting coins (200), stack size (64), sell ratio (0.75), UI tint colours.
- `ItemRarity.Common = 1255000000`. Enum values are serialized as ints, so renumbering needs an
  asset migration. Fold this into the `ItemDefinition` change.
- Several classes sit in the global namespace (see ARCHITECTURE.md for why they weren't moved yet).
- `_Inventory/` mixes data, logic and UI for four systems. Split it per system as each gets rewritten.
- Only `SampleScene` is in Build Settings. `TownLevel` and `Level_1` are blockouts.
- No save system and no tests.
