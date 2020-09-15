using EntityStates;
using EntityStates.Pounder;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores.States.REX
{
    public class Harvest : BaseState
    {
        private ProjectileDamage damage;
        private float collectTimer;
        #region Statics
        public static float duration = 15;
        public static float collectFreq = 1;
        public static GameObject deathEffect = Resources.Load<GameObject>("prefabs/effects/impacteffects/RoboCratePodGroundImpact");
        #endregion

        public override void OnEnter() { 
            base.OnEnter();
            damage = GetComponent<ProjectileDamage>();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            collectTimer -= Time.deltaTime;
            if (collectTimer <= 0 && projectileController.owner) {
                collectTimer += collectFreq;
                base.PlayAnimation("Base", "Pound");

                new BlastAttack
                {
                    attacker = projectileController.owner,
                    attackerFiltering = AttackerFiltering.NeverHit,
                    baseDamage = 2.5f * projectileController.owner.GetComponent<CharacterBody>().damage,
                    baseForce = -3000,
                    crit = damage.crit,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = damage.damageType,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = base.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = transform.position,
                    procChainMask = default,
                    procCoefficient = 1,
                    radius = 20,
                    teamIndex = projectileController.teamFilter.teamIndex,
                }.Fire();
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXTreebot"), new EffectData
                {
                    origin = base.transform.position,
                    scale = 5
                }, true);
            }
			if (NetworkServer.active && base.fixedAge > duration)
			{
                EffectManager.SpawnEffect(deathEffect, new EffectData
                {
                    origin = base.transform.position,
                    scale = 15
                }, true);
                EntityState.Destroy(base.gameObject);
			}
        }
    }
}
