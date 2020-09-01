using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Cores.States.AncientWisp
{
    class FireCannon : BaseState
    {
        private float duration;
        public static float baseDuration = 0.5f;

        private Animator modelAnimator;

        private Transform rightHand;
        private Transform leftHand;

        private bool hasFired;

        public static GameObject projectilePrefab = Resources.Load<GameObject>("prefabs/projectiles/ArchWispCannon");

        private float returnToIdlePercentage = EntityStates.AncientWispMonster.Throw.returnToIdlePercentage;
        public override void OnEnter()
        {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;
            modelAnimator = GetModelAnimator();
            var modelTransform = GetModelTransform();
            if (modelTransform) {
                var locator = modelTransform.GetComponent<ChildLocator>();
                if (locator) {
                    var rightHandInstance = locator.FindChild("MuzzleRight");
                    var leftHandInstance = locator.FindChild("MuzzleLeft");
                    if (rightHandInstance) {
                        this.rightHand = rightHandInstance;
                    }
                    if (leftHandInstance) {
                        this.leftHand = leftHandInstance;
                    }
                }
            }
            PlayCrossfade("Gesture", "Throw1", "Throw.playbackRate", this.duration / (1f - returnToIdlePercentage), 0.2f);
            /*if (this.modelAnimator)
            {
                var index = this.modelAnimator.GetLayerIndex("Gesture");
                //If we're on the right hand
                if (this.modelAnimator.GetCurrentAnimatorStateInfo(index).IsName("Throw1")) {
                    base.PlayCrossfade("Gesture", "Throw2", "Throw.playbackRate", this.duration / (1f - returnToIdlePercentage), 0.2f);
                }
                //Else, we're on the left, and we're gonna go with the left hand.
                else {
                    base.PlayCrossfade("Gesture", "Throw1", "Throw.playbackRate", this.duration / (1f - returnToIdlePercentage), 0.2f);
                }
            }*/
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.modelAnimator && this.modelAnimator.GetFloat("Throw.activate") > 0f && !this.hasFired) {
                var aim = GetAimRay();
                ProjectileManager.instance.FireProjectile(new FireProjectileInfo() {
                    crit = RollCrit(),
                    damage = 3 * damageStat,
                    damageColorIndex = RoR2.DamageColorIndex.Default,
                    damageTypeOverride = RoR2.DamageType.AOE,
                    force = 100,
                    owner = gameObject,
                    position = rightHand.position,
                    procChainMask = default,
                    projectilePrefab = projectilePrefab,
                    rotation = Util.QuaternionSafeLookRotation(aim.direction),
                    target = null,
                    useFuseOverride = false,
                    useSpeedOverride = false,
                });
                hasFired = true;
            }
            if (fixedAge >= duration && isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            PlayCrossfade("Gesture", "Idle", .7f);
        }

        private void FireBlast(string muzzle) {
            //TODO:
            //Don't finish later.
            throw new System.NotImplementedException();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
