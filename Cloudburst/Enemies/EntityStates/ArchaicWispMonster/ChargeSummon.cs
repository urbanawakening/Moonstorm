using EntityStates.GreaterWispMonster;

namespace Cloudburst.Enemies.EntityStatez.ArchaicWispMonster
{
    public class ChargeSummon : ChargeCannons
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                Summon ArchWispSummon = new Summon();
                this.outer.SetNextState(ArchWispSummon);
                return;
            }
        }
    }
}
