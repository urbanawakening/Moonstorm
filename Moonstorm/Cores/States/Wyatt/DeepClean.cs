using EntityStates;
using EntityStates.ParentMonster;
using RoR2;
using RoR2.Navigation;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace Moonstorm.Cores.States.Wyatt
{
    class DeepClean : BaseSkillState
    { 

        private float timer;

        public static float baseDuration = 0.1f;
        public override void OnEnter()
        {
            base.OnEnter();
            /*if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
            }
            if (this.characterModel)
            {
                this.characterModel.invisibilityCount++;
            }
            if (this.hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }*/
            if (base.characterMotor)
            {
                //base.characterMotor.enabled = false;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.deltaTime;

            base.characterMotor.rootMotion += base.inputBank.aimDirection * (this.moveSpeedStat * 35 * Time.fixedDeltaTime);
            ShockEnemies();

            if (fixedAge >= baseDuration && isAuthority)
            {
                outer.SetNextStateToMain();
            };
        }
        public override void OnExit()
        {
            Util.PlaySound(LoomingPresence.endSoundString, base.gameObject);
            if (base.characterDirection)
            {
                base.characterDirection.forward = base.GetAimRay().direction;
            }
            /*if (this.characterModel)
            {
                this.characterModel.invisibilityCount--;
            }
            if (this.hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            } */
            if (base.characterMotor)
            {
                //base.characterMotor.enabled = true;
            }
            base.OnExit();
        }

        public void ShockEnemies()
        {
            LightningOrb lightningOrb2 = new LightningOrb();
            lightningOrb2.origin = base.transform.position;
            lightningOrb2.damageValue = 1f * damageStat;
            lightningOrb2.isCrit = RollCrit();
            lightningOrb2.bouncesRemaining = 1;
            lightningOrb2.teamIndex = GetTeam();
            lightningOrb2.attacker = gameObject;
            lightningOrb2.bouncedObjects = new List<HealthComponent>
                            {
                                base.healthComponent//.GetComponent<HealthComponent>()
                            };
            lightningOrb2.procChainMask = default;
            lightningOrb2.procCoefficient = 1;
            lightningOrb2.lightningType = LightningOrb.LightningType.Ukulele;
            lightningOrb2.damageColorIndex = DamageColorIndex.Default;
            lightningOrb2.range += 10;
            lightningOrb2.damageType = DamageTypeCore.antiGrav;
            HurtBox hurtBox2 = lightningOrb2.PickNextTarget(transform.position);
            if (hurtBox2)
            {
                lightningOrb2.target = hurtBox2;
                OrbManager.instance.AddOrb(lightningOrb2);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
