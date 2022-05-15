using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Chunk : MonoBehaviour, IResettable
{
    public List<Obstacle> Obstacles { get; private set; }
    public List<Coin> Coins { get; private set; }
    public BoxCollider Collider { get; private set; }
    private ChunkSpawner spawner;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        Obstacles = new List<Obstacle>();
        Coins = new List<Coin>();   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            StartCoroutine(spawner.DelayedSpawn(this));
        }
    }
    public void Init(ChunkSpawner spawner)
    {
        this.spawner = spawner;
    }
    public void ResetToDefault()
    {
        foreach (var obstacle in Obstacles)
        {
            obstacle.ResetToDefault();
        }
        foreach (var coin in Coins)
        {
            coin.ResetToDefault();
        }
    }
}
