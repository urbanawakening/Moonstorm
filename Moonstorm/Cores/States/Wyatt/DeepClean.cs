using EntityStates;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace Moonstorm.Cores.States.Wyatt
{
    class DeepClean : BaseSkillState
    {
        private float timer;

        public static float baseDuration = 4;
        public override void OnEnter()
        {
            base.OnEnter();

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.deltaTime;

            if (timer >= 1) {

                LightningOrb lightningOrb2 = new LightningOrb();
                lightningOrb2.origin = base.transform.position;
                lightningOrb2.damageValue = 1 * damageStat;
                lightningOrb2.isCrit = RollCrit();
                lightningOrb2.bouncesRemaining = 10;
                lightningOrb2.teamIndex = GetTeam();
                lightningOrb2.attacker = gameObject;
                lightningOrb2.bouncedObjects = new List<HealthComponent>
                            {
                                base.healthComponent//.GetComponent<HealthComponent>()
                            };
                lightningOrb2.procChainMask = default;
                lightningOrb2.procChainMask.AddProc(ProcType.ChainLightning);
                lightningOrb2.procCoefficient = 0;
                lightningOrb2.lightningType = LightningOrb.LightningType.TreePoisonDart;
                lightningOrb2.damageColorIndex = DamageColorIndex.Item;
                lightningOrb2.range += 10;
                HurtBox hurtBox2 = lightningOrb2.PickNextTarget(transform.position);
                if (hurtBox2)
                {
                    lightningOrb2.target = hurtBox2;
                    OrbManager.instance.AddOrb(lightningOrb2);
                }
                /*lightningOrb.damageValue = 4 * damageStat;
                lightningOrb.isCrit = RollCrit();
                lightningOrb.bouncesRemaining = 10;
                lightningOrb.teamIndex = GetTeam();
                lightningOrb.damageType = DamageTypeCore.antiGrav;
                lightningOrb.attacker = base.gameObject;
                lightningOrb.procChainMask = default;
                lightningOrb.procCoefficient = 0f;
                lightningOrb.lightningType = LightningOrb.LightningType.Loader;
                lightningOrb.damageColorIndex = DamageColorIndex.Item;
                lightningOrb.range = 20f;

                HurtBox hurtBox = lightningOrb.PickNextTarget(base.gameObject.transform.position);
                if (hurtBox)
                {
                    lightningOrb.target = hurtBox;
                    OrbManager.instance.AddOrb(lightningOrb);
                }*/
                timer = 0;
            }


            if (fixedAge >= baseDuration && isAuthority)
            {
                outer.SetNextStateToMain();
            };
        }
    }
}
