/*using R2API;
using RoR2;
using System;

namespace Cloudburst.Achievements

{
    public class GrabOrDie : ModdedUnlockable<VanillaSpriteProvider>
    {
        // All of the strings defined here must be unique in order to interop with both the base game and other mods.
        // For that reason, you should always use something of the format "Author_Mod_Achievement_..."
        // Note that changing these values is a breaking change for your mod. All users will have to redo the challenge if the id changes.
        // Also, note that in most cases it is a good idea for you to add a config option to reset unlockable progress for users. 
        // Not only does this make testing much easier, but it also allows users to gain some potential replayability.


        // The name used to identify the achievement.
        public override String AchievementIdentifier { get; } = "CLOUDBURST_GRABORDIE_ACHIEVEMENT_ID";

        // The key used to identify things that are unlocked by this.
        public override String UnlockableIdentifier { get; } = "CLOUDBURST_GRABORDIE_REWARD_ID";

        // The key of a prereq for unlocking this. Use "" for none.
        public override String PrerequisiteUnlockableIdentifier { get; } = "";

        // Language token for the achievement name.
        public override String AchievementNameToken { get; } = "CLOUDBURST_GRABORDIE_ACHIEVEMENT_NAME";

        // Language token for the achievement description.
        public override String AchievementDescToken { get; } = "CLOUDBURST_GRABORDIE_ACHIEVEMENT_DESC";

        // Language token for the unlockable name.
        public override String UnlockableNameToken { get; } = "CLOUDBURST_GRABORDIE_UNLOCKABLE_NAME";

        // The sprite provider.
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("VANILLA PATH");

        public override void OnInstall()
        {
            base.OnInstall();
            base.localUser.onMasterChanged += this.OnMasterChanged;
            this.SetupMasterController(base.localUser.cachedMasterController);  
        }

        public override void OnUninstall()
        {
            this.SetupMasterController(null);
            base.localUser.onMasterChanged -= this.OnMasterChanged;
            base.OnUninstall();
        }

        private void SetupMasterController(PlayerCharacterMasterController anotherMasterController)
        {
            if (this.currentMasterController == anotherMasterController) return;
            if (this.inv != null) this.inv.onInventoryChanged -= this.OnInventoryChanged;
            
            this.currentMasterController = anotherMasterController;
            PlayerCharacterMasterController playerCharacterMasterController = this.currentMasterController;
            Inventory anotherFuckinInv;
            if (playerCharacterMasterController == null) {
                anotherFuckinInv = null;
            }
            else {
                CharacterMaster master = playerCharacterMasterController.master;
                anotherFuckinInv = (master != null) ? master.inventory : null;
            }
            this.inv = anotherFuckinInv;
            if (this.inv != null) {
                this.inv.onInventoryChanged += this.OnInventoryChanged;
            }
        }
        private void OnInventoryChanged()
        {
            if (this.inv && base.localUser.cachedBody)
            {
                if (this.localUser.cachedBody.healthComponent.combinedHealthFraction < 0.3f)
                {
                    base.Grant();
                }
            }
        }
        private void OnMasterChanged()
        {
            this.SetupMasterController(base.localUser.cachedMasterController);
        }

        private PlayerCharacterMasterController currentMasterController;

        private Inventory inv;
    }
}*/