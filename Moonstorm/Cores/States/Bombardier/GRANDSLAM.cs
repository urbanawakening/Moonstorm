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
            if (!hasLanded)
            {
                hasLanded = true;




                /*LogCore.LogI(y);
                LogCore.LogI(Mathf.Abs(y));
                LogCore.LogI(Mathf.Abs(y) /2);*/
            }
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