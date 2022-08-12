using UnityEngine;

public class KeyBinding : IBinding
{
    private KeyCode keyBinding;
    private KeyCode alternativeKeyBinding;
    public bool IsPressed => Input.GetKeyDown(keyBinding) || Input.GetKeyDown(alternativeKeyBinding);
    public bool IsReleased => Input.GetKeyUp(keyBinding) || Input.GetKeyUp(alternativeKeyBinding);
    public bool IsRestricted { get; set; } 

    public KeyBinding(KeyCode key,KeyCode alternative = KeyCode.None)
    {
        keyBinding = key;
        alternativeKeyBinding = alternative;
        IsRestricted = false;
    }

    public void UpdateBinding(KeyCode key)
    {
        keyBinding = key;
    }
    public void UpdateAlternativeBinding(KeyCode key)
    {
        keyBinding = key;
    }

}
