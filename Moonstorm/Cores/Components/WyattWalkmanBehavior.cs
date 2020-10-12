using System;
using RoR2;
using RoR2.Stats;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.Components.Wyatt
{
    public class WyattWalkmanBehavior : NetworkBehaviour {
        private CharacterBody characterBody;

        private void Awake()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
        }

        private void Start()
        {
            GiveHeadphonesAuthority();
        }

        public void GiveHeadphonesAuthority()
        {
            if (NetworkServer.active)
            {
                GiveHeadphonesInternal();
                return;
            }
            CmdGiveHeadphonesInternal();
        }

        [Command]
        private void CmdGiveHeadphonesInternal()
        {
            GiveHeadphonesInternal();
        }

        [Server]
        private void GiveHeadphonesInternal()
        {
            if (characterBody && characterBody.inventory)
            {
                characterBody.inventory.GiveItem(ItemCore.instance.wyattWalkmanIndex, 1);
            }
        }

        public void TriggerBehaviorAuthority()
        {
            if (NetworkServer.active)
            {
                TriggerBehaviorInternal();
                return;
            }
            CmdTriggerBehaviorInternal();
        }

        [Command]
        private void CmdTriggerBehaviorInternal()
        {
            TriggerBehaviorInternal();
        }

        [Server]
        private void TriggerBehaviorInternal()
        {
            if (characterBody && characterBody.GetBuffCount(BuffCore.instance.wyattCombatIndex) < 10)
            {
                characterBody.AddBuff(BuffCore.instance.wyattCombatIndex);
            }
        }


        public void UntriggerAuthority()
        {
            if (NetworkServer.active)
            {
                UntriggerInternal();
                return;
            }
            CmdUntriggerInternal();
        }

        [Command]
        private void CmdUntriggerInternal()
        {
            UntriggerInternal();
        }

        [Server]
        private void UntriggerInternal()
        {
            for (int i = 0; i < characterBody.GetBuffCount(BuffCore.instance.wyattCombatIndex); i++)
            {
                characterBody.RemoveBuff(BuffCore.instance.wyattCombatIndex);
            }
        }
    }
}