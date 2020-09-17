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
using Cloudburst.Cores.Components;
using Cloudburst.Cores.Skills;

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
            SetComponents();
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
                primaryColor = new Color(253f / 255f, 118f / 255f, 26f / 255f), 
                unlockableName = ""
            };
            SurvivorAPI.AddSurvivor(def);
        }

        private void CreateSkills()
        {
            API.CreateEmptySkills(body);
            skillLocator = body.GetComponent<SkillLocator>();

            skillLocator.passiveSkill.enabled = true;

            var passive = skillLocator.passiveSkill;
            passive.skillDescriptionToken = "BOMBARDIER_PASSIVE_DESC";
            passive.skillNameToken = "BOMBARDIER_PASSIVE_NAME";
            passive.icon = null;
            skillLocator.passiveSkill = passive;

            LanguageAPI.Add("BOMBARDIER_PASSIVE_DESC", "You are <style=cIsUtility>immune</style> to fall damage.");
            LanguageAPI.Add("BOMBARDIER_PASSIVE_NAME", "Lessons Learned");


            CreatePrimary();
            CreateSecondary();
            CreateUtility();
            CreateSpecial();
        }

        private void SetComponents() {
            var tracker = body.AddComponent<BombardierTracker>();
            tracker.maxTrackingDistance = 150;
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
            primarySkillDef.skillDescriptionToken = "BOMBARDIER_PRIMARY_DESC";
            primarySkillDef.skillName = "Rocket";
            primarySkillDef.skillNameToken = "BOMBARDIER_PRIMARY_NAME";
            primarySkillDef.icon = null;
            primarySkillDef.keywordTokens = Array.Empty<String>();

            LanguageAPI.Add(primarySkillDef.skillDescriptionToken, "Fire a rocket that does <style=cIsDamage>300%</style> damage and <style=cIsDamage>explodes on contact</style>.");
            LanguageAPI.Add(primarySkillDef.skillNameToken, "Rocket Blast");

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
            secondarySkillDef.shootDelay = 0.05f;
            secondarySkillDef.stockToConsume = 1;
            secondarySkillDef.skillDescriptionToken = "BOMBARDIER_SECONDARY_DESC";
            secondarySkillDef.skillName = "StunningRocket";
            secondarySkillDef.skillNameToken = "BOMBARDIER_SECONDARY_NAME";
            secondarySkillDef.icon = null;
            secondarySkillDef.keywordTokens = new string[] {
                 "KEYWORD_STUNNING",
             };

            LanguageAPI.Add(secondarySkillDef.skillDescriptionToken, "<style=cIsDamage>Stunning</style>. Fire a stunning rocket that does <style=cIsDamage>150%</style> damage and <style=cIsDamage>explodes in a large radius on contact</style>.");
            LanguageAPI.Add(secondarySkillDef.skillNameToken, "Concussive Payload");

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
            utilitySkillDef.skillDescriptionToken = "BOMBARDIER_UTILITY_DESC";
            utilitySkillDef.skillName = "aaa";
            utilitySkillDef.skillNameToken = "BOMBARDIER_UTILITY_NAME";
            utilitySkillDef.icon = null;
            utilitySkillDef.keywordTokens = new string[] {
                 "KEYWORD_FORCEFUL",
            };

            LanguageAPI.Add("KEYWORD_FORCEFUL", "<style=cKeywordName>Forceful</style><style=cSub>This skill applies the Forceful debuff, which doubles force applied to enemies.</style>");
            LanguageAPI.Add(utilitySkillDef.skillDescriptionToken, "<style=cIsDamage>Forceful</style>. Pull enemies towards you for <style=cIsDamage>400%</style> damage and <style=cIsUtility>boost yourself upwards</style>.");
            LanguageAPI.Add(utilitySkillDef.skillNameToken, "Plan B");

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
            SkillDef specialSkillDef = ScriptableObject.CreateInstance<BombardierTrackingSkillDef>();
            specialSkillDef.activationState = new SerializableEntityStateType(typeof(Mortar));
            specialSkillDef.activationStateMachineName = "Weapon";
            specialSkillDef.baseMaxStock = 1;
            specialSkillDef.baseRechargeInterval = 12;
            specialSkillDef.beginSkillCooldownOnSkillEnd = true;
            specialSkillDef.canceledFromSprinting = false;
            specialSkillDef.fullRestockOnAssign = true;
            specialSkillDef.interruptPriority = InterruptPriority.Skill;
            specialSkillDef.isBullets = false;
            specialSkillDef.isCombatSkill = true;
            specialSkillDef.mustKeyPress = true;
            specialSkillDef.noSprint = false;
            specialSkillDef.rechargeStock = 1;
            specialSkillDef.requiredStock = 1;
            specialSkillDef.shootDelay = 0.5f;
            specialSkillDef.stockToConsume = 1;
            specialSkillDef.skillDescriptionToken = "BOMBARDIER_SPECIAL_DESC";
            specialSkillDef.skillName = "Seekers";
            specialSkillDef.skillNameToken = "BOMBARDIER_SPECIAL_NAME";
            specialSkillDef.icon = null;

            LanguageAPI.Add(specialSkillDef.skillDescriptionToken, "Fire nine seeking rockets that each deal <style=cIsDamage>600%</style> damage.");
            LanguageAPI.Add(specialSkillDef.skillNameToken, "Seeker Barrage");

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
