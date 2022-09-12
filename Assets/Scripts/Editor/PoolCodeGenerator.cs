using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System;
using System.Collections.Generic;

public class PoolCodeGenerator
{
    private CodeCompileUnit targetUnit;
    private string targetClassName;
    private string poolNameSpaceName;
    private CodeTypeDeclaration targetClass;
    private string outputFilePath;
    public Type generatedClassType { get; private set; }
    public PoolCodeGenerator(string outputFilePath, string targetClassName, string poolNameSpaceName, string pooledObjectClassName,Type pooledObjectClassType)
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
        Type t = Type.GetType(pooledObjectClassName);

        //Assembly assem = poolingObjectType.Assembly;
        //string poolName = $"{target.name}Pool";
        //Type poolType = assem.GetType($"{targetClassName}");
        var typeHierarchy = pooledObjectClassType.GetClassHierarchy();
        var linkedTypeHierarchy = new LinkedList<Type>(typeHierarchy);
        Type baseClass = null;
        Type baseClassChild = null;
        foreach (var type in typeHierarchy)
        {
            if (type.ToString() == "UnityEngine.MonoBehaviour")
            {    
                var baseNode = linkedTypeHierarchy.Find(type);
                baseClass = baseNode.Previous.Value;
                var baseNodeChild = linkedTypeHierarchy.Find(baseClass);
                baseClassChild = baseNodeChild.Previous.Value;  
            }
        }
        //poolNamespace.Types.Add(targetClass);
        targetClass.Name = baseClassChild.ToString() + "Pool";
        this.outputFilePath = outputFilePath + targetClass.Name + ".cs";
        poolNamespace.Types.Add(targetClass);
        targetUnit.Namespaces.Add(poolNamespace);
        targetClass.BaseTypes.Add(new CodeTypeReference("BasePool",new CodeTypeReference(baseClassChild.ToString())));//Add("BasePool");
        generatedClassType = baseClassChild;

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
