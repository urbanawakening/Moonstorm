using EntityStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudburst.Cores.States.HAND
{
    class AnimationTest : BaseSkillState
    {
        public static float baseDuration = 1.0f;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;

            var childLocator = gameObject.AddOrGetComponent<ChildLocator>();
            childLocator.FindChild("ArmIK");
            PlayAnimation("Gesture", "Slam", "Slam.playbackRate", duration * 2);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority) {
                outer.SetNextStateToMain();
                return;
            }
        }
    }
}
