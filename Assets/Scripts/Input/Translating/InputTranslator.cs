
using System.Collections.Generic;

public interface IInputTranslator
{
    //public void Init(IBindingHolder<T> holder);
    public void AddCommandTranslator(ICommandTranslator translator);
    public void RemoveCommandTranslator(ICommandTranslator translator);
    public void RestictTranslation(List<ECommand> commands, bool isRestricted);

    public bool IsTranslationResticted(List<ECommand> commands);

    public void Tick();
}
public class InputTranslator<T> : IInputTranslator where T : IBinding
{
    private List<ICommandTranslator> commandTranslators;
    private IBindingHolder<T> bindingHolder;

    public InputTranslator(IBindingHolder<T> holder)
    {
        commandTranslators = new List<ICommandTranslator>();
        bindingHolder = holder;
        bindingHolder.Init();
    }
    //public void Init(IBindingHolder<T> holder)
    //{
    //    commandTranslators = new List<ICommandTranslator>();
    //    bindingHolder = holder; 
    //    bindingHolder.Init();
    //}

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

    public void RestictTranslation(List<ECommand> commands, bool isRestricted)
    {
        foreach (var keyBinding in bindingHolder.InputBindings)
        {
            if (commands.Contains(keyBinding.Key))
            {
                keyBinding.Value.IsRestricted = isRestricted;
            }
        }     
    }

    public bool IsTranslationResticted(List<ECommand> commands)
    {
        foreach (ECommand command in commands)
        {
            if (bindingHolder.InputBindings.ContainsKey(command))
            {
                return bindingHolder.InputBindings[command].IsRestricted;
            }
        }
        return false;   
    }

    public void Tick()
    {
        if (commandTranslators.Count == 0)
            return;
            
        var commands = new Dictionary<ECommand, PressedState>();

        foreach (var keyBinding in bindingHolder.InputBindings)
        {
            if (keyBinding.Value.IsRestricted)
                continue;
            if (keyBinding.Value.IsPressed())
                commands.Add(keyBinding.Key, new PressedState(true, false));
            if (keyBinding.Value.IsReleased())
                commands.Add(keyBinding.Key, new PressedState(false, true));
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
