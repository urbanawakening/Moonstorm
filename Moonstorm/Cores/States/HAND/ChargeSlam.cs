using EntityStates;
using RoR2;
using UnityEngine;

namespace Moonstorm.Cores.States.HAND
{
    public class ChargeSlam : BaseSkillState {
        public static float baseDuration;
        public static GameObject effectPrefab;
        public static GameObject chargeEffectPrefab;
        private float chargeDuration;
        private float charge;
        private Transform hammerChildTransform;
        private bool playedAudioCue = false;
        public override void OnEnter() {
            base.OnEnter();
            chargeDuration = baseDuration / attackSpeedStat;
            if (isAuthority)
            {
                if (GetModelAnimator())
                {
                    PlayAnimation("Gesture", "ChargeSlam", "ChargeSlam.playbackRate", chargeDuration);
                }
                if (characterBody)
                {
                    characterBody.SetAimTimer(chargeDuration + .5f);
                    characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                }
                Transform modelTransform = base.GetModelTransform();
                if (modelTransform)
                {
                    ChildLocator locator = modelTransform.GetComponent<ChildLocator>();
                    hammerChildTransform = locator.FindChild("SwingCenter");
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //charge = Mathf.Clamp01(fixedAge / chargeDuration);
            float bonus = CalculateCharge();

            //Removed because it wasn't fun.
            //AddRecoil(bonus / 3, bonus / 3, bonus / 3, bonus / 3);
            if (!Charging() && fixedAge >= chargeDuration && isAuthority)
            {
                outer.SetNextState(new Slam
                {
                    charge = bonus
                });
            };

        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        public bool Charging()
        {
            return inputBank.skill3.down;
        }
        private float CalculateCharge() {
            charge += Time.deltaTime * 2;
            bool capped = charge <= 3;
            float bonus = capped ? charge : 3;
            bool actuallyCapped = (charge == 3);   
            if (actuallyCapped && !playedAudioCue) {
                Util.PlaySound("Play_MULT_m1_snipe_charge_end", base.gameObject);
                //EffectManager.SimpleMuzzleFlash(chargeEffectPrefab, base.gameObject, "SwingCenter", true);
                EffectManager.SimpleEffect(chargeEffectPrefab, hammerChildTransform.position, hammerChildTransform.rotation, true);
                playedAudioCue = true;
            }
            return bonus;
        }
    }
}