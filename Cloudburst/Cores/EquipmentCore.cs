using R2API;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores
{
    class EquipmentCore
    {
        //TODO:
        //Figure out why equipments aren't spawning properly
        //Give equipments models
        //Give equipment icons
        //(Post 1.0)Give them display rules.
        #region Indexes
        protected EquipmentIndex lumpkinIndex;
        protected EquipmentIndex unstableQuantumReactorIndex;
        protected EquipmentIndex brokenPrinterIndex;
        protected EquipmentIndex midasIndex;
        //public static EquipmentIndex cocktailIndex { get; }
        #endregion
        #region Lists
        protected List<GameObject> projectileList = new List<GameObject>{

            Resources.Load<GameObject>("prefabs/projectiles/ArtifactShellSeekingSolarFlare"),
            Resources.Load<GameObject>("prefabs/projectiles/BeetleQueenSpit"),
            Resources.Load<GameObject>("prefabs/projectiles/CommandoGrenadeProjectile"),
            Resources.Load<GameObject>("prefabs/projectiles/DroneRocket"),
            Resources.Load<GameObject>("prefabs/projectiles/Sawmerang"),
            Resources.Load<GameObject>("prefabs/projectiles/RoboBallProjectile"),
            Resources.Load<GameObject>("prefabs/projectiles/SMMaulingRockLarge"),
            Resources.Load<GameObject>("prefabs/projectiles/SMMaulingRockMedium"),
            Resources.Load<GameObject>("prefabs/projectiles/SMMaulingRockSmall"),
            Resources.Load<GameObject>("prefabs/projectiles/SporeGrenadeProjectile"),
            Resources.Load<GameObject>("prefabs/projectiles/Sunder"),
            Resources.Load<GameObject>("prefabs/projectiles/SyringeProjectile"),
            Resources.Load<GameObject>("prefabs/projectiles/SuperRoboBallProjectile"),
            Resources.Load<GameObject>("prefabs/projectiles/SyringeProjectile"),
            Resources.Load<GameObject>("prefabs/projectiles/SyringeProjectileHealing"),
            Resources.Load<GameObject>("prefabs/projectiles/BellBall"),
            Resources.Load<GameObject>("prefabs/projectiles/CommandoGrenadeProjectile"),
            Resources.Load<GameObject>("prefabs/projectiles/CrocoDiseaseProjectile"),
            Resources.Load<GameObject>("prefabs/projectiles/CrocoSpit"),
            Resources.Load<GameObject>("prefabs/projectiles/DaggerProjectile"),
        };
        #endregion
        #region GameObjects
        private GameObject greaterWarbanner;
        //private GameObject molotovCocktailZone;
        #endregion
        public EquipmentCore() {
            LogCore.LogI("Initializing Core: " + base.ToString());

            CreateGreaterWarbanner();  RegisterTokens();
            
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

            RegisterNewEquip(new EquipmentDef()
            {
                addressToken = "",
                appearsInMultiPlayer = true,
                appearsInSinglePlayer = true,
                canDrop = true,
                colorIndex = ColorCatalog.ColorIndex.Equipment,
                cooldown = 30,
                descriptionToken = "EQUIPMENT_LUMPKIN_DESC",
                enigmaCompatible = true,
                isBoss = false,
                isLunar = false,
                loreToken = "EQUIPMENT_LUMPKIN_LORE",
                name = "Lumpkin",
                nameToken = "EQUIPMENT_LUMPKIN_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "EQUIPMENT_LUMPKIN_PICKUP",
                unlockableName = "",
            });
            RegisterNewEquip(new EquipmentDef()
            {
                addressToken = "",
                appearsInMultiPlayer = true,
                appearsInSinglePlayer = true,
                canDrop = true,
                colorIndex = ColorCatalog.ColorIndex.Equipment,
                cooldown = 15,
                descriptionToken = "EQUIPMENT_LUMPKIN_DESC",
                enigmaCompatible = true,
                isBoss = false,
                isLunar = false,
                loreToken = "",
                name = "Reactor",
                nameToken = "EQUIPMENT_UNSTABLEQUANTUMREACTOR_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "EQUIPMENT_UNSTABLEQUANTUMREACTOR_PICKUP",
                unlockableName = "",
            });
            RegisterNewEquip(new EquipmentDef()
            {
                addressToken = "",
                appearsInMultiPlayer = true,
                appearsInSinglePlayer = true,
                canDrop = true,
                colorIndex = ColorCatalog.ColorIndex.Equipment,
                cooldown = 55,
                descriptionToken = "EQUIPMENT_BROKENPRINTER_DESC",
                enigmaCompatible = true,
                isBoss = false,
                isLunar = false,
                loreToken = "EQUIPMENT_BROKENPRINTER_LORE",
                name = "Printer",
                nameToken = "EQUIPMENT_BROKENPRINTER_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "EQUIPMENT_BROKENPRINTER_PICKUP",
                unlockableName = "",
            });

            On.RoR2.EquipmentSlot.PerformEquipmentAction += EquipmentSlot_PerformEquipmentAction;

        }

        protected void CreateGreaterWarbanner() {
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
            if (equipmentIndex == brokenPrinterIndex) {
                GameObject gameObject = Object.Instantiate<GameObject>(greaterWarbanner, self.characterBody.transform.position, Quaternion.identity);
                gameObject.GetComponent<TeamFilter>().teamIndex = self.characterBody.teamComponent.teamIndex;
                gameObject.GetComponent<BuffWard>().radius = 25;
                NetworkServer.Spawn(gameObject);
                return true;
            }
            if (equipmentIndex == unstableQuantumReactorIndex) {
                FireProjectile(self.characterBody);
                return true;
            }
            if (equipmentIndex == lumpkinIndex) {
                Scream(self.characterBody);
                return true;
            }
            return orig(self, equipmentIndex);
        }

        protected internal void RegisterTokens() {
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_NAME", "The Lumpkin");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_PICKUP", "And his screams were Brazilian...");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_DESC", "Release a Brazilian scream that does your maximum health for damage and stuns enemies within 35m.");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_LORE", "\"Lumpkin, one of the many commanders in the War of 2019 possessed a scream that could deafen his oppenents. It was unknown how he could scream so loudly, until he was killed in the final battle of WW19 and had his lungs ripped from his chest. \r\n\r\nHis lungs, pictured above, allowed him to scream loudly without injuring himself.\"\r\n\r\n-Exhibit at The National WW19 Museum");

            LanguageAPI.Add("EQUIPMENT_REACTOR_NAME", "Unstable Quantum Reactor");
            LanguageAPI.Add("EQUIPMENT_REACTOR_PICKUP", "Fire a random projectile on use.");
            LanguageAPI.Add("EQUIPMENT_REACTOR_DESC", "Fire a random projectile.");

            LanguageAPI.Add("EQUIPMENT_BROKENPRINTER_NAME", "Greater Warbanner");
            LanguageAPI.Add("EQUIPMENT_BROKENPRINTER_PICKUP", "Place a greater warbanner which boosts regen, critical hit chance, and lowers cooldowns.");
            LanguageAPI.Add("EQUIPMENT_BROKENPRINTER_DESC", "Place a greater warbanner which grants 20% health regeneration, 10% critical hit chance, and 50% cooldown reduction.");
            LanguageAPI.Add("EQUIPMENT_BROKENPRINTER_LORE", "Order: \u201CGreater Warbanner\u201D\r\nTracking Number: 72******\r\nEstimated Delivery: 08\\25\\2057\\\r\nShipping Method: High Priority\r\nShipping Address: 836 Lane, Lab [42], Mars\r\nShipping Details:\r\n\r\nI got this thing off of my dead grandmother, it's a relic from the great war of 2019. It  gives this aura of energy and motivation to do whatever someone desires! Anyways -- I'm passing it onto you, since I am dying to this cancer and I'd like you to have it before I go six feet under. Much love, Uncle Abe.\n");
        }

        protected internal void RegisterNewEquip(EquipmentDef equipmentDef) {
            var customEquip = new CustomEquipment(equipmentDef, new ItemDisplayRule[0]);
            switch (equipmentDef.name)
            {
                case "Lumpkin":
                    lumpkinIndex = ItemAPI.Add(customEquip);
                    break;
                case "Reactor":
                    unstableQuantumReactorIndex = ItemAPI.Add(customEquip);
                    break;
                case "Printer":
                    brokenPrinterIndex = ItemAPI.Add(customEquip);
                    break;
                default:

                    LogCore.LogF(equipmentDef.name + " :: EquipmentDef.Name not found!");
                    break;
            }
        }

        protected internal void Scream(CharacterBody Screamer)
        {
            BlastAttack impactAttack = new BlastAttack
            {
                attacker = Screamer.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = Screamer.maxHealth,
                baseForce = 30,
                bonusForce = new Vector3(0, 0, 0),
                crit = false,
                damageColorIndex = DamageColorIndex.CritHeal,
                damageType = DamageType.AOE,
                falloffModel = BlastAttack.FalloffModel.None,
                inflictor = Screamer.gameObject,
                losType = BlastAttack.LoSType.NearestHit,
                position = Screamer.transform.position,
                procChainMask = default,
                procCoefficient = 1.2f,
                radius = 35,
                teamIndex = Screamer.teamComponent.teamIndex
            };
            impactAttack.Fire();
            EffectData effect = new EffectData()
            {
                origin = Screamer.transform.position,
                scale = 35
            };
            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact"), effect, true);
        }

        protected internal void SummonAlly(CharacterBody user)
        {
            CharacterMaster characterMaster;
            characterMaster = new MasterSummon
            {
                masterPrefab = MasterCatalog.GetMasterPrefab(MasterCatalog.FindAiMasterIndexForBody(user.bodyIndex)),
                position = user.footPosition + user.transform.up + user.transform.up + user.transform.up,
                rotation = user.transform.rotation,
                summonerBodyObject = user.gameObject,
                ignoreTeamMemberLimit = false,
                teamIndexOverride = user.teamComponent.teamIndex,
                loadout = user.master.loadout
            }.Perform();

            characterMaster.bodyPrefab = user.master.bodyPrefab;
            characterMaster.Respawn(characterMaster.GetBody().footPosition, Quaternion.identity);

            Inventory inventory = characterMaster.inventory;
            if (inventory)
            {
                inventory.CopyItemsFrom(user.inventory);
                inventory.ResetItem(ItemIndex.WardOnLevel);
                inventory.ResetItem(ItemIndex.BeetleGland);
                inventory.ResetItem(ItemIndex.CrippleWardOnLevel);
                inventory.ResetItem(ItemIndex.TPHealingNova);
                inventory.ResetItem(ItemIndex.FocusConvergence);
                inventory.ResetItem(ItemIndex.TitanGoldDuringTP);
                inventory.GiveItem(ItemIndex.BoostDamage, 5);
                inventory.GiveItem(ItemIndex.CutHp, 2);
                inventory.CopyEquipmentFrom(user.inventory);
            }
        }

        protected internal void FireProjectile(CharacterBody user)
        {
            InputBankTest aimRay = user.inputBank;
            int rng = UnityEngine.Random.Range(0, projectileList.Count);
            var projInfo = new FireProjectileInfo
            {
                crit = false,
                damage = 5 * user.damage,
                owner = user.gameObject,
                position = user.transform.position,
                projectilePrefab = projectileList[rng],
                rotation = Util.QuaternionSafeLookRotation(aimRay.aimDirection),
            };
            ProjectileManager.instance.FireProjectile(projInfo);
        }
    }
}
