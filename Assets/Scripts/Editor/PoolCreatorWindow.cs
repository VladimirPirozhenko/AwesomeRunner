using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class PoolCreatorWindow : EditorWindow
{
    private int poolCapacity = 10;
    private GameObject poolingObject;
    private PoolCreator creator;
    private void OnEnable()
    {
        AssemblyReloadEvents.afterAssemblyReload += CreatePoolPrefab;
    }
    private void OnDisable()
    {
        AssemblyReloadEvents.afterAssemblyReload -= CreatePoolPrefab;
    }

    [MenuItem("Pooling/PoolCreatorWindow")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PoolCreatorWindow));
    }

    void OnGUI()
    {
        // The actual window code goes here
        EditorGUILayout.IntField("Pool Capacity: ", poolCapacity);
        //GameObject pObj = EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20),
        //    "Blablabla",
        //    obj,
        //    typeof(GameObject));

        //poolingObject = EditorGUILayout.ObjectField(poolingObject, typeof(PoolingObject<>), true) as PoolingObject<>;
        //Type type;
       // foreach (var component in poolingObject.GetComponents(typeof(PoolingObject<>)))
       // {
           // type = component.GetType();
            
        
        //Type poolingObjectType = poolingObject.GetType();

        // Assembly assem = poolingObjectType.Assembly;
        //string poolName = $"{poolingObject.name}Pool";
        // Type poolType = assem.GetType("Turret");

        //var poolingObjectComponent = poolingObject.GetComponent(poolingObjectType);
            if (GUILayout.Button("Create Pool From This Object"))
            {
               // creator = new PoolCreator(component, poolCapacity, "Pools");
            }
       // }
    }
    private void CreatePoolPrefab()
    {
        if (creator == null)
            return;
        creator.GeneratePoolPrefab();
    }

}