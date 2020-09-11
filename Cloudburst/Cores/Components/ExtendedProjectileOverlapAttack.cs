using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores.HAND.Components { 

    [RequireComponent(typeof(HitBoxGroup))]
    [RequireComponent(typeof(ProjectileDamage))]
    [RequireComponent(typeof(ProjectileController))]
    public class ExtendedProjectileOverlapAttack : MonoBehaviour
    {
        #region Components
        protected ProjectileController projectileController;
        protected ProjectileDamage projectileDamage;
        #endregion

        public float damageCoefficient;
        public float overlapProcCoefficient = 1f;

        public GameObject impactEffect;

        public Vector3 forceVector;

        public int maximumOverlapTargets = 100;

        private OverlapAttack attack;

        public float fireFrequency = 60f;
        public float resetInterval = -1f;

        private float rTimer;
        private float fTimer;
        private void Start()
        {
            this.projectileController = base.GetComponent<ProjectileController>();
            this.projectileDamage = base.GetComponent<ProjectileDamage>();
            DefineOverlapAttack();
        }
        private void DefineOverlapAttack()
        {
            attack = new OverlapAttack();
            attack.procChainMask = projectileController.procChainMask;
            attack.procCoefficient = projectileController.procCoefficient * overlapProcCoefficient;
            attack.attacker = projectileController.owner;
            attack.inflictor = base.gameObject;
            attack.teamIndex = projectileController.teamFilter.teamIndex;
            attack.damage = damageCoefficient * projectileDamage.damage;
            attack.forceVector = forceVector + projectileDamage.force * base.transform.forward;
            attack.hitEffectPrefab = impactEffect;
            attack.isCrit = projectileDamage.crit;
            attack.damageColorIndex = projectileDamage.damageColorIndex;
            attack.damageType = projectileDamage.damageType;
            attack.procChainMask = projectileController.procChainMask;
            attack.maximumOverlapTargets = maximumOverlapTargets;
            attack.hitBoxGroup = base.GetComponent<HitBoxGroup>();
        }
        public void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                if (this.resetInterval >= 0f)
                {
                    this.rTimer -= Time.fixedDeltaTime;
                    if (this.rTimer <= 0f)
                    {
                        this.rTimer = this.resetInterval;
                        Reset();
                    }
                }
                this.fTimer -= Time.fixedDeltaTime;
                if (this.fTimer <= 0f)
                {
                    this.fTimer = 1f / this.fireFrequency;
                    bool hit = this.attack.Fire(null);
                    if (hit) {
                        OnHit(attack, transform);
                    }
                }
            }
        }
        public void Reset()
        {
            this.attack.damageType = this.projectileDamage.damageType;
            this.attack.ResetIgnoredHealthComponents();
        }
        public virtual void OnHit(OverlapAttack attack, Transform position) {

        }
    }
}