using System;
using System.Security.Policy;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores.States.Nemmando
{
    class FireDecisiveStrike : BasicMeleeAttack
	{

		public int step = 0;
		public float charge = 0f;
		public static float recoilAmplitude = 0.5f;
		public static float baseDurationBeforeInterruptable = 0.5f;
		public float bloom = 1f;
		
		private string animationStateName;
		
		private float durationBeforeInterruptable;
		private float minDamageCoef = 1.5f;
		private float maxDamageCoef = 2.5f;

		public override bool allowExitFire
		{
			get
			{
				return base.characterBody && !base.characterBody.isSprinting;
			}
		}

		public override void OnEnter()
		{
			this.hitBoxGroupName = "SwordLarge";
			this.baseDuration = 0.5f;
			this.hitPauseDuration = 0.01f;
			this.damageCoefficient = Mathf.Lerp(this.minDamageCoef, this.maxDamageCoef, this.charge);
			base.OnEnter();
			base.characterDirection.forward = base.GetAimRay().direction;
			this.durationBeforeInterruptable = BladeOfCessation.baseDurationBeforeInterruptable / this.attackSpeedStat;
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
		{
			base.AuthorityModifyOverlapAttack(overlapAttack);
			overlapAttack.damageType = DamageTypeCore.gougeOnHit | DamageType.Generic;
		}

		public override void PlayAnimation()
		{
			this.animationStateName = "GroundLight3";
			bool @bool = this.animator.GetBool("isMoving");
			bool bool2 = this.animator.GetBool("isGrounded");

			if (!@bool && bool2)
			{
				base.PlayCrossfade("FullBody, Override", this.animationStateName, "GroundLight.playbackRate", this.duration, 0.05f);
			}
			else
			{
				base.PlayCrossfade("Gesture, Additive", this.animationStateName, "GroundLight.playbackRate", this.duration, 0.05f);
				base.PlayCrossfade("Gesture, Override", this.animationStateName, "GroundLight.playbackRate", this.duration, 0.05f);
			}
		}

		public override void OnMeleeHitAuthority()
		{
			base.OnMeleeHitAuthority();
			base.characterBody.AddSpreadBloom(this.bloom);
		}

		public override void BeginMeleeAttackEffect()
		{
			this.swingEffectMuzzleString = this.animationStateName;
			base.AddRecoil(-0.1f * FireDecisiveStrike.recoilAmplitude, 0.1f * FireDecisiveStrike.recoilAmplitude, -1f * FireDecisiveStrike.recoilAmplitude, 1f * FireDecisiveStrike.recoilAmplitude);
			base.BeginMeleeAttackEffect();
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write((byte)this.step);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.step = (int)reader.ReadByte();
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			if (base.fixedAge >= this.durationBeforeInterruptable)
			{
				return InterruptPriority.Skill;
			}
			return InterruptPriority.PrioritySkill;
		}

	}
}

