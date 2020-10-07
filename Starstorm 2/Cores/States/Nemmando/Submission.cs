using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EntityStates;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores.States.Nemmando
{
	public class Submission : BaseSkillState
	{
		public static GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark");
		public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/critspark");
		public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracerbanditshotgun");

		private float preparationTime = 0.2f;
		private float duration = 0.6f;
		private int bulletCount = 0;
		private int maxBullets = 6;
		private float maxDistance = 60f;
		private float nextShotInterval = 0f;
		private List<HurtBox> enemiesFound;
		private BullseyeSearch search;

		public static float baseMaxDuration = 2f;
		public static float baseDuration = 0.5f;
		public static float damageCoefficient = 1.5f;

		public override void OnEnter()
		{
			base.OnEnter();
			var aimRay = this.GetAimRay();
			this.nextShotInterval = this.preparationTime;
			this.bulletCount = this.maxBullets;

			this.search = new BullseyeSearch();
			this.search.filterByLoS = true;
			this.search.searchOrigin = aimRay.origin;
			this.search.searchDirection = aimRay.direction;
			this.search.sortMode = BullseyeSearch.SortMode.Angle;
			this.search.maxDistanceFilter = this.maxDistance;
			this.search.minAngleFilter = 0f;
			this.search.maxAngleFilter = 75f;
			this.search.RefreshCandidates();
			this.search.FilterOutGameObject(base.gameObject);
			this.search.teamMaskFilter = TeamMask.AllExcept(GetTeam());

			FindTargets();
		}

		public override void OnExit()
		{
			this.characterBody.AddBuff(BuffCore.instance.awarenessIndex);
			base.OnExit();
		}

		private void FindTargets()
        {
			this.enemiesFound = this.search.GetResults().ToList();

			if (this.enemiesFound.Any() && this.enemiesFound.Count != this.maxBullets)
			{
				var enemiesFoundHelper = this.enemiesFound.OrderBy(x => x.healthComponent.combinedHealth).ToList();
				this.enemiesFound = enemiesFoundHelper;
				while (this.enemiesFound.Count != this.maxBullets)
				{
					if (this.enemiesFound.Count < this.maxBullets)
					{
						this.enemiesFound.AddRange(enemiesFoundHelper);
					}
					else
					{
						this.enemiesFound = this.enemiesFound.Take(this.maxBullets).ToList();
					}
				}
			}
		}

		private void Shoot()
        {
			if (base.isAuthority && this.enemiesFound.Count > 0)
			{
				HurtBox enemy = this.enemiesFound.First();
				if (enemy)
				{
					Vector3 toOther = enemy.transform.position - this.characterBody.transform.position;
					var aimRay = base.GetAimRay();
					new BulletAttack
					{
						owner = base.gameObject,
						weapon = base.gameObject,
						origin = aimRay.origin,
						aimVector = toOther,
						minSpread = 0f,
						maxSpread = 0f,
						bulletCount = 1,
						procCoefficient = 0.25f,
						damage = Submission.damageCoefficient * this.damageStat,
						force = 0f,
						falloffModel = BulletAttack.FalloffModel.None,
						tracerEffectPrefab = Submission.tracerEffectPrefab,
						muzzleName = "AimOrigin",
						hitEffectPrefab = Submission.hitEffectPrefab,
						isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
						HitEffectNormal = false,
						radius = 1f,
						damageType = DamageType.Generic
					}.Fire();
				}
				this.enemiesFound.RemoveAt(0);					
			}
			this.bulletCount--;
			this.nextShotInterval += this.duration / (float)this.maxBullets;			
				
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if(base.fixedAge >= this.preparationTime && this.bulletCount > 0 && base.fixedAge + this.preparationTime >= this.nextShotInterval)
            {
				if (base.isAuthority)
                {
					Shoot();
                }
            }
			if (base.fixedAge >= this.duration + this.preparationTime + 0.1f && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

	}
}
