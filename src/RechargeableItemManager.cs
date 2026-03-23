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
    public static class RechargeableItemManager
    {
        private static BreakableItemRelationships rechargeableHandler;

        public static void Init()
        {
            rechargeableHandler = new();
        }

        public static void AddItemRelationship(ItemDef.Pair[] relationships)
        {
            rechargeableHandler.addRelationship(relationships);
        }
        public static void AddItemRelationship(ItemDef unbroken, ItemDef broken)
        {
            rechargeableHandler.addRelationship(unbroken, broken);
        }

        public static ItemDef GetConsumedItem(ItemDef item)
        {
            return rechargeableHandler.GetConsumed(item);
        }
        public static ItemDef GetChargedItem(ItemDef item)
        {
            return rechargeableHandler.GetUnconsumed(item);
        }
        
        public static int DischargeItem(Inventory inventory, ItemDef item, int limit)
        {
            return rechargeableHandler.Consume(inventory, item, limit);
        }
        public static int RechargeItem(Inventory inventory, ItemDef item, int limit)
        {
            return rechargeableHandler.Restore(inventory, item, limit);
        }

        public static void ManualRechargeEvent()
        {
            
        }
        public static void ManualDischargeEvent()
        {
            
        }
    }
}