using EntityStates;
using EntityStates.Engi.EngiWeapon;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Starstorm2.Cores.States.Nucleator
{
    class FireIrradiate : BaseSkillState
    {
        private GameObject nucleatorProjectile = NucleatorCore.instance.primaryProjectile;

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
                base.StartAimMode(aimRay, 1f, false);
                if (base.isAuthority)
                {
                    var baseRadius = this.charge * 10;
                    this.nucleatorProjectile.GetComponent<NucleatorProjectile>().baseRadius = baseRadius;
                    this.nucleatorProjectile.GetComponent<NucleatorProjectile>().charge = this.charge;

                    this.nucleatorProjectile.GetComponent<ProjectileDamage>().damage = this.damage;

                    if(this.charge > NucleatorSkillStateBase.overchargeThreshold)
                    {
                        this.nucleatorProjectile.GetComponent<ProjectileDirectionalTargetFinder>().enabled = true;
                        this.nucleatorProjectile.GetComponent<ProjectileSteerTowardTarget>().enabled = true;
                    }
                    else
                    {
                        this.nucleatorProjectile.GetComponent<ProjectileDirectionalTargetFinder>().enabled = false;
                        this.nucleatorProjectile.GetComponent<ProjectileSteerTowardTarget>().enabled = false;
                    }

                    FireProjectileInfo info = new FireProjectileInfo()
                    {
                        crit = RollCrit(),
                        damage = 0f,
                        damageColorIndex = RoR2.DamageColorIndex.Default,
                        damageTypeOverride = DamageType.Generic,
                        force = 0f,
                        owner = gameObject,
                        position = aimRay.origin + aimRay.direction.normalized,
                        procChainMask = default,
                        projectilePrefab = NucleatorCore.instance.primaryProjectile,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        target = null,
                        useFuseOverride = false,
                        useSpeedOverride = false                        
                    };
                    ProjectileManager.instance.FireProjectile(info);
                }
            }
        }
    }
}

