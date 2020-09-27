using RoR2;
using Starstorm2.Cores;
using UnityEngine;

namespace EntityStates.Executioner
{
    public class ExecutionerDash : BaseSkillState
    {
        public static float baseDuration = 0.9f;
        public static float speedMultiplier = 2.2f;
        public static float debuffRadius = 20f;
        public static float debuffDuration = 3.0f;

        private float duration;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();

            this.animator = base.GetModelAnimator();

            this.duration = baseDuration;
            Util.PlayScaledSound(Huntress.BeginArrowSnipe.blinkSoundString, base.gameObject, 1.0f);
            base.PlayAnimation("FullBody, Override", "Utility", "Utility.rate", this.duration);

            //create dash aoe
            Vector3 orig = base.characterBody.corePosition;
            BlastAttack b = new BlastAttack()
            {
                baseDamage = 0f,
                procCoefficient = 0f,
                radius = debuffRadius,
                falloffModel = BlastAttack.FalloffModel.None,
                baseForce = 50f,
                damageType = DamageType.Stun1s,
                attacker = base.gameObject,
                position = orig,
            };
            b.teamIndex = TeamComponent.GetObjectTeam(b.attacker);
            b.Fire();

            //apply debuff
            Collider[] colliders = Physics.OverlapSphere(orig, debuffRadius, LayerIndex.defaultLayer.mask);
            foreach (Collider c in colliders)
            {
                HealthComponent h = c.GetComponent<HealthComponent>();
                if (h)
                {
                    TeamComponent team = h.GetComponent<TeamComponent>();
                    if (team && team.teamIndex != TeamComponent.GetObjectTeam(b.attacker))
                    {
                        CharacterBody body = c.GetComponent<CharacterBody>();
                        if (body && body.baseNameToken != "BROTHER_BODY_NAME")
                            h.body.AddTimedBuff(ExecutionerCore.fearDebuff, debuffDuration);
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.isSprinting = true;

            if (base.characterDirection && base.characterMotor)
            {
                base.characterMotor.rootMotion += (base.characterDirection.moveVector * base.characterBody.moveSpeed * speedMultiplier) * Time.fixedDeltaTime;
            }

            if (base.fixedAge >= this.duration)
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
