using R2API.Utils;
using RoR2;
using System;
using UnityEngine;

namespace Moonstorm.Cores
{
    class DamageTypeCore
    {
        private UInt32 currentMax = 0u;

        public static DamageTypeCore instance;

        public static DamageType bombardierKnockback;
        public static DamageType antiGrav;
        public static DamageType pullEnemies;
        public static DamageType spiked;

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
            bool isOsp = (damageInfo.damageType & DamageTypeCore.bombardierKnockback) != DamageType.Generic;
            bool antiGravType = (damageInfo.damageType & antiGrav) != DamageType.Generic;
            bool spikedType = (damageInfo.damageType & spiked) != DamageType.Generic;
            bool falldmgType = (damageInfo.damageType & DamageType.FallDamage) != DamageType.Generic;
            bool pullType = (damageInfo.damageType & pullEnemies) != DamageType.Generic;
            //bool isOsp2 = (damageInfo.damageType & DamageType.AOE) != DamageType.Generic;


            //LogCore.LogI(damageInfo.damageType);
            if (self.body && self.body.HasBuff(BuffCore.instance.bombardierForceIndex))
            {
                damageInfo.force *= 2;
            }
            if (isOsp )
            { 
                if (self.body) {
                    self.body.AddTimedBuff(BuffCore.instance.bombardierForceIndex, 3);
                }
            }
            var rigid = self.body.rigidbody;
            if (self.body) {
                var motor = self.body.characterMotor;
                if (antiGravType && self.body) {
                    //implement alternate behavior if it's a flier
                    if (self.body.characterMotor)
                    {
                        self.body.AddTimedBuff(BuffCore.instance.antiGravIndex, 5);

                    }
                    else
                    {
                        if (rigid)
                        {
                            rigid.AddForce(new UnityEngine.Vector3(0, -10000, 0));
                        }
                    }
                }
                if (damageInfo.attacker && pullType)
                {
                    //var distance = Vector3.Distance(damageInfo.attacker.transform.position, self.transform.position);
                    Vector3 position2 = self.transform.position;
                    Vector3 normalized = (damageInfo.attacker.transform.position - position2).normalized;

                    if (motor)
                    {
                        motor.ApplyForce(normalized * 700, true);
                    }
                    else if (rigid)
                    {
                        rigid.AddForce(normalized * 700, ForceMode.Impulse);
                    }
                }
            }
            if (self.body.HasBuff(BuffCore.instance.antiGravIndex) && falldmgType)
            {
                damageInfo.damage *= 2;
                //real good feeling.
            }
            if (spikedType && rigid) {
                //bing bing WA-FUCKING-HOO 
                rigid.AddForce(new UnityEngine.Vector3(0, -10000, 0));
            }

            orig(self, damageInfo);
        }

        protected void AddDamageTypes() {
            bombardierKnockback = AddDamageType();
            antiGrav = AddDamageType();
            spiked = AddDamageType();
            pullEnemies = AddDamageType();
        }

        protected DamageType AddDamageType() {
            currentMax *= 2;
            var damageType = (RoR2.DamageType)currentMax;

            LogCore.LogI("Added DamageType:" + damageType.ToString());
            return damageType;
        }
    }
}
