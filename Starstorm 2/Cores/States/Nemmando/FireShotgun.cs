using System;
using EntityStates;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores.States.Nemmando
{
	// Token: 0x02000AB2 RID: 2738
	public class FireShotgun : BaseSkillState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.AddRecoil(-1f * FireShotgun.recoilAmplitude, -2f * FireShotgun.recoilAmplitude, -0.5f * FireShotgun.recoilAmplitude, 0.5f * FireShotgun.recoilAmplitude);
			this.maxDuration = FireShotgun.baseMaxDuration / this.attackSpeedStat;
			this.minDuration = FireShotgun.baseMinDuration / this.attackSpeedStat;
			Ray aimRay = base.GetAimRay();
			base.StartAimMode(aimRay, 2f, false);
			Chat.AddMessage((FireShotgun.damageCoefficient * this.damageStat / (float)FireShotgun.bulletCount).ToString());
			if (base.isAuthority)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					maxSpread = 3f,
					bulletCount = (uint)FireShotgun.bulletCount,
					procCoefficient = 1f / (float)FireShotgun.bulletCount,
					damage = FireShotgun.damageCoefficient * this.damageStat / (float)FireShotgun.bulletCount,
					force = FireShotgun.force,
					falloffModel = BulletAttack.FalloffModel.None,
					tracerEffectPrefab = FireShotgun.tracerEffectPrefab,
					muzzleName = "AimOrigin",
					hitEffectPrefab = FireShotgun.hitEffectPrefab,
					isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
					HitEffectNormal = false,
					radius = 1f,
					damageType = DamageType.BleedOnHit | DamageType.Generic
				}.Fire();
			}
			base.characterBody.AddSpreadBloom(FireShotgun.spreadBloomValue);
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.buttonReleased |= !base.inputBank.skill1.down;
			if (base.fixedAge >= this.maxDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			if (this.buttonReleased && base.fixedAge >= this.minDuration)
			{
				return InterruptPriority.Any;
			}
			return InterruptPriority.Skill;
		}

		public static GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark");
		public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/critspark");
		public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracerbanditshotgun");

		public static float damageCoefficient = 5f;

		public static float force = 5f;

		public static int bulletCount = 6;

		public static float baseMaxDuration = 2f;

		public static float baseMinDuration = 0.5f;

		public static float recoilAmplitude;

		public static float spreadBloomValue = 25f;

		private float maxDuration;

		private float minDuration;

		private bool buttonReleased;
	}
}
