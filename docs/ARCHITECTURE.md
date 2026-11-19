# Architecture

This describes the code as it is today on `main`. The planned changes are in [ROADMAP.md](ROADMAP.md).

All gameplay code lives in `Adventure&Co/Assets/Scripts/` and compiles into the default
`Assembly-CSharp` (there are no assembly definitions yet).

## Folder map

| Folder | Namespace | Contents |
| --- | --- | --- |
| `Abstract/` | `Abstract` | `Stats`: base health / damage / walk speed for anything that fights |
| `Player/` | `Player` | `PlayerController` (movement, jump, attack input), `PlayerCombat`, `PlayerStats` |
| `NPC/Enemies/` | `NPC.Enemies` | `EnemyMovement` (patrol + chase), `EnemyCombat` (bite attack), `EnemyStats`, `HealthbarBehaviour` |
| `NPC/Shopkeeper/` | `NPC.Shopkeeper` | `Shopkeeper`: opens the shop widget with its item list |
| `Items/` | `Items` | `MasterItem` (item prefab component) and empty per-item subclasses |
| `Items/pickups/` | `Items.pickups` | `Pickup`: world object you press **E** on to add an item |
| `Structures/` | `Structures` | `ItemInfo` (ScriptableObject with the item data), `InventorySlot` (item + amount) |
| `Enums/` | `Enums` | `ItemCategory`, `ItemRarity` |
| `Interfaces/` | `Interfaces` | `ICombat` |
| `Static/` | `Static` | `UniqueIDGenerator`: runtime-only incrementing IDs |
| `_Inventory/` | `_Inventory` / global | Inventory, storage (chests), crafting, shop logic **and** their UI |
| `UI/` | global | `UiHandle`: the floating name tag over interactables |
| `Camera/` | global | `CameraFollow` |
| `ScriptableObjects/` | (assets) | `ItemInfo` assets: HealthPotion, Map, Ring, Crafting/ScrapWood, Crafting/Helmet01 |

> Several classes (`ShopWidget`, `ShopEntry`, `Chest`, `UiHandle`, `CameraFollow`, `Map`,
> `MainUIController`) are in the global namespace. **Don't just move them into a namespace.**
> Button `OnClick` bindings in `MainUi.prefab` / `SampleScene.unity` store the type name, so those
> bindings would break silently. Move a class only when you rewire the UI that uses it.

## Core data model

```
ItemInfo (ScriptableObject)          what an item is: name, icon, category, rarity, price,
   ▲                                  stackable?, usable?, crafting recipe
   │ .info
MasterItem (MonoBehaviour on an       the item's identity: inventories hold references to
   item prefab, e.g. HealthPotion)    these prefabs, never instances
   ▲
   │ .ItemClass
InventorySlot { ItemClass, Amount }   one stack in a container
```

- **Identity is the prefab reference.** Two stacks are "the same item" when
  `slotA.ItemClass == slotB.ItemClass`.
- **Recipes** sit on the output item: `ItemInfo.Recipe` is an `InventorySlot[]` of ingredients.
  `ItemInfo.Craftable` marks items that can show up in the crafting menu.
- **Prices:** `ItemInfo.Price` is the buy price. `RetailPrice` (75% of it) is what the shop pays you.
- **Max stack size** is 64. It's hard-coded in both `Inventory` and `Storage`.

## Systems

### Player and combat
- `PlayerController` reads input in `Update` and applies velocity in `FixedUpdate`
  (`Rigidbody2D.linearVelocity`). Grounding works by trigger contact with objects tagged `Ground`.
- Attacks only fire Animator triggers (`AttackingBasic`, `AttackingStrong`). Damage is applied by
  **animation events** that call `PlayerCombat.DealDamage()`, which overlap-circles the `Enemies`
  layer at `attackPoint`.
- Enemies attack the same way: `EnemyCombat` triggers the `Attack` animation when the player is
  inside its circle, and an animation event calls `DealDamage()`.
- `EnemyMovement` ping-pongs between `patrolPoints`. If it's aggressive, it chases the player while
  the player is inside its trigger.
- Death only sets the Animator's `isDead` bool. Nothing despawns and nothing drops loot yet.

### Inventory (`_Inventory/Inventory.cs`)
- Lives on the `Inventory` GameObject in the scene and holds `InventorySlot[] Slots` (null = empty).
- `AddItem` fills partial stacks first, then empty slots. It returns `(Success, Remainder)` and
  queues the "item obtained" toast through `MainWidget`.
- It also holds the player's **coins** (start: 200) and the **unlocked crafting recipes**
  (`startingCraftableItems`).
- After every change it calls `UpdateSlotAtIndex`, which redraws the matching `InventoryUISlot`,
  and it refreshes the crafting menu.

### Inventory UI
- `MainWidget` is the bootstrapper on `MainUi`. It wires `Inventory` ↔ `InventoryGrid`, generates
  slots and initialises the crafting menu.
- `InventoryGrid` instantiates one `InventoryUISlot` per slot.
- `InventoryUISlot` handles hover (shows `DetailUI`), right-click (`ActionMenu`), double-click
  (use) and drag/drop through `EventTrigger` entries added at runtime. Drop targets are found with
  `Physics2D.OverlapPointAll` against colliders on the UI slots.
- `ActionMenu` offers Use / Split / Throw. Throw becomes **Sell** while a shop is open.
- `RemoveFromInventory` is the "how many?" dialog for throwing or selling stacks.
- `MainUIController` toggles the inventory (**I**) and crafting (**V**) canvases.

### Storage / chests
- Every `Chest` in the world keeps its own `InventorySlot[]`.
- On interaction the chest pushes its contents into the single shared `Storage` component, which
  backs the storage UI (`StorageGrid` / `StorageUISlot`). As items move, `Storage` writes changes
  back to the active chest (looked up by ID in its serialized `chests` list).
- `Storage` is essentially a copy of `Inventory`, and `StorageUISlot` is a copy of `InventoryUISlot`.

### Crafting
- `CraftingMenu` lists `Inventory.UnlockedCraftableItems` as `CraftableEntry` rows. Selecting one
  shows its recipe as `RecipeEntry` rows, which turn red or green from the current inventory counts.
- **Craft** adds the output first. If that succeeds, it removes the ingredients.

### Shops
- `Shopkeeper` (press **E** in range) sends its `List<MasterItem>` to the shared `ShopWidget`.
- `ShopWidget` builds one `ShopEntry` per item, with an amount slider and a Buy button.
  Stock is unlimited.
- Selling goes through the inventory's action menu while `ShopWidget.IsShopOpen` is true.

### Interaction
- `Pickup`, `Shopkeeper` and `Chest` each detect the player with `OnTriggerEnter2D` and then
  check `Input.GetKeyDown(KeyCode.E)` in their own `Update`.
- `UiHandle` shows or hides the floating name label.

## Scene wiring (SampleScene)

`MainUi` (Canvas) is the UI root. It holds `MainWidget` and `MainUIController`, with child canvases
for the inventory grid, action menu, throw dialog, detail tooltip, dragged-item ghost, crafting
menu, shop widget, storage UI and the "item obtained" toast. Most cross-references are set by hand
in the Inspector, so if a reference is missing you get a `NullReferenceException` at Start.
