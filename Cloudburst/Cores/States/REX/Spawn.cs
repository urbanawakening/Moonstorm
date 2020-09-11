using EntityStates.Pounder;
using EntityStates;
using RoR2;

namespace Cloudburst.Cores.States.REX
{
    class Spawn : BaseState
    {
        private float duration = 2;
        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound(EntityStates.Pounder.Spawn.spawnSoundString, gameObject);
            EffectManager.SimpleMuzzleFlash(EntityStates.Pounder.Spawn.spawnPrefab, gameObject, "Feet", false);
            PlayAnimation("Base", "Spawn", "Spawn.playbackRate", duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextState(new Harvest());
                return;
            }
        }
    }
}
