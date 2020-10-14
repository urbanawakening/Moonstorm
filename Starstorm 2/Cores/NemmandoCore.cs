using System;
using System.Collections.Generic;
using BepInEx;
using EntityStates;
using KinematicCharacterController;
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
        public static GameObject defaultCrosshair;

        public const string charName = "Nemesis Commando";
        public const string charSubt = "Some edgy boi";
        public const string charDesc = @"";

        public const string charOutr = "..and so he left, in search of chicken nuggets";
        public const string charLore = @"";

        public NemmandoCore() => Load();

        public void Load()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            if (instance == null)
            {
                instance = this;
            }
            CreateNemmando();
        }

        public void CreateNemmando()
        {
            CreatePrefab();
            RegisterCharacter();
            SkillSetup();
        }

        internal static void CreatePrefab()
        {
            characterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "ExecutionerBody", true);
            characterPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            UnityEngine.Object.Destroy(characterPrefab.transform.Find("ModelBase").gameObject);
            UnityEngine.Object.Destroy(characterPrefab.transform.Find("CameraPivot").gameObject);
            UnityEngine.Object.Destroy(characterPrefab.transform.Find("AimOrigin").gameObject);

            GameObject model = AssetsCore.mainAssetBundle.LoadAsset<GameObject>("NemmandoModel");

            GameObject modelBase = new GameObject("ModelBase");
            modelBase.transform.parent = characterPrefab.transform;
            modelBase.transform.localPosition = new Vector3(0f, -0.81f, 0f);
            modelBase.transform.localRotation = Quaternion.identity;
            modelBase.transform.localScale = Vector3.one;

            GameObject camPivot = new GameObject("CameraPivot");
            camPivot.transform.parent = characterPrefab.transform;
            camPivot.transform.localPosition = new Vector3(0f, -0.81f, 0f);
            camPivot.transform.rotation = Quaternion.identity;
            camPivot.transform.localScale = Vector3.one;

            GameObject aimOrigin = new GameObject("AimOrigin");
            aimOrigin.transform.parent = characterPrefab.transform;
            aimOrigin.transform.localPosition = new Vector3(0f, 1.4f, 0f);
            aimOrigin.transform.rotation = Quaternion.identity;
            aimOrigin.transform.localScale = Vector3.one;

            Transform transform = model.transform;
            transform.parent = modelBase.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            CharacterDirection dir = characterPrefab.GetComponent<CharacterDirection>();
            dir.moveVector = Vector3.zero;
            dir.targetTransform = modelBase.transform;
            dir.overrideAnimatorForwardTransform = null;
            dir.rootMotionAccumulator = null;
            dir.modelAnimator = model.GetComponentInChildren<Animator>();
            dir.driveFromRootRotation = false;
            dir.turnSpeed = 720f;

            //set up character body
            CharacterBody body = characterPrefab.GetComponent<CharacterBody>();
            body.name = "NemmandoBody";
            body.baseNameToken = "NEMMANDO_NAME";
            body.subtitleNameToken = "NEMMANDO_SUBTITLE";
            body.portraitIcon = AssetsCore.mainAssetBundle.LoadAsset<Texture2D>("exeportrait");
            body.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            body.rootMotionInMainState = false;
            body.mainRootSpeed = 0;
            body.bodyIndex = -1;
            body.baseDamage = 14f;
            body.levelDamage = 3f;
            body.baseMaxHealth = 95f;
            body.levelMaxHealth = 28f;
            body.baseRegen = 1f;
            body.levelRegen = 0.2f;
            body.baseMaxShield = 0f;
            body.levelMaxShield = 0f;
            body.baseArmor = 20;
            body.aimOriginTransform = aimOrigin.transform;
            body.hullClassification = HullClassification.Human;

            LoadoutAPI.AddSkill(typeof(ExecutionerMain));
            body.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(typeof(ExecutionerMain));

            CharacterMotor motor = characterPrefab.GetComponent<CharacterMotor>();
            motor.walkSpeedPenaltyCoefficient = 1f;
            motor.characterDirection = dir;
            motor.muteWalkMotion = false;
            motor.mass = 100f;
            motor.airControl = 0.25f;
            motor.disableAirControlUntilCollision = false;
            motor.generateParametersOnAwake = true;

            CameraTargetParams camParams = characterPrefab.GetComponent<CameraTargetParams>();
            camParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").GetComponent<CameraTargetParams>().cameraParams;
            camParams.cameraPivotTransform = null;
            camParams.aimMode = CameraTargetParams.AimType.Standard;
            camParams.recoil = Vector2.zero;
            camParams.idealLocalCameraPos = Vector3.zero;
            camParams.dontRaycastToPivot = false;

            ModelLocator locator = characterPrefab.GetComponent<ModelLocator>();
            locator.modelTransform = transform;
            locator.modelBaseTransform = modelBase.transform;
            locator.dontReleaseModelOnDeath = false;
            locator.autoUpdateModelTransform = true;
            locator.dontDetatchFromParent = false;
            locator.noCorpse = false;
            locator.normalizeToFloor = false;
            locator.preserveModel = false;

            ChildLocator childLoc = model.GetComponent<ChildLocator>();

            CharacterModel charModel = model.AddComponent<CharacterModel>();
            charModel.body = body;
            charModel.baseRendererInfos = new CharacterModel.RendererInfo[]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = model.GetComponentInChildren<SkinnedMeshRenderer>().material,
                    renderer = model.GetComponentInChildren<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };
            charModel.autoPopulateLightInfos = true;
            charModel.invisibilityCount = 0;
            charModel.temporaryOverlays = new List<TemporaryOverlay>();

            TeamComponent teamComponent = null;
            if (characterPrefab.GetComponent<TeamComponent>() != null) teamComponent = characterPrefab.GetComponent<TeamComponent>();
            else teamComponent = characterPrefab.GetComponent<TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            HealthComponent health = characterPrefab.GetComponent<HealthComponent>();
            health.health = 95f;
            health.shield = 0f;
            health.barrier = 0f;
            health.magnetiCharge = 0f;
            health.body = null;
            health.dontShowHealthbar = false;
            health.globalDeathEventChanceCoefficient = 1f;

            characterPrefab.GetComponent<Interactor>().maxInteractionDistance = 3f;
            characterPrefab.GetComponent<InteractionDriver>().highlightInteractor = true;

            CharacterDeathBehavior deathBehavior = characterPrefab.GetComponent<CharacterDeathBehavior>();
            deathBehavior.deathStateMachine = characterPrefab.GetComponent<EntityStateMachine>();
            deathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            //SfxLocator sfx = characterPrefab.GetComponent<SfxLocator>();

            Rigidbody rigidbody = characterPrefab.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider collider = characterPrefab.GetComponent<CapsuleCollider>();
            collider.isTrigger = false;
            collider.material = null;
            collider.center = Vector3.zero;
            collider.radius = 0.5f;
            collider.height = 1.82f;
            collider.direction = 1;

            KinematicCharacterMotor kMotor = characterPrefab.GetComponent<KinematicCharacterMotor>();
            kMotor.CharacterController = motor;
            kMotor.Capsule = collider;
            kMotor.Rigidbody = rigidbody;

            kMotor.DetectDiscreteCollisions = false;
            kMotor.GroundDetectionExtraDistance = 0f;
            kMotor.MaxStepHeight = 0.2f;
            kMotor.MinRequiredStepDepth = 0.1f;
            kMotor.MaxStableSlopeAngle = 55f;
            kMotor.MaxStableDistanceFromLedge = 0.5f;
            kMotor.PreventSnappingOnLedges = false;
            kMotor.MaxStableDenivelationAngle = 55f;
            kMotor.RigidbodyInteractionType = RigidbodyInteractionType.None;
            kMotor.PreserveAttachedRigidbodyMomentum = true;
            kMotor.HasPlanarConstraint = false;
            kMotor.PlanarConstraintAxis = Vector3.up;
            kMotor.StepHandling = StepHandlingMethod.None;
            kMotor.LedgeHandling = true;
            kMotor.InteractiveRigidbodyHandling = true;
            kMotor.SafeMovement = false;

            HurtBoxGroup hurtbox = model.AddComponent<HurtBoxGroup>();

            CapsuleCollider coll1 = model.GetComponentInChildren<CapsuleCollider>();
            HurtBox hb = coll1.gameObject.AddComponent<HurtBox>();
            hb.gameObject.layer = LayerIndex.entityPrecise.intVal;

            hb.healthComponent = health;
            hb.isBullseye = true;
            hb.damageModifier = HurtBox.DamageModifier.Normal;
            hb.hurtBoxGroup = hurtbox;
            hb.indexInGroup = 0;

            hurtbox.hurtBoxes = new HurtBox[] { hb };
            hurtbox.mainHurtBox = hb;
            hurtbox.bullseyeCount = 1;

            FootstepHandler footstep = model.AddComponent<FootstepHandler>();
            footstep.baseFootstepString = "Play_player_footstep";
            footstep.sprintFootstepOverrideString = "";
            footstep.enableFootstepDust = true;
            footstep.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");

            //RagdollController ragdoll = model.GetComponent<RagdollController>();
            //TODO
            //ragdoll.bones = null;
            //ragdoll.componentsToDisableOnRagdoll = null;

            AimAnimator aimer = model.AddComponent<AimAnimator>();
            aimer.inputBank = characterPrefab.GetComponent<InputBankTest>();
            aimer.directionComponent = dir;
            aimer.pitchRangeMax = 60f;
            aimer.pitchRangeMin = -60f;
            aimer.yawRangeMax = 90f;
            aimer.yawRangeMin = -90f;
            aimer.pitchGiveupRange = 30f;
            aimer.yawGiveupRange = 10f;
            aimer.giveupDuration = 3f;
        }

        void RegisterCharacter()
        {
            GameObject nemmandodisplay = PrefabAPI.InstantiateClone(characterPrefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "NemmandoDisplay", true);
            nemmandodisplay.AddComponent<NetworkIdentity>();

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(characterPrefab);
            };

            CharacterBody component = characterPrefab.GetComponent<CharacterBody>();
            component.baseDamage = 14f;
            component.baseCrit = 1f;
            component.levelCrit = 0f;
            component.baseMaxHealth = 150f;
            component.levelMaxHealth = 15f;
            component.baseArmor = 20f;
            component.baseRegen = 2f;
            component.levelRegen = 0.2f;
            component.baseMoveSpeed = 8f;
            component.levelMoveSpeed = 0.25f;
            component.baseAttackSpeed = 1f;
            component.levelDamage = 4f;
            component.name = "Nemesis Commando";
            component.subtitleNameToken = "Nemesis Commando";

            NemmandoCore.defaultCrosshair = component.crosshairPrefab;

            characterPrefab.GetComponent<CharacterBody>().preferredPodPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/toolbotbody").GetComponent<CharacterBody>().preferredPodPrefab;
            LanguageAPI.Add("");

            LanguageAPI.Add("NEMMANDO_DESCRIPTION", "This mofo looks for sum nice chicken.");
            LanguageAPI.Add("NEMMANDO_OUTRO_FLAVOR", "...and so he left, with KFC in his pockets.");

            var mySurvivorDef = new SurvivorDef
            {
                bodyPrefab = characterPrefab,
                descriptionToken = "NEMMANDO_DESCRIPTION",
                displayPrefab = nemmandodisplay,
                primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                name = "NEMMANDO",
                outroFlavorToken = "NEMMANDO_OUTRO_FLAVOR",
                unlockableName = "",
            };

            SurvivorAPI.AddSurvivor(mySurvivorDef);
        }

        void SkillSetup()
        {
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
            LanguageAPI.Add("NEMMANDO_PRIMARY_BLADE_NAME", "Blade of Cessation");
            LanguageAPI.Add("NEMMANDO_PRIMARY_BLADE_DESCRIPTION", @"Slice enemies with a chance to bleed");

            var mySkillDef = ScriptableObject.CreateInstance<SteppedSkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(BladeOfCessation));
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
            mySkillDef.skillDescriptionToken = "NEMMANDO_PRIMARY_BLADE_DESCRIPTION";
            mySkillDef.skillName = "NEMMANDO_PRIMARY_BLADE_NAME";
            mySkillDef.skillNameToken = "NEMMANDO_PRIMARY_BLADE_NAME";
            mySkillDef.stepCount = 2;
            mySkillDef.resetStepsOnIdle = false;
           

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
            LanguageAPI.Add("NEMMANDO_SECONDARY_CONCUSSION_NAME", "Concussion Grenade");
            LanguageAPI.Add("NEMMANDO_SECONDARY_CONCUSSION_DESCRIPTION", "Throw grenade above you stunning enemies on explosion.");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ChargeDecisiveStrike));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 3f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.noSprint = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("");
            mySkillDef.skillDescriptionToken = "NEMMANDO_SECONDARY_CONCUSSION_DESCRIPTION";
            mySkillDef.skillName = "NEMMANDO_SECONDARY_CONCUSSION_NAME";
            mySkillDef.skillNameToken = "NEMMANDO_SECONDARY_CONCUSSION_NAME";

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
            LanguageAPI.Add("NEMMANDO_UTILITY_DODGE_NAME", "Redacted, think of a name for utility");
            LanguageAPI.Add("NEMMANDO_UTILITY_DODGE_DESCRIPTION", "Pretty please");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(DodgeState));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("");
            mySkillDef.skillDescriptionToken = "NEMMANDO_UTILITY_DODGE_DESCRIPTION";
            mySkillDef.skillName = "NEMMANDO_UTILITY_DODGE_NAME";
            mySkillDef.skillNameToken = "NEMMANDO_UTILITY_DODGE_NAME";

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
            LanguageAPI.Add("NEMMANDO_SPECIAL_SUBMISSION_NAME", "Submission");
            LanguageAPI.Add("NEMMANDO_SPECIAL_SUBMISSION_DESCRIPTION", "Shoot enemies in front of you, don't stop after safe word.");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(Submission));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 10f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("");
            mySkillDef.skillDescriptionToken = "NEMMANDO_SPECIAL_SUBMISSION_DESCRIPTION";
            mySkillDef.skillName = "NEMMANDO_SPECIAL_SUBMISSION_NAME";
            mySkillDef.skillNameToken = "NEMMANDO_SPECIAL_SUBMISSION_NAME";

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