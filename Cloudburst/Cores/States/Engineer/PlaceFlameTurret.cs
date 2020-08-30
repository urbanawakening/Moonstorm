using EntityStates.Engi.EngiWeapon;
using System;
using UnityEngine;

namespace Cloudburst.Cores.Engineer
{
    public class PlaceFlameTurret : PlaceTurret
    {
        public PlaceFlameTurret()
        {
            blueprintPrefab = Resources.Load<GameObject>("prefabs/EngiTurretBlueprints");
            wristDisplayPrefab = Resources.Load<GameObject>("prefabs/EngiTurretWristDisplay");
            turretMasterPrefab = EngineerCore.flameTurretMaster;
        }
    }
}
