using Moonstorm.Cores.Components;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores
{
    class ProjectileCore
    {
        public ProjectileCore instance;

        public static GameObject bombardierBombProjectile;
        public static GameObject bombardierFireBombProjectile;
        public static GameObject bombardierSeekerBombProjectile;

        public static GameObject MIRVProjectile;
        public static GameObject MIRVClusterProjectile;

        public static GameObject stickyProjectile;

        public ProjectileCore() => CreateProjectiles();


        protected void CreateProjectiles()
        {
            instance = this;

            LogCore.LogI("Initializing Core: " + base.ToString());

            CreateBombardierProjectiles();
            CreateMiscProjectiles();
        }

        #region Misc   
        protected private void CreateMiscProjectiles() {
            CreateMIRVProjectile();
        }

        protected private void CreateMIRVProjectile()
        {
            MIRVProjectile = Resources.Load<GameObject>("prefabs/projectiles/CryoCanisterBombletsProjectile").InstantiateClone("MIRVEquipmentProjectile", true);
            MIRVClusterProjectile = Resources.Load<GameObject>("prefabs/projectiles/CryoCanisterBombletsProjectile").InstantiateClone("MIRVClusterEquipmentProjectile", true);
            if (API.RegisterNewProjectile(MIRVProjectile) && API.RegisterNewProjectile(MIRVClusterProjectile))
            {
            }
            else LogCore.LogF("FATAL ERROR:" + MIRVProjectile.name + " failed to register!");
        }

        protected private void CreateStickyProjectile()
        {
            stickyProjectile = Resources.Load<GameObject>("prefabs/projectiles/EngiMine").InstantiateClone("MIRVEquipmentProjectile", true);
            if (API.RegisterNewProjectile(stickyProjectile))
            {
                stickyProjectile.AddComponent<BombardierStickyBombProjectile>();

                GameObject.Destroy(stickyProjectile.GetComponent<ProjectileDeployToOwner>());
                GameObject.Destroy(stickyProjectile.GetComponent<Deployable>());

                GameObject.Destroy(stickyProjectile.GetComponent<RTPCController>());
                foreach (var comp in stickyProjectile.GetComponents<EntityStateMachine>()) {
                    GameObject.Destroy(comp);
                }
                GameObject.Destroy(stickyProjectile.GetComponent<NetworkStateMachine>());
                GameObject.Destroy(stickyProjectile.GetComponent<ProjectileSphereTargetFinder>());
                GameObject.Destroy(stickyProjectile.GetComponent<ProjectileTargetComponent>());
               
                
                //GameObject.Destroy(stickyProjectile.GetComponent<ProjectileTargetComponent>());
                //GameObject.Destroy(stickyProjectile.GetComponent<ProjectileTargetComponent>());
                //GameObject.Destroy(stickyProjectile.GetComponent<ProjectileTargetComponent>());
            }
            else LogCore.LogF("FATAL ERROR:" + stickyProjectile.name + " failed to register!");
        }



        #endregion
        #region Bombardier
        protected private void CreateBombardierProjectiles() {
            CreateBombardierRocketProjectile();
            CreateBombardierBetterRocketProjectile();
            CreateBombardierPayloadProjectile();
            CreateStickyProjectile();
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

                impact.blastProcCoefficient = 0.3f;
                impact.blastDamageCoefficient = 1.1f;

                controller.ghostPrefab = Resources.Load<GameObject>("prefabs/projectileghosts/ArchWispCannonGhost");
                controller.procCoefficient = 0.6f;
                
                impact.impactEffect = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXArchWispCannonImpact");
                impact.blastProcCoefficient = 1f;
                impact.blastDamageCoefficient = 1f;
                impact.blastRadius = 25;
            }
            else LogCore.LogF("FATAL ERROR:" + bombardierFireBombProjectile.name + " failed to register to the projectile catalog!");
        }
        private protected void CreateBombardierPayloadProjectile()
        {
            bombardierSeekerBombProjectile = Resources.Load<GameObject>("prefabs/projectiles/MissileProjectile").InstantiateClone("BombardierSeekerRocketProjectile", true);
            var registered = API.RegisterNewProjectile(bombardierFireBombProjectile);
            if (registered)
            {
                var controller = bombardierSeekerBombProjectile.GetComponent<ProjectileController>();
                var impact = bombardierSeekerBombProjectile.AddOrGetComponent<ProjectileImpactExplosion>();
                var missile = bombardierSeekerBombProjectile.GetComponent<MissileController>();

                missile.deathTimer = 15;
                missile.giveupTimer = 10;
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
                impact.impactEffect = Resources.Load<GameObject>("prefabs/effects/LightningStakeNova");
                impact.lifetime = 10;
                impact.lifetimeAfterImpact = 0;
                impact.lifetimeRandomOffset = 0;
            }
            else LogCore.LogF("FATAL ERROR:" + bombardierSeekerBombProjectile.name + " failed to register to the projectile catalog!");
        }
        #endregion
    }
}
