using System;
using System.Linq;
using Cloudburst.Enemies;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Enemies.EntityStatez.MegaMushrum
{
    public class SporeGrenade : BaseState
    {
        public static GameObject chargeEffectPrefab = EntityStates.MiniMushroom.SporeGrenade.chargeEffectPrefab;
        public static string attackSoundString = "Play_minimushroom_spore_shoot";
        public static string chargeUpSoundString = "Play_minimushroom_spore_chargeUp";
        public static float recoilAmplitude = 1f;
        public static GameObject projectilePrefab = EntityStates.MiniMushroom.SporeGrenade.projectilePrefab;
        public static float baseDuration = 4.4f;
        public static string muzzleString = "Muzzle";
        public static float damageCoefficient = 5;
        public static float timeToTarget = 1.5f;
        public static float projectileVelocity = 25;
        public static float minimumDistance = 10;
        public static float maximumDistance = 30;
        public static float baseChargeTime = 2f;
        private uint chargeupSoundID;
        private float duration;
        private float chargeTime;
        private bool hasFired;
        private Animator modelAnimator;
        private GameObject chargeEffectInstance;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = SporeGrenade.baseDuration / this.attackSpeedStat;
            this.chargeTime = SporeGrenade.baseChargeTime / this.attackSpeedStat;
            this.modelAnimator = base.GetModelAnimator();
            //Debug.Log(""+ EntityStates.MiniMushroom.SporeGrenade.projectilePrefab.name.ToString() + "");
            if (this.modelAnimator)
            {
                this.modelAnimator.SetBool("isCharged", false);
                base.PlayAnimation("Gesture, Additive", "Charge");
                this.chargeupSoundID = Util.PlaySound(SporeGrenade.chargeUpSoundString, base.characterBody.modelLocator.modelTransform.gameObject);
            }
            Transform transform = base.FindModelChild("ChargeSpot");
            if (transform)
            {
                this.chargeEffectInstance = UnityEngine.Object.Instantiate<GameObject>(SporeGrenade.chargeEffectPrefab, transform);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.chargeTime)
            {
                if (!this.hasFired)
                {
                    this.hasFired = true;
                    Animator animator = this.modelAnimator;
                    if (animator != null)
                    {
                        animator.SetBool("isCharged", true);
                    }
                    if (base.isAuthority)
                    {
                        this.FireGrenade(SporeGrenade.muzzleString);
                    }
                }
                if (base.isAuthority && base.fixedAge >= this.duration)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }
        public override void OnExit()
        {
            base.PlayAnimation("Gesture, Additive", "Empty");
            AkSoundEngine.StopPlayingID(this.chargeupSoundID);
            if (this.chargeEffectInstance)
            {
                EntityState.Destroy(this.chargeEffectInstance);
            }
            base.OnExit();
        }
        private void FireGrenade(string targetMuzzle)
        {
            Ray aimRay = base.GetAimRay();
            Ray ray = new Ray(aimRay.origin, Vector3.up);
            Transform transform = base.FindModelChild(targetMuzzle);
            if (transform)
            {
                ray.origin = transform.position;
            }
            BullseyeSearch bullseyeSearch = new BullseyeSearch();
            bullseyeSearch.searchOrigin = aimRay.origin;
            bullseyeSearch.searchDirection = aimRay.direction;
            bullseyeSearch.filterByLoS = false;
            bullseyeSearch.teamMaskFilter = TeamMask.allButNeutral;
            if (base.teamComponent)
            {
                bullseyeSearch.teamMaskFilter.RemoveTeam(base.teamComponent.teamIndex);
            }
            bullseyeSearch.sortMode = BullseyeSearch.SortMode.Angle;
            bullseyeSearch.RefreshCandidates();
            HurtBox hurtBox = bullseyeSearch.GetResults().FirstOrDefault<HurtBox>();
            bool flag = false;
            Vector3 a = Vector3.zero;
            RaycastHit raycastHit;
            if (hurtBox)
            {
                a = hurtBox.transform.position;
                flag = true;
            }
            else if (Physics.Raycast(aimRay, out raycastHit, 1000f, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.Ignore))
            {
                a = raycastHit.point;
                flag = true;
            }
            float magnitude = SporeGrenade.projectileVelocity;
            if (flag)
            {
                Vector3 vector = a - ray.origin;
                Vector2 a2 = new Vector2(vector.x, vector.z);
                float magnitude2 = a2.magnitude;
                Vector2 vector2 = a2 / magnitude2;
                if (magnitude2 < SporeGrenade.minimumDistance)
                {
                    magnitude2 = SporeGrenade.minimumDistance;
                }
                if (magnitude2 > SporeGrenade.maximumDistance)
                {
                    magnitude2 = SporeGrenade.maximumDistance;
                }
                float y = Trajectory.CalculateInitialYSpeed(SporeGrenade.timeToTarget, vector.y);
                float num = magnitude2 / SporeGrenade.timeToTarget;
                Vector3 direction = new Vector3(vector2.x * num, y, vector2.y * num);
                magnitude = direction.magnitude;
                ray.direction = direction;
            }
            for (float num = 0f; num < 9f; num += 1f)
            {
                float num2 = 6.2831855f;
                Vector3 forward = new Vector3(Mathf.Cos(num / 9f * num2), 0f, Mathf.Sin(num / 9f * num2));
                ProjectileManager.instance.FireProjectile(SporeGrenade.projectilePrefab, ray.origin, Quaternion.LookRotation(forward), base.gameObject, this.damageStat * SporeGrenade.damageCoefficient, 0f, base.RollCrit(), DamageColorIndex.Default, null, magnitude - 10);
            }
        }
    }
}
