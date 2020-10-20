using RoR2;
using UnityEngine.Networking;

namespace Starstorm2.Cores
{
    public class Starstorm2ItemManager : NetworkBehaviour
    {
        public HealthComponent health;

        public void Awake()
        {
            health = gameObject.GetComponent<HealthComponent>();
        }

        #region Dungus
        public void HealFractionAuthority(float frac)
        {
            if (NetworkServer.active)
            {
                HealFractionInternal(frac);
                return;
            }
            CmdHealFractionInternal(frac);
        }

        [Server]
        private void HealFractionInternal(float frac)
        {
            if (!health)
            {
                health = gameObject.GetComponent<HealthComponent>();
                LogCore.LogE(gameObject.name + "'s health component was null!");
            }
            health.HealFraction(frac, default);
        }

        [Command]
        private void CmdHealFractionInternal(float frac)
        {
            HealFractionInternal(frac);
        }
        #endregion
        #region Diary
        public void AddExperienceAuthority(uint experience)
        {
            if (NetworkServer.active)
            {
                AddExperienceInternal(experience);
                return;
            }
            else
            {
                //Chat.AddMessage("AddExperienceAuthority broke");
            }
            CmdAddExperienceInternal(experience);
        }

        [Server]
        private void AddExperienceInternal(uint experience)
        {
            TeamManager.instance.GiveTeamExperience(TeamIndex.Player, experience);
        }

        [Command]
        private void CmdAddExperienceInternal(uint experience)
        {
            AddExperienceInternal(experience);
        }
        #endregion
    }
}
