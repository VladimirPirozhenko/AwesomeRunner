using System.Collections;
using UnityEngine;

public interface IBinding 
{
    public bool IsPressed { get; }
    public bool IsReleased { get; }
    public bool IsRestricted { get; set; }
}
