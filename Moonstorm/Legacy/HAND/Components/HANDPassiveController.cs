using Cloudburst.HAND.Orbs;
using RoR2;
using RoR2.Orbs;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.HAND.Components
{
    [RequireComponent(typeof(CharacterBody))]
    public class HANDPassiveController : MonoBehaviour
    {
        public SkillDef armorDef;
        public SkillDef regenDef;
        public SkillDef speedDef;
        public GenericSkill passiveSkillSlot;
        public enum Passive
        {
            Invalid = -1,
            Regen,
            Armor,
            SPEED
        };
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


    }
}