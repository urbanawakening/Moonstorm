using R2API;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;

namespace Cloudburst
{
    public sealed class Equipment
    {
        #region Indexes
        public static EquipmentIndex lumpkinIndex { get; private set; }
        public static EquipmentIndex unstableQuantumReactorIndex { get; private set; }
        public static EquipmentIndex brokenPrinterIndex { get; private set; }
        //public static EquipmentIndex cocktailIndex { get; }
        #endregion
        #region Lists
        public static List<GameObject> projectileList = new List<GameObject>{

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
        //private static GameObject molotovCocktail;
        //private static GameObject molotovCocktailZone;
        #endregion
        public Equipment()
        {
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
            #region Tokens
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_NAME", "The Lumpkin");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_PICKUP", "And his screams were Brazilian...");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_DESC", "Release a Brazilian scream that does your maximum health for damage and stuns enemies within 35m.");
            LanguageAPI.Add("EQUIPMENT_LUMPKIN_LORE", "\"Lumpkin, one of the many commanders in the War of 2019 possessed a scream that could deafen his oppenents. It was unknown how he could scream so loudly, until he was killed in the final battle of WW19 and had his lungs ripped from his chest. \r\n\r\nHis lungs, pictured above, allowed him to scream loudly without injuring himself.\"\r\n\r\n-Exhibit at The National WW19 Museum");

            LanguageAPI.Add("EQUIPMENT_UNSTABLEQUANTUMREACTOR_NAME", "Unstable Quantum Reactor");
            LanguageAPI.Add("EQUIPMENT_UNSTABLEQUANTUMREACTOR_PICKUP", "Fire a random projectile on use.");
            LanguageAPI.Add("EQUIPMENT_UNSTABLEQUANTUMREACTOR_DESC", "Fire a random projectile.");

            LanguageAPI.Add("EQUIPMENT_BROKENPRINTER_NAME", "Broken Printer");
            LanguageAPI.Add("EQUIPMENT_BROKENPRINTER_PICKUP", "Summon a powerful but fragile ally that aids you in combat.");
            LanguageAPI.Add("EQUIPMENT_BROKENPRINTER_DESC", "Summon a powerful ally that inherits your items but has half health.");
            LanguageAPI.Add("EQUIPMENT_BROKENPRINTER_LORE", "Order: \u201CBroken Printer\u201D\r\nTracking Number: 72******\r\nEstimated Delivery: 08\\25\\2057\\\r\nShipping Method: High Priority\r\nShipping Address: 836 Lane, Lab [42], Mars\r\nShipping Details:\r\n\r\nI can\u2019t speak for the rest of the researchers here, but I have no clue why you decided to send this here. We don\u2019t know what\u2019s wrong with this thing, and we don\u2019t know if there is anything wrong with it, it\u2019s completely alien. I don\u2019t think a person actually made this thing, there are materials in it that we\u2019ve never seen before.\r\n\r\nI\u2019m sending this back to you because it\u2019s more trouble than it\u2019s worth to keep here, and frankly those things it prints are horrifyingly hollow.\r\n");

            //LanguageAPI.Add("EQUIPMENT_VIVELAFRANCE_NAME", "Molotov Cocktail");
            //LanguageAPI.Add("EQUIPMENT_VIVELAFRANCE_PICKUP", "Throw a molotov cocktail.");
            //LanguageAPI.Add("EQUIPMENT_VIVELAFRANCE_DESC", "Throw a molotov cocktail that ignites enemies.");
            #endregion
            #region EquipmentDefs
            EquipmentDef lumpkinDef = new EquipmentDef()
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
            };
            EquipmentDef reactorDef = new EquipmentDef()
            {
                addressToken = "",
                appearsInMultiPlayer = true,
                appearsInSinglePlayer = true,
                canDrop = true,
                colorIndex = ColorCatalog.ColorIndex.Equipment,
                cooldown = 25,
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
            };
            EquipmentDef printerDef = new EquipmentDef()
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
            };
            /*EquipmentDef vivaLaFranceDef = new EquipmentDef()
            {
                addressToken = "",
                appearsInMultiPlayer = true,
                appearsInSinglePlayer = true,
                canDrop = true,
                colorIndex = ColorCatalog.ColorIndex.Equipment,
                cooldown = 55,
                descriptionToken = "EQUIPMENT_VIVELAFRANCE_DESC",
                enigmaCompatible = true,
                isBoss = false,
                isLunar = false,
                loreToken = "",
                name = "Cocktail",
                nameToken = "EQUIPMENT_VIVELAFRANCE_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "EQUIPMENT_VIVELAFRANCE_PICKUP",
                unlockableName = "",
            };*/
            #endregion
            #region CustomEquipments
            var lumpkin = new CustomEquipment(lumpkinDef, new[] { rule });
            var reactor = new CustomEquipment(reactorDef, new[] { rule });
            var printer = new CustomEquipment(printerDef, new[] { rule });
            //var cocktail = new CustomEquipment(vivaLaFranceDef, new[] { rule });
            #endregion
            #region Setting Indexes
            lumpkinIndex = ItemAPI.Add(lumpkin);
            unstableQuantumReactorIndex = ItemAPI.Add(reactor);
            brokenPrinterIndex = ItemAPI.Add(printer);
            //cocktailIndex = ItemAPI.Add(cocktail);
            #endregion
            #region Create CloudburstEquipmentDefs
            CloudburstEquipDef lumpkinEquipDef = new CloudburstEquipDef();
            CloudburstEquipDef reactorEquipDef = new CloudburstEquipDef();
            CloudburstEquipDef printerEquipDef = new CloudburstEquipDef();

            lumpkinEquipDef.cloudburstIndex = CloudburstEquipmentIndex.Lumpkin;
            lumpkinEquipDef.customEquip = lumpkin;
            lumpkinEquipDef.equipmentDef = lumpkinDef;
            lumpkinEquipDef.equipmentIndex = lumpkinIndex;
            lumpkinEquipDef.name = "Lumpkin";

            reactorEquipDef.cloudburstIndex = CloudburstEquipmentIndex.Reactor;
            reactorEquipDef.customEquip = reactor;
            reactorEquipDef.equipmentDef = reactorDef;
            reactorEquipDef.equipmentIndex = unstableQuantumReactorIndex;
            reactorEquipDef.name = "Reactor";

            printerEquipDef.cloudburstIndex = CloudburstEquipmentIndex.AllyPrinter;
            printerEquipDef.customEquip = printer;
            printerEquipDef.equipmentDef = printerDef;
            printerEquipDef.equipmentIndex = lumpkinIndex;
            printerEquipDef.name = "AllyPrinter";
            #endregion
        }
        public static void Scream(CharacterBody Screamer)
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
        public static void SummonMjnion(CharacterBody user)
        {
            CharacterMaster characterMaster;
            characterMaster = new MasterSummon
            {
                masterPrefab = MasterCatalog.GetMasterPrefab(MasterCatalog.FindAiMasterIndexForBody(user.bodyIndex)),
                position = user.footPosition + user.transform.up + user.transform.up + user.transform.up,
                rotation = user.transform.rotation,
                summonerBodyObject = null,
                ignoreTeamMemberLimit = false,
                teamIndexOverride = user.teamComponent.teamIndex,
                loadout = user.master.loadout
            }.Perform();
                
            characterMaster.bodyPrefab = user.master.bodyPrefab;
            characterMaster.Respawn(characterMaster.GetBody().footPosition, Quaternion.identity);

            Inventory inventory = characterMaster.inventory;
            if (inventory) {
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
        public static void BecomeUnstable(CharacterBody user)
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
        /*public static void ViveLaFrance(CharacterBody user)
        {
            InputBankTest aimRay = user.inputBank;
            if (molotovCocktail == null)
            {
                molotovCocktail = Resources.Load<GameObject>("prefabs/projectiles/SporeGrenadeProjectile").InstantiateClone("MegaSporeGrenadeProjectile", false);
                molotovCocktailZone = Resources.Load<GameObject>("prefabs/projectiles/BeetleQueenAcid").InstantiateClone("CIScepMageFlamethrowerCloud");

                ProjectileController molotovController = molotovCocktail.GetComponent<ProjectileController>();
                molotovController.procCoefficient = 1;
                molotovController.ghostPrefab = Resources.Load<GameObject>("prefabs/projectileghosts/FireballGhost");

                ProjectileDamage molotovDamage = molotovCocktail.GetComponent<ProjectileDamage>();
                molotovDamage.damageType = DamageType.IgniteOnHit;

                ProjectileImpactExplosion projectileImpactExplosion = molotovCocktail.GetComponent<ProjectileImpactExplosion>();
                projectileImpactExplosion.childrenProjectilePrefab = molotovCocktailZone;
                projectileImpactExplosion.impactEffect = Resources.Load<GameObject>("prefabs/effects/impacteffects/FireballVehicleDamageEffect");
                molotovCocktailZone = Resources.Load<GameObject>("prefabs/projectiles/BeetleQueenAcid").InstantiateClone("CIScepMageFlamethrowerCloud");

                ThreeEyedGames.Decal decal = molotovCocktailZone.GetComponent<ThreeEyedGames.Decal>();
                decal.Material = Resources.Load<Material>("Materials/matBlighted");
                BaseHelpers.RegisterNewProjectile(molotovCocktail);
                BaseHelpers.RegisterNewProjectile(molotovCocktailZone);

            }
            var projInfo = new FireProjectileInfo
            {
                crit = false,
                damage = 5 * user.damage,
                owner = user.gameObject,
                position = user.transform.position,
                projectilePrefab = molotovCocktail,
                rotation = Util.QuaternionSafeLookRotation(aimRay.aimDirection),
            };
            ProjectileManager.instance.FireProjectile(projInfo);
        }*/
    }
}