using EntityStates;
using EntityStates.Huntress.HuntressWeapon;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.States.Huntress
{
    class ShootArrow : BaseSkillState {
        public static GameObject effectPrefab = FireArrow.effectPrefab;

        public string attackSoundString;
        public float baseDuration;
        public int maxArrowCount;
        public float baseArrowReloadDuration;
        private float duration;
        protected float arrowReloadDuration;
        private float arrowReloadTimer;
        protected bool isCrit;
        private int firedArrowCount;

        private ChildLocator locator;
        private HuntressTracker huntressTracker;
        private Animator animator;
        private FireSeekingArrow _goodState;
        public ShootArrow() {
            if (_goodState == null) {
                _goodState = Instantiate(new SerializableEntityStateType(typeof(FireSeekingArrow))) as FireSeekingArrow;
            }
            attackSoundString = _goodState.attackSoundString;
            maxArrowCount = _goodState.maxArrowCount;
            baseArrowReloadDuration = _goodState.baseArrowReloadDuration;
            baseDuration = _goodState.baseDuration;
        }

        private void ShootAnArrow(string targetMuzzle)
        {
            if (this.firedArrowCount >= this.maxArrowCount || this.arrowReloadTimer > 0f)
            {
                return;
            }
            this.firedArrowCount++;
            this.arrowReloadTimer = this.arrowReloadDuration;

            Util.PlaySound(attackSoundString, base.gameObject);
            
            var aimRay = base.GetAimRay();
            var modelTransform = GetModelTransform();

            if (modelTransform)
            {
                ChildLocator locator = modelTransform.GetComponent<ChildLocator>();
                if (locator)
                {
                    Transform transform = locator.FindChild(targetMuzzle);
                    if (transform)
                    {
                        aimRay.origin = transform.position;
                    }
                }
            }
            if (effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(effectPrefab, base.gameObject, targetMuzzle, false);
            }

            base.PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", duration, duration * 0.2f / attackSpeedStat);
            base.PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", duration, duration * 0.2f / attackSpeedStat);

            FireProjectileInfo info = new FireProjectileInfo()
            {
                crit = RollCrit(),
                damage = 2.1f * damageStat,
                damageColorIndex = DamageColorIndex.Default,
                damageTypeOverride = DamageType.Generic,
                force = 1000,
                owner = gameObject,
                position = aimRay.origin,
                procChainMask = default,
                projectilePrefab = HuntressCore.arrowObject,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                target = null,
                useFuseOverride = false,
                useSpeedOverride = false,
            };
            ProjectileManager.instance.FireProjectile(info);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Transform modelTransform = base.GetModelTransform();
            this.huntressTracker = base.GetComponent<HuntressTracker>();
            if (modelTransform)
            {
                this.animator = modelTransform.GetComponent<Animator>();
            }
            Util.PlayScaledSound(this.attackSoundString, base.gameObject, this.attackSpeedStat);

            this.duration = this.baseDuration / this.attackSpeedStat;
            this.arrowReloadDuration = this.baseArrowReloadDuration / this.attackSpeedStat;
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.duration + 1f);
            }
            base.PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", this.duration, this.duration * 0.2f / this.attackSpeedStat);
            base.PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", this.duration, this.duration * 0.2f / this.attackSpeedStat);
            this.isCrit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
        }

        public override void OnExit()
        {
            base.OnExit();
            ShootAnArrow("Muzzle");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.arrowReloadTimer -= Time.fixedDeltaTime;
            if (this.animator.GetFloat("FireSeekingShot.fire") > 0f)
            {
                ShootAnArrow("Muzzle");
            }
            if (base.fixedAge > this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
