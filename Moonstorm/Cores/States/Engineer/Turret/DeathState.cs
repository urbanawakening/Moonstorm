using System;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.Engineer.ETStates {
    public class DeathState : GenericCharacterDeath
    {
        public GameObject initialExplosion = Resources.Load<GameObject>("prefabs/effects/ExplosionEngiTurret");
        public GameObject deathExplosion = Resources.Load<GameObject>("prefabs/effects/VolatileBatteryExplosion");
        private float deathDuration;
        public override void PlayDeathAnimation(float crossfadeDuration = 0.1f)
        {
            Animator modelAnimator = base.GetModelAnimator();
            if (modelAnimator)
            {
                int layerIndex = modelAnimator.GetLayerIndex("Body");
                modelAnimator.PlayInFixedTime("Death", layerIndex);
                modelAnimator.Update(0f);
                this.deathDuration = modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).length;
                if (this.initialExplosion)
                {
                    UnityEngine.Object.Instantiate<GameObject>(this.initialExplosion, base.transform.position, base.transform.rotation, base.transform);
                }
            }
        }
        public override bool shouldAutoDestroy
        {
            get
            {
                return false;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge > this.deathDuration && NetworkServer.active && this.deathExplosion)
            {
                float radius = characterBody.maxHealth * .5f;
                EffectManager.SpawnEffect(this.deathExplosion, new EffectData
                {
                    origin = base.transform.position,
                    scale = radius
                }, true);
                new BlastAttack
                {
                    attacker = gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = characterBody.maxHealth * 3,
                    baseForce = 0,
                    crit = RollCrit(),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    damageType = DamageType.IgniteOnHit,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = transform.position,
                    procChainMask = default,
                    procCoefficient = 1,
                    radius = radius,
                    teamIndex = characterBody.teamComponent.teamIndex
                }.Fire();
                EntityState.Destroy(base.gameObject);
            }
        }

        public override void OnExit()
        {
            base.DestroyModel();
            base.OnExit();
        }
    }
}