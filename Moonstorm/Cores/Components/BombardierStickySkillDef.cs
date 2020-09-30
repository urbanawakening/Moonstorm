using JetBrains.Annotations;
using Moonstorm.Cores.Components;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.Skills
{
    public class BombardierStickySkillDef : SkillDef {
        private float timer;
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            timer = 0;
            return new BombardierStickySkillDef.InstanceData
            {
                identity = skillSlot.GetComponent<NetworkIdentity>(),
                input = skillSlot.GetComponent<InputBankTest>(),
                stickyManager = skillSlot.GetComponent<BombardierStickyBombManager>()
            };
        }
        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot)
        {
            base.OnFixedUpdate(skillSlot);
            if (!skillSlot.CanExecute()) {
                timer += Time.deltaTime;
            }
            else {
                timer = 0;
            }

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (!skillSlot.CanExecute() && timer > .2f && instanceData != null && instanceData.input && instanceData.input.skill2.down) {
                if (Util.HasEffectiveAuthority(instanceData.identity))
                {
                    instanceData.stickyManager.PopStickiesAuthority();
                }
            }
        }

        public class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public NetworkIdentity identity;
            public InputBankTest input;
            public BombardierStickyBombManager stickyManager;
        }
    }
}