using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudburst.Cores.Engineer;
using Cloudburst.Cores.Engineer.ETStates;
using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace Cloudburst.Cores
{
    class EngineerCore
    {
        //TODO
        //Fix skins
        //Tweak primary
        protected GameObject flameTurret;
        public static GameObject flameTurretMaster;
        public static GameObject projectileGameObject;
        protected GameObject engineerObject;
        public EngineerCore() => EngineerEdits();


        protected void EngineerEdits() {
            engineerObject = Resources.Load<GameObject>("prefabs/characterbodies/EngiBody");
            RegisterNewTurret();
            AddNewSpecial();
            AddNewPrimary();
            CreateAltPrimaryProjectile();
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
            //IL.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
        }

        private void CreateAltPrimaryProjectile() {
            projectileGameObject = Resources.Load<GameObject>("prefabs/projectiles/EngiGrenadeProjectile").InstantiateClone("EngiGrenadeImpactProjectile", true);
            API.RegisterNewProjectile(projectileGameObject);
            ProjectileImpactExplosion impact = projectileGameObject.GetComponent<ProjectileImpactExplosion>();
            impact.destroyOnWorld = true;
            UnityEngine.Object.Destroy(projectileGameObject.GetComponent<ProjectileStickOnImpact>());
            
        }
        private int CharacterMaster_GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot)
        {
            bool engi = slot == DeployableSlot.EngiTurret;
            if (!engi) return orig(self, slot);
            else {
                //Reflection will never fail me!
                GameObject fuckIL = self.GetPropertyValue<GameObject>("bodyInstanceObject");
                if (fuckIL) {
                    if (fuckIL.GetComponent<SkillLocator>().special.skillDef.skillName == "FUCKIL") {
                        return 4;
                    }
                    return 2;
                }
                return 2;
            }
        }

        private void ILCharacterMaster_GetDeployableSameSlotLimit(ILContext il)
        {
            //FUCK THIS I'M GOING THE DESTRUCTIVE ROUTE
            ILCursor c = new ILCursor(il);
            int AAAA = 2;
            int otherAA = 0;
            c.GotoNext(
                //IL_0067: ldc.i4.2
                x => x.MatchLdcI4(0),
                //IL_0068: stloc.0   
                x => x.MatchStloc(out otherAA)
                );
            c.Index += 0;


            c.GotoNext(
                //IL_0067: ldc.i4.2
                x => x.MatchLdcI4(2),
                //IL_0068: stloc.0   
                x => x.MatchStloc(out AAAA)
                //x => x.MatchStloc(0)
                //we don't talk about this one
                //x => x.Next.Operand = (object)4,
                );
            c.Index += 16;

            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldloc, AAAA);
            c.Emit(OpCodes.Ldloc, otherAA);

            c.EmitDelegate<Func<CharacterMaster, int>>((cm) =>
            {
                return 2;
            });

            /*c.Remove();
            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Func<CharacterMaster, int>>((cm) =>
            {
                return 2;
            });*/

            //LogCore.LogI(c);
            //c.Emit(OpCodes.Stloc_0);

            //c.Next.Operand = 100;
            /*c.GotoNext(
                //BEFORE CALLVIRT
                x => x.MatchLdarg(0),
                x => x.MatchCall<CharacterMaster>("get_bodyInstanceObject"),
                //do later - done at 10:44am 
               x => x.MatchCallvirt<GameObject>("GetComponent"),
                x => x.MatchLdfld<GenericSkill>("secondary"),
                //another callvirt. boy oh boy.
                x => x.MatchCallvirt<GenericSkill>("get_maxStock"),
                x => x.MatchStloc(4)
                );
            c.Index += 4;*/
        }

        protected void RegisterNewTurret() {
            flameTurret = Resources.Load<GameObject>("prefabs/characterbodies/EngiWalkerTurretBody").InstantiateClone("EngiFlameTurretBody", true);
            API.RegisterNewBody(flameTurret);
            flameTurretMaster = Resources.Load<GameObject>("prefabs/charactermasters/EngiWalkerTurretMaster").InstantiateClone("EngiFlameTurretMaster", true);
            API.RegisterNewMaster(flameTurretMaster);

            flameTurretMaster.GetComponent<CharacterMaster>().bodyPrefab = flameTurret;
            FlameTurrets();
        }
        protected void AddNewSpecial()
        {
            SkillDef origDef = Resources.Load<SkillDef>("skilldefs/engibody/EngiBodyPlaceWalkerTurret");
            var newDef = ScriptableObject.CreateInstance<SkillDef>();

            API.CopySkillDefSettings(origDef, newDef);

            LoadoutAPI.AddSkill(typeof(PlaceFlameTurret));
            LoadoutAPI.AddSkillDef(newDef);

            LanguageAPI.Add("ENGI_SPECIAL_ALT2_NAME", "TR27 Immolator Turret");
            LanguageAPI.Add("ENGI_SPECIAL_ALT2_DESCRIPTION", "Place a <style=cIsUtility>mobile</style> turret that <style=cIsUtility>inherits all your items.</style> Fires a plume of flame for <style=cIsDamage>60% damage per second</style> that <style=cIsDamage>ignites enemies</style>. <style=cIsDamage>Explodes on death for 3x</style> maximum health. Can place up to 4.");

            newDef.baseRechargeInterval = 15;
            newDef.activationState = new SerializableEntityStateType(typeof(PlaceFlameTurret));
            newDef.baseMaxStock = 4;
            newDef.skillName = "FUCKIL";
            newDef.skillNameToken = "ENGI_SPECIAL_ALT2_NAME";
            newDef.skillDescriptionToken = "ENGI_SPECIAL_ALT2_DESCRIPTION";

            var locator = engineerObject.GetComponent<SkillLocator>();

            var family = locator.special.skillFamily;

            SkillFamily.Variant variant = new SkillFamily.Variant();

            variant.skillDef = newDef;
            variant.unlockableName = "";

            int prevLength = family.variants.Length;
            Array.Resize<SkillFamily.Variant>(ref family.variants, prevLength + 1);
            family.variants[prevLength] = variant;
        }

        protected void AddNewPrimary()
        {
            SkillDef origDef = Resources.Load<SkillDef>("skilldefs/engibody/EngiBodyFireGrenade");
            var newDef = ScriptableObject.CreateInstance<SkillDef>();

            API.CopySkillDefSettings(origDef, newDef);

            LoadoutAPI.AddSkill(typeof(FireVolley));
            LoadoutAPI.AddSkillDef(newDef);

            LanguageAPI.Add("ENGINEER_PRIMARY_ALT_NAME", "Impact Grenades");

            newDef.activationState = new SerializableEntityStateType(typeof(FireVolley));
            newDef.skillName = "GrenadeVolley";
            newDef.skillNameToken = "ENGINEER_PRIMARY_ALT_NAME";
            //newDef.beginSkillCooldownOnSkillEnd = true;
            //newDef.baseRechargeInterval = .1f;
            newDef.canceledFromSprinting = false;
            //newDef.noSprint = false;
            
                
            var locator = engineerObject.GetComponent<SkillLocator>();

            var family = locator.primary.skillFamily;

            SkillFamily.Variant variant = new SkillFamily.Variant();

            variant.skillDef = newDef;
            variant.unlockableName = "";

            int prevLength = family.variants.Length;
            Array.Resize<SkillFamily.Variant>(ref family.variants, prevLength + 1);
            family.variants[prevLength] = variant;
        }


        protected void FlameTurrets() {
            var locator = flameTurret.GetComponent<SkillLocator>();
            LoadoutAPI.AddSkill(typeof(FireFlameThrower));
            locator.primary.skillFamily.variants[locator.primary.skillFamily.defaultVariantIndex].skillDef.activationState = new SerializableEntityStateType(typeof(FireFlameThrower));

            var characterDeathBehavior = flameTurret.GetComponent<CharacterDeathBehavior>();
            var body = flameTurret.GetComponent<CharacterBody>();

            body.baseMaxHealth = 70;
            body.levelMaxHealth = 21;

            //KILL
            LoadoutAPI.AddSkill(typeof(DeathState));
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(DeathState));
        }
    }
}
