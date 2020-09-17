using R2API;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Cores
{
    class ProjectileCore
    {
        public ProjectileCore instance;
        public static GameObject bombardierBombProjectile;
        public static GameObject bombardierFireBombProjectile;
        public static GameObject bombardierSeekerBombProjectile;
        public ProjectileCore() => CreateProjectiles();


        protected void CreateProjectiles()
        {
            instance = this;

            LogCore.LogI("Initializing Core: " + base.ToString());

            CreateBombardierRocketProjectile();
            CreateBombardierBetterRocketProjectile();
            CreateBombardierSeekerRocketProjectile();
        }

        protected private void CreateBombardierRocketProjectile() {
            bombardierBombProjectile = Resources.Load<GameObject>("prefabs/projectiles/PaladinRocket").InstantiateClone("BombardierRocketProjectile", true);
            var registered = API.RegisterNewProjectile(bombardierBombProjectile);
            if (registered)
            {
                var impact = bombardierBombProjectile.GetComponent<ProjectileImpactExplosion>();
                impact.blastProcCoefficient = 0.3f;
                impact.blastDamageCoefficient = 1.1f;
            }
            else LogCore.LogF("FATAL ERROR:" + bombardierBombProjectile.name + " failed to register to the projectile catalog!");
        }

        protected private void CreateBombardierBetterRocketProjectile()
        {
            bombardierFireBombProjectile = Resources.Load<GameObject>("prefabs/projectiles/PaladinRocket").InstantiateClone("BombardierFireRocketProjectile", true);
            var registered = API.RegisterNewProjectile(bombardierFireBombProjectile);
            if (registered)
            {
                var impact = bombardierFireBombProjectile.GetComponent<ProjectileImpactExplosion>();
                var controller = bombardierFireBombProjectile.GetComponent<ProjectileController>();
                var damage = bombardierFireBombProjectile.GetComponent<ProjectileDamage>();

                damage.damageType = DamageTypeCore.bombardierKnockback;

                impact.blastDamageCoefficient = 1;
                impact.blastRadius = 30;
                damage.force = -6000;

                controller.ghostPrefab = Resources.Load<GameObject>("prefabs/projectileghosts/ArchWispCannonGhost");
                controller.procCoefficient = 0.6f;

                impact.blastProcCoefficient = 5.5f;
                impact.blastDamageCoefficient = 1f;
            }
            else LogCore.LogF("FATAL ERROR:" + bombardierFireBombProjectile.name + " failed to register to the projectile catalog!");
        }
        protected private void CreateBombardierSeekerRocketProjectile()
        {
            bombardierSeekerBombProjectile = Resources.Load<GameObject>("prefabs/projectiles/MissileProjectile").InstantiateClone("BombardierSeekerRocketProjectile", true);
            var registered = API.RegisterNewProjectile(bombardierFireBombProjectile);
            if (registered)
            {
                var controller = bombardierSeekerBombProjectile.GetComponent<ProjectileController>();

                controller.ghostPrefab = bombardierBombProjectile.GetComponent<ProjectileController>().ghostPrefab;

                /*controller.procCoefficient = 0.6f;

                impact.blastProcCoefficient = 5.5f;
                impact.blastDamageCoefficient = 1f;*/
            }
            else LogCore.LogF("FATAL ERROR:" + bombardierSeekerBombProjectile.name + " failed to register to the projectile catalog!");
        }

    }
}
