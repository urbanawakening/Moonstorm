using System;
using Cloudburst.Cores.HAND.Components;
using EntityStates;
using RoR2;
using UnityEngine;

namespace Cloudburst.Cores.States.HAND
{
    public class FullSwing : BaseSkillState
    {
        public static float baseDuration = 1.1f;
        //0.443662
        public static float returnToIdlePercentage = EntityStates.HAND.Weapon.FullSwing.returnToIdlePercentage;
        public static float damageCoefficient = 4f;
        public static float forceMagnitude = 1250;
        public static float radius = 3;

        public float hitPauseDuration = 0.1f;
        public bool enteredHitPause = false;
        public bool exitedHitPause = false;
        public float shorthopVelocityFromHit = 8f;
        private float hitPauseTimer = 0f;
        private Vector3 storedVelocity;
        private bool scaleHitPauseDurationAndVelocityWithAttackSpeed = true;

        public static GameObject swingEffectPrefab = Resources.Load<GameObject>("prefabs/effects/handslamtrail");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxmedium");

        private Transform hammerChildTransform;

        private OverlapAttack attack;

        private Animator modelAnimator;

        private OverclockComponent overclock;
        private DroneComponent droneComponent;

        private float duration;
        private bool hasSwung;
        public override void OnEnter()
        {
            base.OnEnter();
            droneComponent = base.gameObject.GetComponent<DroneComponent>();
            duration = baseDuration / attackSpeedStat;
            overclock = GetComponent<OverclockComponent>(); 
            modelAnimator = GetModelAnimator();
            attack = new OverlapAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                damage = damageCoefficient * base.damageStat,
                damageColorIndex = DamageColorIndex.Default,
                damageType = overclock.GetDamageType(),
                forceVector = new Vector3(0, 0, 0),
                hitEffectPrefab = hitEffectPrefab,
                impactSound = RoR2.Audio.NetworkSoundEventIndex.Invalid,
                inflictor = base.gameObject,
                isCrit = base.RollCrit(),
                maximumOverlapTargets = 100,
                procChainMask = default,
                procCoefficient = 1,
                pushAwayForce = forceMagnitude,
                teamIndex = base.characterBody.teamComponent.teamIndex
            };
            if (base.GetModelTransform())
            {
                Transform modelTransform = base.GetModelTransform();
                this.attack.hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Hammer");
                if (modelTransform.HasComponent<ChildLocator>())
                {
                    this.hammerChildTransform = modelTransform.GetComponent<ChildLocator>().FindChild("SwingCenter");
                }
            }
            if (this.modelAnimator)
            {
                int layerIndex = this.modelAnimator.GetLayerIndex("Gesture");
                var animatorStateInfo = this.modelAnimator.GetCurrentAnimatorStateInfo(layerIndex);

                if (this.modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsName("FullSwing3") || this.modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsName("FullSwing1"))
                {
                    base.PlayCrossfade("Gesture", "FullSwing2", "FullSwing.playbackRate", this.duration / (1f - returnToIdlePercentage), 0.2f);
                }
                else if (this.modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsName("FullSwing2"))
                {
                    base.PlayCrossfade("Gesture", "FullSwing3", "FullSwing.playbackRate", this.duration / (1f - FullSwing.returnToIdlePercentage), 0.2f);
                }
                else
                {
                    base.PlayCrossfade("Gesture", "FullSwing1", "FullSwing.playbackRate", this.duration / (1f - returnToIdlePercentage), 0.2f);
                }
            }
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(2f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.modelAnimator && this.modelAnimator.GetFloat("Hammer.hitBoxActive") > 0.5f) {
                if (!this.hasSwung) {
                    //EffectManager.SimpleMuzzleFlash(swingEffectPrefab, base.gameObject, "SwingCenter", true);
                    Util.PlaySound("Play_MULT_shift_hit", base.gameObject);
                    this.hasSwung = true;
                    EffectManager.SimpleMuzzleFlash(swingEffectPrefab, base.gameObject, "SwingCenter", true);
                    //if (droneComponent && characterBody.HasBuff(Main.overclock)) {
                    //    droneComponent.ConsumeDroneStackAuthority(2);
                    //}
                }
                this.attack.forceVector = this.hammerChildTransform.right * -forceMagnitude;
                bool hit = this.attack.Fire(null);
                if (hit) {
                    EnterHitPause();
                }
            }
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            else
            {
                if (base.isAuthority && this.enteredHitPause && this.hitPauseTimer > 0f) {
                    this.hitPauseTimer -= Time.fixedDeltaTime;
                    base.characterMotor.velocity = Vector3.zero;
                    if (this.hitPauseTimer <= 0f) {
                        this.ExitHitPause();
                    }
                }
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            if (!hasSwung)
            {
                if (enteredHitPause) {
                    ExitHitPause();
                }
            }
            if (enteredHitPause && !exitedHitPause) {
                ExitHitPause();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {

            return InterruptPriority.Skill;
        }
        private void ExitHitPause()
        {
            this.hitPauseTimer = 0f;
            if (!isGrounded)
            {
                this.storedVelocity.y = Mathf.Max(this.storedVelocity.y, this.scaleHitPauseDurationAndVelocityWithAttackSpeed ? (this.shorthopVelocityFromHit / Mathf.Sqrt(this.attackSpeedStat)) : this.shorthopVelocityFromHit);
                base.characterMotor.velocity = this.storedVelocity;
                this.storedVelocity = Vector3.zero;
            }
            if (modelAnimator)
            {
                modelAnimator.speed = 1f;
            }
            exitedHitPause = true;
        }
        private void EnterHitPause()
        {
            this.storedVelocity += base.characterMotor.velocity;
            if (!isGrounded)
            {
                base.characterMotor.velocity = Vector3.zero;
            }
            if (modelAnimator)
            {
                modelAnimator.speed = 0f;
            }
            this.hitPauseTimer = (this.scaleHitPauseDurationAndVelocityWithAttackSpeed ? (this.hitPauseDuration / this.attackSpeedStat) : this.hitPauseDuration);
            enteredHitPause = true;
        }
    }
}