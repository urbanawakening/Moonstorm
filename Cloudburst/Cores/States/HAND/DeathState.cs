using EntityStates;
using RoR2;
using UnityEngine;

namespace Cloudburst.Cores.States.HAND
{
    public class DeathState : GenericCharacterDeath
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }
        private void AttemptDeathBehavior()
        {
            if (this.attemptedDeathBehavior)
            {
                return;
            }
            this.attemptedDeathBehavior = true;
            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXRoboBallBossDeath"), new EffectData
            {
                origin = base.transform.position,
                scale = 15
            }, true);
            GameObject model = base.gameObject.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();
            if (characterModel) {
                characterModel.invisibilityCount++;
             }
        }
        public override void FixedUpdate()
        {
            this.stopwatch += Time.fixedDeltaTime;
            if (this.stopwatch >= DeathState.duration)
            {
                this.AttemptDeathBehavior();
            }
        }
        public override void OnExit()
        {
            if (!this.outer.destroying)
            {
                this.AttemptDeathBehavior();
            }
            base.OnExit();
        }

        public static float duration = .1f;
        private float stopwatch;
        private bool attemptedDeathBehavior;
    }
}