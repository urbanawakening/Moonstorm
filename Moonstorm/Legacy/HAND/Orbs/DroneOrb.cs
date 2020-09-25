using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.HAND.Orbs
{
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
            if (attacker && target) {
                HealthComponent healthComponent = attacker.GetComponent<HealthComponent>();
                if (healthComponent) {
                };
                DotController.InflictDot(target.healthComponent.gameObject, attacker, HAN_D.cleanDoT, 10);
            }
        }

        protected override GameObject GetOrbEffect()
        {
            return Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/LoaderLightningOrbEffect");
        }
    }
}