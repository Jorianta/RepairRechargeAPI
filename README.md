# Repair Recharge API
A utility mod for developers that implements a unified api for adding and interacting with items that break like delicate watches and items that are consumed like sale star.

## Use Cases / Features
This mod adds two item relationship types to the game, like the one that controls void item transformations: The breakableItemRelationship, for items like Delicate Watches and Dios which are consumed under certain conditions and do not come back by their own functionality, and the rechargeableItemRelationship, for items like Sale Star which can be consumed but become usable again by themselves when certain conditions are met.

These relationships allow mod creators to identify the consumed and unconsumed versions of these items to "repair" or "recharge" them, or forcibly consume them as is desired. Mod creators can also add these relationships to their own custom items, thus allowing any mods that use this api to interact with them just as easily as with vanilla items.

Additionally, the mod adds several events that related to these item relationships:
    itemBreakEvent(CharacterBody master, ItemDef unconsumed, ItemDef consumed): fired when a breakableItem is consumed (e.g., dropping below 25% health and healing with an elixer)
    itemDischargeEvent(CharacterBody master, ItemDef unconsumed, ItemDef consumed): fired when a rechargeableItem is consumed (e.g., using regen scrap at a printer)
    itemRechargeEvent(CharacterBody master, ItemDef unconsumed, ItemDef consumed): fired when a rechargeableItem returns to its unconsumed state (e.g. regen scrap restoration on stage start)
    itemRepairEvent(CharacterBody master, ItemDef unconsumed, ItemDef consumed): to fire when a breakableItem is "repaired," or returned to its unconsumed version

these events are fired once per item as appropriate whenever one of these utility functions is called:
    breakItem(CharacterBody master, ItemDef item, int count);
    dischargeItem(CharacterBody master, ItemDef item, int count);
    repairItem(CharacterBody master, ItemDef item, int count);
    rechargeItem(CharacterBody master, ItemDef item, int count);

but can also be invoked manually if preferred.
