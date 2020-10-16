using System;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores.States.Nemmando
{
    class BladeOfCessation : BasicMeleeAttack, SteppedSkillDef.IStepSetter
	{

		public int step = 0;
		public static float recoilAmplitude = 0.5f;
		public static float baseDurationBeforeInterruptable = 0.5f;
		public float bloom = 1f;
		public static float comboFinisherBaseDuration = 0.5f;
		public static float comboFinisherhitPauseDuration = 0.15f;
		public static float comboFinisherBloom = 0.5f;
		public static float comboFinisherBaseDurationBeforeInterruptable = 0.5f;
		private string animationStateName;
		private float durationBeforeInterruptable;

		private bool isComboFinisher
		{
			get
			{
				return this.step == 2;
			}
		}

		public override bool allowExitFire
		{
			get
			{
				return base.characterBody && !base.characterBody.isSprinting;
			}
		}

		void SteppedSkillDef.IStepSetter.SetStep(int i)
		{
			this.step = i;
		}

		public override void OnEnter()
		{
			this.hitBoxGroupName = "SwordHitbox";
			this.baseDuration = 0.5f;
			this.duration = this.baseDuration / base.attackSpeedStat;
			this.hitPauseDuration = 0.01f;
			this.damageCoefficient = 1.5f;

			if (this.isComboFinisher)
			{
				this.hitPauseDuration = BladeOfCessation.comboFinisherhitPauseDuration;
				this.bloom = BladeOfCessation.comboFinisherBloom;
				this.hitBoxGroupName = "SwordHitboxLarge";
				this.baseDuration = BladeOfCessation.comboFinisherBaseDuration;
			}

			base.OnEnter();
			base.characterDirection.forward = base.GetAimRay().direction;
			this.durationBeforeInterruptable = (this.isComboFinisher ? (BladeOfCessation.comboFinisherBaseDurationBeforeInterruptable / this.attackSpeedStat) : (BladeOfCessation.baseDurationBeforeInterruptable / this.attackSpeedStat));
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
		{
			base.AuthorityModifyOverlapAttack(overlapAttack);
			if(this.isComboFinisher)
            {
				overlapAttack.damageType = DamageTypeCore.gougeOnHit | DamageType.Generic;
			}
			else
            {
				overlapAttack.damageType = DamageType.Generic;
			}
		}

		public override void PlayAnimation()
		{
			this.animationStateName = "";
			switch (this.step)
			{
				case 0:
					this.animationStateName = "Primary1";
					break;
				case 1:
					this.animationStateName = "Primary2";
					break;
				case 2:
					this.animationStateName = "Primary3";
					break;
			}
			bool @bool = this.animator.GetBool("isMoving");
			bool bool2 = this.animator.GetBool("isGrounded");
			
			if (!@bool && bool2)
			{
				base.PlayCrossfade("FullBody, Override", this.animationStateName, "Primary.rate", this.duration, 0.05f);
			}
			else
			{
				base.PlayCrossfade("UpperBody, Override", this.animationStateName, "Primary.rate", this.duration, 0.05f);
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
			base.AddRecoil(-0.1f * BladeOfCessation.recoilAmplitude, 0.1f * BladeOfCessation.recoilAmplitude, -1f * BladeOfCessation.recoilAmplitude, 1f * BladeOfCessation.recoilAmplitude);
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

