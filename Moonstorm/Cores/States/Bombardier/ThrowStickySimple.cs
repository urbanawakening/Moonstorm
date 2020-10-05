using System;
using EntityStates;
using Moonstorm.Cores.Components;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.States.Bombardier
{
    public class ThrowSticky : FireRocket
    {

        public override Vector3 GetKnockback(Ray aimRay)
        {
            return new Vector3(0, 0, 0);
        }

        public override float GetBloom()
        {
            return 200;
        }

        public override bool ShouldPopStickies()
        {
            return false;
        }

        public override FireProjectileInfo GetInfo(Ray aimRay)
        {
            FireProjectileInfo info = new FireProjectileInfo()
            {
                crit = RollCrit(),
                damage = damageCoeff * damageStat,
                damageColorIndex = RoR2.DamageColorIndex.Default,
                damageTypeOverride = DamageType.Stun1s,
                force = 5000,
                owner = gameObject,
                position = transform.position,
                procChainMask = default,
                projectilePrefab = ProjectileCore.stickyProjectile,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                target = null,
                useFuseOverride = false,
                useSpeedOverride = false
            };
            return info;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

    public class ThrowStickySimple : BaseSkillState
    {
        public static float baseDuration = 0.1f;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            base.StartAimMode(2f, false);
            if (base.isAuthority)
            {
                FireProjectile();
            }
        }

        public virtual void FireProjectile()
        {
            var aimRay = GetAimRay();

            if (base.isAuthority) {
                ProjectileManager.instance.FireProjectile(GetInfo(aimRay));
            }
        }

        public virtual FireProjectileInfo GetInfo(Ray aimRay)
        {
            FireProjectileInfo info = new FireProjectileInfo()
            {
                crit = RollCrit(),
                damage = 3 * damageStat,
                damageColorIndex = RoR2.DamageColorIndex.Default,
                damageTypeOverride = DamageType.Shock5s,
                force = 2500,
                owner = gameObject,
                position = transform.position,
                procChainMask = default,
                projectilePrefab = ProjectileCore.stickyProjectile,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                target = null,
                useFuseOverride = false,
                useSpeedOverride = false
            };
            return info;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
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

    namespace Moonstorm.Cores.States.Bombardier
    {
        class ThrowStickies : BaseSkillState
        {

            public static float baseDuration = 1f;



            private float duration;

            public override void OnEnter()
            {
                base.OnEnter();
                this.duration = baseDuration / this.attackSpeedStat;
                base.StartAimMode(2f, false);
                if (base.isAuthority)
                {
                    FireProjectile();
                }
            }

            public virtual void FireProjectile()
            {
                var aimRay = GetAimRay();

                FireProjectileInfo info = new FireProjectileInfo()
                {
                    crit = RollCrit(),
                    damage = 1 * damageStat,
                    damageColorIndex = RoR2.DamageColorIndex.Default,
                    damageTypeOverride = DamageType.Stun1s,
                    force = 1500,
                    owner = gameObject,
                    position = transform.position,
                    procChainMask = default,
                    projectilePrefab = GrabProjectile(),
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                    target = null,
                    useFuseOverride = false,
                    useSpeedOverride = false
                };
                ModifyProjectileInfo(info);
                ProjectileManager.instance.FireProjectile(info);

            }

            public virtual GameObject GrabProjectile()
            {
                return ProjectileCore.stickyProjectile;
            }

            public virtual void ModifyProjectileInfo(FireProjectileInfo info)
            {
                //do nothing!
            }

            public virtual bool ShouldApplyBloom()
            {
                return true;
            }

            public virtual float GetBloom()
            {
                return 100;
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if (base.fixedAge >= this.duration && base.isAuthority)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.Skill;
                //things on the dev team are really getting uhh
                //heated.
                //and two things are gonna happen
                //A: dev team has a falling out and all our work goes to shit (the bad ending)
                //B: we find a middle ground 
                //the latter is much less likely to happen as each side has made their point
                //i guess i'm just writing this down to write it down but at the same time
                //we make the mod, without us coders, there is no mod
                //so ultimately in a situation like this i feel like it's our responsability 
                //to fix this mess and make the final decision

                //i know this sounds like crap or whatever but i'm genuinely getting scared all hell is gonna break loose 
            }
        }
    }
}