using System;
using EntityStates;
using EntityStates.Croco;
using RoR2;
using UnityEngine;

namespace Cloudburst.Cores.ClayMan.Skills
{
    public class Leap : BaseState
    {
        public static float verticalJumpSpeed = 15;
        private bool endState;
        private Animator animator;
        private bool playedImpact;
        public override void OnEnter()
        {
            base.OnEnter();
            endState = false;
            this.animator = base.GetModelAnimator();
            Util.PlaySound("Play_clayboss_m2_rise", base.gameObject);
            Vector3 direction = base.GetAimRay().direction;
            if (base.isAuthority)
            {
                base.characterBody.isSprinting = true;
                direction.y = Mathf.Max(direction.y, BaseLeap.minimumY);
                Vector3 a = direction.normalized * BaseLeap.aimVelocity * this.moveSpeedStat;
                Vector3 b = Vector3.up * BaseLeap.upwardVelocity;
                Vector3 b2 = new Vector3(direction.x, 0f, direction.z).normalized * BaseLeap.forwardVelocity;
                base.characterMotor.Motor.ForceUnground();
                base.characterMotor.velocity = a + b + b2;
            }
            base.characterMotor.onHitGround += CharacterMotor_onHitGround;
            base.PlayCrossfade("Body", "LeapAirLoop", 0.15f);
        }

        private void CharacterMotor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            BlastAttack impactAttack = new BlastAttack
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = 3 * base.damageStat,
                baseForce = 10,
                bonusForce = new Vector3(0, 0, 0),
                crit = false,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.AOE,
                falloffModel = BlastAttack.FalloffModel.None,
                inflictor = base.gameObject,
                losType = BlastAttack.LoSType.NearestHit,
                position = hitGroundInfo.position,
                procChainMask = default,
                procCoefficient = 1.2f,
                radius = 15,
                teamIndex = TeamIndex.Monster
            };
            impactAttack.Fire();
            EffectData effect = new EffectData()
            {
                origin = hitGroundInfo.position,
                scale = 15
            };
            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact"), effect, true);
            this.endState = true;
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.animator.SetFloat("Leap.cycle", Mathf.Clamp01(Util.Remap(base.characterMotor.velocity.y, -Leap.verticalJumpSpeed, Leap.verticalJumpSpeed, 1f, 0f)));
            if (base.characterMotor.isGrounded && !this.playedImpact && this.endState == true)
            {
                this.playedImpact = true;
                int layerIndex = this.animator.GetLayerIndex("Impact");
                if (layerIndex >= 0)
                {
                    this.animator.SetLayerWeight(layerIndex, 1.5f);
                    this.animator.PlayInFixedTime("LightImpact", layerIndex, 0f);
                }
                if (base.isAuthority && this.endState == true)
                {

                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }

        public override void OnExit()
        {
            base.PlayAnimation("Body", "Idle");
            base.characterMotor.onHitGround -= CharacterMotor_onHitGround;
            base.OnExit();

        }
    }
}