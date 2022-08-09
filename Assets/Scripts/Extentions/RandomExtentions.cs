using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomExtentions 
{
    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    
}
