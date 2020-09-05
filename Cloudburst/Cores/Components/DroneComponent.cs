using RoR2;
using RoR2.Orbs;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores.HAND.Components
{
    [RequireComponent(typeof(CharacterBody))]
    public class DroneComponent : NetworkBehaviour, IOnKilledOtherServerReceiver
    {
        private SkillLocator skillLocator;
        private CharacterBody body;
        private HANDDroneTracker tracker;

        #region Interface Implementation
        public void OnKilledOtherServer(DamageReport damageReport) {
            if (skillLocator.special.stock < 10) {
                skillLocator.special.AddOneStock();
            }

            
        }
        #endregion
        private void Awake() {
            body = base.GetComponent<CharacterBody>();
            skillLocator = base.GetComponent<SkillLocator>();
            tracker = base.GetComponent<HANDDroneTracker>();
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
            var genericDamageOrb = new DroneOrb();
            genericDamageOrb.damageValue = 1 * body.damage;
            genericDamageOrb.isCrit = isCrit;
            genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
            genericDamageOrb.attacker = base.gameObject;
            genericDamageOrb.procCoefficient = 1;
            HurtBox hurtBox = initialOrbTarget;
            if (hurtBox)
            {
                genericDamageOrb.origin = transform.position;
                genericDamageOrb.target = hurtBox;

                OrbManager.instance.AddOrb(genericDamageOrb);
            }
        }
        [Command]
        private void CmdFireDrone() {
            FireDroneInternal();
        }

    }
}

