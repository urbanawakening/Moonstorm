using R2API.Networking;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starstorm2.Cores
{
    class DamageTypeCore
    {
        private UInt32 currentMax = 0u;

        public static DamageTypeCore instance;

        public DamageTypeCore() {
            instance = this;
            LogCore.LogI("Initializing Core: " + base.ToString());

            //maxPossible = 0b_1ul << 32;

            var damageValues = Enum.GetValues(typeof(RoR2.DamageType)) as UInt32[];
            for (int i = 0; i < damageValues.Length; i++)
            {
                var value = damageValues[i];
                if (value > currentMax)
                {
                    currentMax = value;
                }
            }
            AddDamageTypes();

            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
        }

        protected void AddDamageTypes() {
        }

        protected DamageType AddDamageType() {
            currentMax *= 2;
            var damageType = (RoR2.DamageType)currentMax;
            return damageType;
        }
    }
}
