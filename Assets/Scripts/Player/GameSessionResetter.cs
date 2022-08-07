using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionResetter : MonoBehaviour, IResettable
{
    private List<IResettable> resettables = new List<IResettable>();

    public void RegisterResettable(IResettable resettable)
    {
        resettables.Add(resettable);
    }
    public void UnregisterResettable(IResettable resettable)
    {
        resettables.Remove(resettable);
    }   
    public void ResetToDefault()
    {
        throw new System.NotImplementedException();
    }
}
