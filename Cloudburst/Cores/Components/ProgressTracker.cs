using UnityEngine.Networking;

namespace Cloudburst.Cores.Components
{
    class ProgressTracker : NetworkBehaviour {
        [SyncVar]
        public static bool isVoidCompleted;

        public static ProgressTracker instance;

        public void OnEnable() {
            SingletonHelper.Assign<ProgressTracker>(instance, this);
            LogCore.LogI("ProgressTracker attached.");
            RoR2.ArenaMissionController.onBeatArena += ArenaMissionController_onBeatArena;
        }

        private void ArenaMissionController_onBeatArena()
        {
            isVoidCompleted = true;
            LogCore.LogI("Void completed! Releasing void elites!");
        }

        public static bool IsVoidComplete() {
            return isVoidCompleted;
        }

        public void OnDisable() {
            SingletonHelper.Unassign<ProgressTracker>(instance, this);
            LogCore.LogI("ProgressTracker destroyed.");
        }
    }
}
