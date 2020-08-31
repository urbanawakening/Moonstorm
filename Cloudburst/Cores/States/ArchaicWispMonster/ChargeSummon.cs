using EntityStates.GreaterWispMonster;

namespace Cloudburst.Cores.ArchaicWisp.Skills
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
