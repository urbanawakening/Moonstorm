using RoR2;
using RoR2.Orbs;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.HAND.Components
{
    [RequireComponent(typeof(CharacterBody))]
    public class HANDPassiveController : NetworkBehaviour, IOnDamageDealtServerReceiver
    {
        public SkillDef armorDef;
        public SkillDef regenDef;
        public SkillDef speedDef;
        public GenericSkill passiveSkillSlot;
        private CharacterBody body;
        public enum Passive
        {
            Invalid = -1,
            Regen,
            Armor,
            SPEED
        };
        public void Awake() {
            body = base.GetComponent<CharacterBody>();
        }
        public Passive GetBonus()
        {
            if (this.passiveSkillSlot)
            {
                if (this.passiveSkillSlot.skillDef == armorDef)
                {
                    return Passive.Armor;
                }
                if (this.passiveSkillSlot.skillDef == regenDef)
                {
                    return Passive.Regen;
                }
                if (this.passiveSkillSlot.skillDef == speedDef)
                {
                    return Passive.SPEED;
                }
            }
            return Passive.Invalid;
        }

        public void OnDamageDealtServer(DamageReport damageReport)
        {
            if (body.GetBuffCount(BuffCore.instance.droneIndex) < 10)
            {
                body.AddBuff(BuffCore.instance.droneIndex);
            }
        }
        public void ConsumeDroneStackAuthority(float stacks)
        {
            if (NetworkServer.active)
            {
                RemoveDroneStackInternal(stacks);
                return;
            }
            CmdRemoveDroneStack(stacks);
        }

        private void RemoveDroneStackInternal(float stacks)
        {
            for (int i = 0; i < stacks; i++)
            {
                body.RemoveBuff(BuffCore.instance.droneIndex);

            }
        }

        [Command]
        private void CmdRemoveDroneStack(float stacks)
        {
            RemoveDroneStackInternal(stacks);
        }
    }
}