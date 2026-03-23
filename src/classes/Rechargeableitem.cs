using System;
using System.Linq;
using BepInEx;
using Newtonsoft.Json.Utilities;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace RepairRechargeAPI.classes
{
    internal class RechargeableItemRelationships : BaseConsumableRelationshipHandler
    {

        protected ItemDef.Pair[] _vanillaRelationships = [
            new() {itemDef1 = DLC1Content.Items.RegeneratingScrap, itemDef2 = DLC1Content.Items.RegeneratingScrapConsumed }, //Regen Scrap
            new() {itemDef1 = DLC2Content.Items.LowerPricedChests, itemDef2 = DLC2Content.Items.LowerPricedChestsConsumed }, //Sale Star
            new() {itemDef1 = DLC2Content.Items.TeleportOnLowHealth, itemDef2 = DLC2Content.Items.TeleportOnLowHealth }, //Transmitter
        ];

        protected override ItemDef.Pair[] VanillaRelationships
        {
            get {
                return _vanillaRelationships;
            }
        }
    }
}