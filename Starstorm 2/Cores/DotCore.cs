using R2API;
using RoR2;
using static RoR2.DotController;

namespace Starstorm2.Cores
{
    class DoTCore
    {
        public static DoTCore instance;

        protected internal DotIndex detritive;

        public DoTCore() => RegisterDoTs();

        protected internal void RegisterDoTs()
        {
            instance = this;

            LogCore.LogI("Initializing Core: " + base.ToString());

            detritive = RegisterDoT(1, 0.5f, DamageColorIndex.Item, BuffCore.instance.detritiveIndex);
        }

        protected internal DotIndex RegisterDoT(float interval, float damageCoeff, DamageColorIndex colorIndex, BuffIndex assocatedBuff)
        {
            return DotAPI.RegisterDotDef(interval, damageCoeff, colorIndex, assocatedBuff);
        }
    }
}
