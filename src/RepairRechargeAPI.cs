using System;
using System.Linq;
using System.Reflection;
using BepInEx;
using IL.RoR2.ContentManagement;
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
        public const string PluginVersion = "1.0.0";
        public static BepInEx.PluginInfo pluginInfo;

        internal static AssetBundle assets;
     
        public void Awake()
        {   
            Log.Init(Logger);
            
            Log.Debug("Loading Item Relationship Types");
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RepairRechargeAPI.itemrelationships.bundle"))
            {
                assets = AssetBundle.LoadFromStream(stream);
            }

            RepairRechargeAPIContent.Init();

            // test();
        }


        private static void test()
        {
            On.RoR2.CharacterMaster.OnServerStageBegin += (o,master,stage) =>
            {
                o(master,stage);


                var targets = BreakableItemManager.ListBrokenStacks(master.inventory);

                foreach (var item in targets)
                {
                    BreakableItemManager.FixItem(master.inventory, item, master.inventory.GetItemCountEffective(item));
                }
            };

            On.RoR2.GlobalEventManager.ProcessHitEnemy += (o,globalevtmanager,damageInfo,victim) =>
            {
                o(globalevtmanager,damageInfo,victim);

                if(! (victim?.TryGetComponent(out CharacterBody victimBody) ?? false) || ! victimBody.inventory ) return;

                var targets = BreakableItemManager.ListUnbrokenStacks(victimBody.inventory);

                if(targets.Count() <= 0) return;

                Util.ShuffleList(targets);

                BreakableItemManager.BreakItem(victimBody.inventory, targets[0], 3);

                
            };

            On.RoR2.GlobalEventManager.ProcessHitEnemy += (o,globalevtmanager,damageInfo,victim) =>
            {
                o(globalevtmanager,damageInfo,victim);

                if(! (damageInfo?.attacker?.TryGetComponent(out CharacterBody attackerBody) ?? false) || ! attackerBody.inventory ) return;

                var targets = BreakableItemManager.ListBrokenStacks(attackerBody.inventory);
                
                if(targets.Count() <= 0) return;

                Util.ShuffleList(targets);

                BreakableItemManager.FixItem(attackerBody.inventory, targets[0], 3);
            };
        }
    }
}

