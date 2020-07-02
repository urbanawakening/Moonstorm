using Cloudburst;
using Cloudburst.Enemies.EntityStatez.ClayManMonster;
using EntityStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using static RoR2.CharacterAI.AISkillDriver;

namespace CloudBurst.Enemies
{
    //TODO:
    //Fix stupid targeting issue ugh
    internal sealed class ClayMan
    {
        public static GameObject clayMan;
        public static GameObject clayManMaster;

        public static SkillLocator skillLocator;

        public static void BuildClayMen()
        {
            clayMan = Resources.Load<GameObject>("prefabs/characterbodies/ClayBody");
            clayManMaster = Resources.Load<GameObject>("prefabs/charactermasters/ClaymanMaster");
            skillLocator = clayMan.GetComponent<SkillLocator>();

            BuildBody();
            BuildDirectorCard();
            RebuildSkillDrivers();
            RebuildSkills();
            FixHurtBox();
            FixInteractorandEquipment();
            BaseLogger.Log("Built Clay Men!");
        }

        private static void BuildDirectorCard()
        {
            On.RoR2.CharacterSpawnCard.Awake += CharacterSpawnCard_Awake;
            CharacterSpawnCard characterSpawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            On.RoR2.CharacterSpawnCard.Awake -= CharacterSpawnCard_Awake;

            DirectorAPI.DirectorCardHolder directorCardHolder = new DirectorAPI.DirectorCardHolder();
            DirectorCard directorCard = new DirectorCard();

            characterSpawnCard.directorCreditCost = 50;
            characterSpawnCard.forbiddenAsBoss = false;
            characterSpawnCard.name = "cscClaymanDude";
            //characterSpawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.None;
            characterSpawnCard.hullSize = HullClassification.Human;
            characterSpawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            characterSpawnCard.noElites = false;
            characterSpawnCard.prefab = clayManMaster;
            characterSpawnCard.occupyPosition = false;
            characterSpawnCard.sendOverNetwork = true;

            directorCard.allowAmbushSpawn = true;
            directorCard.forbiddenUnlockable = "";
            directorCard.minimumStageCompletions = 1;
            directorCard.preventOverhead = false;
            directorCard.requiredUnlockable = "";
            directorCard.selectionWeight = 1;
            directorCard.spawnCard = characterSpawnCard;
            directorCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;

            directorCardHolder.Card = directorCard;
            directorCardHolder.InteractableCategory = DirectorAPI.InteractableCategory.None;
            directorCardHolder.MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters;

            DirectorAPI.MonsterActions += delegate (List<DirectorAPI.DirectorCardHolder> list, DirectorAPI.StageInfo stage)
            {
                if (stage.stage == DirectorAPI.Stage.SkyMeadow || stage.stage == DirectorAPI.Stage.AbandonedAqueduct || stage.stage == DirectorAPI.Stage.RallypointDelta || stage.stage == DirectorAPI.Stage.ScorchedAcres)
                {
                    if (!list.Contains(directorCardHolder))
                    {
                        list.Add(directorCardHolder);
                    }
                }
            };
        }
        private static void FixHurtBox()
        {
            GameObject badVariableName = clayMan.GetComponent<ModelLocator>().modelTransform.gameObject;
            CapsuleCollider hitBox = badVariableName.gameObject.AddComponent<CapsuleCollider>();
            ChildLocator childLocator = clayMan.GetComponentInChildren<ChildLocator>();

            hitBox.radius *= 2;

            badVariableName.gameObject.layer = LayerIndex.entityPrecise.intVal;

            HurtBox hurtBox = badVariableName.AddComponent<HurtBox>();

            HurtBoxGroup hurtBoxGroup = badVariableName.AddComponent<HurtBoxGroup>();

            hurtBox.damageModifier = HurtBox.DamageModifier.Normal;
            hurtBox.healthComponent = clayMan.GetComponent<HealthComponent>();
            hurtBox.hurtBoxGroup = hurtBoxGroup;
            hurtBox.indexInGroup = 0;
            hurtBox.isBullseye = true;

            hurtBoxGroup.bullseyeCount = 1;
            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                hurtBox
            };
            hurtBoxGroup.mainHurtBox = hurtBox;
        }
        private static void FixInteractorandEquipment()
        {
            Interactor interactor = clayMan.AddOrGetComponent<Interactor>();
            clayMan.AddOrGetComponent<InteractionDriver>().highlightInteractor = true;
            interactor.maxInteractionDistance = 4;

            //clayMan.AddOrGetComponent<TeamComponent>().teamIndex = TeamIndex.Monster;

            clayMan.AddComponent<EquipmentSlot>();
        }
        private static void CharacterSpawnCard_Awake(On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self)
        {
            self.loadout = new SerializableLoadout();
            orig(self);
        }
        private static void BuildBody()
        {
            CharacterBody characterBody = clayMan.GetComponent<CharacterBody>();
            if (characterBody)
            {
                LanguageAPI.Add("CLAYMAN_BODY_TOKEN", "Clay Man");
                characterBody.baseAcceleration = 80f;
                characterBody.baseArmor = 10; //Base armor this character has, set to 20 if this character is melee 
                characterBody.baseAttackSpeed = 1; //Base attack speed, usually 1
                characterBody.baseCrit = 1;  //Base crit, usually one
                characterBody.baseDamage = 12; //Base damage
                characterBody.baseJumpCount = 1; //Base jump amount, set to 2 for a double jump. 
                characterBody.baseJumpPower = 14; //Base jump power
                characterBody.baseMaxHealth = 110; //Base health, basically the health you have when you start a new run
                characterBody.baseMaxShield = 0; //Base shield, basically the same as baseMaxHealth but with shields
                characterBody.baseMoveSpeed = 8; //Base move speed, this is usual 7
                characterBody.baseNameToken = "CLAYMAN_BODY_TOKEN"; //The base name token. 
                characterBody.subtitleNameToken = ""; //Set this to true if its a boss
                characterBody.baseRegen = 1.4f; //Base health regen.
                characterBody.bodyFlags = (CharacterBody.BodyFlags.ImmuneToGoo); ///Base body flags, should be self explanatory 
                //characterBody.crosshairPrefab = characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/HuntressBody").GetComponent<CharacterBody>().crosshairPrefab; //The crosshair prefab.
                characterBody.hideCrosshair = false; //Whether or not to hide the crosshair
                characterBody.hullClassification = HullClassification.Human; //The hull classification, usually used for AI
                characterBody.isChampion = false; //Set this to true if its A. a boss or B. a miniboss
                characterBody.levelArmor = 0; //Armor gained when leveling up. 
                characterBody.levelAttackSpeed = 0; //Attackspeed gained when leveling up. 
                characterBody.levelCrit = 0; //Crit chance gained when leveling up. 
                characterBody.levelDamage = 2.5f; //Damage gained when leveling up. 
                characterBody.levelArmor = 3; //Armor gaix; //Health gained when leveling up. 
                characterBody.levelMaxShield = 0; //Shield gained when leveling up. 
                characterBody.levelMoveSpeed = 0; //Move speed gained when leveling up. 
                characterBody.levelRegen = 0.6f; //Regen gained when leveling up. 
                                                 //characterBody.portraitIcon = portrait; //The portrait icon, shows up in multiplayer and the death UI
                                                 //characterBody.preferredPodPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/robocratepod"); //The pod prefab this survivor spawns in. Options: Resources.Load<GameObject>("prefabs/networkedobjects/robocratepod"); Resources.Load<GameObject>("prefabs/networkedobjects/survivorpod"); 
            }
        }
        private static void RebuildSkillDrivers()
        {
            BaseHelpers.DestroySkillDrivers(clayManMaster);

            AISkillDriver Swing = clayManMaster.AddComponent<AISkillDriver>();
            AISkillDriver Chase = clayManMaster.AddComponent<AISkillDriver>();
            AISkillDriver Leap = clayManMaster.AddComponent<AISkillDriver>();
            AISkillDriver anotherSkillDriver = clayManMaster.AddComponent<AISkillDriver>();
            AISkillDriver andAnotherSkillDriver = clayManMaster.AddComponent<AISkillDriver>();

            Swing.skillSlot = SkillSlot.Primary;
            Swing.requireSkillReady = false;
            Swing.requireEquipmentReady = false;
            Swing.moveTargetType = TargetType.CurrentEnemy;
            Swing.minUserHealthFraction = float.NegativeInfinity;
            Swing.maxUserHealthFraction = float.PositiveInfinity;
            Swing.minTargetHealthFraction = float.NegativeInfinity;
            Swing.maxTargetHealthFraction = float.PositiveInfinity;
            Swing.minDistance = 0;
            Swing.maxDistance = 2;
            Swing.selectionRequiresTargetLoS = false;
            Swing.activationRequiresTargetLoS = false;
            Swing.activationRequiresAimConfirmation = false;
            Swing.movementType = MovementType.ChaseMoveTarget;
            Swing.moveInputScale = 1;
            Swing.aimType = AimType.AtMoveTarget;
            Swing.ignoreNodeGraph = true;
            Swing.driverUpdateTimerOverride = -1;
            Swing.resetCurrentEnemyOnNextDriverSelection = false;
            Swing.noRepeat = false;
            Swing.shouldSprint = false;
            Swing.shouldFireEquipment = false;
            Swing.shouldTapButton = false;

            Leap.skillSlot = SkillSlot.Secondary;
            Leap.requireSkillReady = false;
            Leap.requireEquipmentReady = false;
            Leap.moveTargetType = TargetType.CurrentEnemy;
            Leap.minUserHealthFraction = float.NegativeInfinity;
            Leap.maxUserHealthFraction = float.PositiveInfinity;
            Leap.minTargetHealthFraction = float.NegativeInfinity;
            Leap.maxTargetHealthFraction = float.PositiveInfinity;
            Leap.minDistance = 0;
            Leap.maxDistance = 30;
            Leap.selectionRequiresTargetLoS = false;
            Leap.activationRequiresTargetLoS = false;
            Leap.activationRequiresAimConfirmation = false;
            Leap.movementType = MovementType.ChaseMoveTarget;
            Leap.moveInputScale = 1;
            Leap.aimType = AimType.AtMoveTarget;
            Leap.ignoreNodeGraph = true;
            Leap.driverUpdateTimerOverride = -1;
            Leap.resetCurrentEnemyOnNextDriverSelection = false;
            Leap.noRepeat = false;
            Leap.shouldSprint = false;
            Leap.shouldFireEquipment = false;
            Leap.shouldTapButton = false;

            Chase.skillSlot = SkillSlot.None;
            Chase.requireSkillReady = false;
            Chase.requireEquipmentReady = false;
            Chase.moveTargetType = TargetType.CurrentEnemy;
            Chase.minUserHealthFraction = float.NegativeInfinity;
            Chase.maxUserHealthFraction = float.PositiveInfinity;
            Chase.minTargetHealthFraction = float.NegativeInfinity;
            Chase.maxTargetHealthFraction = float.PositiveInfinity;
            Chase.minDistance = 10;
            Chase.maxDistance = 10;
            Chase.selectionRequiresTargetLoS = true;
            Chase.activationRequiresTargetLoS = false;
            Chase.activationRequiresAimConfirmation = false;
            Chase.movementType = MovementType.ChaseMoveTarget;
            Chase.moveInputScale = 1;
            Chase.aimType = AimType.AtMoveTarget;
            Chase.ignoreNodeGraph = true;
            Chase.driverUpdateTimerOverride = -1;
            Chase.resetCurrentEnemyOnNextDriverSelection = false;
            Chase.noRepeat = false;
            Chase.shouldSprint = false;
            Chase.shouldFireEquipment = false;
            Chase.shouldTapButton = false;

            anotherSkillDriver.customName = "FollowNodeGraphToTarget";
            anotherSkillDriver.skillSlot = SkillSlot.None;
            anotherSkillDriver.requireSkillReady = false;
            anotherSkillDriver.requireEquipmentReady = false;
            anotherSkillDriver.moveTargetType = TargetType.CurrentEnemy;
            anotherSkillDriver.minUserHealthFraction = float.NegativeInfinity;
            anotherSkillDriver.maxUserHealthFraction = float.PositiveInfinity;
            anotherSkillDriver.minTargetHealthFraction = float.NegativeInfinity;
            anotherSkillDriver.maxTargetHealthFraction = float.PositiveInfinity;
            anotherSkillDriver.minDistance = 0;
            anotherSkillDriver.maxDistance = float.PositiveInfinity;
            anotherSkillDriver.selectionRequiresTargetLoS = false;
            anotherSkillDriver.activationRequiresTargetLoS = false;
            anotherSkillDriver.activationRequiresAimConfirmation = false;
            anotherSkillDriver.movementType = MovementType.ChaseMoveTarget;
            anotherSkillDriver.moveInputScale = 1;
            anotherSkillDriver.aimType = AimType.MoveDirection;
            anotherSkillDriver.ignoreNodeGraph = false;
            anotherSkillDriver.driverUpdateTimerOverride = -1;
            anotherSkillDriver.resetCurrentEnemyOnNextDriverSelection = false;
            anotherSkillDriver.noRepeat = false;
            anotherSkillDriver.shouldSprint = false;
            anotherSkillDriver.shouldFireEquipment = false;
            anotherSkillDriver.shouldTapButton = false;

            andAnotherSkillDriver.customName = "ChaseOffNodegraph";
            andAnotherSkillDriver.skillSlot = SkillSlot.None;
            andAnotherSkillDriver.requireSkillReady = false;
            andAnotherSkillDriver.requireEquipmentReady = false;
            andAnotherSkillDriver.moveTargetType = TargetType.CurrentEnemy;
            andAnotherSkillDriver.minUserHealthFraction = float.NegativeInfinity;
            andAnotherSkillDriver.maxUserHealthFraction = float.PositiveInfinity;
            andAnotherSkillDriver.minTargetHealthFraction = float.NegativeInfinity;
            andAnotherSkillDriver.maxTargetHealthFraction = float.PositiveInfinity;
            andAnotherSkillDriver.minDistance = 0;
            andAnotherSkillDriver.maxDistance = 10;
            andAnotherSkillDriver.selectionRequiresTargetLoS = true;
            andAnotherSkillDriver.activationRequiresTargetLoS = false;
            andAnotherSkillDriver.activationRequiresAimConfirmation = false;
            andAnotherSkillDriver.movementType = MovementType.ChaseMoveTarget;
            andAnotherSkillDriver.moveInputScale = 1;
            andAnotherSkillDriver.aimType = AimType.AtMoveTarget;
            andAnotherSkillDriver.ignoreNodeGraph = true;
            andAnotherSkillDriver.driverUpdateTimerOverride = -1;
            andAnotherSkillDriver.resetCurrentEnemyOnNextDriverSelection = false;
            andAnotherSkillDriver.noRepeat = false;
            andAnotherSkillDriver.shouldSprint = false;
            andAnotherSkillDriver.shouldFireEquipment = false;
            andAnotherSkillDriver.shouldTapButton = false;
        }
        private static void RebuildSkills()
        {
            BaseHelpers.DestroyGenericSkillComponents(clayMan);
            CreateSkillFamilies();
            CreatePrimary();
            CreateSecondary();
        }
        private static void CreateSkillFamilies()
        {
            skillLocator.SetFieldValue<GenericSkill[]>("allSkills", new GenericSkill[0]);
            {
                skillLocator.primary = clayMan.AddComponent<GenericSkill>();
                SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
                newFamily.variants = new SkillFamily.Variant[1];
                LoadoutAPI.AddSkillFamily(newFamily);
                skillLocator.primary.SetFieldValue("_skillFamily", newFamily);
            }
            {
                skillLocator.secondary = clayMan.AddComponent<GenericSkill>();
                SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
                newFamily.variants = new SkillFamily.Variant[1];
                LoadoutAPI.AddSkillFamily(newFamily);
                skillLocator.secondary.SetFieldValue("_skillFamily", newFamily);
            }
            /*{
                skillLocator.utility = clayMan.AddComponent<GenericSkill>();
                SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
                newFamily.variants = new SkillFamily.Variant[1];
                LoadoutAPI.AddSkillFamily(newFamily);
                skillLocator.utility.SetFieldValue("_skillFamily", newFamily);
            }
            {
                skillLocator.special = clayMan.AddComponent<GenericSkill>();
                SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
                newFamily.variants = new SkillFamily.Variant[1];
                LoadoutAPI.AddSkillFamily(newFamily);
                skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            }*/
        }
        private static void CreatePrimary()
        {
            SkillDef primarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            primarySkillDef.activationState = new SerializableEntityStateType(typeof(SwipeForward));
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
            SkillFamily primarySkillFamily = skillLocator.primary.skillFamily;

            primarySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = primarySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(primarySkillDef.skillNameToken, false, null)

            };
        }
        private static void CreateSecondary()
        {

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            secondarySkillDef.activationState = new SerializableEntityStateType(typeof(Leap));
            secondarySkillDef.activationStateMachineName = "Weapon";
            secondarySkillDef.baseMaxStock = 1;
            secondarySkillDef.baseRechargeInterval = 10f;
            secondarySkillDef.beginSkillCooldownOnSkillEnd = true;
            secondarySkillDef.canceledFromSprinting = false;
            secondarySkillDef.fullRestockOnAssign = true;
            secondarySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            secondarySkillDef.isBullets = false;
            secondarySkillDef.isCombatSkill = true;
            secondarySkillDef.mustKeyPress = false;
            secondarySkillDef.noSprint = false;
            secondarySkillDef.rechargeStock = 1;
            secondarySkillDef.requiredStock = 1;
            secondarySkillDef.shootDelay = 0.5f;
            secondarySkillDef.stockToConsume = 1;
            secondarySkillDef.skillDescriptionToken = "AAAAAAAAAAAAAAAAAAAAAA";
            secondarySkillDef.skillName = "aaa";
            secondarySkillDef.skillNameToken = "aa";

            LoadoutAPI.AddSkillDef(secondarySkillDef);
            SkillFamily secondarySkillFamily = skillLocator.secondary.skillFamily;

            secondarySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = secondarySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(secondarySkillDef.skillNameToken, false, null)

            };
        }
    }
}
