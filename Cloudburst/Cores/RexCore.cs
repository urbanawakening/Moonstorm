using Cloudburst.Cores.Components;
using Cloudburst.Cores.States.REX;
using EntityStates;
using EntityStates.Treebot.Weapon;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using UnityEngine;

namespace Cloudburst.Cores
{
    class RexCore
    {
        public static RexCore instance;

        public RexCore() => REX();
        protected GameObject collectorObject;
            protected void REX()
            {
            instance = this;


            var healingProjectile = Resources.Load<GameObject>("prefabs/projectiles/SyringeProjectileHealing");
            var healingComponent = healingProjectile.GetComponent<ProjectileHealOwnerOnDamageInflicted>();
            var rexBody = Resources.Load<GameObject>("prefabs/characterbodies/TreebotBody");
            var skillLocator = rexBody.GetComponent<SkillLocator>();

            //uhhh no
            //rexBody.AddComponent<EnableGoldAffixEffect>().EnableGoldAffix();


            var skillFamily = skillLocator.special.skillFamily;

            var harvest = ScriptableObject.CreateInstance<SkillDef>();

            harvest.activationState = new SerializableEntityStateType(typeof(CreatePounder));
            harvest.activationStateMachineName = "Weapon";
            harvest.baseMaxStock = 1;
            harvest.baseRechargeInterval = 22;
            harvest.beginSkillCooldownOnSkillEnd = true;
            harvest.canceledFromSprinting = false;
            harvest.dontAllowPastMaxStocks = false;
            harvest.forceSprintDuringState = false;
            harvest.fullRestockOnAssign = true;
            harvest.icon = AssetsCore.rexHarvester;
            harvest.interruptPriority = InterruptPriority.PrioritySkill;
            harvest.isBullets = false;
            harvest.isCombatSkill = false;
            harvest.keywordTokens = new string[1] {
                    "KEYWORD_WEAK"
                };
            harvest.mustKeyPress = true;
            harvest.noSprint = false;
            harvest.rechargeStock = 1;
            harvest.requiredStock = 1;
            harvest.shootDelay = 0.1f;
            harvest.skillDescriptionToken = "TREEBOT_SPECIAL_ALT1_DESCRIPTION";
            harvest.skillName = "TREEBOT_SPECIAL_ALT1_NAME";
            harvest.skillNameToken = "TREEBOT_SPECIAL_ALT1_NAME";
            harvest.stockToConsume = 1;

            LanguageAPI.Add(harvest.skillNameToken, "DIRECTIVE: HARVEST");
            LanguageAPI.Add(harvest.skillDescriptionToken, "Place a <style=cIsUtility>harvester</style> that deals <style=cIsDamage>250%</style> and <style=cIsDamage>Weakens</style> per attempted harvest.");

            var variant = new SkillFamily.Variant()
            {
                skillDef = harvest,
                unlockableName = AchievementCore.GetUnlockableString("Paradox"),
            };

            var length = skillFamily.variants.Length;

            Array.Resize(ref skillFamily.variants, length + 1);
            skillFamily.variants[length] = variant;

            healingComponent.fractionOfDamage = 0.3f;

            CreateCollector();
        }

        protected void CreateCollector()
        {
            collectorObject = Resources.Load<GameObject>("prefabs/projectiles/TreebotPounderProjectile");// this broke shit .InstantiateClone("TreebotCollectorProjectile", true);
            var stateMachine = collectorObject.GetComponent<EntityStateMachine>();

            LoadoutAPI.AddSkill(typeof(Harvest));
            LoadoutAPI.AddSkill(typeof(Spawn));
            stateMachine.mainStateType = new SerializableEntityStateType(typeof(Harvest));
            stateMachine.initialStateType = new SerializableEntityStateType(typeof(Spawn));

            //collectorObject.AddComponent<DestroyOnTimer>().duration = 15;

            API.RegisterNewProjectile(collectorObject);
            CreatePounder.projectilePrefab = collectorObject;
        }
    }
}
