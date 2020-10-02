/*using System;
using System.Collections.Generic;
using BepInEx;
using EntityStates;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using Starstorm2.Cores.States.Nucleator;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores
{
    public class NucleatorCore
    {
        public static NucleatorCore instance;
        public static GameObject characterPrefab;
        public static BuffIndex radionuclideSurgeBuffIndex;

        public NucleatorCore() => Load();

        public void Load()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            if (instance == null)
            {
                instance = this;
            }
            CreateNucleator();
        }

        public void CreateNucleator()
        {
            InitPrefab();
            BuffSetup();
            RegisterCharacter();
            SkillSetup();
        }

        void InitPrefab()
        {
            characterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/MercBody"), "NucleatorBody");
        }

        void RegisterCharacter()
        {
            GameObject nucleatorDisplay = PrefabAPI.InstantiateClone(characterPrefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "NucleatorDisplay", true);
            nucleatorDisplay.AddComponent<NetworkIdentity>();

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
            component.name = "Nucleator";
            component.subtitleNameToken = "Nucleator";

            characterPrefab.GetComponent<CharacterBody>().preferredPodPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/toolbotbody").GetComponent<CharacterBody>().preferredPodPrefab;
            LanguageAPI.Add("NUCLEATOR_DESCRIPTION"
            , @"The Nucleator is a radioactive juggernaut with rad-proof armor,
    which allows him to manipulate nuclear components for long periods of time.
    He takes advantage of radiation; being able to inflict it to enemies for the
    strongest contamination effect. Nucleator can charge his skills for maximum output,
    however, be careful as overcharging them may lead to self-harm!");

            LanguageAPI.Add("NUCLEATOR_OUTRO_FLAVOR", "...and so he left, health status undisclosed.");

            var mySurvivorDef = new SurvivorDef
            {
                bodyPrefab = characterPrefab,
                descriptionToken = "NUCLEATOR_DESCRIPTION",
                displayPrefab = nucleatorDisplay,
                primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                name = "Nucleator",
                outroFlavorToken = "NUCLEATOR_OUTRO_FLAVOR",
                unlockableName = "",
            };

            SurvivorAPI.AddSurvivor(mySurvivorDef);
        }

        void BuffSetup()
        {
            NucleatorCore.radionuclideSurgeBuffIndex = BuffAPI.Add(new CustomBuff(new BuffDef()
            {
                name = "RadionucliteSurge",
                iconPath = "",
                buffColor = new Color(0, 130, 0),
                canStack = false,
                isDebuff = false
            }));
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
            LanguageAPI.Add("NUCLEATOR_PRIMARY_SKILLNAME_NAME", "Irradiate");
            LanguageAPI.Add("NUCLEATOR_PRIMARY_SKILLNAME_DESCRIPTION", @"Charge and fire a bullet for up to 500% damage, 900% on overcharge.
The bullet's damage increases the farther the it travels. ");
            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ChargeIrradiate));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
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
            mySkillDef.skillDescriptionToken = "NUCLEATOR_PRIMARY_SKILLNAME_DESCRIPTION";
            mySkillDef.skillName = "NUCLEATOR_PRIMARY_SKILLNAME_NAME";
            mySkillDef.skillNameToken = "NUCLEATOR_PRIMARY_SKILLNAME_NAME";

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
            LanguageAPI.Add("NUCLEATOR_SECONDARY_QUARANTINE_NAME", "Quarantine");
            LanguageAPI.Add("NUCLEATOR_SECONDARY_QUARANTINE_DESCRIPTION", "Push enemies in front of you for 300% piercing damage.");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ChargeQuarantine));
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
            mySkillDef.skillDescriptionToken = "NUCLEATOR_SECONDARY_QUARANTINE_DESCRIPTION";
            mySkillDef.skillName = "NUCLEATOR_SECONDARY_QUARANTINE_NAME";
            mySkillDef.skillNameToken = "NUCLEATOR_SECONDARY_QUARANTINE_NAME";

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
            LanguageAPI.Add("NUCLEATOR_UTILITY_FISSION_NAME", "Fission Impulse");
            LanguageAPI.Add("NUCLEATOR_UTILITY_FISSION_DESCRIPTION", "Hold to launch yourself into any direction dealing 550% damage.");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ChargeFissionImpulse));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("");
            mySkillDef.skillDescriptionToken = "NUCLEATOR_UTILITY_FISSION_DESCRIPTION";
            mySkillDef.skillName = "NUCLEATOR_UTILITY_FISSION_NAME";
            mySkillDef.skillNameToken = "NUCLEATOR_UTILITY_FISSION_NAME";

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
            LanguageAPI.Add("NUCLEATOR_SPECIAL_SURGE_NAME", "Radionuclide Surge");
            LanguageAPI.Add("NUCLEATOR_SPECIAL_SURGE_DESCRIPTION", "Enter a nuclear state for 6 seconds, adding radiation to every attack while becoming invulnerable to overcharging skills.");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ApplyRadionuclideSurge));
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
            mySkillDef.skillDescriptionToken = "NUCLEATOR_SPECIAL_SURGE_DESCRIPTION";
            mySkillDef.skillName = "NUCLEATOR_SPECIAL_SURGE_NAME";
            mySkillDef.skillNameToken = "NUCLEATOR_SPECIAL_SURGE_NAME";

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
}*/