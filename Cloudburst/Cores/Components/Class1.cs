using System;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using Cloudburst.Cores.HAND.Components;
using Cloudburst.Cores.Components;

namespace Cloudburst.Cores.Skills
{
    public class BombardierTrackingSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                tracker = skillSlot.GetComponent<BombardierTracker>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            HANDDroneTracker HANDDroneTracker = ((BombardierTrackingSkillDef.InstanceData)skillSlot.skillInstanceData).tracker;
            return (HANDDroneTracker != null) ? HANDDroneTracker.GetTrackingTarget() : null;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return BombardierTrackingSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HasTarget(skillSlot);
        }


        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {

            public BombardierTracker tracker;
        }
    }
}
