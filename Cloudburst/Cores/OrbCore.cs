using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Cloudburst.Cores
{
    class OrbCore {
        public static OrbCore instance;

        public OrbCore() => RegisterOrbs();

        protected internal void RegisterOrbs() {
            instance = this;

            LogCore.LogI("Initializing Core: " + base.ToString());
            RegisterNewOrb(typeof(DroneAttackOrb));
        }

        protected internal void RegisterNewOrb(Type t) {
            OrbAPI.AddOrb(t);
            LogCore.LogI(string.Format("Added {0} to the OrbCatalog", t.Name));
        }
    }

    public class DroneAttackOrb : GenericDamageOrb
    {
        public override void Begin()
        {
            speed = 120f;
            base.Begin();
        }
        public override void OnArrival()
        {
            base.OnArrival();
            if (attacker && target)
            {
                DotController.InflictDot(target.healthComponent.gameObject, attacker, DoTCore.instance.clean, 8);
                EffectManager.SimpleEffect(Resources.Load<GameObject>("prefabs/effects/HANDHeal"), attacker.transform.position, attacker.transform.rotation, true);
            }
        }

        protected override GameObject GetOrbEffect()
        {
            return Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/LoaderLightningOrbEffect");
        }
    }
    public class DroneRetrivalOrb : Orb
    {
        private SkillLocator locator;
        public override void Begin()
        {
            duration = distanceToTarget / 30f;

            var hurtBox = target.GetComponent<HurtBox>();
            var characterBody = (hurtBox != null) ? hurtBox.healthComponent.GetComponent<CharacterBody>() : null;

            if (characterBody) {
                locator = characterBody.skillLocator;
            }

            var effect = GetOrbEffect();
            if (effect)
            {
                EffectData effectData = new EffectData
                {
                    scale = 2,
                    origin = origin,
                    genericFloat = duration
                };
                effectData.SetHurtBoxReference(target);
                EffectManager.SpawnEffect(effect, effectData, true);
            }

            base.Begin();
        }
        public override void OnArrival()
        {
            base.OnArrival();
            //TODO:
            //This code probably doesn't work in MP
            if (target && locator) {
                if (locator.special.stock < 10) {
                    locator.special.AddOneStock();
                }
            }
            
            //if (attacker && target)
            //{
            //    DotController.InflictDot(target.healthComponent.gameObject, attacker, DoTCore.instance.clean, 8);
            //    EffectManager.SimpleEffect(Resources.Load<GameObject>("prefabs/effects/HANDHeal"), attacker.transform.position, attacker.transform.rotation, true);
            //}
        }

        protected virtual GameObject GetOrbEffect()
        {
            return EffectCore.HANDRetrivalOrb;
        }
    }
}

