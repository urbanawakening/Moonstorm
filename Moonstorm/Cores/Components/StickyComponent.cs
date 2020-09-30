using RoR2;
using RoR2.Projectile;
using UnityEngine;

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

            manager.AddBombAuthority(base.gameObject);
        }

        public void Pop() {
            //pop!
            //LogCore.LogI("far away, i'll still be waiting for your train tonight");

            if (!hasSwung)
            {
                hasSwung = true;

                BlastAttack attack = new BlastAttack()
                {
                    attacker = controller.owner,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = 4.5f * controller.owner.GetComponent<CharacterBody>().damage,
                    baseForce = 2000,
                    bonusForce = new Vector3(0, 0, 0),
                    crit = false,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageTypeCore.bombardierKnockback,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = base.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 0.5f,
                    radius = 15,
                    teamIndex = controller.owner.GetComponent<CharacterBody>().teamComponent.teamIndex,
                };
                attack.Fire();
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
