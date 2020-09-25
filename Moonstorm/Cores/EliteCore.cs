using Moonstorm.Cores.Components;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.CombatDirector;

namespace Moonstorm.Cores
{
    class EliteCore
    {
        public static EliteCore instance;

        //TODO:
        //Implement Lunar elites.
        //Figure out the color issue.
        protected BuffIndex voidIndex;
        protected EquipmentIndex voidEquip;
        protected EliteIndex voidElite;
        //protected int voidTier;

        protected BuffIndex tarIndex;
        protected EquipmentIndex tarEquip;
        protected EliteIndex tarElite;

        protected BuffIndex lunarIndex;
        protected EquipmentIndex lunarEquip;
        protected EliteIndex lunarElite;

        protected BuffIndex warIndex;
        protected BuffIndex warFriendlyBuffIndex;
        protected EquipmentIndex warEquip;
        protected EliteIndex warElite;

        public static GameObject warBubble;

        public EliteCore() => RegisterElites();

        protected internal void RegisterElites()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());
            instance = this;

            Void();
            War();
            Tar();

            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self)
            {
                float attackSpeed = self.attackSpeed;
                float armor = self.armor;
                float moveSpeed = self.moveSpeed;
                if (self.HasBuff(tarIndex)) {
                    //"hey what if i made an enemy that had 1000% move speed?"
                    //"wait. oh, no"
                    self.SetPropertyValue("moveSpeed", moveSpeed += 5);
                };
                if (self.HasBuff(warIndex)) {
                    self.SetPropertyValue("attackSpeed", attackSpeed += 1);
                    self.SetPropertyValue("moveSpeed", moveSpeed += 3);
                    self.SetPropertyValue("armor", armor += 1f);
                }
                if (self.HasBuff(warFriendlyBuffIndex) && !self.HasBuff(warIndex))
                {
                    self.SetPropertyValue("attackSpeed", attackSpeed += .5f);
                    self.SetPropertyValue("moveSpeed", moveSpeed += 1.5f);
                    self.SetPropertyValue("armor", armor += .5f);
                };
            }
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (damageInfo == null || self == null || damageInfo == null || damageInfo.attacker == null || victim == null) return;

            GameObject attacker = damageInfo.attacker;
            CharacterBody attackerBody = attacker.GetComponent<CharacterBody>();

            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            if (attacker && attackerBody && victimBody)
            {
                Inventory attackerInv = attackerBody.inventory;
                if (attackerBody.HasBuff(tarIndex))
                {
                    victimBody.AddTimedBuff(BuffIndex.ClayGoo, 2);
                    attackerBody.healthComponent.HealFraction(0.25f, damageInfo.procChainMask);
                }
                if (attackerBody.HasBuff(voidIndex))
                {
                    Util.PlaySound("Play_nullifier_attack1_root", victimBody.gameObject);
                    victimBody.AddTimedBuff(BuffIndex.NullifyStack, 10);
                }
            }

            orig(self, damageInfo, victim);
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            if (self && self.inventory)
            {
                Inventory inv = self.inventory;
                self.AddItemBehavior<AffixWarBehavior>(self.HasBuff(warIndex) ? 1 : 0);

                if (self.HasBuff(tarIndex))
                {
                    Transform modelBaseTransform = self.modelLocator.modelBaseTransform;
                    if (modelBaseTransform)
                    {
                        EnableGoldAffixEffect gold = self.gameObject.AddComponent<EnableGoldAffixEffect>();
                        if (!gold.isGoldEffectEnabled) {
                            gold.EnableGoldAffix();
                        }
                        //Legacy
                        /*var characterModel = modelBaseTransform.GetComponent<CharacterModel>();

                        TemporaryOverlay temporaryOverlay = modelBaseTransform.gameObject.AddComponent<TemporaryOverlay>();
                        temporaryOverlay.duration = 9000000f;
                        temporaryOverlay.animateShaderAlpha = true;
                        temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 9000000f, 1f);
                        temporaryOverlay.destroyComponentOnEnd = false;
                        temporaryOverlay.originalMaterial = CharacterModel.clayGooMaterial;

                        temporaryOverlay.AddToCharacerModel(characterModel);*/
                    }
                }
            }
        }

        protected internal void Void()
        {
            Color voidColor = new Color(0.79607844f, 0.4745098f, 0.8352941f);

            EquipmentDef equipmentDef = new EquipmentDef
            {
                name = "AffixPurple",
                cooldown = 10f,
                pickupModelPath = "",
                pickupIconPath = "",
                nameToken = "EQUIPMENT_AFFIXPURPLE_NAME",
                pickupToken = "EQUIPMENT_AFFIXPURPLE_PICKUP",
                descriptionToken = "EQUIPMENT_AFFIXPURPLE_DESC",
                canDrop = false,
                enigmaCompatible = false
            };

            BuffDef buffDef = new BuffDef
            {
                name = "VoidEmbrace",
                buffColor = voidColor,
                iconPath = "Textures/BuffIcons/texBuffNullifiedIcon",
                canStack = false
            };

            EliteDef eliteDef = new EliteDef
            {
                name = "Void",
                modifierToken = "ELITE_MODIFIER_VOID",
                color = buffDef.buffColor,
            };

            //this is bad, but it will have to do
            /*bool CanVoidSpawn() {
                return KinIsntActive() && ProgressTracker.IsVoidComplete();
            }*/

            //R2API is garbage
            //how the fuck do you make an api so shit
            //that it doesn't even fucking work
            //and you have to fucking do dirty hacks for it to work
            //fuck R2API 
            /*EliteTierDef def = new EliteTierDef() {
                costMultiplier = 6,
                damageBoostCoefficient = 4,
                healthBoostCoefficient = 2,
                isAvailable = new Func<bool>(CanVoidSpawn)
            };*/

            //voidTier = EliteAPI.AddCustomEliteTier(def);

            var customEquipment = new CustomEquipment(equipmentDef, new ItemDisplayRule[0]);
            var customBuff = new CustomBuff(buffDef);
            //LogCore.LogI(voidTier);
            var customElite = new CustomElite(eliteDef, 2); //voidTier);

            //set our indexes
            voidElite = EliteAPI.Add(customElite);
            voidIndex = BuffAPI.Add(customBuff);
            voidEquip = ItemAPI.Add(customEquipment);

            //set the definitions' indexes
            eliteDef.eliteEquipmentIndex = voidEquip;
            equipmentDef.passiveBuff = voidIndex;
            buffDef.eliteIndex = voidElite;

            //and finally, add a prefix
            LanguageAPI.Add(eliteDef.modifierToken, "Void {0}");
            LanguageAPI.Add(equipmentDef.nameToken, "Void's Embrace");
            LanguageAPI.Add(equipmentDef.pickupToken, "Become an aspect of Void");
            LanguageAPI.Add(equipmentDef.descriptionToken, "");
        }

        protected internal void Lunar()
        {
            EquipmentDef equipmentDef = new EquipmentDef
            {
                name = "AffixMoon",
                cooldown = 10f,
                pickupModelPath = "",
                pickupIconPath = "",
                nameToken = "EQUIPMENT_AFFIXMOON_NAME",
                pickupToken = "EQUIPMENT_AFFIXMOON_PICKUP",
                descriptionToken = "EQUIPMENT_AFFIXMOON_DESC",
                canDrop = false,
                enigmaCompatible = false
            };

            BuffDef buffDef = new BuffDef
            {
                name = "LunarEmbrace",
                iconPath = "Textures/BuffIcons/texBuffLunarShellIcon",
                canStack = false
            };

            EliteDef eliteDef = new EliteDef
            {
                name = "Lunar",
                modifierToken = "ELITE_MODIFIER_LUNAR",
                color = Color.red
            };

            CustomEquipment customEquipment = new CustomEquipment(equipmentDef, new ItemDisplayRule[0]);
            CustomBuff customBuff = new CustomBuff(buffDef);
            CustomElite customElite = new CustomElite(eliteDef, 2);

            lunarElite = EliteAPI.Add(customElite);
            lunarIndex = BuffAPI.Add(customBuff);
            lunarEquip = ItemAPI.Add(customEquipment);

            eliteDef.eliteEquipmentIndex = lunarEquip;
            equipmentDef.passiveBuff = lunarIndex;
            buffDef.eliteIndex = lunarElite;

            LanguageAPI.Add(eliteDef.modifierToken, "Lunar {0}");
            LanguageAPI.Add(equipmentDef.nameToken, "Lunar Embrace");
            LanguageAPI.Add(equipmentDef.pickupToken, "Become an aspect of the Moon");
            LanguageAPI.Add(equipmentDef.descriptionToken, "");

        }

        protected internal bool KinIsntActive() {
            return !RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.eliteOnlyArtifactDef);
        }

        protected internal void War()
        {
            Color warColor = new Color(0.8392157f, 0.7882353f, 0.22745098f);

            EquipmentDef equipmentDef = new EquipmentDef
            {
                name = "AffixOrange",
                cooldown = 10f,
                pickupModelPath = "",
                pickupIconPath = "",
                nameToken = "EQUIPMENT_AFFIXORANGE_NAME",
                pickupToken = "EQUIPMENT_AFFIXORANGE_PICKUP",
                descriptionToken = "EQUIPMENT_AFFIXORANGE_DESC",
                canDrop = false,
                enigmaCompatible = false
            };

            BuffDef buffDef = new BuffDef
            {
                name = "Enrolled",
                buffColor = warColor,
                iconPath = "Textures/BuffIcons/texBuffWarbannerIcon",
                canStack = false
            };

            BuffDef buffDef2 = new BuffDef
            {
                name = "Frenzied",
                buffColor = warColor,
                iconPath = "Textures/BuffIcons/texBuffWarbannerIcon",
                canStack = false
            };

            EliteDef eliteDef = new EliteDef
            {
                name = "Enrolled",
                modifierToken = "ELITE_MODIFIER_WAR",
                color = warColor,
                //eliteEquipmentIndex = voidEquip
            };

            CustomEquipment customEquipment = new CustomEquipment(equipmentDef, new ItemDisplayRule[0]);
            CustomBuff customBuff = new CustomBuff(buffDef);
            CustomBuff customBuff2 = new CustomBuff(buffDef2);
            CustomElite customElite = new CustomElite(eliteDef, 1);

            warElite = EliteAPI.Add(customElite);
            warIndex = BuffAPI.Add(customBuff);
            warFriendlyBuffIndex = BuffAPI.Add(customBuff2);
            warEquip = ItemAPI.Add(customEquipment);

            eliteDef.eliteEquipmentIndex = warEquip;
            equipmentDef.passiveBuff = warIndex;
            buffDef.eliteIndex = warElite;

            LanguageAPI.Add(eliteDef.modifierToken, "War {0}");
            LanguageAPI.Add(equipmentDef.nameToken, "War's Embrace");
            LanguageAPI.Add(equipmentDef.pickupToken, "Become an aspect of War");
            LanguageAPI.Add(equipmentDef.descriptionToken, "");
            warBubble = Resources.Load<GameObject>("Prefabs/NetworkedObjects/WarBannerWard").InstantiateClone("AffixWarWard", true);
            BuffWard ward = warBubble.GetComponent<BuffWard>();
            ward.buffType = warFriendlyBuffIndex;
            ward.floorWard = false;
            NetworkedBodyAttachment attachment = warBubble.AddComponent<NetworkedBodyAttachment>();
            attachment.shouldParentToAttachedBody = true;
            attachment.forceHostAuthority = false;

            Transform mdlWarbanner = warBubble.transform.Find("mdlWarbanner");
            if (mdlWarbanner)
            {
                mdlWarbanner.gameObject.SetActive(false);
            }
        }

        protected internal void Tar()
        {
            Color tarColor = new Color(0.2f, 0.09019608f, 0.09019608f);
            EquipmentDef equipmentDef = new EquipmentDef {
                name = "AffixTar",
                cooldown = 10f,
                pickupModelPath = "",
                pickupIconPath = "",
                nameToken = "EQUIPMENT_AFFIXTAR_NAME",
                pickupToken = "EQUIPMENT_AFFIXTAR_PICKUP",
                descriptionToken = "EQUIPMENT_AFFIXTAR_DESC",
                canDrop = false,
                enigmaCompatible = false
            };

            BuffDef buffDef = new BuffDef {
                name = "TarBorne",
                iconPath = "Textures/BuffIcons/texMovespeedBuffIcon",
                canStack = false,
                buffColor = tarColor
            };

            EliteDef eliteDef = new EliteDef {
                name = "TarBorne",
                modifierToken = "ELITE_MODIFIER_TAR",
            };

            CustomEquipment customEquipment = new CustomEquipment(equipmentDef, new ItemDisplayRule[0]);
            CustomBuff customBuff = new CustomBuff(buffDef);
            CustomElite customElite = new CustomElite(eliteDef, 1);

            tarEquip = ItemAPI.Add(customEquipment);
            tarIndex = BuffAPI.Add(customBuff);
            tarElite = EliteAPI.Add(customElite);

            eliteDef.eliteEquipmentIndex = tarEquip;
            equipmentDef.passiveBuff = tarIndex;
            buffDef.eliteIndex = tarElite;

            LanguageAPI.Add(eliteDef.modifierToken, "Tarborne {0}");
            LanguageAPI.Add(equipmentDef.nameToken, "Their Embrace");
            LanguageAPI.Add(equipmentDef.pickupToken, "Become an aspect of Tar");
            LanguageAPI.Add(equipmentDef.descriptionToken, "");
        }
    }
    public class AffixWarBehavior : CharacterBody.ItemBehavior
    {
        private GameObject warWard;
        private void FixedUpdate()
        {
            if (!NetworkServer.active)
            {
                return;
            }
            bool enabled = stack > 0;
            if (warWard != enabled)
            {
                if (enabled)
                {
                    warWard = Instantiate<GameObject>(EliteCore.warBubble);
                    warWard.GetComponent<TeamFilter>().teamIndex = body.teamComponent.teamIndex;
                    warWard.GetComponent<BuffWard>().Networkradius = 15 + body.radius;
                    warWard.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(body.gameObject);
                    return;
                }
                Destroy(warWard);
                warWard = null;
            }
        }
        private void OnDisable()
        {
            if (warWard)
            {
                Destroy(warWard);
            }
        }

    }
}

