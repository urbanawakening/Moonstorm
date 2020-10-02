using RoR2;
using System.Collections.Generic;
using UnityEngine;

//FIXME: axe drop damage doesn't show damage numbers
//FIXME: make exe no-collide during axe jump?

namespace EntityStates.Executioner
{
    public class ExecutionerAxe : BaseSkillState
    {
        public static float baseDuration = 0.7f;

        private float duration;
        private bool storedAirControlState;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();

            this.animator = base.GetModelAnimator();

            this.duration = baseDuration / this.attackSpeedStat;
            base.PlayAnimation("FullBody, Override", "SpecialUp", "SpecialUp.rate", duration);
            if (base.isAuthority)
            {
                base.characterMotor.rootMotion.y += 0.5f;
                base.characterMotor.velocity.y = 30f * this.attackSpeedStat;
                storedAirControlState = base.characterMotor.disableAirControlUntilCollision;
                base.characterMotor.disableAirControlUntilCollision = true;
                //iframes during and shortly after (prevents fall damage from slamming into ground)
                base.characterBody.AddBuff(BuffIndex.ArmorBoost);

                //SetCollisions(false);
            }
        }

        public override void OnExit()
        {
            if (base.isAuthority)
            {
                //SetCollisions(true);
            }
            base.characterMotor.disableAirControlUntilCollision = storedAirControlState;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextState(new ExecutionerAxeSlam());
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private void SetCollisions(bool val)
        {
            var hitbox = base.gameObject.GetComponent<Collider>();
            if (hitbox)
                hitbox.enabled = val;
        }
    }

    public class ExecutionerAxeSlam : BaseSkillState
    {
        public static float dropDamageCoefficient = 1.0f;
        public static float slamDamageCoefficient = 15.0f;
        public static float procCoefficient = 1.0f;
        //shorter value if axe slam should be finite
        //public static float baseDuration = 0.4f;
        public static float baseDuration = 10f;
        public static float dropAttackRadius = 4f;
        public static float slamRadius = 14f;

        private ChildLocator locator;
        private GameObject slamEffect = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");
        private EffectData slamEffectData;
        private Animator animator;
        private List<HealthComponent> hitTargets;
        private bool crit;

        public override void OnEnter()
        {
            base.OnEnter();
            hitTargets = new List<HealthComponent>();

            this.animator = base.GetModelAnimator();
            base.PlayAnimation("FullBody, Override", "SpecialSwing");

            if (base.isAuthority)
            {
                this.locator = base.GetModelTransform().GetComponent<ChildLocator>();
                base.characterMotor.velocity.y = -120f;
                base.characterMotor.rootMotion.y -= 0.5f;
                base.characterMotor.velocity.x = 0f;
                base.characterMotor.velocity.z = 0f;
                this.crit = RollCrit();
                base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            }
        }

        public override void OnExit()
        {
            slamEffectData = new EffectData() {
                color = new Color32(0, 255, 0, 255),
                scale = slamRadius,
                origin = base.characterBody.footPosition
            };

            base.PlayAnimation("FullBody, Override", "SpecialImpact");
            EffectManager.SpawnEffect(slamEffect, slamEffectData, false);

            if (base.isAuthority)
            {
                Vector3 atkCenter = base.characterBody.footPosition;

                //get number of enemies hit to divide damage
                //(there is surely a better way to get enemy hit count)
                var hits = new List<HealthComponent>();
                Collider[] colliders = Physics.OverlapSphere(atkCenter, slamRadius, LayerIndex.defaultLayer.mask);
                foreach (Collider h in colliders)
                {
                    HealthComponent hp = h.GetComponent<HealthComponent>();
                    if (hp)
                    {
                        if (hp.GetComponent<TeamComponent>().teamIndex != TeamComponent.GetObjectTeam(base.gameObject))
                            hits.Add(hp);
                    }
                }
                int hitcount = hits.Count;
                if (hitcount < 1) hitcount = 1;
                else if (hitcount > 10) hitcount = 1;

                BlastAttack blast = new BlastAttack()
                {
                    radius = slamRadius,
                    procCoefficient = procCoefficient,
                    position = atkCenter,
                    attacker = base.gameObject,
                    teamIndex = TeamIndex.Player,
                    crit = this.crit,
                    baseDamage = (base.characterBody.damage * slamDamageCoefficient) / hitcount,
                    damageColorIndex = DamageColorIndex.Default,
                    falloffModel = BlastAttack.FalloffModel.None,
                    damageType = DamageType.Generic
                };
                blast.teamIndex = TeamComponent.GetObjectTeam(blast.attacker);
                blast.Fire();

                //cooldown reduction on kills
                foreach (HealthComponent hp in hits)
                {
                    if (!hp.alive)
                    {
                        var locator = base.characterBody.skillLocator;
                        if (locator)
                        {
                            //if (locator.secondary && locator.secondary.stock < locator.secondary.maxStock)
                                //locator.secondary.rechargeStopwatch += 1f;
                            if (locator.utility && locator.utility.cooldownRemaining > 0)
                                locator.utility.rechargeStopwatch += 1f;
                            if (locator.special && locator.special.cooldownRemaining > 0)
                                locator.special.rechargeStopwatch += 1f;
                        }
                        //should cdr apply once per kill or once per attack? if once per attack uncomment the following
                        //break;
                    }
                }

                base.characterMotor.velocity.y = 0f;
                base.characterBody.RemoveBuff(BuffIndex.ArmorBoost);
                base.characterBody.bodyFlags -= CharacterBody.BodyFlags.IgnoreFallDamage;
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority)
            {
                DamageInfo dropDamage = new DamageInfo();
                dropDamage.attacker = base.gameObject;
                dropDamage.crit = this.crit;
                dropDamage.damage = (base.characterBody.damage * dropDamageCoefficient);
                dropDamage.damageColorIndex = DamageColorIndex.Default;
                dropDamage.damageType = DamageType.Generic;
                dropDamage.procChainMask = default(ProcChainMask);
                dropDamage.procCoefficient = procCoefficient;

                Vector3 atkCenter = base.characterBody.corePosition;
                Collider[] colliders = Physics.OverlapSphere(atkCenter, dropAttackRadius, LayerIndex.defaultLayer.mask);
                foreach (Collider h in colliders)
                {
                    HealthComponent hp = h.GetComponent<HealthComponent>();
                    if (hp)
                    {
                        if (!hitTargets.Contains(hp))
                        {
                            if (hp.GetComponent<TeamComponent>().teamIndex != TeamComponent.GetObjectTeam(base.gameObject))
                            {
                                hp.TakeDamage(dropDamage);
                                hitTargets.Add(hp);
                            }
                        }
                    }
                }
            }

            if ((base.fixedAge >= baseDuration || base.characterMotor.isGrounded) && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
