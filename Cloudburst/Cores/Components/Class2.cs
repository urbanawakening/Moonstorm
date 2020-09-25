using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores.Components
{
    class BombardierStickyBombManager : NetworkBehaviour
    {
        [SyncVar]
        private GameObject[] bombs;
        public void Awake() {

        }

        public void AddBombAuthority() {
            if (NetworkServer.active) {
                AddStickyInternal();
                return;
            }
            CmdAddSticky();
        }

        [Server]
        private void AddStickyInternal() {

        }

        [Command]
        private void CmdAddSticky() {
            AddStickyInternal();
        }

        public void FixedUpdate() {

        }
    }
}
