
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour // TODO: ISpawner
{
    [SerializeField] [Range(1, 100)] private int chunkCount;
    [SerializeField] private float spawnDelay;
    [SerializeField] private ChunkGenerator chunkGenerator;

    private ObjectPool<Chunk> chunkPool;
    private Chunk lastChunk;
    private WaitForSeconds waitBeforeSpawn;

    private void Start()
    {
        waitBeforeSpawn = new WaitForSeconds(spawnDelay);
        SpawnInitialChunks();
    }
    public void SpawnInitialChunks()
    {
        chunkPool = new ObjectPool<Chunk>(CreateChunk, ResetChunk, HideChunk, chunkCount);

       // BoxCollider previousCollider = chunkPool[0].Collider;
        float zOffset = 0;
        lastChunk = chunkPool[0];
        for (int i = 0; i < chunkCount / 2; i++)
        {
            chunkPool[i].gameObject.SetActive(true);  
           // previousCollider = ;
            chunkPool[i].transform.position = lastChunk.transform.position + (zOffset*Vector3.forward);// lastChunkPosition + newSpawnPosition;
            zOffset = chunkPool[i].Collider.size.z;
            lastChunk = chunkPool[i];
        }
    }
    private Chunk CreateChunk()
	{
		Chunk chunk = chunkGenerator.Generate();
        chunk.Init(this);
        chunk.transform.parent = this.transform;
        chunk.gameObject.SetActive(false);
        return chunk;
	}
    private void ResetChunk(Chunk chunk)
    {
        chunk.ResetToDefault();
        chunk.gameObject.SetActive(true);
    }
    private void HideChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
    }

    public IEnumerator DelayedSpawn(Chunk chunkToDespawn)
    {
        yield return waitBeforeSpawn;
        InternalSpawn(chunkToDespawn);
    }
    private void InternalSpawn(Chunk chunkToDespawn)
    {
        chunkPool.ReturnToPool(chunkToDespawn);
        Chunk newChunk = chunkPool.Get();
        newChunk.transform.position = lastChunk.transform.position + lastChunk.Collider.size.z * Vector3.forward;//lastChunkPosition + newSpawnPosition;
        lastChunk = newChunk;
    }
}

