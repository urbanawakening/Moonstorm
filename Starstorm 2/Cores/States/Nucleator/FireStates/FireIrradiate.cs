using EntityStates;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores.States.Nucleator
{
    class FireIrradiate : BaseSkillState
    {
        public GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark");
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/critspark");
        public GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracerbanditshotgun");

        public static float minChargeDamageCoef = 1F;
        public static float maxChargeDamageCoef = 5F;
        public static float maxOverchargeDamageCoef = 9F;
        
        public float charge;

        private float damage;
        private float duration = 0.3f;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            CalculateDamage();
            Shoot();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        private void CalculateDamage()
        {
            float chargeCoef;
            float damageCoef;
            float overchargeThreshold = NucleatorSkillStateBase.overchargeThreshold;

            if (this.charge < overchargeThreshold)
            {
                chargeCoef = this.charge / overchargeThreshold;
                damageCoef = chargeCoef * (maxChargeDamageCoef - minChargeDamageCoef) + minChargeDamageCoef;
                this.damage = this.damageStat * damageCoef;
            }
            else
            {
                chargeCoef = (this.charge - overchargeThreshold) / (1 - overchargeThreshold);
                damageCoef = chargeCoef * (maxOverchargeDamageCoef - maxChargeDamageCoef) + maxChargeDamageCoef;
                this.damage = this.damageStat * damageCoef;
            }
        }

        private void Shoot()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                bool isCrit = base.RollCrit();

                Util.PlayScaledSound(EntityStates.Commando.CommandoWeapon.FirePistol.firePistolSoundString, base.gameObject, this.attackSpeedStat);
                base.characterBody.AddSpreadBloom(EntityStates.Commando.CommandoWeapon.FirePistol2.spreadBloomValue * 1.5f);
                Ray aimRay = base.GetAimRay();
                base.StartAimMode(aimRay, 2f, false);
                if (base.isAuthority)
                {
                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0f,
                        maxSpread = base.characterBody.spreadBloomAngle,
                        bulletCount = 1U,
                        procCoefficient = 1f,
                        damage = this.damage,
                        force = 3,
                        falloffModel = BulletAttack.FalloffModel.None,
                        tracerEffectPrefab = this.tracerEffectPrefab,
                        hitEffectPrefab = this.hitEffectPrefab,
                        isCrit = base.RollCrit(),
                        HitEffectNormal = false,
                        stopperMask = LayerIndex.world.mask,
                        smartCollision = true,
                        maxDistance = 300f
                    }.Fire();
                }
            }
        }
    }
}

