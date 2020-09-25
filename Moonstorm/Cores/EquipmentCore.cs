using R2API;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores
{
    class EquipmentCore
    {
        public static EquipmentCore instance;

        //TODO:
        //Figure out why equipments aren't spawning properly
        //Give equipments models
        //Give equipment icons
        //(Post 1.0): Give them display rules.

        private ItemIndex GetRandomItem(List<ItemIndex> items)
        {
            bool isEmpty = items.Count <= 0;
            if (!isEmpty)
            {
                int itemID = UnityEngine.Random.Range(0, items.Count);

                return items[itemID];
            }
            else LogCore.LogE("SSSHIT!"); return ItemIndex.ShieldOnly;

        }

        #region Indexes
        protected EquipmentIndex lumpkinIndex;
        protected EquipmentIndex unstableQuantumReactorIndex;
        protected EquipmentIndex brokenPrinterIndex;
        protected EquipmentIndex rockFruitIndex;
        protected EquipmentIndex MIRVIndex;
        //protected EquipmentIndex midasIndex;
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
            RegisterNewEquip(new EquipmentDef()
            {
                addressToken = "",
                appearsInMultiPlayer = true,
                appearsInSinglePlayer = true,
                canDrop = true,
                colorIndex = ColorCatalog.ColorIndex.Equipment,
                cooldown = 25,
                descriptionToken = "EQUIPMENT_ROCKFRUIT_DESC",
                enigmaCompatible = true,
                isBoss = false,
                isLunar = true,
                loreToken = "EQUIPMENT_ROCKFRUIT_LORE",
                name = "RockFruit",
                nameToken = "EQUIPMENT_ROCKFRUIT_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "EQUIPMENT_ROCKFRUIT_PICKUP",
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
                descriptionToken = "EQUIPMENT_MIRV_DESC",
                enigmaCompatible = true,
                isBoss = false,
                isLunar = true,
                loreToken = "EQUIPMENT_MIRV_LORE",
                name = "MIRV",
                nameToken = "EQUIPMENT_MIRV_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "EQUIPMENT_MIRV_PICKUP",
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
            if (equipmentIndex == brokenPrinterIndex)
            {
                GameObject gameObject = Object.Instantiate<GameObject>(greaterWarbanner, self.characterBody.transform.position, Quaternion.identity);
                gameObject.GetComponent<TeamFilter>().teamIndex = self.characterBody.teamComponent.teamIndex;
                gameObject.GetComponent<BuffWard>().radius = 25;
                NetworkServer.Spawn(gameObject);
                return true;
            }
            if (equipmentIndex == unstableQuantumReactorIndex)
            {
                FireProjectile(self.characterBody);
                return true;
            }
            if (equipmentIndex == lumpkinIndex)
            {
                Scream(self.characterBody);
                return true;
            }
            if (equipmentIndex == rockFruitIndex)
            {
                ConsumeRock(self.characterBody.inventory);
                return true;
            }
            if (equipmentIndex == MIRVIndex)
            {
                ThrowMIRV(self.characterBody);
            }
            return orig(self, equipmentIndex);
        }

        protected internal void RegisterTokens()
        {
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_NAME", "The Lumpkin");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_PICKUP", "And his screams were Brazilian...");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_DESC", "Release a Brazilian scream that does your maximum health for damage and stuns enemies within 35m.");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_LORE", "\"Lumpkin, one of the many commanders in the War of 2019 possessed a scream that could deafen his oppenents. It was unknown how he could scream so loudly, until he was killed in the final battle of WW19 and had his lungs ripped from his chest. \r\n\r\nHis lungs, pictured above, allowed him to scream loudly without injuring himself.\"\r\n\r\n-Exhibit at The National WW19 Museum");

            LanguageAPI.Add("EQUIPMENT_REACTOR_NAME", "Unstable Quantum Reactor");
            LanguageAPI.Add("EQUIPMENT_REACTOR_PICKUP", "Fire a random projectile on use.");
            LanguageAPI.Add("EQUIPMENT_REACTOR_DESC", "Fire a random projectile.");

            LanguageAPI.Add("EQUIPMENT_PRINTER_NAME", "Greater Warbanner");
            LanguageAPI.Add("EQUIPMENT_PRINTER_PICKUP", "Place a greater warbanner which boosts regen, critical hit chance, and lowers cooldowns.");
            LanguageAPI.Add("EQUIPMENT_PRINTER_DESC", "Place a greater warbanner which grants 20% health regeneration, 10% critical hit chance, and 50% cooldown reduction.");
            LanguageAPI.Add("EQUIPMENT_PRINTER_LORE", "Order: \u201CGreater Warbanner\u201D\r\nTracking Number: 72******\r\nEstimated Delivery: 08\\25\\2057\\\r\nShipping Method: High Priority\r\nShipping Address: 836 Lane, Lab [42], Mars\r\nShipping Details:\r\n\r\nI got this thing off of my dead grandmother, it's a relic from the great war of 2019. It  gives this aura of energy and motivation to do whatever someone desires! Anyways -- I'm passing it onto you, since I am dying to this cancer and I'd like you to have it before I go six feet under. Much love, Uncle Abe.\n");

            LanguageAPI.Add("EQUIPMENT_ROCKFRUIT_NAME", "Rock Fruit");
            LanguageAPI.Add("EQUIPMENT_ROCKFRUIT_PICKUP", "Swap a random item for another random item.");
            LanguageAPI.Add("EQUIPMENT_ROCKFRUIT_DESC", "Swap a random item in your inventory for another random item of the same tier.");
            //LanguageAPI.Add("EQUIPMENT_PRINTER_LORE", "Order: \u201CGreater Warbanner\u201D\r\nTracking Number: 72******\r\nEstimated Delivery: 08\\25\\2057\\\r\nShipping Method: High Priority\r\nShipping Address: 836 Lane, Lab [42], Mars\r\nShipping Details:\r\n\r\nI got this thing off of my dead grandmother, it's a relic from the great war of 2019. It  gives this aura of energy and motivation to do whatever someone desires! Anyways -- I'm passing it onto you, since I am dying to this cancer and I'd like you to have it before I go six feet under. Much love, Uncle Abe.\n");
        }

        protected internal void RegisterNewEquip(EquipmentDef equipmentDef)
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
                case "RockFruit":
                    rockFruitIndex = ItemAPI.Add(customEquip);
                    break;
                case "MIRV":
                    MIRVIndex = ItemAPI.Add(customEquip);
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

        protected internal void ThrowMIRV(CharacterBody user)
        {
            InputBankTest aimRay = user.inputBank;
            var projInfo = new FireProjectileInfo
            {
                crit = false,
                damage = 5 * user.damage,
                owner = user.gameObject,
                position = user.transform.position,
                projectilePrefab = ProjectileCore.MIRVProjectile,
                rotation = Util.QuaternionSafeLookRotation(aimRay.aimDirection),
            };
            ProjectileManager.instance.FireProjectile(projInfo);
        }

        protected internal void ConsumeRock(Inventory user)
        {
            //TODO:
            //This is horrible.

            var allItems = new List<ItemIndex>();
            for (var item = ItemIndex.Syringe; item < (ItemIndex)ItemCatalog.itemCount; item++)
            {
                var def = ItemCatalog.GetItemDef(item);
                if (def.tier != ItemTier.NoTier)
                {
                    if (user.GetItemCount(item) > 0)
                    {
                        allItems.Add(item);
                    }
                }
            }

            var randoItem = GetRandomItem(allItems);
            user.RemoveItem(randoItem);

            var itemIndex = ItemCatalog.GetItemDef(randoItem).tier;

            List<ItemIndex> list = new List<ItemIndex>();

            var dropList = GetDropListListFromItemTier(itemIndex);

            if (dropList != null)
            {
                foreach (var item in dropList)
                {
                    //var AAAAAAAA = new Xoroshiro128Plus(2023402033023);
                    var pickupDef = PickupCatalog.GetPickupDef(item);
                    if (pickupDef != null)
                    {
                        list.Add(pickupDef.itemIndex);
                        //YOOOOOOOOOOOOOOOOO    
                    }
                }
            }
            else LogCore.LogF("FUCK FUCK FUCK FUCK FUCK");

            bool isEmpty = list.Count <= 0;

            if (!isEmpty)
            {
                var randomItem = GetRandomItem(list);

                if (user && randomItem != ItemIndex.None && !isEmpty)
                {
                    LogCore.LogI(randomItem);
                    user.GiveItem(randomItem);
                }
            }

            /*var itemDef = ItemCatalog.GetItemDef(randoItem);
            var itemTier = itemDef.tier;

            List<ItemIndex> list = new List<ItemIndex>();

            foreach (var item in GetDropListListFromItemTier(itemTier)) {
                var newItemDef = ItemCatalog.GetItemDef(PickupCatalog.GetPickupDef(item).itemIndex);
                list.Add(newItemDef.itemIndex);
            }

            var FINALLY = GetRandomItem(list);
            user.GiveItem(FINALLY);*/
        }

        public List<PickupIndex> GetDropListListFromItemTier(ItemTier itemTier)
        {
            switch (itemTier)
            {
                case ItemTier.Tier1:
                    return Run.instance.availableTier1DropList;
                case ItemTier.Tier2:
                    return Run.instance.availableTier2DropList;
                case ItemTier.Tier3:
                    return Run.instance.availableTier3DropList;
                case ItemTier.Lunar:
                    return Run.instance.availableLunarDropList;
                case ItemTier.Boss:
                    return Run.instance.availableBossDropList;
                default:
                    LogCore.LogE("Cannot find droplist for " + itemTier);
                    return null;
            }
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
