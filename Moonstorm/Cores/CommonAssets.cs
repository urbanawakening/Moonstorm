using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CommonAssets
{
    public static GameObject lunarStarburst;
    public static GameObject smallChestStarburst;
    public static GameObject goldChestStarburst;
    public static GameObject lunarWispMinigunHitspark;
    public static GameObject burstIntoFlames;

    public static Material lemurianSwingEffectMaterial;
    public static Material ancientWispPillarEffect;

    public static GameObject goldAffix;
    public static void Load() {
        LogCore.LogI("Loading common assets...");

        lunarStarburst = Resources.Load<GameObject>("prefabs/LunarChestStarburst");
        smallChestStarburst = Resources.Load<GameObject>("prefabs/Chest1Starburst");
        goldChestStarburst = Resources.Load<GameObject>("prefabs/GoldChestStarburst");
        lunarWispMinigunHitspark = Resources.Load<GameObject>("prefabs/effects/impacteffects/LunarWispMinigunHitspark");
        burstIntoFlames = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");

        goldAffix = Resources.Load<GameObject>("Prefabs/GoldAffixEffect");

        lemurianSwingEffectMaterial = Resources.Load<GameObject>("prefabs/effects/LemurianBiteTrail").transform.Find("SwingTrail").GetComponent<Renderer>().material;
        ancientWispPillarEffect = Resources.Load<GameObject>("prefabs/effects/impacteffects/AncientWispPillar").transform.Find("Particles/Flames, Tube, CenterHuge").GetComponent<Renderer>().material;

        LogCore.LogI("Common assets loading finished!");
    }
}