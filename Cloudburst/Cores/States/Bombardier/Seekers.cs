using Cloudburst.Cores.Components;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Cores.States.Bombardier
{
    class Mortar : FireRocket
    {
        public override GameObject GrabProjectile()
        {
            return ProjectileCore.bombardierSeekerBombProjectile;
        }
        public override void FireProjectile()
        {
            /*var aimRay = GetAimRay();

            timeBeforeKnockback += Time.deltaTime;

            if (ShouldHaveKnockback() && !isGrounded && characterMotor)
            {
                characterMotor.ApplyForce(GetKnockback(aimRay), false, false);
            }*/

            var tracker = gameObject.GetComponent<BombardierTracker>();
            var target = tracker.GetTrackingTarget();

            var projectile = GrabProjectile();
            for (float num = 0f; num < 9f; num += 1f)
            {
                float num2 = 6.2831855f;
                Vector3 forward = new Vector3(Mathf.Cos(num / 9f * num2), 0f, Mathf.Sin(num / 9f * num2));

                FireProjectileInfo info = new FireProjectileInfo()
                {
                    crit = RollCrit(),
                    damage = 2.5f * damageStat,
                    damageColorIndex = RoR2.DamageColorIndex.Default,
                    damageTypeOverride = damageType,
                    force = 100,
                    owner = gameObject,
                    position = transform.position,
                    procChainMask = default,
                    projectilePrefab = projectile,
                    rotation = Util.QuaternionSafeLookRotation(forward),
                    target = target.gameObject,
                    useFuseOverride = false,
                    useSpeedOverride = false
                };
                ModifyProjectileInfo(info);
                ProjectileManager.instance.FireProjectile(info);
            }
        }
    }
}
