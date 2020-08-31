using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudburst.Cores
{
    class GlobalHooks
    {
        public static void CharacterSpawnCard_Awake(On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self) {
            self.loadout = new SerializableLoadout();
            orig(self);
        }
    }
}
