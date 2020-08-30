using System;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Enemies.EntityStatez.ClayManMonster
{
    public class SwipeForward : BaseState
    {
        public static float baseDuration = 2f;
        public static float damageCoefficient = 4f;
        public static float forceMagnitude = 16f;
        public static float selfForceMagnitude = 10f;
        public static float radius = 8f;

        public static GameObject hitEffectPrefab = EntityStates.ClaymanMonster.SwipeForward.hitEffectPrefab;
        public static GameObject swingEffectPrefab = Resources.Load<GameObject>("prefabs/effects/ClaymanSwordSwing");

        //public static string attackString = EntityStates.ClaymanMonster.SwipeForward.attackString;

        private OverlapAttack attack;
        private Animator modelAnimator;
        private float duration;
        private bool hasSlashed;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = SwipeForward.baseDuration / this.attackSpeedStat;
            this.modelAnimator = base.GetModelAnimator();
            Transform modelTransform = base.GetModelTransform();
            this.attack = new OverlapAttack();
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = TeamIndex.Monster;
            this.attack.damage = SwipeForward.damageCoefficient * this.damageStat;
            this.attack.hitEffectPrefab = SwipeForward.hitEffectPrefab;
            this.attack.isCrit = Util.CheckRoll(this.critStat, base.characterBody.master);
            //Util.PlaySound(SwipeForward.attackString, base.gameObject);
            if (modelTransform)
            {
                this.attack.hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Sword");
            }
            if (this.modelAnimator)
            {
                base.PlayAnimation("Gesture, Override", "SwipeForward", "SwipeForward.playbackRate", this.duration);
                base.PlayAnimation("Gesture, Additive", "SwipeForward", "SwipeForward.playbackRate", this.duration);
            }
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(2f);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (NetworkServer.active && this.modelAnimator && this.modelAnimator.GetFloat("SwipeForward.hitBoxActive") > 0.1f)
            {
                if (!this.hasSlashed)
                {
                    EffectManager.SimpleMuzzleFlash(SwipeForward.swingEffectPrefab, base.gameObject, "SwingCenter", true);
                    HealthComponent healthComponent = base.characterBody.healthComponent;
                    CharacterDirection component = base.characterBody.GetComponent<CharacterDirection>();
                    if (healthComponent)
                    {
                        healthComponent.TakeDamageForce(SwipeForward.selfForceMagnitude * component.forward, true, false);
                    }
                    this.hasSlashed = true;
                }
                this.attack.forceVector = base.transform.forward * SwipeForward.forceMagnitude;
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
