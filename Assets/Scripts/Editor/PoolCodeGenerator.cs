using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
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
