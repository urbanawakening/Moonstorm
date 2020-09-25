using Cloudburst.HAND.Components;
using EntityStates;

namespace Cloudburst.HAND.HStates
{
    public class BeginOverclock : BaseState { 
        public override void OnEnter() {
            if (base.isAuthority) {
                base.GetComponent<OverclockComponent>().ActivateOverclockAuthority();
            }
            this.outer.SetNextStateToMain();
        }
    }
}
