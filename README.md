# Repair Recharge API
A utility mod for developers that implements a unified api for adding and interacting with items that break (e.g. delicate watches) and items that recharge (e.g. sale star).

## Use Cases / Features
This mod adds two item relationship types to the game, like the one that controls void item transformations: The breakableItemRelationshipType, for items like Delicate Watches and Dios which are consumed under certain conditions and do not come back by their own functionality, and the rechargeableItemRelationshipType, for items like Sale Star which can be consumed but become usable again by themselves when certain conditions are met.

These relationships allow mod creators to identify the consumed and unconsumed versions of these items to "repair" or "recharge" them, or forcibly consume them as is desired. 
Mod creators can also add these relationships to their own custom items through ItemRelationshipProviders, thus allowing any mods that use this api to interact with them just as easily as with vanilla items.

Thinking of using this mod? Here's some advice on how you might implement it:

## I want to repair/recharge items with my mod!
Use the BreakableItemManager and RechargeableItemManagers classes provide several helpful static methods to interact with these items.

You can use
    BreakableItemManager::getBrokenItem(ItemIndex item);<br>
    BreakableItemManager::getFixedItem(ItemIndex item);<br>
and
    RechargeableItemManager::getDischargedItem(ItemIndex item);<br>
    RechargeableItemManager::getChargedItem(ItemIndex item);<br>
to get the consumed/unconsumed counterpart of any item if it exists (returns ItemIndex.None if it doesn't)
    
If you want a list of consumable/consumed items from an inventory, use:
    BreakableItemManager::ListUnbrokenStacks(Inventory inventory);<br>
    BreakableItemManager::ListBrokenStacks(Inventory inventory);<br>
    RechargeableItemManager::ListChargedStacks(Inventory inventory);<br>
    RechargeableItemManager::ListDischarged(Inventory inventory);<br>


And if you like, you can forcibly transform breakable/rechargeable items between their two states with these helper functions:
    BreakableItemManager::breakItem(CharacterBody master, ItemDef item, int count, bool allowTemp = false);<br>
    BreakableItemManager::repairItem(CharacterBody master, ItemDef item, int count, bool allowTemp = false);<br>
    RechargeableItemManager::dischargeItem(CharacterBody master, ItemDef item, int count, bool allowTemp = false);<br>
    RechargeableItemManager::rechargeItem(CharacterBody master, ItemDef item, int count, bool allowTemp = false);<br>
You can also use the vanilla ItemTransformation struct in conjunction with the other methods to achieve finer-tuned control of transformations if you prefer.

## I add items that break with my mod!


