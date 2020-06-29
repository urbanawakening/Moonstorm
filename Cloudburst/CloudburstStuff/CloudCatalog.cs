using CloudBurst;
using System;
using System.Collections.Generic;

namespace Cloudburst.CloudburstStuff
{
    public static class CloudCatalog
    {
        private static CloudburstItemDef[] cloudburstItemDefs;
        private static CloudburstEquipDef[] cloudburstEquipmentDefs;
            
        public static event Action<List<Type>> getItemEntries;
        public static event Action<List<Type>> getEquipmentEntries;
        public static event Action<List<Type>> getEnemiesEntries;
    }
}
