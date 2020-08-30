using Cloudburst.Cores.HAND.Components;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace Cloudburst.Cores
{
    public class BuffCore
    {
        public static BuffCore instance;

        protected internal BuffIndex lightBringerIndex;
        protected internal BuffIndex scrapIndex;
        protected internal BuffIndex overclockIndex;
        protected internal BuffIndex droneIndex;
        protected internal BuffIndex surgeIndex;
        protected internal BuffIndex cleanIndex;
        
        internal bool Loaded { get; private set; } = false;
        public BuffCore() => RegisterBuffs(); 

        protected void RegisterBuffs() {
            instance = this;

            RegisterBuff(lightBringerIndex, new BuffDef
            {
                buffIndex = BuffIndex.Count,
                buffColor = Color.yellow,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/TexMovespeedBuffIcon",
                isDebuff = false,
                name = "LightBringer"
            });

            RegisterBuff(droneIndex, new BuffDef
            {
                buffIndex = BuffIndex.Count,
                //buffColor = Color.yellow,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "@EngimaHANDREBOOTED:Assets/Import/HAND_ICONS/Passive.png",
                isDebuff = false,
                name = "Drone",
            });

            RegisterBuff(overclockIndex, new BuffDef
            {
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "@EngimaHANDREBOOTED:Assets/Import/HAND_ICONS/OverclockBuff.png",
                isDebuff = false,
                name = "Overclock"
            });

            RegisterBuff(surgeIndex, new BuffDef
            {
                buffColor = new Color(0.3764706f, 0.84313726f, 0.8980392f),
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texbuffonfireicon",
                isDebuff = false,
                name = "Surge"
            });

            RegisterBuff(cleanIndex, new BuffDef
            {   
                buffColor = new Color(0.3764706f, 0.84313726f, 0.8980392f),
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texbuffonfireicon",
                isDebuff = false,
                name = "Sparkle"
            });

            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            float attackSpeed = self.attackSpeed;
            float armor = self.armor;
            float moveSpeed = self.moveSpeed;
            float regen = self.regen;

            HANDPassiveController controller = self.gameObject.GetComponent<HANDPassiveController>();

            if (self.HasBuff(overclockIndex))
            {
                //BaseLogger.Log(attackSpeed.ToString());   
                self.SetPropertyValue("attackSpeed", attackSpeed += .5f);
                self.SetPropertyValue("moveSpeed", moveSpeed += .4f);

            }
            if (self.HasBuff(droneIndex)) // && controller)
            {
                var currentPassive = controller.GetBonus();
                //Debug.Log(currentPassive);
                for (int i = 0; i < self.GetBuffCount(droneIndex); i++)
                {
                    switch (currentPassive)
                    {
                        case HANDPassiveController.Passive.Armor:
                            self.SetPropertyValue("armor", armor += 10f);
                            break;
                        case HANDPassiveController.Passive.Regen:
                            self.SetPropertyValue("regen", regen += .2f);
                            break;
                        case HANDPassiveController.Passive.SPEED:
                            self.SetPropertyValue("moveSpeed", moveSpeed += 1f);
                            break;
                    }
                }
            }
            if (self.HasBuff(lightBringerIndex))
            {


                self.SetPropertyValue("attackSpeed", attackSpeed += 2);
                self.SetPropertyValue("moveSpeed", moveSpeed += 2f);
                self.SetPropertyValue("armor", armor += 20);
            }
        }

        protected internal void RegisterBuff(BuffIndex buffIndex    , BuffDef buffDef) {
            var customBuff = new CustomBuff(buffDef);
            switch (buffDef.name) {
                case "LightBringer":
                    lightBringerIndex = BuffAPI.Add(customBuff);
                    break;
                case "Drone":
                    droneIndex = BuffAPI.Add(customBuff);
                    break;
                case "Overclock":
                    overclockIndex = BuffAPI.Add(customBuff);
                    break;
                case "Surge":
                    surgeIndex = BuffAPI.Add(customBuff);
                    break;
                case "Sparkle":
                    LogCore.LogI("Hi!");
                    cleanIndex = BuffAPI.Add(customBuff);
                    break;
                case "Scrap":
                    scrapIndex = BuffAPI.Add(customBuff);
                    break;
                default:
                    LogCore.LogF(string.Format("{0} doesn't have a switch case!", buffDef));
                    break;
            }
        }
    }
}
