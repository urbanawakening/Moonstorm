using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace Cloudburst.Cores.States.HAND
{
    public class ChargeBurst : BaseSkillState
    {
        public static float baseDuration;
        private float duration;
        private float stopwatch;
        private float bonus;
        private bool playedAudioCue = false;
        public override void OnEnter()
        {
            duration = baseDuration / attackSpeedStat;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += (fixedAge * 5);
            bool capped = stopwatch <= 15;
            bonus = capped ? stopwatch : 15;
            if (capped && !playedAudioCue)
            {
                playedAudioCue = true;
                Util.PlaySound("Play_MULT_m1_snipe_charge_end", base.gameObject);
            }
            if (base.fixedAge >= this.duration && base.isAuthority && !Charging())
            {
                new BlastAttack()
                {
                    attacker = base.gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = 10 * base.damageStat,
                    baseForce = 0,
                    bonusForce = new Vector3(0, 0, 0),
                    crit = base.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = base.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 0.5f,
                    radius = 10,
                    teamIndex = characterBody.teamComponent.teamIndex,
                }.Fire();
                outer.SetNextState(new Burst
                {
                    chargeBonus = bonus
                });
                return;
            }
        }
        public bool Charging()
        {
            return inputBank.skill3.down;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}