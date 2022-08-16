using System.Collections;
using UnityEngine;

public interface IBinding 
{
    public bool IsPressed();
    public bool IsReleased();
    public bool IsRestricted { get; set; }
}
