using EntityStates;
using EntityStates.Captain.Weapon;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores.States.Nucleator
{
    class NucleatorSkillStateBase : BaseSkillState
    {
        public static float baseMaxChargeTime = 2F;
        private static GameObject chargeupVfxPrefab = ChargeCaptainShotgun.chargeupVfxPrefab;
        public static float overchargeThreshold = 0.66F;
        public static float maxOverchargePlayerDamageDealt = 0.5F;


        private Transform muzzleTransform;
        private GameObject chargeupVfxGameObject;

        public float charge;
        public float lastCharge;
        public float lastDamage;
        public float maxChargeTime;

        private float playerHealthFinal;
        private float playerHealth;
        private float damageDealt;
        private float nextDamageInstance = 0.05f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.maxChargeTime = NucleatorSkillStateBase.baseMaxChargeTime / this.attackSpeedStat;
            this.playerHealth = this.characterBody.healthComponent.combinedHealth;
            this.playerHealthFinal = playerHealth - playerHealth * maxOverchargePlayerDamageDealt;

            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                this.muzzleTransform = base.FindModelChild("MuzzleCenter");
                if (this.muzzleTransform)
                {
                    this.chargeupVfxGameObject = UnityEngine.Object.Instantiate<GameObject>(NucleatorSkillStateBase.chargeupVfxPrefab, this.muzzleTransform);
                    this.chargeupVfxGameObject.GetComponent<ScaleParticleSystemDuration>().newDuration = this.maxChargeTime;
                }
            }
        }
        public override void OnExit()
        {
            if(damageDealt > 0 && this.playerHealth - this.playerHealthFinal > damageDealt)
            {
                Chat.AddMessage((this.playerHealth - this.playerHealthFinal).ToString());
                Chat.AddMessage(damageDealt.ToString());
                TakeDamage(this.playerHealth - this.playerHealthFinal - damageDealt);
            }

            if (this.chargeupVfxGameObject)
            {
                EntityState.Destroy(this.chargeupVfxGameObject);
                this.chargeupVfxGameObject = null;
            }
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.SetAimTimer(1f);
            this.lastCharge = this.charge;
            this.charge = base.fixedAge / this.maxChargeTime;

            if (this.charge <= 1.0f
                && this.charge > overchargeThreshold 
                && charge - overchargeThreshold > this.nextDamageInstance)
            {
                var chargeCoef = (this.charge - overchargeThreshold) / (1 - overchargeThreshold);
                var damageCoef = chargeCoef * maxOverchargePlayerDamageDealt;
                var damage = Mathf.Abs(chargeCoef * (playerHealthFinal - playerHealth)) - damageDealt;

                if (!this.characterBody.HasBuff(NucleatorCore.radionuclideSurgeBuffIndex))
                {
                    TakeDamage(damage);
                }

                damageDealt += damage;
                this.nextDamageInstance += 0.05f;
            }
        }

        private void TakeDamage(float damage)
        {
            this.characterBody.healthComponent.TakeDamage(
                new DamageInfo()
                {
                    damageType = DamageType.BypassOneShotProtection | DamageType.BypassArmor | DamageType.NonLethal,
                    crit = false,
                    damage = damage,
                    position = this.characterBody.transform.position
                });
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

