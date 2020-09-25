using Cloudburst.HAND.Orbs;
using RoR2;
using RoR2.Orbs;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.HAND.Components
{
    [RequireComponent(typeof(CharacterBody))]
    public class DroneComponent : NetworkBehaviour, IOnKilledOtherServerReceiver
    {
        private SkillLocator skillLocator;
        private CharacterBody body;
        private HuntressTracker tracker;

        #region Interface Implementation
        public void OnKilledOtherServer(DamageReport damageReport) {
            if (skillLocator.utility.stock < 10) {
                skillLocator.utility.AddOneStock();
            }
            if (body.GetBuffCount(HAN_D.drone) < 10) {
                body.AddBuff(HAN_D.drone);
            }
            
        }
        #endregion
        private void Awake() {
            body = base.GetComponent<CharacterBody>();
            skillLocator = base.GetComponent<SkillLocator>();
            tracker = base.GetComponent<HuntressTracker>();
        }
        public void ConsumeDroneStackAuthority(float stacks) {
            if (NetworkServer.active) {
                RemoveDroneStackInternal(stacks);
                return;
            }
            CmdRemoveDroneStack(stacks);
        }
        public void FireDroneAuthority()
        {
            if (NetworkServer.active)
            {
                FireDroneInternal();
                return;
            }
            CmdFireDrone();
        }
        private void FireDroneInternal() {
            HurtBox initialOrbTarget = tracker.GetTrackingTarget();
            bool isCrit = false;
            if (body && body.master) isCrit = Util.CheckRoll(body.crit, body.master);
            GenericDamageOrb genericDamageOrb = new DroneOrb();
            genericDamageOrb.damageValue = 0;
            genericDamageOrb.isCrit = isCrit;
            genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
            genericDamageOrb.attacker = base.gameObject;
            genericDamageOrb.procCoefficient = 1;
            HurtBox hurtBox = initialOrbTarget;
            if (hurtBox)
            {
                genericDamageOrb.origin = transform.position;
                genericDamageOrb.target = hurtBox;
                if (body.GetBuffCount(HAN_D.drone) > 0) {
                    RemoveDroneStackInternal(1);
                }
                OrbManager.instance.AddOrb(genericDamageOrb);
            }
        }
        [Command]
        private void CmdFireDrone() {
            FireDroneInternal();
        }
        private void RemoveDroneStackInternal(float stacks) {
            for (int i = 0; i < stacks; i++) {
                body.RemoveBuff(HAN_D.drone);

            }
        }

        [Command]
        private void CmdRemoveDroneStack(float stacks) {
            RemoveDroneStackInternal(stacks);
        }


    }
}

