using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using R2API;
using RoR2.Skills;
using EntityStates;
using RoR2;
using Cloudburst.Cores.States.Bombardier;

namespace Cloudburst.Cores
{
    class BombardierCore
    {
        public static BombardierCore instance;
        public BombardierCore() => CreateBombardier();

        protected internal GameObject body;
        protected internal SkillLocator skillLocator;
        protected void CreateBombardier() {
            instance = this;

            LogCore.LogI("Initializing Core: " + base.ToString());

            CreateBody();
            CreateSkills();
            AddToSurvivorSelect();
        }

        protected void CreateBody() {
            body = Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody").InstantiateClone("BombardierBody", true);
            API.RegisterNewBody(body);
        }

        protected void AddToSurvivorSelect() {
            GameObject display = body.GetComponent<ModelLocator>().modelTransform.gameObject;

            string desc = "Bombardier loves bombs. Hell yeah!" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sample Text 1" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sample Text 2" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sample Text 3" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sample Text 4" + Environment.NewLine + Environment.NewLine;

            LanguageAPI.Add("BOMBARDIER_BASENAMETOKEN", "Bombardier");
            LanguageAPI.Add("BOMBARDIER_DESCRIPTION", desc);
            LanguageAPI.Add("BOMBARDIER_FLAVORTOKEN", "And so he left, with a new bomb for a new adventure...");

            SurvivorDef def = new SurvivorDef() {
                bodyPrefab = body,
                descriptionToken = "BOMBARDIER_DESCRIPTION",
                displayNameToken = "BOMBARDIER_BASENAMETOKEN",
                //nigga stop smoking and work
                displayPrefab = display,
                name = "Bombardier",
                outroFlavorToken = "BOMBARDIER_FLAVORTOKEN",
                //primaryColor = (Color)new Color32(),
                unlockableName = ""
            };
            SurvivorAPI.AddSurvivor(def);
        }

        private void CreateSkills()
        {
            API.CreateEmptySkills(body);
            skillLocator = body.GetComponent<SkillLocator>();

            CreatePrimary();
            CreateSecondary();
            CreateUtility();
            CreateSpecial();
        }

        private void CreatePrimary()
        {
            LoadoutAPI.AddSkill(typeof(FireRocket));

            SkillDef primarySkillDef = ScriptableObject.CreateInstance<SkillDef>();

            primarySkillDef.activationState = new SerializableEntityStateType(typeof(FireRocket));
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
            primarySkillDef.skillDescriptionToken = "";
            primarySkillDef.skillName = "";
            primarySkillDef.skillNameToken = "";
            primarySkillDef.icon = AssetsCore.hurtIcon;
            primarySkillDef.keywordTokens = new string[] {
                 "KEYWORD_AGILE",
                 "KEYWORD_STUNNING",
                 "KEYWORD_CLOCKED"
             };

            LoadoutAPI.AddSkillDef(primarySkillDef);
            SkillFamily primarySkillFamily = skillLocator.primary.skillFamily;

            primarySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = primarySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(primarySkillDef.skillNameToken, false, null)

            };
        }

        private void CreateSecondary() {
            LoadoutAPI.AddSkill(typeof(FireFlameRocket));

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            secondarySkillDef.activationState = new SerializableEntityStateType(typeof(FireFlameRocket));
            secondarySkillDef.activationStateMachineName = "Weapon";
            secondarySkillDef.baseMaxStock = 1;
            secondarySkillDef.baseRechargeInterval = 4f;
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
            secondarySkillDef.skillDescriptionToken = "";
            secondarySkillDef.skillName = "aaa";
            secondarySkillDef.skillNameToken = "";
            secondarySkillDef.icon = AssetsCore.forcedReassemblyIcon;
            secondarySkillDef.keywordTokens = new string[] {
                 "KEYWORD_AGILE",
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
            LoadoutAPI.AddSkill(typeof(UpwardsBlast));

            SkillDef utilitySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            utilitySkillDef.activationState = new SerializableEntityStateType(typeof(UpwardsBlast));
            utilitySkillDef.activationStateMachineName = "Weapon";
            utilitySkillDef.baseMaxStock = 1;
            utilitySkillDef.baseRechargeInterval = 6f;
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
            utilitySkillDef.skillDescriptionToken = "";
            utilitySkillDef.skillName = "aaa";
            utilitySkillDef.skillNameToken = "";
            utilitySkillDef.icon = AssetsCore.forcedReassemblyIcon;
            utilitySkillDef.keywordTokens = new string[] {
                 "KEYWORD_STUNNING",
             };

            LoadoutAPI.AddSkillDef(utilitySkillDef);
            SkillFamily utilitySkillFamily = skillLocator.utility.skillFamily;

            utilitySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = utilitySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(utilitySkillDef.skillNameToken, false, null)
            };
        }

        private void CreateSpecial() {
            SkillDef specialSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            specialSkillDef.activationState = new SerializableEntityStateType(typeof(FireRocket));
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
            specialSkillDef.skillDescriptionToken = "HAND_SPECIAL_DESCRIPTION";
            specialSkillDef.skillName = "aaa";
            specialSkillDef.skillNameToken = "HAND_SPECIAL_NAME";
            specialSkillDef.icon = AssetsCore.droneIcon;

            LoadoutAPI.AddSkillDef(specialSkillDef);
            SkillFamily specialSkillFamily = skillLocator.special.skillFamily;

            specialSkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = specialSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(specialSkillDef.skillNameToken, false, null)
            };
        }
    }
}
