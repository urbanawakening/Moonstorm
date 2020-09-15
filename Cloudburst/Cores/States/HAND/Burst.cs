using Cloudburst.Cores.HAND.Components;
using EntityStates;
using RoR2;
using UnityEngine;

namespace Cloudburst.Cores.States.HAND
{
    public class Burst : BaseSkillState
    {
        public float chargeBonus;
        private float duration;
        public static float baseDuration;
        public static float damageCoeff;
        private DroneComponent droneComponent;

        public override void OnEnter()
        {
            duration = baseDuration / attackSpeedStat;
            droneComponent = base.gameObject.GetComponent<DroneComponent>();
            Util.PlaySound("Play_MULT_m2_main_explode", base.gameObject);
            /*if (droneComponent && characterBody.HasBuff(BuffCore.instance.overclockIndex)) {
                droneComponent.ConsumeDroneStackAuthority(4);
            }*/
            if (base.isAuthority)
            {
                characterMotor.ApplyForce(new Vector3(characterMotor.moveDirection.x, characterMotor.mass * (15 + chargeBonus), characterMotor.moveDirection.z), true, false);

                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/omnieffect/omniexplosionvfx"), new EffectData
                {
                    origin = base.transform.position,
                    scale = 10
                }, true);
                if (isGrounded)
                {
                    EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, new EffectData
                    {
                        origin = base.transform.position,
                        scale = 10
                    }, true);
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}