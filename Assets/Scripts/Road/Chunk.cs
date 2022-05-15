using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EChunkType
{
    CROSS_CHUNK,
    LEFT_CHUNK,
    RIGHT_CHUNK,
    STRAIGHT_CHUNK
}
public enum EChunkDirection
{
    NORTH,
    SOUTH,
    EAST,
    WEST
}

[RequireComponent(typeof(BoxCollider))]
public class Chunk : MonoBehaviour
{
    //[SerializeField] public Transform startPoint;
    //[SerializeField] public Transform endPoint;
   // private ChunkSpawner spawner;
    private ChunkSpawner spawner;
    [SerializeField]  public EChunkDirection enterDirection;
    [SerializeField]  public EChunkDirection exitDirection;
    [SerializeField] public EChunkType chunkType;

    [SerializeField]  public bool isTurning;
    public List<Obstacle> Obstacles { get; private set; }
    public List<Coin> Coins { get; private set; }
    public BoxCollider Collider { get; private set; }
	//public EChunkType chunkType = EChunkType.STRAIGHT_CHUNK;
   // [SerializeField] public EChunkDirection chunkDirection;// = EChunkDirection.ALONG_Z;
    public void Init(ChunkSpawner spawner)
    {
        this.spawner = spawner;
    }
    private void Awake()
    {
        //spawner =  GameObject.Find("ChunkSpawner").GetComponent<ChunkSpawner>();
        Collider = GetComponent<BoxCollider>();
        Obstacles = new List<Obstacle>();
        Coins = new List<Coin>();   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            StartCoroutine(spawner.DelayedSpawn(this));
            if (isTurning)
            {
                player.PlayerStateMachine.SetState(player.PlayerTurnState);
            }
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
