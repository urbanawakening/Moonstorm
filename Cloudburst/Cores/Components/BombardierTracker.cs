using Cloudburst.Cores.HAND.Components;
using UnityEngine;

namespace Cloudburst.Cores.Components
{
    public class BombardierTracker : HANDDroneTracker
    {
        //GameObject indicator = Resources.Load<GameObject>("prefabs/EngiShieldRetractIndicator");
        public override GameObject GetIndicator()
        {

            return Resources.Load<GameObject>("Prefabs/EngiShieldRetractIndicator");

        }
    }
}
