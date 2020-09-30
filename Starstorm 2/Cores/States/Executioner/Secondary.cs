using RoR2;
using System.Collections.Generic;
using UnityEngine;

//FIXME: ion gun doesn't build charges in mp if player is not host

namespace EntityStates.Executioner
{
    public class ExecutionerIonGun : BaseSkillState
    {
        public static float damageCoefficient = 3.5f;
        public static float procCoefficient = 1.0f;
        public static float baseDuration = 0.1f;
        public static float recoil = 0.2f;
        public static float range = 200f; //copied from default range

        private float duration;
        private BulletAttack bullet;
        private string muzzleString;
        private float shotTimer;
        private int shotsToFire;
        private GenericSkill skill;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            //how do we get this skill's slot without hardcoding like this
            skill = base.characterBody.skillLocator.GetSkill(SkillSlot.Secondary);
            this.shotsToFire = skill.stock;
            this.duration = baseDuration / this.attackSpeedStat;
            base.characterBody.SetAimTimer(2f);
            //this.anim = base.GetModelAnimator();
            this.muzzleString = "Muzzle";

            Shoot();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.shotTimer += Time.fixedDeltaTime;
            if (shotTimer >= duration)
            {
                //fire successive shots if more than one stored, deduct charges manually
                shotTimer = 0;
                if (shotsToFire > 0)
                {
                    Shoot();
                    shotsToFire--;
                }
                else
                {
                    if (this.isAuthority)
                        this.outer.SetNextStateToMain();
                }
                skill.DeductStock(1);
            }
        }

        private void Shoot()
        {
            bool isCrit = base.RollCrit();

            Util.PlayScaledSound(Commando.CommandoWeapon.FireBarrage.fireBarrageSoundString, base.gameObject, this.attackSpeedStat);
            base.AddRecoil(-2f * recoil, -3f * recoil, -1f * recoil, 1f * recoil);
            //base.characterBody.AddSpreadBloom(Commando.CommandoWeapon.FirePistol2.spreadBloomValue * 1.0f);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, this.muzzleString, false);
            base.PlayAnimation("UpperBody, Override", "Secondary", "Secondary.rate", this.duration);

            if (base.isAuthority)
            {
                float dmg = damageCoefficient * this.damageStat;
                Ray r = base.GetAimRay();

                //attempt aim snapping
                BullseyeSearch search = new BullseyeSearch();
                search.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
                search.filterByLoS = true;
                search.maxDistanceFilter = ExecutionerIonGun.range;
                search.searchOrigin = r.origin;
                search.searchDirection = r.direction;
                search.maxAngleFilter = 0.5f;
                search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
                search.RefreshCandidates();
                List<HurtBox> targets = new List<HurtBox>(search.GetResults());
                if (targets.Count > 0 && targets[0].healthComponent)
                {
                    //idx = (idx + 1) % targets.Count;
                    Vector3 vec = (targets[0].transform.position - base.characterBody.corePosition).normalized;
                    r.direction = vec;
                }
                bullet = new BulletAttack
                {
                    aimVector = r.direction,
                    origin = r.origin,
                    damage = damageCoefficient * damageStat,
                    damageType = DamageType.Generic,
                    damageColorIndex = DamageColorIndex.Default,
                    minSpread = 0f,
                    maxSpread = 0.2f,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = Commando.CommandoWeapon.FireBarrage.force,
                    //hitMask = LayerIndex.CommonMasks.bullet,
                    isCrit = isCrit,
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = procCoefficient,
                    radius = 0.5f,
                    //stopperMask = LayerIndex.world.collisionMask,
                    weapon = base.gameObject,
                    tracerEffectPrefab = Commando.CommandoWeapon.FireBarrage.tracerEffectPrefab,
                    hitEffectPrefab = Commando.CommandoWeapon.FireBarrage.hitEffectPrefab
                    //HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                };
                bullet.Fire();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
