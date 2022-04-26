using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Chunk : MonoBehaviour
{
    [SerializeField] public List<Obstacle> obstacles { get; set; }  
    private ChunkSpawner spawner;
    public BoxCollider Collider { get; private set; }
    private void Awake()
    {
        spawner =  GameObject.Find("ChunkSpawner").GetComponent<ChunkSpawner>();
        Collider = GetComponent<BoxCollider>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            StartCoroutine(spawner.DelayedSpawn());
        }
    }
}
