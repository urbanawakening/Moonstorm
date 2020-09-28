using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.Components
{
    class SlamController : NetworkBehaviour
    {
        public bool hasLanded;

        public Action<CharacterMotor.HitGroundInfo> onHitGround;
        public void Awake()
        {
            //click and she's here.
            //check the door, moron.
        }

        public void SubscribeToOnHitGroundAuthority() {
            if (NetworkServer.active)
            {
                SubscribeToOnHitGroundInternal();
                return;
            }
            CmdSubscribeToOnHitGroundInternal();

        }

        public void UnsubcribeToOnHitGroundAuthority() {
            if (NetworkServer.active)
            {
                UnsubscribeToOnHitGroundInternal();
                return;
            }
            CmdUnsubscribeToOnHitGroundInternal();
        }


        [Command]
        public void CmdUnsubscribeToOnHitGroundInternal()
        {
            UnsubscribeToOnHitGroundInternal();
        }

        [Server]
        public void UnsubscribeToOnHitGroundInternal()
        {
            CharacterMotor motor = base.gameObject.GetComponent<CharacterMotor>();
            motor.onHitGround -= Motor_onHitGround;
        }


        [Command]
        public void CmdSubscribeToOnHitGroundInternal()
        {
            SubscribeToOnHitGroundInternal();
        }



        [Server]
        public void SubscribeToOnHitGroundInternal()
        {
            CharacterMotor motor = base.gameObject.GetComponent<CharacterMotor>();
            motor.onHitGround += Motor_onHitGround;
        }

        private void Motor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo) {
            
        }
    }
}
