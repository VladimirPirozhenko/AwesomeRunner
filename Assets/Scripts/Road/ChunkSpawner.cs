
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{   
    [SerializeField] public float ChunkWidth { get; private set; }
    [SerializeField] private List<Chunk> chunkPrefabs;
    [SerializeField] [Range(5,50)] private int chunkCount;

    private ObjectPool<Chunk> chunkPool;
    [SerializeField] private float spawnDelay;


    private void Awake()
    {
        chunkPool = new ObjectPool<Chunk>(chunkPrefabs, chunkCount, true); // MAKE INACTIVE AND SHOW ONLY HALF
    }
    void Start()
    {
        
        Vector3 initialPosition = Vector3.zero;
        BoxCollider previousCollider = chunkPool.First.Collider;
        ChunkWidth = previousCollider.size.x;
        foreach (Chunk chunk in chunkPool)
        {      
            chunk.transform.parent = this.transform;
            Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + previousCollider.size.z);
            chunk.transform.position = newPosition;
            previousCollider = chunk.Collider;
            initialPosition.z = newPosition.z;
        }
    }
    public IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
    }
    public void Spawn()
    {
        Chunk firstChunk = chunkPool.First;
        chunkPool.ReturnToPool(firstChunk);

        Chunk lastChunk = chunkPool.Last;
        Vector3 lastChunkPosition = lastChunk.transform.position;
                              
        chunkPool.FirstToLast();

        Chunk newChunk = chunkPool.Get();
        newChunk.transform.position = new Vector3(lastChunkPosition.x, lastChunkPosition.y, lastChunkPosition.z + lastChunk.Collider.size.z); 
    }
}
