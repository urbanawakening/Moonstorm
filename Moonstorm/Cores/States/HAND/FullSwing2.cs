using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace Moonstorm.Cores.States.HAND
{
    public class FullSwing2 : BasicMeleeAttack
    {
        public static float returnToIdlePercentage = EntityStates.HAND.Weapon.FullSwing.returnToIdlePercentage;

        public FullSwing2() {
            baseDuration = 1.1f;
            beginStateSoundString = "Play_MULT_shift_hit";
            beginSwingSoundString = "Play_MULT_shift_hit";
            damageCoefficient = 3.0f;
            forceForwardVelocity = false;
            forceVector = new Vector3(1, 1, 1);

            //Transform modelTransform = GetModelTransform();
            //if (modelTransform)
            //{
            //    hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Hammer");
            //}
            hitBoxGroupName = "Hammer";
            hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxmedium");
            hitPauseDuration = 0.1f;
            procCoefficient = 1;
            pushAwayForce = 2500;
            scaleHitPauseDurationAndVelocityWithAttackSpeed = true;
            shorthopVelocityFromHit = 8;
            swingEffectPrefab = Resources.Load<GameObject>("prefabs/effects/handslamtrail");
        }
        public override void PlayAnimation()
        {
            base.PlayAnimation();
            var modelAnimator = base.GetModelAnimator();
            if (modelAnimator)
            {
                int layerIndex = modelAnimator.GetLayerIndex("Gesture");
                if (modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsName("FullSwing3") || modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsName("FullSwing1"))
                {
                    PlayCrossfade("Gesture", "FullSwing2", "FullSwing.playbackRate", this.duration / (1f - returnToIdlePercentage), 0.2f);
                }
                else if (modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsName("FullSwing2"))
                {
                    PlayCrossfade("Gesture", "FullSwing3", "FullSwing.playbackRate", this.duration / (1f - returnToIdlePercentage), 0.2f);
                }
                else
                {
                    PlayCrossfade("Gesture", "FullSwing1", "FullSwing.playbackRate", this.duration / (1f - returnToIdlePercentage), 0.2f);
                }
            }
        }
    }
}
