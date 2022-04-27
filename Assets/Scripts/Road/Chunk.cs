using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Chunk : MonoBehaviour
{
    //private List<Obstacle> obstacles;[SerializeField] 
    private ChunkSpawner spawner;
    public List<Obstacle> Obstacles { get; private set; }  
    
    public BoxCollider Collider { get; private set; }
    private void Awake()
    {
        spawner =  GameObject.Find("ChunkSpawner").GetComponent<ChunkSpawner>();
        Collider = GetComponent<BoxCollider>();
        Obstacles = new List<Obstacle>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            StartCoroutine(spawner.DelayedSpawn());
        }
    }
    public void ResetToDefault()
    {
        foreach (var obstacle in Obstacles)
        {
            obstacle.ResetToDefault();
        }
    }
}
