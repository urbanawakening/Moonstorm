using RoR2;

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
