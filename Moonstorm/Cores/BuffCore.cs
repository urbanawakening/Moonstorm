using Moonstorm.Cores.HAND.Components;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace Moonstorm.Cores
{
    public class BuffCore
    {
        public static BuffCore instance;

        protected internal BuffIndex scrapIndex;
        protected internal BuffIndex overclockIndex;
        protected internal BuffIndex droneIndex;
        protected internal BuffIndex surgeIndex;
        protected internal BuffIndex cleanIndex;
        protected internal BuffIndex bombardierForceIndex;
        protected internal BuffIndex baboomChargeIndex;
        protected internal BuffIndex skinIndex;
        protected internal BuffIndex antiGravIndex;
        protected internal BuffIndex antiGravFriendlyIndex;
        protected internal BuffIndex wyattCombatIndex;

        internal bool Loaded { get; private set; } = false;
        public BuffCore() => RegisterBuffs();

        protected void RegisterBuffs()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            instance = this;

            RegisterBuff(new BuffDef
            {
                buffIndex = BuffIndex.Count,
                //buffColor = Color.yellow,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "@EngimaHANDREBOOTED:Assets/Import/HAND_ICONS/Passive.png",
                isDebuff = false,
                name = "Drone",
            });

            RegisterBuff(new BuffDef
            {
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "@EngimaHANDREBOOTED:Assets/Import/HAND_ICONS/OverclockBuff.png",
                isDebuff = false,
                name = "Overclock"
            });

            RegisterBuff(new BuffDef
            {
                buffColor = new Color(0.3764706f, 0.84313726f, 0.8980392f),
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texbuffonfireicon",
                isDebuff = false,
                name = "Surge"
            });

            RegisterBuff(new BuffDef
            {
                buffColor = new Color(0.3764706f, 0.84313726f, 0.8980392f),
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texbuffonfireicon",
                isDebuff = false,
                name = "Sparkle"
            });
            RegisterBuff(new BuffDef()
            {
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texMovespeedBuffIcon",
                name = "BombardierForce",
                buffColor = new Color(0.8392157f, 0.7882353f, 0.22745098f)
            });
            RegisterBuff(new BuffDef()
            {
                buffIndex = BuffIndex.Count,
                canStack = true,
                isDebuff = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texMovespeedBuffIcon",
                name = "BaboonCharge",
                buffColor = new Color(0.8392157f, 0.7882353f, 0.22745098f)
            });
            RegisterBuff(new BuffDef()
            {
                buffIndex = BuffIndex.Count,
                canStack = true,
                isDebuff = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffBodyArmorIcon",
                name = "SkinStack",
                buffColor = new Color32(219, 224, 198, byte.MaxValue)
            });
            RegisterBuff(new BuffDef()
            {
                buffIndex = BuffIndex.Count,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffPulverizeIcon",
                name = "AntiGrav",
                buffColor = new Color(0.6784314f, 0.6117647f, 0.4117647f)
            });
            RegisterBuff(new BuffDef()
            {
                buffIndex = BuffIndex.Count,
                canStack = true,
                isDebuff = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffTeslaIcon",
                name = "WyattCombat",
                buffColor = new Color(1f, 0.7882353f, 0.05490196f)
            });
            RegisterBuff(new BuffDef()
            {
                buffIndex = BuffIndex.Count,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffGenericShield",
                name = "AntiGravFriendly",
                buffColor = new Color(0.6784314f, 0.6117647f, 0.4117647f)
            });

            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.RemoveBuff += CharacterBody_RemoveBuff;
            On.RoR2.CharacterBody.AddBuff += CharacterBody_AddBuff;
        }

        private void CharacterBody_AddBuff(On.RoR2.CharacterBody.orig_AddBuff orig, CharacterBody self, BuffIndex buffType)
        {
            bool shouldOrigSelf = true;
            if (self)
            {
                if (buffType == this.antiGravIndex && self.baseNameToken == "WYATT_BODY_NAME")
                {
                    shouldOrigSelf = false;
                    //self.AddTimedBuff(this.antiGravFriendlyIndex, 10);
                }
            }
            if (shouldOrigSelf)
            {
                orig(self, buffType);
            }
        }

        private void CharacterBody_RemoveBuff(On.RoR2.CharacterBody.orig_RemoveBuff orig, CharacterBody self, BuffIndex buffType)
        {
            if (buffType == antiGravIndex)
            {
                if (self.characterMotor)
                {
                    self.characterMotor.useGravity = true;
                }
            }
            orig(self, buffType);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self)
            {
                var attackSpeed = self.attackSpeed;
                var armor = self.armor;
                var moveSpeed = self.moveSpeed;
                var regen = self.regen;
                var crit = self.crit;

                var controller = self.gameObject.GetComponent<HANDPassiveController>();

                if (self.HasBuff(overclockIndex))
                {
                    //BaseLogger.Log(attackSpeed.ToString());   
                    self.SetPropertyValue("attackSpeed", attackSpeed += .5f);
                    self.SetPropertyValue("moveSpeed", moveSpeed += .4f);

                }
                if (self.HasBuff(overclockIndex))
                {
                    //BaseLogger.Log(attackSpeed.ToString());   
                    self.SetPropertyValue("attackSpeed", attackSpeed += .5f);
                    self.SetPropertyValue("moveSpeed", moveSpeed += .4f);

                }
                if (self.HasBuff(skinIndex)) // && controller)
                {
                    self.SetPropertyValue("armor", armor += 5f);
                }
                if (self.HasBuff(antiGravIndex))
                {
                    if (self.characterMotor)
                    {
                        self.characterMotor.useGravity = false;
                    }
                    self.SetPropertyValue("attackSpeed", attackSpeed -= .5f);
                    self.SetPropertyValue("moveSpeed", moveSpeed -= 1f);
                }
                if (self.HasBuff(antiGravFriendlyIndex)) {
                    self.SetPropertyValue("moveSpeed", moveSpeed += 6f);
                }
                if (self.HasBuff(wyattCombatIndex)) {
                    for (int i = 0; i < self.GetBuffCount(wyattCombatIndex); i++) {
                        self.SetPropertyValue("moveSpeed", moveSpeed += 1f);
                        self.SetPropertyValue("regen", regen += .1f);
                    }
                }
                if (self.HasBuff(droneIndex)) // && controller)
                {

                    var currentPassive = controller.GetBonus();
                    //Debug.Log(currentPassive);
                    for (int i = 0; i < self.GetBuffCount(droneIndex); i++)
                    {
                        switch (currentPassive)
                        {
                            //space nazis
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
            }
        }

        protected internal void RegisterBuff(BuffDef buffDef)
        {
            var customBuff = new CustomBuff(buffDef);
            switch (buffDef.name)
            {
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
                    cleanIndex = BuffAPI.Add(customBuff);
                    break;
                case "Scrap":
                    scrapIndex = BuffAPI.Add(customBuff);
                    break;
                case "BombardierForce":
                    bombardierForceIndex = BuffAPI.Add(customBuff);
                    break;
                case "BaboonCharge":
                    baboomChargeIndex = BuffAPI.Add(customBuff);
                    break;
                case "SkinStack":
                    skinIndex = BuffAPI.Add(customBuff);
                    break;
                case "AntiGrav":
                    antiGravIndex = BuffAPI.Add(customBuff);
                    break;
                case "WyattCombat":
                    wyattCombatIndex = BuffAPI.Add(customBuff);
                    break;
                case "AntiGravFriendly":
                    antiGravFriendlyIndex = BuffAPI.Add(customBuff);
                    break;
                //throw new System.NotImplementedException("not implemented yet!");
                default:
                    //feel my haunted lust
                    LogCore.LogF(string.Format("{0} doesn't have a case!", buffDef.name));
                    break;
            }
        }
    }
}
