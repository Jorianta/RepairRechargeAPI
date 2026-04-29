using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using BepInEx;
using Newtonsoft.Json.Utilities;
using R2API;
using R2API.Utils;
using RepairRechargeAPI.classes;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RepairRechargeAPI
{
    public class RechargeableItemManager : TransformationRelationshipManager
    {
        public static ItemRelationshipType RechargeableItemRelationship
        {
            get => rechargeableHandler.itemRelationshipType;
        }

        private static RechargeableItemManager rechargeableHandler;

        public static readonly ItemTransformationTypeIndex DischargeTransformation = (ItemTransformationTypeIndex)6;
        public static readonly ItemTransformationTypeIndex RechargeTransformation = (ItemTransformationTypeIndex)7;

        public static void Init(ItemRelationshipType itemRelationship)
        {
            rechargeableHandler = new(itemRelationship);
        }
        protected RechargeableItemManager(ItemRelationshipType itemRelationship) : base(itemRelationship){}

        internal static ItemRelationshipProvider GetVanillaRelationships()
        {
            ItemDef scrap = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/RegeneratingScrap/RegeneratingScrap.asset").WaitForCompletion();
            ItemDef scrapConsumed = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/RegeneratingScrap/RegeneratingScrapConsumed.asset").WaitForCompletion();

            ItemDef transmitter = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC2/Items/TeleportOnLowHealth/TeleportOnLowHealth.asset").WaitForCompletion();
            ItemDef transmitterConsumed = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC2/Items/TeleportOnLowHealth/TeleportOnLowHealthConsumed.asset").WaitForCompletion();

            ItemDef saleStar = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC2/Items/LowerPricedChests/LowerPricedChests.asset").WaitForCompletion();
            ItemDef saleStarConsumed = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC2/Items/LowerPricedChests/LowerPricedChestsConsumed.asset").WaitForCompletion();

            ItemRelationshipProvider itemRelationshipProvider = rechargeableHandler.createProvider(
                "VANILLA_RECHARGEABLERELATIONSHIPS",
                [
                    new(){itemDef1 = scrap, itemDef2 = scrapConsumed},
                    new(){itemDef1 = transmitter, itemDef2 = transmitterConsumed},
                    new(){itemDef1 = saleStar, itemDef2 = saleStarConsumed}
                ]
            );
            
            return itemRelationshipProvider;
            
        }

        public static List<ItemIndex> ListChargedStacks(Inventory inventory)
        {
            return rechargeableHandler.ListTransformableStacks(inventory);
        }
        public static List<ItemIndex> ListDischargedStacks(Inventory inventory)
        {
            return rechargeableHandler.ListTransformedStacks(inventory);
        }

        public static ItemIndex GetDischargedItem(ItemIndex item)
        {
            return rechargeableHandler.GetTransformed(item);
        }
        public static ItemIndex GetChargedItem(ItemIndex item)
        {
            return rechargeableHandler.GetUntransformed(item);
        }
        
        public static int DischargeItem(Inventory inventory, ItemIndex item, int limit, bool allowTemp = false)
        {
            return rechargeableHandler.Transform(inventory, item, limit, allowTemp);
        }
        public static int RechargeItem(Inventory inventory, ItemIndex item, int limit, bool allowTemp = false)
        {
            return rechargeableHandler.Restore(inventory, item, limit, allowTemp);
        }
    }
}