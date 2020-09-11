using Cloudburst.Cores.States.Commando;
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
    public class ModificationCore
    {
        public static GameObject collectorObject;

        public ModificationCore() => Rebalance();
        protected void Rebalance() {
            LogCore.LogI("Initializing Core: " + base.ToString());

            Commando();
            REX();
        }

        protected void Commando()
        {
            var commandoBody = Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");
            /*var body = commandoBody.GetComponent<CharacterBody>();

            body.baseMaxHealth = 110;
            body.baseRegen = 1;
            body.baseMaxShield = 0;
            body.baseMoveSpeed = 7;
            body.baseJumpPower = 15;
            body.baseDamage = 12;
            body.baseAttackSpeed = 1;
            body.baseCrit = 1;
            body.baseJumpCount = 1;
            body.sprintingSpeedMultiplier = 1.45f;
            //i *might* have forgotton about this
            //body.baseJumpCount = 33;
            body.levelRegen = 0.2f;
            body.levelMaxShield = 0;
            body.levelMoveSpeed = 0;
            body.levelJumpPower = 0;
            body.levelDamage = 2.4f;
            body.levelAttackSpeed = 0;
            body.levelCrit = 0;
            body.levelArmor = 0;*/
            //I wanted to do a rebalance but decided against it

            LoadoutAPI.AddSkill(typeof(CommandoGrenade));

            var locator = commandoBody.GetComponent<SkillLocator>();

            var toolbotDef = Resources.Load<SkillDef>("skilldefs/toolbotbody/ToolBotBodyStunDrone");
            var origDef = Resources.Load<SkillDef>("skilldefs/commandobody/ThrowGrenade");

            var def = locator.special.skillFamily.variants[1].skillDef;

            var icon = origDef.icon;
            var skillNameToken = origDef.skillNameToken;
            var skillDescriptionToken = origDef.skillDescriptionToken;

            API.CopySkillDefSettings(toolbotDef, def);

            def.icon = icon;
            def.skillNameToken = skillNameToken;
            def.skillDescriptionToken = skillDescriptionToken;
            def.baseMaxStock = 2;
            def.activationState = new EntityStates.SerializableEntityStateType(typeof(CommandoGrenade));
            def.keywordTokens = Array.Empty<String>(); 

            var grenade = Resources.Load<GameObject>("prefabs/projectiles/CommandoGrenadeProjectile");
            grenade.GetComponent<ProjectileImpactExplosion>().lifetimeAfterImpact = 0;
        }

        protected void REX() {
            var healingProjectile = Resources.Load<GameObject>("prefabs/projectiles/SyringeProjectileHealing");
            var healingComponent = healingProjectile.GetComponent<ProjectileHealOwnerOnDamageInflicted>();
            var rexBody = Resources.Load<GameObject>("prefabs/characterbodies/TreebotBody");
            var skillLocator = rexBody.GetComponent<SkillLocator>();

            var skillFamily = skillLocator.special.skillFamily;

            var harvest = ScriptableObject.CreateInstance<SkillDef>();

            harvest.activationState = new SerializableEntityStateType(typeof(CreatePounder));
            harvest.activationStateMachineName = "Weapon";
            harvest.baseMaxStock = 1;
            harvest.baseRechargeInterval = 2;
            harvest.beginSkillCooldownOnSkillEnd = true;
            harvest.canceledFromSprinting = false;
            harvest.dontAllowPastMaxStocks = false;
            harvest.forceSprintDuringState = false;
            harvest.fullRestockOnAssign = true;
            harvest.icon = null;
            harvest.interruptPriority = InterruptPriority.PrioritySkill;
            harvest.isBullets = false;
            harvest.isCombatSkill = false;
            harvest.keywordTokens = Array.Empty<string>();
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
            LanguageAPI.Add(harvest.skillDescriptionToken, "Place a <style=cIsUtility>harvester</style> that deals <style=cIsDamage>200%</style> per attempted harvest.");


            var variant = new SkillFamily.Variant() {
                skillDef = harvest,
                unlockableName = "",
            };

            var length = skillFamily.variants.Length;

            Array.Resize(ref skillFamily.variants, length + 1) ;
            skillFamily.variants[length ] = variant;

            healingComponent.fractionOfDamage = 0.3f;

            CreateCollector();
        }

        protected void CreateCollector() {
            collectorObject = Resources.Load<GameObject>("prefabs/projectiles/TreebotPounderProjectile");//.InstantiateClone("TreebotCollector", true);
            var stateMachine = collectorObject.GetComponent<EntityStateMachine>();

            LoadoutAPI.AddSkill(typeof(Harvest));
            LoadoutAPI.AddSkill(typeof(Spawn));
            stateMachine.mainStateType = new SerializableEntityStateType(typeof(Harvest));
            stateMachine.initialStateType = new SerializableEntityStateType(typeof(Spawn));

            //collectorObject.AddComponent<DestroyOnTimer>().duration = 15;

            //API.RegisterNewProjectile(collectorObject);
            //CreatePounder.projectilePrefab = collectorObject;
        }
    }
}