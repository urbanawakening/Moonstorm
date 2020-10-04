using System;
using System.Collections.Generic;
using BepInEx;
using EntityStates;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using Starstorm2.Cores.States.Nemmando;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores
{
    public class NemmandoCore
    {
        public static NemmandoCore instance;
        public static GameObject characterPrefab;

        public NemmandoCore() => Load();

        public void Load()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            if (instance == null)
            {
                instance = this;
            }
            CreateNEMMANDO();
        }

        public void CreateNEMMANDO()
        {
            InitPrefab();
            RegisterCharacter();
            SkillSetup();
        }

        void InitPrefab()
        {
            characterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/MercBody"), "NEMMANDOBody");
        }

        void RegisterCharacter()
        {
            GameObject NEMMANDODisplay = PrefabAPI.InstantiateClone(characterPrefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "NemmandoDisplay", true);
            NEMMANDODisplay.AddComponent<NetworkIdentity>();

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(characterPrefab);
            };

            CharacterBody component = characterPrefab.GetComponent<CharacterBody>();
            component.baseDamage = 10f;
            component.baseCrit = 1f;
            component.levelCrit = 0f;
            component.baseMaxHealth = 400f;
            component.levelMaxHealth = 40f;
            component.baseArmor = 20f;
            component.baseRegen = 2f;
            component.levelRegen = 0.2f;
            component.baseMoveSpeed = 8f;
            component.levelMoveSpeed = 0.25f;
            component.baseAttackSpeed = 1f;
            component.name = "Nemesis Commando";
            component.subtitleNameToken = "Nemesis Commando";

            characterPrefab.GetComponent<CharacterBody>().preferredPodPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/toolbotbody").GetComponent<CharacterBody>().preferredPodPrefab;
            LanguageAPI.Add("");

            LanguageAPI.Add("NEMMANDO_OUTRO_FLAVOR", "...and so he left.");

            var mySurvivorDef = new SurvivorDef
            {
                bodyPrefab = characterPrefab,
                descriptionToken = "NEMMANDO_DESCRIPTION",
                displayPrefab = NEMMANDODisplay,
                primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                name = "NEMMANDO",
                outroFlavorToken = "NEMMANDO_OUTRO_FLAVOR",
                unlockableName = "",
            };

            SurvivorAPI.AddSurvivor(mySurvivorDef);
        }

        void SkillSetup()
        {
            // get rid of the original skills first, otherwise we'll have commando's loadout and we don't want that
            //API.CreateEmptySkills(characterPrefab);
            foreach (GenericSkill obj in characterPrefab.GetComponentsInChildren<GenericSkill>())
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }

            SkillLocator skillLocator = characterPrefab.GetComponent<SkillLocator>();
            skillLocator.passiveSkill.enabled = false;

            PrimarySetup(skillLocator);
            SecondarySetup(skillLocator);
            UtilitySetup(skillLocator);
            SpecialSetup(skillLocator);
        }

        private void PrimarySetup(SkillLocator skillLocator)
        {
            LanguageAPI.Add("NEMMANDO_PRIMARY_SKILLNAME_NAME", "");
            LanguageAPI.Add("NEMMANDO_PRIMARY_SKILLNAME_DESCRIPTION", @"");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(FireScorch));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0.1f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("");
            mySkillDef.skillDescriptionToken = "NEMMANDO_PRIMARY_SKILLNAME_DESCRIPTION";
            mySkillDef.skillName = "NEMMANDO_PRIMARY_SKILLNAME_NAME";
            mySkillDef.skillNameToken = "NEMMANDO_PRIMARY_SKILLNAME_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.primary = characterPrefab.AddComponent<GenericSkill>();

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);

            skillLocator.primary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamilyVariants = skillLocator.primary.skillFamily;

            skillFamilyVariants.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void SecondarySetup(SkillLocator skillLocator)
        {
            LanguageAPI.Add("NEMMANDO_SECONDARY_QUARANTINE_NAME", "");
            LanguageAPI.Add("NEMMANDO_SECONDARY_QUARANTINE_DESCRIPTION", "");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(FireSuppresiveFire));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 3f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("");
            mySkillDef.skillDescriptionToken = "NEMMANDO_SECONDARY_QUARANTINE_DESCRIPTION";
            mySkillDef.skillName = "NEMMANDO_SECONDARY_QUARANTINE_NAME";
            mySkillDef.skillNameToken = "NEMMANDO_SECONDARY_QUARANTINE_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.secondary = characterPrefab.AddComponent<GenericSkill>();

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);

            skillLocator.secondary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamilyVariants = skillLocator.secondary.skillFamily;

            skillFamilyVariants.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void UtilitySetup(SkillLocator skillLocator)
        {
            LanguageAPI.Add("NEMMANDO_UTILITY_FISSION_NAME", "");
            LanguageAPI.Add("NEMMANDO_UTILITY_FISSION_DESCRIPTION", "");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(FirePlanB));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            //mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("");
            mySkillDef.skillDescriptionToken = "NEMMANDO_UTILITY_FISSION_DESCRIPTION";
            mySkillDef.skillName = "NEMMANDO_UTILITY_FISSION_NAME";
            mySkillDef.skillNameToken = "NEMMANDO_UTILITY_FISSION_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.utility = characterPrefab.AddComponent<GenericSkill>();

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);

            skillLocator.utility.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamilyVariants = skillLocator.utility.skillFamily;

            skillFamilyVariants.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void SpecialSetup(SkillLocator skillLocator)
        {
            LanguageAPI.Add("NEMMANDO_SPECIAL_SURGE_NAME", "");
            LanguageAPI.Add("NEMMANDO_SPECIAL_SURGE_DESCRIPTION", "");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(FireBlazeborne));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 13f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("");
            mySkillDef.skillDescriptionToken = "NEMMANDO_SPECIAL_SURGE_DESCRIPTION";
            mySkillDef.skillName = "NEMMANDO_SPECIAL_SURGE_NAME";
            mySkillDef.skillNameToken = "NEMMANDO_SPECIAL_SURGE_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.special = characterPrefab.AddComponent<GenericSkill>();

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);

            skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamilyVariants = skillLocator.special.skillFamily;

            skillFamilyVariants.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }
    }
}