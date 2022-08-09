
using System;
using System.Collections;
using UnityEngine;


public class ChunkSpawner : MonoBehaviour // TODO: ISpawner
{
    [SerializeField] [Range(1, 100)] private int chunkCount;
    [SerializeField] private float spawnDelay;
    [SerializeField] private ChunkGenerator chunkGenerator;
    [SerializeField] private ChunkPool chunkPool; 

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
        lastChunk = chunkPool.GetFromPool();
        for (int i = 0; i < chunkPool.Capacity / 2; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Chunk newChunk = chunkPool.GetFromPool();
        newChunk.ChangeTransformBasedOnPreviousChunk(lastChunk);
        chunkGenerator.Generate(newChunk);
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

