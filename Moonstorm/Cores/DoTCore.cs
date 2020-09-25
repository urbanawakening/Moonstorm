using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static R2API.DotAPI;
using static RoR2.DotController;

namespace Moonstorm.Cores
{
    class DoTCore
    {
        //1:53.
        public static DoTCore instance;

        protected internal DotIndex clean;

        public DoTCore() => RegisterDoTs();

        protected internal void RegisterDoTs()
        {
            instance = this;

            LogCore.LogI("Initializing Core: " + base.ToString());

            clean = RegisterDoT(1, 0.5f, DamageColorIndex.WeakPoint, BuffCore.instance.cleanIndex);
        }

        protected internal DotIndex RegisterDoT(float interval, float damageCoeff, DamageColorIndex colorIndex, BuffIndex assocatedBuff)
        {
            return DotAPI.RegisterDotDef(interval, damageCoeff, colorIndex, assocatedBuff);
        }
    }
}
