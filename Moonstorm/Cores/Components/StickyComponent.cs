using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores.Components
{
    class BombardierStickyBombProjectile : MonoBehaviour
    {
        private bool hasSwung;

        private ProjectileController controller;
        private BombardierStickyBombManager manager;
        public void Start() {
            controller = base.gameObject.GetComponent<ProjectileController>();
            manager = controller.owner.GetComponent<BombardierStickyBombManager>();
            var networkComponent = gameObject.GetComponent<NetworkIdentity>();

            if (Util.HasEffectiveAuthority(networkComponent))
            {
                manager.AddBombAuthority(base.gameObject);
            }
            //manager.AddStickyProjectile(base.gameObject);
        }

        public void Pop() {
            //pop!
            //LogCore.LogI("far away, i'll still be waiting for your train tonight");

            if (!hasSwung)
            {
                hasSwung = true;

                var body = controller.owner.GetComponent<CharacterBody>();
                var motor = body.characterMotor;

                BlastAttack attack = new BlastAttack()
                {
                    attacker = controller.owner,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = 4.5f * controller.owner.GetComponent<CharacterBody>().damage,
                    baseForce = 2000,
                    bonusForce = new Vector3(0, 0, 0),
                    crit = false,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = base.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 1f,
                    radius = 15,
                    teamIndex = body.teamComponent.teamIndex,
                };
                attack.Fire();
                /*BlastAttack backBlast = new BlastAttack()
                {
                    attacker = controller.owner,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = 0,
                    baseForce = 1,
                    bonusForce = new Vector3(0, 5000, 0),
                    crit = false,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = base.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 0f,
                    radius = 15,
                    teamIndex = TeamIndex.None
                };
                backBlast.Fire();*/
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX"), new EffectData
                {
                    origin = transform.position,
                    scale = 15
                }, true);

                Destroy(base.gameObject);
            }
            var sphere = Physics.OverlapSphere(transform.position, 15);
            foreach (var body in sphere) {
                var cb = body.gameObject.GetComponentInParent<CharacterBody>();
                if (cb) {
                    if (cb.Equals(controller.owner.GetComponent<CharacterBody>())) {
                        //cb.rigidbody.AddExplosionForce(500000, base.transform.position, 50000000000, 10000, ForceMode.Impulse);
                        //LogCore.LogI("hello");
                        var aim = cb.inputBank.GetAimRay();
                        cb.characterMotor.ApplyForce(-(aim.direction * 1000), true, false);
                        //cb.rigidbody.AddExplosionForce(cb.characterMotor.mass * 60, transform.position, 15);
                        //characterMotor.ApplyForce(new Vector3(0, , 0), true, false);
                        //(hitPoint2.hitPosition - this.position).normalized;
                    }
                }
            }
        }
        public void OnDestroy() {
           //Pop();
        }
    }
}
