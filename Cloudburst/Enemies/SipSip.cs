using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static RoR2.GivePickupsOnStart;

namespace Cloudburst.Enemies
{
    internal sealed class SipSip
    {
        private static GameObject sipSipBody;
        private static GameObject sipSipMaster;
        public static void BuildMegaMushrums()
        {
            sipSipBody = Resources.Load<GameObject>("prefabs/characterbodies/ScavLunar1Body").InstantiateClone("ScavLunar5Body", true);
            sipSipMaster = Resources.Load<GameObject>("prefabs/charactermasters/ScavLunar1Master").InstantiateClone("ScavLunar5Master", true);

            BaseHelpers.RegisterNewBody(sipSipBody);
            BaseHelpers.RegisterNewMaster(sipSipMaster);

            sipSipMaster.GetComponent<CharacterMaster>().bodyPrefab = sipSipBody;

            BaseLogger.Log("SipSip is greedy.");
        }
        private static void AddGreedyItems() {

            ItemInfo ghor = new ItemInfo();
            ghor.count = 5;
            ghor.itemString = "BonusGoldPackOnKill";

            ItemInfo teddie = new ItemInfo();
            teddie.count = 10;
            teddie.itemString = "Bear";

            ItemInfo brittle = new ItemInfo();
            brittle.count = 3;
            brittle.itemString = "GoldOnHit";

            ItemInfo[] array = new ItemInfo[] {
                ghor,
                teddie,
                brittle
            };

            GivePickupsOnStart pickupComp = sipSipMaster.AddComponent<GivePickupsOnStart>();
            pickupComp.equipmentString = "GoldGat";
            pickupComp.itemInfos = array;
        }
    }
}