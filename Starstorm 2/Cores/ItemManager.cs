using RoR2;
using UnityEngine.Networking;

namespace Starstorm2.Cores
{
    class Starstorm2ItemManager : NetworkBehaviour
    {
        public void AddExperienceAuthority(uint experience) {
            if (NetworkServer.active) {
                AddExperienceInternal(experience);
                return;
            }
            CmdAddExperienceInternal(experience);
        }

        [Server]
        private void AddExperienceInternal(uint experience) {
            TeamManager.instance.GiveTeamExperience(TeamIndex.Player, experience);
        }

        [Command]
        private void CmdAddExperienceInternal(uint experience) {
            AddExperienceInternal(experience);
        }
    }
}
