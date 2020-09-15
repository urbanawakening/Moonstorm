using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudburst.Cores.States.Bombardier
{
    class FireBetterRocket : FireRocket
    {
        FireBetterRocket() {
            baseDuration = 1f;
            projectilePrefab = ProjectileCore.bombardierFireBombProjectile;
            damageCoeff = 1.6f;
            damageType = RoR2.DamageType.PercentIgniteOnHit;
        }
    }
}
