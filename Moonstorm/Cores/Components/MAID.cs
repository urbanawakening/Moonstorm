using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.Components
{
    class MAID : MonoBehaviour
    {
        public ProjectileController controller;
        public GameObject owner;
        //public CharacterBody body;
        public void Awake()
        {
            controller = base.gameObject.GetComponent<ProjectileController>();
            owner = controller.owner;
            //body = controller.owner.GetComponent<CharacterBody>();
            if (owner) {
                owner.GetComponent<MAIDManager>().DeployMAIDAuthority(base.gameObject);
            }
        }

        public void OnTriggerEnter(Collider collider) {
            if (collider.gameObject.name == "WyattWinch(Clone)") {
                LiterallyDieJustForAFunnyVideo();
            }
        }

        public void LiterallyDieJustForAFunnyVideo() {
            BlastAttack attack = new BlastAttack()
            {
                attacker = owner,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = 999999,
                baseForce = -3000,
                bonusForce = new Vector3(0, 0, 0),
                crit = false,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Shock5s,
                falloffModel = BlastAttack.FalloffModel.None,
                impactEffect = EffectIndex.Invalid,
                inflictor = base.gameObject,
                losType = BlastAttack.LoSType.NearestHit,
                position = base.transform.position,
                procChainMask = default,
                procCoefficient = 0.5f,
                radius = 30,
                teamIndex = TeamIndex.Player,
            };
            attack.Fire();
            EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact"), new EffectData
            {
                origin = transform.position,
                scale = 15
            }, true);
            Destroy(base.gameObject);
        }
    }
}
