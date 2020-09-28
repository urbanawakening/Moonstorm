namespace Moonstorm.Cores
{
    class DebuggingCore
    {
        public static DebuggingCore instance;

        public DebuggingCore() => Debug();

        protected void Debug() {
            LogCore.LogI("Initializing Core: " + base.ToString());
            instance = this;

            //This is so we can connect to ourselves.
            //Instructions:
            //Step One: Assuming this line is in your codebase, start two instances of RoR2 (do this through the .exe directly)
            //Step Two: Host a game with one instance of RoR2.
            //Step Three: On the instance that isn't hosting, open up the console (ctrl + alt + tilde) and enter the command "connect localhost:7777"
            //DO NOT MAKE A MISTAKE SPELLING THE COMMAND OR YOU WILL HAVE TO RESTART THE CLIENT INSTANCE!!
            //Step Four: Test whatever you were going to test.
            On.RoR2.Networking.GameNetworkManager.OnClientConnect += (self, user, t) => { };

            //On.EntityStates.Missions.Arena.NullWard.Active.FixedUpdate += Active_FixedUpdate;
            //On.EntityStates.Toolbot.AimGrenade.OnEnter += AimGrenade_OnEnter;
            //On.EntityStates.Commando.CommandoWeapon.FireRocket.OnEnter += FireRocket_OnEnter;
            //On.RoR2.GenericSkill.ExecuteIfReady += GenericSkill_ExecuteIfReady;
            Unlockall();
        }

        private void AimGrenade_OnEnter(On.EntityStates.Toolbot.AimGrenade.orig_OnEnter orig, EntityStates.Toolbot.AimGrenade self)
        {
            LogCore.LogI(self.projectilePrefab);
            orig(self);
        }

        private bool GenericSkill_ExecuteIfReady(On.RoR2.GenericSkill.orig_ExecuteIfReady orig, RoR2.GenericSkill self)
        {
            //LogCore.LogI(self.activationState.stateType.FullName);
            return orig(self);
        }

        private void FireRocket_OnEnter(On.EntityStates.Commando.CommandoWeapon.FireRocket.orig_OnEnter orig, EntityStates.Commando.CommandoWeapon.FireRocket self)
        {
            orig(self);
            LogCore.LogI("hi");
        }

        private void Active_FixedUpdate(On.EntityStates.Missions.Arena.NullWard.Active.orig_FixedUpdate orig, EntityStates.Missions.Arena.NullWard.Active self)
        {
            self.outer.SetNextState(new EntityStates.Missions.Arena.NullWard.Complete());
            return;
        }

        protected void Unlockall()
        {
            /*On.RoR2.UserProfile.HasUnlockable_string += (o, s, i) => true;
            On.RoR2.UserProfile.HasUnlockable_UnlockableDef += (o, s, i) => true;
            On.RoR2.UserProfile.HasSurvivorUnlocked += (o, s, i) => true;
            On.RoR2.UserProfile.HasDiscoveredPickup += (o, s, i) => true;
            On.RoR2.UserProfile.HasAchievement += (o, s, i) => true;
            On.RoR2.UserProfile.CanSeeAchievement += (o, s, i) => true;
            On.RoR2.Stats.StatSheet.HasUnlockable += (o, s, i) => true;
            On.RoR2.PreGameController.AnyUserHasUnlockable += (o, s) => true;

            On.RoR2.Run.IsUnlockableUnlocked += (o, s, i) => true;
            On.RoR2.Run.DoesEveryoneHaveThisUnlockableUnlocked += (o, s, i) => true;*/
        }
    }
}
