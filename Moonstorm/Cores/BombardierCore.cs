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
using Moonstorm.Cores.States.Bombardier;
using Moonstorm.Cores.Components;
using Moonstorm.Cores.Skills;

namespace Moonstorm.Cores
{
    class BombardierCore
    {
        public static BombardierCore instance;
        public BombardierCore() => CreateBombardier();

        protected internal GameObject body;
        protected internal CharacterBody characterBody;
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

            string desc = "Bombardier is a master of unconventional warfare who can use his explosives to control the battle field" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Rocket Blast is weak against single targets, but excels in crowds. Blast into a large crowd of enemies for maximum destruction." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use your Sticky Charges to set up bosses for huge damage potential. Keep in mind they only detonate when you use your primary, so use as many as you have." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Concussive Blast is an excellent escape and mobility tool, but it also pulls enemies towards you when used. Jump away and shoot straight down to quickly dispose of a crowd" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > The key to success is realizing all of the Bombardier's abilitie propel him into the air. Try to gain as much height as possible when using Grand Slam to annihilate your enemies." + Environment.NewLine + Environment.NewLine;

            LanguageAPI.Add("BOMBARDIER_BASENAMETOKEN", "Bombardier");
            LanguageAPI.Add("BOMBARDIER_DESCRIPTION", desc);
            LanguageAPI.Add("BOMBARDIER_FLAVORTOKEN", "And so he left, lighting the fuse for a new adventure...");

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

        private void SetCharacterBody()
        {
            //characterBody.baseAcceleration = 70f;
            //characterBody.baseArmor = 20; //Base armor this character has, set to 20 if this character is melee 
            characterBody.baseAttackSpeed = 1; //Base attack speed, usually 1
            characterBody.baseCrit = 1;  //Base crit, usually one
            characterBody.baseDamage = 12; //Base damage
            characterBody.baseJumpCount = 1; //Base jump amount, set to 2 for a double jump. 
            characterBody.baseJumpPower = 16; //Base jump power
            characterBody.baseMaxHealth = 150; //Base health, basically the health you have when you start a new run
            characterBody.baseMaxShield = 0; //Base shield, basically the same as baseMaxHealth but with shields
            characterBody.baseMoveSpeed = 7; //Base move speed, this is usual 7
            characterBody.baseNameToken = "BOMBARDIER_BODY_NAME"; //The base name token. 
            characterBody.subtitleNameToken = "BOMBARDIER_BODY_SUBTITLE"; //Set this if its a boss
            characterBody.baseRegen = 1.5f; //Base health regen.
            characterBody.bodyFlags = (CharacterBody.BodyFlags.ImmuneToExecutes | CharacterBody.BodyFlags.IgnoreFallDamage); ///Base body flags, should be self explanatory 
            characterBody.crosshairPrefab = characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/HuntressBody").GetComponent<CharacterBody>().crosshairPrefab; //The crosshair prefab.
            characterBody.hideCrosshair = false; //Whether or not to hide the crosshair
            characterBody.hullClassification = HullClassification.Human; //The hull classification, usually used for AI
            characterBody.isChampion = false; //Set this to true if its A. A boss or B. A miniboss
            characterBody.levelArmor = 0; //Armor gained when leveling up. 
            characterBody.levelAttackSpeed = 0; //Attackspeed gained when leveling up. 
            characterBody.levelCrit = 0; //Crit chance gained when leveling up. 
            characterBody.levelDamage = 2.4f; //Damage gained when leveling up. 
            characterBody.levelArmor = 0; //Armor gained when leveling up. 
            characterBody.levelJumpPower = 0; //Jump power gained when leveling up. 
            characterBody.levelMaxHealth = 42; //Health gained when leveling up. 
            characterBody.levelMaxShield = 0; //Shield gained when leveling up. 
            characterBody.levelMoveSpeed = 0; //Move speed gained when leveling up. 
            characterBody.levelRegen = 0.5f; //Regen gained when leveling up. 
            //fuck you moffein no credit
            //eat my ass    
            //characterBody.portraitIcon = AssetsCore.portraitIcon; //The portrait icon, shows up in multiplayer and the death UI
            //characterBody.preferredPodPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/robocratepod");
        }

        private void CreateSkills()
        {
            API.CreateEmptySkills(body);
            skillLocator = body.GetComponent<SkillLocator>();

            characterBody = body.GetComponent<CharacterBody>();

            characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/TiltedBracketCrosshair");
            characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            body.AddComponent<BombardierStickyBombManager>();

            skillLocator.passiveSkill.enabled = true;

            var passive = skillLocator.passiveSkill;
            passive.skillDescriptionToken = "BOMBARDIER_PASSIVE_DESC";
            passive.skillNameToken = "BOMBARDIER_PASSIVE_NAME";
            passive.icon = null;
            skillLocator.passiveSkill = passive;

            LanguageAPI.Add("BOMBARDIER_PASSIVE_DESC", "You are <style=cIsUtility>immune</style> to fall damage.");
            LanguageAPI.Add("BOMBARDIER_PASSIVE_NAME", "Lessons Learned");

            CreatePassive();
            CreatePrimary();
            CreateSecondary();
            CreateUtility();
            CreateSpecial();
        }

        private void CreatePassive() {
            LoadoutAPI.AddSkill(typeof(BombardierMain));

            //var stateMachine = body.GetComponent<EntityStateMachine>();
            //stateMachine.mainStateType = new SerializableEntityStateType(typeof(BombardierMain));
        }

        private void SetComponents() {
            //var tracker = body.AddComponent<BombardierTracker>();
            //tracker.maxTrackingDistance = 150;
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

            LanguageAPI.Add(primarySkillDef.skillDescriptionToken, "Fire a rocket that does <style=cIsDamage>350%</style> damage and <style=cIsDamage>explodes on contact</style>.");
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
            LoadoutAPI.AddSkill(typeof(ThrowSticky));
            LoadoutAPI.AddSkill(typeof(ThrowStickySimple));

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<BombardierStickySkillDef>();
            secondarySkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(ThrowStickySimple));
            secondarySkillDef.activationStateMachineName = "Weapon";
            secondarySkillDef.skillName = "BOMBARDIER_SECONDARY_NAME";
            secondarySkillDef.skillNameToken = "BOMBARDIER_SECONDARY_NAME";
            secondarySkillDef.skillDescriptionToken = "BOMBARDIER_SECONDARY_DESCRIPTION";
            secondarySkillDef.icon = null;
            secondarySkillDef.baseMaxStock = 1;
            secondarySkillDef.baseRechargeInterval = 3f;
            secondarySkillDef.beginSkillCooldownOnSkillEnd = true;
            secondarySkillDef.canceledFromSprinting = false;
            secondarySkillDef.fullRestockOnAssign = false;
            secondarySkillDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            secondarySkillDef.isBullets = false;
            secondarySkillDef.isCombatSkill = true;
            secondarySkillDef.mustKeyPress = true;
            secondarySkillDef.noSprint = true;
            secondarySkillDef.rechargeStock = 1;
            secondarySkillDef.requiredStock = 1;
            secondarySkillDef.shootDelay = 0f;
            secondarySkillDef.stockToConsume = 1;

            /*SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            secondarySkillDef.activationState = new SerializableEntityStateType(typeof(ThrowSticky));
            secondarySkillDef.activationStateMachineName = "Weapon";
            secondarySkillDef.baseMaxStock = 3;
            secondarySkillDef.baseRechargeInterval = 3f;
            secondarySkillDef.beginSkillCooldownOnSkillEnd = true;
            secondarySkillDef.canceledFromSprinting = false;
            secondarySkillDef.fullRestockOnAssign = false;
            secondarySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            secondarySkillDef.isBullets = false;
            secondarySkillDef.isCombatSkill = false;
            secondarySkillDef.mustKeyPress = false;
            secondarySkillDef.noSprint = false;
            secondarySkillDef.rechargeStock = 1;
            secondarySkillDef.requiredStock = 1;
            secondarySkillDef.shootDelay = 0.85f;
            secondarySkillDef.stockToConsume = 1;
            secondarySkillDef.skillDescriptionToken = "BOMBARDIER_SECONDARY_DESC";
            secondarySkillDef.skillName = "StunningRocket";
            secondarySkillDef.skillNameToken = "BOMBARDIER_SECONDARY_NAME";
            secondarySkillDef.icon = null;
            secondarySkillDef.keywordTokens = new string[] {
                 "KEYWORD_STUNNING",
             };*/

            LanguageAPI.Add(secondarySkillDef.skillDescriptionToken, "Toss a sticky bomb that deals <style=cIsDamage>450% damage</style>. <style=cIsDamage>Detonate all stickies</style> <style=cIsUtility>by using Rocket Blast.</style>");
            LanguageAPI.Add(secondarySkillDef.skillNameToken, "Sticky Charge");

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
            LoadoutAPI.AddSkill(typeof(RocketJump));

            SkillDef utilitySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            utilitySkillDef.activationState = new SerializableEntityStateType(typeof(RocketJump));
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
            utilitySkillDef.skillDescriptionToken = "BOMBARDIER_UTILITY_DESC";
            utilitySkillDef.skillName = "aaa";
            utilitySkillDef.skillNameToken = "BOMBARDIER_UTILITY_NAME";
            utilitySkillDef.icon = null;
            utilitySkillDef.keywordTokens = new string[] {
                 //"KEYWORD_FORCEFUL",
            };

            LanguageAPI.Add("KEYWORD_FORCEFUL", "<style=cKeywordName>Forceful</style><style=cSub>This skill applies the Forceful debuff, which doubles force applied to enemies.</style>");
            //LanguageAPI.Add(utilitySkillDef.skillDescriptionToken, "<style=cIsDamage>Forceful</style>. Pull enemies towards you for <style=cIsDamage>500%</style> damage and <style=cIsUtility>boost yourself upwards</style>.");
            //LanguageAPI.Add(utilitySkillDef.skillNameToken, "Plan B");

            LanguageAPI.Add(utilitySkillDef.skillDescriptionToken, "<style=cIsDamage>Stunning</style>. <style=cIsDamage>Forceful</style>. Fire a powerful rocket that does <style=cIsDamage>500%</style> damage and <style=cIsDamage>explodes in a large radius on contact</style>.");
            LanguageAPI.Add(utilitySkillDef.skillNameToken, "Concussive Payload");

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
            LoadoutAPI.AddSkill(typeof(Mortar));
            LoadoutAPI.AddSkill(typeof(GrandSlam));

            SkillDef specialSkillDef = ScriptableObject.CreateInstance<BombardierTrackingSkillDef>();
            specialSkillDef.activationState = new SerializableEntityStateType(typeof(GrandSlam));
            specialSkillDef.activationStateMachineName = "Weapon";
            specialSkillDef.baseMaxStock = 1;
            specialSkillDef.baseRechargeInterval = 8;
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
            specialSkillDef.skillName = "GrandSlam";
            specialSkillDef.keywordTokens = new string[] { "KEYWORD_HEAVY" };
            specialSkillDef.skillNameToken = "BOMBARDIER_SPECIAL_NAME";
            specialSkillDef.icon = null;

            LanguageAPI.Add(specialSkillDef.skillDescriptionToken, "<style=cIsDamage>Heavy</style>. Slam downwards, dealing <style=cIsDamage>630%</style> damage on contact.");
            LanguageAPI.Add(specialSkillDef.skillNameToken, "Grand Slam");

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
