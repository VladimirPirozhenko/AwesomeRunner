
using System;

public enum EInputDirection { LEFT, RIGHT , UP ,DOWN };

public interface IPlayerInput 
{
    public EInputDirection? ScanDirection();
    public bool IsShooting();
   // void ActivateAbility();
}
