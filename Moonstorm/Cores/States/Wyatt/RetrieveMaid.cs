using EntityStates;
using Moonstorm.Cores.Components;
using Moonstorm.Cores.HAND;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.States.Wyatt
{
    class RetrieveMaid : BaseSkillState
    {
        public static float baseDuration = 0.1f;

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority) {
                gameObject.GetComponent<MAIDManager>().RetrieveMAIDAuthority();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= baseDuration && isAuthority)
            {
                skillLocator.special.SetBaseSkill(WyattCore.instance.throwPrimary);
                outer.SetNextStateToMain(); 
            };
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}