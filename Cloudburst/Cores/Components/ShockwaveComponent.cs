using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Cores.HAND.Components
{
    class ShockwaveProjectileComponent : ExtendedProjectileOverlapAttack
    {
        public override void OnHit(OverlapAttack attack, Transform pos)
        {
            base.OnHit(attack, pos);
            var complete = 0;
            for (float num = 0f; num < 9f; num += 1f)
            {
                float num2 = 6.2831855f;
                Vector3 forward = new Vector3(Mathf.Cos(num / 9f * num2), 0f, Mathf.Sin(num / 9f * num2));
                FireProjectileInfo fireProjectileInfo2 = new FireProjectileInfo
                {
                    crit = false,
                    damage = 3 * projectileController.owner.GetComponent<CharacterBody>().damage,
                    damageColorIndex = DamageColorIndex.Default,
                    damageTypeOverride = DamageType.Generic,
                    position = pos.position,
                    force = 2500,
                    owner = projectileController.owner,
                    procChainMask = default,
                    projectilePrefab = Resources.Load<GameObject>("prefabs/projectiles/sunder"),
                    rotation = Quaternion.LookRotation(forward),
                    target = null,
                    useFuseOverride = false,
                    useSpeedOverride = false
                };
                ProjectileManager.instance.FireProjectile(fireProjectileInfo2);
                complete++;
                //LogCore.LogI(complete);
            }
            if (complete == 9) {
                Destroy(gameObject);
            }
        }
    }
}
