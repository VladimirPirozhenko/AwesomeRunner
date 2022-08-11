public struct PressedState
{
    public bool IsPressed { get; private set; }
    public bool IsReleased { get; private set; }
    public PressedState(bool isPressed,bool isReleased)
    {
        IsPressed = isPressed;
        IsReleased = isReleased;    
    }

    public void UpdateState(bool isPressed, bool isReleased)
    {
        IsPressed = isPressed;
        IsReleased = isReleased;
    }
}
