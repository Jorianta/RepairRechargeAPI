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
    internal class BreakableItemRelationships : BaseConsumableRelationshipHandler
    {

        protected ItemDef.Pair[] _vanillaRelationships = [

            new() {itemDef1 = RoR2Content.Items.ExtraLife, itemDef2 = RoR2Content.Items.ExtraLifeConsumed }, //Dios
            new() {itemDef1 = DLC1Content.Items.FragileDamageBonus, itemDef2 = DLC1Content.Items.FragileDamageBonusConsumed }, //Watch
            new() {itemDef1 = DLC1Content.Items.HealingPotion, itemDef2 = DLC1Content.Items.HealingPotionConsumed }, //Elixer
            new() {itemDef1 = DLC1Content.Items.ExtraLifeVoid, itemDef2 = DLC1Content.Items.ExtraLifeVoidConsumed }, //Larvae
        ];

        protected override ItemDef.Pair[] VanillaRelationships
        {
            get {
                return _vanillaRelationships;
            }
        }
    }
}

