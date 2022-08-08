
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkSpawner : MonoBehaviour // TODO: ISpawner
{
    [SerializeField] private StraightChunk chunkPrefab;
    [SerializeField] private TurningChunk turningChunkPrefab;
    [SerializeField] [Range(1, 100)] private int chunkCount;
    [SerializeField] private float spawnDelay;
    [SerializeField] private ChunkGenerator chunkGenerator;

    private ObjectPool<Chunk> chunkPool; //¬€Õ≈—“» ¬  À¿——
    private Chunk lastChunk;
    private WaitForSeconds waitBeforeSpawn;

    private void Start()
    {
        waitBeforeSpawn = new WaitForSeconds(spawnDelay);
        SpawnInitialChunks();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Spawn();
    }
    public void SpawnInitialChunks()
    {
        chunkPool = new ObjectPool<Chunk>(CreateChunk, GetChunk, ReleaseChunk, chunkCount);
        lastChunk = chunkPool[0];
        for (int i = 0; i < chunkCount / 2; i++)
        {
            Chunk chunk = chunkPool.Get();
            chunk.ChangeTransformBasedOnPreviousChunk(lastChunk);
            lastChunk = chunk;
        }
    }
    private Chunk CreateChunk()
    {
        Chunk chunk = Instantiate(chunkPrefab);
        chunk.Init(this);
        chunk.transform.parent = this.transform;
        chunk.gameObject.SetActive(false);
        return chunk;
    }

    private void GetChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(true);
    }

    private void ReleaseChunk(Chunk chunk)
    {
        foreach (Coin coin in chunk.Coins)
        {
            //chunkGenerator.CoinPool.ReturnToPool(coin);
        }
        foreach (Obstacle obstacle in chunk.Obstacles)
        {
            //chunkGenerator.ObstaclePool.ReturnToPool(obstacle);
        }
        chunk.ResetToDefault();
        chunk.Coins.Clear();
        chunk.Obstacles.Clear();
        chunk.gameObject.SetActive(false);
    }
   
    public void Spawn()
    {
        Chunk newChunk = chunkPool.Get();
        newChunk.ChangeTransformBasedOnPreviousChunk(lastChunk);
        lastChunk = newChunk;
    }



    public void DelayedReturnToPool(Chunk chunk)
    {
        StartCoroutine(ReturnToPool(chunk));
    }
    public IEnumerator ReturnToPool(Chunk chunk) //EVENT
    {
        yield return waitBeforeSpawn;
        chunkPool.ReturnToPool(chunk);
    }
    
}

