using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.Components
{
    class HookProjectileImpact : NetworkBehaviour, IProjectileImpactBehavior
    {
        private ProjectileController projectileController;

        public float reelDelayTime = 0.3f;
        public float reelSpeed = 120f;
        public float ownerPullFactor = 1f;
        public float victimPullFactor = 1f;
        public float maxDistance = 50;

        public GameObject impactSpark = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniImpactVFX");
        public GameObject impactSuccess = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactBeetle");

        [SyncVar]
        private HookProjectileImpact.HookState hookState;

        [SyncVar]
        private GameObject victim;

        private SetStateOnHurt victimSetStateOnHurt;
        private Transform ownerTransform;
        private ProjectileDamage projectileDamage;
        private Rigidbody rigidbody;

        private float liveTimer;
        private float delayTimer;
        private float flyTimer;

        private NetworkInstanceId ___victimNetId;

        private enum HookState
        {
            Flying,
            HitDelay,
            Reel,
            ReelFail
        }

        private void Awake()
        {
            this.rigidbody = base.GetComponent<Rigidbody>();
            this.projectileController = base.GetComponent<ProjectileController>();
            this.projectileDamage = base.GetComponent<ProjectileDamage>();
            if (this.projectileController && this.projectileController.owner)
            {
                this.ownerTransform = this.projectileController.owner.transform;
            }
            this.liveTimer = this.maxDistance / this.reelSpeed;
        }

        public void FixedUpdate()
        {
            if (NetworkServer.active && !this.projectileController.owner)
            {
                UnityEngine.Object.Destroy(base.gameObject);
                return;
            }
            if (this.victim)
            {
                base.transform.position = this.victim.transform.position;
            }
            switch (this.hookState)
            {
                case HookProjectileImpact.HookState.Flying:
                    if (NetworkServer.active)
                    {
                        this.flyTimer += Time.fixedDeltaTime;
                        if (this.flyTimer >= this.liveTimer)
                        {
                            this.hookState = HookProjectileImpact.HookState.ReelFail;
                            return;
                        }
                    }
                    break;
                case HookProjectileImpact.HookState.HitDelay:
                    if (NetworkServer.active)
                    {
                        if (!this.victim)
                        {
                            this.hookState = HookProjectileImpact.HookState.Reel;
                            return;
                        }
                        this.delayTimer += Time.fixedDeltaTime;
                        if (this.delayTimer >= this.reelDelayTime)
                        {
                            this.hookState = HookProjectileImpact.HookState.Reel;
                            return;
                        }
                    }
                    break;
                case HookProjectileImpact.HookState.Reel:
                    {
                        bool flag = true;
                        if (this.victim)
                        {
                            flag = Reel();
                        }
                        if (NetworkServer.active)
                        {
                            if (!this.victim)
                            {
                                this.hookState = HookProjectileImpact.HookState.ReelFail;
                            }
                            if (flag)
                            {
                                if (this.victimSetStateOnHurt)
                                {
                                    this.victimSetStateOnHurt.SetPain();
                                }
                                UnityEngine.Object.Destroy(base.gameObject);
                                return;
                            }
                        }
                        break;
                    }
                case HookProjectileImpact.HookState.ReelFail:
                    if (NetworkServer.active)
                    {
                        if (this.rigidbody)
                        {
                            this.rigidbody.isKinematic = true;
                        }
                        this.ownerTransform = this.projectileController.owner.transform;
                        if (this.ownerTransform)
                        {
                            base.transform.position = Vector3.MoveTowards(base.transform.position, this.ownerTransform.position, this.reelSpeed * Time.fixedDeltaTime);
                            if (base.transform.position == this.ownerTransform.position)
                            {
                                UnityEngine.Object.Destroy(base.gameObject);
                            }
                        }
                    }
                    break;
                default:
                    return;
            }

        }

        public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
        {
            EffectManager.SimpleImpactEffect(this.impactSpark, impactInfo.estimatedPointOfImpact, -base.transform.forward, true);
            if (this.hookState != HookProjectileImpact.HookState.Flying)
            {
                return;
            }
            Chat.AddMessage(impactInfo.collider.name);
            //LogCore.LogI(impactInfo.collider.name);
            //var maid = impactInfo.collider.GetComponentInParent<MAID>();
            //if (maid) {
            //    maid.LiterallyFuckingDieJustForAFunnyVideo();
            //}
            HurtBox component = impactInfo.collider.GetComponent<HurtBox>();
            if (component)
            {
                HealthComponent healthComponent = component.healthComponent;
                if (healthComponent)
                {
                    TeamComponent component2 = healthComponent.GetComponent<TeamComponent>();
                    TeamFilter component3 = base.GetComponent<TeamFilter>();
                    if (healthComponent.gameObject == this.projectileController.owner || component2.teamIndex == component3.teamIndex)
                    {
                        return;
                    }
                    this.victim = healthComponent.gameObject;
                    this.victimSetStateOnHurt = this.victim.GetComponent<SetStateOnHurt>();
                    if (this.victimSetStateOnHurt)
                    {
                        this.victimSetStateOnHurt.SetPain();
                    }
                    DamageInfo damageInfo = new DamageInfo();
                    if (this.projectileDamage)
                    {
                        damageInfo.damage = this.projectileDamage.damage;
                        damageInfo.crit = this.projectileDamage.crit;
                        damageInfo.attacker = (this.projectileController.owner ? this.projectileController.owner.gameObject : null);
                        damageInfo.inflictor = base.gameObject;
                        damageInfo.position = impactInfo.estimatedPointOfImpact;
                        damageInfo.force = this.projectileDamage.force * base.transform.forward;
                        damageInfo.procChainMask = this.projectileController.procChainMask;
                        damageInfo.procCoefficient = this.projectileController.procCoefficient;
                        damageInfo.damageColorIndex = this.projectileDamage.damageColorIndex;
                    }
                    else
                    {
                        LogCore.LogE("No projectile damage component!");
                    }
                    healthComponent.TakeDamage(damageInfo);
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
                    this.hookState = HookProjectileImpact.HookState.HitDelay;
                    EffectManager.SimpleImpactEffect(this.impactSuccess, impactInfo.estimatedPointOfImpact, -base.transform.forward, true);
                }
            }
            if (!this.victim)
            {
                this.hookState = HookProjectileImpact.HookState.ReelFail;
            }
        }

        private bool Reel()
        {
            Vector3 vector = this.projectileController.owner.transform.position - this.victim.transform.position;
            Vector3 normalized = vector.normalized;
            float num = vector.magnitude;
            Collider component = this.projectileController.owner.GetComponent<Collider>();
            Collider component2 = this.victim.GetComponent<Collider>();
            if (component && component2)
            {
                num = Util.EstimateSurfaceDistance(component, component2);
            }
            bool flag = num <= 2f;
            Rigidbody rigidbody = null;
            float num2 = -1f;
            CharacterMotor component3 = this.projectileController.owner.GetComponent<CharacterMotor>();
            if (component3)
            {
                num2 = component3.mass;
            }
            else
            {
                rigidbody = this.projectileController.owner.GetComponent<Rigidbody>();
                if (rigidbody)
                {
                    num2 = rigidbody.mass;
                }
            }
            Rigidbody rigidbody2 = null;
            float num3 = -1f;
            CharacterMotor component4 = this.victim.GetComponent<CharacterMotor>();
            if (component4)
            {
                num3 = component4.mass;
            }
            else
            {
                rigidbody2 = this.victim.GetComponent<Rigidbody>();
                if (rigidbody2)
                {
                    num3 = rigidbody2.mass;
                }
            }
            float num4 = 0f;
            float num5 = 0f;
            if (num2 > 0f && num3 > 0f)
            {
                num4 = 1f - num2 / (num2 + num3);
                num5 = 1f - num4;
            }
            else if (num2 > 0f)
            {
                num4 = 1f;
            }
            else if (num3 > 0f)
            {
                num5 = 1f;
            }
            else
            {
                flag = true;
            }
            if (flag)
            {
                num4 = 0f;
                num5 = 0f;
            }
            Vector3 velocity = normalized * (num4 * this.ownerPullFactor * -this.reelSpeed);
            Vector3 velocity2 = normalized * (num5 * this.victimPullFactor * this.reelSpeed);
            if (component3)
            {
                component3.velocity = velocity;
            }
            if (rigidbody)
            {
                rigidbody.velocity = velocity;
            }
            if (component4)
            {
                component4.velocity = velocity2;
            }
            if (rigidbody2)
            {
                rigidbody2.velocity = velocity2;
            }
            CharacterDirection component5 = this.projectileController.owner.GetComponent<CharacterDirection>();
            CharacterDirection component6 = this.victim.GetComponent<CharacterDirection>();
            if (component5)
            {
                component5.forward = -normalized;
            }
            if (component6)
            {
                component6.forward = normalized;
            }
            return flag;
        }

        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            if (forceAll)
            {
                writer.Write((int)this.hookState);
                writer.Write(this.victim);
                return true;
            }
            bool flag = false;
            if ((base.syncVarDirtyBits & 1u) != 0u)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write((int)this.hookState);
            }
            if ((base.syncVarDirtyBits & 2u) != 0u)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(this.victim);
            }
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
            }
            return flag;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            if (initialState)
            {
                this.hookState = (HookProjectileImpact.HookState)reader.ReadInt32();
                this.___victimNetId = reader.ReadNetworkId();
                return;
            }
            int num = (int)reader.ReadPackedUInt32();
            if ((num & 1) != 0)
            {
                this.hookState = (HookProjectileImpact.HookState)reader.ReadInt32();
            }
            if ((num & 2) != 0)
            {
                this.victim = reader.ReadGameObject();
            }
        }

        public override void PreStartClient()
        {
            if (!this.___victimNetId.IsEmpty())
            {
                this.victim = ClientScene.FindLocalObject(this.___victimNetId);
            }
        }
    }
}
