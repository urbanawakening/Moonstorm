using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.States.Bombardier
{
    class GrandSlam : BaseSkillState
    {

        public static float baseDuration = 2f;
        private float duration;
        private bool hasLanded;
        private float y;
        private float bY;
        public override void OnEnter()
        {
            base.OnEnter();
            y = 0;
            bY = base.characterDirection.moveVector.y;

            this.duration = baseDuration / this.attackSpeedStat;
            base.StartAimMode(2f, false);
            if (base.isAuthority)
            {
                Slam();
                if (characterMotor) {
                    characterMotor.onHitGround += CharacterMotor_onHitGround;
                }
            }
        }

        private void CharacterMotor_onHitGround(ref RoR2.CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            hasLanded = true;


            //kek!
            BlastAttack attack = new BlastAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = (6.3f + (Mathf.Abs(y) / 2)) * base.damageStat,
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
                radius = 15 + Mathf.Abs(y) / 2,
                teamIndex = characterBody.teamComponent.teamIndex,
            };
            attack.Fire();
            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX"), new EffectData
            {
                origin = transform.position,
                scale = 15 + Mathf.Abs(y) / 2,
            }, true);

            /*LogCore.LogI(y);
            LogCore.LogI(Mathf.Abs(y));
            LogCore.LogI(Mathf.Abs(y) /2);*/
        }

        public virtual void Slam()
        {
            if (characterMotor) {
                characterMotor.ApplyForce(new Vector3(characterMotor.moveDirection.x, characterMotor.mass * 15, characterMotor.moveDirection.z), true, false);


                characterMotor.ApplyForce(new Vector3(characterMotor.moveDirection.x, -(characterMotor.mass * 70), characterMotor.moveDirection.z), true, false);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!hasLanded) {
                y += base.characterDirection.moveVector.y;
                //LogCore.LogI(y);
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
            characterMotor.onHitGround -= CharacterMotor_onHitGround;
        }
    }
}