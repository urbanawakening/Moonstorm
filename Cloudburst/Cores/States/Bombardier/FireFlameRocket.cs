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
        public override void ModifyProjectileInfo(FireProjectileInfo info)
        {
            base.ModifyProjectileInfo(info);

            var damageType = DamageTypeCore.bombardierKnockback;
            //LogCore.LogI(damageType);
            if (damageType > DamageType.Shock5s) {
                LogCore.LogI(damageType);
            }

            info.projectilePrefab = ProjectileCore.bombardierFireBombProjectile;
            info.force = 0;
            info.damageTypeOverride = damageType;
        }
    }
}