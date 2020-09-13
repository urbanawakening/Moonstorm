using EntityStates;
using R2API;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cloudburst.Cores
{
    class WIPCore
    {
        public static WIPCore instance;
        public WIPCore() => WIP();

        protected void WIP() {
            instance = this;
            Huntress();
        }

        protected void Huntress() {
            var body = Resources.Load<GameObject>("prefabs/characterbodies/HuntressBody");
            var skillLocator = body.GetComponent<SkillLocator>();

            SkillDef primarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            primarySkillDef.activationState = new SerializableEntityStateType(typeof(EntityStates.Huntress.HuntressWeapon.ChargeArrow));
            primarySkillDef.activationStateMachineName = "Weapon";
            primarySkillDef.baseMaxStock = 1;
            primarySkillDef.baseRechargeInterval = 2;
            primarySkillDef.beginSkillCooldownOnSkillEnd = true;
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

            LoadoutAPI.AddSkillDef(primarySkillDef);
            skillLocator.primary.skillDef = primarySkillDef;
            skillLocator.utility.skillDef.activationState = new SerializableEntityStateType(typeof(EntityStates.Huntress.BackflipState));

        }

    }
}
