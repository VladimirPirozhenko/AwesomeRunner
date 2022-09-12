using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ReflectionExtentions
{
    public static IEnumerable<Type> GetClassHierarchy(this Type type)
    {
        while (type != null)
        {
            yield return type;
            type = type.BaseType;
        }
    }
}