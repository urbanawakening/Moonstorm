using Cloudburst.Cores.ArchaicWisp.Skills;
using R2API;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using static R2API.DirectorAPI;

namespace Cloudburst.Cores
{
    //TODO:
    //Make an icon.
    internal sealed class ArchaicWispCore
    {
        public GameObject archWisp;
        private CharacterSpawnCard archWispCharacterSpawnCard;
        private DirectorCard archWispDirectorCard;
        private DirectorCardHolder archWispDirectorCardHolder;
        public ArchaicWispCore() => CreateArchaicWisps();

        private void CreateArchaicWisps() {
            archWisp = Resources.Load<GameObject>("prefabs/characterbodies/archwispbody");

            BuildBody();
            BuildDirectorCard();
            ModifyPrimary();

            LogCore.LogM("Built Archaic Wisps!");
        }

        private void BuildDirectorCard()
        {
            archWispCharacterSpawnCard = Resources.Load<CharacterSpawnCard>("spawncards/characterspawncards/cscarchwisp");

            archWispDirectorCardHolder = new DirectorAPI.DirectorCardHolder();
            archWispCharacterSpawnCard.directorCreditCost = 250;
            archWispCharacterSpawnCard.forbiddenAsBoss = true;

            archWispDirectorCard = new DirectorCard
            {
                allowAmbushSpawn = true,
                forbiddenUnlockable = "",
                minimumStageCompletions = 4,
                preventOverhead = false,
                requiredUnlockable = "",
                selectionWeight = 1,
                spawnCard = archWispCharacterSpawnCard,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
            };

            archWispDirectorCardHolder.Card = archWispDirectorCard;
            archWispDirectorCardHolder.InteractableCategory = InteractableCategory.None;
            archWispDirectorCardHolder.MonsterCategory = DirectorAPI.MonsterCategory.Minibosses;

            MonsterActions += delegate (List<DirectorCardHolder> list, DirectorAPI.StageInfo stage)
            {
                if (!list.Contains(archWispDirectorCardHolder) && stage.stage == DirectorAPI.Stage.SkyMeadow || stage.stage == DirectorAPI.Stage.AbandonedAqueduct || stage.stage == DirectorAPI.Stage.RallypointDelta || stage.stage == DirectorAPI.Stage.ScorchedAcres)
                {
                    list.Add(archWispDirectorCardHolder);

                }
            };
        }

        private void BuildBody()
        {
            CharacterBody characterBody = archWisp.GetComponent<CharacterBody>();
            if (characterBody)
            {
                LanguageAPI.Add("ARCHAICWISP_BODY_NAME", "Archaic Wisp");
                characterBody.baseAcceleration = 14f;
                characterBody.baseArmor = 0; //Base armor this character has, set to 20 if this character is melee 
                characterBody.baseAttackSpeed = 1; //Base attack speed, usually 1
                characterBody.baseCrit = 0;  //Base crit, usually one
                characterBody.baseDamage = 30; //Base damage
                characterBody.baseJumpCount = 1; //Base jump amount, set to 2 for a double jump. 
                characterBody.baseJumpPower = 0; //Base jump power
                characterBody.baseMaxHealth = 3000; //Base health, basically the health you have when you start a new run
                characterBody.baseMaxShield = 0; //Base shield, basically the same as baseMaxHealth but with shields
                characterBody.baseMoveSpeed = 7; //Base move speed, this is usual 7
                characterBody.baseNameToken = "ARCHAICWISP_BODY_NAME"; //The base name token. 
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
                characterBody.levelDamage = 5f; //Damage gained when leveling up. 
                characterBody.levelArmor = 0; //Armor gained when leveling up. 
                characterBody.levelJumpPower = 0; //Jump power gained when leveling up. 
                characterBody.levelMaxHealth = 300; //Health gained when leveling up. 
                characterBody.levelMaxShield = 0; //Shield gained when leveling up. 
                characterBody.levelMoveSpeed = 0; //Move speed gained when leveling up. 
                characterBody.levelRegen = 0f; //Regen gained when leveling up. 
                                               //characterBody.portraitIcon = portrait; //The portrait icon, shows up in multiplayer and the death UI
                                               //characterBody.preferredPodPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/robocratepod"); //The pod prefab this survivor spawns in. Options: Resources.Load<GameObject>("prefabs/networkedobjects/robocratepod"); Resources.Load<GameObject>("prefabs/networkedobjects/survivorpod"); 
            }
        }
        private void ModifyPrimary()
        {
            SkillLocator skillLocator = archWisp.GetComponent<SkillLocator>();
            if (skillLocator)
            {
                LoadoutAPI.AddSkill(typeof(Summon));
                LoadoutAPI.AddSkill(typeof(ChargeSummon));
                SkillFamily skillFamily = skillLocator.primary.skillFamily;
                SkillDef primary = skillFamily.variants[skillFamily.defaultVariantIndex].skillDef;

                primary.rechargeStock = 1;
                primary.shootDelay = .3f;
                primary.stockToConsume = 1;
                primary.baseMaxStock = 3;
                primary.baseRechargeInterval = 5;
                primary.activationState = new EntityStates.SerializableEntityStateType(typeof(ChargeSummon));
            }
        }
    }
}
