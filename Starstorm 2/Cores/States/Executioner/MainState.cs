using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Starstorm2.Cores;

//TODO: should check that secondary is ion gun before attempting to store/load charges

namespace EntityStates.Executioner
{
    public class ExecutionerMain : GenericCharacterMain
    {
        private Animator animator;
        private GenericSkill ionGunSkill;
        private IonGunChargeComponent storedChargeComp; 

        public override void OnEnter()
        {
            base.OnEnter();

            this.animator = base.GetModelAnimator();

            //set up ion gun stock system
            ionGunSkill = skillLocator.secondary;
            storedChargeComp = characterBody.GetComponent<IonGunChargeComponent>();
            if (ionGunSkill && storedChargeComp)
                ionGunSkill.stock = storedChargeComp.storedCharges;

            GlobalEventManager.onCharacterDeathGlobal += OnKillHandler;
        }

        public override void OnExit()
        {
            GlobalEventManager.onCharacterDeathGlobal -= OnKillHandler;

            if (ionGunSkill && storedChargeComp)
                storedChargeComp.storedCharges = ionGunSkill.stock;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void OnKillHandler(DamageReport report)
        {
            if (report != null && report.attacker)
            {
                if (report.attacker == base.gameObject)
                {
                    if (ionGunSkill.stock < ionGunSkill.maxStock)
                        ionGunSkill.AddOneStock();
                }
            }
        }
    }
}

