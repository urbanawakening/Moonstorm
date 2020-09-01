using EntityStates;
using UnityEngine;

namespace Cloudburst.Cores.States.AncientWisp
{
    class ChargeCannon : BaseState
    {
        public static float baseDuration = 1;
        private float duration;

        //public static GameObject chargeEffect = Resources.Load<GameObject>("prefabs/effects");

        //GameObjects
        //private GameObject chargeEffectRight;
        //private GameObject chargeEffectLeft;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            PlayAnimation("Gesture", "ChargeRHCannon", "ChargeRHCannon.playbackRate", duration);

            var modelTransform = GetModelTransform();

            if (modelTransform) {
                var locator = modelTransform.GetComponent<ChildLocator>();
                if (locator) {
                    var rightHand = locator.FindChild("MuzzleRight");
                    var leftHand = locator.FindChild("MuzzleLeft");
                    if (rightHand) {
                        //chargeEffectRight = Object.Instantiate<GameObject>(chargeEffect, rightHand.position, rightHand.rotation);
                        //chargeEffectRight.transform.parent = rightHand;
                    }
                    if (leftHand) {
                        //chargeEffectLeft = Object.Instantiate<GameObject>(chargeEffect, rightHand.position, rightHand.rotation);
                        //chargeEffectLeft.transform.parent = leftHand;
                    }
                }
            }

            if (characterBody) {
                characterBody.SetAimTimer(duration);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority) {
                this.outer.SetNextState(new FireCannon());
                return;
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            /*if (chargeEffectLeft)
            {
                Destroy(chargeEffectLeft);
            }
            if (chargeEffectRight)
            {
                Destroy(chargeEffectRight);
            }*/
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
