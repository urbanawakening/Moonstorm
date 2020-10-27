using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores
{
    public class BuffCore
    {
        public static BuffCore instance;

        protected internal BuffIndex lightBringerIndex;
        protected internal BuffIndex detritiveIndex;
        protected internal BuffIndex greaterBannerIndex;
        protected internal BuffIndex strangeCanPoisonIndex;
        protected internal BuffIndex sigilBuffIndex;
        protected internal BuffIndex greenChocBuffIndex;

        protected internal BuffIndex gougeIndex;
        protected internal BuffIndex awarenessIndex;

        private GameObject greenChocPrefab;
        private GameObject greenChocEffect;

        public BuffCore()
        {
            RegisterBuffs();
            RegisterEffects();
            Hook();
        }

        protected void RegisterBuffs()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            instance = this;

            lightBringerIndex = RegisterBuff(new BuffDef
            {
                //buffIndex = BuffIndex.Count,
                buffColor = Color.yellow,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/TexMovespeedBuffIcon",
                isDebuff = false,
                name = "LightBringer"
            });

            detritiveIndex = RegisterBuff(new BuffDef
            {
                buffColor = new Color(0.3764706f, 0.84313726f, 0.8980392f),
                //buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffFullCritIcon",
                isDebuff = false,
                name = "Infested"
            });

            greaterBannerIndex = RegisterBuff(new BuffDef
            {
                //buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffWarbannerIcon",
                name = "GreaterWarbanner",
                buffColor = new Color(0.8392157f, 0.7882353f, 0.22745098f)
            });

            strangeCanPoisonIndex = RegisterBuff(new BuffDef
            {
                //buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffBleedingIcon",
                name = "StrangeCanPoison",
                isDebuff = true,
                buffColor = Color.green
            });

            sigilBuffIndex = RegisterBuff(new BuffDef
            {
                canStack = false,
                iconPath = "@Starstorm2:Assets/Items/HuntersSigil/HuntersSigil_Icon.png",
                name = "SigilCritDefBoost",
                isDebuff = false,
                buffColor = Color.white
            });

            greenChocBuffIndex = RegisterBuff(new BuffDef
            {
                canStack = false,
                iconPath = "@Starstorm2:Assets/Items/GreenChocolate/GreenChocolate_Icon.png",
                name = "GreenChocAttackBoost",
                isDebuff = false,
                buffColor = Color.white
            });

            #region Nemmando

            gougeIndex = RegisterBuff(new BuffDef
            {
                buffIndex = BuffIndex.Count,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffBleedingIcon",
                name = "Gouge",
                isDebuff = true,
                buffColor = Color.red
            });

            awarenessIndex = RegisterBuff(new BuffDef
            {
                buffIndex = BuffIndex.Count,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffBleedingIcon",
                name = "Awareness",
                isDebuff = false,
                buffColor = Color.green
            });
            #endregion
        }

        private void RegisterEffects()
        {
            greenChocPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/WarCryEffect"), "GreenChocEffect", true);
            greenChocPrefab.AddComponent<NetworkIdentity>();
            var particles = greenChocPrefab.GetComponentInChildren<ParticleSystem>();
            if (particles)
            {
                var mainmod = particles.main;
                //this doesn't do anything :(
                mainmod.startColor = new Color(0.58f, 0.90f, 0.66f);
            }
        }

        private void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.OnClientBuffsChanged += CharacterBody_OnClientBuffsChanged;
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
                    //self.SetPropertyValue("crit", crit += 20f); //this is 0.2% crit chance (not what you wanted?)
                    //self.SetPropertyValue("regen", regen += 20f); //and this is 0.2% max hp/sec
                    self.crit += 20f;
                    self.regen = self.baseRegen * 1.5f;
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
                if (self.HasBuff(greenChocBuffIndex))
                {
                    self.crit += 30f;
                    self.damage += (self.baseDamage + self.levelDamage * (self.level - 1)) * 0.5f;
                }
            }
        }

        private void CharacterBody_OnClientBuffsChanged(On.RoR2.CharacterBody.orig_OnClientBuffsChanged orig, CharacterBody self)
        {
            orig(self);
            if (self.HasBuff(greenChocBuffIndex) && !greenChocEffect)
            {
                Transform transform = self.mainHurtBox ? self.mainHurtBox.transform : self.transform;
                if (transform)
                {
                    /*
                    EffectData effectData = new EffectData();
                    effectData.color = new Color(0.58f, 0.90f, 0.66f);
                    effectData.start = transform.position;
                    EffectManager.SpawnEffect(greenChocPrefab, effectData, true);
                    */
                    greenChocEffect = Object.Instantiate(greenChocPrefab, transform.position, Quaternion.identity, transform);
                }
            }
            if (!self.HasBuff(greenChocBuffIndex) && greenChocEffect)
            {
                Object.Destroy(greenChocEffect);
            }
        }

        protected internal BuffIndex RegisterBuff(BuffDef buffDef)
        {
            var customBuff = new CustomBuff(buffDef);
            return BuffAPI.Add(customBuff);
        }
    }
}