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
    //this shit is a fucking failure man
    //my computer won't charge
    //all my art is on said computer which won't fucking charge
    //i might've just lost months of hard work
    //and i have no fucking money to buy a replacement
    //or do to literally anything else with it
    //i own a literal fucking brick

    //meanwhile i have to do code for this fucking starstorm 2 thing
    //and then do code for fucking moonfall or whatever it is
    //because i am the only person well versed enough and with bad enough decision making to code for it
    //this shit is a fucking failure man
    //a literal, fucking failure.

    //i have literally no one to comfort me because everyone around has such fucking hostilty towards me
    //i don't have my dogs
    //i don't have my fucking cats
    //i have literally fucking nothing right
    //fucking. nothing.
    //my life is fucking garbage and id kill myself if i had the balls 

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
