using R2API;
using RoR2;
using static RoR2.DotController;

namespace Starstorm2.Cores
{
    class DoTCore
    {
        public static DoTCore instance;

        protected internal DotIndex detritive;
        protected internal DotIndex strangeCanPoison;

        public DoTCore() => RegisterDoTs();

        protected internal void RegisterDoTs()
        {
            instance = this;

            LogCore.LogI("Initializing Core: " + base.ToString());

            detritive = RegisterDoT(1, 0.5f, DamageColorIndex.Item, BuffCore.instance.detritiveIndex);
            strangeCanPoison = RegisterDoT(.5f, 0.5f, DamageColorIndex.Item, BuffCore.instance.strangeCanPoisonIndex);
        }

        protected internal DotIndex RegisterDoT(float interval, float damageCoeff, DamageColorIndex colorIndex, BuffIndex associatedBuff)
        {
            return DotAPI.RegisterDotDef(interval, damageCoeff, colorIndex, associatedBuff);
        }
    }
}
