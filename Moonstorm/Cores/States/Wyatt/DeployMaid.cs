using EntityStates;
using Moonstorm.Cores.HAND;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.States.Wyatt
{
    class DeployMaid : BaseSkillState
    {
        public static float baseDuration = 0.1f;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                FireProjectileInfo info = new FireProjectileInfo()
                {
                    crit = RollCrit(),
                    damage = 3 * damageStat,
                    damageColorIndex = RoR2.DamageColorIndex.Default,
                    damageTypeOverride = DamageType.Shock5s,
                    force = 2500,
                    owner = gameObject,
                    position = transform.position,
                    procChainMask = default,
                    projectilePrefab = ProjectileCore.wyattMaidBubble,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                    target = null,
                    useFuseOverride = false,
                    useSpeedOverride = false
                };
                ProjectileManager.instance.FireProjectile(info);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= baseDuration && isAuthority)
            {
                skillLocator.special.SetBaseSkill(WyattCore.instance.retrievePrimary);
                outer.SetNextStateToMain();
            };
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}