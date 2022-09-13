
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.IO;
using static UnityEditor.EditorGUI;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(PoolingObject<>),true)]
public class PoolCreatorInspector : Editor
{
    private string targetClassName;
    private string poolNamespaceName = "Pools";
    private bool pendingToGeneration = false;
    private int poolCapacity = 20;
    private bool groupEnabled = false;
    private const string defaultPrefabPath = "Prefabs/Pools";
    private string prefabPath = "Prefabs/Pools";
    private const string defaultScriptPath = "Scripts/Pools";
    private string generatedScriptPath = "Scripts/Pools";
    private const string assetsString = "Assets/";
    private string generatedClassTypeString;
    private Type generatedClassType;
    PoolCodeGenerator generator;
    private Dictionary<string, System.Reflection.TypeInfo> nameTypeLookup = new Dictionary<string, TypeInfo>();
    private void Awake()
    {
     
    }
    private void OnEnable()
    {
        AssemblyReloadEvents.afterAssemblyReload += GeneratePoolPrefab;
    }
    private void OnDisable()
    { 
        AssemblyReloadEvents.afterAssemblyReload -= GeneratePoolPrefab;
    }
  
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
       
        GUILayout.Space(20);
        GUIContent poolCreationSettingsContent = new GUIContent("Pool Creation Settings","Additional options to create pool from this object");

        GUIContent prefabPathContentLabel = new GUIContent("Prefab Path: ");
        GUIContent prefabPathContentPath = new GUIContent(prefabPath, "Path to pool's prefab folder");

        GUIContent scriptPathContentLabel = new GUIContent("Script Path: ");
        GUIContent scriptPathContentPath = new GUIContent(generatedScriptPath, "Path to pool's script folder");

        groupEnabled = EditorGUILayout.BeginToggleGroup(poolCreationSettingsContent, groupEnabled);
        poolCapacity = EditorGUILayout.IntField("Pool Capacity: ", poolCapacity);
        poolNamespaceName = EditorGUILayout.TextField("Pool Namespace: ", poolNamespaceName);

        GUILayout.Space(10);
        EditorGUILayout.LabelField(prefabPathContentLabel, prefabPathContentPath);

        if (GUILayout.Button("Choose Folder For Pool's Prefab"))
        {
            prefabPath = EditorUtility.OpenFolderPanel("Choose Folder For Pool's Prefab", Application.dataPath, "");
            if (string.IsNullOrEmpty(prefabPath))
            {
                prefabPath = defaultPrefabPath;
            }
            GUIUtility.ExitGUI();
        }

        EditorGUILayout.LabelField(scriptPathContentLabel,scriptPathContentPath);

        if (GUILayout.Button("Choose Folder For Pool's Script"))
        {
            generatedScriptPath = EditorUtility.OpenFolderPanel("Choose Folder For Pool's Script", Application.dataPath, "");
            if (string.IsNullOrEmpty(generatedScriptPath))
            {
                generatedScriptPath = defaultScriptPath;
            }
            GUIUtility.ExitGUI();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Create Pool From This Object"))
        {
            string poolObjectName = target.GetType().Name;
            string targetClassName = poolObjectName + "Pool";
            this.targetClassName = targetClassName;
            if (string.IsNullOrEmpty(generatedScriptPath))
            {
                generatedScriptPath = defaultScriptPath;
            }
            generatedScriptPath = TrimFilePathBeforeSeparator(generatedScriptPath, assetsString);
            if (!Directory.Exists($"{Application.dataPath}/{generatedScriptPath}"))
                Directory.CreateDirectory($"{Application.dataPath}/{generatedScriptPath}");//{targetClassName}.cs
            generator = new PoolCodeGenerator($"{Application.dataPath}/{generatedScriptPath}/", targetClassName, poolNamespaceName, poolObjectName,target.GetType());
           
            //var relativePath = $"{Application.dataPath}/{generatedScriptPath}/{targetClassName}.cs";
            generator.GenerateCSharpCode();
           // AssetDatabase.ImportAsset(relativePath);
            AssetDatabase.Refresh();
            EditorUtility.RequestScriptReload();
            AssetDatabase.SaveAssets();
            pendingToGeneration = true;
            generatedClassTypeString = generator.generatedClassType.ToString(); 
            GUIUtility.ExitGUI();
        }
        EditorGUILayout.EndToggleGroup();
    }

    public void GeneratePoolPrefab()
    {

        if (pendingToGeneration == false)
            return;
        pendingToGeneration = false;
        targetClassName = generatedClassTypeString;
        GameObject poolingObject = new GameObject(targetClassName);
        Type poolingObjectType = null;//target.GetType().BaseType;//generatedClassType;//
       // Assembly assem = poolingObjectType.Assembly;
      
 
        //poolingObjectType = AppDomain.CurrentDomain
        //.GetAssemblies()
        //.SelectMany(x => x.GetTypes())
        //.FirstOrDefault(t => t.Name == targetClassName).GetType();
        Type targetType = target.GetType();
        nameTypeLookup = targetType.Assembly
           .DefinedTypes.Where(t => t.DeclaringType == null)
           .ToDictionary(k => k.Name, v => v);
        poolingObjectType = nameTypeLookup[targetClassName];

        Assembly assem = poolingObjectType.Assembly;    
        string poolName = $"{targetClassName}Pool";
        //string poolName = $"{target.name}Pool";
        Type poolType = assem.GetType($"{poolNamespaceName}.{poolName}");
        //string poolObjectName = target.GetType().Name;
       // targetClassName = poolObjectName + "Pool";
        poolingObject.AddComponent(poolType);
        poolingObject.name = $"{target.name}Pool"; ;

        Type typeOfField = poolType;

        FieldInfo fieldInfo = null;

        while (fieldInfo == null && typeOfField != null)
        {
            fieldInfo = typeOfField.GetField("prefab", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            typeOfField = typeOfField.BaseType;
        }

        if (fieldInfo == null) 
            throw new ArgumentOutOfRangeException("Prefab", string.Format("Field {0} was not found in Type {1}", "prefab", typeOfField.FullName));

        var poolingObjectComponent = poolingObject.GetComponent(poolType);
        fieldInfo.SetValue(poolingObjectComponent, target);
        Type capacityType = poolType;

        PropertyInfo propertyInfo = capacityType.BaseType.GetProperty("Capacity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        propertyInfo.SetValue(poolingObjectComponent, poolCapacity, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);

        if (string.IsNullOrEmpty(prefabPath))
        {
            prefabPath = defaultPrefabPath;
        }

        if (!Directory.Exists(prefabPath))
            Directory.CreateDirectory(prefabPath);

        string prefabFilePath = $"{prefabPath}/{poolName}.prefab";

        prefabFilePath = TrimFilePathBeforeSeparator(prefabFilePath, assetsString);
        prefabFilePath = $"{Application.dataPath}/{prefabFilePath}";

        prefabFilePath = AssetDatabase.GenerateUniqueAssetPath(prefabFilePath);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAssetAndConnect(poolingObject, prefabFilePath, InteractionMode.UserAction, out prefabSuccess);

        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
        Debug.Log("Done");
    }

    private string TrimFilePathBeforeSeparator(string inputFilePath,string separator)
    {
        if (inputFilePath.StartsWith(separator, StringComparison.OrdinalIgnoreCase) == false && inputFilePath.Contains(separator))
        {
            return inputFilePath.Split(separator.ToCharArray()).Last();
        }
        return inputFilePath;   
    }
}
