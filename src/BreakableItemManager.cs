using System.Collections.Generic;
using EntityStates.RoboBallBoss.Weapon;
using IL.RoR2.ContentManagement;
using RepairRechargeAPI.classes;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RepairRechargeAPI
{
    public class BreakableItemManager : TransformationRelationshipManager
    {

        public static ItemRelationshipType BreakableItemRelationship
        {
            get => breakableHandler.itemRelationshipType;
        }
        private static BreakableItemManager breakableHandler;
        public static readonly ItemTransformationTypeIndex BreakTransformation = 0;
        //NOTE THIS IS THE SAME TYPE AS RECHARGING, BECAUSE THERE IS NOT A VANILLA TYPE FOR REPAIRING
        public static readonly ItemTransformationTypeIndex RepairTransformation = (ItemTransformationTypeIndex)7;

        public static void Init(ItemRelationshipType itemRelationshipType)
        {
            breakableHandler = new(itemRelationshipType);
        }

        protected BreakableItemManager(ItemRelationshipType itemRelationship) : base(itemRelationship){}

       internal static ItemRelationshipProvider GetVanillaRelationships()
        {
            ItemDef dio = Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/ExtraLife/ExtraLife.asset").WaitForCompletion();
            ItemDef dioConsumed = Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/ExtraLife/ExtraLifeConsumed.asset").WaitForCompletion();

            ItemDef watch = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/FragileDamageBonus/FragileDamageBonus.asset").WaitForCompletion();
            ItemDef watchConsumed = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/FragileDamageBonus/FragileDamageBonusConsumed.asset").WaitForCompletion();

            ItemDef potion = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/HealingPotion/HealingPotion.asset").WaitForCompletion();
            ItemDef potionConsumed = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/HealingPotion/HealingPotionConsumed.asset").WaitForCompletion();

            ItemDef larva = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/ExtraLifeVoid/ExtraLifeVoid.asset").WaitForCompletion();
            ItemDef larvaConsumed = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/ExtraLifeVoid/ExtraLifeVoidConsumed.asset").WaitForCompletion();

            ItemRelationshipProvider itemRelationshipProvider = breakableHandler.createProvider(
                "VANILLA_BREAKABLERELATIONSHIPS",
                [
                    new(){itemDef1 = dio, itemDef2 = dioConsumed},
                    new(){itemDef1 = watch, itemDef2 = watchConsumed},
                    new(){itemDef1 = potion, itemDef2 = potionConsumed},
                    new(){itemDef1 = larva, itemDef2 = larvaConsumed}
                ]

            );
            return itemRelationshipProvider;
        }

        public static List<ItemIndex> ListUnbrokenStacks(Inventory inventory)
        {
            return breakableHandler.ListTransformableStacks(inventory);
        }
        public static List<ItemIndex> ListBrokenStacks(Inventory inventory)
        {
            return breakableHandler.ListTransformedStacks(inventory);
        }

        public static ItemIndex GetBrokenItem(ItemIndex item)
        {
            return breakableHandler.GetTransformed(item);
        }
        public static ItemIndex GetFixedItem(ItemIndex item)
        {
            return breakableHandler.GetUntransformed(item);
        }
        
        public static int BreakItem(Inventory inventory, ItemIndex item, int limit, bool allowTemp = false)
        {
            return breakableHandler.Transform(inventory, item, limit, allowTemp);
        }
        public static int FixItem(Inventory inventory, ItemIndex item, int limit, bool allowTemp = false)
        {
            return breakableHandler.Restore(inventory, item, limit, allowTemp);
        }
    }
}

