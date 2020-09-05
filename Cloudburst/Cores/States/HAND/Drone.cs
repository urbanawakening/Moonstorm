using Cloudburst.Cores.HAND.Components;
using EntityStates;
using RoR2;
using RoR2.Orbs;
using UnityEngine.Networking;

namespace Cloudburst.Cores.States.HAND
{
    public class Drone : BaseState
    {
        public float orbDamageCoefficient;
        public float orbProcCoefficient;
        public string muzzleString;
        public string attackSoundString;
        public static float baseDuration;
        private float duration;
        protected bool isCrit;
        protected bool hasFired;
        private HurtBox initialOrbTarget;
        private HANDDroneTracker tracker;

        public override void OnEnter()
        {
            base.OnEnter();
            tracker = base.GetComponent<HANDDroneTracker>();

            if (tracker && isAuthority)
            {
                initialOrbTarget = tracker.GetTrackingTarget();
            }
            duration = baseDuration / attackSpeedStat;
            if (characterBody)
            {
                characterBody.SetAimTimer(duration + 1f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!hasFired) {
                this.DeployDrone();
            }
        }

        private void DeployDrone()
        {
            this.hasFired = true;
            base.GetComponent<DroneComponent>().FireDroneAuthority();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!hasFired)
            {
                this.DeployDrone();
            }
            if (base.fixedAge > this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}

/*    public class Drone : BaseSkillState {
        private float duration;
        private DroneComponent droneComponent;
        private HuntressTracker tracker;
        private bool firedDrone = false;
        public static float baseDuration;
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            droneComponent = base.GetComponent<DroneComponent>();
            if (!tracker) {
                tracker = base.gameObject.AddOrGetComponent<HuntressTracker>();
                tracker.maxTrackingAngle = 30;
                tracker.maxTrackingDistance = 100;
                tracker.trackerUpdateFrequency = 5;
            }
            if (droneComponent) { 
            droneComponent.ConsumeDroneStackAuthority(1);
            }
            if (firedDrone)
            {
                firedDrone = true;
                GenericDamageOrb genericDamageOrb = new WinchOrb();
                genericDamageOrb.damageValue = base.characterBody.damage * 1;
                genericDamageOrb.isCrit = RollCrit();
                genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                genericDamageOrb.attacker = base.gameObject;
                genericDamageOrb.procCoefficient = 1;
                HurtBox hurtBox = tracker.GetTrackingTarget();
                if (hurtBox)
                {
                    //EffectManager.SimpleMuzzleFlash(FireSeekingDrone.muzzleflashEffectPrefab, base.gameObject, FireSeekingDrone.muzzleString, true);
                    genericDamageOrb.origin = base.transform.position;
                    genericDamageOrb.target = hurtBox;
                    OrbManager.instance.AddOrb(genericDamageOrb);
                }



                OrbManager.instance.AddOrb(new WinchOrb());
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}*/
