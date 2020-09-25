using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Cores.States.Bombardier
{
    //oh oh she wants me (to be loved)
    class RocketJump : FireRocket
    {
        public override GameObject GrabProjectile()
        {
            return ProjectileCore.bombardierFireBombProjectile;
        }

        //aimRay.direction * (-characterMotor.mass * 35)

        public override float GetBloom()
        {
            return 800;
        }

        public override void ModifyProjectileInfo(FireProjectileInfo info)
        {
            base.ModifyProjectileInfo(info);

            info.damageTypeOverride = RoR2.DamageType.Stun1s;
            info.force = -5000;
            info.damageColorIndex = RoR2.DamageColorIndex.WeakPoint;
        }

        public override Vector3 GetKnockback(Ray aimRay)
        {
            return aimRay.direction * (-characterMotor.mass * 35);
        }
    }
}
