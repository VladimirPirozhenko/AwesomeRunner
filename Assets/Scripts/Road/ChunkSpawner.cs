using System.Collections;
using UnityEngine;
using Pools;
public class ChunkSpawner : MonoBehaviour // TODO: ISpawner
{
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

    public void SpawnInitialChunks()
    {
        lastChunk = chunkPool.Spawn();
        for (int i = 0; i < chunkPool.InitialCapacity; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Chunk newChunk = chunkPool.Spawn(); 
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

