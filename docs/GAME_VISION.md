# Game Vision

## One-liner

A story-driven 2D action-RPG side-scroller. It has the momentum and build-crafting of a roguelite
brawler like *Risk of Rain*, set in a hand-authored medieval-fantasy world with towns, quests and
characters. The look and feel recall classic MMOs like *4Story*.

## Pillars

1. **Combat with momentum.** Readable melee, enemies that push back, and gear that changes *how*
   you fight, not just your numbers. Short runs into hostile zones, then back to town.
2. **A story you move through.** Unlike a pure roguelite, progress is authored: zones unlock
   through story beats, NPCs remember you, and quests explain why you're out in the swamp at all.
3. **Crafting is the progression spine.** Most power comes from what you make, not what drops.
   Materials come from zones, recipes come from the story and from people, and crafted items have
   real choices in them.
4. **A town worth coming back to.** Shops, storage, crafting stations and NPCs make the hub the
   place where a run's loot turns into progress.
5. **The 4Story feel.** Warm, slightly saturated medieval fantasy. Ornate UI frames with gold
   trim, item rarity colours, chunky icons, readable silhouettes. The textures set the mood and
   the UI has to match them.

## Core loop

```
Town (shop, craft, store, take quests)
   │  pick a destination / quest
   ▼
Zone run (fight, gather materials, find recipes & story items)
   │  return or die
   ▼
Back in town (sell, craft upgrades, unlock the next story beat)
```

## Systems we're building toward

### Inventory
- A fixed slot grid that can grow (bags as upgrades), with stack sizes per item.
- Equipment slots (helmet, armour, weapon, rings, ...) that feed `Stats`.
- One container model shared by inventory, chests, shop stock and crafting stations.
- Sort, quick-move (shift-click), and moving whole stacks between containers.

### Crafting (the "interesting" part)
- **Recipes as their own assets**, separate from items. An item can have several recipes, and a
  recipe can need a **station** (workbench, forge, alchemy table).
- **Discovery:** recipes are learned from NPCs, quests, recipe scrolls, or by experimenting.
- **Variable outcomes:** material quality or rarity feeds the result's rarity and stat rolls,
  so crafting is a way to chase better gear and not just a checklist.
- **Upgrade / enhance:** feed materials into existing gear for + levels, a nod to the MMO-style
  enhancement we're going for (risk and reward we can tune).

### Shops
- Shop stock as data: limited quantities, restock timers, shop-specific buy and sell multipliers.
- Buyback for items you just sold.
- Prices driven by rarity, plus reputation or story state (discounts after helping a village).
- One clear buy/sell UI that docks next to the inventory.

### Story & world
- Quest and dialogue systems (planned in the original design).
- Zones: Town (HouseBuilder tiles), Swamp (Swamp tiles, turtle enemy) and more as art arrives.

## Art direction notes

- Current art: Woodcutter player, villagers, swamp enemies, the HouseBuilder and Swamp tilesets,
  and item icons. A lot more texture packs are coming. Bring them in by zone or feature so each
  feature branch only pulls in what it uses.
- The current UI is placeholder Unity grey (`0.45` grey slot tint, default TMP font). The
  art-direction work should define a palette, frame and panel sprites, fonts and rarity colours
  once, and reuse them everywhere.
- Consider switching to **URP with the 2D Renderer** for 2D lights and post-processing (bloom on
  rare drops, warm town lighting, dark swamp). That's a big step towards the mood. Do it early,
  before the asset count grows, because it changes every material.
