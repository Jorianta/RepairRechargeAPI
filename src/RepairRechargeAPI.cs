using System;
using System.Linq;
using BepInEx;
using Newtonsoft.Json.Utilities;
using R2API;
using R2API.Utils;
using RepairRechargeAPI.classes;
using RoR2;
using UnityEngine;

namespace RepairRechargeAPI
{
    [BepInDependency("com.rune580.riskofoptions")]

    // Soft Dependencies
    //[BepInDependency(LookingGlass.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]

    public class RepairRechargeAPI : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Braquen";
        public const string PluginName = "Repair_Recharge_API";
        public const string PluginVersion = "1.0.1";

        public static BepInEx.PluginInfo pluginInfo;

        internal static AssetBundle assetBundle;
        internal static string assetBundleDir => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pluginInfo.Location), "repairrechargeassets");

        private static BreakableItemRelationships breakableHandler;
        private static RechargeableItemRelationships rechargeableHandler;

        public void Awake()
        {
            breakableHandler = new();
            rechargeableHandler = new();
        }

        public static void AddBreakableItemRelationship(ItemDef.Pair[] relationships)
        {
            breakableHandler.addRelationship(relationships);
        }
        public static void AddBreakableItemRelationship(ItemDef unbroken, ItemDef broken)
        {
            breakableHandler.addRelationship(unbroken, broken);
        }
        public static void AddRechargeableItemRelationship(ItemDef.Pair[] relationships)
        {
            rechargeableHandler.addRelationship(relationships);
        }
        public static void AddRechargeableItemRelationship(ItemDef charged, ItemDef consumed)
        {
            rechargeableHandler.addRelationship(charged, consumed);
        }  

        public static ItemDef GetBroken(ItemDef item)
        {
            return breakableHandler.GetConsumed(item);
        }
        public static ItemDef GetFixed(ItemDef item)
        {
            return breakableHandler.GetUnconsumed(item);
        }
        public static ItemDef GetUncharged(ItemDef item)
        {
            return rechargeableHandler.GetConsumed(item);
        }
        public static ItemDef GetCharged(ItemDef item)
        {
            return rechargeableHandler.GetUnconsumed(item);
        }

        public static int BreakItem(CharacterMaster master, ItemDef item, int count, bool targetTemporary = false)
        {
            return breakableHandler.Consume(master.inventory,item, count, targetTemporary);
        }
        public static int DischargeItem(CharacterMaster master, ItemDef item, int count, bool targetTemporary = false)
        {
            return rechargeableHandler.Consume(master.inventory,item, count, targetTemporary);
        }
        public static int RepairItem(CharacterMaster master, ItemDef item, int count, bool targetTemporary = false)
        {
            return breakableHandler.Restore(master.inventory,item, count, targetTemporary);
        }
        public static int RechargeItem(CharacterMaster master, ItemDef item, int count, bool targetTemporary = false)
        {
            return rechargeableHandler.Restore(master.inventory,item, count, targetTemporary);
        }
    }
}

