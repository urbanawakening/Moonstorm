using R2API;
using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;



    public static class API
    {
        #region Catalog Helpers
        /// <summary>
        /// Adds a GameObject to the projectile catalog and returns true
        /// GameObject must be non-null and have a ProjectileController component
        /// returns false if GameObject is null or is missing the component        
        /// </summary>
        /// <param name="projectileObject"></param>
        /// <returns></returns>
        public static bool RegisterNewProjectile(GameObject projectileObject)
        {
            if (projectileObject.HasComponent<ProjectileController>())
            {
                ProjectileCatalog.getAdditionalEntries += list => list.Add(projectileObject);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Adds a GameObject to the body catalog and returns true
        /// GameObject must be non-null and have a CharacterBody component
        /// returns false if GameObject is null or is missing the component
        /// /// </summary>
        /// <param name="bodyObject"></param>
        /// <returns></returns>
        public static bool RegisterNewBody(GameObject bodyObject)
        {
            if (bodyObject)
            {
                BodyCatalog.getAdditionalEntries += list => list.Add(bodyObject);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Adds a GameObject to the master catalog and returns true
        /// GameObject must be non-null and have a CharacterMaster component
        /// returns false if GameObject is null or is missing the component
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public static bool RegisterNewMaster(GameObject master)
        {
            if (master && master.HasComponent<CharacterMaster>())
            {
                MasterCatalog.getAdditionalEntries += list => list.Add(master);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Destroys all the AISkillDrivers on the master GameObject
        /// </summary>
        /// <param name="masterObject"></param>
        public static void DestroySkillDrivers(GameObject master)
        {
            foreach (AISkillDriver skill in master.GetComponentsInChildren<AISkillDriver>())
            {
                Object.Destroy(skill);
            }
        }
        #endregion
        #region AI
        /// <summary>
        /// Copies skilldriver settings from "beingCopiedFrom" to "copier"
        /// Don't forget to set requiredSkill!
        /// </summary>
        /// <param name="beingCopiedFrom"></param>
        /// <param name="copier"></param>
        public static void CopyAISkillSettings(AISkillDriver beingCopiedFrom, AISkillDriver copier)
        {
            copier.activationRequiresAimConfirmation = beingCopiedFrom.activationRequiresAimConfirmation;
            copier.activationRequiresTargetLoS = beingCopiedFrom.activationRequiresTargetLoS;
            copier.aimType = beingCopiedFrom.aimType;
            copier.buttonPressType = beingCopiedFrom.buttonPressType;
            copier.customName = beingCopiedFrom.customName;
            copier.driverUpdateTimerOverride = beingCopiedFrom.driverUpdateTimerOverride;
            copier.ignoreNodeGraph = beingCopiedFrom.ignoreNodeGraph;
            copier.maxDistance = beingCopiedFrom.maxDistance;
            copier.maxTargetHealthFraction = beingCopiedFrom.maxTargetHealthFraction;
            copier.maxUserHealthFraction = beingCopiedFrom.maxUserHealthFraction;
            copier.minDistance = beingCopiedFrom.minDistance;
            copier.minTargetHealthFraction = beingCopiedFrom.minTargetHealthFraction;
            copier.minUserHealthFraction = beingCopiedFrom.minUserHealthFraction;
            copier.moveInputScale = beingCopiedFrom.moveInputScale;
            copier.movementType = beingCopiedFrom.movementType;
            copier.moveTargetType = beingCopiedFrom.moveTargetType;
            copier.nextHighPriorityOverride = beingCopiedFrom.nextHighPriorityOverride;
            copier.noRepeat = beingCopiedFrom.noRepeat;
            //Don't do this because the skilldef is not the same.
            //_out.requiredSkill = _in.requiredSkill;
            copier.requireEquipmentReady = beingCopiedFrom.requireEquipmentReady;
            copier.requireSkillReady = beingCopiedFrom.requireSkillReady;
            copier.resetCurrentEnemyOnNextDriverSelection = beingCopiedFrom.resetCurrentEnemyOnNextDriverSelection;
            copier.selectionRequiresOnGround = beingCopiedFrom.selectionRequiresOnGround;
            copier.selectionRequiresTargetLoS = beingCopiedFrom.selectionRequiresTargetLoS;
            copier.shouldFireEquipment = beingCopiedFrom.shouldFireEquipment;
            copier.shouldSprint = beingCopiedFrom.shouldSprint;
            //shouldTapButton is deprecated, don't use it!
            //_out.shouldTapButton = _in.shouldTapButton;
            copier.skillSlot = beingCopiedFrom.skillSlot;

        }
        #endregion
        #region Skills
        /// <summary>
        /// Destroys generic skill components attached to the survivor object and creates an empty SkillFamily.
        /// </summary>
        /// <param name="survivor"></param>
        public static void CreateEmptySkills(GameObject survivor)
        {
            DestroyGenericSkillComponents(survivor);
            CreateEmptySkillFamily(survivor);
        }
        /// <summary>
        /// Destroys generic skill components attached to the survivor object
        /// </summary>
        /// <param name="survivor"></param>
        public static void DestroyGenericSkillComponents(GameObject survivor)
        {
            foreach (GenericSkill skill in survivor.GetComponentsInChildren<GenericSkill>())
            {
                Object.DestroyImmediate(skill);
            }
        }
        /// <summary>
        /// Creates an EmptySkillFamily. Be sure to call DestroyGenericSkillComponents before doing this.
        /// </summary>
        /// <param name="survivor"></param>
        public static void CreateEmptySkillFamily(GameObject survivor)
        {
            SkillLocator skillLocator = survivor.GetComponent<SkillLocator>();
            skillLocator.SetFieldValue<GenericSkill[]>("allSkills", new GenericSkill[0]);
            {
                skillLocator.primary = survivor.AddComponent<GenericSkill>();
                SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
                newFamily.variants = new SkillFamily.Variant[1];
                LoadoutAPI.AddSkillFamily(newFamily);
                skillLocator.primary.SetFieldValue("_skillFamily", newFamily);
            }
            {
                skillLocator.secondary = survivor.AddComponent<GenericSkill>();
                SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
                newFamily.variants = new SkillFamily.Variant[1];
                LoadoutAPI.AddSkillFamily(newFamily);
                skillLocator.secondary.SetFieldValue("_skillFamily", newFamily);
            }
            {
                skillLocator.utility = survivor.AddComponent<GenericSkill>();
                SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
                newFamily.variants = new SkillFamily.Variant[1];
                LoadoutAPI.AddSkillFamily(newFamily);
                skillLocator.utility.SetFieldValue("_skillFamily", newFamily);
            }
            {
                skillLocator.special = survivor.AddComponent<GenericSkill>();
                SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
                newFamily.variants = new SkillFamily.Variant[1];
                LoadoutAPI.AddSkillFamily(newFamily);
                skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            }
        }

        /// <summary>
        /// Copies skilldef settings from beingCopiedFrom to copier.
        /// </summary>
        /// <param name="beingCopiedFrom"></param>
        /// <param name="copier"></param>
        public static void CopySkillDefSettings(SkillDef beingCopiedFrom, SkillDef copier)
        {
            copier.activationState = beingCopiedFrom.activationState;
            copier.activationStateMachineName = beingCopiedFrom.activationStateMachineName;
            copier.baseMaxStock = beingCopiedFrom.baseMaxStock;
            copier.baseRechargeInterval = beingCopiedFrom.baseRechargeInterval;
            copier.beginSkillCooldownOnSkillEnd = beingCopiedFrom.beginSkillCooldownOnSkillEnd;
            copier.canceledFromSprinting = beingCopiedFrom.canceledFromSprinting;
            copier.dontAllowPastMaxStocks = beingCopiedFrom.dontAllowPastMaxStocks;
            copier.forceSprintDuringState = beingCopiedFrom.forceSprintDuringState;
            copier.fullRestockOnAssign = beingCopiedFrom.fullRestockOnAssign;
            copier.icon = beingCopiedFrom.icon;
            copier.interruptPriority = beingCopiedFrom.interruptPriority;
            copier.isBullets = beingCopiedFrom.isBullets;
            copier.isCombatSkill = beingCopiedFrom.isCombatSkill;
            copier.keywordTokens = beingCopiedFrom.keywordTokens;
            copier.mustKeyPress = beingCopiedFrom.mustKeyPress;
            copier.noSprint = beingCopiedFrom.noSprint;
            copier.rechargeStock = beingCopiedFrom.rechargeStock;
            copier.shootDelay = beingCopiedFrom.shootDelay;
            copier.skillDescriptionToken = beingCopiedFrom.skillDescriptionToken;
            copier.skillName = beingCopiedFrom.skillName;
            copier.skillNameToken = beingCopiedFrom.skillNameToken;
            copier.stockToConsume = beingCopiedFrom.stockToConsume;
        }
        #endregion
    }
