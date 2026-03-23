using System;
using System.Linq;
using System.Reflection.Emit;
using BepInEx;
using Newtonsoft.Json.Utilities;
using R2API;
using R2API.Utils;
using RepairRechargeAPI.classes;
using RoR2;
using UnityEngine;

namespace RepairRechargeAPI
{
    public static class BreakableItemManager
    {
        private static BreakableItemRelationships breakableHandler;

        public static void Init()
        {
            breakableHandler = new();
        }

        //unconsumed, then consumed
        public static void AddItemRelationship(ItemDef.Pair[] relationships)
        {
            breakableHandler.addRelationship(relationships);
        }
        public static void AddItemRelationship(ItemDef unbroken, ItemDef broken)
        {
            breakableHandler.addRelationship(unbroken, broken);
        }

        public static ItemDef GetBrokenItem(ItemDef item)
        {
            return breakableHandler.GetConsumed(item);
        }
        public static ItemDef GetFixedItem(ItemDef item)
        {
            return breakableHandler.GetUnconsumed(item);
        }
        
        public static int BreakItem(Inventory inventory, ItemDef item, int limit)
        {
            return breakableHandler.Consume(inventory, item, limit);
        }
        public static int FixItem(Inventory inventory, ItemDef item, int limit)
        {
            return breakableHandler.Restore(inventory, item, limit);
        }
    }
}

