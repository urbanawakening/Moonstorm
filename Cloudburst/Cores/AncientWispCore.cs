using Cloudburst.Cores.States.AncientWisp;
using EntityStates;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System;
using UnityEngine;
using static RoR2.CharacterAI.AISkillDriver;

namespace Cloudburst.Cores
{
    class AncientWispCore
    {
        public static AncientWispCore instance;
        protected GameObject master;
        protected GameObject body;
        protected SkillLocator skillLocator;
        public static GameObject pillar;
        public AncientWispCore() => CreateAncientWisp();

        protected void CreateAncientWisp() {
            LogCore.LogI("Initializing Core: " + base.ToString());
            instance = this;

            master = Resources.Load<GameObject>("prefabs/charactermasters/AncientWispMaster");
            body = Resources.Load<GameObject>("prefabs/characterbodies/AncientWispBody");
            skillLocator = body.GetComponent<SkillLocator>();

            AI();
            Skills();

            BuildBody();
            Cloudburst.start += FixAncientWispPillarEffect;
        }
    
        private void FixAncientWispPillarEffect()
        {
            //this is insanity

            //what the fuck, yo?
            //ONE LINE AT A TIME

            //maybe this??
            pillar = EntityStates.AncientWispMonster.ChannelRain.delayPrefab; //.InstantiateClone("AncientWispPillarExplosion", true);

            //I think it might be this line that's causing things to be stretched out.
            //IT IS THIS
            //WHAT THE FUCK??
            var centralRing = pillar.transform.Find("Particles/Ring, Center");

            centralRing.gameObject.GetComponent<Renderer>().material = Resources.Load<GameObject>("prefabs/projectileghosts/AncientWispCannonGhost").transform.Find("Particles/FireSphere").GetComponent<Renderer>().material;
            //Gonna try this
            //IT DOESN'T WORK
            //IT DOESN'T FUCKING WOOOOOOOOOOOOOOOOOOOOOORK
            //pillar.transform.Find("Particles/Ring, Center").GetComponent<Renderer>().material = Resources.Load<GameObject>("prefabs/effects/impacteffects/AncientWispPillar").transform.Find("Particles/Flames, Tube, CenterHuge").GetComponent<Renderer>().material;

            centralRing.GetComponent<Renderer>().material = Resources.Load<GameObject>("prefabs/effects/impacteffects/AncientWispPillar").transform.Find("Particles/Flames, Tube, CenterHuge").GetComponent<Renderer>().material;

            //centralRing.gameObject.SetActive(false);


            //none of this worked
            //i have one last idea  
            //this somewhat works
            //ok so it stopped working.
            //why? dunno
            //another thing
            //var locator = pillar.AddComponent<ChildLocator>();
            //Resources.Load<GameObject>("prefabs/effects/impacteffects/AncientWispPillar").transform.Find("Particles/Flames, Tube, CenterHuge").GetComponent<Renderer>().material; 
            //one last thing 
            //there's a null reference somewhere in here
            //locator.FindChild("Ring, Center").GetComponent<Renderer>().material = Resources.Load<GameObject>("prefabs/projectileghosts/AncientWispCannonGhost").transform.Find("Particles/FireSphere").GetComponent<Renderer>().material;
        }

        protected void Skills() {

            API.CreateEmptySkills(body);
            CreatePrimary();
            CreateSecondary();
            CreateUtility();
            CreateSpecial();

        }

        private void BuildBody()
        {
            CharacterBody characterBody = body.GetComponent<CharacterBody>();
            if (characterBody)
            {
                LanguageAPI.Add("ANCIENTWISP_BODY_NAME", "Ancient Wisp");
                characterBody.baseAcceleration = 14f;
                characterBody.baseArmor = 0; //Base armor this character has, set to 20 if this character is melee 
                characterBody.baseAttackSpeed = 1; //Base attack speed, usually 1
                characterBody.baseCrit = 0;  //Base crit, usually one
                characterBody.baseDamage = 40; //Base damage
                characterBody.baseJumpCount = 1; //Base jump amount, set to 2 for a double jump. 
                characterBody.baseJumpPower = 0; //Base jump power
                characterBody.baseMaxHealth = 2100; //Base health, basically the health you have when you start a new run
                characterBody.baseMaxShield = 0; //Base shield, basically the same as baseMaxHealth but with shields
                characterBody.baseMoveSpeed = 7; //Base move speed, this is usual 7
                characterBody.baseNameToken = "ANCIENTWISP_BODY_NAME"; //The base name token. 
                //characterBody.subtitleNameToken = "Hidden summoner"; //Set this to true if its a boss
                characterBody.baseRegen = 0; //Base health regen.
                characterBody.bodyFlags = (CharacterBody.BodyFlags.None); ///Base body flags, should be self explanatory 
                characterBody.crosshairPrefab = characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/HuntressBody").GetComponent<CharacterBody>().crosshairPrefab; //The crosshair prefab.
                characterBody.hideCrosshair = false; //Whether or not to hide the crosshair
                characterBody.hullClassification = HullClassification.Golem; //The hull classification, usually used for AI
                characterBody.isChampion = true; //Set this to true if its A. A boss or B. A miniboss
                characterBody.levelArmor = 0; //Armor gained when leveling up. 
                characterBody.levelAttackSpeed = 0; //Attackspeed gained when leveling up. 
                characterBody.levelCrit = 0; //Crit chance gained when leveling up. 
                characterBody.levelDamage = 8f; //Damage gained when leveling up. 
                characterBody.levelArmor = 0; //Armor gained when leveling up. 
                characterBody.levelJumpPower = 0; //Jump power gained when leveling up. 
                characterBody.levelMaxHealth = 630; //Health gained when leveling up. 
                characterBody.levelMaxShield = 0; //Shield gained when leveling up. 
                characterBody.levelMoveSpeed = 0; //Move speed gained when leveling up. 
                characterBody.levelRegen = 0f; //Regen gained when leveling up. 
                                               //characterBody.portraitIcon = portrait; //The portrait icon, shows up in multiplayer and the death UI
                //characterBody.preferredPodPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/robocratepod"); //The pod prefab this survivor spawns in. Options: Resources.Load<GameObject>("prefabs/networkedobjects/robocratepod"); Resources.Load<GameObject>("prefabs/networkedobjects/survivorpod"); 
            }
        }


        private void CreatePrimary()
        {
            LoadoutAPI.AddSkill(typeof(ChargeCannon));
            LoadoutAPI.AddSkill(typeof(FireCannon));

            SkillDef primarySkillDef = ScriptableObject.CreateInstance<SkillDef>();

            primarySkillDef.activationState = new SerializableEntityStateType(typeof(ChargeCannon));
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
            //LoadoutAPI.AddSkill(typeof(ChargeHomeRun));

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            secondarySkillDef.activationState = new SerializableEntityStateType(typeof(ChargeCannon));
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
            secondarySkillDef.skillDescriptionToken = "aaaaaa";
            secondarySkillDef.skillName = "aaa";
            secondarySkillDef.skillNameToken = "aaaaaaaaaaaaaaaaaa";
            secondarySkillDef.icon = null;
            secondarySkillDef.keywordTokens = Array.Empty<string>();

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
            LoadoutAPI.AddSkill(typeof(StartRain));
            LoadoutAPI.AddSkill(typeof(RAIN));
            LoadoutAPI.AddSkill(typeof(EndRain));

            SkillDef utilitySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            utilitySkillDef.activationState = new SerializableEntityStateType(typeof(StartRain));
            utilitySkillDef.activationStateMachineName = "Weapon";
            utilitySkillDef.baseMaxStock = 1;
            utilitySkillDef.baseRechargeInterval = 15f;
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
            //LoadoutAPI.AddSkill(typeof(Drone));

            SkillDef specialSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            specialSkillDef.activationState = new SerializableEntityStateType(typeof(ChargeCannon));
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

            //var follow = master.AddComponent<AISkillDriver>();

            var chaseEnemy = master.AddComponent<AISkillDriver>();

            var strafe = master.AddComponent<AISkillDriver>();

            //var primary = master.AddComponent<AISkillDriver>();

            var pillar = master.AddComponent<AISkillDriver>();

            strafe.customName = "StrafeEnemy";
            strafe.skillSlot = SkillSlot.Primary ;
            strafe.requireSkillReady = false;
            strafe.requireEquipmentReady = false;
            strafe.minUserHealthFraction = float.NegativeInfinity;
            strafe.maxUserHealthFraction = float.PositiveInfinity;
            strafe.minTargetHealthFraction = float.NegativeInfinity;
            strafe.maxTargetHealthFraction = float.PositiveInfinity;
            strafe.minDistance = 0;
            strafe.maxDistance = 20;
            strafe.selectionRequiresTargetLoS = false;
            strafe.selectionRequiresOnGround = false;
            strafe.moveTargetType = TargetType.CurrentEnemy;
            strafe.activationRequiresTargetLoS = false;
            strafe.activationRequiresAimConfirmation = false;
            strafe.movementType = MovementType.StrafeMovetarget;
            strafe.moveInputScale = 1;
            strafe.aimType = AimType.AtMoveTarget;
            strafe.ignoreNodeGraph = false;
            strafe.shouldSprint = true;
            strafe.shouldFireEquipment = false;
            strafe.shouldTapButton = false;
            strafe.buttonPressType = ButtonPressType.Hold;
            strafe.driverUpdateTimerOverride = -1;
            strafe.resetCurrentEnemyOnNextDriverSelection = false;
            strafe.noRepeat = false;

            chaseEnemy.customName = "ChaseEnemy";
            chaseEnemy.skillSlot = SkillSlot.Utility;
            chaseEnemy.requireSkillReady = false;
            chaseEnemy.requireEquipmentReady = false;
            chaseEnemy.minUserHealthFraction = float.NegativeInfinity;
            chaseEnemy.maxUserHealthFraction = float.PositiveInfinity;
            chaseEnemy.minTargetHealthFraction = float.NegativeInfinity;
            chaseEnemy.maxTargetHealthFraction = float.PositiveInfinity;
            chaseEnemy.minDistance = 0;
            chaseEnemy.maxDistance = float.PositiveInfinity;
            chaseEnemy.selectionRequiresTargetLoS = false;
            chaseEnemy.selectionRequiresOnGround = false;
            chaseEnemy.moveTargetType = TargetType.CurrentEnemy;
            chaseEnemy.activationRequiresTargetLoS = false;
            chaseEnemy.activationRequiresAimConfirmation = false;
            chaseEnemy.movementType = MovementType.ChaseMoveTarget;
            chaseEnemy.moveInputScale = 1;
            chaseEnemy.aimType = AimType.AtMoveTarget;
            chaseEnemy.ignoreNodeGraph = false;
            chaseEnemy.shouldSprint = true;
            chaseEnemy.shouldFireEquipment = false;
            chaseEnemy.shouldTapButton = false;
            chaseEnemy.buttonPressType = ButtonPressType.Hold;
            chaseEnemy.driverUpdateTimerOverride = -1;
            chaseEnemy.resetCurrentEnemyOnNextDriverSelection = false;
            chaseEnemy.noRepeat = false;

            /*primary.customName = "FireBlazingShot";
            primary.skillSlot = SkillSlot.Primary;
            //primary.requiredSkill = CaptainTazer(RoR2.Skills.SkillDef)
            primary.requireSkillReady = true;
            primary.requireEquipmentReady = false;
            primary.minUserHealthFraction = float.NegativeInfinity;
            primary.maxUserHealthFraction = float.PositiveInfinity;
            primary.minTargetHealthFraction = float.NegativeInfinity;
            primary.maxTargetHealthFraction = float.PositiveInfinity;
            primary.minDistance = 0;
            primary.maxDistance = 14;
            primary.selectionRequiresTargetLoS = true;
            primary.selectionRequiresOnGround = false;
            primary.moveTargetType = TargetType.CurrentEnemy;
            primary.activationRequiresTargetLoS = false;
            primary.activationRequiresAimConfirmation = false;
            primary.movementType = MovementType.StrafeMovetarget;
            primary.moveInputScale = 1;
            primary.aimType = AimType.AtMoveTarget;
            primary.ignoreNodeGraph = false;
            primary.shouldSprint = false;                                                                               
            primary.shouldFireEquipment = false;
            primary.shouldTapButton = false;
            primary.buttonPressType = ButtonPressType.Hold;
primary.driverUpdateTimerOverride = -1;
            primary.resetCurrentEnemyOnNextDriverSelection = false;
            primary.noRepeat = false;*/

            pillar.skillSlot = SkillSlot.Utility;
            pillar.requireSkillReady = false;
            pillar.requireEquipmentReady = false;
            pillar.moveTargetType = TargetType.CurrentEnemy;
            pillar.minUserHealthFraction = float.NegativeInfinity;
            pillar.maxUserHealthFraction = float.PositiveInfinity;
            pillar.minTargetHealthFraction = float.NegativeInfinity;
            pillar.maxTargetHealthFraction = float.PositiveInfinity;
            pillar.minDistance = 0;
            pillar.maxDistance = 30;
            pillar.selectionRequiresTargetLoS = false;
            pillar.activationRequiresTargetLoS = false;
            pillar.activationRequiresAimConfirmation = false;
            pillar.movementType = MovementType.ChaseMoveTarget;
            pillar.moveInputScale = 1;
            pillar.aimType = AimType.AtMoveTarget;
            pillar.ignoreNodeGraph = true;
            pillar.driverUpdateTimerOverride = -1;
            pillar.resetCurrentEnemyOnNextDriverSelection = false;
            pillar.noRepeat = false;
            pillar.shouldSprint = false;
            pillar.shouldFireEquipment = false;
            pillar.buttonPressType = ButtonPressType.Hold;

            /*follow.skillSlot = SkillSlot.None;
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
            strafe.noRepeat = false;*/
        }
    }
}
