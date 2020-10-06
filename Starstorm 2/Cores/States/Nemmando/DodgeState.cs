using System;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores.States.Nemmando
{
	// Token: 0x02000AA2 RID: 2722
	public class DodgeState : BaseSkillState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (base.isAuthority && base.inputBank && base.characterDirection)
			{
				this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
			}
			Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
			Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

			base.characterBody.AddBuff(BuffIndex.ArmorBoost);

			if (base.characterMotor && base.characterDirection)
			{
				base.characterMotor.velocity.y = 0f;
			}
			Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
			this.previousPosition = base.transform.position - b;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.cameraTargetParams)
			{
				//base.cameraTargetParams.fovOverride = Mathf.Lerp(DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
			}

			Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
			if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
			{
				base.characterMotor.rootMotion += this.forwardDirection * this.moveSpeedStat * this.speedCoefficient * Time.fixedDeltaTime;			}

			this.previousPosition = base.transform.position;
			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override void OnExit()
		{
			if (base.cameraTargetParams)
			{
				base.cameraTargetParams.fovOverride = -1f;
			}
			base.characterBody.RemoveBuff(BuffIndex.ArmorBoost);
			base.OnExit();
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(this.forwardDirection);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.forwardDirection = reader.ReadVector3();
		}

		public float duration = 1f;

		public float speedCoefficient = 5f;

		public static float dodgeFOV = -1f;

		private float rollSpeed;

		private Vector3 forwardDirection;

		private Vector3 previousPosition;
	}
}
