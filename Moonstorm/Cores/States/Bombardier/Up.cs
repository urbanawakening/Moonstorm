using Moonstorm.Cores.HAND.Components;
using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace Moonstorm.Cores.States.Bombardier
{
    public class UpwardsBlast : BaseSkillState
    {
        private float duration;

        private bool hasSwung;

        public static float baseDuration = 0.5f;

        #region Effects
        public static GameObject explosionPrefab = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");
        #endregion

        public float charge;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / base.attackSpeedStat;

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!hasSwung)
            {
                hasSwung = true;

                BlastAttack attack = new BlastAttack()
                {
                    attacker = base.gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = 5f * base.damageStat,
                    baseForce = -3000,
                    bonusForce = new Vector3(0, 0, 0),
                    crit = base.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageTypeCore.bombardierKnockback,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = base.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 0.5f,
                    radius = 15,
                    teamIndex = characterBody.teamComponent.teamIndex,
                };
                attack.Fire();
                EffectManager.SpawnEffect(explosionPrefab, new EffectData
                {
                    origin = transform.position,
                    scale = 15
                }, true);
                characterMotor.ApplyForce(new Vector3(characterMotor.moveDirection.x, characterMotor.mass * 35, characterMotor.moveDirection.z), true, false);

            }
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}

