using R2API;
using RoR2;
using System;

namespace Moonstorm.Achievements
{
    public class HitLevelCap : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    {
        // All of the strings defined here must be unique in order to interop with both the base game and other mods.
        // For that reason, you should always use something of the format "Author_Mod_Achievement_..."
        // Note that changing these values is a breaking change for your mod. All users will have to redo the challenge if the id changes.
        // Also, note that in most cases it is a good idea for you to add a config option to reset unlockable progress for users. 
        // Not only does this make testing much easier, but it also allows users to gain some potential replayability.


        // The name used to identify the achievement.
        public override String AchievementIdentifier { get; } = "CLOUDBURST_HITLEVELCAP_ACHIEVEMENT_ID";

        // The key used to identify things that are unlocked by this.
        public override String UnlockableIdentifier { get; } = "CLOUDBURST_HITLEVELCAP_REWARD_ID";

        // The key of a prereq for unlocking this. Use "" for none.
        public override String PrerequisiteUnlockableIdentifier { get; } = "";

        // Language token for the achievement name.
        public override String AchievementNameToken { get; } = "CLOUDBURST_HITLEVELCAP_ACHIEVEMENT_NAME";

        // Language token for the achievement description.
        public override String AchievementDescToken { get; } = "CLOUDBURST_HITLEVELCAP_ACHIEVEMENT_DESC";

        // Language token for the unlockable name.
        public override String UnlockableNameToken { get; } = "CLOUDBURST_HITLEVELCAP_UNLOCKABLE_NAME";

        // The sprite provider.
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("NotAPath");

        private CharacterBody body;

        public override void OnInstall() {
            base.OnInstall();
            localUser.onBodyChanged += LocalUser_onBodyChanged;
            RoR2Application.onFixedUpdate += RoR2Application_onFixedUpdate;
        }

        private void LocalUser_onBodyChanged()
        {
            body = localUser.cachedBody;
        }

        private void RoR2Application_onFixedUpdate()
        {
            LogCore.LogI(TeamManager.naturalLevelCap);
            if (body && body.level == TeamManager.naturalLevelCap) {
                base.Grant();
            }   
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            localUser.onBodyChanged -= LocalUser_onBodyChanged;
            RoR2Application.onFixedUpdate -= RoR2Application_onFixedUpdate;
        }
    }
}
