using System;
using System.Runtime.CompilerServices;
using Cloudburst.Cores.HAND;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores.States.HAND
{
    public class HomeRun : BaseSkillState
    {
        public static float baseDuration = 1f;
        public static float impactDamageCoefficient = 2f;
        public static float earthquakeDamageCoefficient = 6f;
        public static float forceMagnitude = 5000f;
        public static float radius = 3f;
        public static GameObject hitEffectPrefab;
        public static GameObject swingEffectPrefab = Resources.Load<GameObject>("prefabs/effects/HANDSlamTrail");
        public static GameObject projectilePrefab = HAN_DCore.instance.sunder;
        private Transform hammerChildTransform;
        private OverlapAttack attack;
        private Animator modelAnimator;
        private float duration;
        private bool hasSwung;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            this.modelAnimator = base.GetModelAnimator();
            Transform modelTransform = base.GetModelTransform();
            this.attack = new OverlapAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                damage = impactDamageCoefficient * damageStat,
                damageColorIndex = DamageColorIndex.Default,
                damageType = Util.CheckRoll(50) ? DamageType.Stun1s : DamageType.Generic,
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
            if (modelTransform)
            {
                this.attack.hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Hammer");
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                if (component)
                {
                    this.hammerChildTransform = component.FindChild("SwingCenter");
                }
            }
            if (this.modelAnimator)
            {
                base.PlayAnimation("Gesture", "Slam", "Slam.playbackRate", this.duration);
            }
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(2f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.modelAnimator && this.modelAnimator.GetFloat("Hammer.hitBoxActive") > 0.5f)
            {
                if (!this.hasSwung)
                {
                    Ray aimRay = base.GetAimRay();
                    EffectManager.SimpleMuzzleFlash(swingEffectPrefab, base.gameObject, "SwingCenter", true);
                    ProjectileManager.instance.FireProjectile(projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageStat * earthquakeDamageCoefficient, forceMagnitude, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                    this.hasSwung = true;
                }
                this.attack.forceVector = this.hammerChildTransform.right;
                this.attack.Fire(null);
            }
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}