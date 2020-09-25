using EntityStates;
using RoR2;
using UnityEngine;

namespace Cloudburst.Cores.States.Bombardier
{
    class BombardierMain : GenericCharacterMain
    {
        public static GameObject explosionPrefab = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");

        public override void OnEnter()
        {
            base.OnEnter();
            if (characterMotor) {
                characterMotor.onHitGround += CharacterMotor_onHitGround;
            }
        }

        private void CharacterMotor_onHitGround(ref RoR2.CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            BlastAttack attack = new BlastAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = 1.5f * base.damageStat,
                baseForce = -200,
                bonusForce = new Vector3(0, 0, 0),
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageTypeCore.bombardierKnockback,
                falloffModel = BlastAttack.FalloffModel.None,
                impactEffect = EffectIndex.Invalid,
                inflictor = base.gameObject,
                losType = BlastAttack.LoSType.NearestHit,
                position = hitGroundInfo.position,
                procChainMask = default,
                procCoefficient = 0.5f,
                radius = 10,
                teamIndex = characterBody.teamComponent.teamIndex,
            };
            attack.Fire();
            EffectManager.SpawnEffect(explosionPrefab, new EffectData
            {
                origin = transform.position,
                scale = 10
            }, true);

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}
