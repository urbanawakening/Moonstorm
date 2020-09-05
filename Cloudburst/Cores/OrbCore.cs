using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Cloudburst.Cores
{
    class OrbCore
    {
        public OrbCore() => RegisterOrbs();

        protected internal void RegisterOrbs() {
            RegisterNewOrb(typeof(DroneOrb));
        }

        protected internal void RegisterNewOrb(Type t) {
            OrbAPI.AddOrb(t);
            LogCore.LogI(string.Format("Added {0} to the OrbCatalog", t.Name));
        }
    }

    public class DroneOrb : GenericDamageOrb
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
}

