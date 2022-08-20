
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.IO;

[CustomEditor(typeof(PoolingObject<>),true)]
public class PoolCreatorInspector : Editor
{
    private string targetClassName;
    private string poolNamespaceName;
    private bool pendingToGeneration = false;
    private int poolCapacity = 10;
    private void OnEnable()
    {
       // pendingToGeneration = false;
        AssemblyReloadEvents.afterAssemblyReload += GeneratePoolPrefab;
    }
    private void OnDisable()
    { 
       // pendingToGeneration = false;
        AssemblyReloadEvents.afterAssemblyReload -= GeneratePoolPrefab;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.IntField("Pool Capacity: ",poolCapacity);

        if (GUILayout.Button("Create Pool From This Object"))
        {
            string poolObjectName = target.GetType().Name;
            string targetClassName = poolObjectName + "Pool";
            string poolNamespaceName = "Pools";
            this.targetClassName = targetClassName;
            this.poolNamespaceName = poolNamespaceName;
            PoolCodeGenerator generator = new PoolCodeGenerator($"{Application.dataPath}/Scripts/Road/Pools/{targetClassName}.cs", targetClassName, poolNamespaceName, poolObjectName); //$"{targetClassName}.cs"
            var relativePath = $"Assets/Scripts/Road/Pools/{targetClassName}.cs";
            generator.GenerateCSharpCode();
            AssetDatabase.ImportAsset(relativePath);
            AssetDatabase.Refresh();
            EditorUtility.RequestScriptReload();
            AssetDatabase.SaveAssets();
            pendingToGeneration = true;
        }
    }

    public void GeneratePoolPrefab()
    {

        if (pendingToGeneration == false)
            return;
        pendingToGeneration = false;

        GameObject poolingObject = new GameObject(targetClassName);
        Type poolingObjectType = target.GetType();
        Assembly assem = poolingObjectType.Assembly;
        string poolName = $"{target.name}Pool";
        Type poolType = assem.GetType($"{poolNamespaceName}.{targetClassName}");

        poolingObject.AddComponent(poolType);
        poolingObject.name = poolName;

        Type typeOfField = poolType;
        FieldInfo fieldInfo = null;
        while (fieldInfo == null && typeOfField != null)
        {
            fieldInfo = typeOfField.GetField("prefab", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            typeOfField = typeOfField.BaseType;
        }
        if (fieldInfo == null) throw new ArgumentOutOfRangeException("Prefab", string.Format("Field {0} was not found in Type {1}", "prefab", typeOfField.FullName));

        var poolingObjectComponent = poolingObject.GetComponent(poolType);
        fieldInfo.SetValue(poolingObjectComponent, target);
        Type capacityType = poolType;
  
        PropertyInfo propertyInfo = capacityType.BaseType.GetProperty("Capacity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        propertyInfo.SetValue(poolingObjectComponent, poolCapacity, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);

        if (!Directory.Exists("Assets/Prefabs/Pools"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Pools");

        string localPath = "Assets/Prefabs/Pools/" + poolName + ".prefab";

        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAssetAndConnect(poolingObject, localPath, InteractionMode.UserAction, out prefabSuccess);

        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
        Debug.Log("Done");
    }

}
