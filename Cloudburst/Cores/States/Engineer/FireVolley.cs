using EntityStates;
using UnityEngine;
using RoR2.Projectile;
using RoR2;
using EntityStates.Engi.EngiWeapon;

namespace Cloudburst.Cores.Engineer
{
    public class FireVolley : BaseSkillState
    {
        public static GameObject effectPrefab = FireGrenades.effectPrefab;
        public static GameObject projectilePrefab = FireGrenades.projectilePrefab;
        public int grenadeCountMax = 3;
        public static float damageCoefficient = 1;
        public static float fireDuration = 2f;
        public static float arcAngle = 5f;
        public static float recoilAmplitude = 1f;
        public static string attackSoundString;
        public static float spreadBloomValue = 0.3f;
        private Ray projectileRay;
        private Transform modelTransform;
        private float duration;
        private float fireTimer;
        private int grenadeCount;
        public float baseDuration = 2f;
        private void FireGrenade(string targetMuzzle)
        {
            Util.PlaySound(FireGrenades.attackSoundString, gameObject);
            projectileRay = GetAimRay();
            if (modelTransform)
            {
                ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();
                if (component)
                {
                    Transform transform = component.FindChild(targetMuzzle);
                    if (transform)
                    {
                        this.projectileRay.origin = transform.position;
                    }
                }
            }
            AddRecoil(-1f * FireGrenades.recoilAmplitude, -2f * FireGrenades.recoilAmplitude, -1f * FireGrenades.recoilAmplitude, 1f * FireGrenades.recoilAmplitude);
            if (FireGrenades.effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(FireGrenades.effectPrefab, gameObject, targetMuzzle, false);
            }
            if (isAuthority)
            {
                float x = Random.Range(0f, characterBody.spreadBloomAngle);
                float z = Random.Range(0f, 360f);
                Vector3 up = Vector3.up;
                Vector3 axis = Vector3.Cross(up, this.projectileRay.direction);
                Vector3 vector = Quaternion.Euler(0f, 0f, z) * (Quaternion.Euler(x, 0f, 0f) * Vector3.forward);
                float y = vector.y;
                vector.y = 0f;
                float angle = Mathf.Atan2(vector.z, vector.x) * 57.29578f - 90f;
                float angle2 = Mathf.Atan2(y, vector.magnitude) * 57.29578f + FireGrenades.arcAngle;
                Vector3 forward = Quaternion.AngleAxis(angle, up) * (Quaternion.AngleAxis(angle2, axis) * this.projectileRay.direction);

                for (int i = 0; i < 2; i++) {
                    forward *= 2;
                    ProjectileManager.instance.FireProjectile(EngineerCore.projectileGameObject, this.projectileRay.origin, Util.QuaternionSafeLookRotation(forward), base.gameObject, this.damageStat * FireGrenades.damageCoefficient, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                }
            }
            base.characterBody.AddSpreadBloom(FireGrenades.spreadBloomValue);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FireGrenades.baseDuration / this.attackSpeedStat;
            this.modelTransform = base.GetModelTransform();
            base.StartAimMode(duration, false);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.fireTimer -= Time.fixedDeltaTime;
            float num = fireDuration / this.attackSpeedStat / (float)this.grenadeCountMax;
            if (this.fireTimer <= 0f && this.grenadeCount < grenadeCountMax)
            {
                fireTimer += num;
                if (grenadeCount % 2 == 0) {
                    FireGrenade("MuzzleLeft");
                    PlayCrossfade("Gesture Left Cannon, Additive", "FireGrenadeLeft", 0.1f);
                }
                else
                {
                    FireGrenade("MuzzleRight");
                    PlayCrossfade("Gesture Right Cannon, Additive", "FireGrenadeRight", 0.1f);
                }
                grenadeCount++;
            }
            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
