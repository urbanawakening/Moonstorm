using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores.Components {
    class EnableGoldAffixEffect : MonoBehaviour
    {
        private GameObject goldAffixEffect;
        private SkinnedMeshRenderer mainSkinnedMeshRenderer;
        private CharacterModel characterModel;

        public bool isGoldEffectEnabled { get; private set; }

        public void Awake() {
            GameObject model = GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            characterModel = model.GetComponent<CharacterModel>();
        }   
        public void EnableGoldAffix() {
            mainSkinnedMeshRenderer = Reflection.GetFieldValue<SkinnedMeshRenderer>(characterModel, "mainSkinnedMeshRenderer");
            if (!this.goldAffixEffect)
            {
                goldAffixEffect = Instantiate<GameObject>(CommonAssets.goldAffix, base.transform);
                ParticleSystem.ShapeModule shape = goldAffixEffect.GetComponent<ParticleSystem>().shape;



                if (mainSkinnedMeshRenderer)
                {
                    isGoldEffectEnabled = true;
                    shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
                    shape.skinnedMeshRenderer = mainSkinnedMeshRenderer;
                    return;
                }
            }
            else {
                Destroy(this.goldAffixEffect);
                this.goldAffixEffect = null;
            }
        }
        public void OnDisable() {
            if (this.goldAffixEffect) {
                Destroy(this.goldAffixEffect);
                this.goldAffixEffect = null;
            }
        }
    }
}
