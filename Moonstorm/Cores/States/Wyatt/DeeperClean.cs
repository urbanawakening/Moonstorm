using EntityStates;
using EntityStates.ParentMonster;
using RoR2;
using RoR2.Navigation;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace Moonstorm.Cores.States.Wyatt
{
    class DeeperClean : BaseSkillState
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
            characterMotor.velocity = new Vector3(characterMotor.velocity.x, 0, characterMotor.velocity.z);
            CreateBlinkEffect(Util.GetCorePosition(base.gameObject), characterMotor.moveDirection);

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.deltaTime;

            base.characterMotor.rootMotion += base.inputBank.aimDirection * (7 * 35 * Time.fixedDeltaTime);
            ShockEnemies();

            if (fixedAge >= baseDuration && isAuthority)
            {
                outer.SetNextStateToMain();
            };
        }
        public override void OnExit()
        {

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
            lightningOrb2.damageType = (DamageTypeCore.antiGrav | DamageType.Stun1s);
            HurtBox hurtBox2 = lightningOrb2.PickNextTarget(transform.position);
            if (hurtBox2)
            {
                lightningOrb2.target = hurtBox2;
                OrbManager.instance.AddOrb(lightningOrb2);
            }
        }

        private void CreateBlinkEffect(Vector3 origin, Vector3 direction)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(direction);
            effectData.origin = origin;
            EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, effectData, true);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
