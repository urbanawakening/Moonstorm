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

                damage.damageType = RoR2.DamageType.Stun1s;

                impact.blastDamageCoefficient = 1;
                impact.blastRadius = 30;
                //OmniExplosionVFXArchWispCannonImpact OmniExplosionVFX
                impact.impactEffect = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXArchWispCannonImpact");

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
                var impact = bombardierSeekerBombProjectile.AddOrGetComponent<ProjectileImpactExplosion>();
                var missile = bombardierSeekerBombProjectile.GetComponent<MissileController>();

                missile.deathTimer = 10;
                missile.giveupTimer = 5;
                missile.turbulence = 0;
                missile.maxSeekDistance = 200;

                controller.ghostPrefab = Resources.Load<GameObject>("prefabs/projectileghosts/ElectricOrbGhost");
                controller.procCoefficient = 0.6f;

                impact.blastAttackerFiltering = RoR2.AttackerFiltering.Default;
                impact.blastDamageCoefficient = 2.5f;
                impact.blastProcCoefficient = 1;
                impact.blastRadius = 10;
                impact.childrenCount = 0;
                impact.childrenDamageCoefficient = 0;
                impact.childrenProjectilePrefab = null;
                impact.destroyOnEnemy = true;
                impact.destroyOnWorld = true;
                impact.falloffModel = RoR2.BlastAttack.FalloffModel.None;
                impact.fireChildren = false;
                impact.impactEffect = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionVFX");
                impact.lifetime = 10;
                impact.lifetimeAfterImpact = 0;
                impact.lifetimeRandomOffset = 0;
            }
            else LogCore.LogF("FATAL ERROR:" + bombardierSeekerBombProjectile.name + " failed to register to the projectile catalog!");
        }

    }
}
