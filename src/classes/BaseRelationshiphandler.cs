using System;
using System.Linq;
using System.Reflection.Emit;
using BepInEx;
using Newtonsoft.Json.Utilities;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace RepairRechargeAPI.classes
{
    internal abstract class BaseConsumableRelationshipHandler
    {
        public ItemRelationshipType itemRelationshipType;

        protected virtual ItemDef.Pair[] VanillaRelationships
        {
            get {
                return [];
            }
        }

        protected BaseConsumableRelationshipHandler()
        {
            InitRelationships();
        }
        internal void InitRelationships()
        {
            itemRelationshipType = new ItemRelationshipType();
            ContentAddition.AddItemRelationshipType(itemRelationshipType);

            addRelationship(VanillaRelationships);
        }
        
        public void addRelationship(ItemDef unconsumed, ItemDef consumed)
        {
            addRelationship(new ItemDef.Pair{itemDef1 = unconsumed, itemDef2 = consumed});
        }
        public void addRelationship(params ItemDef.Pair[] relationships)
        {
            if(!ItemCatalog.availability.available) ItemCatalog.availability.CallWhenAvailable(() =>{
                addRelationship(relationships);
            }); 


            if(!ItemCatalog.itemRelationships.ContainsKey(itemRelationshipType))
            {
                throw new Exception("Unitialized item relationship");
            };

            var relationshipArray = ItemCatalog.itemRelationships[itemRelationshipType];
            relationshipArray = relationshipArray.Union(relationships).ToArray();

            foreach (ItemDef.Pair itemDef in ItemCatalog.itemRelationships[itemRelationshipType])
            {
                Debug.Log("Added Relationship to type " + itemRelationshipType.ToString() +": (" + itemDef.itemDef1 + ", " + itemDef.itemDef2 + ")");
            }
        }

        public ItemDef GetUnconsumed(ItemDef consumed)
        {
            ItemDef.Pair pair = Array.Find(ItemCatalog.GetItemPairsForRelationship(itemRelationshipType).ToArray(), 
                (ItemDef.Pair pair) => pair.itemDef2 == consumed
            );

            return pair.itemDef1;
        }
        public ItemDef GetConsumed(ItemDef unconsumed)
        {
            ItemDef.Pair pair = Array.Find(ItemCatalog.GetItemPairsForRelationship(itemRelationshipType).ToArray(), 
                (ItemDef.Pair pair) => pair.itemDef1 == unconsumed
            );

            return pair.itemDef2;
        }

        public int Consume(Inventory inventory, ItemDef item, int count, bool temp = false)
        {
            ItemDef consumed = GetConsumed(item);
            if(consumed == null) return 0;

            return Convert(inventory, item.itemIndex, consumed.itemIndex, count);
        }
        public int Restore(Inventory inventory, ItemDef item, int count, bool temp = false)
        {
            ItemDef restored = GetUnconsumed(item);
            if(restored == null) return 0;

            return Convert(inventory, item.itemIndex, restored.itemIndex, count);
        }
        protected static int Convert(Inventory inventory, ItemIndex from, ItemIndex to, int count)
        {
            Inventory.ItemTransformation itemTransformation = new Inventory.ItemTransformation
            {
                originalItemIndex = from,
                newItemIndex = to,
                minToTransform = 0,
                maxToTransform = count
            };
            if (itemTransformation.TryTransform(inventory, out var result))
            {
                return result.totalTransformed;
            }
            return 0;
        }
    }
}

