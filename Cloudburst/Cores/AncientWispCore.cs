using EntityStates;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static RoR2.CharacterAI.AISkillDriver;

namespace Cloudburst.Cores
{
    class AncientWispCore
    {
        protected GameObject master;
        protected GameObject body;
        protected SkillLocator skillLocator;
        public AncientWispCore() => CreateAncientWisp();

        protected void CreateAncientWisp() {
            master = Resources.Load<GameObject>("prefabs/charactermasters/AncientWispMaster");
            body = Resources.Load<GameObject>("prefabs/characterbodies/AncientWispBody");
            skillLocator = body.GetComponent<SkillLocator>();

            AI();
            FixSwingEffect();
        }

        protected void Skills() {

            API.CreateEmptySkills(body);


        }
        private void CreatePrimary()
        {
            SkillDef primarySkillDef = ScriptableObject.CreateInstance<SkillDef>();

            primarySkillDef.activationState = new SerializableEntityStateType(typeof(FullSwing));
            primarySkillDef.activationStateMachineName = "Weapon";
            primarySkillDef.baseMaxStock = 1;
            primarySkillDef.baseRechargeInterval = 0f;
            primarySkillDef.beginSkillCooldownOnSkillEnd = true;
            primarySkillDef.canceledFromSprinting = false;
            primarySkillDef.fullRestockOnAssign = true;
            primarySkillDef.interruptPriority = InterruptPriority.Any;
            primarySkillDef.isBullets = false;
            primarySkillDef.isCombatSkill = true;
            primarySkillDef.mustKeyPress = false;
            primarySkillDef.noSprint = false;
            primarySkillDef.rechargeStock = 1;
            primarySkillDef.requiredStock = 1;
            primarySkillDef.shootDelay = 0.1f;
            primarySkillDef.stockToConsume = 0;
            primarySkillDef.skillDescriptionToken = "AAAAAAAAAAAAAA";
            primarySkillDef.skillName = "AAAAAAAAAAAAAAA";
            primarySkillDef.skillNameToken = "AAAAAAAAAAAAAAAA";
            primarySkillDef.icon = null;
            primarySkillDef.keywordTokens = Array.Empty<string>();

            LoadoutAPI.AddSkillDef(primarySkillDef);
            SkillFamily primarySkillFamily = skillLocator.primary.skillFamily;

            primarySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = primarySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(primarySkillDef.skillNameToken, false, null)

            };
        }

        private void CreateSecondary()
        {
            LoadoutAPI.AddSkill(typeof(ChargeHomeRun));

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            secondarySkillDef.activationState = new SerializableEntityStateType(typeof(ChargeHomeRun));
            secondarySkillDef.activationStateMachineName = "Weapon";
            secondarySkillDef.baseMaxStock = 1;
            secondarySkillDef.baseRechargeInterval = 3f;
            secondarySkillDef.beginSkillCooldownOnSkillEnd = true;
            secondarySkillDef.canceledFromSprinting = false;
            secondarySkillDef.fullRestockOnAssign = false;
            secondarySkillDef.interruptPriority = InterruptPriority.Skill;
            secondarySkillDef.isBullets = false;
            secondarySkillDef.isCombatSkill = true;
            secondarySkillDef.mustKeyPress = false;
            secondarySkillDef.noSprint = false;
            secondarySkillDef.rechargeStock = 1;
            secondarySkillDef.requiredStock = 1;
            secondarySkillDef.shootDelay = 0.08f;
            secondarySkillDef.stockToConsume = 1;
            secondarySkillDef.skillDescriptionToken = "HAND_SECONDARY_DESCRIPTION";
            secondarySkillDef.skillName = "aaa";
            secondarySkillDef.skillNameToken = "HAND_SECONDARY_NAME";
            secondarySkillDef.icon = AssetsCore.forcedReassemblyIcon;
            secondarySkillDef.keywordTokens = new string[] {
                 "KEYWORD_AGILE",
                 "KEYWORD_CHARGEABLE",
                 "KEYWORD_CLOCKED"
             };

            LoadoutAPI.AddSkillDef(secondarySkillDef);
            SkillFamily secondarySkillFamily = skillLocator.secondary.skillFamily;

            secondarySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = secondarySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(secondarySkillDef.skillNameToken, false, null)

            };
        }

        private void CreateUtility()
        {
            SkillDef utilitySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            utilitySkillDef.activationState = new SerializableEntityStateType(typeof(ChargeSlam));
            utilitySkillDef.activationStateMachineName = "Weapon";
            utilitySkillDef.baseMaxStock = 1;
            utilitySkillDef.baseRechargeInterval = 5f;
            utilitySkillDef.beginSkillCooldownOnSkillEnd = true;
            utilitySkillDef.canceledFromSprinting = false;
            utilitySkillDef.fullRestockOnAssign = false;
            utilitySkillDef.interruptPriority = InterruptPriority.Skill;
            utilitySkillDef.isBullets = false;
            utilitySkillDef.isCombatSkill = true;
            utilitySkillDef.mustKeyPress = false;
            utilitySkillDef.noSprint = false;
            utilitySkillDef.rechargeStock = 1;
            utilitySkillDef.requiredStock = 1;
            utilitySkillDef.shootDelay = 0.08f;
            utilitySkillDef.stockToConsume = 1;
            utilitySkillDef.skillDescriptionToken = "AAAAAAAAAAAAAA";
            utilitySkillDef.skillName = "aaa";
            utilitySkillDef.skillNameToken = "AAAAAAAAAAAAAAAAAAA";
            utilitySkillDef.icon = null;
            utilitySkillDef.keywordTokens = Array.Empty<string>();

            LoadoutAPI.AddSkillDef(utilitySkillDef);
            SkillFamily utilitySkillFamily = skillLocator.utility.skillFamily;

            utilitySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = utilitySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(utilitySkillDef.skillNameToken, false, null)
            };
        }

        private void CreateSpecial()
        {
            LoadoutAPI.AddSkill(typeof(Drone));

            SkillDef specialSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            specialSkillDef.activationState = new SerializableEntityStateType(typeof(Drone));
            specialSkillDef.activationStateMachineName = "Weapon";
            specialSkillDef.baseMaxStock = 10;
            specialSkillDef.baseRechargeInterval = 0;
            specialSkillDef.beginSkillCooldownOnSkillEnd = true;
            specialSkillDef.canceledFromSprinting = false;
            specialSkillDef.fullRestockOnAssign = true;
            specialSkillDef.interruptPriority = InterruptPriority.Skill;
            specialSkillDef.isBullets = false;
            specialSkillDef.isCombatSkill = true;
            specialSkillDef.mustKeyPress = true;
            specialSkillDef.noSprint = false;
            specialSkillDef.rechargeStock = 0;
            specialSkillDef.requiredStock = 1;
            specialSkillDef.shootDelay = 0.5f;
            specialSkillDef.stockToConsume = 1;
            specialSkillDef.skillDescriptionToken = "AAAAAAAAAAAAAAAAAAA";
            specialSkillDef.skillName = "aaa";
            specialSkillDef.skillNameToken = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            specialSkillDef.icon = null; 
            specialSkillDef.keywordTokens = Array.Empty<string>(); 

            LoadoutAPI.AddSkillDef(specialSkillDef);
            SkillFamily specialSkillFamily = skillLocator.special.skillFamily;

            specialSkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = specialSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(specialSkillDef.skillNameToken, false, null)
            };
        }

        protected void AI() {
            var masterComp = master.GetComponent<CharacterMaster>();

            masterComp.bodyPrefab = Resources.Load<GameObject>("prefabs/characterbodies/AncientWispBody");
            
            API.DestroySkillDrivers(master);

            var follow = master.AddComponent<AISkillDriver>();

            var strafe = master.AddComponent<AISkillDriver>();

            var primary = master.AddComponent<AISkillDriver>();

            primary.skillSlot = SkillSlot.Primary;
            primary.requireSkillReady = true;
            primary.requireEquipmentReady = false;
            primary.moveTargetType = TargetType.CurrentEnemy;
            primary.minUserHealthFraction = float.NegativeInfinity;
            primary.maxUserHealthFraction = float.PositiveInfinity;
            primary.minTargetHealthFraction = float.NegativeInfinity;
            primary.maxTargetHealthFraction = float.PositiveInfinity;
            primary.minDistance = 0;
            primary.maxDistance = 100;
            primary.selectionRequiresTargetLoS = false;
            primary.activationRequiresTargetLoS = false;
            primary.activationRequiresAimConfirmation = false;
            primary.movementType = MovementType.ChaseMoveTarget;
            primary.moveInputScale = 1;
            primary.aimType = AimType.AtMoveTarget;
            primary.ignoreNodeGraph = false;
            primary.driverUpdateTimerOverride = -1;
            primary.resetCurrentEnemyOnNextDriverSelection = false;
            primary.noRepeat = false;
            primary.shouldSprint = true;
            primary.shouldFireEquipment = false;
            primary.buttonPressType = ButtonPressType.Hold;

            follow.skillSlot = SkillSlot.None;
            follow.requireSkillReady = false;
            follow.requireEquipmentReady = false;
            follow.minUserHealthFraction = float.NegativeInfinity;
            follow.maxUserHealthFraction = float.PositiveInfinity;
            follow.minTargetHealthFraction = float.NegativeInfinity;
            follow.maxTargetHealthFraction = float.PositiveInfinity;
            follow.minDistance = 0;
            follow.maxDistance = float.PositiveInfinity;
            follow.selectionRequiresTargetLoS = false;
            follow.selectionRequiresOnGround = false;
            follow.moveTargetType = TargetType.CurrentEnemy;
            follow.activationRequiresTargetLoS = false;
            follow.activationRequiresAimConfirmation = false;
            follow.movementType = MovementType.ChaseMoveTarget;
            follow.moveInputScale = 1;
            follow.aimType = AimType.AtMoveTarget;
            follow.ignoreNodeGraph = false;
            follow.shouldSprint = false;
            follow.shouldFireEquipment = false;
            //follow.shouldTapButton = false;
            follow.buttonPressType = ButtonPressType.Hold;
            follow.driverUpdateTimerOverride = -1;
            follow.resetCurrentEnemyOnNextDriverSelection = false;
            follow.noRepeat = false;

            strafe.customName = "StrafeTarget";
            strafe.skillSlot = SkillSlot.None;
            strafe.requireSkillReady = false;
            strafe.requireEquipmentReady = false;
            strafe.minUserHealthFraction = float.NegativeInfinity;
            strafe.maxUserHealthFraction = float.PositiveInfinity;
            strafe.minTargetHealthFraction = float.NegativeInfinity;
            strafe.maxTargetHealthFraction = float.PositiveInfinity;
            strafe.minDistance = 0;
            strafe.maxDistance = float.PositiveInfinity;
            strafe.selectionRequiresTargetLoS = false;
            strafe.selectionRequiresOnGround = false;
            strafe.moveTargetType = TargetType.CurrentEnemy;
            strafe.activationRequiresTargetLoS = false;
            strafe.activationRequiresAimConfirmation = false;
            strafe.movementType = MovementType.StrafeMovetarget;
            strafe.moveInputScale = 1;
            strafe.aimType = AimType.AtMoveTarget;
            strafe.ignoreNodeGraph = false;
            strafe.shouldSprint = false;
            strafe.shouldFireEquipment = false;
            //strafe.shouldTapButton = false;
            strafe.buttonPressType = ButtonPressType.Hold;
            strafe.driverUpdateTimerOverride = -1;
            strafe.resetCurrentEnemyOnNextDriverSelection = false;
            strafe.noRepeat = false;
        }

        protected void FixSwingEffect() {
            var swingEffect = Resources.Load<GameObject>("prefabs/effects/AncientWispSwingTrail");
            var swing = swingEffect.transform.Find("SwingTrail");
            var renderer = swing.GetComponent<Renderer>();

            renderer.material = Resources.Load<GameObject>("prefabs/projectileghosts/AncientWispCannonGhost").transform.Find("Particles/FireSphere").GetComponent<Renderer>().material;
            //Resources.Load<Material>("materials/matGrandparentMeleeSwipe");
        }
    }
}
