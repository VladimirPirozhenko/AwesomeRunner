
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADD TURNS
public class ChunkSpawner : MonoBehaviour
{   
    [SerializeField] public float ChunkWidth { get; private set; }
    //[SerializeField] private List<Chunk> chunkPrefabs;
    [SerializeField] Chunk chunkPrefab;
    [SerializeField] [Range(5,100)] private int chunkCount;

    private ObjectPool<Chunk> chunkPool;
    [SerializeField] private float spawnDelay;

    private Vector3 firstChunkPosition;
    private Vector3 lastChunkPosition;

    private Chunk firstChunk;
    private Chunk lastChunk;
    private void Awake()
    {

        chunkPool = new ObjectPool<Chunk>( CreateChunk, ResetChunk, HideChunk, chunkCount); // MAKE INACTIVE AND SHOW ONLY HALF
    }
    void Start()
    {  
        Vector3 initialPosition = Vector3.zero;
        BoxCollider previousCollider = chunkPool[0].Collider;
        firstChunk = chunkPool[0];
        firstChunkPosition = chunkPool[0].transform.position;
        ChunkWidth = previousCollider.size.x;
        int iteration = 0;
        foreach (Chunk chunk in chunkPool)
        {
            chunk.transform.parent = this.transform;
            iteration++;
            if (iteration > chunkCount/2)
            {
                chunk.gameObject.SetActive(false);
            }    
            Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + previousCollider.size.z);
            chunk.transform.position = newPosition;
            previousCollider = chunk.Collider;
            initialPosition.z = newPosition.z; 
            if (chunk.gameObject.activeInHierarchy)
            {
                lastChunk = chunk;
                lastChunkPosition = chunk.transform.position;
            }
        }
    }
    private Chunk CreateChunk()
    {
        return chunkPrefab;
    }
    private void ResetChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(true);
    }
    private void HideChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
    }

    public IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
    }
    public void Spawn()
    {
        firstChunkPosition.z += firstChunk.Collider.size.z;
        Chunk firstChunkToRemove = chunkPool.TryGetFromPos(firstChunkPosition, true); 
        bool isReturned = chunkPool.ReturnToPool(firstChunkToRemove);
        //Debug.Log(isReturned);
        Chunk newChunk = chunkPool.Get();
        newChunk.transform.position = new Vector3(lastChunkPosition.x, lastChunkPosition.y, lastChunkPosition.z + lastChunk.Collider.size.z);
        lastChunkPosition.z = newChunk.transform.position.z;
    }
}

