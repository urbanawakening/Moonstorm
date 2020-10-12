using System;
using System.Collections.Generic;
using System.Linq;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores.States.Nemmando
{
	public class DodgeState : BaseSkillState
	{

		public float duration = 0.75f;
		public float initialSpeedCoefficient = 7.5f;
		public float finalSpeedCoefficient = 1f;
		public static float dodgeFOV = -1f;

		private float rollSpeed;
		private Vector3 forwardDirection;
		private Vector3 previousPosition;
		private Ray aimRay;
		private float nextShotInterval;

		private int bulletCount = 6;
		private int maxBullets = 6;
		private float maxDistance = 30f;
		private BullseyeSearch search;
		private float damageCoef = 1f;

		public static GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark");
		public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/critspark");
		public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracerbanditshotgun");

		private Transform FindClosestEnemyInFront()
		{
			this.search.searchOrigin = aimRay.origin;
			this.search.RefreshCandidates();
			this.search.FilterOutGameObject(base.gameObject);

			var results = this.search.GetResults();

			return results.Any() ? results.ToList()
				.OrderBy(x => Vector3.Distance(x.transform.position, this.characterBody.transform.position))
				.FirstOrDefault()
				.transform : null;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			this.aimRay = this.GetAimRay();

			if (base.characterBody.HasBuff(BuffCore.instance.awarenessIndex))
			{
				this.maxBullets *= 2;
				base.characterBody.RemoveBuff(BuffCore.instance.awarenessIndex);
			}

			this.bulletCount = this.maxBullets;
			this.search = new BullseyeSearch();
			this.search.filterByLoS = true;
			this.search.searchDirection = aimRay.direction;
			this.search.sortMode = BullseyeSearch.SortMode.Distance;
			this.search.maxDistanceFilter = this.maxDistance;
			this.search.minAngleFilter = 0f;
			this.search.maxAngleFilter = 30f;
			this.search.teamMaskFilter = TeamMask.AllExcept(GetTeam());
			base.characterBody.AddBuff(BuffIndex.ArmorBoost);

			if (base.isAuthority && base.inputBank && base.characterDirection)
			{
				this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
			}

			//if (base.isAuthority) 
			//	this.Shoot();

			Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
			Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

			RecalculateRollSpeed();
			if (base.characterMotor && base.characterDirection)
			{
				base.characterMotor.velocity.y = 0f;
				base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
			}

			Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
			this.previousPosition = base.transform.position - b;
		}

		private void RecalculateRollSpeed()
		{
			this.rollSpeed = this.moveSpeedStat * Mathf.Lerp(this.initialSpeedCoefficient, this.finalSpeedCoefficient, base.fixedAge / this.duration);
		}

		private void Shoot()
        {
			this.aimRay = this.GetAimRay();
			var enemyPosition = this.FindClosestEnemyInFront();
			if(enemyPosition && this.bulletCount > 0)
			{
				Vector3 toOther = enemyPosition.position - this.characterBody.transform.position;
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = this.aimRay.origin,
					aimVector = toOther.normalized,
					minSpread = 0f,
					maxSpread = 0f,
					bulletCount = 1,
					procCoefficient = 0.1f,
					damage = this.damageStat * this.damageCoef,
					force = 0f,
					falloffModel = BulletAttack.FalloffModel.None,
					tracerEffectPrefab = DodgeState.tracerEffectPrefab,
					muzzleName = "AimOrigin",
					hitEffectPrefab = DodgeState.hitEffectPrefab,
					isCrit = this.RollCrit(),
					HitEffectNormal = false,
					radius = 1f,
					damageType = DamageType.Generic,
				}.Fire();
			}
			this.bulletCount--;
			this.nextShotInterval += this.duration / (float)this.maxBullets;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			RecalculateRollSpeed();

			//if (base.isAuthority && this.bulletCount > 0 && this.fixedAge >= this.nextShotInterval)
			//{
			//	this.Shoot();
			//}

			if(base.isAuthority)
			{
				Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
				if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
				{
					Vector3 vector = normalized * this.rollSpeed;
					float y = vector.y;
					vector.y = 0f;
					float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
					vector = this.forwardDirection * d;
					vector.y += Mathf.Max(y, 0f);
					base.characterMotor.velocity = vector;
				}

				this.previousPosition = base.transform.position;
				if (base.fixedAge >= this.duration && base.isAuthority)
				{
					this.outer.SetNextStateToMain();
					return;
				}
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
	}
}
