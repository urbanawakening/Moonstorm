using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.States.Wyatt
{
    class FireWinch : BaseSkillState
    {
        public static float baseDuration = 0.1f;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                crit = base.RollCrit(),
                damage = this.damageStat * 2,
                damageColorIndex = DamageColorIndex.Default,
                force = 0f,
                owner = base.gameObject,
                position = aimRay.origin,
                procChainMask = default(ProcChainMask),
                projectilePrefab = ProjectileCore.winch,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                target = null,
                useSpeedOverride = true,
                speedOverride = 150
            };
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= baseDuration && isAuthority)
            {
                outer.SetNextStateToMain();
            };
        }
    }
}