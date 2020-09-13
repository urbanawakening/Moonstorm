using UnityEngine.Networking;

namespace Cloudburst.Cores.Components
{
    class ProgressTracker : NetworkBehaviour {
        [SyncVar]
        public static bool isVoidCompleted;

        public static ProgressTracker instance;

        public void OnEnable() {

        }

        public void OnDisable() {

        }
    }
}
