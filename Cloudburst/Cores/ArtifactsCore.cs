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
    class ArtifactsCore
    {
        protected ArtifactDef smallDef;
        public ArtifactsCore() => RegisterArtifacts();


        protected void RegisterArtifacts() {
            LogCore.LogI("Initializing Core: " + base.ToString());
            ArtifactCatalog.getAdditionalEntries += delegate (List<ArtifactDef> artifactDefs) {
                artifactDefs.Add(ArtifactofDiminutive());
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
            LanguageAPI.Add(artifactDef.descriptionToken, "Every character is now smaller.");


            smallDef = artifactDef;
            return artifactDef;
        }

        protected void Hook() {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart; ;

        }

        private void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body)
        {
            if (RunArtifactManager.instance && RunArtifactManager.instance.IsArtifactEnabled(smallDef)) {
                body.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.localScale = Vector3.one * 0.9f;
                self.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.Translate(new Vector3(0f, 1.6f, 0f));
                body.aimOriginTransform.Translate(new Vector3(0f, 0f, 0f));
                body.GetComponentInChildren<CharacterMotor>().mass = 10;
                foreach (KinematicCharacterMotor kinematicCharacterMotor2 in body.GetComponentsInChildren<KinematicCharacterMotor>())
                {
                    kinematicCharacterMotor2.SetCapsuleDimensions(kinematicCharacterMotor2.Capsule.radius * 0.5f, kinematicCharacterMotor2.Capsule.height * 0.5f, 0f);
                }
            }
            orig(self, body);
        }
    }
}
