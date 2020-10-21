using EntityStates;
using Moonstorm.Cores.HAND.Components;
using RoR2;
using UnityEngine;

namespace Moonstorm.Cores.States.Wyatt
{
    class TrashOut : BaseSkillState
    {
        enum ActionStage {
            StartUp,
            NoTarget,
            FoundTarget,
            HitTarget
        }
        
        private ActionStage stage;
        private HANDDroneTracker tracker;
        private HurtBox target;

        private float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            tracker = base.gameObject.GetComponent<HANDDroneTracker>();

            if (base.isAuthority) {
                stage = ActionStage.StartUp;
                target = tracker.GetTrackingTarget();

                if (characterBody)
                {
                    characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                }

                if (!isGrounded) {
                    //we good
                }
                else
                {
                    //We have to unground ourselves so we can actually hit the target.
                    base.SmallHop(base.characterMotor, 10f);
                }

                if (target && target.healthComponent && target.healthComponent.alive) {
                    stage = ActionStage.FoundTarget;
                }

                LogCore.LogI("Stage: " + stage.ToString());
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //LogCore.LogI("Stage: " + stage.ToString());
            stopwatch += Time.deltaTime;

            if (stage == ActionStage.FoundTarget)
            {
                if (this.target)
                {
                    Vector3 velocity = (target.transform.position - base.transform.position).normalized * 120f;
                    base.characterMotor.velocity = velocity;
                    base.characterDirection.forward = base.characterMotor.velocity.normalized;
                    float distance = Util.SphereVolumeToRadius(target.volume);

                    if (stopwatch > 5)
                    {
                        LogCore.LogI(stopwatch);
                        this.activatorSkillSlot.AddOneStock();
                        LogCore.LogI("Can't reach target, skill refunded!");
                        this.outer.SetNextStateToMain();
                    }


                    if (Vector3.Distance(base.transform.position, target.transform.position) < distance + 5f && target)
                    {
                        new BlastAttack
                        {
                            position = target.transform.position,
                            baseForce = 3000,
                            attacker = base.gameObject,
                            inflictor = base.gameObject,
                            teamIndex = base.GetTeam(),
                            baseDamage = 5 * this.damageStat,
                            attackerFiltering = default,
                            bonusForce = new Vector3(0, -6000, 0),
                            damageType = DamageType.Stun1s | DamageTypeCore.spiked,
                            crit = RollCrit(),
                            damageColorIndex = DamageColorIndex.Default,
                            falloffModel = BlastAttack.FalloffModel.None,
                            impactEffect = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactLightning").GetComponent<EffectIndex>(),
                            procCoefficient = 1f,
                            radius = 5
                        }.Fire();
                        EffectData effectData = new EffectData
                        {
                            rotation = Quaternion.identity,
                            scale = 1f,
                            start = base.transform.position,
                            origin = base.transform.position
                        };
                        EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/MaulingRockImpact.prefab"), effectData, true);

                        base.characterMotor.velocity = Vector3.up * 20f;
                        stage = ActionStage.HitTarget;


                        LogCore.LogI("Stage: " + stage.ToString());

                        this.outer.SetNextStateToMain();
                    }
                }
                else
                {
                    outer.SetNextStateToMain();
                }
            }
            else {
                LogCore.LogE("Something is seriously fucked. Stage: " + stage.ToString());
                this.outer.SetNextStateToMain();

            }
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
