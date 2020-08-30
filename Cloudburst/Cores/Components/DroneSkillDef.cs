using System;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using Cloudburst.Cores.HAND.Components;

namespace Cloudburst.Cores.HAND.Skills
{
    public class HANDDroneSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new HANDDroneSkillDef.InstanceData
            {
                droneTracker = skillSlot.GetComponent<HANDDroneTracker>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            HANDDroneTracker HANDDroneTracker = ((HANDDroneSkillDef.InstanceData)skillSlot.skillInstanceData).droneTracker;
            return (HANDDroneTracker != null) ? HANDDroneTracker.GetTrackingTarget() : null;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HANDDroneSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HasTarget(skillSlot);
        }


        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {

            public HANDDroneTracker droneTracker;
        }
    }
}
