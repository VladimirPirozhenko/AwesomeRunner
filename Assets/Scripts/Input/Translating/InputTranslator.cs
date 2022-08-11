
using System.Collections.Generic;
public class InputTranslator<T> where T : IBinding
{
    private List<ICommandTranslator> commandTranslators;
    private IBindingHolder<T> bindingHolder;

    public void Init(IBindingHolder<T> holder)
    {
        commandTranslators = new List<ICommandTranslator>();
        bindingHolder = holder; 
        bindingHolder.Init();
    }

    public void AddCommandTranslator(ICommandTranslator translator)
    {
        if (commandTranslators.Contains(translator))
            return;
            
        commandTranslators.Add(translator);
    }

    public void RemoveCommandTranslator(ICommandTranslator translator)
    {
        if (commandTranslators.Contains(translator))
            commandTranslators.Remove(translator);
    }

    public void Tick()
    {
        if (commandTranslators.Count == 0)
            return;
            
        var commands = new Dictionary<ECommand, PressedState>();

        foreach (var keyBinding in bindingHolder.InputBindings)
        {
            if (keyBinding.Value.IsPressed)
                commands.Add(keyBinding.Key, new PressedState(keyBinding.Value.IsPressed, keyBinding.Value.IsReleased));
            if (keyBinding.Value.IsReleased)
                commands.Add(keyBinding.Key, new PressedState(keyBinding.Value.IsPressed, keyBinding.Value.IsReleased));
        }
            
        if (commands.Count == 0)
            return;
              
        foreach (var commandTranslator in commandTranslators)
        {
            foreach (var command in commands)
            {
                var eCommand = command.Key;
                var pressedState = command.Value;   
                commandTranslator.TranslateCommand(eCommand, pressedState);
            }

        }
    }
}
