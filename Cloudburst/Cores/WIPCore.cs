using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudburst.Cores
{
    class WIPCore
    {
        public static WIPCore instance;
        public WIPCore() => WIP();

        protected void WIP() {
            instance = this;
            Huntress();
        }

        protected void Huntress() {

        }
            
    }
}
