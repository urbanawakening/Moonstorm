using Cloudburst.Cores.HAND.Components;
using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace Cloudburst.Cores.States.HAND
{
    public class Slam : BaseSkillState
    {
        private float duration;
        private Transform hammerChildTransform;

        private bool hasSwung;

        public OverlapAttack attack;
        #region OverlapAttack floats
        public static float baseDuration;
        public static float damageCoefficient;
        public static float forceMagnitude;
        #endregion
        #region BlastAttack floats
        public static float blastDamageCoefficient;
        public static float blastForce;
        public static float blastRadius;
        #endregion
        #region Effects
        public static GameObject swingEffectPrefab = Resources.Load<GameObject>("prefabs/effects/handslamtrail");
        public static GameObject hitEffectPrefab;
        public static GameObject rumbleEffectPrefab;
        public static GameObject overclockRumbleEffectPrefab;
        #endregion
        #region HitPause
        public float hitPauseDuration = 0.1f;
        public bool enteredHitPause = false;
        public bool exitedHitPause = false;
        public float shorthopVelocityFromHit = 8f;
        private float hitPauseTimer = 0f;
        private Vector3 storedVelocity;
        private bool scaleHitPauseDurationAndVelocityWithAttackSpeed = true;
        #endregion
        private DroneComponent drone;
        private OverclockComponent overclock;
        private Animator modelAnimator;
        public float charge;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / base.attackSpeedStat;
            modelAnimator = GetModelAnimator();
            drone = base.GetComponent<DroneComponent>();
            overclock = base.GetComponent<OverclockComponent>();
            this.attack = new OverlapAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                damage = damageCoefficient * damageStat,
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
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                this.attack.hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Hammer");
                ChildLocator locator = modelTransform.GetComponent<ChildLocator>();
                if (locator)
                {
                    this.hammerChildTransform = locator.FindChild("SwingCenter");
                }
            }
            if (base.GetModelAnimator())
            {
                base.PlayAnimation("Gesture", "Slam", "Slam.playbackRate", this.duration);
            }
            if (characterBody)
            {
                characterBody.SetAimTimer(2);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (GetModelAnimator() && GetModelAnimator().GetFloat("Hammer.hitBoxActive") > 0.5f)
            {
                if (!hasSwung)
                {
                    float radius = characterBody.HasBuff(BuffCore.instance.overclockIndex) ? blastRadius : blastRadius * 2;
                    float force = characterBody.HasBuff(BuffCore.instance.overclockIndex) ? blastForce : blastForce * 2;
                    float dmg = blastDamageCoefficient + charge;
                    bool fullyCharged = charge == 3;
                    hasSwung = true;
                    /*if (drone && characterBody.HasBuff(BuffCore.instance.overclockIndex)) {
                        drone.ConsumeDroneStackAuthority(2);
                    }*/

                    GameObject effect = fullyCharged ? overclockRumbleEffectPrefab : rumbleEffectPrefab;
                    BlastAttack attack = new BlastAttack()
                    {
                        attacker = base.gameObject,
                        attackerFiltering = AttackerFiltering.Default,
                        baseDamage = dmg * base.damageStat,
                        baseForce = force,
                        bonusForce = new Vector3(0, 0, 0),
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = overclock.GetDamageType(),
                        falloffModel = BlastAttack.FalloffModel.None,
                        impactEffect = EffectIndex.Invalid,
                        inflictor = base.gameObject,
                        losType = BlastAttack.LoSType.NearestHit,
                        position = base.transform.position,
                        procChainMask = default,
                        procCoefficient = 0.5f,
                        radius = radius,
                        teamIndex = characterBody.teamComponent.teamIndex,
                    };
                    attack.Fire();
                    EffectManager.SpawnEffect(effect, new EffectData
                    {
                        origin = hammerChildTransform.position,
                        scale = radius
                    }, true);
                    EffectManager.SimpleMuzzleFlash(swingEffectPrefab, base.gameObject, "SwingCenter", true);
                    if (fullyCharged)
                    {
                        characterMotor.ApplyForce(new Vector3(characterMotor.moveDirection.x, characterMotor.mass * 35, characterMotor.moveDirection.z), true, false);
                    }
                }
                attack.forceVector = hammerChildTransform.right;
                bool hit = attack.Fire(null);
                if (hit)
                {
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
                if (isAuthority && enteredHitPause && hitPauseTimer > 0f)
                {
                    hitPauseTimer -= Time.fixedDeltaTime;
                    characterMotor.velocity = Vector3.zero;
                    if (hitPauseTimer <= 0f)
                    {
                        ExitHitPause();
                    }
                }
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            if (!hasSwung)
            {
                if (enteredHitPause)
                {
                    ExitHitPause();
                }
            }
            if (enteredHitPause && !exitedHitPause)
            {
                ExitHitPause();
            }

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
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
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
