using Moonstorm.Cores.HAND.Components;
using EntityStates;

namespace Moonstorm.Cores.States.HAND
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
