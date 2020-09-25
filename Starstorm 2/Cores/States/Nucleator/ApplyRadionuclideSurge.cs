using EntityStates;
using Starstorm2.Cores;

namespace Starstorm2.Cores.States.Nucleator
{
    class ApplyRadionuclideSurge : BaseSkillState
    {
        private float stateDuration = 0.1F;
        private float buffDuration = 6f;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.AddTimedBuffAuthority(NucleatorCore.radionuclideSurgeBuffIndex, buffDuration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.stateDuration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

