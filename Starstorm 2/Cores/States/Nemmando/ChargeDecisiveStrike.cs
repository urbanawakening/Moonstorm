using EntityStates;
using EntityStates.Captain.Weapon;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace Starstorm2.Cores.States.Nemmando
{
    class ChargeDecisiveStrike : BaseSkillState
    {
        public static float baseMaxChargeTime = 1F;
        private static GameObject chargeupVfxPrefab = ChargeCaptainShotgun.chargeupVfxPrefab;

        private Transform muzzleTransform;
        private GameObject chargeupVfxGameObject;
        private float speedCoef;
        private float initialSpeedCoefficient = 2.5f;
        private float finalSpeedCoefficient = 7.5f;
        private float buttonReleaseTime = 0f;
        private float minChargeTime = 0.35f;
        private float damageCoef = 1.5f;
        private bool buttonReleased = false;
        public float baseDashTime = 0.35f;
        private Vector3 dashDirection;

        public float charge;
        public float lastCharge;
        public float maxChargeTime;

        private bool isCrosshairInitialized = false;

        public override void OnEnter()
        {
            base.OnEnter();
            this.maxChargeTime = ChargeDecisiveStrike.baseMaxChargeTime / this.attackSpeedStat;
            this.characterBody.crosshairPrefab = NucleatorCore.chargeCrosshair;

            if(!isCrosshairInitialized)
            {
                foreach (Transform child in NucleatorCore.chargeCrosshair.transform)
                {
                    if (child.name == "Zoom Amount") Destroy(child.gameObject);
                }
                isCrosshairInitialized = true;
            }
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                this.muzzleTransform = base.FindModelChild("MuzzleCenter");
                if (this.muzzleTransform)
                {
                    this.chargeupVfxGameObject = UnityEngine.Object.Instantiate<GameObject>(ChargeDecisiveStrike.chargeupVfxPrefab, this.muzzleTransform);
                    this.chargeupVfxGameObject.GetComponent<ScaleParticleSystemDuration>().newDuration = this.maxChargeTime;
                }
            }
        }
        public override void OnExit()
        {
            if (this.chargeupVfxGameObject)
            {
                EntityState.Destroy(this.chargeupVfxGameObject);
                this.chargeupVfxGameObject = null;
            }

            this.characterBody.crosshairPrefab = NemmandoCore.defaultCrosshair;
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.SetAimTimer(1f);

            if(base.isAuthority)
            {
                if(this.buttonReleased)
                {
                    if (this.charge > minChargeTime)
                    {
                        if (this.fixedAge <= this.buttonReleaseTime + this.baseDashTime)
                        {
                            if (base.fixedAge <= this.buttonReleaseTime + this.baseDashTime)
                            {
                                if (base.characterMotor && base.characterDirection)
                                {
                                    base.characterMotor.velocity = Vector3.zero;
                                    base.characterMotor.rootMotion += this.dashDirection * (this.moveSpeedStat * this.speedCoef * Time.fixedDeltaTime);
                                    base.characterBody.isSprinting = true;
                                }
                            }
                        }
                        else
                        {
                            FireDecisiveStrike fireDecisiveStrike = new FireDecisiveStrike();
                            fireDecisiveStrike.charge = this.charge;

                            this.outer.SetNextState(fireDecisiveStrike);
                            return;
                        }
                    }
                    else
                    {
                        var aimRay = this.GetAimRay();
                        new BulletAttack
                        {
                            owner = base.gameObject,
                            weapon = base.gameObject,
                            origin = aimRay.origin,
                            aimVector = aimRay.direction.normalized,
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
                        this.outer.SetNextStateToMain();
                        return;
                    }
                }

                if ((base.fixedAge >= this.maxChargeTime || !base.inputBank || !base.inputBank.skill2.down))
                {
                    if (!this.buttonReleased)
                    {
                        this.buttonReleased = true;
                        this.buttonReleaseTime = base.fixedAge;
                        this.dashDirection = base.GetAimRay().direction;
                        RecalculateDashSpeed();
                    }
                }
                else
                {
                    this.lastCharge = this.charge;
                    this.charge = base.fixedAge / this.maxChargeTime;
                }
            }
        }
        
        private void RecalculateDashSpeed()
        {
            this.speedCoef = Mathf.Lerp(this.initialSpeedCoefficient, this.finalSpeedCoefficient, this.charge);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

