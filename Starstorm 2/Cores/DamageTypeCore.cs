using RoR2;
using System;

namespace Starstorm2.Cores
{
    class DamageTypeCore
    {
        private UInt32 currentMax = 0u;

        public static DamageTypeCore instance;

        public static DamageType burnOnHit;

        public DamageTypeCore()
        {
            instance = this;
            LogCore.LogI("Initializing Core: " + base.ToString());

            //maxPossible = 0b_1ul << 32;

            var damageValues = Enum.GetValues(typeof(DamageType)) as UInt32[];
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
            if (damageInfo.damageType.HasFlag(burnOnHit) && self && self.body && damageInfo.attacker)
            {
                DotController.InflictDot(self.gameObject, damageInfo.attacker, DotController.DotIndex.Burn, 5);
            }

            orig(self, damageInfo);

        }

        protected void AddDamageTypes()
        {
            burnOnHit = AddDamageType();
            LogCore.LogI(burnOnHit);
        }

        protected DamageType AddDamageType()
        {
            currentMax *= 2;
            var damageType = (DamageType)currentMax;
            return damageType;
        }
    }
}