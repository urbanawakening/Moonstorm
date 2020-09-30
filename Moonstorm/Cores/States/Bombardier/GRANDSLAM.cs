using EntityStates;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.States.Bombardier
{
    class GrandSlam : BaseSkillState
    {

        public static float baseDuration = 2f;
        private float duration;
        private bool hasLanded;
        private float y;
        private bool isClient;
        public override void OnEnter()
        {
            base.OnEnter();
            y = 0;
            this.duration = baseDuration / this.attackSpeedStat;
            base.StartAimMode(2f, false);
            if (base.isAuthority)
            {
                Slam();
                if (characterMotor && NetworkServer.active)
                {
                    characterMotor.onHitGround += CharacterMotor_onHitGround;
                }
                else {
                    isClient = true;
                }
            }
        }

        private void CharacterMotor_onHitGround(ref RoR2.CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            if (!hasLanded)
            {
                hasLanded = true;

                var boost = Mathf.Abs(y) / 210;
                LogCore.LogI(boost);

                BlastAttack attack = new BlastAttack()
                {
                    attacker = base.gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = (6.3f + boost) * base.damageStat,
                    baseForce = -3000,
                    bonusForce = new Vector3(0, 0, 0),
                    crit = base.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.Linear,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = base.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 0.5f,
                    radius = 15 + Mathf.Abs(y) / 2,
                    teamIndex = characterBody.teamComponent.teamIndex,
                };
                attack.Fire();
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX"), new EffectData
                {
                    origin = transform.position,
                    scale = 15 + boost,
                }, true);
                /*LogCore.LogI(y);
                LogCore.LogI(Mathf.Abs(y));
                LogCore.LogI(Mathf.Abs(y) /2);*/
            }
            else if (!NetworkServer.active) {
                hasLanded = true;
            }
        }
        
        public virtual void CLientSlam()
        {
            hasLanded = true;

            var boost = Mathf.Abs(y) / 210;
            LogCore.LogI(boost);

            BlastAttack attack = new BlastAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = (6.3f + boost) * base.damageStat,
                baseForce = -3000,
                bonusForce = new Vector3(0, 0, 0),
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BlastAttack.FalloffModel.Linear,
                impactEffect = EffectIndex.Invalid,
                inflictor = base.gameObject,
                losType = BlastAttack.LoSType.NearestHit,
                position = base.transform.position,
                procChainMask = default,
                procCoefficient = 0.5f,
                radius = 15 + Mathf.Abs(y) / 2,
                teamIndex = characterBody.teamComponent.teamIndex,
            };
            attack.Fire();
            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX"), new EffectData
            {
                origin = transform.position,
                scale = 15 + boost,
            }, true);
            /*LogCore.LogI(y);
            LogCore.LogI(Mathf.Abs(y));
            LogCore.LogI(Mathf.Abs(y) /2);*/
        }

        public virtual void Slam()
        {
            if (characterMotor) {
                //characterMotor.ApplyForce(new Vector3(characterMotor.moveDirection.x, characterMotor.mass * 15, characterMotor.moveDirection.z), true, false);
                characterMotor.ApplyForce(new Vector3(characterMotor.moveDirection.x, -(characterMotor.mass * 70), characterMotor.moveDirection.z), true, false);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            characterBody.AddSpreadBloom(10);

            if (!hasLanded) {
                y += characterMotor.velocity.y;
                //LogCore.LogI(y);
            }

            if (isClient && !hasLanded && isGrounded) {
                CLientSlam();
            }

            if (base.fixedAge >= this.duration && base.isAuthority && this.hasLanded)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (NetworkServer.active) {
                characterMotor.onHitGround -= CharacterMotor_onHitGround;
            }

        }
    }
}