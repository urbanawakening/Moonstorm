using RoR2;
using UnityEngine;

namespace EntityStates.Executioner
{
    public class ExecutionerPistol : BaseSkillState
    {
        public static float damageCoefficient = 1.2f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.25f;
        public static float recoil = 1f;
        public static float spreadBloom = EntityStates.Commando.CommandoWeapon.FirePistol2.spreadBloomValue;
        public static GameObject tracerPrefab = Commando.CommandoWeapon.FirePistol.tracerEffectPrefab;

        private float duration;
        private float fireDuration;
        private string muzzleString;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.duration = baseDuration / this.attackSpeedStat;
            this.fireDuration = 0.1f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";
            this.hasFired = false;

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.fireDuration)
            {
                Shoot();
            }
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private void Shoot()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                bool isCrit = base.RollCrit();

                Util.PlayScaledSound(Commando.CommandoWeapon.FirePistol2.firePistolSoundString, base.gameObject, this.attackSpeedStat);
                base.AddRecoil(-0.4f * recoil, -0.8f * recoil, -0.3f * recoil, 0.3f * recoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, this.muzzleString, false);
                base.PlayAnimation("UpperBody, Override", "Primary", "Primary.rate", this.duration);

                if (base.isAuthority)
                {
                    float dmg = damageCoefficient * this.damageStat;
                    Ray r = base.GetAimRay();
                    BulletAttack bullet = new BulletAttack
                    {
                        aimVector = r.direction,
                        origin = r.origin,
                        damage = damageCoefficient * damageStat,
                        damageType = DamageType.Generic,
                        damageColorIndex = DamageColorIndex.Default,
                        minSpread = 0f,
                        maxSpread = base.characterBody.spreadBloomAngle * 0.5f,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        force = Commando.CommandoWeapon.FirePistol.force,
                        //hitMask = LayerIndex.CommonMasks.bullet,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 0.2f,
                        //stopperMask = LayerIndex.world.collisionMask,
                        weapon = base.gameObject,
                        tracerEffectPrefab = Commando.CommandoWeapon.FirePistol.tracerEffectPrefab,
                        hitEffectPrefab = Commando.CommandoWeapon.FirePistol.hitEffectPrefab
                        //HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                    };
                    bullet.Fire();
                }
                base.characterBody.AddSpreadBloom(spreadBloom);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

