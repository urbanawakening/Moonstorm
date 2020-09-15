using EntityStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using UnityEngine;
using static RoR2.DotController;
using Cloudburst.Cores.HAND.Components;
using Cloudburst.Cores.HAND.Skills;
using Cloudburst.Cores.States.HAND;

namespace Cloudburst.Cores.HAND
{
    public class HAN_DCore
    {
        #region Instance and logger
        public static HAN_DCore instance;
        #endregion
        #region Prefabs
        public GameObject winch;
        public GameObject HANDBody;
        public GameObject sunder;
        #endregion
        #region Components
        public SkillLocator skillLocator;
        public OverclockComponent overclockComponent;
        public DroneComponent droneComponent;
        public HANDPassiveController passiveContoller;
        public CharacterBody characterBody;
        #endregion

        public HAN_DCore() => Load();

        public void Load() {
            LogCore.LogI("Initializing Core: " + base.ToString());

            if (instance == null)
            {
                instance = this;
            }
            CreateHAND();
#if RELEASE
            LogW("You're on a debug build. If you see this after downloading from the thunderstore, panic!");
            
            On.RoR2.Networking.GameNetworkManager.OnClientConnect += (self, user, t) => { };
#endif
        }
        private void CreateHAND()
        {
            CreateTokens();
            CreateHANDPrefab();
            CreateSunderPrefab();
            //CreateWinchPrefab();
            SetComponents();
            SetSkills();
            CreateSurvivorDef();
            Hook();
        }
        #region Orbs

        #endregion
        #region Prefab Creation
        private void CreateSunderPrefab() {
            sunder = Resources.Load<GameObject>("prefabs/projectiles/Sunder").InstantiateClone("HANDSunder", true);
            API.RegisterNewProjectile(sunder);
            UnityEngine.Object.Destroy(sunder.GetComponent<ProjectileOverlapAttack>());

            var overlap = sunder.AddComponent<ShockwaveProjectileComponent>();
            overlap.damageCoefficient = 2;
            overlap.impactEffect = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniImpactVFX");
            overlap.forceVector = new Vector3(0.0f, 1400.0f, 0.0f);
            overlap.overlapProcCoefficient = 1;
            overlap.maximumOverlapTargets = 100;
            overlap.fireFrequency = 60;
            overlap.resetInterval = -1f;
        }

        private void CreateWinchPrefab()
        {
            //this.winch = Resources.Load<GameObject>("prefabs/projectiles/EngiHarpoon").InstantiateClone("HANDWinch", true);
            GameObject groveWinch = Resources.Load<GameObject>("prefabs/projectiles/GravekeeperHookProjectileSimple");

            this.winch.GetComponent<ProjectileController>().ghostPrefab = groveWinch.GetComponent<ProjectileController>().ghostPrefab;

            ProjectileProximityBeamController beamController = winch.AddComponent<ProjectileProximityBeamController>();

            ProjectileDamage damage = winch.GetComponent<ProjectileDamage>();

            MissileController missile = winch.GetComponent<MissileController>();

            winch.GetComponent<ProjectileSingleTargetImpact>().impactEffect = Resources.Load<GameObject>("prefabs/effects/omnieffects/omniimpactvfxslash");

            beamController.attackFireCount = 1;
            beamController.attackInterval = 0.01f;
            beamController.listClearInterval = 0.33f;
            beamController.attackRange = 5;
            beamController.minAngleFilter = 0;
            beamController.maxAngleFilter = 180;
            beamController.procCoefficient = 0;
            beamController.damageCoefficient = 0.3f;
            beamController.bounces = 0;
            beamController.inheritDamageType = false;
            beamController.lightningType = RoR2.Orbs.LightningOrb.LightningType.Ukulele;

            damage.damageType = DamageType.Stun1s;
            damage.force = -5000;

            missile.deathTimer = 4;
            missile.giveupTimer = 4;
            missile.giveupTimer = 25;

            API.RegisterNewProjectile(winch);
            #region Old
            /*this.winch = Resources.Load<GameObject>("prefabs/projectiles/SyringeProjectile").InstantiateClone("HANDWinch", true);

            GameObject groveWinch = Resources.Load<GameObject>("prefabs/projectiles/GravekeeperHookProjectileSimple");

            //this is retarded
            DestroyImmediate(winch.GetComponent<ProjectileSingleTargetImpact>());
            DestroyImmediate(winch.GetComponent<ProjectileDamage>());
            DestroyImmediate(winch.GetComponent<ProjectileSimple>());
            DestroyImmediate(winch.GetComponent<ProjectileController>());
            DestroyImmediate(winch.GetComponent<TeamFilter>());
            DestroyImmediate(winch.GetComponent<ProjectileNetworkTransform>());
            DestroyImmediate(winch.GetComponent<NetworkIdentity>());

            gameObject.layer = LayerIndex.projectile.intVal;

            NetworkIdentity networkIdentity = winch.AddComponent<NetworkIdentity>();
            ProjectileController projectileController = winch.AddComponent<ProjectileController>();
            ProjectileNetworkTransform projectileNetworkTransform = winch.AddComponent<ProjectileNetworkTransform>();
            ProjectileSimple projectileSimple = winch.AddComponent<ProjectileSimple>();
            TeamFilter teamFilter = winch.AddComponent<TeamFilter>();
            ProjectileSingleTargetImpact projectileSingleTargetImpact = winch.AddComponent<ProjectileSingleTargetImpact>();
            ProjectileDamage projectileDamage = winch.AddComponent<ProjectileDamage>();
            SphereCollider sphereCollider = winch.GetComponent<SphereCollider>();
            ProjectileTargetComponent targetComp = winch.AddComponent<ProjectileTargetComponent>();
            MissileController missileController = winch.AddComponent<MissileController>();
            QuaternionPID pid = winch.AddComponent<QuaternionPID>();
            QuaternionPID pid2 = Resources.Load<GameObject>("prefabs/projectiles/EngiHarpoon").GetComponent<QuaternionPID>();


            networkIdentity.localPlayerAuthority = true;
            networkIdentity.serverOnly = false;

            projectileController.ghostPrefab = groveWinch.GetComponent<ProjectileController>().ghostPrefab;
            projectileController.isPrediction = false;
            projectileController.allowPrediction = true;
            projectileController.predictionId = 0;
            projectileController.procChainMask = default;
            projectileController.procCoefficient = 1;

            projectileNetworkTransform.positionTransmitInterval = 0.03333334f;
            projectileNetworkTransform.interpolationFactor = 1;
            projectileNetworkTransform.allowClientsideCollision = false;

            projectileSimple.velocity = 280;
            projectileSimple.lifetime = 3;
            projectileSimple.updateAfterFiring = false;
            projectileSimple.enableVelocityOverLifetime = false;
            projectileSimple.velocityOverLifetime = groveWinch.GetComponent<ProjectileSimple>().velocityOverLifetime;

            projectileSingleTargetImpact.destroyWhenNotAlive = true;
            projectileSingleTargetImpact.destroyOnWorld = true;
            projectileSingleTargetImpact.impactEffect = Resources.Load<GameObject>("prefabs/effects/omnieffects/omniimpactvfxslash");

            projectileDamage.damage = 0;
            projectileDamage.crit = false;
            projectileDamage.force = 0;
            projectileDamage.damageColorIndex = DamageColorIndex.Default;
            projectileDamage.damageType = DamageType.BypassArmor | DamageType.Stun1s;

            missileController.maxVelocity = 25;
            missileController.rollVelocity = 0;
            missileController.acceleration = 3;
            missileController.delayTimer = 0.3f;
            missileController.giveupTimer = 8;
            missileController.deathTimer = 8;
            missileController.turbulence = 0;
            missileController.maxSeekDistance = 100;

            pid.customName = pid2.customName;
            pid.gain = pid2.gain;
            pid.inputQuat = pid2.inputQuat;
            pid.outputVector = pid2.outputVector;
            pid.targetQuat = pid2.targetQuat;*/
            #endregion
        }
        private void CreateHANDPrefab()
        {
            if (!HANDBody)
            {
                this.HANDBody = Resources.Load<GameObject>("prefabs/characterbodies/HANDBody");
            }
        }
        #endregion
        #region Components
        private void SetComponents()
        {
            skillLocator = HANDBody.GetComponent<SkillLocator>();
            overclockComponent = HANDBody.AddComponent<OverclockComponent>();
            droneComponent = HANDBody.AddComponent<DroneComponent>();
            characterBody = HANDBody.GetComponent<CharacterBody>();
            SfxLocator sfxLocator = HANDBody.GetComponent<SfxLocator>();
            CharacterDeathBehavior characterDeathBehavior = HANDBody.GetComponent<CharacterDeathBehavior>();
            HANDDroneTracker tracker = HANDBody.AddComponent<HANDDroneTracker>();
            OverclockComboActivation combo = HANDBody.AddComponent<OverclockComboActivation>();
            passiveContoller = HANDBody.AddComponent<HANDPassiveController>();

            //kil
            LoadoutAPI.AddSkill(typeof(DeathState));
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(DeathState));

            //sfx
            sfxLocator.fallDamageSound = "Play_MULT_shift_hit";
            sfxLocator.landingSound = "play_char_land";

            tracker.maxTrackingAngle = 20;
            tracker.maxTrackingDistance = 100;
            tracker.trackerUpdateFrequency = 5;
            tracker.indicatorPrefab = Resources.Load<GameObject>("Prefabs/EngiShieldRetractIndicator");

            SetCharacterBody();
            FixSetStateOnHurt();
        }

        private void FixSetStateOnHurt()
        {
            SetStateOnHurt hurtState = HANDBody.AddOrGetComponent<SetStateOnHurt>(); //AddOrGetComponent is because 

            hurtState.canBeFrozen = true; //If this gameobject can be frozen. Set to true if this is a survivor
            hurtState.canBeHitStunned = false; //If this gameobject can be stunned by being hit. Set to false if this is a survivor.
            hurtState.canBeStunned = false; //If this gameobject is able to be stunned. Set to false if this is a survivor
            hurtState.hitThreshold = 5f; //The hit threshold, set to 5 if this is a survivor.

            int i = 0;
            EntityStateMachine[] esmr = new EntityStateMachine[2];
            foreach (EntityStateMachine esm in HANDBody.GetComponentsInChildren<EntityStateMachine>())
            {
                switch (esm.customName)
                {
                    case "Body":
                        hurtState.targetStateMachine = esm;
                        break;
                    default:
                        if (i < 2)
                        {
                            esmr[i] = esm;
                        }
                        i++;
                        break;
                }
            }
            hurtState.idleStateMachine = esmr;
        }

        private void SetCharacterBody()
        {
            characterBody.baseAcceleration = 70f;
            characterBody.baseArmor = 20; //Base armor this character has, set to 20 if this character is melee 
            characterBody.baseAttackSpeed = 1; //Base attack speed, usually 1
            characterBody.baseCrit = 1;  //Base crit, usually one
            characterBody.baseDamage = 12; //Base damage
            characterBody.baseJumpCount = 1; //Base jump amount, set to 2 for a double jump. 
            characterBody.baseJumpPower = 16; //Base jump power
            characterBody.baseMaxHealth = 150; //Base health, basically the health you have when you start a new run
            characterBody.baseMaxShield = 0; //Base shield, basically the same as baseMaxHealth but with shields
            characterBody.baseMoveSpeed = 7; //Base move speed, this is usual 7
            characterBody.baseNameToken = "HAND_BODY_NAME"; //The base name token. 
            characterBody.subtitleNameToken = "HAND_BODY_SUBTITLE"; //Set this if its a boss
            characterBody.baseRegen = 1.5f; //Base health regen.
            characterBody.bodyFlags = (CharacterBody.BodyFlags.ImmuneToExecutes | CharacterBody.BodyFlags.Mechanical); ///Base body flags, should be self explanatory 
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
            //credits to moffein for the icon
            characterBody.portraitIcon = AssetsCore.portraitIcon; //The portrait icon, shows up in multiplayer and the death UI
            characterBody.preferredPodPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/robocratepod");
        }
        #endregion
        #region Skill Creation

        private void SetSkills()
        {
            API.CreateEmptySkills(HANDBody);
            FixSkin();
            RegisterDamageTypes();
            CreatePassives();
            CreatePrimary();
            CreateSecondary();
            CreateUtility();
            CreateSpecial();
            InitSkillsStates();
        }

        private void FixSkin()
        {
            GameObject model = HANDBody.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            SkinnedMeshRenderer FixRenderInfo()
            {
                SkinnedMeshRenderer mainRenderer = Reflection.GetFieldValue<SkinnedMeshRenderer>(characterModel, "mainSkinnedMeshRenderer"); //Reflection.GetFieldValue<CharacterModel.RendererInfo>(modelController, "baseRenderInfos")
                if (mainRenderer == null)
                {
                    CharacterModel.RendererInfo[] info = Reflection.GetFieldValue<CharacterModel.RendererInfo[]>(characterModel, "baseRendererInfos");
                    if (info != null)
                    {
                        foreach (CharacterModel.RendererInfo rendererInfo in info)
                        {
                            if (rendererInfo.renderer is SkinnedMeshRenderer)
                            {
                                mainRenderer = (SkinnedMeshRenderer)rendererInfo.renderer;
                                break;
                            }
                        }
                        if (mainRenderer != null)
                        {
                            characterModel.SetFieldValue<SkinnedMeshRenderer>("mainSkinnedMeshRenderer", mainRenderer);
                        }
                    }
                }
                return mainRenderer;
            }

            void CreateSkinInfo(SkinnedMeshRenderer mainRenderer)
            {
                ModelSkinController skinController = model.AddOrGetComponent<ModelSkinController>();

                LoadoutAPI.SkinDefInfo defaultSkinInfo = default(LoadoutAPI.SkinDefInfo);
                defaultSkinInfo.BaseSkins = Array.Empty<SkinDef>();
                defaultSkinInfo.GameObjectActivations = Array.Empty<SkinDef.GameObjectActivation>();
                defaultSkinInfo.Icon = LoadoutAPI.CreateSkinIcon(
                    //top - FILLED
                    new Color(215f / 255f, 83f / 255f, 65f / 255f),
                    //right - FILLED
                    new Color(137f / 255f, 207f / 255f, 240f / 255f),
                    //bottom - FILLED
                    new Color(70f / 255, 130f / 255f, 180f / 255f),
                    //left - FILLED
                    new Color(253f / 255f, 118f / 255f, 26f / 255f));

                defaultSkinInfo.MeshReplacements = new SkinDef.MeshReplacement[] {
                    new SkinDef.MeshReplacement {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                    }
                };
                defaultSkinInfo.MinionSkinReplacements = Array.Empty<SkinDef.MinionSkinReplacement>();
                defaultSkinInfo.Name = "DEFAULT_SKIN";
                defaultSkinInfo.NameToken = "DEFAULT_SKIN";
                defaultSkinInfo.ProjectileGhostReplacements = Array.Empty<SkinDef.ProjectileGhostReplacement>();
                defaultSkinInfo.RendererInfos = characterModel.baseRendererInfos;
                defaultSkinInfo.RootObject = model;
                defaultSkinInfo.UnlockableName = "";


                //, ));

                SkinDef defaultSkin = LoadoutAPI.CreateNewSkinDef(defaultSkinInfo);

                skinController.skins = new SkinDef[1]
                {
                defaultSkin,
                };
            }

            CreateSkinInfo(FixRenderInfo());
        }


        private void CreatePassives()
        {
            var passiveFamily = ScriptableObject.CreateInstance<SkillFamily>();

            var skillSlot = HANDBody.AddComponent<GenericSkill>();

            var armorDef = ScriptableObject.CreateInstance<SkillDef>();
            var regenDef = ScriptableObject.CreateInstance<SkillDef>();
            var speedDef = ScriptableObject.CreateInstance<SkillDef>();

            armorDef.activationState = new SerializableEntityStateType(typeof(Uninitialized));
            armorDef.activationStateMachineName = "Weapon";
            armorDef.baseMaxStock = 1;
            armorDef.baseRechargeInterval = 0f;
            armorDef.beginSkillCooldownOnSkillEnd = true;
            armorDef.canceledFromSprinting = false;
            armorDef.fullRestockOnAssign = true;
            armorDef.interruptPriority = InterruptPriority.Any;
            armorDef.isBullets = false;
            armorDef.isCombatSkill = true;
            armorDef.mustKeyPress = false;
            armorDef.noSprint = false;
            armorDef.rechargeStock = 1;
            armorDef.requiredStock = 1;
            armorDef.shootDelay = 0.1f;
            armorDef.stockToConsume = 0;
            armorDef.skillDescriptionToken = "HAND_PASSIVE_DESCRIPTION";
            armorDef.skillName = "Armor";
            armorDef.skillNameToken = "HAND_PASSIVE_NAME";
            armorDef.icon = AssetsCore.overclockIcon;
            armorDef.keywordTokens = Array.Empty<string>();
            armorDef.skillDescriptionToken = "HAND_PASSIVE_DESCRIPTION";
            armorDef.icon = AssetsCore.overclockIcon;
            armorDef.skillNameToken = "HAND_PASSIVE_NAME";

            API.CopySkillDefSettings(armorDef, regenDef);
            API.CopySkillDefSettings(armorDef, speedDef);

            regenDef.skillName = "Regen";
            regenDef.icon = null;
            regenDef.skillNameToken = "HAND_PASSIVE_ALT_NAME";
            regenDef.skillDescriptionToken = "HAND_PASSIVE_ALT_DESCRIPTION";

            speedDef.skillName = "Speed";
            speedDef.icon = null;
            speedDef.skillNameToken = "HAND_PASSIVE_ALT2_NAME";
            speedDef.skillDescriptionToken = "HAND_PASSIVE_ALT2_DESCRIPTION";

            passiveFamily.variants = new SkillFamily.Variant[1] {
                new SkillFamily.Variant {
                    skillDef = armorDef,
                    unlockableName = ""
                },
                /*new SkillFamily.Variant {
                    skillDef = regenDef,
                    unlockableName = ""
                },
                new SkillFamily.Variant {
                    skillDef = speedDef,
                    unlockableName = ""
                }*/

            };

            LoadoutAPI.AddSkillDef(armorDef);
            LoadoutAPI.AddSkillDef(regenDef);
            LoadoutAPI.AddSkillDef(speedDef);
            LoadoutAPI.AddSkillFamily(passiveFamily);

            skillSlot.SetFieldValue("_skillFamily", passiveFamily);

            passiveContoller.armorDef = armorDef;
            passiveContoller.regenDef = regenDef;
            passiveContoller.speedDef = speedDef;
            passiveContoller.passiveSkillSlot = skillSlot;
        }

        private void CreatePrimary()
        {
            //--FIXED--
            //Alright, here's the problem:
            //Interrupt priorities are weird, and because they are weird
            //they cause the primary to not fire off after the secondary is fired.
            //Obvious problem is obvious, but setting the primary's interrupt priority to PrioritySkill 
            //makes it so you can't hold it down. Extremely obvious problem is obvious, but I have no idea how's to fix this.
            //FIX: Apparently I forgot to call base.FixedUpdate(); in its FixedUpdate void, whoops!

            LoadoutAPI.AddSkill(typeof(FullSwing));
            LoadoutAPI.AddSkill(typeof(FullSwing2));
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
            primarySkillDef.skillDescriptionToken = "HAND_PRIMARY_DESCRIPTION";
            primarySkillDef.skillName = "HAND_PRIMARY_NAME";
            primarySkillDef.skillNameToken = "HAND_PRIMARY_NAME";
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

        private void CreateSecondary()
        {
            LoadoutAPI.AddSkill(typeof(ChargeHomeRun));
            LoadoutAPI.AddSkill(typeof(HomeRun));
            LoadoutAPI.AddSkill(typeof(AnimationTest));

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            secondarySkillDef.activationState = new SerializableEntityStateType(typeof(AnimationTest));
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
            LoadoutAPI.AddSkill(typeof(BeginOverclock));
            LoadoutAPI.AddSkill(typeof(ChargeBurst));
            LoadoutAPI.AddSkill(typeof(Burst));
            LoadoutAPI.AddSkill(typeof(ChargeSlam));
            LoadoutAPI.AddSkill(typeof(Slam));

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
            utilitySkillDef.skillDescriptionToken = "HAND_UTILITY_DESCRIPTION";
            utilitySkillDef.skillName = "aaa";
            utilitySkillDef.skillNameToken = "HAND_UTILITY_NAME";
            utilitySkillDef.icon = AssetsCore.forcedReassemblyIcon;
            utilitySkillDef.keywordTokens = new string[] {
                 "KEYWORD_AGILE",
                 "KEYWORD_CHARGEABLE",
                 "KEYWORD_CLOCKED"
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

        private void CreateSpecial()
        {
            LoadoutAPI.AddSkill(typeof(Drone));

            SkillDef specialSkillDef = ScriptableObject.CreateInstance<HANDDroneSkillDef>();
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

        private void InitSkillsStates()
        {
            ChargeSlam.baseDuration = 1f;
            ChargeSlam.effectPrefab = Resources.Load<GameObject>("prefabs/effects/muzzleflashes/MuzzleflashLoader");
            ChargeSlam.chargeEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleGuardDeathImpact");
            Slam.baseDuration = .5f;
            Slam.blastDamageCoefficient = 3;
            Slam.blastForce = 200f;
            Slam.blastRadius = 7;
            Slam.damageCoefficient = 3f;
            Slam.forceMagnitude = 1200;
            Slam.hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactToolbotDashLarge");
            Slam.rumbleEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleGuardDeathImpact");
            Slam.overclockRumbleEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
            Drone.baseDuration = 0.9f;
            //Charge burst used to have a duration. 
            //It doesn't anymore.
            //IT DOES NOW
            ChargeBurst.baseDuration = 0.2f;
            //Same with BURST
            //NOW IT DOES
            Burst.baseDuration = 0.2f;
            Burst.damageCoeff = 8;
        }
        #endregion
        #region Hooks
        private void Hook()
        {
            On.RoR2.CameraRigController.OnEnable += FixFadingInLobby;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void FixFadingInLobby(On.RoR2.CameraRigController.orig_OnEnable orig, CameraRigController rig)
        {
            if (SceneCatalog.GetSceneDefForCurrentScene().baseSceneName is "lobby") rig.enableFading = false;
            orig(rig);
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            //The baron is nobody's puppet!
            /*var hurt = self.GetComponent<SetStateOnHurt>();
            if (damageInfo.damageType == BonusToStunned && hurt && hurt.targetStateMachine.state.GetType() == typeof(StunState)) {
                damageInfo.damage *= 2;
                LogM("custom dmg type working!!");
            }*/
            if (self.body.HasBuff(BuffCore.instance.cleanIndex) && damageInfo.attacker && damageInfo.dotIndex == DotIndex.None)
            { //damageInfo.attacker.GetComponent<CharacterBody>().baseNameToken == "HAND_BODY_NAME") {
                damageInfo.damage *= 1.5f;
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/omnieffect/omniexplosionvfx"), new EffectData
                {
                    origin = self.transform.position,
                    scale = 10
                }, true);
                var FUCKTHEDOTCONTROLLER = damageInfo.attacker.GetComponent<HealthComponent>();
                if (FUCKTHEDOTCONTROLLER)
                {
                    FUCKTHEDOTCONTROLLER.HealFraction(0.25f, default);
                }
            }
            orig(self, damageInfo);
        }

        #endregion
        #region DamageTypes
        private void RegisterDamageTypes()
        {
            //BonusToStunned = DamageTypeExtension.AddDamageType();
        }

        #endregion
        #region Misc
        private void CreateTokens()
        {
            //LanguageAPI.Add("HAND_BODY_NAME", "HAN-D");
            LanguageAPI.Add("HAND_BODY_SUBTITLE", "Lean, Mean, Cleaning Machine");
            LanguageAPI.Add("KEYWORD_CHARGEABLE", "<style=cKeywordName>Chargeable</style><style=cSub>This skill can be held down to enhance its function.</style>");
            LanguageAPI.Add("KEYWORD_CLOCKED", "<style=cKeywordName>Overclocked</style><style=cSub>This has a chance to become stunning when overclock is on.</style>");
            LanguageAPI.Add("KEYWORD_CLEANS", "<style=cKeywordName>Cleaning</style><style=cSub>This skill cleans enemies, which deals damage overtime and doubles melee damage dealt to enemies.</style>");
            //LanguageAPI.Add("HAND_DESCRIPTION", Environment.NewLine + "NO MANUAL PROVIDED FOR SELECTED HAN-D UNIT.");

            LanguageAPI.Add("HAND_PASSIVE_NAME", "OVERCLOCK");
            LanguageAPI.Add("HAND_PASSIVE_DESCRIPTION", "<style=cIsDamage>USING MELEE SKILLS AND LANDING HITS</style> IN QUICK SUCCESSION ACTIVATES <style=cIsDamage>OVERCLOCK</style>.");

            //LanguageAPI.Add("HAND_PASSIVE_NAME", "SCRAP_COLLECTOR");
            //LanguageAPI.Add("HAND_PASSIVE_DESCRIPTION", "<style=cIsUtility>GENERATE SCRAP</style> WHEN HITTING AN ENEMY. <style=cIsUtility>TEN ARMOR</style> PER DRONE. MAXIMUM TEN SCRAP");

            /*LanguageAPI.Add("HAND_PASSIVE_ALT_NAME", "FLESH_COLLECTOR");
            LanguageAPI.Add("HAND_PASSIVE_ALT_DESCRIPTION", "<style=cIsUtility>LEECH FLESH</style> AFTER HITTING AN ENEMY. GAIN <style=cIsHealing>25% REGEN</style> PER FLESH. MAXIMUM POUNDS OF FLESH");

            LanguageAPI.Add("HAND_PASSIVE_ALT2_NAME", "OLI_COLLECTOR");
            */
            LanguageAPI.Add("HAND_PASSIVE_ALT2_DESCRIPTION", "<style=cIsUtility>GENERATE OLI</style> WHEN HITTING AN ENEMY. GAIN <style=cIsUtility>80% MOVE SPEED</style> PER PINT. MAXIMUM TEN PINTS OF OIL");

            LanguageAPI.Add("HAND_PRIMARY_NAME", "HURT");
            LanguageAPI.Add("HAND_PRIMARY_DESCRIPTION", "<style=cIsUtility>AGILE</style>. APPLY FORCE TO ALL COMBATANTS FOR <color=#E5C962>300% DAMAGE</color>. <style=cIsDamage>OVERCLOCKED</style>");

            LanguageAPI.Add("HAND_UTILITY_NAME", "FORCED_REASSEMBLY");
            LanguageAPI.Add("HAND_UTILITY_DESCRIPTION", "<style=cIsUtility>AGILE</style>. <style=cIsDamage>CHARGEABLE</style>. APPLY GREAT FORCE TO THE GROUND, DAMAGING NEARBY ENEMIES FOR <style=cIsDamage>400% DAMAGE</style>. <style=cIsDamage>OVERCLOCKED</style>");

            LanguageAPI.Add("HAND_SPECIAL_NAME", "DRONE");
            LanguageAPI.Add("HAND_SPECIAL_DESCRIPTION", "DEPLOY A <style=cIsDamage>CLEANING</style> DRONE THAT <style=cIsHealing>HEALS ON HIT</style> AND DOES <style=cIsDamage>300% DAMAGE</style>.");

            //LanguageAPI.Add("HAND_UTILITY_NAME", "EXPUNGE");
            //LanguageAPI.Add("HAND_UTILITY_DESCRIPTION", "<style=cIsUtility>AGILE</style>. EXPLUNGE HEAT SYSTEMS AND <style=cIsUtility>YOURSELF BOOST UPWARDS</style>.<style=cIsUtility> CHARGE TO BOOST YOURSELF FURTHER UPWARDS</style>");

            //LanguageAPI.Add("HAND_UTILITY_NAME", "OVERCLOCK");
            //LanguageAPI.Add("HAND_UTILITY_DESCRIPTION", "INCREASE <style=cIsDamage>ATTACK SPEED BY 100%</style> <style=cIsUtility>MOVE SPEED BY 40%</style>. <style=cIsDamage>EXTEND DURATION BY ATTACKING ENEMIES</style>.");
        }
        private void CreateSurvivorDef()
        {
            GameObject HANDDisplay = this.HANDBody.GetComponent<ModelLocator>().modelTransform.gameObject;
            string desc = "HAN-D is a strong bruiser who can dish out a beating and take one.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Activate OVERCLOCK and use FORCED_REASSEMBLY to gain health when in a pitch!" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Target enemies you've hit with DRONE to deal double damage!" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use DRONE to gain health when in a pitch!" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Hold FORCED_REASSEMBLY to charge it and boost yourself upwards!" + Environment.NewLine + Environment.NewLine;

            LanguageAPI.Add("HAND_DESCRIPTION", desc);
            LanguageAPI.Add("HAND_FLAVORTOKEN", "And so it left, humanity growing within closed circuits...");

            SurvivorDef def = new SurvivorDef()
            {
                bodyPrefab = this.HANDBody,
                descriptionToken = "HAND_DESCRIPTION",
                displayNameToken = "HAND_BODY_NAME",
                displayPrefab = HANDDisplay,
                name = "AAAAAAAAA",
                outroFlavorToken = "HAND_FLAVORTOKEN",
                primaryColor = new Color(0.4862745098f, 0.94901960784f, 0.71764705882f),
                unlockableName = "",
            };
            SurvivorAPI.AddSurvivor(def);
        }
        #endregion
    }
}
