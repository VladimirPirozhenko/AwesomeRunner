#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public static class PoolPrefabCreator
{
    private const string POOL_PREFAB_PATH = "Assets/Prefabs/Pools/";

    [MenuItem("Tools/Prefab/CreatePoolPrefabFromSelected")]
    private static void CreatePoolPrefab()
    {
        //TODO: Make tool or inspector GUI to make pool out of IPoolable
        //IPoolable poolable = Selection.activeGameObject.GetComponent<IPoolable>();
        //poolable.
    }
}

#endif