using Cloudburst.Cores.States.Huntress;
using EntityStates;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cloudburst.Cores
{


    class HuntressCore
    {
        public static HuntressCore instance;
        public static GameObject arrowObject;
        public HuntressCore() => WIP();

        protected void WIP() {
            instance = this;
            //AddPrimarySkill();
            //CreateSeekingArrow();
        }

        protected void CreateSeekingArrow() {
            //my weak hand and knees
            arrowObject = Resources.Load<GameObject>("prefabs/projectiles/Arrow").InstantiateClone("HuntressSeekingArrow", true);

            var projectileTargetComponent = arrowObject.AddComponent<ProjectileTargetComponent>();
            var projectileFinder = arrowObject.AddComponent<ProjectileDirectionalTargetFinder>();
            var projectileSteer = arrowObject.AddComponent<ProjectileSteerTowardTarget>();
            var projectileSimple = arrowObject.GetComponent<ProjectileSimple>();

            projectileFinder.lookCone = 6.5f;
            projectileFinder.lookRange = 60f;
            projectileFinder.targetSearchInterval = 0.15f;
            projectileFinder.onlySearchIfNoTarget = false;
            projectileFinder.allowTargetLoss = true;
            projectileFinder.testLoS = true;
            projectileFinder.ignoreAir = false;

            projectileSteer.rotationSpeed = 45f;

            projectileSimple.updateAfterFiring = true;
        }

        protected void AddPrimarySkill() {
            var body = Resources.Load<GameObject>("prefabs/characterbodies/HuntressBody");
            var skillLocator = body.GetComponent<SkillLocator>();
            var skillFamily = skillLocator.primary.skillFamily;
            LoadoutAPI.AddSkill(typeof(ShootArrow));

            SkillDef primarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            primarySkillDef.activationState = new SerializableEntityStateType(typeof(ShootArrow));
            primarySkillDef.activationStateMachineName = "Weapon";
            primarySkillDef.baseMaxStock = 1;
            primarySkillDef.baseRechargeInterval = 0;
            primarySkillDef.beginSkillCooldownOnSkillEnd = false;
            primarySkillDef.canceledFromSprinting = false;
            primarySkillDef.fullRestockOnAssign = true;
            primarySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            primarySkillDef.isBullets = false;
            primarySkillDef.isCombatSkill = true;
            primarySkillDef.mustKeyPress = false;
            primarySkillDef.noSprint = false;
            primarySkillDef.rechargeStock = 1;
            primarySkillDef.requiredStock = 1;
            primarySkillDef.shootDelay = 0.5f;
            primarySkillDef.stockToConsume = 1;
            primarySkillDef.skillDescriptionToken = "AAAAAAAAAAAAAAAAAAAAAA";
            primarySkillDef.skillName = "aaa";
            primarySkillDef.skillNameToken = "aa";


            var variant = new SkillFamily.Variant()
            {
                skillDef = primarySkillDef,
                unlockableName = "",
            };

            var length = skillFamily.variants.Length;

            Array.Resize(ref skillFamily.variants, length + 1);
            skillFamily.variants[length] = variant;

            LoadoutAPI.AddSkillDef(primarySkillDef);
            //skillLocator.utility.skillDef.activationState = new SerializableEntityStateType(typeof(EntityStates.Huntress.BackflipState));
        }
    }
}
