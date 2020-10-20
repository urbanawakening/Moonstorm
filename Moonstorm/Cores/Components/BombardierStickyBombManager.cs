using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.Components
{
    public class BombardierStickyBombManager : NetworkBehaviour {
        private List<GameObject> bombs;
        public void Awake()
        {
            //click and she's gone!
            
            bombs = new List<GameObject>();
        }

        #region Authorities
        public void AddBombAuthority(GameObject sticky) {
            if (NetworkServer.active)
            {
                AddStickyInternal(sticky);
                return;
            }
            CmdAddSticky(sticky);
        }

        public void PopStickiesAuthority() {
            if (NetworkServer.active)
            {
                PopStickiesInternal();
                return;
            }
            CmdPopStickiesInternal() ;
        }
        #endregion

        [Server]
        public void AddStickyProjectile(GameObject sticky) {
            bombs.Add(sticky);
        }

        [Server]
        private void AddStickyInternal(GameObject sticky) {
            bombs.Add(sticky);
        }

        [Command]
        private void CmdAddSticky(GameObject sticky)
        {
            AddStickyInternal(sticky);
        }

        [Command]
        private void CmdPopStickiesInternal()
        {
            PopStickiesInternal();
        }

        private void PopStickiesInternal() {
            foreach (var item in bombs)
            {   
                if (item)
                {
                    var sticky = item.GetComponent<BombardierStickyBombProjectile>();
                    if (sticky) {
                        sticky.Pop();
                    }
                }

            };
            bombs.Clear();
        }

    }
}
