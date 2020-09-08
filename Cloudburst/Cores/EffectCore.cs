using UnityEngine;
using R2API;

namespace Cloudburst.Cores
{
    class EffectCore
    {
        public static GameObject HANDRetrivalOrb;
        public EffectCore() => Effects();

        protected void Effects() {
            CreateNewEffects();
            Repair();
        }
        
        protected void Repair() {
            RepairAncientWispPillar();
            RepairHANDSwingEffect();
            RepairAncientWispSwingEffect();
        }

        protected void CreateNewEffects() {
            //all that glitters dies here

            HANDRetrivalOrb = Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/InfusionOrbEffect").InstantiateClone("HANDDroneOrbEffect", false);
            var core = HANDRetrivalOrb.transform.Find("VFX/Core");
            var coreRenderer = core.GetComponent<Renderer>();

            coreRenderer.material = Resources.Load<Material>("materials/matGrandparentMeleeSwipe");


            //var trail = orbEffect.transform.Find("TrailParent/Trail");
            //var trailRenderer = trail.GetComponent<Renderer>();
            //trailRenderer.material = Resources.Load<Material>("materials/matGrandparentMeleeSwipe");
            //EffectAPI.AddEffect(orbEffect);
        }

        #region HAN-D
        private void RepairHANDSwingEffect()
        {
            GameObject HANDSwingTrail = Resources.Load<GameObject>("prefabs/effects/handslamtrail");
            GameObject healEffect = Resources.Load<GameObject>("prefabs/effects/HANDheal");
            GameObject lemurianSwingTrail = Resources.Load<GameObject>("prefabs/effects/LemurianBiteTrail");

            Transform lemSwingTrailTransform = lemurianSwingTrail.transform.Find("SwingTrail");
            Transform HANDHealRingEffectTransform = healEffect.transform.Find("SwingTrail");
            Transform HANDSwingTrailTransform = HANDSwingTrail.transform.Find("SlamTrail");

            var lemurianMat = lemSwingTrailTransform.GetComponent<Renderer>().material;

            var ringRenderer = HANDHealRingEffectTransform.GetComponent<Renderer>();
            var HANDrenderer = HANDSwingTrailTransform.GetComponent<Renderer>();

            if (HANDrenderer) { 
                HANDrenderer.material = lemurianMat;
            }
            if (ringRenderer) {
                ringRenderer.material = lemurianMat;
            }
        }
        #endregion
        #region AncientWisp

        protected void RepairAncientWispSwingEffect()
        {
            var swingEffect = Resources.Load<GameObject>("prefabs/effects/AncientWispSwingTrail");
            var swing = swingEffect.transform.Find("SwingTrail");

            var renderer = swing.GetComponent<Renderer>();

            renderer.material = Resources.Load<GameObject>("prefabs/projectileghosts/AncientWispCannonGhost").transform.Find("Particles/FireSphere").GetComponent<Renderer>().material;
            //Resources.Load<Material>("materials/matGrandparentMeleeSwipe");
        }

        private void RepairAncientWispPillar() {
            var pillarEffect = Resources.Load<GameObject>("prefabs/effects/impacteffects/AncientWispPillar");

            var sparks = pillarEffect.transform.Find("Particles/Sparks");
            var centerHuge = pillarEffect.transform.Find("Particles/Flames, Tube (1)");
            var flashWhite = pillarEffect.transform.Find("Particles/Flash,White");
            var flash = pillarEffect.transform.Find("Particles/Flash");

            var mat = pillarEffect.transform.Find("Particles/Flames, Tube, CenterHuge").GetComponent<Renderer>().material;

            sparks.GetComponent<Renderer>().material = mat;
            centerHuge.GetComponent<Renderer>().material = mat;
            flashWhite.GetComponent<Renderer>().material = mat;
            flash.GetComponent<Renderer>().material = mat;
        }
        #endregion
    }
}
