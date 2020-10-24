using R2API;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores
{
    class EquipmentCore
    {
        public static EquipmentCore instance;

        //TODO:
        //Give equipments models
        //Give equipment icons
        //(Post 1.0): Give them display rules.


        protected internal EquipmentIndex greaterWarbannerIndex;
        protected internal EquipmentIndex cloakIndex;

        private const string modPrefix = "@Starstorm2:";
        private const string prefabPath = modPrefix + "Assets/Items/";

        private GameObject greaterWarbanner;

        public EquipmentCore()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            CreateGreaterWarbanner(); RegisterTokens();

            #region Basic
            var prefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");
            var rule = new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = prefab,
                childName = "Chest",
                localScale = new Vector3(0f, 0, 0f),
                localAngles = new Vector3(0f, 0f, 0f),
                localPos = new Vector3(0, 0f, 0f)
            };
            string pickUpIconPath = "Textures/MiscIcons/texMysteryIcon";
            string pickUpModelPath = "Prefabs/PickupModels/PickupMystery";
            #endregion

            greaterWarbannerIndex = RegisterNewEquip(new EquipmentDef()
            {
                addressToken = "",
                appearsInMultiPlayer = true,
                appearsInSinglePlayer = true,
                canDrop = true,
                colorIndex = ColorCatalog.ColorIndex.Equipment,
                cooldown = 55,
                descriptionToken = "EQUIPMENT_GREATERWARBANNER_DESC",
                enigmaCompatible = true,
                isBoss = false,
                isLunar = false,
                loreToken = "EQUIPMENT_GREATERWARBANNER_LORE",
                name = "GreaterWarbanner",
                nameToken = "EQUIPMENT_GREATERWARBANNER_NAME",
                pickupIconPath = prefabPath + "GreaterWarbanner/GreaterWarbanner_Icon.png",
                pickupModelPath = prefabPath + "GreaterWarbanner/MDLGreaterWarbanner.prefab",
                pickupToken = "EQUIPMENT_GREATERWARBANNER_PICKUP",
                unlockableName = "",
            });
            cloakIndex = RegisterNewEquip(new EquipmentDef()
            {
                addressToken = "",
                appearsInMultiPlayer = true,
                appearsInSinglePlayer = true,
                canDrop = true,
                colorIndex = ColorCatalog.ColorIndex.Equipment,
                cooldown = 45f,
                descriptionToken = "EQUIPMENT_CLOAK_DESC",
                enigmaCompatible = true,
                isBoss = false,
                isLunar = false,
                //loreToken = "EQUIPMENT_CLOAK_LORE",
                name = "Cloak",
                nameToken = "EQUIPMENT_CLOAK_NAME",
                pickupIconPath = prefabPath + "CloakingHeadband/CloakingHeadband_Icon.png",
                //pickupModelPath = prefabPath + "GreaterWarbanner/MDLGreaterWarbanner.prefab",
                pickupToken = "EQUIPMENT_CLOAK_PICKUP",
                unlockableName = "",
            });

            On.RoR2.EquipmentSlot.PerformEquipmentAction += EquipmentSlot_PerformEquipmentAction;

        }

        protected void CreateGreaterWarbanner()
        {
            greaterWarbanner = Resources.Load<GameObject>("Prefabs/NetworkedObjects/WarbannerWard").InstantiateClone("GreaterWarbannerWard", true);
            greaterWarbanner.GetComponent<BuffWard>().buffType = BuffCore.instance.greaterBannerIndex;

            var sphereIndicator = greaterWarbanner.transform.Find("Indicator/IndicatorSphere");
            var banner = greaterWarbanner.transform.Find("mdlWarbanner/Plane");
            //var holder = greaterWarbanner.transform.Find("mdlWarbanner/Cylinder");

            var sphereRenderer = sphereIndicator.GetComponent<Renderer>();
            var bannerRenderer = banner.GetComponent<Renderer>();
            //var holderRenderer = holder.GetComponent<Renderer>();

            sphereRenderer.material = Resources.Load<Material>("materials/matWolfhatOverlay");
            bannerRenderer.material = Resources.Load<Material>("materials/matWolfhatOverlay");
            //holderRenderer.material = Resources.Load<Material>("materials/matWolfhatOverlay");
        }

        private bool EquipmentSlot_PerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentIndex equipmentIndex)
        {
            if (equipmentIndex == greaterWarbannerIndex)
            {
                GameObject gameObject = Object.Instantiate<GameObject>(greaterWarbanner, self.characterBody.transform.position, Quaternion.identity);
                gameObject.GetComponent<TeamFilter>().teamIndex = self.characterBody.teamComponent.teamIndex;
                gameObject.GetComponent<BuffWard>().radius = 25;
                NetworkServer.Spawn(gameObject);
                return true;
            }
            if (equipmentIndex == cloakIndex)
            {
                self.characterBody.AddTimedBuff(BuffIndex.Cloak, 8f);
                return true;
            }
            return orig(self, equipmentIndex);
        }

        protected internal void RegisterTokens()
        {
            LanguageAPI.Add("EQUIPMENT_GREATERWARBANNER_NAME", "Greater Warbanner");
            LanguageAPI.Add("EQUIPMENT_GREATERWARBANNER_PICKUP", "Place a greater warbanner which boosts regen, critical hit chance, and lowers cooldowns.");
            LanguageAPI.Add("EQUIPMENT_GREATERWARBANNER_DESC", "Place a greater warbanner which grants <style=cIsHealing>20% health regeneration</style>, <style=cIsDamage>10% critical hit chance</style>, and <style=cIsUtility>50% cooldown reduction</style>.");
            LanguageAPI.Add("EQUIPMENT_GREATERWARBANNER_LORE", "Order: \u201CGreater Warbanner\u201D\r\nTracking Number: 72******\r\nEstimated Delivery: 08\\25\\2057\\\r\nShipping Method: High Priority\r\nShipping Address: 836 Lane, Lab [42], Mars\r\nShipping Details:\r\n\r\nI got this thing off of my dead grandmother, it's a relic from the great war of 2019. It  gives this aura of energy and motivation to do whatever someone desires! Anyways -- I'm passing it onto you, since I am dying to this cancer and I'd like you to have it before I go six feet under. Much love, Uncle Abe.\n");

            LanguageAPI.Add("EQUIPMENT_CLOAK_NAME", "Cloaking Headband");
            LanguageAPI.Add("EQUIPMENT_CLOAK_PICKUP", "Becompe temporarily invisible.");
            LanguageAPI.Add("EQUIPMENT_CLOAK_DESC", "Become <style=cIsUtility>invisible</style> for 8 seconds.");
        }

        protected internal EquipmentIndex RegisterNewEquip(EquipmentDef equipmentDef)
        {
            var internalName = equipmentDef.name;
            var upperName = internalName.ToUpper(CultureInfo.InvariantCulture);
            if (equipmentDef.nameToken == null)
            {
                equipmentDef.nameToken = string.Format(CultureInfo.InvariantCulture, "ITEM_{0}_NAME", upperName);
            }
            if (equipmentDef.descriptionToken == null)
            {
                equipmentDef.descriptionToken = string.Format(CultureInfo.InvariantCulture, "ITEM_{0}_DESC", upperName);
            }
            if (equipmentDef.pickupToken == null)
            {
                equipmentDef.pickupToken = string.Format(CultureInfo.InvariantCulture, "ITEM_{0}_PICKUP", upperName);
            }
            if (equipmentDef.loreToken == null)
            {
                equipmentDef.loreToken = string.Format(CultureInfo.InvariantCulture, "ITEM_{0}_LORE", upperName);
            }
            var customEquip = new CustomEquipment(equipmentDef, new ItemDisplayRule[0]);
            return ItemAPI.Add(customEquip);
        }
    }
}
