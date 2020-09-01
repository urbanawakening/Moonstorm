using EntityStates;

namespace Cloudburst.Cores.States.AncientWisp
{
    class EndRain : BaseState
    {
        public static float baseDuration = 1;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            PlayAnimation("Body", "EndRain", "EndRain.playbackRate", duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }
    }
}
