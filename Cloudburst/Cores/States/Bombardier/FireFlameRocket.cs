using System;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;


namespace Cloudburst.Cores.States.Bombardier
{
    public class FireFlameRocket : FireRocket
    {
        public override GameObject GrabProjectile()
        {
            return ProjectileCore.bombardierFireBombProjectile;
        }

        public override Vector3 GetKnockback(Ray aimRay)
        {
            return aimRay.direction * (-3000);
        }

        public override float GetBloom()
        {
            return 200;
        }

        public override void ModifyProjectileInfo(FireProjectileInfo info)
        {
            base.ModifyProjectileInfo(info);

            info.projectilePrefab = ProjectileCore.bombardierFireBombProjectile;
            info.force = -6000;
            info.damage = 2 * damageStat;
            info.damageTypeOverride = DamageType.Stun1s;
        }
    }
}