using RoR2;
using RoR2.CharacterAI;
using RoR2.Projectile;
using UnityEngine;

public static class BaseHelpers
{
    #region Catalogs
    /// <summary>
    /// Adds a GameObject to the BodyCatalog and returns true
    /// Returns false if the GameObject is null
    /// Will not work after BodyCatalog is initalized.
    /// </summary>
    /// <param Body Prefab="g"></param>
    /// <returns></returns>
    public static bool RegisterNewBody(GameObject g)
    {
        if (g)
        {
            RoR2.BodyCatalog.getAdditionalEntries += list =>
            {
                list.Add(g);
            };

            return true;
        }
        return false;
    }
    public static System.Boolean RegisterNewMaster(GameObject g)
    {
        if (g && g.HasComponent<CharacterMaster>())
        {
            RoR2.MasterCatalog.getAdditionalEntries += list =>
            {
                list.Add(g);
            };

            return true;
        }
        return false;
    }
    public static void DestroyGenericSkillComponents(GameObject gameObject)
    {
        foreach (GenericSkill skill in gameObject.GetComponentsInChildren<GenericSkill>())
        {
            UnityEngine.Object.DestroyImmediate(skill);
        }
    }
    public static void DestroySkillDrivers(GameObject gameObject)
    {
        foreach (AISkillDriver skill in gameObject.GetComponentsInChildren<AISkillDriver>())
        {
            UnityEngine.Object.DestroyImmediate(skill);
        }
    }

    /// <summary>
    /// Adds a GameObject to the projectile catalog and returns true
    /// GameObject must be non-null and have a ProjectileController component
    /// returns false if GameObject is null or is missing the component
    /// </summary>
    /// <param Projectile Prefab="g"></param>
    /// <returns></returns>
    public static System.Boolean RegisterNewProjectile(GameObject g)
    {
        if (g.HasComponent<ProjectileController>())
        {
            RoR2.ProjectileCatalog.getAdditionalEntries += list =>
            {
                list.Add(g);
            };

            return true;
        }
        return false;
    }
    public static bool HasComponent<T>(this GameObject g) where T : Component
    {
        return g.GetComponent(typeof(T)) != null;
    }

    public static bool HasComponent<T>(this MonoBehaviour m) where T : Component
    {
        return m.GetComponent(typeof(T)) != null;
    }

    public static bool HasComponent<T>(this Transform t) where T : Component
    {
        return t.GetComponent(typeof(T)) != null;
    }




    public static int ComponentCount<T>(this GameObject g) where T : Component
    {
        return g.GetComponents(typeof(T)).Length;
    }

    public static int ComponentCount<T>(this MonoBehaviour m) where T : Component
    {
        return m.GetComponents(typeof(T)).Length;
    }

    public static int ComponentCount<T>(this Transform t) where T : Component
    {
        return t.GetComponents(typeof(T)).Length;
    }

    public static T GetComponent<T>(this GameObject g, int index) where T : Component
    {
        return g.GetComponents(typeof(T))[index] as T;
    }

    public static T GetComponent<T>(this MonoBehaviour m, int index) where T : Component
    {
        return m.gameObject.GetComponents(typeof(T))[index] as T;
    }

    public static T GetComponent<T>(this Transform t, int index) where T : Component
    {
        return t.GetComponents(typeof(T))[index] as T;
    }
    public static T AddComponent<T>(this MonoBehaviour m) where T : Component
    {
        return m.gameObject.AddComponent(typeof(T)) as T;
    }

    public static T AddComponent<T>(this Transform t) where T : Component
    {
        return t.gameObject.AddComponent(typeof(T)) as T;
    }

    public static T AddOrGetComponent<T>(this GameObject g) where T : Component
    {
        return (g.HasComponent<T>() ? g.GetComponent(typeof(T)) : g.AddComponent(typeof(T))) as T;
    }

    public static T AddOrGetComponent<T>(this MonoBehaviour m) where T : Component
    {
        return (m.HasComponent<T>() ? m.GetComponent(typeof(T)) : m.gameObject.AddComponent(typeof(T))) as T;
    }

    public static T AddOrGetComponent<T>(this Transform t) where T : Component
    {
        return (t.HasComponent<T>() ? t.GetComponent(typeof(T)) : t.gameObject.AddComponent(typeof(T))) as T;
    }
    #endregion
}
