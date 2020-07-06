using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Misc
{
    internal sealed class Modifications
    {
        public static void MakeDeskPlantBetter()
        {
            //ugh 
            GameObject deskPlant = Resources.Load<GameObject>("Prefabs/NetworkedObjects/InterstellarDeskPlant");
            DeskPlantController deskplantController = deskPlant.GetComponent<DeskPlantController>();

            deskplantController.healingRadius = 6;
            deskplantController.radiusIncreasePerStack = 3;
        }
        public static void RenameDaisy()
        {
            //It's a lily, not a daisy.
            LanguageAPI.Add("ITEM_TPHEALINGNOVA_NAME", "Lepton Lily");
        }

        public static void ModifyPlasmaBolt()
        {
            GameObject LoaderPylon = Resources.Load<GameObject>("prefabs/projectiles/loaderpylon");
            GameObject PlasmaBolt = Resources.Load<GameObject>("prefabs/projectiles/magelightningboltbasic");


            ProjectileController PlasmaBolt_PC = PlasmaBolt.GetComponent<ProjectileController>();
            ProjectileProximityBeamController PlasmaBolt_PPBC = PlasmaBolt.AddComponent<ProjectileProximityBeamController>();
            ProjectileProximityBeamController LoaderPylon_PPBC = LoaderPylon.GetComponent<ProjectileProximityBeamController>();

            PlasmaBolt_PC.ghostPrefab = Resources.Load<GameObject>("prefabs/projectileghosts/electricwormseekerghost");

            PlasmaBolt_PPBC.attackFireCount = LoaderPylon_PPBC.attackFireCount;
            PlasmaBolt_PPBC.attackRange = 10;
            PlasmaBolt_PPBC.bounces = 0;
            PlasmaBolt_PPBC.damageCoefficient = 0.5f;
            PlasmaBolt_PPBC.lightningType = RoR2.Orbs.LightningOrb.LightningType.Ukulele;
            PlasmaBolt_PPBC.listClearInterval = LoaderPylon_PPBC.listClearInterval;
            PlasmaBolt_PPBC.maxAngleFilter = LoaderPylon_PPBC.maxAngleFilter;
            PlasmaBolt_PPBC.minAngleFilter = LoaderPylon_PPBC.minAngleFilter;
            PlasmaBolt_PPBC.procCoefficient = 0.2f;
        }

        public static void MakeIceWallSolid()
        {
            GameObject ArtificerIceWallPrefab = Resources.Load<GameObject>("prefabs/projectiles/mageicewallpillarprojectile");
            ArtificerIceWallPrefab.layer = 11;
        }

    }
}
