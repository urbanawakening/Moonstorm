using Cloudburst.Cores.States.Commando;
using Cloudburst.Cores.States.REX;
using EntityStates;
using EntityStates.Treebot.Weapon;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using UnityEngine;

namespace Cloudburst.Cores
{
    public class CommandoCore
    {
        public static CommandoCore instance;

        public static GameObject collectorObject;

        public CommandoCore() => Commando();

        protected void Commando() {
            instance = this;

            LogCore.LogI("Initializing Core: " + base.ToString());

            var commandoBody = Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");
            /*var body = commandoBody.GetComponent<CharacterBody>();

            body.baseMaxHealth = 110;
            body.baseRegen = 1;
            body.baseMaxShield = 0;
            body.baseMoveSpeed = 7;
            body.baseJumpPower = 15;
            body.baseDamage = 12;
            body.baseAttackSpeed = 1;
            body.baseCrit = 1;
            body.baseJumpCount = 1;
            body.sprintingSpeedMultiplier = 1.45f;
            //i *might* have forgotton about this
            //body.baseJumpCount = 33;
            body.levelRegen = 0.2f;
            body.levelMaxShield = 0;
            body.levelMoveSpeed = 0;
            body.levelJumpPower = 0;
            body.levelDamage = 2.4f;
            body.levelAttackSpeed = 0;
            body.levelCrit = 0;
            body.levelArmor = 0;*/
            //I wanted to do a rebalance but decided against it

            LoadoutAPI.AddSkill(typeof(CommandoGrenade));

            var locator = commandoBody.GetComponent<SkillLocator>();

            var toolbotDef = Resources.Load<SkillDef>("skilldefs/toolbotbody/ToolBotBodyStunDrone");
            var origDef = Resources.Load<SkillDef>("skilldefs/commandobody/ThrowGrenade");

            var def = locator.special.skillFamily.variants[1].skillDef;

            var icon = origDef.icon;
            var skillNameToken = origDef.skillNameToken;
            var skillDescriptionToken = origDef.skillDescriptionToken;

            API.CopySkillDefSettings(toolbotDef, def);

            def.icon = icon;
            def.skillNameToken = skillNameToken;
            def.skillDescriptionToken = skillDescriptionToken;
            def.baseMaxStock = 2;
            def.activationState = new EntityStates.SerializableEntityStateType(typeof(CommandoGrenade));
            def.keywordTokens = Array.Empty<String>(); 

            var grenade = Resources.Load<GameObject>("prefabs/projectiles/CommandoGrenadeProjectile");
            grenade.GetComponent<ProjectileImpactExplosion>().lifetimeAfterImpact = 0;
        }
    }
}