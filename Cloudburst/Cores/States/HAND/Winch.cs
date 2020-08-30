using Cloudburst.Cores.HAND;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cloudburst.Cores.States.HAND
{
    public class Winch : BaseSkillState
    {
        private float duration = 1;
        public override void OnEnter()
        {
            base.OnEnter();
            Transform modelTransform = base.GetModelTransform();
            ChildLocator locator = modelTransform.GetComponent<ChildLocator>();
            Transform pos = locator.FindChild("WinchHole");

            List<GameObject> list = new List<GameObject>();
            BullseyeSearch bullseyeSearch = new BullseyeSearch();
            bullseyeSearch.searchOrigin = base.gameObject.transform.position;
            bullseyeSearch.maxDistanceFilter = 25;
            bullseyeSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.teamComponent.teamIndex);
            bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
            bullseyeSearch.filterByLoS = true;
            bullseyeSearch.searchDirection = Vector3.up;
            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(base.gameObject);
            List<HurtBox> list2 = bullseyeSearch.GetResults().ToList<HurtBox>();
            for (int i = 0; i < list2.Count; i++)
            {
                GameObject gameObject = list2[i].healthComponent.gameObject;
                if (gameObject)
                {
                    list.Add(gameObject);
                }
            }
            AttachToTargets(list, pos);
        }
        public void AttachToTargets(List<GameObject> gameObjects, Transform pos)
        {


            for (int i = 0; i < gameObjects.Count; i++)
            {
                FireProjectileInfo info = new FireProjectileInfo
                {
                    projectilePrefab = HAN_DCore.instance.winch,
                    position = pos.position,
                    rotation = pos.rotation,
                    owner = base.characterBody.gameObject,
                    damage = base.characterBody.damage * 0.3f,
                    force = -2000,
                    crit = base.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    target = gameObjects[i],
                };
                ProjectileManager.instance.FireProjectile(info);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
