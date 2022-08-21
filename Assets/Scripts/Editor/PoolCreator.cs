using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class PoolCreator 
{
    private string targetClassName;
    private string poolNamespaceName;
    //private bool pendingToGeneration = false;
    private int poolCapacity = 10;
    private GameObject poolingObject;
    //private void OnEnable()
    //{
    //    AssemblyReloadEvents.afterAssemblyReload += GeneratePoolPrefab;
    //}
    //private void OnDisable()
    //{
    //    AssemblyReloadEvents.afterAssemblyReload -= GeneratePoolPrefab;
    //}
    public PoolCreator(GameObject poolingObject,int poolCapacity, string poolNamespaceName)
    {
        this.poolingObject = poolingObject;
        this.poolCapacity = poolCapacity;
        this.poolNamespaceName = poolNamespaceName;
        GeneratePoolScript();
    }
    public void GeneratePoolScript()
    {
        string poolObjectName = poolingObject.GetType().Name;
        string targetClassName = poolObjectName + "Pool";
        this.targetClassName = targetClassName;
        PoolCodeGenerator generator = new PoolCodeGenerator($"{Application.dataPath}/Scripts/Road/Pools/{targetClassName}.cs", targetClassName, poolNamespaceName, poolObjectName); //$"{targetClassName}.cs"
        var relativePath = $"Assets/Scripts/Road/Pools/{targetClassName}.cs";
        generator.GenerateCSharpCode();
        AssetDatabase.ImportAsset(relativePath);
        AssetDatabase.Refresh();
        EditorUtility.RequestScriptReload();
        AssetDatabase.SaveAssets();
       // pendingToGeneration = true;   
    }

    public void GeneratePoolPrefab()
    {
        GameObject poolObject = new GameObject(targetClassName);
        Type poolingObjectType = poolingObject.GetType();
        Assembly assem = poolingObjectType.Assembly;
        string poolName = $"{poolingObject.name}Pool";
        Type poolType = assem.GetType($"{poolNamespaceName}.{targetClassName}");
        //Type genericClass = typeof(BasePool<>);
        //Type constructedClass = genericClass.MakeGenericType(poolingObjectType);
        poolObject.AddComponent(poolType);
        poolObject.name = poolName;

        Type typeOfField = poolType;
        FieldInfo fieldInfo = null;
        while (fieldInfo == null && typeOfField != null)
        {
            fieldInfo = typeOfField.GetField("prefab", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            typeOfField = typeOfField.BaseType;
        }
        if (fieldInfo == null) throw new ArgumentOutOfRangeException("Prefab", string.Format("Field {0} was not found in Type {1}", "prefab", typeOfField.FullName));

        var poolingObjectComponent = poolObject.GetComponent(poolType);
        fieldInfo.SetValue(poolingObjectComponent, poolingObject);
        Type capacityType = poolType;
        //if (capacityType.GetProperty("Capacity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
        //    throw new ArgumentOutOfRangeException("Capacity", string.Format("Property {0} was not found in Type {1}", "Capacity", obj.GetType().FullName));
        //capacityType.InvokeMember("Capacity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { 10 });

        PropertyInfo propertyInfo = capacityType.BaseType.GetProperty("Capacity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        propertyInfo.SetValue(poolingObjectComponent, poolCapacity, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);

        if (!Directory.Exists("Assets/Prefabs/Pools"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Pools");

        string localPath = "Assets/Prefabs/Pools/" + poolName + ".prefab";

        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAssetAndConnect(poolObject, localPath, InteractionMode.UserAction, out prefabSuccess);

        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
        Debug.Log("Done");
    }

}
