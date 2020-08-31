using EntityStates;
using RoR2;
using UnityEngine;

namespace Cloudburst.Cores.States.AncientWisp
{
    class RAIN : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void PlaceRain() {

        }

        private Transform FindOppositeTeamMemberClosest(TeamIndex enemyTeam, Vector3 position) {
            var teamMembers = TeamComponent.GetTeamMembers(enemyTeam);
            float maxDistance = 99999f;
            Transform result = null;
            for (int i = 0; i < teamMembers.Count; i++)
            {
                float spaceInBetween = Vector3.SqrMagnitude(teamMembers[i].transform.position - position);
                if (spaceInBetween < maxDistance)
                {
                    maxDistance = spaceInBetween;
                    result = teamMembers[i].transform;
                }
            }
            return result;
        }
    }
}
