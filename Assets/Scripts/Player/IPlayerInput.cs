
public enum EDirection { LEFT, RIGHT , UP ,DOWN };
public interface IPlayerInput 
{
    public EDirection? ScanDirection();
}
