#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;


public static class PoolPrefabCreator
{
    private const string POOL_PREFAB_PATH = "Assets/Prefabs/Pools/";

    //[MenuItem("Tools/Prefab/CreatePoolPrefabFromSelected")]
    //private static void CreatePoolPrefab()
    //{
    //    GameObject loadedPrefab = PrefabUtility.GetPrefabInstanceHandle(Selection.activeObject) as GameObject;
    //    if ((Selection.activeObject as GameObject).TryGetComponent(out IPoolable<loadedPrefab as MonoBehaviour> poolable))
    //    {
    //        MonoBehaviour mono = poolable as MonoBehaviour;
    //        GameObject instance = PrefabUtility.InstantiatePrefab(mono) as GameObject;
    //        MonoBehaviour poolGameObject = new GameObject().AddComponent<BasePool<MonoBehaviour>>();
    //        var poolComponent = poolGameObject.GetComponent<BasePool<MonoBehaviour>>();
    //        //poolGameObject.
    //        //if (OnCreateRequest != null)
    //        //    return OnCreateRequest();
    //        //var t = typeof(T);
    //        //if (typeof(Component).IsAssignableFrom(t))
    //        //    return (new GameObject(t.Name)).AddComponent<T>();
    //        //return System.Activator.CreateInstance<T>();

    //        var pool = PrefabUtility.InstantiatePrefab(poolComponent);

    //         PrefabUtility.SaveAsPrefabAsset(pool as GameObject, POOL_PREFAB_PATH);
    //    }
    //   //= new List<Image>(loadedPrefab.GetComponent<Image>());
    //    // = PrefabUtility.InstantiatePrefab(Selection.activeObject as GameObject);
    //    //BasePool<MonoBehaviour> pool = GameObject.Instantiate();
    //}

    //[MenuItem("Tools/Prefab/CreatePoolPrefabFromSelected", true)]
    //private static bool ValidateInstantiatePrefab()
    //{
    //    GameObject go = Selection.activeObject as GameObject;
    //    if (go == null)
    //        return false;

    //    return PrefabUtility.IsPartOfPrefabAsset(go);
    //}
}

#endif