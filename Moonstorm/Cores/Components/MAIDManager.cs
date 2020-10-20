using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.Components
{
    public class MAIDManager : NetworkBehaviour
    {
        private GameObject maid;
        public void Awake()
        {
            //click and she's gone!
        }

        #region Deployment
        public void DeployMAIDAuthority(GameObject maid)
        {
            if (NetworkServer.active)
            {
                DeployMAIDInternal(maid);
                return;
            }
            CmdDeployMAIDInternal(maid);
        }

        [Server]
        private void DeployMAIDInternal(GameObject maid)
        {
            this.maid = maid;
        }

        [Command]
        private void CmdDeployMAIDInternal(GameObject maid)
        {
            DeployMAIDInternal(maid);
        }
        #endregion

        #region Retrival
        [Server]
        private void RetrieveMAIDInternal()
        {
            Destroy(maid);
        }

        [Command]
        private void CmdRetrieveMAIDInternal()
        {
            RetrieveMAIDInternal();
        }

        public void RetrieveMAIDAuthority()
        {
            if (NetworkServer.active)
            {
                RetrieveMAIDInternal();
                return;
            }
            CmdRetrieveMAIDInternal();
        }
        #endregion  
    }
}
