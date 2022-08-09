using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomExtentions 
{
    public static bool IsEmpty<T>(this List<T> list)
    {
        return list.Count == 0 ? true : false;  
    }
    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    public static bool GetRandomElement<T>(this List<T> list, out T element)
    {
        element = default(T);   
        if (list.IsEmpty())
            return false;
        element = list[Random.Range(0, list.Count)];
        return true;    
    }
}
