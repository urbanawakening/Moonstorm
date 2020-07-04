using R2API;
using RoR2;
using UnityEngine;

namespace CloudBurst.Misc
{
    internal sealed class MiscModifications
    {
        public static void Modify()
        {
            FixLunarTp();
        }
        private static void FixLunarTp()
        {
            GameObject lunarTeleporter = Resources.Load<GameObject>("prefabs/networkedobjects/TeleporterLunar");
            GameObject teleporter = Resources.Load<GameObject>("prefabs/networkedobjects/Teleporter1");

            lunarTeleporter.layer = teleporter.layer;

            TeleporterInteraction lunarInteraction = lunarTeleporter.GetComponent<TeleporterInteraction>();
            HoldoutZoneController teleporterZoneController = teleporter.GetComponent<HoldoutZoneController>();
            HoldoutZoneController lunarZoneController = lunarTeleporter.AddComponent<HoldoutZoneController>();
            OutsideInteractableLocker lunarInteractableLocker = lunarTeleporter.AddComponent<OutsideInteractableLocker>();
            EntityStateMachine lunarStateMachine = lunarTeleporter.AddComponent<EntityStateMachine>();
            EntityStateMachine stateMachine = teleporter.GetComponent<EntityStateMachine>();

            LanguageAPI.Add("TELEPORTERLUNAR_HOLDOUT_TOKEN", "Observe.");

            lunarZoneController.baseChargeDuration = teleporterZoneController.baseChargeDuration;
            lunarZoneController.baseRadius = 100000;
            lunarZoneController.inBoundsObjectiveToken = "TELEPORTERLUNAR_HOLDOUT_TOKEN";
            lunarZoneController.outOfBoundsObjectiveToken = teleporterZoneController.outOfBoundsObjectiveToken;
            lunarZoneController.radiusIndicator = null;
            lunarZoneController.radiusSmoothTime = teleporterZoneController.radiusSmoothTime;
            lunarZoneController.enabled = true;


            lunarInteractableLocker.updateInterval = 0.1f;
            lunarInteractableLocker.lockPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/PurchaseLock");

            lunarStateMachine.customName = "Main";
            lunarStateMachine.initialStateType = stateMachine.initialStateType;
            lunarStateMachine.mainStateType = stateMachine.mainStateType;
            lunarStateMachine.commonComponents = stateMachine.commonComponents;
            lunarStateMachine.networkIndex = -1;

            lunarInteraction.mainStateMachine = lunarStateMachine;
            lunarInteraction.outsideInteractableLocker = lunarInteractableLocker;

            ChildLocator locator = lunarTeleporter.GetComponent<ChildLocator>();
            int loc = locator.FindChildIndex("InitialBurst");
            Transform loca = locator.FindChild(loc);
            if (loca) {
                BaseLogger.Log("fuck");
            }


            foreach (CombatDirector combatDirector in lunarTeleporter.GetComponents<CombatDirector>())
            {
                if (combatDirector.customName == "Monsters")
                {

                    combatDirector.expRewardCoefficient = 0.1f;
                    combatDirector.minSeriesSpawnInterval = 0.5f;
                    combatDirector.maxSeriesSpawnInterval = 0.5f;
                    combatDirector.minRerollSpawnInterval = 2;
                    combatDirector.maxRerollSpawnInterval = 4;
                    combatDirector.creditMultiplier = 6;
                    combatDirector.spawnDistanceMultiplier = 1.5f;
                    combatDirector.shouldSpawnOneWave = false;
                    combatDirector.targetPlayers = true;
                    combatDirector.skipSpawnIfTooCheap = true;
                    combatDirector.resetMonsterCardIfFailed = true;
                    combatDirector.maximumNumberToSpawnBeforeSkipping = 6;
                    combatDirector.eliteBias = 2;
                }
                else if (combatDirector.customName == "Boss")
                {
                }
            }
        }
    }
}