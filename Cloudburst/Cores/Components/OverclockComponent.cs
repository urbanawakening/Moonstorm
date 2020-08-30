using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores.HAND.Components
{
    [RequireComponent(typeof(CharacterBody))]
    public class OverclockComponent : NetworkBehaviour, IOnDamageDealtServerReceiver, IOnTakeDamageServerReceiver
    {
        private CharacterBody body;

        [SyncVar]
        private bool _overclockOn;

        [SyncVar]
        private bool _surgeOn;

        [SyncVar]
        private float _duration;

        private void Awake() {
            this.body = base.GetComponent<CharacterBody>();
        }
        #region Interface Implementation
        public void OnDamageDealtServer(DamageReport damageReport) {
            if (_overclockOn) {
                this.AddDurationServer(2);
                body.healthComponent.Heal(damageReport.damageDealt / 2, default);
            }
        }
        public void OnTakeDamageServer(DamageReport damageReport) {
            //this.AddOverclockServer(damageReport.damageDealt);
        }
        #endregion
        [Server]
        public void AddDurationServer(float duration) {
            this._duration = 0;
            this._duration = duration;
        }
        public DamageType GetDamageType() {
            if (_overclockOn && Util.CheckRoll(50, body.master)) {
                return DamageType.Stun1s;
            }
            return DamageType.Generic;
        }

        public void ActivateOverclockAuthority()
        {
            if (NetworkServer.active)
            {
                this.ActivateOverclockInternal();
                return;
            }
            this.CmdActivateOverclock();
        }
        public void ActivateSurgeAuthority()
        {
            if (NetworkServer.active)
            {
                this.ActivateSurgeInternal();
                return;
            }
            this.CmdActivateSurge();
        }

        [Server]
        private void ActivateOverclockInternal() {
            this._duration = 2;
            body.AddBuff(BuffCore.instance.overclockIndex);
            body.gameObject.GetComponent<SetStateOnHurt>().canBeFrozen = false;
            this._overclockOn = true;
        }

        [Server]
        private void DeactivateOverclockInternal()
        {
            this._duration = 0;
            body.RemoveBuff(BuffCore.instance.overclockIndex);
            body.gameObject.GetComponent<SetStateOnHurt>().canBeFrozen = true;
            this._overclockOn = false;
        }
        #region Surge
        [Server]
        private void ActivateSurgeInternal()
        {
            this._surgeOn = true;
            body.AddBuff(BuffCore.instance.overclockIndex);
            this._duration = 1;
        }
        [Server]
        private void DeactivateSurgeInternal()
        {
            this._surgeOn = false;
            body.RemoveBuff(BuffCore.instance.overclockIndex);
            this._duration = 0;
        }
        #endregion

        private void FixedUpdate()
        {
            if (NetworkServer.active && this._overclockOn || this._surgeOn) {
                _duration -= Time.deltaTime;
                if (_duration <= 0) {
                    _duration = 0;
                    if (_overclockOn) {
                        this.DeactivateOverclockInternal();
                    }
                    if (this._surgeOn) {
                        this.DeactivateSurgeInternal();
                    } 
                }
            }
         }
        #region Commands
        #region Overclock
        [Command]
        private void CmdActivateOverclock() {
            this.ActivateOverclockInternal();
        }
        [Command]
        private void CmdDeactivateOverclock() {
            this.DeactivateOverclockInternal();
        }
        #endregion
        [Command]
        private void CmdActivateSurge()
        {
            this.ActivateSurgeInternal();
        }

        [Command]
        private void CmdDeactivateSurge()
        {
            this.DeactivateSurgeInternal();
        }
        #endregion
    }
}
