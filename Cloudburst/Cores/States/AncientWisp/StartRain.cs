using EntityStates;

namespace Cloudburst.Cores.States.AncientWisp
{
    class StartRain : BaseState
    {
        public static float baseDuration = 2;
        private float duration;
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;

            PlayAnimation("Body", "ChargeRain", "ChargeRain.playbackRate", duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextState(new RAIN());
                return;
            }
        }
    }
}
