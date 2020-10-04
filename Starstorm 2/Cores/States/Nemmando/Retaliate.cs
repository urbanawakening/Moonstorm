using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores.States.Nemmando
{
    class Retaliate : BaseSkillState
    {
        private float baseDuration = 0.3f;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(this.isAuthority && this.fixedAge > baseDuration)
            {
                this.outer.SetNextStateToMain();
                return;
            }                
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

