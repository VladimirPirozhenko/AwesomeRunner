
using System.Collections.Generic;


public class InputTranslator<T> where T : IBinding
{
    private List<ICommandTranslator> inputTranslators;
    private IBindingHolder<T> bindingHolder;

    public void Init(IBindingHolder<T> holder)
    {
        inputTranslators = new List<ICommandTranslator>();
        bindingHolder = holder; 
        bindingHolder.Init();
    }

    public void AddNode(ICommandTranslator translator)
    {
        if (inputTranslators.Contains(translator))
            return;
            
        inputTranslators.Add(translator);
    }

    public void RemoveNode(ICommandTranslator translator)
    {
        if (inputTranslators.Contains(translator))
            inputTranslators.Remove(translator);
    }

    public void Tick()
    {
        if (inputTranslators.Count == 0)
            return;
            
        var commands = new List<ECommand>();
        foreach (var keyBinding in bindingHolder.InputBindings)
        {
            if (!keyBinding.Value.IsPressed)
                commands.Remove(keyBinding.Key);
            if (commands.Contains(keyBinding.Key))
                continue;
            if (keyBinding.Value.IsPressed)
                commands.Add(keyBinding.Key);
        }
            
        if (commands.Count == 0)
            return;

        foreach (var inputTranslator in inputTranslators)
            foreach (var command in commands)
                inputTranslator.TranslateCommand(command);
    }
}
