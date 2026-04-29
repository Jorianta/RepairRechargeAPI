using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BepInEx;
using HG;
using Newtonsoft.Json.Utilities;
using R2API;
using R2API.Utils;
using RoR2;
using On;
using UnityEngine;

namespace RepairRechargeAPI.classes
{
    public abstract class TransformationRelationshipManager
    {
        public ItemRelationshipType itemRelationshipType;
        public ItemTransformationTypeIndex itemTransformationType;
        public ItemTransformationTypeIndex itemReverseTransformationType;
        private ItemIndex[] TransformationTable = [];
        private ItemIndex[] ReversionTable = [];

        protected TransformationRelationshipManager(ItemRelationshipType itemRelationship)
        {
            itemRelationshipType = itemRelationship;

            On.RoR2.Items.ContagiousItemManager.Init += (orig) => {
                InitTransformationTable();
                orig();
            };
        }

        protected ItemRelationshipProvider createProvider(string name, ItemDef.Pair[] relationships = null)
        {
            ItemRelationshipProvider ItemRelationshipProvider = ScriptableObject.CreateInstance<ItemRelationshipProvider>();
            ItemRelationshipProvider.relationshipType = itemRelationshipType;
            ItemRelationshipProvider.name = name;
            if(relationships != null) ItemRelationshipProvider.relationships = relationships;

            return ItemRelationshipProvider;
        }

        private void InitTransformationTable()
        {
            TransformationTable = new ItemIndex[ItemCatalog.itemCount];
            ArrayUtils.SetAll(TransformationTable, ItemIndex.None);

            ReversionTable = new ItemIndex[ItemCatalog.itemCount];
            ArrayUtils.SetAll(ReversionTable, ItemIndex.None);

            foreach (ItemDef.Pair itemDef in ItemCatalog.itemRelationships[itemRelationshipType])
            {   
                Log.Debug("Initializing " + itemRelationshipType.name + ": " + itemDef.itemDef1.name +" => "+itemDef.itemDef2.name);
                TransformationTable[(int)itemDef.itemDef1.itemIndex] = itemDef.itemDef2.itemIndex;
                ReversionTable[(int)itemDef.itemDef2.itemIndex] = itemDef.itemDef1.itemIndex;
            }
        }

        public List<ItemIndex> ListTransformableStacks(Inventory inventory)
        {
            return ListItemDomain(inventory, TransformationTable);
        }
        public List<ItemIndex> ListTransformedStacks(Inventory inventory)
        {
            return ListItemDomain(inventory, ReversionTable);
        }
        
        //list items that map to another item
        private List<ItemIndex> ListItemDomain(Inventory inventory, ItemIndex[] table)
        {
            List<ItemIndex> list = [];
            foreach(ItemIndex item in inventory.itemAcquisitionOrder)
            {
                if(table[(int)item] == ItemIndex.None) continue;

                list.Add(item);
            }

            return list;
        }

        public ItemIndex GetUntransformed(ItemIndex consumed)
        {
            return ReversionTable[(int)consumed];
        }
        public ItemIndex GetTransformed(ItemIndex unconsumed)
        {
            return TransformationTable[(int)unconsumed];
        }

        public int Transform(Inventory inventory, ItemIndex item, int count, bool allowTemp = false)
        {
            ItemIndex consumed = GetTransformed(item);
            if(consumed == ItemIndex.None) return 0;

            return Convert(inventory, item, consumed, count, itemTransformationType, allowTemp);
        }
        public int Restore(Inventory inventory, ItemIndex item, int count, bool allowTemp = false)
        {
            ItemIndex restored = GetUntransformed(item);
            if(restored == ItemIndex.None) return 0;

            return Convert(inventory, item, restored, count, itemReverseTransformationType, allowTemp);
        }
        protected int Convert(Inventory inventory, ItemIndex from, ItemIndex to, int count, ItemTransformationTypeIndex type, bool allowTemp = false)
        {
            Inventory.ItemTransformation itemTransformation = new()
            {
                originalItemIndex = from,
                newItemIndex = to,
                minToTransform = 1,
                maxToTransform = count,
                forbidTempItems = !allowTemp,
                transformationType = type
            };
            itemTransformation.TryTransform(inventory, out var result);
            
            return result.totalTransformed;
            
        }
    }
}

