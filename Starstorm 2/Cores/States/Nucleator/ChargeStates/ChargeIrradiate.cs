using EntityStates;
using EntityStates.Captain.Weapon;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores.States.Nucleator
{
    class ChargeIrradiate : NucleatorSkillStateBase
    {
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

            if ((base.fixedAge >= this.maxChargeTime || !base.inputBank || !base.inputBank.skill1.down) && base.isAuthority)
            {
                FireIrradiate fireIrradiate = new FireIrradiate();
                fireIrradiate.charge = this.charge;

                this.outer.SetNextState(fireIrradiate);
                return;
            }
        }



        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

