using System;
using EntityStates;
using EntityStates.EngiTurret.EngiTurretWeapon;
using RoR2;
using UnityEngine;

namespace Cloudburst.Cores.Engineer.ETStates
{
    public class FireFlameThrower : BaseState
    {
        public GameObject effectPrefab;
        public GameObject hitEffectPrefab;
        public GameObject laserPrefab;
        public string muzzleString;
        public string attackSoundString;
        public float damageCoefficient;
        public float procCoefficient;
        public float force;
        public float minSpread;
        public float maxSpread;
        public int bulletCount;
        public float fireFrequency;
        public float maxDistance;
        private float fireTimer;
        private Ray laserRay;
        private Transform modelTransform;
        private GameObject laserEffectInstance;
        public int bulletCountCurrent = 1;

        private FireBeam _goodState;
        public FireFlameThrower() {
            if (_goodState == null)
            {             
                _goodState = Instantiate(typeof(FireBeam)) as FireBeam;
            }
            this.bulletCount = _goodState.bulletCount;
            //this.bulletCountCurrent = _goodState.bulletCount;
            this.damageCoefficient = .15f;
            this.effectPrefab = EntityStates.Mage.Weapon.Flamethrower.impactEffectPrefab;
            this.fireFrequency = _goodState.fireFrequency;
            this.force = _goodState.force;
            this.hitEffectPrefab = EntityStates.Mage.Weapon.Flamethrower.impactEffectPrefab;
            this.laserPrefab = Resources.Load<GameObject>("Prefabs/Effects/DroneFlameThrowerEffect");
            this.maxDistance = _goodState.maxDistance + 5;
            this.maxSpread = _goodState.maxSpread;
            this.minSpread = _goodState.minSpread;
            this.muzzleString = _goodState.muzzleString;
            this.procCoefficient = _goodState.procCoefficient;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound(this.attackSoundString, base.gameObject);
            this.fireTimer = 0f;
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();
                if (component)
                {
                    Transform transform = component.FindChild(this.muzzleString);
                    if (transform && this.laserPrefab)
                    {
                        this.laserEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.laserPrefab, transform.position, transform.rotation);
                        this.laserEffectInstance.transform.parent = transform;
                    }
                }
            }
        }

        public override void OnExit()
        {
            if (this.laserEffectInstance)
            {
                EntityState.Destroy(this.laserEffectInstance);
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.laserRay = this.GetLaserRay();
            this.fireTimer += Time.fixedDeltaTime;
            float num = this.fireFrequency * base.characterBody.attackSpeed;
            float num2 = 1f / num;
            if (this.fireTimer > num2)
            {
                this.FireBullet(this.modelTransform, this.laserRay, this.muzzleString);
                this.fireTimer = 0f;
            }

            if (base.isAuthority && !this.ShouldFireLaser())
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public virtual void ModifyBullet(BulletAttack bulletAttack)
        {
                bulletAttack.damageType |= DamageType.PercentIgniteOnHit;
        }

        public virtual bool ShouldFireLaser()
        {
            return base.inputBank && base.inputBank.skill1.down;
        }

        public virtual Ray GetLaserRay()
        {
            return base.GetAimRay();
        }

        private void FireBullet(Transform modelTransform, Ray laserRay, string targetMuzzle)
        {
            if (this.effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.effectPrefab, base.gameObject, targetMuzzle, false);
            }
            if (base.isAuthority)
            {
                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = base.gameObject;
                bulletAttack.weapon = base.gameObject;
                bulletAttack.origin = laserRay.origin;
                bulletAttack.aimVector = laserRay.direction;
                bulletAttack.minSpread = this.minSpread;
                bulletAttack.maxSpread = this.maxSpread;
                bulletAttack.bulletCount = 1u;
                bulletAttack.damage = this.damageCoefficient * this.damageStat / this.fireFrequency;
                bulletAttack.procCoefficient = this.procCoefficient / this.fireFrequency;
                bulletAttack.force = this.force;
                bulletAttack.muzzleName = targetMuzzle;
                bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
                bulletAttack.isCrit = Util.CheckRoll(this.critStat, base.characterBody.master);
                bulletAttack.HitEffectNormal = false;
                bulletAttack.radius = 0f;
                bulletAttack.maxDistance = this.maxDistance;
                this.ModifyBullet(bulletAttack);
                bulletAttack.Fire();
            }
        }
    }
}