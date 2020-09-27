

using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.States.Bombardier
{
    class ThrowStickies : BaseSkillState
    {

        public static float baseDuration = 1f;



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

            FireProjectileInfo info = new FireProjectileInfo()
            {
                crit = RollCrit(),
                damage = 1 * damageStat,
                damageColorIndex = RoR2.DamageColorIndex.Default,
                damageTypeOverride = DamageType.Stun1s,
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

        }

        public virtual GameObject GrabProjectile()
        {
            return ProjectileCore.stickyProjectile;
        }

        public virtual void ModifyProjectileInfo(FireProjectileInfo info)
        {
            //do nothing!
        }

        public virtual bool ShouldApplyBloom()
        {
            return true;
        }

        public virtual float GetBloom()
        {
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
