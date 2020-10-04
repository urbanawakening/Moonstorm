using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores.States.Nemmando
{
    class Submission : BaseSkillState
    {
        public Submission() : base()
        {
        }

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

        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

