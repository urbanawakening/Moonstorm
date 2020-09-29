using EntityStates;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores.States.Nucleator
{
    class FireQuarantine : BaseSkillState
    {
        public static float minChargeForceCoef = 3F;
        public static float maxChargeForceCoef = 5F;
        public static float maxOverchargeDistanceCoef = 8F;
        public static float forceBase = 15F;
        private static float duration = 0.5f;
        
        public float charge;
        
        private float force;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            CalculateForce();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= FireQuarantine.duration / 2)
            {
                Shoot();
            }
            if (base.fixedAge >= FireQuarantine.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        private void CalculateForce()
        {
            float chargeCoef;
            float forceCoef;
            float overchargeThreshold = NucleatorSkillStateBase.overchargeThreshold;

            if (this.charge < overchargeThreshold)
            {
                chargeCoef = this.charge / overchargeThreshold;
                forceCoef = chargeCoef * (maxChargeForceCoef - minChargeForceCoef) + minChargeForceCoef;
                this.force = forceCoef * FireQuarantine.forceBase;
            }
            else
            {
                chargeCoef = (this.charge - overchargeThreshold) / (1 - overchargeThreshold);
                forceCoef = chargeCoef * (maxOverchargeDistanceCoef - maxChargeForceCoef) + maxChargeForceCoef;
                this.force = forceCoef * FireQuarantine.forceBase;
            }
        }

        private void Shoot()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                bool isCrit = base.RollCrit();

                Util.PlayScaledSound(EntityStates.Commando.CommandoWeapon.FirePistol.firePistolSoundString, base.gameObject, this.attackSpeedStat);

                if (base.isAuthority)
                {
                    var aimRay = this.GetAimRay();
                    Collider[] colliders = Physics.OverlapSphere(base.characterBody.corePosition, 25f, LayerIndex.defaultLayer.mask);
                    foreach(Collider col in colliders)
                    {
                        Vector3 vectorToCollider = (col.transform.position - base.characterBody.corePosition).normalized;
                        if (Vector3.Dot(vectorToCollider, aimRay.direction) > 0.5)
                        {
                            var force = col.GetComponent<Rigidbody>().mass * this.force;
                            col.GetComponent<HealthComponent>().TakeDamage(new DamageInfo()
                            {
                                damage = this.damageStat * 3.5f,
                                attacker = base.gameObject,
                                crit = isCrit,
                                position = aimRay.origin,
                                force = vectorToCollider * force,
                                procCoefficient = 1f,
                                damageType = DamageType.Generic | DamageType.Stun1s,
                                damageColorIndex = DamageColorIndex.Default,
                                dotIndex = DotController.DotIndex.None
                            });
                        } 
                    }
                }
            }
        }
    }
}

