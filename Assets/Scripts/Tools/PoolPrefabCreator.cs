#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public static class PoolPrefabCreator
{
    private const string POOL_PREFAB_PATH = "Assets/Prefabs/Pools/";

    [MenuItem("Tools/Prefab/CreatePoolPrefabFromSelected")]
    private static void CreatePoolPrefab() //where T : PoolingObject<T>
    {
        //TODO: Make tool or inspector GUI to make pool out of IPoolable
        PoolingObject poolable = Selection.activeGameObject.GetComponent<PoolingObject>();
        GameObject poolGo = new GameObject();
        poolGo.AddComponent<BasePool<PoolingObject>>(); 
        PrefabUtility.InstantiatePrefab(poolGo);
        poolGo.GetComponent<BasePool<PoolingObject>>().Init();
       
        //poolable.
    }
}

#endif