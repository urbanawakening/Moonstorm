using EntityStates;
using UnityEngine;

namespace Cloudburst.Cores.States.HAND
{
    public class ChargeHomeRun : BaseSkillState
    {
        private float chargeDuration;
        public static float baseDuration = .8f;
        public override void OnEnter()
        {
            base.OnEnter();
            chargeDuration = baseDuration / attackSpeedStat;
            if (GetModelAnimator())
            {
                PlayAnimation("Gesture", "ChargeSlam", "ChargeSlam.playbackRate", chargeDuration);
            }
            if (characterBody)
            {
                characterBody.SetAimTimer(chargeDuration + .5f);
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= chargeDuration && isAuthority)
            {
                outer.SetNextState(
                    new HomeRun()
                    );
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
