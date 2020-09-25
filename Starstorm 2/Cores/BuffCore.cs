using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores
{
    public class BuffCore
    {
        public static BuffCore instance;

        protected internal BuffIndex lightBringerIndex;
        protected internal BuffIndex greaterBannerIndex;
        protected internal BuffIndex detritiveIndex;

        public BuffCore() => RegisterBuffs();

        protected void RegisterBuffs()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            instance = this;

            RegisterBuff(new BuffDef
            {
                buffIndex = BuffIndex.Count,
                buffColor = Color.yellow,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/TexMovespeedBuffIcon",
                isDebuff = false,
                name = "LightBringer"
            });

            RegisterBuff(new BuffDef
            {
                buffColor = new Color(0.3764706f, 0.84313726f, 0.8980392f),
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffFullCritIcon",
                isDebuff = false,
                name = "Infested"
            });

            RegisterBuff(new BuffDef()
            {
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffWarbannerIcon",
                name = "GreaterWarbanner",
                buffColor = new Color(0.8392157f, 0.7882353f, 0.22745098f)
            });


            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
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


                if (self.HasBuff(lightBringerIndex))
                {
                    self.SetPropertyValue("attackSpeed", attackSpeed += 2);
                    self.SetPropertyValue("moveSpeed", moveSpeed += 2f);
                    self.SetPropertyValue("armor", armor += 20);
                }
                if (self.HasBuff(greaterBannerIndex))
                {
                    self.SetPropertyValue("crit", crit += .2f);
                    self.SetPropertyValue("regen", regen += .2f);
                    if (self.skillLocator)
                    {
                        var skillLocator = self.skillLocator;
                        //pssst kid, wanna buff greater warbanner?
                        //just turn to the 0.5f into a 0!
                        var reduction = .5f;
                        if (skillLocator.primary)
                        {
                            skillLocator.primary.cooldownScale = reduction;
                        }
                        if (skillLocator.secondary)
                        {
                            skillLocator.secondary.cooldownScale = reduction;
                        }
                        if (skillLocator.utility)
                        {
                            skillLocator.utility.cooldownScale = reduction;
                        }
                        if (skillLocator.special)
                        {
                            skillLocator.special.cooldownScale = reduction;
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
                case "LightBringer":
                    lightBringerIndex = BuffAPI.Add(customBuff);
                    break;
                case "Infested":
                    detritiveIndex = BuffAPI.Add(customBuff);
                    break;
                case "GreaterWarbanner":
                    greaterBannerIndex = BuffAPI.Add(customBuff);
                    break;
                default:
                    //feel my haunted lust
                    LogCore.LogF(string.Format("{0} doesn't have a case!", buffDef));
                    break;
            }
        }
    }
}
