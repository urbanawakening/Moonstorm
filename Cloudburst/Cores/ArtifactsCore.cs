using KinematicCharacterController;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cloudburst.Cores
{
    //TODO:
    //never release
    class ArtifactsCore
    {
        public static ArtifactsCore instance;
        protected ArtifactDef smallDef;
        protected ArtifactDef massDef;
        public ArtifactsCore() => RegisterArtifacts();


        protected void RegisterArtifacts() {
            LogCore.LogI("Initializing Core: " + base.ToString());
            instance = this;

            ArtifactCatalog.getAdditionalEntries += delegate (List<ArtifactDef> artifactDefs) {
                artifactDefs.Add(ArtifactofDiminutive());
                artifactDefs.Add(ArtifactofMass());
            };
            Hook();
        }

        protected ArtifactDef ArtifactofDiminutive()
        {
            var artifactDef = ScriptableObject.CreateInstance<ArtifactDef>();
            var exampleDef = Resources.Load<ArtifactDef>("ArtifactDefs/Enigma");

            artifactDef.descriptionToken = "ARTIFACT_SMALL_DESCRIPTION";
            artifactDef.nameToken = "ARTIFACT_SMALL_NAME";
            artifactDef.pickupModelPrefab = exampleDef.pickupModelPrefab;
            artifactDef.smallIconDeselectedSprite = exampleDef.smallIconDeselectedSprite;
            artifactDef.smallIconSelectedSprite= exampleDef.smallIconSelectedSprite;
            artifactDef.unlockableName = "";

            LanguageAPI.Add(artifactDef.nameToken, "Artifact of Diminutive");
            LanguageAPI.Add(artifactDef.descriptionToken, "Every character is smaller.");


            smallDef = artifactDef;
            return artifactDef;
        }
        protected ArtifactDef ArtifactofMass()
        {
            var artifactDef = ScriptableObject.CreateInstance<ArtifactDef>();
            var exampleDef = Resources.Load<ArtifactDef>("ArtifactDefs/Enigma");

            artifactDef.descriptionToken = "ARTIFACT_FAT_DESCRIPTION";
            artifactDef.nameToken = "ARTIFACT_FAT_NAME";
            artifactDef.pickupModelPrefab = exampleDef.pickupModelPrefab;
            artifactDef.smallIconDeselectedSprite = exampleDef.smallIconDeselectedSprite;
            artifactDef.smallIconSelectedSprite = exampleDef.smallIconSelectedSprite;
            artifactDef.unlockableName = "";

            LanguageAPI.Add(artifactDef.nameToken, "Artifact of Mass");
            LanguageAPI.Add(artifactDef.descriptionToken, "Every character is larger.");


            massDef = artifactDef;
            return artifactDef;
        }
        protected void Hook() {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart; ;

        }

        private void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body)
        {
            if (RunArtifactManager.instance) {
                if (RunArtifactManager.instance.IsArtifactEnabled(massDef)) {
                    var modelLocator = body.GetComponent<ModelLocator>();
                    var modelBaseTransform = modelLocator.modelBaseTransform;

                    modelBaseTransform.localScale = Vector3.one * 1.5f;
                    modelBaseTransform.Translate(new Vector3(0f, 1f, 0f));
                    body.aimOriginTransform.Translate(new Vector3(0f, 1.5f, 0f));
                    foreach (KinematicCharacterMotor kinematicCharacterMotor in body.GetComponentsInChildren<KinematicCharacterMotor>())
                    {
                        kinematicCharacterMotor.SetCapsuleDimensions(kinematicCharacterMotor.Capsule.radius * 0.8f, kinematicCharacterMotor.Capsule.height * 0.8f, 0f);
                    }
                }
                if (RunArtifactManager.instance.IsArtifactEnabled(smallDef)) {
                    var modelLocator = body.GetComponent<ModelLocator>();
                    var modelBaseTransform = modelLocator.modelBaseTransform;

                    //var resizeType = Vector3.one * 0.5f;
                    modelBaseTransform.localScale *= 0.25f;
                    modelBaseTransform.transform.Translate(new Vector3(0f, 3f, 0f));
                    body.aimOriginTransform.Translate(new Vector3(0f, -3, 0f));
                    foreach (KinematicCharacterMotor kinematicCharacterMotor in body.GetComponentsInChildren<KinematicCharacterMotor>())
                    {
                        kinematicCharacterMotor.SetCapsuleDimensions(kinematicCharacterMotor.Capsule.radius * -6f, kinematicCharacterMotor.Capsule.height * -6, 0);
                    }
                }
            }
            orig(self, body);
        }
    }
}
