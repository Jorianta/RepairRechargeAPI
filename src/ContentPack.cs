using System.Collections;
using System.Linq;
using RoR2;
using RoR2.ContentManagement;
using UnityEngine;

namespace RepairRechargeAPI
{
    public class RepairRechargeAPIContent : IContentPackProvider
    {
        internal ContentPack contentPack = new();
        public string identifier => RepairRechargeAPI.PluginGUID;

        internal static void Init()
        {   
            ContentManager.collectContentPackProviders += (addContentPackProvider) => {
                addContentPackProvider(new RepairRechargeAPIContent());
            };
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {   
            var breakableRelatRequest = RepairRechargeAPI.assets.LoadAssetAsync<ItemRelationshipType>("Assets/BreakableItemRelationshipType.asset");
            while (!breakableRelatRequest.isDone)
            {
                yield return null;
            }
            var breakableRelat = (ItemRelationshipType)breakableRelatRequest.GetResult();
            BreakableItemManager.Init(
                breakableRelat
            );
            args.ReportProgress(0.25f);

            var rechargeRelatRequest = RepairRechargeAPI.assets.LoadAssetAsync<ItemRelationshipType>("Assets/RechargeableItemRelationshipType.asset");
            while (!rechargeRelatRequest.isDone)
            {
                yield return null;
            }
            var rechargeRelat = (ItemRelationshipType)rechargeRelatRequest.GetResult();
            RechargeableItemManager.Init(
                rechargeRelat
            );


            contentPack.itemRelationshipTypes.Add([
                breakableRelat, rechargeRelat
            ]);
            args.ReportProgress(0.5f);
            
            yield return null;

            contentPack.itemRelationshipProviders.Add([
                BreakableItemManager.GetVanillaRelationships(),
                RechargeableItemManager.GetVanillaRelationships()
            ]);

            args.ReportProgress(1f);

            yield break;
        }
        //This is the simplest way of implementing this method, only add more stuff if you know what you're doing.
        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            contentPack.identifier = identifier;
            //This is a static method
            ContentPack.Copy(contentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }
        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }
    }
}
