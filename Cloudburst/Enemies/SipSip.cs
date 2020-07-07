using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static RoR2.GivePickupsOnStart;
using static RoR2.ScriptedCombatEncounter;

namespace Cloudburst.Enemies
{
    internal sealed class SipSip
    {
        private static GameObject sipSipBody;
        private static GameObject sipSipMaster;
        private static SpawnCard card;
        public static void BuildSipsip()
        {
            sipSipBody = Resources.Load<GameObject>("prefabs/characterbodies/ScavLunar1Body").InstantiateClone("ScavLunar5Body", true);
            sipSipMaster = Resources.Load<GameObject>("prefabs/charactermasters/ScavLunar1Master").InstantiateClone("ScavLunar5Master", true);

            BaseHelpers.RegisterNewBody(sipSipBody);
            BaseHelpers.RegisterNewMaster(sipSipMaster);

            sipSipMaster.GetComponent<CharacterMaster>().bodyPrefab = sipSipBody;

            AddGreedyItems();
            SetNameToken();
            CreateSpawnCard();

            DOSTUFF();
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

            foreach (GivePickupsOnStart component in sipSipMaster.GetComponents<GivePickupsOnStart>()) {
                component.enabled = false;
            } 

            GivePickupsOnStart pickupComp = sipSipMaster.AddComponent<GivePickupsOnStart>();
            pickupComp.equipmentString = "GoldGat";
            pickupComp.itemInfos = array;
        }
        private static void DOSTUFF() {

            /* GameObject lunarScavEncounterObject = Resources.Load<GameObject>("prefabs/networkedobjects/encounters/ScavLunarEncounter");
            ScriptedCombatEncounter lunarScavEncounter = lunarScavEncounterObject.GetComponent<ScriptedCombatEncounter>();

            SpawnInfo info = new SpawnInfo();
            SpawnInfo[] newArray = new SpawnInfo[lunarScavEncounter.spawns.Length];
            lunarScavEncounter.spawns.CopyTo(newArray, 0);

            lunarScavEncounter.spawns = newArray;

            info.explicitSpawnPosition = newArray[1].explicitSpawnPosition;
            info.spawnCard = card;

            this code errors and i don't know why

            //info; */
        }
        private static void SetNameToken() {
            LanguageAPI.Add("SCAVLUNAR5_BODY_NAME", "Sipsip The Greedy");
            CharacterBody body = sipSipBody.GetComponent<CharacterBody>();
            body.baseNameToken = "SCAVLUNAR5_BODY_NAME";
        }
        private static void CreateSpawnCard() {
            On.RoR2.CharacterSpawnCard.Awake += Hooks.CharacterSpawnCard_Awake;
            CharacterSpawnCard characterSpawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            On.RoR2.CharacterSpawnCard.Awake -= Hooks.CharacterSpawnCard_Awake;

            characterSpawnCard.directorCreditCost = 2000;
            characterSpawnCard.forbiddenAsBoss = false;
            characterSpawnCard.name = "cscSipSip";
            //characterSpawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.None;
            characterSpawnCard.hullSize = HullClassification.Golem;
            characterSpawnCard.loadout = new SerializableLoadout();
            characterSpawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            characterSpawnCard.noElites = false;
            characterSpawnCard.occupyPosition = false;
            characterSpawnCard.prefab = sipSipMaster;
            characterSpawnCard.sendOverNetwork = true;

            card = characterSpawnCard;
        }
    }
}