//XXX REMOVE FOR PUBLIC BUILDS
//#define NETWORKTEST

using System;
using System.Collections.Generic;
using EntityStates.Executioner;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.Networking;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using EntityStates;
using EntityStates.AI.Walker;
using KinematicCharacterController;
using System.Reflection;

//known bugs/issues:
//multiplayer not tested and probably broken

namespace Starstorm2.Cores
{
    public class ExecutionerCore
    {
        public const string charName = "Executioner";
        public const string charSubt = "Dreaded Guillotine";
        public const string charDesc = @"The Executioner is a high-risk, high-reward survivor that's all about racking up an endless kill count.<color=#CCD3E0>

< ! > Use Service Pistol to score some kills, and use those to charge up Ion Burst for massive damage.

< ! > Saving up Ion Burst charges is a risky move, but can pay off if you can get a bunch of shots off on a boss.

< ! > If you find yourself getting swarmed, Crowd Dispersion can get enemies off your back fast.

< ! > Execution is a great crowd control AND single target tool. Don't forget that its damage depends on how many targets it hits!";

        public const string charOutr = "..and so he left, an empty shell of armor drenched in blood.";
        public const string charLore = @"""Ugh, today’s selection sucks.""

The private lamented, taking a seat with his squadmate. He looked down at his bubbling pile of mush, considering if he should skip out on lunch for the day or not.

""Look Mac, on the bright side tomorrow's steak night."" The burly soldier sitting across from Mac began scarfing down his potatoes. 

""I don’t know how you force yourself to eat that crap, Connor."" Mac looked on at his friend with pure disgust in his face, as he somehow managed to clear his tray in record breaking time.

A third soldier hurriedly ran up to the table and slid into the seat next to Mac. ""Damn, sorry I’m late but you’re not gonna believe this,""

""What happened this time Marty? Did Joanne reject you for the 40th time?"" Connor chuckled, bits of potato and fish flying out of his mouth.""

""Dude for real, cut it out, this is legit. Look behind you, 5th table to the right."" Connor and Mac tilted their heads ever so slightly to see what Marty was on about.

Connor’s laughter turned to eerie silence. ""...No way, is that?""

""Yes, it is. Holy crap I thought they were a myth, I can’t believe one of them is in cafe with us right now,""

Mac kept staring, puzzled at the mysterious soldier in salmon colored armor sitting at the other side of the mess hall. ""Am I supposed to know who that is?""

Connor and Marty turned to Mac with wide eyes, before Marty spoke up. ""Are you being serious? Have you not heard?""

""Heard about what..?"" 

""Jesus Mac, those guys are insane. Rumors have it that they are these super high ranking special ops units, you can tell by his armor. Apparently they are ice cold, no one knows a single thing about them."" 

Mac kept staring.

Connor added, ""I saw one of them once on the field…""

The bewildered privates turned their gaze to their squadmate. Connor was sweating

 ""It was last year, when we were on Pluto. We were at the last stronghold for the Insurgency, one last push and it was ours. We began marching through the compound, going room to room. The place was dead, no one in sight. We were starting to think they escaped, but then we checked the final room and WHAM, we got jumped. My whole platoon was tied up for questioning.""

Connor was beginning to tense up, ""They went through every single one of us, and we didn’t tell them a goddamn thing. They started beating on us, they said if we didn’t tell them where the reinforcements were coming in from, they'd kill us all. We all knew we were going to die, we accepted it, we kept our mouths shut."" 

Mac and Marty leaned forward. ""I’ll never forget it, this big dude pulled out his pistol, and was about to put one right between my eyes. I thought I was done for...but then it happened. A pale blue light came from the back of the room, and cut one of the men clean in half. I have never seen anything like it in my life. A few of them panicked and ran away, some began shooting into the corner where it came from, but it didn’t matter."" Connor made a gun gesture with his hand, ""BAM, BAM, BAM! They all dropped.""

Mac was visibly uncomfortable, ""Do-...do you think this is the same guy?""

""I dunno man, and frankly, I don’t care,"" He let out a long sigh, ""It took a minute for my head to stop spinning, but when it did I just saw him, standing there, drenched in blood, unflinching. He untied us, left, and I never saw him again.""

Marty let out an audible gulp, ""God damn Connor, are you okay?""

""Honestly? No, I am not. I have never been that afraid of anything in my life, and he was on our side. I’m convinced that man was the Grim Reaper, ready to smite down the wicked. God forgive anyone who lays on the other end of that axe, because I sure as hell don’t want to be found guilty of judgement from the Executioners."" 

The two privates sat in complete silence.

";
        public Color charColor = new Color(0.69f, 0.44f, 0.49f);

        public static GameObject execPrefab;
        public static GameObject execDisplay;
        public static GameObject doppelganger;

        public static GameObject dashProjectile;
        public static BuffIndex fearDebuff;

        public ExecutionerCore() => Setup();

        private void Setup()
        {
#if NETWORKTEST
            //This is so we can connect to ourselves.
            //Instructions:
            //Step One: Assuming this line is in your codebase, start two instances of RoR2 (do this through the .exe directly)
            //Step Two: Host a game with one instance of RoR2.
            //Step Three: On the instance that isn't hosting, open up the console (ctrl + alt + tilde) and enter the command "connect localhost:7777"
            //DO NOT MAKE A MISTAKE SPELLING THE COMMAND OR YOU WILL HAVE TO RESTART THE CLIENT INSTANCE!!
            //Step Four: Test whatever you were going to test.
            On.RoR2.Networking.GameNetworkManager.OnClientConnect += (self, user, t) => { };
#endif
            CreatePrefab();
            RegisterBuffs();
            RegisterProjectiles();
            RegisterStates();
            RegisterCharacter();
            RegisterHooks();
        }

        internal static void CreatePrefab()
        {
            execPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "ExecutionerBody", true);
            execPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            UnityEngine.Object.Destroy(execPrefab.transform.Find("ModelBase").gameObject);
            UnityEngine.Object.Destroy(execPrefab.transform.Find("CameraPivot").gameObject);
            UnityEngine.Object.Destroy(execPrefab.transform.Find("AimOrigin").gameObject);

            GameObject model = AssetsCore.mainAssetBundle.LoadAsset<GameObject>("ExecutionerModel");

            GameObject modelBase = new GameObject("ModelBase");
            modelBase.transform.parent = execPrefab.transform;
            modelBase.transform.localPosition = new Vector3(0f, -0.81f, 0f);
            modelBase.transform.localRotation = Quaternion.identity;
            modelBase.transform.localScale = Vector3.one;

            GameObject camPivot = new GameObject("CameraPivot");
            camPivot.transform.parent = execPrefab.transform;
            camPivot.transform.localPosition = new Vector3(0f, -0.81f, 0f);
            camPivot.transform.rotation = Quaternion.identity;
            camPivot.transform.localScale = Vector3.one;

            GameObject aimOrigin = new GameObject("AimOrigin");
            aimOrigin.transform.parent = execPrefab.transform;
            aimOrigin.transform.localPosition = new Vector3(0f, 1.4f, 0f);
            aimOrigin.transform.rotation = Quaternion.identity;
            aimOrigin.transform.localScale = Vector3.one;

            Transform transform = model.transform;
            transform.parent = modelBase.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            CharacterDirection dir = execPrefab.GetComponent<CharacterDirection>();
            dir.moveVector = Vector3.zero;
            dir.targetTransform = modelBase.transform;
            dir.overrideAnimatorForwardTransform = null;
            dir.rootMotionAccumulator = null;
            dir.modelAnimator = model.GetComponentInChildren<Animator>();
            dir.driveFromRootRotation = false;
            dir.turnSpeed = 720f;

            //set up character body
            CharacterBody body = execPrefab.GetComponent<CharacterBody>();
            body.name = "ExecutionerBody";
            body.baseNameToken = "EXECUTIONER_NAME";
            body.subtitleNameToken = "EXECUTIONER_SUBTITLE";
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

            CharacterMotor motor = execPrefab.GetComponent<CharacterMotor>();
            motor.walkSpeedPenaltyCoefficient = 1f;
            motor.characterDirection = dir;
            motor.muteWalkMotion = false;
            motor.mass = 100f;
            motor.airControl = 0.25f;
            motor.disableAirControlUntilCollision = false;
            motor.generateParametersOnAwake = true;

            CameraTargetParams camParams = execPrefab.GetComponent<CameraTargetParams>();
            camParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CameraTargetParams>().cameraParams;
            camParams.cameraPivotTransform = null;
            camParams.aimMode = CameraTargetParams.AimType.Standard;
            camParams.recoil = Vector2.zero;
            camParams.idealLocalCameraPos = Vector3.zero;
            camParams.dontRaycastToPivot = false;

            ModelLocator locator = execPrefab.GetComponent<ModelLocator>();
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
            if (execPrefab.GetComponent<TeamComponent>() != null) teamComponent = execPrefab.GetComponent<TeamComponent>();
            else teamComponent = execPrefab.GetComponent<TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            HealthComponent health = execPrefab.GetComponent<HealthComponent>();
            health.health = 95f;
            health.shield = 0f;
            health.barrier = 0f;
            health.magnetiCharge = 0f;
            health.body = null;
            health.dontShowHealthbar = false;
            health.globalDeathEventChanceCoefficient = 1f;

            execPrefab.GetComponent<Interactor>().maxInteractionDistance = 3f;
            execPrefab.GetComponent<InteractionDriver>().highlightInteractor = true;

            CharacterDeathBehavior deathBehavior = execPrefab.GetComponent<CharacterDeathBehavior>();
            deathBehavior.deathStateMachine = execPrefab.GetComponent<EntityStateMachine>();
            deathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            //SfxLocator sfx = execPrefab.GetComponent<SfxLocator>();

            Rigidbody rigidbody = execPrefab.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider collider = execPrefab.GetComponent<CapsuleCollider>();
            collider.isTrigger = false;
            collider.material = null;
            collider.center = Vector3.zero;
            collider.radius = 0.5f;
            collider.height = 1.82f;
            collider.direction = 1;

            KinematicCharacterMotor kMotor = execPrefab.GetComponent<KinematicCharacterMotor>();
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
            aimer.inputBank = execPrefab.GetComponent<InputBankTest>();
            aimer.directionComponent = dir;
            aimer.pitchRangeMax = 60f;
            aimer.pitchRangeMin = -60f;
            aimer.yawRangeMax = 90f;
            aimer.yawRangeMin = -90f;
            aimer.pitchGiveupRange = 30f;
            aimer.yawGiveupRange = 10f;
            aimer.giveupDuration = 3f;

            IonGunChargeComponent charges = execPrefab.AddComponent<IonGunChargeComponent>();
        }

        private void RegisterBuffs()
        {
            BuffDef fearDef = new BuffDef()
            {
                buffColor = Color.white,
                iconPath = "@Starstorm2:Assets/Executioner/surprised-skull.png",
                isDebuff = true,
                name = "FearDebuff",
                eliteIndex = EliteIndex.None,
                canStack = false
            };

            CustomBuff fear = new CustomBuff(fearDef);
            ExecutionerCore.fearDebuff = BuffAPI.Add(fear);
        }

        private void RegisterProjectiles()
        {
            //unused, to delete
            dashProjectile = Resources.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("ExecutionerDash");

            ProjectileController dashController = dashProjectile.GetComponent<ProjectileController>();
            ProjectileImpactExplosion dashExplosion = dashProjectile.GetComponent<ProjectileImpactExplosion>();
            ProjectileSimple dashSimple = dashProjectile.GetComponent<ProjectileSimple>();
            ProjectileDamage dashDamage = dashProjectile.GetComponent<ProjectileDamage>();
            TeamFilter dashFilter = dashProjectile.GetComponent<TeamFilter>();
            BuffWard dashBuff = dashProjectile.AddComponent<BuffWard>();
            //DebuffZone dashDebuff = dashProjectile.AddComponent<DebuffZone>();

            //dashDebuff.buffDuration = ExecutionerDash.debuffDuration;
            //dashDebuff.buffType = fearDebuff;

            //effect behavior
            dashController.ghostPrefab = null;
            dashController.shouldPlaySounds = false;
            dashExplosion.blastRadius = 14f; //hardcoding :puke:
            dashExplosion.falloffModel = BlastAttack.FalloffModel.None;
            dashExplosion.lifetime = 0.1f;
            dashExplosion.fireChildren = false;
            dashExplosion.blastProcCoefficient = 0f;
            //probably doesn't do anything
            dashExplosion.bonusBlastForce = Vector3.up * 20f;
            dashSimple.velocity = 0f;
            dashSimple.enableVelocityOverLifetime = false;
            dashDamage.crit = false;
            dashDamage.damage = 0f;
            dashDamage.damageColorIndex = DamageColorIndex.Default;
            dashDamage.damageType = DamageType.Generic;
            dashDamage.force = 30f;

            dashFilter.teamIndex = TeamIndex.Player;

            dashBuff.animateRadius = false;
            dashBuff.buffDuration = 20.0f;
            dashBuff.buffType = fearDebuff;
            dashBuff.expires = false;
            dashBuff.floorWard = true;
            dashBuff.interval = 1f;
            dashBuff.invertTeamFilter = true;
            dashBuff.radius = 14f;

            ProjectileCatalog.getAdditionalEntries += delegate (List<GameObject> l)
            {
                l.Add(dashProjectile);
            };

            EffectAPI.AddEffect(dashProjectile);
        }

        private void RegisterStates()
        {
            LoadoutAPI.AddSkill(typeof(ExecutionerPistol));
            LoadoutAPI.AddSkill(typeof(ExecutionerIonGun));
            LoadoutAPI.AddSkill(typeof(ExecutionerDash));
            LoadoutAPI.AddSkill(typeof(ExecutionerAxe));
            LoadoutAPI.AddSkill(typeof(ExecutionerAxeSlam));
        }

        private void RegisterCharacter()
        {
            execDisplay = AssetsCore.mainAssetBundle.LoadAsset<GameObject>("ExecutionerModelMenu");

            LanguageAPI.Add("EXECUTIONER_NAME", charName);
            LanguageAPI.Add("EXECUTIONER_SUBTITLE", charSubt);
            LanguageAPI.Add("EXECUTIONER_DESCRIPTION", charDesc);
            LanguageAPI.Add("EXECUTIONER_OUTRO_FLAVOR", charOutr);
            LanguageAPI.Add("EXECUTIONER_LORE", charLore);

            SurvivorDef def = new SurvivorDef
            {
                name = "EXECUTIONER_NAME",
                descriptionToken = "EXECUTIONER_DESCRIPTION",
                outroFlavorToken = "EXECUTIONER_OUTRO_FLAVOR",

                primaryColor = charColor,
                bodyPrefab = execPrefab,
                displayPrefab = execDisplay
            };

            SurvivorAPI.AddSurvivor(def);

            SetUpSkills();

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(execPrefab);
            };
        }

        private void RegisterHooks()
        {
            //On.RoR2.CharacterBody.RecalculateStats += HandleFearDebuff;

            IL.EntityStates.AI.Walker.Combat.UpdateAI += (il) =>
            {
                ILCursor curs = new ILCursor(il);
                //go to where movement type is checked (applying movement vector)
                curs.GotoNext(x => x.MatchCall<Vector3>("Cross"));
                curs.Index += 2;
                curs.Emit(OpCodes.Ldarg_0);
                curs.Emit(OpCodes.Ldfld, typeof(EntityState).GetFieldCached("outer"));
                curs.Emit(OpCodes.Ldloc_1);
                curs.EmitDelegate<Func<EntityStateMachine, AISkillDriver.MovementType, AISkillDriver.MovementType>>((ESM, MoveType) =>
                {
                    if (ESM.GetComponent<CharacterMaster>().GetBody().HasBuff(fearDebuff))
                    {
                        return AISkillDriver.MovementType.FleeMoveTarget;
                    }
                    else
                        return MoveType;
                });
                curs.Emit(OpCodes.Stloc_1);
            };

            IL.EntityStates.AI.Walker.Combat.UpdateSkillButtonInputs += (il) =>
            {
                ILCursor curs = new ILCursor(il);
                curs.GotoNext(x => x.MatchLdfld<Combat>("currentSkillMeetsActivationConditions"));
                curs.Index += 1;
                curs.Emit(OpCodes.Ldarg_0);
                curs.Emit(OpCodes.Ldfld, typeof(EntityState).GetFieldCached("outer"));
                curs.EmitDelegate<Func<bool, EntityStateMachine, bool>>((cond, ESM) =>
                {
                    if (ESM.GetComponent<CharacterMaster>().GetBody())
                        if (ESM.GetComponent<CharacterMaster>().GetBody().HasBuff(fearDebuff))
                            return false;
                    return cond;
                });
            };
        }

        private void SetUpSkills()
        {
            foreach (GenericSkill sk in execPrefab.GetComponentsInChildren<GenericSkill>())
            {
                UnityEngine.Object.DestroyImmediate(sk);
            }

            SetUpPrimaries();
            SetUpSecondaries();
            SetUpUtilities();
            SetUpSpecials();
            //SetUpPassive();
        }

        private void SetUpPrimaries()
        {
            var dmg = ExecutionerPistol.damageCoefficient * 100f;

            SkillLocator skill = execPrefab.GetComponent<SkillLocator>();

            LanguageAPI.Add("EXECUTIONER_PISTOL_NAME", "Service Pistol");
            LanguageAPI.Add("EXECUTIONER_PISTOL_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{dmg}% damage</style>.");

            SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
            def.activationState = new EntityStates.SerializableEntityStateType(typeof(ExecutionerPistol));
            def.activationStateMachineName = "Weapon";
            def.skillName = "EXECUTIONER_PISTOL_NAME";
            def.skillNameToken = "EXECUTIONER_PISTOL_NAME";
            def.skillDescriptionToken = "EXECUTIONER_PISTOL_DESCRIPTION";
            def.icon = AssetsCore.mainAssetBundle.LoadAsset<Sprite>("skill1icon");
            def.baseMaxStock = 1;
            def.baseRechargeInterval = 0f;
            def.beginSkillCooldownOnSkillEnd = false;
            def.canceledFromSprinting = false;
            def.fullRestockOnAssign = true;
            def.interruptPriority = EntityStates.InterruptPriority.Any;
            def.isBullets = false;
            def.isCombatSkill = true;
            def.mustKeyPress = false;
            def.noSprint = true;
            def.rechargeStock = 1;
            def.requiredStock = 1;
            def.shootDelay = 0f;
            def.stockToConsume = 1;

            LoadoutAPI.AddSkillDef(def);

            skill.primary = execPrefab.AddComponent<GenericSkill>();
            SkillFamily fam = ScriptableObject.CreateInstance<SkillFamily>();
            fam.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(fam);
            skill.primary.SetFieldValue("_skillFamily", fam);

            SkillFamily fam2 = skill.primary.skillFamily;
            fam2.variants[0] = new SkillFamily.Variant
            {
                skillDef = def,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(def.skillNameToken, false, null)
            };
        }

        private void SetUpSecondaries()
        {
            var dmg = ExecutionerIonGun.damageCoefficient * 100f;

            SkillLocator skill = execPrefab.GetComponent<SkillLocator>();

            LanguageAPI.Add("EXECUTIONER_IONGUN_NAME", "Ion Burst");
            LanguageAPI.Add("EXECUTIONER_IONGUN_DESCRIPTION", $"Unload bullets that do <style=cIsDamage>{dmg}% damage</style>. <style=cIsUtility>Restocks a bullet for every enemy you kill</style>.");

            SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
            def.activationState = new EntityStates.SerializableEntityStateType(typeof(ExecutionerIonGun));
            def.activationStateMachineName = "Weapon";
            def.skillName = "EXECUTIONER_IONGUN_NAME";
            def.skillNameToken = "EXECUTIONER_IONGUN_NAME";
            def.skillDescriptionToken = "EXECUTIONER_IONGUN_DESCRIPTION";
            def.icon = AssetsCore.mainAssetBundle.LoadAsset<Sprite>("skill2icon");
            def.baseMaxStock = 10;
            def.baseRechargeInterval = 0f;
            def.beginSkillCooldownOnSkillEnd = true;
            def.canceledFromSprinting = false;
            def.fullRestockOnAssign = false;
            def.interruptPriority = EntityStates.InterruptPriority.Skill;
            def.isBullets = false;
            def.isCombatSkill = true;
            def.mustKeyPress = true;
            def.noSprint = true;
            def.rechargeStock = 0;
            def.requiredStock = 1;
            def.shootDelay = 0f;
            def.stockToConsume = 0;

            LoadoutAPI.AddSkillDef(def);

            skill.secondary = execPrefab.AddComponent<GenericSkill>();
            SkillFamily fam = ScriptableObject.CreateInstance<SkillFamily>();
            fam.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(fam);
            skill.secondary.SetFieldValue("_skillFamily", fam);

            SkillFamily fam2 = skill.secondary.skillFamily;
            fam2.variants[0] = new SkillFamily.Variant
            {
                skillDef = def,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(def.skillNameToken, false, null)
            };
        }

        private void SetUpUtilities()
        {
            var dur = ExecutionerDash.debuffDuration;

            SkillLocator skill = execPrefab.GetComponent<SkillLocator>();

            LanguageAPI.Add("KEYWORD_FEAR", "<style=cKeywordName>Fear</style><style=cSub>Causes afflicted enemies to stop attacking and flee.</style>");

            LanguageAPI.Add("EXECUTIONER_DASH_NAME", "Crowd Dispersion");
            LanguageAPI.Add("EXECUTIONER_DASH_DESCRIPTION", $"Boost forward and <style=cIsUtility>fear</style> all nearby enemies for {dur} seconds.");

            SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
            def.activationState = new EntityStates.SerializableEntityStateType(typeof(ExecutionerDash));
            def.activationStateMachineName = "Weapon";
            def.skillName = "EXECUTIONER_DASH_NAME";
            def.skillNameToken = "EXECUTIONER_DASH_NAME";
            def.skillDescriptionToken = "EXECUTIONER_DASH_DESCRIPTION";
            def.icon = AssetsCore.mainAssetBundle.LoadAsset<Sprite>("skill3icon");
            def.baseMaxStock = 1;
            def.baseRechargeInterval = 7.0f;
            def.beginSkillCooldownOnSkillEnd = true;
            def.canceledFromSprinting = false;
            def.fullRestockOnAssign = true;
            def.interruptPriority = EntityStates.InterruptPriority.Skill;
            def.isBullets = false;
            def.isCombatSkill = false;
            def.mustKeyPress = false;
            def.noSprint = false;
            def.rechargeStock = 1;
            def.requiredStock = 1;
            def.shootDelay = 0f;
            def.stockToConsume = 1;
            def.keywordTokens = new string[] { "KEYWORD_FEAR" };

            LoadoutAPI.AddSkillDef(def);

            skill.utility = execPrefab.AddComponent<GenericSkill>();
            SkillFamily fam = ScriptableObject.CreateInstance<SkillFamily>();
            fam.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(fam);
            skill.utility.SetFieldValue("_skillFamily", fam);

            SkillFamily fam2 = skill.utility.skillFamily;
            fam2.variants[0] = new SkillFamily.Variant
            {
                skillDef = def,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(def.skillNameToken, false, null)
            };
        }

        private void SetUpSpecials()
        {
            var dmg = ExecutionerAxeSlam.slamDamageCoefficient * 100f;

            SkillLocator skill = execPrefab.GetComponent<SkillLocator>();

            LanguageAPI.Add("EXECUTIONER_AXE_NAME", "Execution");
            LanguageAPI.Add("EXECUTIONER_AXE_DESCRIPTION", $"Launch into the air and slam downward with your ion axe, dealing up to <style=cIsDamage>{dmg}% damage</style>. <style=cDeath>Hitting multiple enemies reduces damage.</style> Successful kills <style=cIsUtility>reduce all cooldowns by 1 second</style>.");

            SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
            def.activationState = new EntityStates.SerializableEntityStateType(typeof(ExecutionerAxe));
            def.activationStateMachineName = "Weapon";
            def.skillName = "EXECUTIONER_AXE_NAME";
            def.skillNameToken = "EXECUTIONER_AXE_NAME";
            def.skillDescriptionToken = "EXECUTIONER_AXE_DESCRIPTION";
            def.icon = AssetsCore.mainAssetBundle.LoadAsset<Sprite>("skill4icon");
            def.baseMaxStock = 1;
            def.baseRechargeInterval = 8.0f;
            def.beginSkillCooldownOnSkillEnd = true;
            def.canceledFromSprinting = false;
            def.fullRestockOnAssign = true;
            def.interruptPriority = EntityStates.InterruptPriority.PrioritySkill;
            def.isBullets = false;
            def.isCombatSkill = true;
            def.mustKeyPress = false;
            def.noSprint = true;
            def.rechargeStock = 1;
            def.requiredStock = 1;
            def.shootDelay = 0f;
            def.stockToConsume = 1;

            LoadoutAPI.AddSkillDef(def);

            skill.special = execPrefab.AddComponent<GenericSkill>();
            SkillFamily fam = ScriptableObject.CreateInstance<SkillFamily>();
            fam.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(fam);
            skill.special.SetFieldValue("_skillFamily", fam);

            SkillFamily fam2 = skill.special.skillFamily;
            fam2.variants[0] = new SkillFamily.Variant
            {
                skillDef = def,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(def.skillNameToken, false, null)
            };
        }
    }

    public class IonGunChargeComponent : NetworkBehaviour
    {
        public int storedCharges = 0;
    }
}


