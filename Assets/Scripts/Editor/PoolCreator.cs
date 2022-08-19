
using UnityEditor;
using UnityEngine;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System;
using System.IO;

public class PoolCodeGenerator
{
    private CodeCompileUnit targetUnit;
    private string targetClassName;
    private string poolNameSpaceName;
    private CodeTypeDeclaration targetClass;
    private string outputFilePath;

    public PoolCodeGenerator(string outputFilePath, string targetClassName, string poolNameSpaceName, string pooledObjectClassName)
    {

        this.targetClassName = targetClassName;
        this.poolNameSpaceName = poolNameSpaceName; 
        this.outputFilePath = outputFilePath;
        targetUnit = new CodeCompileUnit();
        //TODO: MAKE NAMESPACE OPTIONAL
        CodeNamespace poolNamespace = new CodeNamespace(poolNameSpaceName);
        targetClass = new CodeTypeDeclaration();
        targetClass.IsClass = true;
        targetClass.Name = this.targetClassName;
        targetClass.TypeAttributes =
            TypeAttributes.Public| TypeAttributes.Sealed;
        poolNamespace.Types.Add(targetClass);
        targetUnit.Namespaces.Add(poolNamespace);
        targetClass.BaseTypes.Add(new CodeTypeReference("BasePool",new CodeTypeReference(pooledObjectClassName)));//Add("BasePool");

    }

    public void GenerateCSharpCode()
    {
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        options.BracingStyle = "C";
        using (StreamWriter sourceWriter = new StreamWriter(outputFilePath,false))
        {
            provider.GenerateCodeFromCompileUnit(
                targetUnit, sourceWriter, options);
        }
    }
}

[CustomEditor(typeof(PoolingObject<>),true)]
public class PoolCreator : Editor
{
    private string targetClassName;
    private string poolNamespaceName;
    private bool pendingToGeneration = false;
    private int poolCapacity = 10;

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
        if (!pendingToGeneration)
            return;
        GameObject poolingObject = new GameObject(targetClassName);
        Type poolingObjectType = target.GetType();
        Assembly assem = poolingObjectType.Assembly;
        string poolName = $"{target.name}Pool";
        Type poolType = assem.GetType($"{poolNamespaceName}.{targetClassName}");
        //Type genericClass = typeof(BasePool<>);
        //Type constructedClass = genericClass.MakeGenericType(poolingObjectType);
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
        PrefabUtility.SaveAsPrefabAssetAndConnect(poolingObject, localPath, InteractionMode.UserAction, out prefabSuccess);

        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
        Debug.Log("Done");
    }
}
