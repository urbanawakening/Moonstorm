using System;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Cores.States.Bombardier
{
    public class FireRocket : BaseSkillState
    {
        public static GameObject projectilePrefab = ProjectileCore.bombardierBombProjectile;
        public static GameObject effectPrefab = EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab;

        public static DamageType damageType = DamageType.Generic;
        public static float damageCoeff = 3.2f;
        public static float procCoeff = 1;
        public static float force = 5000;
        public static float baseDuration = .9f;

        public static float timeBeforeKnockback;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            base.StartAimMode(2f, false);
            if (base.isAuthority)
            {
                FireProjectile();
            }
        }

        public virtual void FireProjectile()
        {
            var aimRay = GetAimRay();

            timeBeforeKnockback += Time.deltaTime;



            FireProjectileInfo info = new FireProjectileInfo()
            {
                crit = RollCrit(),
                damage = damageCoeff * damageStat,
                damageColorIndex = RoR2.DamageColorIndex.Default,
                damageTypeOverride = damageType,
                force = 1500,
                owner = gameObject,
                position = transform.position,
                procChainMask = default,
                projectilePrefab = GrabProjectile(),
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                target = null,
                useFuseOverride = false,
                useSpeedOverride = false
            };
            ModifyProjectileInfo(info);
            ProjectileManager.instance.FireProjectile(info);

            if (ShouldHaveKnockback() && !isGrounded && characterMotor)
            {
                characterMotor.ApplyForce(GetKnockback(aimRay), false, false);
            }

            if (characterBody && ShouldApplyBloom())
            {
                characterBody.AddSpreadBloom(GetBloom());
            }
        }

        public virtual bool ShouldHaveKnockback() {
            return true;
        }

        public virtual GameObject GrabProjectile()
        {
            return ProjectileCore.bombardierBombProjectile;
        }

        public virtual void ModifyProjectileInfo(FireProjectileInfo info) {
            //do nothing!
        }

        public virtual Vector3 GetKnockback(Ray aimRay) {
            return aimRay.direction * (-1000);
        }

        public virtual bool ShouldApplyBloom() {
            return true;
        }

        public virtual float GetBloom() {
            return 100;
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
            return InterruptPriority.Skill;
        }
    }
}