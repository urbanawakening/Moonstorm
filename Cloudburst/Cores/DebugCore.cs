namespace Cloudburst.Cores
{
    class DebugCore
    {
        public DebugCore() => Debug();

        protected void Debug() {
            LogCore.LogI("Initializing Core: " + base.ToString());
            Unlockall();
        }

        protected void Unlockall() {
            On.RoR2.UserProfile.HasUnlockable_string += (o, s, i) => true;
            On.RoR2.UserProfile.HasUnlockable_UnlockableDef += (o, s, i) => true;
            On.RoR2.UserProfile.HasSurvivorUnlocked += (o, s, i) => true;
            On.RoR2.UserProfile.HasDiscoveredPickup += (o, s, i) => true;
            On.RoR2.UserProfile.HasAchievement += (o, s, i) => true;
            On.RoR2.UserProfile.CanSeeAchievement += (o, s, i) => true;
            On.RoR2.Stats.StatSheet.HasUnlockable += (o, s, i) => true;
            On.RoR2.PreGameController.AnyUserHasUnlockable += (o, s) => true;

            On.RoR2.Run.IsUnlockableUnlocked += (o, s, i) => true;
            On.RoR2.Run.DoesEveryoneHaveThisUnlockableUnlocked += (o, s, i) => true;
        }
    }
}
